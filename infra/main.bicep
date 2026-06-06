@description('Base name used for all resources (e.g. tourdefrance)')
param appName string

@description('Azure region for all resources')
param location string = resourceGroup().location

@description('SQL Server administrator login name')
param sqlAdminLogin string

@description('SQL Server administrator password')
@secure()
param sqlAdminPassword string

@description('Docker image tag to deploy')
param containerImageTag string = 'latest'

// ── Derived names ──────────────────────────────────────────────────────────────
var uniqueSuffix = uniqueString(resourceGroup().id)
var acrName = '${replace(appName, '-', '')}acr${uniqueSuffix}'  // lowercase, no dashes, 5-50 chars
var appServicePlanName = '${appName}-plan'
var webAppName = '${appName}-app-${uniqueSuffix}'
var sqlServerName = '${appName}-sql-${uniqueSuffix}'
var sqlDatabaseName = 'tourdefrance'
var logAnalyticsName = '${appName}-logs'
var appInsightsName = '${appName}-insights'

// ── Log Analytics workspace ────────────────────────────────────────────────────
resource logAnalytics 'Microsoft.OperationalInsights/workspaces@2023-09-01' = {
  name: logAnalyticsName
  location: location
  properties: {
    sku: {
      name: 'PerGB2018'
    }
    retentionInDays: 30
  }
}

// ── Application Insights ───────────────────────────────────────────────────────
resource appInsights 'Microsoft.Insights/components@2020-02-02' = {
  name: appInsightsName
  location: location
  kind: 'web'
  properties: {
    Application_Type: 'web'
    WorkspaceResourceId: logAnalytics.id
  }
}

// ── Azure Container Registry ───────────────────────────────────────────────────
resource acr 'Microsoft.ContainerRegistry/registries@2023-07-01' = {
  name: acrName
  location: location
  sku: {
    name: 'Basic'
  }
  properties: {
    adminUserEnabled: false  // using managed identity for pulls
  }
}

// ── App Service Plan (Linux, B1) ───────────────────────────────────────────────
resource appServicePlan 'Microsoft.Web/serverfarms@2023-12-01' = {
  name: appServicePlanName
  location: location
  kind: 'linux'
  sku: {
    name: 'B1'
    tier: 'Basic'
  }
  properties: {
    reserved: true  // required for Linux
  }
}

// ── Azure SQL Server ───────────────────────────────────────────────────────────
resource sqlServer 'Microsoft.Sql/servers@2023-08-01-preview' = {
  name: sqlServerName
  location: location
  properties: {
    administratorLogin: sqlAdminLogin
    administratorLoginPassword: sqlAdminPassword
    version: '12.0'
  }
}

resource sqlFirewallAllowAzure 'Microsoft.Sql/servers/firewallRules@2023-08-01-preview' = {
  parent: sqlServer
  name: 'AllowAllAzureIps'
  properties: {
    startIpAddress: '0.0.0.0'
    endIpAddress: '0.0.0.0'
  }
}

// ── Azure SQL Database (Standard S0) ──────────────────────────────────────────
resource sqlDatabase 'Microsoft.Sql/servers/databases@2023-08-01-preview' = {
  parent: sqlServer
  name: sqlDatabaseName
  location: location
  sku: {
    name: 'S0'
    tier: 'Standard'
  }
  properties: {
    collation: 'SQL_Latin1_General_CP1_CI_AS'
  }
}

// ── Web App ────────────────────────────────────────────────────────────────────
var sqlConnectionString = 'Server=tcp:${sqlServer.name}.database.windows.net,1433;Initial Catalog=${sqlDatabaseName};User ID=${sqlAdminLogin};Password=${sqlAdminPassword};Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;'

resource webApp 'Microsoft.Web/sites@2023-12-01' = {
  name: webAppName
  location: location
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    serverFarmId: appServicePlan.id
    httpsOnly: true
    clientAffinityEnabled: true  // ARR affinity ON for Blazor Server SignalR
    siteConfig: {
      linuxFxVersion: 'DOCKER|${acr.properties.loginServer}/tourdefrance:${containerImageTag}'
      alwaysOn: true
      webSocketsEnabled: true
      appSettings: [
        {
          name: 'WEBSITES_PORT'
          value: '8080'
        }
        {
          name: 'DOCKER_REGISTRY_SERVER_URL'
          value: 'https://${acr.properties.loginServer}'
        }
        {
          name: 'DatabaseProvider'
          value: 'SqlServer'
        }
        {
          name: 'APPLICATIONINSIGHTS_CONNECTION_STRING'
          value: appInsights.properties.ConnectionString
        }
        {
          name: 'ApplicationInsightsAgent_EXTENSION_VERSION'
          value: '~3'
        }
      ]
      connectionStrings: [
        {
          name: 'DefaultConnection'
          connectionString: sqlConnectionString
          type: 'SQLAzure'
        }
      ]
    }
  }
  dependsOn: [
    sqlDatabase
  ]
}

// ── AcrPull role assignment for the Web App managed identity ──────────────────
var acrPullRoleId = '7f951dda-4ed3-4680-a7ca-43fe172d538d'

resource acrPullAssignment 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  name: guid(acr.id, webApp.id, acrPullRoleId)
  scope: acr
  properties: {
    roleDefinitionId: subscriptionResourceId('Microsoft.Authorization/roleDefinitions', acrPullRoleId)
    principalId: webApp.identity.principalId
    principalType: 'ServicePrincipal'
  }
}

// ── Outputs ────────────────────────────────────────────────────────────────────
output acrLoginServer string = acr.properties.loginServer
output webAppName string = webApp.name
output webAppUrl string = 'https://${webApp.properties.defaultHostName}'

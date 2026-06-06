using './main.bicep'

// ── Required parameters ────────────────────────────────────────────────────────
// Set these before deploying. The resource group should already exist.
//
//   az group create --name <resource-group> --location westeurope
//   az deployment group create \
//     --resource-group <resource-group> \
//     --template-file infra/main.bicep \
//     --parameters infra/main.bicepparam \
//     --parameters sqlAdminPassword=<secret>

param appName = 'tourdefrance'

param location = 'westeurope'

param sqlAdminLogin = 'sqladmin'

// sqlAdminPassword must be supplied at deploy time (never commit to source control):
//   --parameters sqlAdminPassword=<secret>

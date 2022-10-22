
Install
------------------------------------------------------
SQL Server Management Studio
Microsoft.EntityFrameworkCore.Tools

Setup
------------------------------------------------------
set project SvoyaIgra.DbMigrations as startup
set project SvoyaIgra.DbMigrations as deafult in PackageManagerConsole

Templates
------------------------------------------------------
Add-Migration <migration_name> -Context QBankDbMigrationContext -Output Migrations/QBank

Update-Database -Context QBankDbMigrationContext

Script-Migration -Context QBankDbMigrationContext -Output SvoyaIgra.DbMigrations/Scripts/Db/<script_name>.sql

Examples
------------------------------------------------------
Script-Migration -Context QBankDbMigrationContext -From 0 -To CreateTables -Output SvoyaIgra.DbMigrations/Scripts/Db/1_CreateTables.sql
Script-Migration -Context QBankDbMigrationContext -From CreateTables -To AddInitialData -Output SvoyaIgra.DbMigrations/Scripts/Db/2_AddInitialData.sql
Script-Migration -Context QBankDbMigrationContext -From AddInitialData -To AuthorReferenceChanged -Output SvoyaIgra.DbMigrations/Scripts/Db/3_AuthorReferenceChanged.sql
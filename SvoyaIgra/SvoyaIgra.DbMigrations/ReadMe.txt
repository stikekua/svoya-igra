
Install
------------------------------------------------------
SQL Server Management Studio
Microsoft.EntityFrameworkCore.Tools


------------------------------------------------------
Add-Migration <migration_name> -Context QBankDbMigrationContext -Output Migrations/QBank

Update-Database -Context QBankDbMigrationContext

Script-Migration -Context QBankDbMigrationContext -Output SvotaIgra.DbMigrations/Scripts/Db/<script_name>.sql

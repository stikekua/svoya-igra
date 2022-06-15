
Install
------------------------------------------------------
SQL Server Management Studio
Microsoft.EntityFrameworkCore.Tools


------------------------------------------------------
Add-Migration <migration_name> -Context SvoyaIgraDbMigrationContext -Output Migrations/SvoyaIgra

Update-Database -Context SvoyaIgraDbMigrationContext

Script-Migration -Context SvoyaIgraDbMigrationContext -Output SvotaIgra.DbMigrations/Scripts/Db/<script_name>.sql

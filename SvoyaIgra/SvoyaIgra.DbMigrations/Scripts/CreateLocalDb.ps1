sqlcmd -S "(localdb)\MSSQLLocalDB" -i .\RecreateDB.sql -I

sqlcmd -S "(localdb)\MSSQLLocalDB" -d "SvoyaIgra" -i .\Db\1_CreateTables.sql -I
sqlcmd -S "(localdb)\MSSQLLocalDB" -d "SvoyaIgra" -i .\Db\1_CreateGameTable.sql -I
sqlcmd -S "(localdb)\MSSQLLocalDB" -d "SvoyaIgra" -i .\Db\2_AddInitialData.sql -I
sqlcmd -S "(localdb)\MSSQLLocalDB" -d "SvoyaIgra" -i .\Db\3_AuthorReferenceChanged.sql -I
sqlcmd -S "(localdb)\MSSQLLocalDB" -d "SvoyaIgra" -i .\Db\4_TopicLangAdded.sql -I
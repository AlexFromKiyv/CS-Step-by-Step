﻿
using Microsoft.Data.SqlClient;
using System.Data.Odbc;
using MyConnectionFactory;
using System.Data;
#if PC
using System.Data.OleDb;
#endif


// Get a specific Connection object
IDbConnection? GetConnection(DataProviderEnum dataProvider) => dataProvider switch
{
    DataProviderEnum.SqlServer => new SqlConnection(),
    DataProviderEnum.Odbc => new OdbcConnection(),
    DataProviderEnum.None => null,
#if PC
    DataProviderEnum.OleDb => new OleDbConnection(),
#endif
    _ => null
};


// Set up a custom connection
void Setup(DataProviderEnum providerEnum)
{
	IDbConnection? dbConnection = GetConnection(providerEnum);

	if (dbConnection != null)
	{
        Console.WriteLine($"Connection object is {dbConnection?.GetType().Name}");
		// Open, use and close connection...
	}
	else
	{
		Console.WriteLine("There is no connection object.");
	}
}



// Testing various connections
void ObtainingSpecificConnеctionObject()
{
    Setup(DataProviderEnum.SqlServer);
    Setup(DataProviderEnum.Odbc);
#if PC
    Setup(DataProviderEnum.OleDb);
#endif
    Setup(DataProviderEnum.None);

}
ObtainingSpecificConnеctionObject();
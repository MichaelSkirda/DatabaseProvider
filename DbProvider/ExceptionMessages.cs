﻿namespace DbProvider
{
	internal class ExceptionMessages
	{
		internal static string NitherProviderNorFactory =
			"Nither DbConnectionProvider nor (DbConnectionFactory with ConnectionString) not provided." +
			"Use constructor (Func<IDbConnection>)" +
			"or (string, Func<string, IDbConnection>) " +
			"or (string, Func<string, IDbConnection>, Func<IDbConnection>)";

		internal static string NoFactory =
			"DbConnectionFactory not provided. " +
			"Use constructor (string, Func<string, IDbConnection>) " +
			"or (string, Func<string, IDbConnection>, Func<IDbConnection>)" +
			"or (Func<string, IDbConnection>)";

		internal static string FailedToCreateConnection = "Failed to create connection.";

		internal static string FailedToGetMutex = "Failed to get mutex.";
		internal static string TransactionAlreadyStarted => "Transaction already started.";
		internal static string NoStartedTransaction => "No running transaction was found.";
	}
}

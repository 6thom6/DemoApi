using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Tools.Database
{
    public class Connection
    {
        private DbProviderFactory _factory;
        private string _connectionString;

        public Connection(DbProviderFactory factory, string connectionString)
        {
            _factory = factory;
            _connectionString = connectionString;

            using (DbConnection connection = CreateConnection())
            {
                connection.Open();
            }
        }

        public int ExecuteNonQuery(Command command)
        {
            using (DbConnection connection = CreateConnection())
            {
                using (DbCommand dbCommand = CreateCommand(command, connection))
                {
                    connection.Open();
                    return dbCommand.ExecuteNonQuery();
                }
            }
        }

        

        public object ExecuteScalar(Command command)
        {
            using (DbConnection connection = CreateConnection())
            {
                using(DbCommand dbCommand = CreateCommand(command, connection))
                {
                    connection.Open();
                    object o = dbCommand.ExecuteScalar();
                    return o is DBNull ? null : o;
                }
                
            }
        }

        public IEnumerable<TResult> ExecuteReader<TResult>(Command command, Func<IDataRecord, TResult> selector)
        {
            using (DbConnection connection = CreateConnection())
            {
                using (DbCommand dbCommand = CreateCommand(command, connection))
                {
                    connection.Open();
                    using(IDataReader dataReader = dbCommand.ExecuteReader())
                    {
                        while(dataReader.Read())
                        {
                            yield return selector(dataReader);
                        }                        
                    }                    
                }

            }
        }

        public IDataReader ExecuteReader(Command command)
        {
            using (DbConnection connection = CreateConnection())
            {
                using (DbCommand dbCommand = CreateCommand(command, connection))
                {
                    connection.Open();
                    return dbCommand.ExecuteReader();
                }

            }
        }

        private DbConnection CreateConnection()
        {
            DbConnection connection = _factory.CreateConnection();
            connection.ConnectionString = _connectionString;
            return connection;
        }

        private DbCommand CreateCommand(Command command, DbConnection connection)
        {
            DbCommand dbCommand = connection.CreateCommand();
            dbCommand.CommandText = command.Query;

            if (command.IsStoredProcedure)
                dbCommand.CommandType = CommandType.StoredProcedure;

            foreach(KeyValuePair<string, object> kvp in command.Parameters)
            {
                DbParameter dbParameter = dbCommand.CreateParameter();
                dbParameter.ParameterName = kvp.Key;
                dbParameter.Value = kvp.Value;

                dbCommand.Parameters.Add(dbParameter);
            }

            return dbCommand;
        }
    }
}

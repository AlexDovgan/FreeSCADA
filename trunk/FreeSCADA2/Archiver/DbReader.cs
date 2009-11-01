using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using FreeSCADA.Common;
using FreeSCADA.Interfaces;

namespace FreeSCADA.Archiver
{
    class DbReader
    {
        DbProviderFactory dbProviderFactory;
        DbConnection dbConnection;

        public bool Open()
        {
            dbProviderFactory = DatabaseFactory.Get(ArchiverMain.Current.DatabaseSettings.DbProvider);
            dbConnection = dbProviderFactory.CreateConnection();
            dbConnection.ConnectionString = ArchiverMain.Current.DatabaseSettings.CreateConnectionString();
            try
            {
                dbConnection.Open();
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message, "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return false;
            }

            if (IsTableExists("Channels") == false)
                throw new Exception("Channels table does not exists");

            
            return true;
        }

        private bool IsTableExists(string tableName)
        {
            DbCommand cmd = dbConnection.CreateCommand();
            cmd.CommandText = String.Format("SELECT * FROM {0};", tableName);
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public void Close()
        {
            if (dbConnection != null)
            {
                dbConnection.Close();
                dbConnection = null;
                dbProviderFactory = null;
            }
        }


        public DataTable  ExeсCommand(string selectCommand)
        {
            DataTable data = new DataTable();
            DbDataAdapter dataAdapter = dbProviderFactory.CreateDataAdapter();
            DbCommand command = dbConnection.CreateCommand();
            command.CommandText = selectCommand;

            try
            {
                dataAdapter.MissingMappingAction = MissingMappingAction.Passthrough;
                dataAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                dataAdapter.SelectCommand = command;
                dataAdapter.Fill(data);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return data;
            }

            return data;
        }
    }
}

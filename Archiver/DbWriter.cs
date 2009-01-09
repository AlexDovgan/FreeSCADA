using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using FreeSCADA.Interfaces;
using FreeSCADA.Common;

namespace FreeSCADA.Archiver
{
	class DbWriter
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
			{
				DbTransaction transaction = dbConnection.BeginTransaction();

				DbCommand cmd = dbConnection.CreateCommand();
				cmd.CommandText = "CREATE TABLE Channels (";
				cmd.CommandText += "PluginId VARCHAR(255), ";
				cmd.CommandText += "ChannelName VARCHAR(255), ";
				cmd.CommandText += "Time DATETIME, ";
				cmd.CommandText += "Value VARCHAR(255), ";
				cmd.CommandText += "Status SMALLINT";
				cmd.CommandText += ");";

				cmd.ExecuteNonQuery();

				transaction.Commit();
			}
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

		public bool WriteChannels(List<ChannelInfo> channels)
		{
			DbTransaction transaction = dbConnection.BeginTransaction();
			DbCommand cmd = dbConnection.CreateCommand();

			foreach (ChannelInfo channelInfo in channels)
			{
				IChannel[] pluginChannels = Env.Current.CommunicationPlugins[channelInfo.PluginId].Channels;
				IChannel channel = null;
				foreach(IChannel ch in pluginChannels)
				{
					if(ch.Name == channelInfo.ChannelName)
					{
						channel = ch;
						break;
					}
				}

				if (channel != null)
				{
					cmd.CommandText = "INSERT INTO Channels VALUES (";
					cmd.CommandText += "'" + channel.PluginId + "', ";
					cmd.CommandText += "'" + channel.Name + "', ";
					cmd.CommandText += "'" + channel.ModifyTime.ToString("yyyy-MM-dd HH:mm:ss.fff") + "', ";
					cmd.CommandText += "'" + channel.Value.ToString() + "', ";
					cmd.CommandText += ((int)channel.StatusFlags).ToString() + ");";

					cmd.ExecuteNonQuery();
				}
			}

			try
			{
				transaction.Commit();
			}
			catch (System.Exception)
			{
				return false;
			}
			return true;
		}
	}
}

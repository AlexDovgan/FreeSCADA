using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Lextm.SharpSnmpLib;
using System.Net;

namespace FreeSCADA.Communication.SNMPPlug
{
	public partial class SettingsForm : Form
	{
		Plugin plugin;
        const int gridColName = 0;
        const int gridColFSType = 1;
        const int gridColAgent = 2;
        const int gridColOid = 3;

        const int agentGridColName = 0;
        const int agentGridColActive = 1;
        const int agentGridColAddr = 2;
        const int agentGridColPara = 3;

        public SettingsForm(Plugin plugin)
		{
			InitializeComponent();
			this.plugin = plugin;

			grid.SelectionMode = SourceGrid.GridSelectionMode.Row;
			grid.Selection.EnableMultiSelection = false;

			DevAge.Drawing.RectangleBorder b = grid.Selection.Border;
			b.SetWidth(0);
            grid.Selection.Border = b;
            grid.Selection.FocusBackColor = grid.Selection.BackColor;

			grid.ColumnsCount = 11;
			grid.RowsCount = 1;
            grid[0, gridColName] = new SourceGrid.Cells.ColumnHeader("Channel name");
            grid[0, gridColFSType] = new SourceGrid.Cells.ColumnHeader("FS2 Channel Type");
            grid[0, gridColAgent] = new SourceGrid.Cells.ColumnHeader("Agent Name");
            grid[0, gridColOid] = new SourceGrid.Cells.ColumnHeader("SNMP variable Oid");
            grid.MouseDoubleClick += new MouseEventHandler(grid_MouseDoubleClick);

            agentGrid.SelectionMode = SourceGrid.GridSelectionMode.Row;
			agentGrid.Selection.EnableMultiSelection = false;

            agentGrid.Selection.Border = b;
            agentGrid.Selection.FocusBackColor = agentGrid.Selection.BackColor;

			agentGrid.ColumnsCount = 4;
			agentGrid.RowsCount = 1;
            agentGrid[0, agentGridColName] = new SourceGrid.Cells.ColumnHeader("Agent name");
            agentGrid[0, agentGridColActive] = new SourceGrid.Cells.ColumnHeader("Agent Active");
            agentGrid[0, agentGridColAddr] = new SourceGrid.Cells.ColumnHeader("Address");
            agentGrid[0, agentGridColPara] = new SourceGrid.Cells.ColumnHeader("Parameters");
            agentGrid.MouseDoubleClick += new MouseEventHandler(agentGrid_MouseDoubleClick);
			LoadStations();
			LoadChannels();

			agentGrid.AutoStretchColumnsToFitWidth = true;
			agentGrid.AutoSizeCells();
 			grid.AutoStretchColumnsToFitWidth = true;
			grid.AutoSizeCells();
       }

        void agentGrid_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            SourceGrid.Grid agentGrid = (SourceGrid.Grid)sender;
            int [] rows = agentGrid.Selection.GetSelectionRegion().GetRowsIndex();
            SNMPAgent agent = (SNMPAgent)agentGrid[rows[0], agentGridColName].Tag;
            string oldname = agent.Name;
            List<string> forbiddenNames = new List<string>();
            for (int i = 1; i < agentGrid.RowsCount; i++)
            {
                if (i != rows[0])
                    forbiddenNames.Add(agentGrid[i, agentGridColName].DisplayText);
            }
            FormProfile fpr = new FormProfile(agent, forbiddenNames);
            if (fpr.ShowDialog() == DialogResult.OK)
            {
                agentGrid[rows[0], agentGridColName].Value = agent.Name;
                agentGrid[rows[0], agentGridColActive].Value = agent.AgentActive;
                agentGrid[rows[0], agentGridColAddr].Value = "IP: " + agent.AgentIP.ToString();
                agentGrid[rows[0], agentGridColPara].Value = "Version = " + agent.VersionCode.ToString() + ", GetCommunity = " + agent.GetCommunity + ", SetCommunity = " + agent.SetCommunity;
                agentGrid.Invalidate();
            }
            else return;
            if (oldname != agent.Name)
            {
                for (int i = 1; i < grid.RowsCount; i++)
                {
                    if (grid[i, gridColAgent].DisplayText == oldname)
                    {
                        (grid[i, gridColName].Tag as SNMPAgent).Name = agent.Name;
                        grid[i, gridColAgent].Value = agent.Name;
                        grid.Invalidate();
                    }
                }
            }
        }

        private void AddAgent(SNMPAgent agent)
        {
            int row = agentGrid.RowsCount;
            agentGrid.RowsCount++;
            agentGrid[row, agentGridColName] = new SourceGrid.Cells.Cell(agent.Name);
            agentGrid[row, agentGridColName].Tag = agent;
            agentGrid[row, agentGridColActive]   = new SourceGrid.Cells.CheckBox("", agent.AgentActive);
            agentGrid[row, agentGridColAddr] = new SourceGrid.Cells.Cell("IP: " + agent.AgentIP.ToString(), typeof(string));
            agentGrid[row, agentGridColPara] = new SourceGrid.Cells.Cell("Version = " + agent.VersionCode.ToString() + ", GetCommunity = " + agent.GetCommunity + ", SetCommunity = " + agent.SetCommunity, typeof(string));
            agentGrid[row, agentGridColName].Editor = null;
            agentGrid[row, agentGridColAddr].Editor = null;
            agentGrid[row, agentGridColPara].Editor = null;
        }

        private void OnAddAgent(object sender, EventArgs e)
		{
            int row = agentGrid.RowsCount;
            VersionCode version = VersionCode.V1;
            IPEndPoint agentIP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 161);
            string getCommunity = "public";
            string setCommunity = "private";
            string name = GetUniqueAgentName();

            SNMPAgent agent = new SNMPAgent(version, agentIP, getCommunity, setCommunity, name);
            List<string> forbiddenNames = new List<string>();
            for (int i = 1; i < agentGrid.RowsCount; i++)
            {
                forbiddenNames.Add(agentGrid[i, agentGridColName].DisplayText);
            }
            FormProfile fpr = new FormProfile(agent, forbiddenNames);
            if (fpr.ShowDialog() == DialogResult.OK)
            {
                AddAgent(agent);
                agentGrid.Selection.ResetSelection(true);
                agentGrid.Selection.SelectRow(row, true);
            }
        }

        private void OnRemoveStation(object sender, EventArgs e)
        {
            foreach (int row in agentGrid.Selection.GetSelectionRegion().GetRowsIndex())
            {
                bool exist = false;
                for (int i = 1; i < grid.RowsCount; i++)
                {
                    if (grid[i, gridColAgent].DisplayText == agentGrid[row, agentGridColName].DisplayText)
                    {
                        exist = true;
                        break;
                    }
                }
                if (exist)
                    MessageBox.Show(StringConstants.VariablesExist, StringConstants.Error);
                else
                    agentGrid.Rows.Remove(row);
            }
        }
        
        private void LoadStations()
        {
            foreach (SNMPAgent apf in plugin.Agents)
            {
                AddAgent(apf);
            }
        }

        void grid_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int[] rows = grid.Selection.GetSelectionRegion().GetRowsIndex();
            SNMPChannelImp chan = (SNMPChannelImp)grid[rows[0], gridColName].Tag;
            string oldname = chan.Name;
            List<string> forbiddenNames = new List<string>();
            for (int i = 1; i < grid.RowsCount; i++)
            {
                if (i != rows[0])
                    forbiddenNames.Add(grid[i, gridColName].DisplayText);
            }
            List<string> stations = new List<string>();
            for (int i = 1; i < agentGrid.RowsCount; i++)
            {
                stations.Add(((SNMPAgent)agentGrid[i, agentGridColName].Tag).Name);
            }
            ModifyChannelForm mcc = new ModifyChannelForm(chan, forbiddenNames, stations, null);
            if ((chan = mcc.DoShow()) != null)
            {
                showChannel(rows[0], chan);
                grid.Invalidate();
            }
            else return;
        }

        private void AddVariable(SNMPChannelImp channel)
        {
            int row = grid.RowsCount;
            grid.RowsCount++;

            showChannel(row, channel);

            grid.Selection.ResetSelection(true);
            grid.Selection.SelectRow(row, true);
        }

        private void showChannel(int row, SNMPChannelImp channel)
        {
            grid[row, gridColName] = new SourceGrid.Cells.Cell(channel.Name);
            grid[row, gridColName].Tag = channel;
            grid[row, gridColName].Editor = null;

            grid[row, gridColFSType] = new SourceGrid.Cells.Cell(channel.GetType().ToString(), typeof(string));
            grid[row, gridColFSType].Editor = null;

            grid[row, gridColAgent] = new SourceGrid.Cells.Cell(channel.AgentName, typeof(string));
            grid[row, gridColAgent].Editor = null;

            grid[row, gridColOid] = new SourceGrid.Cells.Cell(channel.Oid, typeof(string));
            grid[row, gridColOid].Editor = null;
            
        }

        private void OnAddVariable(object sender, EventArgs e)
        {
            if (agentGrid.RowsCount > 1)
            {

                string var = GetUniqueVariableName();
                string statname;
                int [] sel = agentGrid.Selection.GetSelectionRegion().GetRowsIndex();
                if (sel.GetLength(0) > 0)
                    if (sel[0] > 1)
                        statname = agentGrid[sel[0], agentGridColName].DisplayText;
                    else
                        statname = agentGrid[1, agentGridColName].DisplayText;
                else
                    statname = agentGrid[1, agentGridColName].DisplayText;

                int[] rows = grid.Selection.GetSelectionRegion().GetRowsIndex();
                List<string> forbiddenNames = new List<string>();
                for (int i = 1; i < grid.RowsCount; i++)
                {
                    if (rows.Length > 0)
                    {
                        if (i != rows[0])
                            forbiddenNames.Add(grid[i, gridColName].DisplayText);
                    }
                    else
                        forbiddenNames.Add(grid[i, gridColName].DisplayText);
                }
                List<string> stations = new List<string>();
                for (int i = 1; i < agentGrid.RowsCount; i++)
                {
                    stations.Add(((SNMPAgent )agentGrid[i, agentGridColName].Tag).Name);
                }

                SNMPChannelImp ch = (SNMPChannelImp)ChannelFactory.CreateChannel(var, plugin, typeof(int), statname, "");
                ModifyChannelForm mcc = new ModifyChannelForm(ch, forbiddenNames, stations, statname);
                if ((ch = mcc.DoShow()) != null)
                {
                    AddVariable(ch);
                }
            }
            else
                MessageBox.Show(StringConstants.CannotCreateVariable, StringConstants.Error);
        }

        private void OnRemoveVariable(object sender, EventArgs e)
		{
			foreach(int row in grid.Selection.GetSelectionRegion().GetRowsIndex())
				grid.Rows.Remove(row);
		}

        private void LoadChannels()
        {
            foreach (SNMPChannelImp channel in plugin.Channels)
                AddVariable(channel);
        }

        private string GetUniqueVariableName()
        {
            string baseName = "variable_";
            int baseNumber = 1;
            for (; ; )
            {
                string newName = string.Format(System.Globalization.CultureInfo.CurrentUICulture, "{0}{1}", baseName, baseNumber);
                bool exists = false;
                for (int i = 1; i < grid.RowsCount; i++)
                {
                    if (grid[i, gridColName].DisplayText == newName)
                    {
                        exists = true;
                        break;
                    }
                }
                if (exists == false)
                    return newName;

                baseNumber++;
            }
        }

        private string GetUniqueAgentName()
        {
            string baseName = "agent_";
            int baseNumber = 1;
            for (; ; )
            {
                string newName = string.Format(System.Globalization.CultureInfo.CurrentUICulture, "{0}{1}", baseName, baseNumber);
                bool exists = false;
                for (int i = 1; i < agentGrid.RowsCount; i++)
                {
                    if (agentGrid[i, agentGridColName].DisplayText == newName)
                    {
                        exists = true;
                        break;
                    }
                }
                if (exists == false)
                    return newName;

                baseNumber++;
            }
        }

		private void OnOkClick(object sender, EventArgs e)
		{
			SaveSettings();
			Close();
		}

		private void OnCancelClick(object sender, EventArgs e)
		{
			Close();
		}

		private void SaveSettings()
		{
			Interfaces.IChannel[] channels = new Interfaces.IChannel[grid.RowsCount - 1];
			for (int i = 1; i < grid.RowsCount; i++)
			{
                channels[i - 1] = (Interfaces.IChannel)grid[i, gridColName].Tag;
            }
            plugin.Channels = channels;

            SNMPAgent[] agents = new SNMPAgent[agentGrid.RowsCount - 1];
            for (int i = 1; i < agentGrid.RowsCount; i++)
            {
                agents[i - 1] = (SNMPAgent)agentGrid[i, agentGridColName].Tag;
                agents[i - 1].AgentActive = (bool)agentGrid[i, agentGridColActive].Value;
            }
            plugin.Agents = agents;

            foreach (SNMPAgent stat in agents)
            {
                stat.ClearChannels();
                foreach (SNMPChannelImp chan in channels)
                    if (chan.AgentName == stat.Name)
                    {
                        stat.AddChannel(chan);
                        chan.MyAgent = (SNMPAgent)stat;
                    }
            }

			plugin.SaveSettings();
		}
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FreeSCADA.CLServer;
using System.ServiceModel;


namespace FreeSCADA.Communication.CLServer
{
	public partial class ImportProgressForm : Form
	{
		string serverAddress;
		string errorMessage;
		ChannelInfo[] channels;

		public string ErrorMessage { get { return errorMessage; } }
		public ChannelInfo[] Channels { get { return channels; } }

		public ImportProgressForm(string serverAddress)
		{
			this.serverAddress = serverAddress;

			InitializeComponent();
		}

		private void ImportProgressForm_Load(object sender, EventArgs e)
		{
			ChannelInfoRetriever retriever = new ChannelInfoRetriever(statusLabel, progressBar, this);
			System.Threading.ThreadPool.QueueUserWorkItem(x => retriever.GetChannels(serverAddress));
		}

		internal void OnChannelRetrievedComplete(bool isSuccess, ChannelInfo[] channels, string errorMessage)
		{
			this.channels = channels;
			this.errorMessage = errorMessage;

			if (isSuccess)
				DialogResult = DialogResult.OK;
			else
				DialogResult = DialogResult.Abort;
		}
	}

	class ChannelInfoRetriever
	{
		List<ChannelInfo> channels = new List<ChannelInfo>();
		Label statusLabel;
		ProgressBar progressBar;
		ImportProgressForm form;

		public ChannelInfoRetriever(Label statusLabel, ProgressBar progressBar, ImportProgressForm form)
		{
			this.statusLabel = statusLabel;
			this.progressBar = progressBar;
			this.form = form;
		}

		public void GetChannels(string serverAddress)
		{
			List<ChannelInfo> channels = new List<ChannelInfo>();
			ChannelInformationRetrieverClient client = null;
			try
			{
				WriteStatus("Connecting to server...");
				EndpointAddress epAddress = new EndpointAddress(serverAddress);
				client = new ChannelInformationRetrieverClient(new WSDualHttpBinding(WSDualHttpSecurityMode.None), epAddress);

				WriteStatus("Getting channel data...");
				if (client != null)
				{
					long channelCount = client.GetChannelsCount();
					SetProgressBarLimits((int)channelCount);
					for (long i = 0; i < channelCount; i++)
					{
						channels.Add(client.GetChannel(i));
						SetProgressBarValue((int)i);
					}
				}
				client.Close();
			}
			catch (Exception exception)
			{
				string message = string.Format("Error during connection to server: {0}", exception.Message);
				if (client != null && client.State == CommunicationState.Opened)
					client.Close();
				NotifyOperationComplete(false, channels.ToArray(), message);
				return;
			}

			NotifyOperationComplete(true, channels.ToArray(), "");
		}

		void WriteStatus(string text)
		{
			statusLabel.Invoke(new UpdateStatusDelegate(UpdateStatusSync), new object[] { statusLabel, text });
		}

		delegate void UpdateStatusDelegate(Label label, string text);
		public void UpdateStatusSync(Label label, string text)
		{
			label.Text = text;
		}

		delegate void NotifyOperationCompleteDelegate(ImportProgressForm form, bool isSuccess, ChannelInfo[] channels, string errorMessage);
		public void NotifyOperationCompleteSync(ImportProgressForm form, bool isSuccess, ChannelInfo[] channels, string errorMessage)
		{
			form.OnChannelRetrievedComplete(isSuccess, channels, errorMessage);
		}
		public void NotifyOperationComplete(bool isSuccess, ChannelInfo[] channels, string errorMessage)
		{
			form.Invoke(new NotifyOperationCompleteDelegate(NotifyOperationCompleteSync),
				new object[] { form, isSuccess, channels, errorMessage });
		}

		delegate void UpdateProgressBarDelegate(ProgressBar bar, int value);
		public void SetProgressBarLimitsSync(ProgressBar bar, int value)
		{
			bar.Minimum = 0;
			bar.Maximum = value;
			bar.Value = 0;
		}
		public void SetProgressBarValueSync(ProgressBar bar, int value)
		{
			bar.Value = value;
		}
		public void SetProgressBarLimits(int value)
		{
			form.Invoke(new UpdateProgressBarDelegate(SetProgressBarLimitsSync),
				new object[] { progressBar, value });
		}
		public void SetProgressBarValue(int value)
		{
			form.Invoke(new UpdateProgressBarDelegate(SetProgressBarValueSync),
				new object[] { progressBar, value });
		}
	}
}

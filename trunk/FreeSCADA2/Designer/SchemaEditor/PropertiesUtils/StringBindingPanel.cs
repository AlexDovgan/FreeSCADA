﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Data;
using System.Windows;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FreeSCADA.Common.Schema;
using FreeSCADA.Interfaces;

namespace FreeSCADA.Designer.SchemaEditor.PropertiesUtils
{
	internal partial class StringBindingPanel : BaseBindingPanel
	{
		List<IChannel> channels = new List<IChannel>();

		public StringBindingPanel()
		{
			InitializeComponent();
		}

		public override void Initialize(object element, PropertyInfo property, System.Windows.Data.BindingBase binding)
		{
			base.Initialize(element, property, binding);

			DependencyObject depObj;
			DependencyProperty depProp;
			GetPropertyObjects(element, property, out depObj, out depProp);
			if (depObj != null && depProp != null)
			{
				if (depObj.GetValue(depProp) is String)
					expressionEdit.Text = (string)depObj.GetValue(depProp);
			}

			System.Windows.Data.MultiBinding bind = binding as System.Windows.Data.MultiBinding;
			if (bind != null)
			{
				expressionEdit.Text = (bind.Converter as Kent.Boogaart.Converters.FormatConverter).FormatString;
				channels.Clear();
				foreach (System.Windows.Data.BindingBase bindingBase in bind.Bindings)
				{
					if (bindingBase is System.Windows.Data.Binding)
					{
						System.Windows.Data.Binding b = bindingBase as System.Windows.Data.Binding;
						if (b.Source is ChannelDataProvider)
						{
							ChannelDataProvider src = (ChannelDataProvider)b.Source;
                            
							channels.Add(src.Channel);
						}
					}
				}
				FillChannelsGrid();
			}
		}

		public override System.Windows.Data.BindingBase Save()
		{
			if (channels.Count > 0)
			{
				System.Windows.Data.MultiBinding multiBind = new MultiBinding();
				foreach (IChannel channel in channels)
				{
					System.Windows.Data.Binding bind = new System.Windows.Data.Binding("Value");
					ChannelDataProvider cdp = new ChannelDataProvider();
					cdp.ChannelName = channel.PluginId + "." + channel.Name;
					bind.Source = cdp;
                    cdp.Refresh();
					bind.FallbackValue = "{"+cdp.ChannelName+"}";
					multiBind.Bindings.Add(bind);
				}
				multiBind.Converter = new Kent.Boogaart.Converters.FormatConverter(expressionEdit.Text);

				return multiBind;
			}
			else
				return base.Save();
		}

		public override void AddChannel(IChannel channel)
		{
			if (channel != null)
			{
				channels.Add(channel);
				FillChannelsGrid();
			}
		}

		void FillChannelsGrid()
		{
			InitializeGrid();

			foreach (IChannel ch in channels)
			{
				int curRow = channelsGrid.RowsCount;
				channelsGrid.RowsCount++;

				channelsGrid[curRow, 0] = new SourceGrid.Cells.Cell(curRow - 1);
				channelsGrid[curRow, 1] = new SourceGrid.Cells.Cell(ch.Name);

				channelsGrid[curRow, 2] = new SourceGrid.Cells.Button("remove");
				SourceGrid.Cells.Controllers.Button buttonClickEvent = new SourceGrid.Cells.Controllers.Button();
				buttonClickEvent.Executed += new EventHandler(OnRemoveClicked);
				channelsGrid[curRow, 2].Controller.AddController(buttonClickEvent);

				channelsGrid.Rows[curRow].Tag = ch;
			}
		}

		void OnRemoveClicked(object sender, EventArgs e)
		{
			SourceGrid.CellContext context = (SourceGrid.CellContext)sender;
			int curRow = context.CellRange.Start.Row;
			IChannel ch = channelsGrid.Rows[curRow].Tag as IChannel;

			channels.Remove(ch);
			FillChannelsGrid();
		}

		void InitializeGrid()
		{
			channelsGrid.Selection.BackColor = Color.FromArgb(0, 0, 0, 0); //Don't show selection (transparent color)
			DevAge.Drawing.RectangleBorder b = channelsGrid.Selection.Border;
			b.SetWidth(0);
			channelsGrid.Selection.Border = b;
			channelsGrid.Selection.FocusBackColor = channelsGrid.Selection.BackColor;
			channelsGrid.ColumnsCount = 3;

			channelsGrid.RowsCount = 1;
			channelsGrid[0, 0] = new SourceGrid.Cells.ColumnHeader("Number");
			channelsGrid[0, 1] = new SourceGrid.Cells.ColumnHeader("Channel");
			channelsGrid[0, 2] = new SourceGrid.Cells.ColumnHeader("Action");

			channelsGrid.AutoStretchColumnsToFitWidth = true;
			channelsGrid.AutoSizeCells();
		}
	}

	internal class StringBindingPanelFactory : BaseBindingPanelFactory
	{
		override public bool CheckApplicability(object element, PropertyInfo property)
		{
			Type type = BaseBindingPanel.GetPropertyType(element, property);
			if (type.Equals(typeof(String)))
				return true;

			return false;
		}

		override public bool CanWorkWithBinding(System.Windows.Data.BindingBase binding)
		{
			if (binding != null && binding is System.Windows.Data.MultiBinding)
			{
				System.Windows.Data.MultiBinding bind = binding as System.Windows.Data.MultiBinding;
				if (bind.Converter is Kent.Boogaart.Converters.FormatConverter == false)
					return false;

				return true;
			}

			return false;
		}

		override public BaseBindingPanel CreateInstance()
		{
			return new StringBindingPanel();
		}

		override public string Name
		{
			get { return StringResources.StringBindingPanelName; }
		}
	}
}

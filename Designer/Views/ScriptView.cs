using System;
using System.IO;
using Alsing.SourceCode;
using FreeSCADA.Common.Scripting;
using FreeSCADA.Common;
using FreeSCADA.Interfaces;
using System.Collections.Generic;

namespace FreeSCADA.Designer.Views
{
	class ScriptView:DocumentView
	{
		private Alsing.Windows.Forms.SyntaxBoxControl syntaxBoxControl;
		private System.ComponentModel.IContainer components;

		Script script;
	
		public ScriptView(Script script)
		{
			this.script = script;

			InitializeComponent();

			DocumentName = script.Name;

            string iPyFormatXML = (new StreamReader(new MemoryStream(global::FreeSCADA.Designer.Properties.Resources.IronPython))).ReadToEnd();
            SyntaxDefinition syntax = SyntaxDefinition.FromSyntaxXml(iPyFormatXML);
            syntaxBoxControl.Document.Parser.Init(syntax);

			syntaxBoxControl.Document.Text = script.Text;
			IsModified = false;

			script.TextUpdated += new EventHandler(OnScriptTextUpdated);
			syntaxBoxControl.TextChanged += new System.EventHandler(this.syntaxBoxControl_TextChanged);

			System.Windows.Forms.ImageList imageList = new System.Windows.Forms.ImageList();
			imageList.Images.Add(global::FreeSCADA.Designer.Properties.Resources.log_warning);
			imageList.Images.Add(global::FreeSCADA.Designer.Properties.Resources.log_error);

			syntaxBoxControl.GutterIcons = imageList;
			InitializeCommands();
		}

		private void InitializeCommands()
		{
			// Commands to ToolStrip
			DocumentCommands.Add(new CommandInfo(new NullCommand((int)CommandManager.Priorities.EditCommands)));    // Separator
			DocumentCommands.Add(new CommandInfo(new ValidateCommand(this)));
		}


		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.syntaxBoxControl = new Alsing.Windows.Forms.SyntaxBoxControl();
			this.SuspendLayout();
			// 
			// syntaxBoxControl
			// 
			this.syntaxBoxControl.ActiveView = Alsing.Windows.Forms.ActiveView.BottomRight;
			this.syntaxBoxControl.AutoListPosition = null;
			this.syntaxBoxControl.AutoListSelectedText = "a123";
			this.syntaxBoxControl.AutoListVisible = false;
			this.syntaxBoxControl.BackColor = System.Drawing.Color.White;
			this.syntaxBoxControl.BorderStyle = Alsing.Windows.Forms.BorderStyle.None;
			this.syntaxBoxControl.CopyAsRTF = false;
			this.syntaxBoxControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.syntaxBoxControl.FontName = "Courier new";
			this.syntaxBoxControl.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.syntaxBoxControl.InfoTipCount = 1;
			this.syntaxBoxControl.InfoTipPosition = null;
			this.syntaxBoxControl.InfoTipSelectedIndex = 1;
			this.syntaxBoxControl.InfoTipVisible = false;
			this.syntaxBoxControl.Location = new System.Drawing.Point(0, 0);
			this.syntaxBoxControl.LockCursorUpdate = false;
			this.syntaxBoxControl.Name = "syntaxBoxControl";
			this.syntaxBoxControl.ShowScopeIndicator = false;
			this.syntaxBoxControl.Size = new System.Drawing.Size(799, 415);
			this.syntaxBoxControl.SmoothScroll = false;
			this.syntaxBoxControl.SplitviewH = -4;
			this.syntaxBoxControl.SplitviewV = -4;
			this.syntaxBoxControl.TabGuideColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(219)))), ((int)(((byte)(214)))));
			this.syntaxBoxControl.TabIndex = 1;
			this.syntaxBoxControl.Text = "syntaxBoxControl1";
			this.syntaxBoxControl.WhitespaceColor = System.Drawing.SystemColors.ControlDark;
			// 
			// ScriptView
			// 
			this.ClientSize = new System.Drawing.Size(799, 415);
			this.Controls.Add(this.syntaxBoxControl);
			this.Name = "ScriptView";
			this.ResumeLayout(false);

		}

		void OnScriptTextUpdated(object sender, EventArgs e)
		{
			syntaxBoxControl.Document.Text = script.Text;
			Activate();
		}

		private void syntaxBoxControl_TextChanged(object sender, EventArgs e)
		{
			IsModified = true;

			script.TextUpdated -= new EventHandler(OnScriptTextUpdated);
			script.Text = syntaxBoxControl.Document.Text;
			script.TextUpdated += new EventHandler(OnScriptTextUpdated);
		}

		public override bool SaveDocument()
		{
			script.Text = syntaxBoxControl.Document.Text;
			script.Save();
			IsModified = false;
			return true;
		}

		public void MoveCaretToEnd()
		{
			syntaxBoxControl.Caret.MoveAbsoluteEnd(false);
		}

		public void DoSyntaxCheck()
		{
			script.Text = syntaxBoxControl.Document.Text;
			List<Script.ErrorInfo> errors = script.Validate();

			foreach (Row row in syntaxBoxControl.Document)
				row.Images.Clear();

			foreach (Script.ErrorInfo err in errors)
			{
				int line = 0;
				if (err.Line >= 0)
					line = err.Line;

				switch (err.Severity)
				{
					case Script.ErrorInfo.SeverityType.Error:
						syntaxBoxControl.Document[line].Images.Add(1);
						break;
					case Script.ErrorInfo.SeverityType.Warning:
						syntaxBoxControl.Document[line].Images.Add(0);
						break;
				}
			}
		}
	}


	class ValidateCommand : BaseCommand
	{
		ScriptView view;
		public ValidateCommand(ScriptView view)
		{
			this.view = view;
			Priority = (int)CommandManager.Priorities.EditCommands;
			CanExecute = true;
		}

		#region ICommand Members
		public override void Execute()
		{
			view.DoSyntaxCheck();
		}

		public override string Name
		{
			get { return StringResources.CommandValidateName; }
		}

		public override string Description
		{
			get { return StringResources.CommandValidateDescription; }
		}

		public override System.Drawing.Bitmap Icon
		{
			get
			{
				return global::FreeSCADA.Designer.Properties.Resources.checkmark;
			}
		}
		#endregion ICommand Members
	}
}

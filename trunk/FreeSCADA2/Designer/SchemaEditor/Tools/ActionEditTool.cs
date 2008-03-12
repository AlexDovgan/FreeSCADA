using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using FreeSCADA.Common.Schema;
using FreeSCADA.Common.Schema.Actions;
using FreeSCADA.ShellInterfaces;
using FreeSCADA.Common;
namespace FreeSCADA.Designer.SchemaEditor.Tools
{
    class ActionEditTool:BaseTool,ITool
    {

        #region ITool Implementation
        public String ToolName
        {
            get { return "ActionEdit Tool"; }
        }

        public String ToolGroup
        {
            get { return "Actions Tools"; }
        }
        public System.Drawing.Bitmap ToolIcon
        {
            get
            {

                return new System.Drawing.Bitmap(10, 10);
            }
        }
        #endregion

        public ActionEditTool(SchemaDocument doc)
            : base(doc)
        {

        }
        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {

            base.OnPreviewMouseLeftButtonDown(e);
            if (SelectedObject != null)
            {
                RotateAction a = new RotateAction();
                a.ActionChannelName = "data_simulator_plug.Test1";
                if(a.CheckActionFor(SelectedObject as FrameworkElement))
                    ActionsCollection.GetActions((SelectedObject as FrameworkElement)).ActionsList.Add(a);
                workedSchema.LinkActions();
                Env.Current.CommunicationPlugins.Connect();
            }
            
        }
 
    }
}

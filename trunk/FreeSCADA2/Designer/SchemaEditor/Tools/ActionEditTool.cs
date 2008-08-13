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
using FreeSCADA.Designer.SchemaEditor.Manipulators;

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
            get { return "Editor Tools"; }
        }
        public System.Drawing.Bitmap ToolIcon
        {
            get
            {
                return global::FreeSCADA.Designer.Properties.Resources.cog_edit;
            }
        }
        #endregion

        public ActionEditTool(UIElement elemnet)
            : base(elemnet)
        {
            
        }
        protected override BaseManipulator CreateToolManipulator(UIElement obj)
        {
            ActionsEditManipulator manipulator;
            if (ToolManipulator != null)
                (ToolManipulator as ActionsEditManipulator).ActionSelected -= manipulator_ActionSelected;
            manipulator=new ActionsEditManipulator(obj);
            manipulator.ActionSelected += new ActionsEditManipulator.ActionSelectedDelegate(manipulator_ActionSelected);
            return manipulator;
        }

        void manipulator_ActionSelected(BaseAction a)
        {
            RaiseObjectSelected(a);
        }
        protected override void OnPreviewMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            //ReleaseMouseCapture();
            //base.OnPreviewMouseLeftButtonUp(e);
        }
        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonDown(e);
            e.Handled = false;
        }
 
    }
}

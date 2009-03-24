using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Drawing;
using System.Windows;
using System.Windows.Forms.Design;
using FreeSCADA.Common.Schema;


namespace FreeSCADA.Designer.SchemaEditor.PropertiesUtils.PropertyGridTypeEditors
{
    class BrushEditor:UITypeEditor
	{
        public BrushEditor()
		{
		}

		// Indicates whether the UITypeEditor provides a form-based (modal) dialog, 
		// drop down dialog, or no UI outside of the properties window.
		public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
		{
			// if(context.Instance is System.Windows.Controls.ContentControl)
			return UITypeEditorEditStyle.Modal;
			// else
			//     return UITypeEditorEditStyle.None;
		}

		// Displays the UI for value selection.
		public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, System.IServiceProvider provider, object value)
		{

			PropertiesUtils.PropertyWrapper pw;
			if ((pw = context.PropertyDescriptor as PropertiesUtils.PropertyWrapper) == null)
				return value;

            DependencyObject depObj;
            DependencyProperty depProp;
            pw.GetWpfObjects(out depObj, out depProp);
            if (depObj == null || depProp == null)
                return value;
            
			IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
			if (edSvc != null)
			{

                System.Windows.Forms.ColorDialog cd = new System.Windows.Forms.ColorDialog();
                if (cd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    System.Windows.Media.SolidColorBrush scb= new System.Windows.Media.SolidColorBrush();
                    scb.Color=System.Windows.Media.Color.FromArgb(
                        cd.Color.A,
                        cd.Color.R,
                        cd.Color.G,
                        cd.Color.B);
                    return scb;
				}
			}

			return value;
		}


		public override void PaintValue(System.Drawing.Design.PaintValueEventArgs e)
		{
            System.Windows.Media.SolidColorBrush b = (System.Windows.Media.SolidColorBrush)e.Value;
            Color c = Color.FromArgb(
                    b.Color.A,
                    b.Color.R,
                    b.Color.G,
                    b.Color.B);
            e.Graphics.FillRectangle(new SolidBrush(c), e.Bounds);
           
		}


		// Indicates whether the UITypeEditor supports painting a 
		// representation of a property's value.
		public override bool GetPaintValueSupported(System.ComponentModel.ITypeDescriptorContext context)
		{

            return true;
		}
	}
    
   
}

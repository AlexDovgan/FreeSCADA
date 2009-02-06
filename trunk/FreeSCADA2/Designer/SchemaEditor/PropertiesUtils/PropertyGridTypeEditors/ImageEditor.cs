using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows;
using System.Windows.Forms.Design;
using FreeSCADA.Common.Schema;


namespace FreeSCADA.Designer.SchemaEditor.PropertiesUtils.PropertyGridTypeEditors
{
	class ImageEditor : System.Drawing.Design.UITypeEditor
	{
		public ImageEditor()
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

			DependencyObject depObj = pw.ControlledObject as DependencyObject;

			DependencyPropertyDescriptor dpd = DependencyPropertyDescriptor.FromProperty(pw.ControlledProperty);
			if (depObj == null || dpd == null)
				return value;
			DependencyProperty depProp = dpd.DependencyProperty;
			//if (!(depProp is System.Windows.Controls.ContentControl)  )
			//  return value;

			IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
			if (edSvc != null)
			{

				ContentEditorDialog dlg = new ContentEditorDialog(true);
				System.Windows.Forms.DialogResult res = edSvc.ShowDialog(dlg);

				if (res == System.Windows.Forms.DialogResult.OK && dlg.ImagesList.Count > 0)
				{
					if (depObj is AnimatedImage)
						depObj.SetValue(depProp, dlg.ImagesList[0]);
					else if (depObj is System.Windows.Controls.ContentControl)
					{
						AnimatedImage ai = new AnimatedImage();
						ai.ImageName = dlg.ImagesList[0];
						ai.AnimatedControl = true;
						System.ComponentModel.TypeDescriptor.AddAttributes(ai, new Attribute[] { new System.Windows.Markup.RuntimeNamePropertyAttribute(ai.ImageName) });
						depObj.SetValue(depProp, ai);
					}
				}
			}

			return value;
		}


		public override void PaintValue(System.Drawing.Design.PaintValueEventArgs e)
		{
			return;
		}


		// Indicates whether the UITypeEditor supports painting a 
		// representation of a property's value.
		public override bool GetPaintValueSupported(System.ComponentModel.ITypeDescriptorContext context)
		{

			return false;
		}
	}
}

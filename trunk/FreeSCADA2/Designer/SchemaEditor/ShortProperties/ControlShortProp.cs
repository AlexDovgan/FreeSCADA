using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace FreeSCADA.Designer.SchemaEditor.ShortProperties
{


    class ControlShortProp : FrameworkElementShortProp
    {
        Control control;
        public ControlShortProp(Control c)
            : base(c)
        {
            control = c;
        }
        [EditorAttribute(typeof(System.Windows.Forms.Design.FileNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [Description("Object's Style"), Category("Appearence")]
        public string Style
        {
            get
            {
                FrameworkElement c = (FrameworkElement)control.Parent;
                if (control.Style != null)
                {
                    foreach (System.Collections.DictionaryEntry de in c.Resources)
                    {
                        if (de.Value == control.Style)
                            return de.Key.ToString();

                    }
                    return control.Style.GetType().Name;
                }
                else return "";
            }
            set
            {
                RaisePropertiesBrowserChanged((UIElement)control);
                Style st = (Style)XamlReader.Load(File.Open(value, FileMode.Open));
                FrameworkElement c = (FrameworkElement)control.Parent;
                if (c.Resources == null)
                    c.Resources = new ResourceDictionary();
                if (!(c.Resources as ResourceDictionary).Contains(System.IO.Path.GetFileNameWithoutExtension(value)))
                    (c.Resources as ResourceDictionary).Add(System.IO.Path.GetFileNameWithoutExtension(value), st);
                control.SetResourceReference(FrameworkElement.StyleProperty, System.IO.Path.GetFileNameWithoutExtension(value));

            }
        }


    }
}

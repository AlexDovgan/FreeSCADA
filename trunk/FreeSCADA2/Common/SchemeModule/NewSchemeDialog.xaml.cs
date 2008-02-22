using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FreeSCADA.Scheme
{
    /// <summary>
    /// Interaction logic for NewSchemeDialog.xaml
    /// </summary
    
    public partial class NewSchemeDialog : Window
    {
    
        public class SchemeParams
        {
            public string Name;
            public double Width=800;
            public double Height=600;
            public string SchemeName
            {
                get { return Name; }
                set { Name = value; }
            }
            public string  SchemeWidth
            {
                get{return Convert.ToString(Width);}
                set {Width=Convert.ToDouble(value);}
            }
            public string SchemeHeight
            {
                get { return Convert.ToString(Height); }
                set { Height = Convert.ToInt32(value); }
            }

        }

        
        public NewSchemeDialog()
        {
            InitializeComponent();
           
        }

        private void buttonOk_Click(object sender, RoutedEventArgs e)
        {
            if (SchemeNameTextBox.Text == "")
            {
                MessageBox.Show("Please enter Scheme name");
            }
            else
            {
                DialogResult = true;
                Close();
            }
        }
    }
}

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

namespace FreeSCADA.Schema
{
    /// <summary>
    /// Interaction logic for NewSchemaDialog.xaml
    /// </summary
    
    public partial class NewSchemaDialog : Window
    {
    
        public class SchemaParams
        {
            public string Name;
            public double Width=800;
            public double Height=600;
            public string SchemaName
            {
                get { return Name; }
                set { Name = value; }
            }
            public string  SchemaWidth
            {
                get{return Convert.ToString(Width);}
                set {Width=Convert.ToDouble(value);}
            }
            public string SchemaHeight
            {
                get { return Convert.ToString(Height); }
                set { Height = Convert.ToInt32(value); }
            }

        }

        
        public NewSchemaDialog()
        {
            InitializeComponent();
           
        }

        private void buttonOk_Click(object sender, RoutedEventArgs e)
        {
            if (SchemaNameTextBox.Text == "")
            {
                MessageBox.Show("Please enter Schema name");
            }
            else
            {
                DialogResult = true;
                Close();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace test_event
{
    /// <summary>
    /// Interaction logic for test.xaml
    /// </summary>
    public partial class Test : Window
    {
        public Test()
        {
            InitializeComponent();
            if (!App.Windows.ContainsKey("test"))
            {
                App.Windows.Add("test", this);
            }
            
            //ValueChanged = onValueChange;
            //Console.WriteLine("Value: " + Value.Text);
            //Value.TextChanged += onChangeText;
            //btnAff.Click += BtnAff_Click;
            if (App.Windows.ContainsKey("main"))
            {
                ((MainWindow)App.Windows["main"]).ValueChanged += Test_ValueChanged;
            }
            
        }

        private void Test_ValueChanged(string obj)
        {
            this.Value.Text = obj;
        }

        private void BtnAff_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Valeur: " + Value.Text);
        }

        private void onChangeText(object sender, TextChangedEventArgs e)
        {
            Console.WriteLine("Changement");
        }

        public void onValueChange(string valeur)
        {
            
            Console.WriteLine("avant: " + Value.Text);
            Value.Text = valeur;
            Console.WriteLine("après: " + Value.Text + "\n");
        }
    }
}

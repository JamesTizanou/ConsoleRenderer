using System.Security.Cryptography.X509Certificates;
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

namespace test_event
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Test test = new Test();
        public event Action<string> ValueChanged;
        public MainWindow()
        {
            InitializeComponent();
            App.Windows.Add("main",this);
            var test = new Test();
            test.Show();

            ChangerValeur.Click += ChangeValue;
        }

        private void ChangeValue(object sender, RoutedEventArgs e)
        {
            Valeur.Text = new Random().Next(0, 1000).ToString();
            ValueChanged.Invoke(Valeur.Text);
        }
    }
}
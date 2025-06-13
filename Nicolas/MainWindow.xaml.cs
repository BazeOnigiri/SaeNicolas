using Nicolas.Classes;
using Nicolas.UCs;
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

namespace Nicolas
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Commande> commandes;

        public MainWindow()
        {
            InitializeComponent();
            UCLogin login = new UCLogin();
            UCRechercherVin rechercherVin = new UCRechercherVin();
            mainGrid.Children.Add(login);


        }
    }
}
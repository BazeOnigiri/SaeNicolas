using Nicolas.Windows;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Nicolas.UCs
{
    /// <summary>
    /// Logique d'interaction pour UCAjouterDemande.xaml
    /// </summary>
    public partial class UCAjouterDemande : UserControl
    {
        public UCAjouterDemande(Classes.Vin vinSelectionne)
        {
            InitializeComponent();
        }

        private void butNouveauClient_Click(object sender, RoutedEventArgs e)
        {
            FicheClient client = new FicheClient();
            client.Show();
        }
    }
}

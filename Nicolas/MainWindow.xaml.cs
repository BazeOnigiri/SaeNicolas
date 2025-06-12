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
            mainGrid.Children.Add(login);

            /*Commande uneCommande = new Commande(0, 0, null, null, null);
            commandes = uneCommande.FindAll();
            foreach (Commande commande in commandes)
            {
                if (commande.Valider.HasValue && commande.Valider.Value)
                {
                    // Logique pour les commandes validées
                }
                else
                {
                    uneCommande = commande;
                    break;
                }
            }
            UCModifierCommande modifierCommande = new UCModifierCommande(uneCommande);*/

           /* UCVisualiserCommandes visualiserCommande = new UCVisualiserCommandes();
            UCVisualiserDemandes visualiserDemandes = new UCVisualiserDemandes();*/

        }
    }
}
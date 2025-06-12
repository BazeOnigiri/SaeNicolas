using Nicolas.Classes;
using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
    /// Logique d'interaction pour UCModifierCommande.xaml
    /// </summary>
    public partial class UCModifierCommande : UserControl
    {
        private Commande commande;

        public UCModifierCommande(Commande commande)
        {
            InitializeComponent();
            this.commande = commande;
            ChargerDetailsCommande();
            ChargerToutesDemandes();
        }

        private void ChargerDetailsCommande()
        {
            // Affichage des infos de la commande
            txtNumCommande.Text = commande.NumCommande.ToString();
            txtDateCommande.Text = commande.DateCommande?.ToString("dd/MM/yyyy") ?? "";
            txtPrixTotal.Text = commande.PrixTotal?.ToString("F2") + " €";

            // Récupérer les demandes associées à la commande
            var demandesAssociees = new List<dynamic>();
            var toutesDemandes = new Demande().FindAll();
            foreach (var demande in toutesDemandes.Where(d => d.NumCommande == commande.NumCommande))
            {
                var vin = new Vin(demande.NumVin, 0, 0, 0, null, null, null, null);
                vin.Read();
                demandesAssociees.Add(new
                {
                    NumDemande = demande.NumDemande,
                    NomVin = vin.Nomvin,
                    Quantite = demande.QuantiteDemande,
                    PrixVin = vin.PrixVin,
                    DateDemande = demande.DateDemande?.ToString("dd/MM/yyyy")
                });
            }
            dgDemandesAssociees.ItemsSource = demandesAssociees;
        }

        private void ChargerToutesDemandes()
        {
            // Afficher toutes les demandes dans le DataGrid du bas
            var toutesDemandes = new Demande().FindAll();
            dgToutesDemandes.ItemsSource = toutesDemandes;
        }
    }
}

using Microsoft.Ajax.Utilities;
using Nicolas.Classes;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Logique d'interaction pour UCVisualiserCommandes.xaml
    /// </summary>
    public partial class UCVisualiserCommandes : UserControl
    {
        private List<Commande> commandes;

        public string Recherche { get; set; }
        public string Millesime { get; set; }
        public string TypeVin { get; set; }
        public string Appelation { get; set; }
        public string PrixMin { get; set; }
        public string PrixMax { get; set; }
        public DateTime? DateCommande { get; set; }

        public UCVisualiserCommandes()
        {
            InitializeComponent();
            Commande uneCommande = new Commande(0, 0, null, null, null);
            commandes = uneCommande.FindAll();
            dataGridCommandes.ItemsSource = commandes;
            DataContext = this;
        }


        private void btnFiltre_Click(object sender, RoutedEventArgs e)
        {
            popupFiltre.IsOpen = !popupFiltre.IsOpen;
        }

        private void btnClearFiltres_Click(object sender, RoutedEventArgs e)
        {
            btnClearFiltres.IsEnabled = false;

            Recherche = string.Empty;
            Millesime = string.Empty;
            TypeVin = string.Empty;
            Appelation = string.Empty;
            PrixMin = string.Empty;
            PrixMax = string.Empty;
            DateCommande = null;

            textBoxRecherche.Text = "";
            textBoxMillesime.Text = "";
            comboBoxTypeVin.SelectedIndex = 0;
            comboBoxAppelation.SelectedIndex = 0;
            textBoxPrixMin.Text = "";
            textBoxPrixMax.Text = "";
            datePickerCommande.SelectedDate = null;

            dataGridCommandes.ItemsSource = commandes;
        }

        private void btnAppliquerFiltre_Click(object sender, RoutedEventArgs e)
        {
            btnClearFiltres.IsEnabled = true;

            popupFiltre.IsOpen = false;
            IEnumerable<Commande> filtered = commandes;

            // Recherche sur l'employé
            if (!string.IsNullOrWhiteSpace(Recherche))
            {
                filtered = filtered.Where(c =>
                {
                    var employe = new Employe(c.NumEmploye, 0, null, null, null);
                    employe.Read();
                    return (employe.Nom + " " + employe.Prenom).IndexOf(Recherche, StringComparison.OrdinalIgnoreCase) >= 0;
                });
            }

            // Millésime
            if (!string.IsNullOrWhiteSpace(Millesime) && int.TryParse(Millesime, out int millesime))
            {
                filtered = filtered.Where(c =>
                {
                    var details = new DetailCommande(c.NumCommande, 0, null, null).FindAll()
                        .Where(d => d.NumCommande == c.NumCommande).ToList();
                    foreach (var detail in details)
                    {
                        var vin = new Vin(detail.NumVin, 0, 0, 0, null, null, null, null);
                        vin.Read();
                        if (vin.Millesime == millesime)
                            return true;
                    }
                    return false;
                });
            }

            // Type de vin
            if (!string.IsNullOrWhiteSpace(TypeVin))
            {
                filtered = filtered.Where(c =>
                {
                    var details = new DetailCommande(c.NumCommande, 0, null, null).FindAll()
                        .Where(d => d.NumCommande == c.NumCommande).ToList();
                    foreach (var detail in details)
                    {
                        var vin = new Vin(detail.NumVin, 0, 0, 0, null, null, null, null);
                        vin.Read();
                        if (vin.NumTypeVin.ToString() == TypeVin)
                            return true;
                    }
                    return false;
                });
            }

            // Appelation
            if (!string.IsNullOrWhiteSpace(Appelation))
            {
                filtered = filtered.Where(c =>
                {
                    var details = new DetailCommande(c.NumCommande, 0, null, null).FindAll()
                        .Where(d => d.NumCommande == c.NumCommande).ToList();
                    foreach (var detail in details)
                    {
                        var vin = new Vin(detail.NumVin, 0, 0, 0, null, null, null, null);
                        vin.Read();
                        if (vin.NumAppelation.ToString() == Appelation)
                            return true;
                    }
                    return false;
                });
            }

            // Prix min
            if (decimal.TryParse(PrixMin, out decimal prixMin))
                filtered = filtered.Where(c => c.PrixTotal >= prixMin);

            // Prix max
            if (decimal.TryParse(PrixMax, out decimal prixMax))
                filtered = filtered.Where(c => c.PrixTotal <= prixMax);

            // Date
            if (DateCommande != null)
                filtered = filtered.Where(c => c.DateCommande?.Date == DateCommande.Value.Date);

            dataGridCommandes.ItemsSource = filtered.ToList();
        }

        private string TryGetValue(DataRowView row, string nomColonne)
        {
            if (row.DataView.Table.Columns.Contains(nomColonne) && row[nomColonne] != DBNull.Value)
                return row[nomColonne].ToString();
            return "";
        }

        private void dataGridCommandes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGridCommandes.SelectedItem is Commande commande)
            {
                // Récupération du nom de l'employé
                Employe employe = new Employe(commande.NumEmploye, 0, null, null, null);
                employe.Read();

                // Récupération des vins de la commande
                List<DetailCommande> details = new DetailCommande(commande.NumCommande, 0, null, null).FindAll()
                    .Where(d => d.NumCommande == commande.NumCommande).ToList();

                // Récupération des infos vins
                List<string> vins = new List<string>();
                foreach (DetailCommande detail in details)
                {
                    Vin vin = new Vin(detail.NumVin, 0, 0, 0, null, null, null, null);
                    vin.Read();
                    vins.Add($"{vin.Nomvin}\n (Quantité : {detail.Quantite})");
                }

                // Affichage des détails
                textBlockDetails.Text =
                    $"IDcommande : {commande.NumCommande}\n" +
                    $"nomEmploye : {employe.Nom} {employe.Prenom}\n" +
                    $"Montant : {commande.PrixTotal} €\n" +
                    $"validé : {(commande.Valider == true ? "Oui" : "Non")}";

                itemsVins.ItemsSource = vins;
            }
            else
            {
                textBlockDetails.Text = "Sélectionnez une commande pour voir les détails.";
                itemsVins.ItemsSource = null;
            }
        }

        private void BtnSupprimer_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridCommandes.SelectedItem is DataRowView row)
            {
                string numCommande = TryGetValue(row, "NumCommande");
                if (int.TryParse(numCommande, out int id))
                {
                    if (MessageBox.Show("Confirmer la suppression de la commande ?", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        try
                        {
                            string sql = "DELETE FROM Commande WHERE NumCommande = @id";
                            using (NpgsqlCommand cmd = new NpgsqlCommand(sql))
                            {
                                cmd.Parameters.AddWithValue("@id", id);
                                DataAccess.Instance.ExecuteSet(cmd);
                            }
                            textBlockDetails.Text = "Commande supprimée.";
                            itemsVins.ItemsSource = null;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Erreur lors de la suppression : " + ex.Message);
                        }
                    }
                }
            }
        }

        private void BtnModifier_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Fonctionnalité de modification à implémenter.");
        }

        private void BtnAjouter_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Fonctionnalité d’ajout à implémenter.");
        }
    }
}

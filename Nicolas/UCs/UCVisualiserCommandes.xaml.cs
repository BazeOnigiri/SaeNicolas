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
        public List<Appelation> ListeAppelations { get; set; }
        public List<TypeVin> ListeTypesVin { get; set; }

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
            Appelation uneAppelation = new Appelation(0, null);
            TypeVin unTypeVin = new TypeVin(0, null);

            commandes = uneCommande.FindAll();
            dataGridCommandes.ItemsSource = commandes;

            ListeAppelations = uneAppelation.FindAll();
            comboBoxAppelation.Items.Clear();
            comboBoxAppelation.ItemsSource = ListeAppelations;
            comboBoxAppelation.DisplayMemberPath = "NomAppelation";
            comboBoxAppelation.SelectedValuePath = "NumType2";

            ListeTypesVin = unTypeVin.FindAll();
            comboBoxTypeVin.Items.Clear();
            comboBoxTypeVin.ItemsSource = ListeTypesVin;
            comboBoxTypeVin.DisplayMemberPath = "Nomtype";
            comboBoxTypeVin.SelectedValuePath = "NumType";

            dataGridCommandes.Items.Filter = RechercheMotClef;


            DataContext = this;
        }

        private bool RechercheMotClef(object obj)
        {
            if (string.IsNullOrEmpty(textBoxRecherche.Text))
                return true;

            var commande = obj as Commande;
            if (commande == null)
                return false;

            // Récupérer les détails de la commande
            var details = new DetailCommande(commande.NumCommande, 0, null, null).FindAll()
                .Where(d => d.NumCommande == commande.NumCommande).ToList();

            foreach (var detail in details)
            {
                // Récupérer le vin associé
                var vin = new Vin(detail.NumVin, 0, 0, 0, null, null, null, null);
                vin.Read();

                // Vérifier le nom du vin
                if (!string.IsNullOrEmpty(vin.Nomvin) &&
                    vin.Nomvin.StartsWith(textBoxRecherche.Text, StringComparison.OrdinalIgnoreCase))
                    return true;

                // Récupérer le fournisseur associé
                var fournisseur = new Fournisseur(vin.NumFournisseur, null);
                fournisseur.Read();

                if (!string.IsNullOrEmpty(fournisseur.NomFournisseur) &&
                    fournisseur.NomFournisseur.StartsWith(textBoxRecherche.Text, StringComparison.OrdinalIgnoreCase))
                    return true;
            }

            return false;
        }

        private void textBoxRecherche_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(dataGridCommandes.ItemsSource).Refresh();
        }

        private void textBoxRecherche_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                CollectionViewSource.GetDefaultView(dataGridCommandes.ItemsSource).Refresh();
            }
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
            comboBoxTypeVin.SelectedIndex = -1;
            comboBoxAppelation.SelectedIndex = -1;
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
            if (dataGridCommandes.SelectedItem is Commande commande)
            {
                if (MessageBox.Show("Confirmer la suppression de la commande ?", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    try
                    {
                        // 1. Supprimer les détails de la commande
                        var details = new DetailCommande(commande.NumCommande, 0, null, null)
                            .FindAll().Where(d => d.NumCommande == commande.NumCommande).ToList();

                        foreach (var detail in details)
                        {
                            detail.Delete();
                        }

                        // 2. Mettre à jour les demandes liées (numCommande à null)
                        var demandesLiees = new Demande(0, 0, 0, commande.NumCommande, 0, null, null, null)
                            .FindAll().Where(d => d.NumCommande == commande.NumCommande).ToList();

                        foreach (var demande in demandesLiees)
                        {
                            demande.NumCommande = null;
                            demande.Update();
                        }

                        // 3. Supprimer la commande
                        int result = commande.Delete();
                        if (result > 0)
                        {
                            commandes.Remove(commande);
                            dataGridCommandes.ItemsSource = null;
                            dataGridCommandes.ItemsSource = commandes;
                            textBlockDetails.Text = "Commande supprimée.";
                            itemsVins.ItemsSource = null;
                        }
                        else
                        {
                            MessageBox.Show("La commande n'a pas pu être supprimée.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erreur lors de la suppression : " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner une commande à supprimer.");
            }
        }

        private void BtnModifier_Click(object sender, RoutedEventArgs e)
        {
            // On récupère la commande sélectionnée dans le DataGrid
            if (dataGridCommandes.SelectedItem is Commande commande)
            {
                // On vérifie que la commande n'est pas validée
                if (commande.Valider == true)
                {
                    MessageBox.Show("Impossible de modifier une commande validée.");
                    return;
                }

                // Navigation vers l'UC de modification
                var mainWindow = Application.Current.MainWindow as MainWindow;
                if (mainWindow != null)
                {
                    mainWindow.mainGrid.Children.Clear();
                    mainWindow.mainGrid.Children.Add(new UCModifierCommande(commande));
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner une commande à modifier.");
            }
        }

        private void BtnAjouter_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Fonctionnalité d’ajout à implémenter.");
        }
    }
}

using Nicolas.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Nicolas.UCs
{
    public partial class UCRechercherVin : UserControl
    {
        private List<Vin> vins;
        public List<Appelation> ListeAppelations { get; set; }
        public List<TypeVin> ListeTypesVin { get; set; }

        // Propriétés de filtrage
        public string Recherche { get; set; }
        public string Millesime { get; set; }
        public string TypeVin { get; set; }
        public string Appelation { get; set; }
        public string PrixMin { get; set; }
        public string PrixMax { get; set; }

        public UCRechercherVin()
        {
            InitializeComponent();
            ChargerDonnees();
            DataContext = this;
            dataGridVins.Items.Filter = FiltrerVins;
        }

        private void ChargerDonnees()
        {
            // Chargement des vins
            var unVin = new Vin(0, 0, 0, 0, null, null, null, null);
            vins = unVin.FindAll();
            dataGridVins.ItemsSource = vins;

            // Chargement des appellations
            var uneAppelation = new Appelation(0, null);
            ListeAppelations = uneAppelation.FindAll();
            comboBoxAppelation.ItemsSource = ListeAppelations;
            comboBoxAppelation.DisplayMemberPath = "NomAppelation";
            comboBoxAppelation.SelectedValuePath = "NumType2";

            // Chargement des types de vin
            var unTypeVin = new TypeVin(0, null);
            ListeTypesVin = unTypeVin.FindAll();
            comboBoxTypeVin.ItemsSource = ListeTypesVin;
            comboBoxTypeVin.DisplayMemberPath = "Nomtype";
            comboBoxTypeVin.SelectedValuePath = "NumType";
        }

        private bool FiltrerVins(object item)
        {
            if (!(item is Vin vin))
                return false;

            // Filtre de recherche texte
            if (!string.IsNullOrEmpty(Recherche))
            {
                bool matchRecherche = vin.Nomvin.Contains(Recherche, StringComparison.OrdinalIgnoreCase);
                if (!matchRecherche)
                    return false;
            }

            return true;
        }

        private void textBoxRecherche_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                CollectionViewSource.GetDefaultView(dataGridVins.ItemsSource).Refresh();
            }
        }

        private void btnFiltre_Click(object sender, RoutedEventArgs e)
        {
            popupFiltre.IsOpen = !popupFiltre.IsOpen;
        }

        private void btnClearFiltres_Click(object sender, RoutedEventArgs e)
        {
            btnClearFiltres.IsEnabled = false;

            // Réinitialisation des filtres
            Recherche = string.Empty;
            Millesime = string.Empty;
            TypeVin = string.Empty;
            Appelation = string.Empty;
            PrixMin = string.Empty;
            PrixMax = string.Empty;

            // Réinitialisation des contrôles
            textBoxRecherche.Text = string.Empty;
            textBoxMillesime.Text = string.Empty;
            comboBoxTypeVin.SelectedIndex = -1;
            comboBoxAppelation.SelectedIndex = -1;
            textBoxPrixMin.Text = string.Empty;
            textBoxPrixMax.Text = string.Empty;

            dataGridVins.ItemsSource = vins;
            CollectionViewSource.GetDefaultView(dataGridVins.ItemsSource).Refresh();
        }

        private void btnAppliquerFiltre_Click(object sender, RoutedEventArgs e)
        {
            btnClearFiltres.IsEnabled = true;
            popupFiltre.IsOpen = false;

            var vinsFiltered = vins.AsEnumerable();

            // Filtre par millésime
            if (!string.IsNullOrEmpty(Millesime) && int.TryParse(Millesime, out int millesime))
            {
                vinsFiltered = vinsFiltered.Where(v => v.Millesime == millesime);
            }

            // Filtre par type de vin
            if (!string.IsNullOrEmpty(TypeVin))
            {
                vinsFiltered = vinsFiltered.Where(v => v.NumTypeVin.ToString() == TypeVin);
            }

            // Filtre par appellation
            if (!string.IsNullOrEmpty(Appelation))
            {
                vinsFiltered = vinsFiltered.Where(v => v.NumAppelation.ToString() == Appelation);
            }

            // Filtre par prix
            if (decimal.TryParse(PrixMin, out decimal prixMin))
            {
                vinsFiltered = vinsFiltered.Where(v => (decimal) v.PrixVin >= prixMin);
            }
            if (decimal.TryParse(PrixMax, out decimal prixMax))
            {
                vinsFiltered = vinsFiltered.Where(v => (decimal)v.PrixVin <= prixMax);
            }

            dataGridVins.ItemsSource = vinsFiltered.ToList();
        }

        private void dataGridVins_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGridVins.SelectedItem is Vin vin)
            {
                var appelation = new Appelation(vin.NumAppelation, null);
                appelation.Read();

                var typeVin = new TypeVin(vin.NumTypeVin, null);
                typeVin.Read();

                var fournisseur = new Fournisseur(vin.NumFournisseur, null);
                fournisseur.Read();

                textBlockDetails.Text =
                    $"Nom : {vin.Nomvin}\n" +
                    $"Millésime : {vin.Millesime}\n" +
                    $"Prix : {vin.PrixVin} €\n" +
                    $"Appellation : {appelation.NomAppelation}\n" +
                    $"Type : {typeVin.Nomtype}\n" +
                    $"Fournisseur : {fournisseur.NomFournisseur}";
            }
            else
            {
                textBlockDetails.Text = "Sélectionnez un vin pour voir les détails.";
            }
        }

        private void BtnCreerDemande_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridVins.SelectedItem is Vin vinSelectionne)
            {
                var mainWindow = Application.Current.MainWindow as MainWindow;
                if (mainWindow != null)
                {
                    mainWindow.mainGrid.Children.Clear();
                    mainWindow.mainGrid.Children.Add(new UCAjouterDemande(vinSelectionne));
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un vin pour créer une demande.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
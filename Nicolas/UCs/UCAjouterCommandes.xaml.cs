using Nicolas.Classes;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Nicolas.UCs
{
    public partial class UCAjouterCommandes : UserControl, INotifyPropertyChanged
    {
        private DateTime? dateCommande;
        private decimal? prixTotal;
        private ObservableCollection<DemandeAffichage> demandesDisponibles;
        private ObservableCollection<DemandeAffichage> demandesSelectionnees;

        public event PropertyChangedEventHandler? PropertyChanged;

        public DateTime? DateCommande
        {
            get => dateCommande;
            set
            {
                dateCommande = value;
                OnPropertyChanged(nameof(DateCommande));
            }
        }

        public decimal? PrixTotal
        {
            get => prixTotal;
            set
            {
                prixTotal = value;
                OnPropertyChanged(nameof(PrixTotal));
            }
        }

        public ObservableCollection<DemandeAffichage> DemandesDisponibles
        {
            get => demandesDisponibles;
            set
            {
                demandesDisponibles = value;
                OnPropertyChanged(nameof(DemandesDisponibles));
            }
        }

        public ObservableCollection<DemandeAffichage> DemandesSelectionnees
        {
            get => demandesSelectionnees;
            set
            {
                demandesSelectionnees = value;
                OnPropertyChanged(nameof(DemandesSelectionnees));
            }
        }

        public UCAjouterCommandes()
        {
            InitializeComponent();
            DateCommande = DateTime.Now;
            PrixTotal = 0;
            DemandesDisponibles = new ObservableCollection<DemandeAffichage>();
            DemandesSelectionnees = new ObservableCollection<DemandeAffichage>();
            DataContext = this;
            ChargerDemandes();
        }

        private void ChargerDemandes()
        {
            DemandesDisponibles.Clear(); // Vider avant de charger
            var toutesDemandes = new Demande().FindAll();
            foreach (var demande in toutesDemandes.Where(d => d.NumCommande == null))
            {
                var vin = new Vin(demande.NumVin, 0, 0, 0, null, null, null, null);
                vin.Read();

                DemandesDisponibles.Add(new DemandeAffichage
                {
                    NumeroDemande = demande.NumDemande,
                    NomVin = vin.Nomvin,
                    QuantiteDemande = demande.QuantiteDemande,
                    PrixVin = vin.PrixVin,
                    DateDemande = demande.DateDemande?.ToString("dd/MM/yyyy"),
                    NumeroVin = demande.NumVin
                });
            }
        }

        private void RecalculerTotal()
        {
            PrixTotal = DemandesSelectionnees.Sum(d => (decimal)(d.PrixVin ?? 0) * (d.QuantiteDemande ?? 0));
        }

        private void BtnAjouter_Click(object sender, RoutedEventArgs e)
        {
            var demande = dgDemandesDisponibles.SelectedItem as DemandeAffichage;
            if (demande != null)
            {
                DemandesDisponibles.Remove(demande);
                DemandesSelectionnees.Add(demande);
                RecalculerTotal();
            }
        }

        private void BtnRetirer_Click(object sender, RoutedEventArgs e)
        {
            var demande = dgDemandesSelectionnees.SelectedItem as DemandeAffichage;
            if (demande != null)
            {
                DemandesSelectionnees.Remove(demande);
                DemandesDisponibles.Add(demande);
                RecalculerTotal();
            }
        }

        private void BtnCreer_Click(object sender, RoutedEventArgs e)
        {
            if (!DemandesSelectionnees.Any())
            {
                MessageBox.Show("Veuillez sélectionner au moins une demande.", "Erreur");
                return;
            }

            try
            {
                // Création de la commande avec l'employé connecté
                var commande = new Commande(0, 2, DateCommande, false, PrixTotal); // Remplacer 1 par l'ID de l'employé connecté
                var result = commande.Create();
                if (result <= 0)
                {
                    MessageBox.Show("Erreur lors de la création de la commande.", "Erreur");
                    return;
                }

                // Association des demandes
                foreach (var demandeAffichage in DemandesSelectionnees)
                {
                    var demande = new Demande();
                    demande.NumDemande = demandeAffichage.NumeroDemande;
                    demande.Read();
                    demande.NumCommande = commande.NumCommande;
                    demande.Update();
                }

                MessageBox.Show("Commande créée avec succès.", "Succès");
                RetournerALaListe();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la création de la commande : {ex.Message}", "Erreur");
            }
        }

        private void BtnAnnuler_Click(object sender, RoutedEventArgs e)
        {
            RetournerALaListe();
        }

        private void RetournerALaListe()
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            if (mainWindow != null)
            {
                mainWindow.mainGrid.Children.Clear();
                mainWindow.mainGrid.Children.Add(new UCVisualiserCommandes());
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
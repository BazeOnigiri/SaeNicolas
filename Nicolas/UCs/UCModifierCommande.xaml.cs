using Nicolas.Classes;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Nicolas.UCs
{
    public partial class UCModifierCommande : UserControl, INotifyPropertyChanged
    {
        private Commande commande;
        private ObservableCollection<DemandeAffichage> demandesAssociees;
        private ObservableCollection<DemandeAffichage> toutesLesDemandes;
        private decimal prixInitial;

        private int numeroCommande;
        private DateTime? dateCommande;
        private decimal? prixTotal;
        private decimal? nouveauTotal;
        private string differenceMessage;

        public event PropertyChangedEventHandler PropertyChanged;

        public int NumeroCommande
        {
            get => numeroCommande;
            set
            {
                numeroCommande = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NumeroCommande)));
            }
        }

        public DateTime? DateCommande
        {
            get => dateCommande;
            set
            {
                dateCommande = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DateCommande)));
            }
        }

        public decimal? PrixTotal
        {
            get => prixTotal;
            set
            {
                prixTotal = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PrixTotal)));
            }
        }

        public decimal? NouveauTotal
        {
            get => nouveauTotal;
            set
            {
                nouveauTotal = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NouveauTotal)));
            }
        }

        public string DifferenceMessage
        {
            get => differenceMessage;
            set
            {
                differenceMessage = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DifferenceMessage)));
            }
        }

        public UCModifierCommande(Commande commandeSelectionnee)
        {
            InitializeComponent();
            if (commandeSelectionnee == null)
            {
                throw new ArgumentNullException(nameof(commandeSelectionnee), "Commande selectionnée cannot be null.");
            }
            commande = commandeSelectionnee;
            prixInitial = commandeSelectionnee.PrixTotal ?? 0;

            InitialiserDonnees();
            DataContext = this;
        }

        private void InitialiserDonnees()
        {
            NumeroCommande = commande.NumCommande;
            DateCommande = commande.DateCommande;
            PrixTotal = commande.PrixTotal;
            NouveauTotal = PrixTotal;
            DifferenceMessage = "";

            var toutesDemandes = new Demande().FindAll();
            demandesAssociees = new ObservableCollection<DemandeAffichage>();
            toutesLesDemandes = new ObservableCollection<DemandeAffichage>();

            foreach (var demande in toutesDemandes)
            {
                var vin = new Vin(demande.NumVin, 0, 0, 0, null, null, null, null);
                vin.Read();

                var demandeAffichage = new DemandeAffichage
                {
                    NumeroDemande = demande.NumDemande,
                    NomVin = vin.Nomvin,
                    QuantiteDemande = demande.QuantiteDemande,
                    PrixVin = vin.PrixVin,
                    DateDemande = demande.DateDemande?.ToString("dd/MM/yyyy"),
                    NumeroVin = demande.NumVin
                };

                if (demande.NumCommande == commande.NumCommande)
                    demandesAssociees.Add(demandeAffichage);
                else if (demande.NumCommande == null)
                    toutesLesDemandes.Add(demandeAffichage);
            }

            dgDemandesAssociees.ItemsSource = demandesAssociees;
            dgToutesDemandes.ItemsSource = toutesLesDemandes;
        }

        private void BtnAjouterDemande_Click(object sender, RoutedEventArgs e)
        {
            if (dgToutesDemandes.SelectedItem is DemandeAffichage demandeAffichage)
            {
                toutesLesDemandes.Remove(demandeAffichage);
                demandesAssociees.Add(demandeAffichage);
                RecalculerTotal();
            }
        }

        private void BtnRetirerDemande_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is DemandeAffichage demandeAffichage)
            {
                demandesAssociees.Remove(demandeAffichage);
                toutesLesDemandes.Add(demandeAffichage);
                RecalculerTotal();
            }
        }

        public void RecalculerTotal()
        {
            decimal nouveauTotal = 0;
            foreach (var demande in demandesAssociees)
            {
                nouveauTotal += (decimal)(demande.PrixVin ?? 0) * (demande.QuantiteDemande ?? 0);
            }

            NouveauTotal = nouveauTotal;
            var difference = nouveauTotal - prixInitial;
            if (difference != 0)
                DifferenceMessage = $"({(difference > 0 ? "+" : "")}{difference:N2} €)";
            else
                DifferenceMessage = "(Pas de changement)";
        }

        private void BtnValider_Click(object sender, RoutedEventArgs e)
        {
            if (demandesAssociees.Count == 0)
            {
                if (MessageBox.Show("Aucune demande associée. La commande sera supprimée. Continuer ?",
                    "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    commande.Delete();
                    RetournerALaListe();
                }
                return;
            }

            // Mettre à jour toutes les demandes
            var toutesDemandesDB = new Demande().FindAll();

            // Réinitialiser les demandes qui ne sont plus associées
            foreach (var demande in toutesDemandesDB.Where(d => d.NumCommande == commande.NumCommande))
            {
                demande.NumCommande = null;
                demande.Update();
            }

            // Associer les demandes sélectionnées
            foreach (var demandeAffichage in demandesAssociees)
            {
                var demande = toutesDemandesDB.First(d => d.NumDemande == demandeAffichage.NumeroDemande);
                demande.NumCommande = commande.NumCommande;
                demande.Update();
            }

            commande.PrixTotal = NouveauTotal;
            commande.Update();
            RetournerALaListe();
        }

        private void BtnValiderCommande_Click(object sender, RoutedEventArgs e)
        {
            if (demandesAssociees.Count == 0)
            {
                MessageBox.Show("Impossible de valider une commande sans demande associée.", "Erreur");
                return;
            }

            if (MessageBox.Show("Voulez-vous valider cette commande ? Une fois validée, elle ne pourra plus être modifiée.",
                "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                // Sauvegarder d'abord les modifications
                commande.PrixTotal = NouveauTotal;
                commande.Valider = true;
                commande.Update();
                RetournerALaListe();
            }
        }

        private void BtnSupprimer_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Voulez-vous supprimer cette commande ?", "Confirmation",
                MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                var toutesDemandesDB = new Demande().FindAll();
                foreach (var demande in toutesDemandesDB.Where(d => d.NumCommande == commande.NumCommande))
                {
                    demande.NumCommande = null;
                    demande.Update();
                }
                commande.Delete();
                RetournerALaListe();
            }
        }

        private void BtnRetour_Click(object sender, RoutedEventArgs e)
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
    }

    public class DemandeAffichage
    {
        public int NumeroDemande { get; set; }
        public string NomVin { get; set; }
        public int? QuantiteDemande { get; set; }
        public double? PrixVin { get; set; }
        public string DateDemande { get; set; }
        public int NumeroVin { get; set; }
    }
}
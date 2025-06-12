using Nicolas.Classes;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Nicolas.UCs
{
    public partial class UCModifierCommande : UserControl
    {
        private ModifCommandeVM vm;

        public UCModifierCommande(Commande commande)
        {
            InitializeComponent();
            vm = new ModifCommandeVM(commande);
            DataContext = vm;
        }

        private void BtnAjouterDemande_Click(object sender, RoutedEventArgs e)
        {
            if (dgToutesDemandes.SelectedItem is DemandeAffichage demande)
                vm.AjouterDemande(demande);
        }

        private void BtnRetirerDemande_Click(object sender, RoutedEventArgs e)
        {
            if (dgDemandesAssociees.SelectedItem is DemandeAffichage demande)
                vm.RetirerDemande(demande);
        }

        private void BtnValider_Click(object sender, RoutedEventArgs e)
        {
            vm.Valider();
        }

        private void BtnSupprimer_Click(object sender, RoutedEventArgs e)
        {
            vm.Supprimer();
        }

        private void BtnRetour_Click(object sender, RoutedEventArgs e)
        {
            vm.Retour();
        }
    }

    public class DemandeAffichage
    {
        public int NumeroDemande { get; set; }
        public string NomVin { get; set; }
        public int? Quantite { get; set; }
        public double? PrixVin { get; set; }
        public string DateDemande { get; set; }
        public int? QuantiteDemande { get; set; }
        public int NumeroVin { get; set; }
    }

    public class ModifCommandeVM : INotifyPropertyChanged
    {
        public int NumeroCommande { get; }
        public DateTime? DateCommande { get; }
        public decimal? PrixTotal { get; }
        public ObservableCollection<DemandeAffichage> DemandesAssociees { get; }
        public ObservableCollection<DemandeAffichage> ToutesLesDemandes { get; }

        private Commande commande;

        public ModifCommandeVM(Commande commande)
        {
            this.commande = commande;
            NumeroCommande = commande.NumCommande;
            DateCommande = commande.DateCommande;
            PrixTotal = commande.PrixTotal;

            var toutesDemandes = new Demande().FindAll();
            ToutesLesDemandes = new ObservableCollection<DemandeAffichage>(
                toutesDemandes.Select(d =>
                {
                    var vin = new Vin(d.NumVin, 0, 0, 0, null, null, null, null);
                    vin.Read();
                    return new DemandeAffichage
                    {
                        NumeroDemande = d.NumDemande,
                        NomVin = vin.Nomvin,
                        Quantite = d.QuantiteDemande,
                        QuantiteDemande = d.QuantiteDemande,
                        PrixVin = vin.PrixVin,
                        DateDemande = d.DateDemande?.ToString("dd/MM/yyyy"),
                        NumeroVin = d.NumVin
                    };
                })
            );

            DemandesAssociees = new ObservableCollection<DemandeAffichage>(
                toutesDemandes.Where(d => d.NumCommande == commande.NumCommande).Select(d =>
                {
                    var vin = new Vin(d.NumVin, 0, 0, 0, null, null, null, null);
                    vin.Read();
                    return new DemandeAffichage
                    {
                        NumeroDemande = d.NumDemande,
                        NomVin = vin.Nomvin,
                        Quantite = d.QuantiteDemande,
                        PrixVin = vin.PrixVin,
                        DateDemande = d.DateDemande?.ToString("dd/MM/yyyy"),
                        NumeroVin = d.NumVin
                    };
                })
            );
        }

        public void AjouterDemande(DemandeAffichage demande)
        {
            if (DemandesAssociees.Any(d => d.NumeroDemande == demande.NumeroDemande)) return;
            var demandeToAdd = ToutesLesDemandes.FirstOrDefault(d => d.NumeroDemande == demande.NumeroDemande);
            if (demandeToAdd != null)
                DemandesAssociees.Add(demandeToAdd);
            OnPropertyChanged(nameof(DemandesAssociees));
        }

        public void RetirerDemande(DemandeAffichage demande)
        {
            var toRemove = DemandesAssociees.FirstOrDefault(d => d.NumeroDemande == demande.NumeroDemande);
            if (toRemove != null)
                DemandesAssociees.Remove(toRemove);
            OnPropertyChanged(nameof(DemandesAssociees));
        }

        public void Valider()
        {
            var toutesDemandes = new Demande().FindAll();

            foreach (var d in toutesDemandes.Where(d => d.NumCommande == commande.NumCommande))
            {
                if (!DemandesAssociees.Any(da => da.NumeroDemande == d.NumDemande))
                {
                    d.NumCommande = null;
                    d.Update();
                }
            }
            foreach (var d in DemandesAssociees)
            {
                var demande = toutesDemandes.FirstOrDefault(td => td.NumDemande == d.NumeroDemande);
                if (demande != null && demande.NumCommande != commande.NumCommande)
                {
                    demande.NumCommande = commande.NumCommande;
                    demande.Update();
                }
            }

            if (DemandesAssociees.Count == 0)
            {
                commande.Delete();
            }

            Retour();
        }

        public void Supprimer()
        {
            var toutesDemandes = new Demande().FindAll();
            foreach (var d in toutesDemandes.Where(d => d.NumCommande == commande.NumCommande))
            {
                d.NumCommande = null;
                d.Update();
            }
            commande.Delete();
            Retour();
        }

        public void Retour()
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            if (mainWindow != null)
            {
                mainWindow.mainGrid.Children.Clear();
                mainWindow.mainGrid.Children.Add(new UCVisualiserCommandes());
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
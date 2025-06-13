using Nicolas.Classes;
using Nicolas.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Nicolas.UCs
{
    public partial class UCAjouterDemande : UserControl
    {
        private readonly Vin vinSelectionne;
        private Client clientSelectionne;
        private List<Client> tousLesClients;

        public UCAjouterDemande(Vin vinSelectionne)
        {
            InitializeComponent();
            this.vinSelectionne = vinSelectionne;

            // Charger la liste des clients
            var client = new Client();
            tousLesClients = client.FindAll();

            InitialiserControles();
        }

        private void InitialiserControles()
        {
            txtVinSelectionne.Text = $"{vinSelectionne.Nomvin} ({vinSelectionne.Millesime})";
            dpDate.SelectedDate = DateTime.Now;
            txtQuantite.Focus();
        }

        private void txtClient_TextChanged(object sender, TextChangedEventArgs e)
        {
            string recherche = txtClient.Text.ToLower();

            if (string.IsNullOrWhiteSpace(recherche))
            {
                listSuggestions.Visibility = Visibility.Collapsed;
                return;
            }

            var suggestions = tousLesClients
                .Where(c => (c.NomClient?.ToLower().Contains(recherche) ?? false) ||
                            (c.PrenomClient?.ToLower().Contains(recherche) ?? false))
                .Take(3)
                .ToList();

            if (suggestions.Any())
            {
                listSuggestions.Items.Clear();
                foreach (var client in suggestions)
                {
                    listSuggestions.Items.Add($"{client.NumClient} - {client.NomClient} {client.PrenomClient}");
                }
                listSuggestions.Visibility = Visibility.Visible;
            }
            else
            {
                listSuggestions.Visibility = Visibility.Collapsed;
            }
        }

        private void listSuggestions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listSuggestions.SelectedItem != null)
            {
                txtClient.Text = listSuggestions.SelectedItem.ToString();
                listSuggestions.Visibility = Visibility.Collapsed;
            }
        }

        private void butModifClient_Click(object sender, RoutedEventArgs e)
        {
            if (listSuggestions.SelectedItem == null)
            {
                MessageBox.Show("Veuillez d'abord sélectionner un client.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            string selectedClientText = listSuggestions.SelectedItem.ToString();
            int clientId = int.Parse(selectedClientText.Split('-')[0].Trim());

            // Rechercher le client complet dans la liste
            Client clientSelectionne = tousLesClients.FirstOrDefault(c => c.NumClient == clientId);
            
            if (clientSelectionne != null)
            {
                // Ouvrir la fenêtre FicheClient avec le client sélectionné
                FicheClient ficheClient = new FicheClient(clientSelectionne);
                ficheClient.ShowDialog(); // Utilisation de ShowDialog pour bloquer l'interaction avec la fenêtre parent
                
                // Rafraîchir la liste des clients après la modification
                var client = new Client();
                tousLesClients = client.FindAll();
                
                // Mettre à jour le texte avec les nouvelles informations
                Client clientMisAJour = tousLesClients.FirstOrDefault(c => c.NumClient == clientId);
                if (clientMisAJour != null)
                {
                    txtClient.Text = $"{clientMisAJour.NumClient} - {clientMisAJour.NomClient} {clientMisAJour.PrenomClient}";
                }
            }
        }

        private void butNouveauClient_Click(object sender, RoutedEventArgs e)
        {
            FicheClient ficheClient = new FicheClient();
            if (ficheClient.ShowDialog() == true) // ShowDialog retournera true si un client a été créé
            {
                // Rafraîchir la liste des clients
                var client = new Client();
                tousLesClients = client.FindAll();

                // Récupérer le dernier client créé (celui avec le plus grand NumClient)
                var dernierClient = tousLesClients.OrderByDescending(c => c.NumClient).FirstOrDefault();
                if (dernierClient != null)
                {
                    // Mettre à jour le texte avec le nouveau client
                    txtClient.Text = $"{dernierClient.NumClient} - {dernierClient.NomClient} {dernierClient.PrenomClient}";
                    
                    // Effacer les suggestions pour éviter la confusion
                    listSuggestions.Items.Clear();
                    listSuggestions.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void butValiderDemande_Click(object sender, RoutedEventArgs e)
        {
            // Validation des champs
            if (!ValiderChamps())
                return;

            try
            {
                // Création de la demande avec numEmploye en dur = 1
                var demande = new Demande(
                    0, // numDemande (sera généré par la BD)
                    vinSelectionne.NumVin,
                    1, // numEmploye en dur pour l'instant
                    null, // numCommande (null car nouvelle demande)
                    int.Parse(txtClient.Text.Split('-')[0].Trim()), // Extrait juste le numéro du client
                    dpDate.SelectedDate,
                    int.Parse(txtQuantite.Text),
                    "En attente" // État initial de la demande
                );

                // Sauvegarde de la demande
                if (demande.Create() > 0)
                {
                    MessageBox.Show("Demande créée avec succès!", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                    RetournerVersRecherche();
                }
                else
                {
                    MessageBox.Show("Erreur lors de la création de la demande.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur est survenue : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public bool ValiderChamps()
        {
            if (string.IsNullOrEmpty(txtQuantite.Text))
            {
                MessageBox.Show("Veuillez entrer une quantité.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtQuantite.Focus();
                return false;
            }

            if (!int.TryParse(txtQuantite.Text, out int quantite) || quantite <= 0)
            {
                MessageBox.Show("La quantité doit être un nombre positif.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtQuantite.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(txtClient.Text))
            {
                MessageBox.Show("Veuillez sélectionner un client.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtClient.Focus();
                return false;
            }

            if (!int.TryParse(txtClient.Text.Split('-')[0].Trim(), out int _))
            {
                MessageBox.Show("Client invalide. Utilisez le bouton 'Modifier Client' pour sélectionner un client.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!dpDate.SelectedDate.HasValue)
            {
                MessageBox.Show("Veuillez sélectionner une date.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                dpDate.Focus();
                return false;
            }

            return true;
        }

        private void butAnnuler_Click(object sender, RoutedEventArgs e)
        {
            RetournerVersRecherche();
        }

        private void RetournerVersRecherche()
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            if (mainWindow != null)
            {
                mainWindow.mainGrid.Children.Clear();
                mainWindow.mainGrid.Children.Add(new UCRechercherVin());
            }
        }
    }
}
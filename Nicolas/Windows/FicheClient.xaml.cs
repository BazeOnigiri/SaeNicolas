using Nicolas.Classes;
using System;
using System.Windows;

namespace Nicolas.Windows
{
    public partial class FicheClient : Window
    {
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Mail { get; set; }

        public FicheClient()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void butValider_Click(object sender, RoutedEventArgs e)
        {
            if (!ValiderChamps())
                return;

            try
            {
                Client client = new Client(0, Nom, Prenom, Mail);
                int resultat = client.Create();

                if (resultat > 0)
                {
                    MessageBox.Show("Client créé avec succès!", "Succès",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Erreur lors de la création du client.", "Erreur",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur est survenue : {ex.Message}", "Erreur",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool ValiderChamps()
        {
            // Validation du nom
            if (string.IsNullOrWhiteSpace(txtNom.Text))
            {
                MessageBox.Show("Le nom est obligatoire.", "Validation",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                txtNom.Focus();
                return false;
            }

            // Validation du prénom
            if (string.IsNullOrWhiteSpace(txtPrenom.Text))
            {
                MessageBox.Show("Le prénom est obligatoire.", "Validation",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                txtPrenom.Focus();
                return false;
            }

            // Validation basique du format email
            if (!string.IsNullOrWhiteSpace(txtMail.Text) &&
                !txtMail.Text.Contains("@"))
            {
                MessageBox.Show("Format d'email invalide.", "Validation",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                txtMail.Focus();
                return false;
            }

            // Mise à jour des propriétés avec les valeurs des TextBox
            Nom = txtNom.Text.Trim();
            Prenom = txtPrenom.Text.Trim();
            Mail = string.IsNullOrWhiteSpace(txtMail.Text) ? null : txtMail.Text.Trim();

            return true;
        }

        private void butAnnuler_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Ne rien faire, laisser la fenêtre se fermer
        }
    }
}
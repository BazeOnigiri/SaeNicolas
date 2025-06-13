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
        private int? numClientExistant;

        // Constructeur par défaut pour nouveau client
        public FicheClient()
        {
            InitializeComponent();
            DataContext = this;
        }

        // Nouveau constructeur pour client existant
        public FicheClient(Client client)
        {
            InitializeComponent();

            // Sauvegarde du numéro du client existant
            numClientExistant = client.NumClient;

            // Initialise les propriétés avec les données du client
            Nom = client.NomClient;
            Prenom = client.PrenomClient;
            Mail = client.MailClient;

            DataContext = this;
        }

        private void butValider_Click(object sender, RoutedEventArgs e)
        {
            if (!ValiderChamps())
                return;

            try
            {
                Client client = new Client(numClientExistant ?? 0, Nom, Prenom, Mail);

                int resultat;
                if (numClientExistant.HasValue)
                {
                    // Mise à jour d'un client existant
                    resultat = client.Update();
                    if (resultat > 0)
                    {
                        MessageBox.Show("Client mis à jour avec succès!", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                        DialogResult = true;  // Indique que l'opération a réussi
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Erreur lors de la mise à jour du client.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    // Création d'un nouveau client
                    resultat = client.Create();
                    if (resultat > 0)
                    {
                        MessageBox.Show("Client créé avec succès!", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                        DialogResult = true;  // Indique que l'opération a réussi
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Erreur lors de la création du client.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur est survenue : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
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
            DialogResult = false;  // Indique que l'opération a été annulée
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Si DialogResult n'a pas été défini, on considère que c'est une annulation
            if (!this.DialogResult.HasValue)
            {
                this.DialogResult = false;
            }
        }
    }
}
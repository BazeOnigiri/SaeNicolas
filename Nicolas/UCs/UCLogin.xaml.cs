using Nicolas.Classes;
using Npgsql;
using System;
using System.Collections.Generic;
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
    /// Logique d'interaction pour UCLogin.xaml
    /// </summary>
    public partial class UCLogin : UserControl
    {
        public string Login { get; set; }
        public string Mdp { get; set; }

        public UCLogin()
        {
            InitializeComponent();
        }

        private void butConnexion_Click(object sender, RoutedEventArgs e)
        {
            // Récupérer les valeurs depuis les champs de saisie
            Login = txtIdentifiant.Text;
            Mdp = txtMdp.Text; // Supposant que vous avez un PasswordBox nommé txtMotDePasse

            try
            {
                // D'abord tester la connexion avec les credentials fournis
                string testConnectionString = $"Host=srv-peda-new;Port=5433;Username={Login};Password={Mdp};Database=kiehlt_sea201;Options='-c search_path=kiehlt'";

                using (var testConnection = new NpgsqlConnection(testConnectionString))
                {
                    testConnection.Open(); // Test de connexion
                    testConnection.Close();
                }

                // Si la connexion fonctionne, initialiser DataAccess
                DataAccess.Initialize(Login, Mdp);

                // Maintenant récupérer la liste des employés
                Employe employe = new Employe();
                List<Employe> employes = employe.FindAll();

                // Rechercher l'employé par login
                Employe employeConnecte = employes.FirstOrDefault(emp => emp.Login == Login);

                if (employeConnecte != null)
                {
                    int numRole = employeConnecte.NumRole;

                    // Connexion réussie
                    MainWindow mainWindow = (MainWindow)Window.GetWindow(this);
                    if (mainWindow != null)
                    {
                        mainWindow.mainGrid.Children.Clear();

                        UserControl userControlToAdd = null;

                        switch (numRole)
                        {
                            case 1:
                                userControlToAdd = new UCRechercherVin();
                                break;
                            case 2:
                                userControlToAdd = new UCVisualiserCommandes();
                                break;
                            default:
                                txtErreur.Text = "Rôle non reconnu pour cet utilisateur";
                                return;
                        }

                        if (userControlToAdd != null)
                        {
                            // Si vous avez besoin de passer l'employé connecté aux UserControls :
                            // if (userControlToAdd is UCRechercherVin ucRechercherVin)
                            //     ucRechercherVin.EmployeConnecte = employeConnecte;
                            // if (userControlToAdd is UCVisualiserCommandes ucVisualiserCommandes)
                            //     ucVisualiserCommandes.EmployeConnecte = employeConnecte;

                            mainWindow.mainGrid.Children.Add(userControlToAdd);
                        }
                    }
                }
                else
                {
                    txtErreur.Text = "Identifiant incorrect ! Veuillez réessayer";
                }
            }
            catch (Exception ex)
            {
                // Gérer les erreurs de connexion à la base de données
                txtErreur.Text = "Erreur de connexion à la base de données. Vérifiez vos identifiants.";
                LogError.Log(ex, "Erreur lors de la connexion avec les credentials utilisateur");
            }
        }
    }
}
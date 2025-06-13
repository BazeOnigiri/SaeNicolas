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
        public UCLogin()
        {
            InitializeComponent();
        }

        private void butConnexion_Click(object sender, RoutedEventArgs e)
        {
            using (NpgsqlCommand cmdSelect = new NpgsqlCommand("SELECT EXISTS (SELECT 1 FROM employe WHERE login = @login)"))
            {
                Employe employe = new Employe();
                List<Employe> employes = employe.FindAll();

                // Rechercher l'employé par login
                Employe employeConnecte = employes.FirstOrDefault(emp => emp.Login == txtIdentifiant.Text);

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
            
        }
    }
}

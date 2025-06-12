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
            using (NpgsqlCommand cmdSelect = new NpgsqlCommand($"SELECT EXISTS (SELECT 1 FROM employe WHERE login = '{txtIdentifiant.Text}')"))
            {
                object login = DataAccess.Instance.ExecuteSelectUneValeur(cmdSelect);
                bool res = Convert.ToBoolean(login);
                if (res == true)
                {
                    MainWindow mainWindow = (MainWindow)Window.GetWindow(this);
                    if (mainWindow != null)
                    {
                        mainWindow.Content = new UCVisualiserCommande();
                    }
                }
                else
                    txtErreur.Text = "Identifiant incorrect ! Veuillez réessayer";
            }
        }
    }
}

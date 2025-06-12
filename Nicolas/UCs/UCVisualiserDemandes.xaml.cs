using Nicolas.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Logique d'interaction pour UCVisualiserDemandes.xaml
    /// </summary>
    public partial class UCVisualiserDemandes : UserControl
    {
        
        public UCVisualiserDemandes()
        {
            
            InitializeComponent();
            ChargeData();

        }

        public void ChargeData()
        {
            try
            {
                ObservableCollection<Demande> lesDemandes = new ObservableCollection<Demande>(new Demande().FindAll());
                dgDemande.ItemsSource = lesDemandes;
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Problème lors de récupération des données, veuillez consulter votre admin");

                Application.Current.Shutdown();
            }
        }
       
    }

}

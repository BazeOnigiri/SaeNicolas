using Nicolas.Classes;
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
using System.Windows.Shapes;

namespace Nicolas.Windows
{
    /// <summary>
    /// Logique d'interaction pour FicheClient.xaml
    /// </summary>
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
            Client client = new Client(0,Nom, Prenom, Mail);
            client.Create();
        }
    }
}

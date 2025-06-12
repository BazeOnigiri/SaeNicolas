using Nicolas.Classes;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// Logique d'interaction pour UCVisualiserCommandes.xaml
    /// </summary>
    public partial class UCVisualiserCommandes : UserControl
    {
        public UCVisualiserCommandes()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void btnFiltre_Click(object sender, RoutedEventArgs e)
        {
            popupFiltre.IsOpen = !popupFiltre.IsOpen;
        }

        private void btnAppliquerFiltre_Click(object sender, RoutedEventArgs e)
        {
            popupFiltre.IsOpen = false;
        }

        private string TryGetValue(DataRowView row, string nomColonne)
        {
            if (row.DataView.Table.Columns.Contains(nomColonne) && row[nomColonne] != DBNull.Value)
                return row[nomColonne].ToString();
            return "";
        }

        private void BtnSupprimer_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridCommandes.SelectedItem is DataRowView row)
            {
                string numCommande = TryGetValue(row, "NumCommande");
                if (int.TryParse(numCommande, out int id))
                {
                    if (MessageBox.Show("Confirmer la suppression de la commande ?", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        try
                        {
                            string sql = "DELETE FROM Commande WHERE NumCommande = @id";
                            using (NpgsqlCommand cmd = new NpgsqlCommand(sql))
                            {
                                cmd.Parameters.AddWithValue("@id", id);
                                DataAccess.Instance.ExecuteSet(cmd);
                            }
                            textBlockDetails.Text = "Commande supprimée.";
                            itemsVins.ItemsSource = null;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Erreur lors de la suppression : " + ex.Message);
                        }
                    }
                }
            }
        }
    }
}

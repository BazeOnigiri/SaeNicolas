using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nicolas.Classes
{
    internal class DetailCommande
    {
        private int numCommande;
        private int numVin;
        private int? quantite;
        private decimal? prix;

        public DetailCommande(int numCommande, int numVin, int? quantite, decimal? prix)
        {
            NumCommande = numCommande;
            NumVin = numVin;
            Quantite = quantite;
            Prix = prix;
        }

        public int NumCommande
        {
            get
            {
                return numCommande;
            }

            set
            {
                numCommande = value;
            }
        }

        public int NumVin
        {
            get
            {
                return numVin;
            }

            set
            {
                numVin = value;
            }
        }

        public int? Quantite
        {
            get
            {
                return quantite;
            }

            set
            {
                quantite = value;
            }
        }

        public decimal? Prix
        {
            get
            {
                return prix;
            }

            set
            {
                if (value != null && value < 0)
                    prix= Math.Round((decimal)value, 2);
            }
        }

        public List<DetailCommande> FindAll()
        {
            List<DetailCommande> lesDetailCommandes = new List<DetailCommande>();
            using (NpgsqlCommand cmdSelect = new NpgsqlCommand("select * from Commande;"))
            {
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);
                foreach (DataRow dr in dt.Rows)
                    lesDetailCommandes.Add(new DetailCommande((Int32)dr["numCommande"], (Int32)dr["numVin"], 
                        (Int32)dr["quantite"], (Decimal?)dr["prix"]));
            }
            return lesDetailCommandes;
        }
    }
}

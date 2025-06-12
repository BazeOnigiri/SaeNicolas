using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nicolas.Classes
{
    internal class Commande
    {
        private int numCommande;
        private int numEmploye;
        private DateTime? dateCommande;
        private bool? valider;
        private decimal? prixTotal;

        public Commande(int numCommande, int numEmploye, DateTime? dateCommande, bool? valider, decimal? prixTotal)
        {
            NumCommande = numCommande;
            NumEmploye = numEmploye;
            DateCommande = dateCommande;
            Valider = valider;
            PrixTotal = prixTotal;
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

        public int NumEmploye
        {
            get
            {
                return numEmploye;
            }

            set
            {
                numEmploye = value;
            }
        }

        public DateTime? DateCommande
        {
            get
            {
                return dateCommande;
            }

            set
            {
                dateCommande = value;
            }
        }

        public bool? Valider
        {
            get
            {
                return valider;
            }

            set
            {
                valider = value;
            }
        }

        public decimal? PrixTotal
        {
            get
            {
                return prixTotal;
            }

            set
            {
                if (value != null && value < 0)
                    prixTotal = Math.Round((decimal)value, 2);
            }
        }

        public List<Commande> FindAll()
        {
            List<Commande> lesCommandes = new List<Commande>();
            using (NpgsqlCommand cmdSelect = new NpgsqlCommand("select * from Commande;"))
            {
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);
                foreach (DataRow dr in dt.Rows)
                {
                    int numCommande = dr["numCommande"] != DBNull.Value ? Convert.ToInt32(dr["numCommande"]) : 0;
                    int numEmploye = dr["numEmploye"] != DBNull.Value ? Convert.ToInt32(dr["numEmploye"]) : 0;
                    DateTime? dateCommande = dr["dateCommande"] != DBNull.Value ? Convert.ToDateTime(dr["dateCommande"]) : (DateTime?)null;
                    bool? valider = dr["valider"] != DBNull.Value ? Convert.ToBoolean(dr["valider"]) : (bool?)null;
                    decimal? prixTotal = dr["prixTotal"] != DBNull.Value ? Convert.ToDecimal(dr["prixTotal"]) : (decimal?)null;

                    lesCommandes.Add(new Commande(numCommande, numEmploye, dateCommande, valider, prixTotal));
                }
            }
            return lesCommandes;
        }
    }
}

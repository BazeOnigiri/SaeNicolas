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
                    lesCommandes.Add(new Commande((Int32)dr["numCommande"], (Int32)dr["numEmploye"],
                   (DateTime?)dr["dateCommande"], (bool?)dr["valider"], (Decimal?)dr["prixTotal"]));
            }
            return lesCommandes;
        }
    }
}

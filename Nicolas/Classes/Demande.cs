using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nicolas.Classes
{
    
    internal class Demande
    {
        private int numDemande;
        private int numVin;
        private int numEmploye;
        private int numCommande;
        private int numClient;
        private DateTime? dateDemande;
        private int quantiteDemande;
        private string etatDemande;

        public Demande(int numDemande, int numVin, int numEmploye,int numCommande, int numClient, DateTime? dateDemande, int quantiteDemande, string etatDemande)
        {
            NumDemande = numDemande;
            NumVin = numVin;
            NumEmploye = numEmploye;
            NumCommande = numCommande;
            NumClient = numClient;
            DateDemande = dateDemande;
            QuantiteDemande = quantiteDemande;
            EtatDemande = etatDemande;
        }

        public int NumDemande
        {
            get
            {
                return numDemande;
            }

            set
            {
                numDemande = value;
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

        public int NumClient
        {
            get
            {
                return numClient;
            }

            set
            {
                numClient = value;
            }
        }

        public DateTime? DateDemande
        {
            get
            {
                return dateDemande;
            }

            set
            {
                dateDemande = value;
            }
        }

        public int QuantiteDemande
        {
            get
            {
                return quantiteDemande;
            }

            set
            {
                quantiteDemande = value;
            }
        }

        public string EtatDemande
        {
            get
            {
                return etatDemande;
            }

            set
            {
                etatDemande = value;
            }
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

        public List<Demande> FindAll()
        {
            List<Demande> lesDemandes = new List<Demande>();
            using (NpgsqlCommand cmdSelect = new NpgsqlCommand("select * from demande ;"))
            {
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);
                foreach (DataRow dr in dt.Rows)
                    lesDemandes.Add(new Demande((Int32)dr["numDemande"], (Int32)dr["numVin"],
                   (Int32)dr["numEmploye"], (Int32)dr["numCommande"], (Int32)dr["numClient"], (DateTime?)dr["dateDemande"], (Int32)dr["quantiteDemande"], (String)dr["etatDemande"]));
            }
            return lesDemandes;
        }
    }
}

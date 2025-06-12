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
        private int? numCommande;
        private int numClient;
        private DateTime? dateDemande;
        private int? quantiteDemande;
        private string? accepter;

        public Demande()
        {
        }

        public Demande(int numDemande, int numVin, int numEmploye, int? numCommande, int numClient, DateTime? dateDemande, int? quantiteDemande, string? accepter)
        {
            NumDemande = numDemande;
            NumVin = numVin;
            NumEmploye = numEmploye;
            NumCommande = numCommande;
            NumClient = numClient;
            DateDemande = dateDemande;
            QuantiteDemande = quantiteDemande;
            Accepter = accepter;
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

        public int? QuantiteDemande
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

        public string? Accepter
        {
            get
            {
                return accepter;
            }

            set
            {
                accepter = value;
            }
        }

        public int? NumCommande
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
                {
                    int numDemande = dr["numDemande"] != DBNull.Value ? Convert.ToInt32(dr["numDemande"]) : 0;
                    int numVin = dr["numVin"] != DBNull.Value ? Convert.ToInt32(dr["numVin"]) : 0;
                    int numEmploye = dr["numEmploye"] != DBNull.Value ? Convert.ToInt32(dr["numEmploye"]) : 0;
                    int? numCommande = dr["numCommande"] != DBNull.Value ? Convert.ToInt32(dr["numCommande"]) : (int?)null;
                    int numClient = dr["numClient"] != DBNull.Value ? Convert.ToInt32(dr["numClient"]) : 0;
                    DateTime? dateDemande = dr["dateDemande"] != DBNull.Value ? Convert.ToDateTime(dr["dateDemande"]) : (DateTime?)null;
                    int? quantiteDemande = dr["quantiteDemande"] != DBNull.Value ? Convert.ToInt32(dr["quantiteDemande"]) : (int?)null;
                    string? etatDemande = dr["etatDemande"] != DBNull.Value ? dr["etatDemande"].ToString() : null;

                    lesDemandes.Add(new Demande(numDemande, numVin, numEmploye, numCommande, numClient, dateDemande, quantiteDemande, etatDemande));
                }
                return lesDemandes;
            }
        }
    }
}

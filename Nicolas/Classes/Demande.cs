using Nicolas.Interfaces;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nicolas.Classes
{
    public class Demande : ICrude<Demande>
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
            get { return numDemande; }
            set { numDemande = value; }
        }

        public int NumVin
        {
            get { return numVin; }
            set { numVin = value; }
        }

        public int NumEmploye
        {
            get { return numEmploye; }
            set { numEmploye = value; }
        }

        public int NumClient
        {
            get { return numClient; }
            set { numClient = value; }
        }

        public DateTime? DateDemande
        {
            get { return dateDemande; }
            set { dateDemande = value; }
        }

        public int? QuantiteDemande
        {
            get { return quantiteDemande; }
            set { quantiteDemande = value; }
        }

        public string? Accepter
        {
            get { return accepter; }
            set { accepter = value; }
        }

        public int? NumCommande
        {
            get { return numCommande; }
            set { numCommande = value; }
        }

        public int Create()
        {
            int nb = 0;
            using (var cmdInsert = new NpgsqlCommand("insert into Demande (numVin, numEmploye, numCommande, numClient, dateDemande, quantiteDemande, accepter) values (@numVin, @numEmploye, @numCommande, @numClient, @dateDemande, @quantiteDemande, @accepter) RETURNING numDemande"))
            {
                cmdInsert.Parameters.AddWithValue("numVin", this.NumVin);
                cmdInsert.Parameters.AddWithValue("numEmploye", this.NumEmploye);
                cmdInsert.Parameters.AddWithValue("numCommande", (object?)this.NumCommande ?? DBNull.Value);
                cmdInsert.Parameters.AddWithValue("numClient", this.NumClient);
                cmdInsert.Parameters.AddWithValue("dateDemande", (object?)this.DateDemande ?? DBNull.Value);
                cmdInsert.Parameters.AddWithValue("quantiteDemande", (object?)this.QuantiteDemande ?? DBNull.Value);
                cmdInsert.Parameters.AddWithValue("accepter", (object?)this.Accepter ?? DBNull.Value);
                nb = DataAccess.Instance.ExecuteInsert(cmdInsert);
            }
            this.NumDemande = nb;
            return nb;
        }

        public void Read()
        {
            using (var cmdSelect = new NpgsqlCommand("select * from Demande where numDemande = @numDemande;"))
            {
                cmdSelect.Parameters.AddWithValue("numDemande", this.NumDemande);
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);
                if (dt.Rows.Count > 0)
                {
                    this.NumVin = dt.Rows[0]["numVin"] != DBNull.Value ? Convert.ToInt32(dt.Rows[0]["numVin"]) : 0;
                    this.NumEmploye = dt.Rows[0]["numEmploye"] != DBNull.Value ? Convert.ToInt32(dt.Rows[0]["numEmploye"]) : 0;
                    this.NumCommande = dt.Rows[0]["numCommande"] != DBNull.Value ? Convert.ToInt32(dt.Rows[0]["numCommande"]) : (int?)null;
                    this.NumClient = dt.Rows[0]["numClient"] != DBNull.Value ? Convert.ToInt32(dt.Rows[0]["numClient"]) : 0;
                    this.DateDemande = dt.Rows[0]["dateDemande"] != DBNull.Value ? Convert.ToDateTime(dt.Rows[0]["dateDemande"]) : (DateTime?)null;
                    this.QuantiteDemande = dt.Rows[0]["quantiteDemande"] != DBNull.Value ? Convert.ToInt32(dt.Rows[0]["quantiteDemande"]) : (int?)null;
                    this.Accepter = dt.Rows[0]["accepter"] != DBNull.Value ? dt.Rows[0]["accepter"].ToString() : null;
                }
            }
        }


        public int Delete()
        {
            using (var cmdDelete = new NpgsqlCommand("delete from Demande where numDemande = @numDemande;"))
            {
                cmdDelete.Parameters.AddWithValue("numDemande", this.NumDemande);
                return DataAccess.Instance.ExecuteSet(cmdDelete);
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
                    string? accepter = dr["accepter"] != DBNull.Value ? dr["accepter"].ToString() : null;

                    lesDemandes.Add(new Demande(numDemande, numVin, numEmploye, numCommande, numClient, dateDemande, quantiteDemande, accepter));
                }
                return lesDemandes;
            }
        }

        public List<Demande> FindBySelection(string criteres)
        {
            // À adapter selon la logique de recherche souhaitée
            throw new NotImplementedException();
        }

        public int Update()
        {
            using (var cmdUpdate = new NpgsqlCommand(
        "update Demande set numVin = @numVin, numEmploye = @numEmploye, numCommande = @numCommande, numClient = @numClient, dateDemande = @dateDemande, quantiteDemande = @quantiteDemande, accepter = @accepter where numDemande = @numDemande;"))
            {
                cmdUpdate.Parameters.AddWithValue("numVin", this.NumVin);
                cmdUpdate.Parameters.AddWithValue("numEmploye", this.NumEmploye);
                cmdUpdate.Parameters.AddWithValue("numCommande", (object?)this.NumCommande ?? DBNull.Value);
                cmdUpdate.Parameters.AddWithValue("numClient", this.NumClient);
                cmdUpdate.Parameters.AddWithValue("dateDemande", (object?)this.DateDemande ?? DBNull.Value);
                cmdUpdate.Parameters.AddWithValue("quantiteDemande", (object?)this.QuantiteDemande ?? DBNull.Value);
                cmdUpdate.Parameters.AddWithValue("accepter", (object?)this.Accepter ?? DBNull.Value);
                cmdUpdate.Parameters.AddWithValue("numDemande", this.NumDemande);
                return DataAccess.Instance.ExecuteSet(cmdUpdate);
            }
        }
    }
}
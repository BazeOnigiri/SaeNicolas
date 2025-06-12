using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nicolas.Interfaces;

namespace Nicolas.Classes
{
    internal class DetailCommande : ICrude<DetailCommande>
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
            get { return numCommande; }
            set { numCommande = value; }
        }

        public int NumVin
        {
            get { return numVin; }
            set { numVin = value; }
        }

        public int? Quantite
        {
            get { return quantite; }
            set { quantite = value; }
        }

        public decimal? Prix
        {
            get { return prix; }
            set
            {
                if (value != null && value < 0)
                    prix = Math.Round((decimal)value, 2);
            }
        }

        public int Create()
        {
            int nb = 0;
            using (var cmdInsert = new NpgsqlCommand("insert into DetailCommande (numCommande, numVin, quantite, prix) values (@numCommande, @numVin, @quantite, @prix) RETURNING numCommande"))
            {
                cmdInsert.Parameters.AddWithValue("numCommande", this.NumCommande);
                cmdInsert.Parameters.AddWithValue("numVin", this.NumVin);
                cmdInsert.Parameters.AddWithValue("quantite", (object?)this.Quantite ?? DBNull.Value);
                cmdInsert.Parameters.AddWithValue("prix", (object?)this.Prix ?? DBNull.Value);
                nb = DataAccess.Instance.ExecuteInsert(cmdInsert);
            }
            this.NumCommande = nb;
            return nb;
        }

        public void Read()
        {
            using (var cmdSelect = new NpgsqlCommand("select * from DetailCommande where numCommande = @numCommande and numVin = @numVin;"))
            {
                cmdSelect.Parameters.AddWithValue("numCommande", this.NumCommande);
                cmdSelect.Parameters.AddWithValue("numVin", this.NumVin);
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);
                if (dt.Rows.Count > 0)
                {
                    this.Quantite = dt.Rows[0]["quantite"] != DBNull.Value ? Convert.ToInt32(dt.Rows[0]["quantite"]) : (int?)null;
                    this.Prix = dt.Rows[0]["prix"] != DBNull.Value ? Convert.ToDecimal(dt.Rows[0]["prix"]) : (decimal?)null;
                }
            }
        }

        public int Update()
        {
            using (var cmdUpdate = new NpgsqlCommand("update DetailCommande set quantite = @quantite, prix = @prix where numCommande = @numCommande and numVin = @numVin;"))
            {
                cmdUpdate.Parameters.AddWithValue("quantite", (object?)this.Quantite ?? DBNull.Value);
                cmdUpdate.Parameters.AddWithValue("prix", (object?)this.Prix ?? DBNull.Value);
                cmdUpdate.Parameters.AddWithValue("numCommande", this.NumCommande);
                cmdUpdate.Parameters.AddWithValue("numVin", this.NumVin);
                return DataAccess.Instance.ExecuteSet(cmdUpdate);
            }
        }

        public int Delete()
        {
            using (var cmdDelete = new NpgsqlCommand("delete from DetailCommande where numCommande = @numCommande and numVin = @numVin;"))
            {
                cmdDelete.Parameters.AddWithValue("numCommande", this.NumCommande);
                cmdDelete.Parameters.AddWithValue("numVin", this.NumVin);
                return DataAccess.Instance.ExecuteSet(cmdDelete);
            }
        }

        public List<DetailCommande> FindAll()
        {
            List<DetailCommande> lesDetailCommandes = new List<DetailCommande>();
            using (NpgsqlCommand cmdSelect = new NpgsqlCommand("select * from DetailCommande;"))
            {
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);
                foreach (DataRow dr in dt.Rows)
                {
                    int numCommande = dr["numCommande"] != DBNull.Value ? Convert.ToInt32(dr["numCommande"]) : 0;
                    int numVin = dr["numVin"] != DBNull.Value ? Convert.ToInt32(dr["numVin"]) : 0;
                    int? quantite = dr["quantite"] != DBNull.Value ? Convert.ToInt32(dr["quantite"]) : (int?)null;
                    decimal? prix = dr["prix"] != DBNull.Value ? Convert.ToDecimal(dr["prix"]) : (decimal?)null;

                    lesDetailCommandes.Add(new DetailCommande(numCommande, numVin, quantite, prix));
                }
            }
            return lesDetailCommandes;
        }

        public List<DetailCommande> FindBySelection(string criteres)
        {
            // À adapter selon la logique de recherche souhaitée
            throw new NotImplementedException();
        }
    }
}
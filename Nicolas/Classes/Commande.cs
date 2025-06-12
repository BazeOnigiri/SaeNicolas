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
    internal class Commande : ICrude<Commande>
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
            get { return numCommande; }
            set { numCommande = value; }
        }

        public int NumEmploye
        {
            get { return numEmploye; }
            set { numEmploye = value; }
        }

        public DateTime? DateCommande
        {
            get { return dateCommande; }
            set { dateCommande = value; }
        }

        public bool? Valider
        {
            get { return valider; }
            set { valider = value; }
        }

        public decimal? PrixTotal
        {
            get { return prixTotal; }
            set
            {
                prixTotal = value;
            }
        }

        public int Create()
        {
            int nb = 0;
            using (var cmdInsert = new NpgsqlCommand("insert into Commande (numEmploye, dateCommande, valider, prixTotal) values (@numEmploye, @dateCommande, @valider, @prixTotal) RETURNING numCommande"))
            {
                cmdInsert.Parameters.AddWithValue("numEmploye", this.NumEmploye);
                cmdInsert.Parameters.AddWithValue("dateCommande", (object?)this.DateCommande ?? DBNull.Value);
                cmdInsert.Parameters.AddWithValue("valider", (object?)this.Valider ?? DBNull.Value);
                cmdInsert.Parameters.AddWithValue("prixTotal", (object?)this.PrixTotal ?? DBNull.Value);
                nb = DataAccess.Instance.ExecuteInsert(cmdInsert);
            }
            this.NumCommande = nb;
            return nb;
        }

        public void Read()
        {
            using (var cmdSelect = new NpgsqlCommand("select * from Commande where numCommande = @numCommande;"))
            {
                cmdSelect.Parameters.AddWithValue("numCommande", this.NumCommande);
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);
                if (dt.Rows.Count > 0)
                {
                    this.NumEmploye = dt.Rows[0]["numEmploye"] != DBNull.Value ? Convert.ToInt32(dt.Rows[0]["numEmploye"]) : 0;
                    this.DateCommande = dt.Rows[0]["dateCommande"] != DBNull.Value ? Convert.ToDateTime(dt.Rows[0]["dateCommande"]) : (DateTime?)null;
                    this.Valider = dt.Rows[0]["valider"] != DBNull.Value ? Convert.ToBoolean(dt.Rows[0]["valider"]) : (bool?)null;
                    this.PrixTotal = dt.Rows[0]["prixTotal"] != DBNull.Value ? Convert.ToDecimal(dt.Rows[0]["prixTotal"]) : (decimal?)null;
                }
            }
        }

        public int Update()
        {
            using (var cmdUpdate = new NpgsqlCommand("update Commande set numEmploye = @numEmploye, dateCommande = @dateCommande, valider = @valider, prixTotal = @prixTotal where numCommande = @numCommande;"))
            {
                cmdUpdate.Parameters.AddWithValue("numEmploye", this.NumEmploye);
                cmdUpdate.Parameters.AddWithValue("dateCommande", (object?)this.DateCommande ?? DBNull.Value);
                cmdUpdate.Parameters.AddWithValue("valider", (object?)this.Valider ?? DBNull.Value);
                cmdUpdate.Parameters.AddWithValue("prixTotal", (object?)this.PrixTotal ?? DBNull.Value);
                cmdUpdate.Parameters.AddWithValue("numCommande", this.NumCommande);
                return DataAccess.Instance.ExecuteSet(cmdUpdate);
            }
        }

        public int Delete()
        {
            using (var cmdDelete = new NpgsqlCommand("delete from Commande where numCommande = @numCommande;"))
            {
                cmdDelete.Parameters.AddWithValue("numCommande", this.NumCommande);
                return DataAccess.Instance.ExecuteSet(cmdDelete);
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

        public List<Commande> FindBySelection(string criteres)
        {
            // À adapter selon la logique de recherche souhaitée
            throw new NotImplementedException();
        }
    }
}

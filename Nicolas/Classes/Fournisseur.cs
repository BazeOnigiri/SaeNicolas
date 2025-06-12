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
    internal class Fournisseur : ICrude<Fournisseur>
    {
        private int numFournisseur;
        private string? nomFournisseur;

        public Fournisseur(int numFournisseur, string? nomFournisseur)
        {
            NumFournisseur = numFournisseur;
            NomFournisseur = nomFournisseur;
        }

        public int NumFournisseur
        {
            get { return numFournisseur; }
            set { numFournisseur = value; }
        }

        public string? NomFournisseur
        {
            get { return nomFournisseur; }
            set { nomFournisseur = value; }
        }

        public int Create()
        {
            int nb = 0;
            using (var cmdInsert = new NpgsqlCommand("insert into Fournisseur (nomFournisseur) values (@nomFournisseur) RETURNING numFournisseur"))
            {
                cmdInsert.Parameters.AddWithValue("nomFournisseur", (object?)this.NomFournisseur ?? DBNull.Value);
                nb = DataAccess.Instance.ExecuteInsert(cmdInsert);
            }
            this.NumFournisseur = nb;
            return nb;
        }

        public void Read()
        {
            using (var cmdSelect = new NpgsqlCommand("select * from Fournisseur where numFournisseur = @numFournisseur;"))
            {
                cmdSelect.Parameters.AddWithValue("numFournisseur", this.NumFournisseur);
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);
                if (dt.Rows.Count > 0)
                {
                    this.NomFournisseur = dt.Rows[0]["nomFournisseur"] != DBNull.Value ? dt.Rows[0]["nomFournisseur"].ToString() : null;
                }
            }
        }

        public int Update()
        {
            using (var cmdUpdate = new NpgsqlCommand("update Fournisseur set nomFournisseur = @nomFournisseur where numFournisseur = @numFournisseur;"))
            {
                cmdUpdate.Parameters.AddWithValue("nomFournisseur", (object?)this.NomFournisseur ?? DBNull.Value);
                cmdUpdate.Parameters.AddWithValue("numFournisseur", this.NumFournisseur);
                return DataAccess.Instance.ExecuteSet(cmdUpdate);
            }
        }

        public int Delete()
        {
            using (var cmdDelete = new NpgsqlCommand("delete from Fournisseur where numFournisseur = @numFournisseur;"))
            {
                cmdDelete.Parameters.AddWithValue("numFournisseur", this.NumFournisseur);
                return DataAccess.Instance.ExecuteSet(cmdDelete);
            }
        }

        public List<Fournisseur> FindAll()
        {
            List<Fournisseur> lesFournisseurs = new List<Fournisseur>();
            using (NpgsqlCommand cmdSelect = new NpgsqlCommand("select * from Fournisseur;"))
            {
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);
                foreach (DataRow dr in dt.Rows)
                {
                    int numFournisseur = dr["numFournisseur"] != DBNull.Value ? Convert.ToInt32(dr["numFournisseur"]) : 0;
                    string? nomFournisseur = dr["nomFournisseur"] != DBNull.Value ? dr["nomFournisseur"].ToString() : null;

                    lesFournisseurs.Add(new Fournisseur(numFournisseur, nomFournisseur));
                }
            }
            return lesFournisseurs;
        }

        public List<Fournisseur> FindBySelection(string criteres)
        {
            // À adapter selon la logique de recherche souhaitée
            throw new NotImplementedException();
        }
    }
}
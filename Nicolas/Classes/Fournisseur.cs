using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nicolas.Classes
{
    internal class Fournisseur
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
            get
            {
                return numFournisseur;
            }

            set
            {
                numFournisseur = value;
            }
        }

        public string? NomFournisseur
        {
            get
            {
                return nomFournisseur;
            }

            set
            {
                nomFournisseur = value;
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
    }
}

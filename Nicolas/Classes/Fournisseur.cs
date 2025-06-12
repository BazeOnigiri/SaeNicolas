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
                    lesFournisseurs.Add(new Fournisseur((Int32)dr["numFournisseur"], (String?)dr["nomFournisseur"]));
            }
            return lesFournisseurs;
        }
    }
}

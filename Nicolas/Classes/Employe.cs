using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nicolas.Classes
{
    internal class Employe
    {
        private int numEmploye;
        private int numRole;
        private string? nom;
        private string? prenom;
        private string? login;

        public Employe(int numEmploye, int numRole, string? nom, string? prenom, string? login)
        {
            NumEmploye = numEmploye;
            NumRole = numRole;
            Nom = nom;
            Prenom = prenom;
            Login = login;
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

        public int NumRole
        {
            get
            {
                return numRole;
            }

            set
            {
                numRole = value;
            }
        }

        public string? Nom
        {
            get
            {
                return nom;
            }

            set
            {
                nom = value;
            }
        }

        public string? Prenom
        {
            get
            {
                return prenom;
            }

            set
            {
                prenom = value;
            }
        }

        public string? Login
        {
            get
            {
                return login;
            }

            set
            {
                login = value;
            }
        }

        public List<Employe> FindAll()
        {
            List<Employe> lesEmployes = new List<Employe>();
            using (NpgsqlCommand cmdSelect = new NpgsqlCommand("select * from Employe;"))
            {
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);
                foreach (DataRow dr in dt.Rows)
                {
                    int numEmploye = dr["numEmploye"] != DBNull.Value ? Convert.ToInt32(dr["numEmploye"]) : 0;
                    int numRole = dr["numRole"] != DBNull.Value ? Convert.ToInt32(dr["numRole"]) : 0;
                    string? nom = dr["nom"] != DBNull.Value ? dr["nom"].ToString() : null;
                    string? prenom = dr["prenom"] != DBNull.Value ? dr["prenom"].ToString() : null;
                    string? login = dr["login"] != DBNull.Value ? dr["login"].ToString() : null;

                    lesEmployes.Add(new Employe(numEmploye, numRole, nom, prenom, login));
                }
            }
            return lesEmployes;
        }
    }
}

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
    internal class Employe : ICrude<Employe>
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

        public Employe()
        {
        }

        public int NumEmploye
        {
            get { return numEmploye; }
            set { numEmploye = value; }
        }

        public int NumRole
        {
            get { return numRole; }
            set { numRole = value; }
        }

        public string? Nom
        {
            get { return nom; }
            set { nom = value; }
        }

        public string? Prenom
        {
            get { return prenom; }
            set { prenom = value; }
        }

        public string? Login
        {
            get { return login; }
            set { login = value; }
        }

        public int Create()
        {
            int nb = 0;
            using (var cmdInsert = new NpgsqlCommand("insert into Employe (numRole, nom, prenom, login) values (@numRole, @nom, @prenom, @login) RETURNING numEmploye"))
            {
                cmdInsert.Parameters.AddWithValue("numRole", this.NumRole);
                cmdInsert.Parameters.AddWithValue("nom", (object?)this.Nom ?? DBNull.Value);
                cmdInsert.Parameters.AddWithValue("prenom", (object?)this.Prenom ?? DBNull.Value);
                cmdInsert.Parameters.AddWithValue("login", (object?)this.Login ?? DBNull.Value);
                nb = DataAccess.Instance.ExecuteInsert(cmdInsert);
            }
            this.NumEmploye = nb;
            return nb;
        }

        public void Read()
        {
            using (var cmdSelect = new NpgsqlCommand("select * from Employe where numEmploye = @numEmploye;"))
            {
                cmdSelect.Parameters.AddWithValue("numEmploye", this.NumEmploye);
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);
                if (dt.Rows.Count > 0)
                {
                    this.NumRole = dt.Rows[0]["numRole"] != DBNull.Value ? Convert.ToInt32(dt.Rows[0]["numRole"]) : 0;
                    this.Nom = dt.Rows[0]["nom"] != DBNull.Value ? dt.Rows[0]["nom"].ToString() : null;
                    this.Prenom = dt.Rows[0]["prenom"] != DBNull.Value ? dt.Rows[0]["prenom"].ToString() : null;
                    this.Login = dt.Rows[0]["login"] != DBNull.Value ? dt.Rows[0]["login"].ToString() : null;
                }
            }
        }

        public int Update()
        {
            using (var cmdUpdate = new NpgsqlCommand("update Employe set numRole = @numRole, nom = @nom, prenom = @prenom, login = @login where numEmploye = @numEmploye;"))
            {
                cmdUpdate.Parameters.AddWithValue("numRole", this.NumRole);
                cmdUpdate.Parameters.AddWithValue("nom", (object?)this.Nom ?? DBNull.Value);
                cmdUpdate.Parameters.AddWithValue("prenom", (object?)this.Prenom ?? DBNull.Value);
                cmdUpdate.Parameters.AddWithValue("login", (object?)this.Login ?? DBNull.Value);
                cmdUpdate.Parameters.AddWithValue("numEmploye", this.NumEmploye);
                return DataAccess.Instance.ExecuteSet(cmdUpdate);
            }
        }

        public int Delete()
        {
            using (var cmdDelete = new NpgsqlCommand("delete from Employe where numEmploye = @numEmploye;"))
            {
                cmdDelete.Parameters.AddWithValue("numEmploye", this.NumEmploye);
                return DataAccess.Instance.ExecuteSet(cmdDelete);
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

        public List<Employe> FindBySelection(string criteres)
        {
            // À adapter selon la logique de recherche souhaitée
            throw new NotImplementedException();
        }
    }
}
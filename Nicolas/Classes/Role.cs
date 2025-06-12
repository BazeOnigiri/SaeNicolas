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
    internal class Role : ICrude<Role>
    {
        private int numRole;
        private string? nomRole;

        public Role(int numRole, string? nomRole)
        {
            NumRole = numRole;
            NomRole = nomRole;
        }

        public Role()
        {
        }

        public int NumRole
        {
            get { return numRole; }
            set { numRole = value; }
        }

        public string? NomRole
        {
            get { return nomRole; }
            set { nomRole = value; }
        }

        public int Create()
        {
            int nb = 0;
            using (var cmdInsert = new NpgsqlCommand("insert into Role (nomRole) values (@nomRole) RETURNING numRole"))
            {
                cmdInsert.Parameters.AddWithValue("nomRole", (object?)this.NomRole ?? DBNull.Value);
                nb = DataAccess.Instance.ExecuteInsert(cmdInsert);
            }
            this.NumRole = nb;
            return nb;
        }

        public void Read()
        {
            using (var cmdSelect = new NpgsqlCommand("select * from Role where numRole = @numRole;"))
            {
                cmdSelect.Parameters.AddWithValue("numRole", this.NumRole);
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);
                if (dt.Rows.Count > 0)
                {
                    this.NomRole = dt.Rows[0]["nomRole"] != DBNull.Value ? dt.Rows[0]["nomRole"].ToString() : null;
                }
            }
        }

        public int Update()
        {
            using (var cmdUpdate = new NpgsqlCommand("update Role set nomRole = @nomRole where numRole = @numRole;"))
            {
                cmdUpdate.Parameters.AddWithValue("nomRole", (object?)this.NomRole ?? DBNull.Value);
                cmdUpdate.Parameters.AddWithValue("numRole", this.NumRole);
                return DataAccess.Instance.ExecuteSet(cmdUpdate);
            }
        }

        public int Delete()
        {
            using (var cmdDelete = new NpgsqlCommand("delete from Role where numRole = @numRole;"))
            {
                cmdDelete.Parameters.AddWithValue("numRole", this.NumRole);
                return DataAccess.Instance.ExecuteSet(cmdDelete);
            }
        }

        public List<Role> FindAll()
        {
            List<Role> lesRoles = new List<Role>();
            using (NpgsqlCommand cmdSelect = new NpgsqlCommand("select * from Role;"))
            {
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);
                foreach (DataRow dr in dt.Rows)
                {
                    int numRole = dr["numRole"] != DBNull.Value ? Convert.ToInt32(dr["numRole"]) : 0;
                    string? nomRole = dr["nomRole"] != DBNull.Value ? dr["nomRole"].ToString() : null;

                    lesRoles.Add(new Role(numRole, nomRole));
                }
            }
            return lesRoles;
        }

        public List<Role> FindBySelection(string criteres)
        {
            // À adapter selon la logique de recherche souhaitée
            throw new NotImplementedException();
        }
    }
}
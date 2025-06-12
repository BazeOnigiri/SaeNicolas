using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nicolas.Classes
{
    internal class Role
    {
        private int numRole;
        private string? nomRole;

        public Role(int numRole, string? nomRole)
        {
            NumRole = numRole;
            NomRole = nomRole;
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

        public string? NomRole
        {
            get
            {
                return nomRole;
            }

            set
            {
                nomRole = value;
            }
        }

        public List<Role> FindAll()
        {
            List<Role> lesRoles = new List<Role>();
            using (NpgsqlCommand cmdSelect = new NpgsqlCommand("select * from Role;"))
            {
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);
                foreach (DataRow dr in dt.Rows)
                    lesRoles.Add(new Role((Int32)dr["numRole"], (String)dr["nomRole"]));
            }
            return lesRoles;
        }
    }
}

using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nicolas.Classes
{
    internal class TypeVin
    {
        private int numType;
        private string nomtype;

        public TypeVin(int numType, string nomtype)
        {
            NumType = numType;
            Nomtype = nomtype;
        }

        public int NumType
        {
            get
            {
                return numType;
            }

            set
            {
                numType = value;
            }
        }

        public string Nomtype
        {
            get
            {
                return nomtype;
            }

            set
            {
                nomtype = value;
            }
        }

        public List<TypeVin> FindAll()
        {
            List<TypeVin> lesTypeVins = new List<TypeVin>();
            using (NpgsqlCommand cmdSelect = new NpgsqlCommand("select * from TypeVin;"))
            {
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);
                foreach (DataRow dr in dt.Rows)
                    lesTypeVins.Add(new TypeVin((Int32)dr["numType"], (String)dr["nomTypeVin"]));
            }
            return lesTypeVins;
        }
    }
}

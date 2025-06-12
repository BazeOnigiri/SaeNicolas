using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nicolas.Classes
{
    internal class Appelation
    {
        private int numType;
        private string nomAppelation;

        public Appelation(int numType, string nomAppelation)
        {
            NumType = numType;
            NomAppelation = nomAppelation;
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

        public string NomAppelation
        {
            get
            {
                return nomAppelation;
            }

            set
            {
                nomAppelation = value;
            }
        }


        public List<Appelation> FindAll()
        {
            List<Appelation> lesAppelations = new List<Appelation>();
            using (NpgsqlCommand cmdSelect = new NpgsqlCommand("select * from Appelation;"))
            {
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);
                foreach (DataRow dr in dt.Rows)
                    lesAppelations.Add(new Appelation((Int32)dr["numType2"], (String)dr["nomAppelation"]));
            }
            return lesAppelations;
        }
    }
}
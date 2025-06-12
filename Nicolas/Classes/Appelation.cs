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
    public class Appelation : ICrude<Appelation>
    {
        private int numType;
        private string? nomAppelation;

        public Appelation() { }

        public Appelation(int numType, string? nomAppelation)
        {
            NumType = numType;
            NomAppelation = nomAppelation;
        }

        public int NumType
        {
            get { return numType; }
            set { numType = value; }
        }

        public string? NomAppelation
        {
            get { return nomAppelation; }
            set { nomAppelation = value; }
        }

        public int Create()
        {
            int nb = 0;
            using (var cmdInsert = new NpgsqlCommand("insert into Appelation (numType2, nomAppelation) values (@numType, @nomAppelation) RETURNING numType2"))
            {
                cmdInsert.Parameters.AddWithValue("numType", this.NumType);
                cmdInsert.Parameters.AddWithValue("nomAppelation", (object?)this.NomAppelation ?? DBNull.Value);
                nb = DataAccess.Instance.ExecuteInsert(cmdInsert);
            }
            this.NumType = nb;
            return nb;
        }

        public void Read()
        {
            using (var cmdSelect = new NpgsqlCommand("select * from Appelation where numType2 = @numType;"))
            {
                cmdSelect.Parameters.AddWithValue("numType", this.NumType);
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);
                if (dt.Rows.Count > 0)
                {
                    this.NomAppelation = dt.Rows[0]["nomAppelation"] != DBNull.Value ? dt.Rows[0]["nomAppelation"].ToString() : null;
                }
            }
        }

        public int Update()
        {
            using (var cmdUpdate = new NpgsqlCommand("update Appelation set nomAppelation = @nomAppelation where numType2 = @numType;"))
            {
                cmdUpdate.Parameters.AddWithValue("nomAppelation", (object?)this.NomAppelation ?? DBNull.Value);
                cmdUpdate.Parameters.AddWithValue("numType", this.NumType);
                return DataAccess.Instance.ExecuteSet(cmdUpdate);
            }
        }

        public int Delete()
        {
            using (var cmdDelete = new NpgsqlCommand("delete from Appelation where numType2 = @numType;"))
            {
                cmdDelete.Parameters.AddWithValue("numType", this.NumType);
                return DataAccess.Instance.ExecuteSet(cmdDelete);
            }
        }

        public List<Appelation> FindAll()
        {
            List<Appelation> lesAppelations = new List<Appelation>();
            using (NpgsqlCommand cmdSelect = new NpgsqlCommand("select * from Appelation;"))
            {
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);
                foreach (DataRow dr in dt.Rows)
                {
                    int numType = dr["numType2"] != DBNull.Value ? Convert.ToInt32(dr["numType2"]) : 0;
                    string? nomAppelation = dr["nomAppelation"] != DBNull.Value ? dr["nomAppelation"].ToString() : null;

                    lesAppelations.Add(new Appelation(numType, nomAppelation));
                }
            }
            return lesAppelations;
        }

        public List<Appelation> FindBySelection(string criteres)
        {
            // À adapter selon la logique de recherche souhaitée
            throw new NotImplementedException();
        }
    }
}
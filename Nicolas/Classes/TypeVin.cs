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
    public class TypeVin : ICrude<TypeVin>
    {
        private int numType;
        private string? nomtype;

        public TypeVin(int numType, string? nomtype)
        {
            NumType = numType;
            Nomtype = nomtype;
        }

        public TypeVin()
        {
        }

        public int NumType
        {
            get { return numType; }
            set { numType = value; }
        }

        public string? Nomtype
        {
            get { return nomtype; }
            set { nomtype = value; }
        }

        public int Create()
        {
            int nb = 0;
            using (var cmdInsert = new NpgsqlCommand("insert into TypeVin (nomType) values (@nomtype) RETURNING numType"))
            {
                cmdInsert.Parameters.AddWithValue("nomtype", (object?)this.Nomtype ?? DBNull.Value);
                nb = DataAccess.Instance.ExecuteInsert(cmdInsert);
            }
            this.NumType = nb;
            return nb;
        }

        public void Read()
        {
            using (var cmdSelect = new NpgsqlCommand("select * from TypeVin where numType = @numType;"))
            {
                cmdSelect.Parameters.AddWithValue("numType", this.NumType);
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);
                if (dt.Rows.Count > 0)
                {
                    this.Nomtype = dt.Rows[0]["nomType"] != DBNull.Value ? dt.Rows[0]["nomType"].ToString() : null;
                }
            }
        }

        public int Update()
        {
            using (var cmdUpdate = new NpgsqlCommand("update TypeVin set nomType = @nomtype where numType = @numType;"))
            {
                cmdUpdate.Parameters.AddWithValue("nomtype", (object?)this.Nomtype ?? DBNull.Value);
                cmdUpdate.Parameters.AddWithValue("numType", this.NumType);
                return DataAccess.Instance.ExecuteSet(cmdUpdate);
            }
        }

        public int Delete()
        {
            using (var cmdDelete = new NpgsqlCommand("delete from TypeVin where numType = @numType;"))
            {
                cmdDelete.Parameters.AddWithValue("numType", this.NumType);
                return DataAccess.Instance.ExecuteSet(cmdDelete);
            }
        }

        public List<TypeVin> FindAll()
        {
            List<TypeVin> lesTypeVins = new List<TypeVin>();
            using (NpgsqlCommand cmdSelect = new NpgsqlCommand("select * from TypeVin;"))
            {
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);
                foreach (DataRow dr in dt.Rows)
                {
                    int numType = dr["numType"] != DBNull.Value ? Convert.ToInt32(dr["numType"]) : 0;
                    string? nomtype = dr["nomType"] != DBNull.Value ? dr["nomType"].ToString() : null;

                    lesTypeVins.Add(new TypeVin(numType, nomtype));
                }
            }
            return lesTypeVins;
        }

        public List<TypeVin> FindBySelection(string criteres)
        {
            // À adapter selon la logique de recherche souhaitée
            throw new NotImplementedException();
        }
    }
}
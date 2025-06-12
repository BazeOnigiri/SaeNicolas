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
    public class Vin : ICrude<Vin>
    {
        private int numVin;
        private int numFournisseur;
        private int numTypeVin;
        private int numAppelation;
        private string? nomvin;
        private double? prixVin;
        private string? descriptif;
        private int? millesime;

        public Vin(int numVin, int numFournisseur, int numTypeVin, int numAppelation, string? nomvin, double? prixVin, string? descriptif, int? millesime)
        {
            NumVin = numVin;
            NumFournisseur = numFournisseur;
            NumTypeVin = numTypeVin;
            NumAppelation = numAppelation;
            Nomvin = nomvin;
            PrixVin = prixVin;
            Descriptif = descriptif;
            Millesime = millesime;
        }

        public Vin()
        {
        }

        public int NumVin
        {
            get { return numVin; }
            set { numVin = value; }
        }

        public int NumFournisseur
        {
            get { return numFournisseur; }
            set { numFournisseur = value; }
        }

        public int NumTypeVin
        {
            get { return numTypeVin; }
            set { numTypeVin = value; }
        }

        public int NumAppelation
        {
            get { return numAppelation; }
            set { numAppelation = value; }
        }

        public string? Nomvin
        {
            get { return nomvin; }
            set { nomvin = value; }
        }

        public double? PrixVin
        {
            get { return prixVin; }
            set { prixVin = value; }
        }

        public string? Descriptif
        {
            get { return descriptif; }
            set { descriptif = value; }
        }

        public int? Millesime
        {
            get { return millesime; }
            set { millesime = value; }
        }

        public int Create()
        {
            int nb = 0;
            using (var cmdInsert = new NpgsqlCommand("insert into Vin (numFournisseur, numTypeVin, numAppelation, nomVin, prixVin, descriptif, millesime) values (@numFournisseur, @numTypeVin, @numAppelation, @nomVin, @prixVin, @descriptif, @millesime) RETURNING numVin"))
            {
                cmdInsert.Parameters.AddWithValue("numFournisseur", this.NumFournisseur);
                cmdInsert.Parameters.AddWithValue("numTypeVin", this.NumTypeVin);
                cmdInsert.Parameters.AddWithValue("numAppelation", this.NumAppelation);
                cmdInsert.Parameters.AddWithValue("nomVin", (object?)this.Nomvin ?? DBNull.Value);
                cmdInsert.Parameters.AddWithValue("prixVin", (object?)this.PrixVin ?? DBNull.Value);
                cmdInsert.Parameters.AddWithValue("descriptif", (object?)this.Descriptif ?? DBNull.Value);
                cmdInsert.Parameters.AddWithValue("millesime", (object?)this.Millesime ?? DBNull.Value);
                nb = DataAccess.Instance.ExecuteInsert(cmdInsert);
            }
            this.NumVin = nb;
            return nb;
        }

        public void Read()
        {
            using (var cmdSelect = new NpgsqlCommand("select * from Vin where numVin = @numVin;"))
            {
                cmdSelect.Parameters.AddWithValue("numVin", this.NumVin);
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);
                if (dt.Rows.Count > 0)
                {
                    this.NumFournisseur = dt.Rows[0]["numFournisseur"] != DBNull.Value ? Convert.ToInt32(dt.Rows[0]["numFournisseur"]) : 0;
                    this.NumTypeVin = dt.Rows[0]["numType"] != DBNull.Value ? Convert.ToInt32(dt.Rows[0]["numType"]) : 0;
                    this.NumAppelation = dt.Rows[0]["numType2"] != DBNull.Value ? Convert.ToInt32(dt.Rows[0]["numType2"]) : 0;
                    this.Nomvin = dt.Rows[0]["nomVin"] != DBNull.Value ? dt.Rows[0]["nomVin"].ToString() : null;
                    this.PrixVin = dt.Rows[0]["prixVin"] != DBNull.Value ? Convert.ToDouble(dt.Rows[0]["prixVin"]) : (double?)null;
                    this.Descriptif = dt.Rows[0]["descriptif"] != DBNull.Value ? dt.Rows[0]["descriptif"].ToString() : null;
                    this.Millesime = dt.Rows[0]["millesime"] != DBNull.Value ? Convert.ToInt32(dt.Rows[0]["millesime"]) : (int?)null;
                }
            }
        }

        public int Update()
        {
            using (var cmdUpdate = new NpgsqlCommand("update Vin set numFournisseur = @numFournisseur, numTypeVin = @numTypeVin, numAppelation = @numAppelation, nomVin = @nomVin, prixVin = @prixVin, descriptif = @descriptif, millesime = @millesime where numVin = @numVin;"))
            {
                cmdUpdate.Parameters.AddWithValue("numFournisseur", this.NumFournisseur);
                cmdUpdate.Parameters.AddWithValue("numTypeVin", this.NumTypeVin);
                cmdUpdate.Parameters.AddWithValue("numAppelation", this.NumAppelation);
                cmdUpdate.Parameters.AddWithValue("nomVin", (object?)this.Nomvin ?? DBNull.Value);
                cmdUpdate.Parameters.AddWithValue("prixVin", (object?)this.PrixVin ?? DBNull.Value);
                cmdUpdate.Parameters.AddWithValue("descriptif", (object?)this.Descriptif ?? DBNull.Value);
                cmdUpdate.Parameters.AddWithValue("millesime", (object?)this.Millesime ?? DBNull.Value);
                cmdUpdate.Parameters.AddWithValue("numVin", this.NumVin);
                return DataAccess.Instance.ExecuteSet(cmdUpdate);
            }
        }

        public int Delete()
        {
            using (var cmdDelete = new NpgsqlCommand("delete from Vin where numVin = @numVin;"))
            {
                cmdDelete.Parameters.AddWithValue("numVin", this.NumVin);
                return DataAccess.Instance.ExecuteSet(cmdDelete);
            }
        }

        public List<Vin> FindAll()
        {
            List<Vin> lesVins = new List<Vin>();
            using (NpgsqlCommand cmdSelect = new NpgsqlCommand("select * from Vin;"))
            {
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);
                foreach (DataRow dr in dt.Rows)
                {
                    int numVin = dr["numVin"] != DBNull.Value ? Convert.ToInt32(dr["numVin"]) : 0;
                    int numFournisseur = dr["numFournisseur"] != DBNull.Value ? Convert.ToInt32(dr["numFournisseur"]) : 0;
                    int numTypeVin = dr["numType"] != DBNull.Value ? Convert.ToInt32(dr["numType"]) : 0;
                    int numAppelation = dr["numType2"] != DBNull.Value ? Convert.ToInt32(dr["numType2"]) : 0;
                    string? nomvin = dr["nomVin"] != DBNull.Value ? dr["nomVin"].ToString() : null;
                    double? prixVin = dr["prixVin"] != DBNull.Value ? Convert.ToDouble(dr["prixVin"]) : (double?)null;
                    string? descriptif = dr["descriptif"] != DBNull.Value ? dr["descriptif"].ToString() : null;
                    int? millesime = dr["millesime"] != DBNull.Value ? Convert.ToInt32(dr["millesime"]) : (int?)null;

                    lesVins.Add(new Vin(numVin, numFournisseur, numTypeVin, numAppelation, nomvin, prixVin, descriptif, millesime));
                }
            }
            return lesVins;
        }

        public List<Vin> FindBySelection(string criteres)
        {
            // À adapter selon la logique de recherche souhaitée
            throw new NotImplementedException();
        }
    }
}
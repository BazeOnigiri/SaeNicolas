using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nicolas.Classes
{
    internal class Vin
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

        public int NumVin
        {
            get
            {
                return numVin;
            }

            set
            {
                numVin = value;
            }
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

        public int NumTypeVin
        {
            get
            {
                return numTypeVin;
            }

            set
            {
                numTypeVin = value;
            }
        }

        public int NumAppelation
        {
            get
            {
                return numAppelation;
            }

            set
            {
                numAppelation = value;
            }
        }

        public string? Nomvin
        {
            get
            {
                return nomvin;
            }

            set
            {
                nomvin = value;
            }
        }

        public double? PrixVin
        {
            get
            {
                return prixVin;
            }

            set
            {
                prixVin = value;
            }
        }

        public string? Descriptif
        {
            get
            {
                return descriptif;
            }

            set
            {
                descriptif = value;
            }
        }

        public int? Millesime
        {
            get
            {
                return millesime;
            }

            set
            {
                millesime = value;
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
                    int numTypeVin = dr["numTypeVin"] != DBNull.Value ? Convert.ToInt32(dr["numTypeVin"]) : 0;
                    int numAppelation = dr["numAppelation"] != DBNull.Value ? Convert.ToInt32(dr["numAppelation"]) : 0;
                    string? nomvin = dr["nomVin"] != DBNull.Value ? dr["nomVin"].ToString() : null;
                    double? prixVin = dr["prixVin"] != DBNull.Value ? Convert.ToDouble(dr["prixVin"]) : (double?)null;
                    string? descriptif = dr["descriptif"] != DBNull.Value ? dr["descriptif"].ToString() : null;
                    int? millesime = dr["millesime"] != DBNull.Value ? Convert.ToInt32(dr["millesime"]) : (int?)null;

                    lesVins.Add(new Vin(numVin, numFournisseur, numTypeVin, numAppelation, nomvin, prixVin, descriptif, millesime));
                }
            }
            return lesVins;
        }
    }
}

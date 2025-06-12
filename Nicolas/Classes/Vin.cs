using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nicolas.Classes
{
    internal class Vin
    {
        private int numVin;
        private int numFournisseur;
        private int numtype1;
        private int numtype2;
        private string nomvin;
        private double prixVin;
        private string descriptif;
        private int millesime;

        public Vin(int numVin, int numFournisseur, int numtype1, int numtype2, string nomvin, double prixVin, string descriptif, int millesime)
        {
            NumVin = numVin;
            NumFournisseur = numFournisseur;
            Numtype1 = numtype1;
            Numtype2 = numtype2;
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

        public int Numtype1
        {
            get
            {
                return numtype1;
            }

            set
            {
                numtype1 = value;
            }
        }

        public int Numtype2
        {
            get
            {
                return numtype2;
            }

            set
            {
                numtype2 = value;
            }
        }

        public string Nomvin
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

        public double PrixVin
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

        public string Descriptif
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

        public int Millesime
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
    }
}

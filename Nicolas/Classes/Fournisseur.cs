using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nicolas.Classes
{
    internal class Fournisseur
    {
        private int numFournisseur;
        private string nomFournisseur;

        public Fournisseur(int numFournisseur, string nomFournisseur)
        {
            NumFournisseur = numFournisseur;
            NomFournisseur = nomFournisseur;
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

        public string NomFournisseur
        {
            get
            {
                return nomFournisseur;
            }

            set
            {
                nomFournisseur = value;
            }
        }
    }
}

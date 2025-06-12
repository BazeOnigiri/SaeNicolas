using System;
using System.Collections.Generic;
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
    }
}

using System;
using System.Collections.Generic;
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
    }
}

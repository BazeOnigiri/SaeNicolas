using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nicolas.Interfaces
{
    internal interface ICrude<T>
    {
        public int Create();

        public void Read();

        public int Update();

        public int Delete();

        public List<T> FindAll();

        public List<T> FindBySelection(string criteres);
    }
}

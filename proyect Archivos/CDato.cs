using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proyect_Archivos
{
    class CDato
    {
        private long Ptr_dato;
        private long Direccion;
        private List<object> campos;

        public CDato()
        {
            campos = new List<object>();
        }
    }
}

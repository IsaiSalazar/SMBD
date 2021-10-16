using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proyect_Archivos
{
   public  class CEntidad
    {
        private char[] nombre;
        private long direccion;
        private long ptr_atrib;
        private long ptr_datos;
        private long ptr_ent;
        public CEntidad()
        {

        }
        public char[] asignombre
        {
            set
            {
                nombre = value;
            }
            get
            {
                return nombre;
            }
        }
        public long asigdireccion
        {
            set
            {
                direccion = value;
            }
            get
            {
                return direccion;
            }
        }
        public long atrib
        {
            set
            {
                ptr_atrib = value;
            }
            get
            {
                return ptr_atrib;
            }
        }
        public long asigdat
        {
            set
            {
                ptr_datos = value;
            }
            get
            {
                return ptr_datos;
            }
        }
        public long asigent
        {
            set
            {
                ptr_ent = value;
            }
            get
            {
                return ptr_ent;
            }
        }
    }
}

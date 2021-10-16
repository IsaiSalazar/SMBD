using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proyect_Archivos
{
    public class CAtributo
    {
        private char[] nombre;
        private long dir_atributo;
        private char tipo_dato;
        private int long_dato;
        private int tipo_indice;   
        private long dir_indice;
        private long dir_sig_atributo;

        public CAtributo()
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
                dir_atributo = value;
            }
            get
            {
                return dir_atributo;
            }
        }

        public long atrib
        {
            set
            {
                dir_sig_atributo = value;
            }
            get
            {
                return dir_sig_atributo;
            }
        }
        public long ind
        {
            set
            {
                dir_indice = value;
            }
            get
            {
                return dir_indice;
            }
        }
        public char tipoat
        {
            set
            {
                tipo_dato = value;
            }
            get
            {
                return tipo_dato;
            }
        }
        public int tipoind
        {
            set
            {
                tipo_indice = value;
            }
            get
            {
                return tipo_indice;
            }
        }
        public int longitud
        {
            set
            {
                long_dato = value;
            }
            get
            {
                return long_dato;
            }
        }
    }
}

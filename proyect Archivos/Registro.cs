using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proyect_Archivos
{
    class Registro
    {


        private long direccionRegistro;
        private List<Byte[]> listaAtributosRegistro;
        private long direccionSiguienteRegistro;

        public int dameTamañoRegistro()
        {
            int tam = 16;
            foreach (var item in listaAtributosRegistro)
            {
                tam += item.Length;
            }
            return tam;
        }

        //Método set get para la dirección del registro
        public long dimeDireccionRegstro
        {
            set
            {
                direccionRegistro = value;
            }
            get
            {
                return direccionRegistro;
            }
        }

        //Método set get para la lista en bytes de los atributos del registro
        public List<Byte[]> dimeAtributosRegistro
        {
            set
            {
                listaAtributosRegistro = value;
            }
            get
            {
                return listaAtributosRegistro;
            }
        }

        //Método set get para la dirección del siguiente registro
        public long dimeDireccionSiguienteRegstro
        {
            set
            {
                direccionSiguienteRegistro = value;
            }
            get
            {
                return direccionSiguienteRegistro;
            }
        }
    }
}

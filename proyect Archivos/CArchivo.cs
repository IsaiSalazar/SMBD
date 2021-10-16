using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO; // No olvidar este using.
namespace proyect_Archivos
{
    class CArchivo
    {
        private string nombre;
        private FileStream archivo;

        public CArchivo()
        {

        }



        public FileStream asignarchivo
        {
            set
            {
                archivo = value;
            }
            get
            {
                return archivo;
            }
        }
        public string asignanombre
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
        public void cierre()
        {
            archivo.Close();
        }
    }
}

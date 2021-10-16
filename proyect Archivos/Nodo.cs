using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proyect_Archivos
{
    class Nodo
    {

        //Atributos de clase
        public char tipo;
        public long direccion;
        public List<long> listaDirecciones;
        public List<int> listaClaves;

        //Constructor
        public Nodo()
        {
            listaDirecciones = new List<long>();
            listaClaves = new List<int>();
            for (int i = 0; i < CDiccionario.gradoArbol; i++)
            {
                listaDirecciones.Add(-1);
                if (i != CDiccionario.gradoArbol - 1)
                    listaClaves.Add(-1);
            }
        }

        public bool insertaEnHoja(int dato, long direccion)
        {
            if (numeroClaves() < CDiccionario.gradoArbol - 1)
            {
                List<long> direcciones = new List<long>();
                List<int> claves = new List<int>();

                foreach (int clave in listaClaves)
                    if (clave != -1)
                        claves.Add(clave);

                foreach (long dir in listaDirecciones)
                    if (dir != -1)
                        direcciones.Add(dir);

                claves.Add(dato);
                claves.Sort();
                int idxTemporal = 0;
                for (int i = 0; i < claves.Count; i++)
                {
                    listaClaves[i] = claves[i];
                    if (claves[i] != dato)
                        listaDirecciones[i] = direcciones[idxTemporal++];
                    else
                        listaDirecciones[i] = direccion;
                }
                return true;
            }
            return false;
        }

        public bool insertaEnRaizOIntermedio(int dato, long direccion)
        {
            if (numeroClaves() < CDiccionario.gradoArbol - 1)
            {
                List<long> direcciones = new List<long>();
                List<int> claves = new List<int>();

                foreach (int clave in listaClaves)
                    if (clave != -1)
                        claves.Add(clave);

                foreach (long dir in listaDirecciones)
                    if (dir != -1)
                        direcciones.Add(dir);

                if (direcciones.Count > 1) direcciones.RemoveAt(0);

                claves.Add(dato);
                claves.Sort();

                int idxTemporal = 0;

                for (int i = 0; i < claves.Count; i++)
                {
                    listaClaves[i] = claves[i];
                    if (claves[i] != dato)
                        listaDirecciones[i + 1] = direcciones[idxTemporal++];
                    else
                        listaDirecciones[i + 1] = direccion;
                }
                return true;
            }
            return false;
        }

        public int damePosicionActualDato(int dato)
        {
            if (listaClaves.Count == 0) return 0;
            int index;

            for (index = 0; index < CDiccionario.gradoArbol - 1; index++)
            {
                if (dato < listaClaves[index] || listaClaves[index] == -1)
                    return index;
            }

            return index;
        }

        public int numeroClaves()
        {
            int total = 0;
            foreach (var item in listaClaves)
            {
                if (item != -1)
                    total++;
                else
                    return total;
            }
            return total;
        }

        public bool eliminaEnHoja(int dato)
        {
            if (listaClaves.Contains(dato))
            {
                int index = listaClaves.IndexOf(dato);
                long siguienteDireccion = listaDirecciones[CDiccionario.gradoArbol - 1];
                listaDirecciones[CDiccionario.gradoArbol - 1] = (long)-1;

                listaClaves.RemoveAt(index);
                listaDirecciones.RemoveAt(index);
                listaClaves.Add(-1);
                listaDirecciones.Add(siguienteDireccion);
                return true;
            }
            else
                return false;
        }

        public bool eliminaNodoRaizOIntermedio(int dato, long direccion)
        {
            if (listaClaves.Contains(dato) && listaDirecciones.Contains(direccion))
            {
                listaDirecciones.Remove(direccion);
                listaClaves.Remove(dato);
                listaDirecciones.Add((long)-1);
                listaClaves.Add(-1);
                return true;
            }
            else
                return false;
        }

        public long dameApuntadorHoja(int dato)
        {
            return listaDirecciones[listaClaves.IndexOf(dato)];
        }
    }
}

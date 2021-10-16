using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proyect_Archivos
{
    class Arbol
    {
        //Atributos de clase
        public List<Nodo> listaDeNodos;
        public CAtributo atributo;

        //Cobstructor de la clase
        public Arbol(List<Nodo> listaNodos, CAtributo atributo)
        {
            listaDeNodos = listaNodos;
            this.atributo = atributo;
        }

        //Método para saber si el árbol contiene una raíz
        public bool contieneRaiz()
        {
            foreach (Nodo nodo in listaDeNodos)
            {
                if (nodo.tipo == 'R')
                    return true;
            }
            return false;
        }

        //Método para obtener la raíz del árbol
        public Nodo dameNodoRaiz()
        {
            foreach (Nodo nodo in listaDeNodos)
            {
                if (nodo.tipo == 'R')
                    return nodo;
            }
            return null;
        }

        //Método para obtener un nodo en específico
        public Nodo dameNodo(long direccionDelNodo)
        {
            foreach (Nodo nodo in listaDeNodos)
            {
                if (nodo.direccion == direccionDelNodo)
                    return nodo;
            }
            return null;
        }

        //Método para obtener el nodo padre del nodo actual
        public Nodo dameNodoPadre(Nodo nodoHijo)
        {
            foreach (var nodo in listaDeNodos)
            {
                foreach (var item in nodo.listaDirecciones)
                {
                    if (item == nodoHijo.direccion)
                    {
                        return nodo;
                    }
                }
            }
            return null;
        }

        //Método para saber si el nodo contiene llave
        public bool contieneClave(int dato)
        {
            foreach (Nodo nodo in listaDeNodos)
            {
                if (nodo.tipo == 'H' && nodo.listaClaves.Contains(dato))
                {
                    return true;
                }
            }
            return false;
        }

        //Método para obtener la dirección de la llave
        public long dameDireccionDeLaClave(int dato)
        {
            foreach (Nodo nodo in listaDeNodos)
            {
                if (nodo.tipo == 'H' && nodo.listaClaves.Contains(dato))
                {
                    int index = nodo.listaClaves.IndexOf(dato);
                    return nodo.listaDirecciones[index];
                }
            }
            return -1;
        }

        //Método para obtener el nodo que contiene la llave
        public Nodo dameNodoDeLaLlave(int dato)
        {
            foreach (Nodo nodo in listaDeNodos)
            {
                if (nodo.tipo == 'H' && nodo.listaClaves.Contains(dato))
                {
                    return nodo;
                }
            }
            return null;
        }

        //Método para obtener el nodo vecino izquierdo del nodo actual
        public Nodo dameNodoVecinoIzquierdo(Nodo nodoActual)
        {
            Nodo nodoPadre = dameNodoPadre(nodoActual);
            int idxNodo = nodoPadre.listaDirecciones.IndexOf(nodoActual.direccion);
            if (idxNodo != 0)
            {
                return dameNodo(nodoPadre.listaDirecciones[idxNodo - 1]);
            }
            else
                return null;
        }

        //Método para obtener el nodo vecino derecho del nodo actual
        public Nodo dameNodoVecinoDerecho(Nodo nodoActual)
        {
            Nodo nodoPadre = dameNodoPadre(nodoActual);
            int idxNodo = nodoPadre.listaDirecciones.IndexOf(nodoActual.direccion);
            if (idxNodo < nodoPadre.listaDirecciones.Count - 1)
            {
                return dameNodo(nodoPadre.listaDirecciones[idxNodo + 1]);
            }
            else
                return null;
        }

        //Método para saber si dos nodos comparten el mismo padre
        public bool tieneMismoNodoPadre(Nodo nodo1, Nodo nodo2)
        {
            Nodo nodoPadre = dameNodoPadre(nodo1);
            return nodoPadre.listaDirecciones.Contains(nodo2.direccion);
        }
    }
}

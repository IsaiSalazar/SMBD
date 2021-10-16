using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace proyect_Archivos
{
    public class Consulta
    {
        String consul;
        public List<String> tablas = new List<String>();
        public List<String> atributos = new List<String>();
        public List<String[]> condJoin = new List<String[]>();
        public String[] condOpe = new String[3];
        public bool multiTablas = false;
        public Consulta(String sentencia)
        {
            consul = arreglaCadena(sentencia);
            tablas.Clear();
            atributos.Clear();
            consul = obtenUltima(consul);
            rellenaEstructuras(consul);
            //MessageBox.Show(consul);
        }
        private String arreglaCadena(String recibida)
        {
            String Cadena = recibida;
            Cadena = Cadena.Replace("\t", " ");
            Cadena = Cadena.Replace("\r\n", " ").Replace("\n", " ").Replace("\r", " ");
            Cadena = Cadena.Replace("<", " < ");
            Cadena = Cadena.Replace(">", " > ");
            Cadena = Cadena.Replace("=", " = ");

            while (Cadena.Contains("  "))
            {
                Cadena = Cadena.Replace("  ", " ");
            }
            Cadena = Cadena.Replace("< >", "<>");
            Cadena = Cadena.Replace("< =", "<=");
            Cadena = Cadena.Replace("> =", ">=");
            Cadena = Cadena.Replace(", ", ",");
            Cadena = Cadena.Replace(" ,", ",");
            return Cadena;
        }
        private String obtenUltima(String cadena)
        {
            String[] temporal = cadena.Split(';');
            if (temporal.Length >= 2)
            {
                return temporal[temporal.Length - 2];
            }
            else
            {
                MessageBox.Show("Falta un punto y coma");
                return null;
            }

        }
        private void rellenaEstructuras(String cadena)
        {
            String[] arr = cadena.Split(' ');
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i].Equals("select", StringComparison.InvariantCultureIgnoreCase))
                {

                    String[] atr = arr[i + 1].Split(',');
                    for (int j = 0; j < atr.Length; j++)
                    {
                        atributos.Add(atr[j]);
                    }
                    i++;
                }
                else
                {
                    if (arr[i].Equals("from", StringComparison.InvariantCultureIgnoreCase) || arr[i].Equals("join", StringComparison.InvariantCultureIgnoreCase))
                    {
                        tablas.Add(arr[i + 1]);
                        i++;
                    }
                    else
                    {
                        if (arr[i].Equals("on", StringComparison.InvariantCultureIgnoreCase) || arr[i].Equals("and", StringComparison.InvariantCultureIgnoreCase))
                        {
                            String[] cond = new String[3];
                            cond[0] = arr[i + 1];
                            cond[1] = arr[i + 2];
                            cond[2] = arr[i + 3];
                            condJoin.Add(cond);
                            multiTablas = true;
                            i += 3;
                        }
                        else
                        {
                            if (arr[i].Equals("where", StringComparison.InvariantCultureIgnoreCase))
                            {
                                condOpe[0] = arr[i + 1];
                                condOpe[1] = arr[i + 2];
                                condOpe[2] = arr[i + 3];

                            }
                        }
                    }
                }
            }
        }
    }
}

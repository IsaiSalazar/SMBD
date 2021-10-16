using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
namespace proyect_Archivos
{
    class CDiccionario
    {
        private long cab;
        private CArchivo Archivo;
        public FileStream archivoDeDatos;
        public FileStream archivoIndice;
        private List<CEntidad> lista_Ent;
        private List<CAtributo> lista_Atrib;
        private List<long> lis_int;
        public bool abierto;
        private string ruta;
        private string rutada;
        public const int gradoArbol = 5;
        public const int tamañoBloque = 1048;// 2048;
        public const int numeroRegistros = (tamañoBloque) / 8;



        public List<CAtributo>  regretlieatribut()
        {
            return lista_Atrib;
        }
        public CDiccionario()
        {
            lista_Ent = new List<CEntidad>();
            lista_Atrib = new List<CAtributo>();
            Archivo = new CArchivo();
            lis_int = new List<long>();
        }

        public void eliminaciontotal()
        {
            for(int i=0;i <lista_Ent.Count;i++)
            { File.Delete(rutada +"\\"+ new string(lista_Ent[i].asignombre).Replace(" ", "")+".dat");
            }
            for (int i = 0; i < lista_Atrib.Count; i++)
            {
                File.Delete(rutada + "\\" + new string(lista_Atrib[i].asignombre).Replace(" ", "") + ".dat");
            }
        }
        #region Archivo
        public void asignarutacarpet(string rut)
        {
            this.rutada = rut;
        }

        public bool abrirarchi(string ruta)
    {

            string path = Directory.GetCurrentDirectory();
            FileStream archivo = new FileStream(ruta, FileMode.Open, FileAccess.ReadWrite);
            if (archivo != null)
            {
                abierto = true;
                Archivo.asignarchivo = archivo;
                Archivo.asignanombre = ruta;
                return true;
            }
            else
                return false;
    }
        public void creaArchivoDiccionario(string ruta)
        {
            FileStream archivo = new FileStream(ruta, FileMode.Create);
            BinaryWriter escritorBinario = new BinaryWriter(archivo);
            escritorBinario.Write((long)-1);
            aigcab = (long)-1;
            this.ruta = ruta; 
            archivo.Close();
        }
        public void cierre()
    {
            Archivo.cierre();
    }
  
        public CArchivo regresarch()
        {
            return this.Archivo;
        }
        #endregion

        /// //////////////////////////ENTIDADES///////////////////////
        #region Entidades

        public long aigcab
        {
            set
            {
                cab = value;
            }
            get
            {
                return cab;
            }
        }

        public int numdeentida()
        {
            return lista_Ent.Count;
        }
        public List<CEntidad> regrelistaen()
        {
            return lista_Ent;
        }
        //retornar el valor de la cabecera
        public long buscacabecer()
        {
            BinaryReader lectorBinario = new BinaryReader(Archivo.asignarchivo);//creacion de lector de archivo binario
            lectorBinario.BaseStream.Position = 0;//establece la posicion del apuntador del strem en 0
            return lectorBinario.ReadInt64();//le los primero 64 bits o 8 bytes que corresponden al long de la cabecera
        }
        public void insertaEntidad(string nombreEntidad)
        {
            if (abierto)
            {
              CEntidad entidad = new CEntidad();

                entidad.asignombre = transformachar(nombreEntidad);
                if (lista_Ent.Count == 0)
                {
                    entidad.asigdireccion = (long)8;
                }
                else
                { entidad.asigdireccion = Archivo.asignarchivo.Length;
                }
                entidad.atrib = -1;
                entidad.asigdat = -1;
                entidad.asigent = dameDireccionDeLaSiguienteEntidad(entidad);
                escribeRegistroEntidad(entidad);
            }
        }
        public long dameDireccionDeLaSiguienteEntidad(CEntidad registro)
        {
            List<CEntidad> listaEntidades = dameRegistrosEntidad();
            if (listaEntidades.Count == 0)
            {
                escribeCabecera(registro.asigdireccion);
                return -1;
            }
            else
            {
                int i = devuelveIndiceOrdenEntidad(listaEntidades, registro.asignombre);
                if (i == 0)
                {
                    //Principio
                    escribeCabecera(registro.asigdireccion);
                    return listaEntidades[i].asigdireccion;
                }
                else if (i == -1)
                {
                    //Final
                    escribeDireccionSiguienteEntidad(listaEntidades[listaEntidades.Count - 1].asigdireccion, registro.asigdireccion);
                    return -1;
                }
                else
                {
                    //Intermedio
                    escribeDireccionSiguienteEntidad(listaEntidades[i - 1].asigdireccion, registro.asigdireccion);
                    return listaEntidades[i].asigdireccion;
                }
            }
        }
        public int devuelveIndiceOrdenEntidad(List<CEntidad> listaEntidades, char[] insertar)
        {
            for (int i = 0; i < listaEntidades.Count; i++)
            {
                if (string.Compare(new string(insertar), new string(listaEntidades[i].asignombre)) <= 0)
                    return i;
            }
            return -1;
        }
        public List<CEntidad> dameRegistrosEntidad()
        {
            List<CEntidad> listaEntidades = new List<CEntidad>();
            BinaryReader lectorBinario = new BinaryReader(Archivo.asignarchivo);
            lectorBinario.BaseStream.Position = 0;
            long direccion = lectorBinario.ReadInt64();
            for (int i = 0; direccion > 0; i++)
            {
                try
                {
                    lectorBinario.BaseStream.Position = direccion;
                    listaEntidades.Add(new CEntidad());
                    listaEntidades[i].asignombre = System.Text.Encoding.UTF8.GetString(lectorBinario.ReadBytes(30)).ToCharArray();
                    listaEntidades[i].asigdireccion = lectorBinario.ReadInt64();
                    listaEntidades[i].atrib = lectorBinario.ReadInt64();
                    listaEntidades[i].asigdat = lectorBinario.ReadInt64();
                    direccion = listaEntidades[i].asigent = lectorBinario.ReadInt64();
                }
                catch
                {
                    direccion = -1;
                }
            }
            return listaEntidades;
        }
        public void llenalista()
        {
            lista_Ent.Clear();
            BinaryReader lectorBinario = new BinaryReader(Archivo.asignarchivo);
            lectorBinario.BaseStream.Position = 0;
            long direccion = lectorBinario.ReadInt64();
            for (int i = 0; direccion > 0; i++)
            {
                try
                {
                    lectorBinario.BaseStream.Position = direccion;
                    lista_Ent.Add(new CEntidad());
                    lista_Ent[i].asignombre = System.Text.Encoding.UTF8.GetString(lectorBinario.ReadBytes(30)).ToCharArray();
                    lista_Ent[i].asigdireccion = lectorBinario.ReadInt64();
                    lista_Ent[i].atrib = lectorBinario.ReadInt64();
                    lista_Ent[i].asigdat = lectorBinario.ReadInt64();
                    direccion = lista_Ent[i].asigent = lectorBinario.ReadInt64();
                }
                catch
                {
                    direccion = -1;
                }
            }

        }


        public void escribeDireccionSiguienteEntidad(long direccion, long direccionSiguienteEntidad)
        {
            BinaryWriter escritorBinario = new BinaryWriter(Archivo.asignarchivo);
            escritorBinario.BaseStream.Position = direccion + 54;
            escritorBinario.Write(direccionSiguienteEntidad);
        }
        public void escribeRegistroEntidad(CEntidad registro)
        {
            BinaryWriter escritorBinario = new BinaryWriter(Archivo.asignarchivo);
            escritorBinario.BaseStream.Position = registro.asigdireccion;
            escritorBinario.Write(registro.asignombre);
            escritorBinario.Write(registro.asigdireccion);
            escritorBinario.Write(registro.atrib);
            escritorBinario.Write(registro.asigdat);
            escritorBinario.Write(registro.asigent);
            //escribeCabecera(registro.asigdireccion);
        }
        public void escribeCabecera(long direccion)
        {
            BinaryWriter escritorBinario = new BinaryWriter(Archivo.asignarchivo);
            escritorBinario.BaseStream.Position = 0;
            escritorBinario.Write(direccion);
        }
        public void buscadatos()
        {
            long inicio = buscacabecer();

        }
        public bool existenoment(string x)
        {
            for(int i=0;i<lista_Ent.Count;i++)
            {
                string du = new string(lista_Ent[i].asignombre).Replace(" ","");
                if (du==x)
                {
                    return true;
                }
            }
            return false;
        }

        public static char[] transformachar(string nombre)
        {
            int tamaño = 30;
            char[] nombreEntidad = new char[tamaño];
            for (int i = 0; i < tamaño; i++)
            {
                if (i < nombre.Length)
                    nombreEntidad[i] = nombre.ToCharArray()[i];
                else
                    nombreEntidad[i] = ' ';
            }
            return nombreEntidad;
        }
        public long idreturn()
        {
            return Archivo.asignarchivo.Length;
        }
        public long atreturn(string x)
        {
            for(int i=0;i < lista_Ent.Count;i++)
            {
                if(new string(lista_Ent[i].asignombre).Replace(" ", "") == x)
                {
                    return lista_Ent[i].atrib;
                }
            }
            return -1;
        }

        public long idreturn2(string x)
        {
            for (int i = 0; i < lista_Ent.Count; i++)
            {
                if (new string(lista_Ent[i].asignombre).Replace(" ", "") == x)
                {
                    return lista_Ent[i].asigdireccion;
                }
            }
            return -1;
        }
        public long sigeturn(string x)
        {
            for (int i = 0; i < lista_Ent.Count; i++)
            {
                if (new string(lista_Ent[i].asignombre).Replace(" ", "") == x)
                {
                    return lista_Ent[i].asigent;
                }
            }
            return -1;
        }
        public long dateturn(string x)
        {
            for (int i = 0; i < lista_Ent.Count; i++)
            {
                if (new string(lista_Ent[i].asignombre).Replace(" ", "") == x)
                {
                    return lista_Ent[i].asigdat;
                }
            }
            return -1;
        }



        public void actualizaEntidad(int x, string nombre)
        {
            List<CEntidad> listaEntidades = dameRegistrosEntidad();
            CEntidad entidad = lista_Ent[x];
             char[] ppp = entidad.asignombre;
           entidad.asignombre = transformachar(nombre);
            eliminaEntidad2(x);
          
            escribeDireccionSiguienteEntidad(entidad.asigdireccion, dameDireccionDeLaSiguienteEntidad(entidad));
            escribeNombreEntidad(entidad.asigdireccion, transformachar(nombre));
            System.IO.File.Move(rutada+"\\"+ new string(ppp).Replace(" ", "") + ".dat", rutada + "\\" + new string(entidad.asignombre).Replace(" ", "") + ".dat");
        }
        public void escribeNombreEntidad(long direccion, char[] nombre)
        {
            BinaryWriter escritorBinario = new BinaryWriter(Archivo.asignarchivo);
            escritorBinario.BaseStream.Position = direccion;
            escritorBinario.Write(nombre);
        }
        public void eliminaEntidad(int x)
        {
            List<CEntidad> listaEntidades = dameRegistrosEntidad();
            char[] elim;
            if (x == 0)
            { escribeCabecera(lista_Ent[0].asigent);
                elim = lista_Ent[x].asignombre;
            }
            else if (x == listaEntidades.Count - 1)
            {
                escribeDireccionSiguienteEntidad(listaEntidades[x - 1].asigdireccion, (long)-1);
                elim = lista_Ent[x].asignombre;
            }
            else
            {
                escribeDireccionSiguienteEntidad(listaEntidades[x - 1].asigdireccion, listaEntidades[x].asigent);
                elim = lista_Ent[x ].asignombre;
            }
            //File.Delete(rutada + "\\" + new string(elim).Replace(" ", "") + ".dat");
            /* if(lista_Ent.Count==0)
              {
                  Archivo.asignarchivo.SetLength(0);
                  creaArchivoDiccionario(this.ruta);
                  abrirarchi(this.ruta);
              }*/
        }
        public void eliminaEntidad2(int x)
        {
            List<CEntidad> listaEntidades = dameRegistrosEntidad();
            char[] elim;
            if (x == 0)
            {
                escribeCabecera(lista_Ent[0].asigent);
                elim = lista_Ent[x].asignombre;
            }
            else if (x == listaEntidades.Count - 1)
            {
                escribeDireccionSiguienteEntidad(listaEntidades[x - 1].asigdireccion, (long)-1);
                elim = lista_Ent[x].asignombre;
            }
            else
            {
                escribeDireccionSiguienteEntidad(listaEntidades[x - 1].asigdireccion, listaEntidades[x].asigent);
                elim = lista_Ent[x].asignombre;
            }
            File.Delete(rutada + "\\" + new string(elim).Replace(" ", "") + ".dat");
            /* if(lista_Ent.Count==0)
              {
                  Archivo.asignarchivo.SetLength(0);
                  creaArchivoDiccionario(this.ruta);
                  abrirarchi(this.ruta);
              }*/
        }
        public bool cambiono(string x,int x1)
        {
            for (int i = 0; i < lista_Ent.Count; i++)
            {
                string s = new string(lista_Ent[i].asignombre).Replace(" ", "");
                if (s == x&& i!=x1)
                {
                    return false;
                }
            }
            return true;
        }
        #endregion
        //////////////////////////////////////FIN DE ENTIDADES//////////////

        ///entidades y atributos/////////////////////////////////////////////
        ///


        public void orden()
        {
            lis_int.Clear();
            for (int i = 0; i < lista_Ent.Count; i++)
                lis_int.Add(lista_Ent[i].asigdireccion);
        //    for (int i = 0; i < lista_Atrib.Count; i++)
         //lis_int.Add(lista_Atrib[i].asigdireccion);

           // lis_int.Sort();
        }
        public int regresaorde()
        {
            return lis_int.Count;
        }
        public long miposi(int x)
        {
            return lis_int[x];
        }
        public int soy(long x)
        {

            for(int i=0;i<lista_Ent.Count;i++)
            {       
              if(lista_Ent[i].asigdireccion==x)
            {
                    return 0;
                }
            }
            for (int i = 0; i < lista_Atrib.Count; i++)
            {
                if (lista_Atrib[i].asigdireccion == x)
                {
                    return 1;
                }
            }
            return -1;

        }

        public long regreente(long x)
        {
            for (int i = 0; i < lista_Ent.Count; i++)
            {
                if (lista_Ent[i].asigdireccion == x)
                {
                    return lista_Ent[i].asigdireccion;
                }
            }
            return -1;
        }
        public string regenom(long x)
        {
            for (int i = 0; i < lista_Ent.Count; i++)
            {
                if (lista_Ent[i].asigdireccion == x)
                {
                    return new string(lista_Ent[i].asignombre).Replace(" ", "");
                }
            }
            return "error";
        }
        public long regreatribe(long x)
        {
            for (int i = 0; i < lista_Ent.Count; i++)
            {
                if (lista_Ent[i].asigdireccion == x)
                {
                    return lista_Ent[i].atrib;
                }
            }
            return -1;
        }
        public long regdato(long x)
        {
            for (int i = 0; i < lista_Ent.Count; i++)
            {
                if (lista_Ent[i].asigdireccion == x)
                {
                    return lista_Ent[i].asigdat;
                }
            }
            return -1;
        }
        public long regsige(long x)
        {
            for (int i = 0; i < lista_Ent.Count; i++)
            {
                if (lista_Ent[i].asigdireccion == x)
                {
                    return lista_Ent[i].asigent;
                }
            }
            return -1;
        }

        public long regresatamarch()
        {
            return Archivo.asignarchivo.Length;
        }

        public static char[] convierteStringACharN(int tamaño, string nombre)
        {
            char[] nombreEntidad = new char[tamaño];
            for (int i = 0; i < tamaño; i++)
            {
                if (i < nombre.Length)
                    nombreEntidad[i] = nombre.ToCharArray()[i];
                else
                    nombreEntidad[i] = ' ';
            }
            return nombreEntidad;
        }
        //////////////////////////fin de entidades y atributos//////////////////////////////////7
        ///
        #region Atributos
        public void insertaAtributo(int idxEntidad, string nombre, char tipo, int indice, int longitud,long inter)
    {
        if (abierto)
        {
            CAtributo atributo = new CAtributo();
            atributo.asignombre = transformachar(nombre);
            atributo.asigdireccion =Archivo.asignarchivo.Length;
            atributo.tipoat = tipo;
            atributo.longitud = longitud;
            atributo.tipoind = indice;
            atributo.ind = inter;
            atributo.atrib = (long)-1;
            escribeRegistroAtributo(atributo);
            apuntaNuevoAtributo(idxEntidad, atributo.asigdireccion);
            lista_Atrib.Add(atributo);
        }
    }

        public void escribeRegistroAtributo(CAtributo registro)
        {
            BinaryWriter escritorBinario = new BinaryWriter(Archivo.asignarchivo);
            escritorBinario.BaseStream.Position = registro.asigdireccion;

            escritorBinario.Write(registro.asignombre);
            escritorBinario.Write(registro.asigdireccion);
            escritorBinario.Write(registro.tipoat);
            escritorBinario.Write(registro.longitud);
            escritorBinario.Write(registro.tipoind);
            escritorBinario.Write(registro.ind);
            escritorBinario.Write(registro.atrib);

        }

        public void apuntaNuevoAtributo(int idxEntidad, long direccionAtributo)
        {
            List<CEntidad> listaEntidades = dameRegistrosEntidad();

            long direccionAtri = listaEntidades[idxEntidad].atrib;
            //Si no apunta la dirección la cabecera apunta al nuevo atributo
            if (direccionAtri == -1)
            {
                escribeCabeceraAtributos(listaEntidades[idxEntidad].asigdireccion, direccionAtributo);
            }
            //Se busca el último atributo y apunta al nuevo atributo
            else
            {
                long direccion = direccionAtri;
                BinaryReader lectorBinario = new BinaryReader(Archivo.asignarchivo);

                while (direccion > 0)
                {
                    direccionAtri = direccion;
                    lectorBinario.BaseStream.Position = direccion + 55;
                    direccion = lectorBinario.ReadInt64();
                }
                escribeDireccionSiguienteAtributo(direccionAtri, direccionAtributo);
            }
        }
        public void escribeCabeceraAtributos(long direccion, long cabecera)
        {
            BinaryWriter escritorBinario = new BinaryWriter(Archivo.asignarchivo);
            escritorBinario.BaseStream.Position = (direccion + 38);
            escritorBinario.Write(cabecera);
        }
        public void escribeDireccionSiguienteAtributo(long direccion, long dir_siguiente)
        {
            BinaryWriter escritorBinario = new BinaryWriter(Archivo.asignarchivo);
            escritorBinario.BaseStream.Position = direccion + 55;
            escritorBinario.Write(dir_siguiente);
        }
         public List<CAtributo> dameRegistrosAtributo(long cabecera)
        {
            List<CAtributo> listaAtributos = new List<CAtributo>();
            BinaryReader lectorBinario = new BinaryReader(Archivo.asignarchivo);
            long direcicon = cabecera;
            if(cabecera>0)
            for (int i = 0; direcicon > 0; i++)
            {
                try
                {
                    lectorBinario.BaseStream.Position = direcicon;
                        listaAtributos.Add(new CAtributo());
                        listaAtributos[i].asignombre = System.Text.Encoding.UTF8.GetString(lectorBinario.ReadBytes(30)).ToCharArray();
                        listaAtributos[i].asigdireccion = lectorBinario.ReadInt64();
                        listaAtributos[i].tipoat = System.Convert.ToChar(lectorBinario.ReadByte());
                        listaAtributos[i].longitud = lectorBinario.ReadInt32();
                        listaAtributos[i].tipoind = lectorBinario.ReadInt32();
                        listaAtributos[i].ind = lectorBinario.ReadInt64();
                     //   listaAtributos[i].atrib = lectorBinario.ReadInt64();
                       //  long neq = listaAtributos[i].atrib;
                    direcicon = listaAtributos[i].atrib = lectorBinario.ReadInt64();
                        //int p =0;
                }
                catch
                {
                    direcicon = -1;
                }
            }
            return listaAtributos;
        }
        public void actualizaAtributo(int idxEntidad, int indexAtributo, string nombre, char tipo, int indice, int longitud,long indide)
        {
            List<CEntidad> listaEntidades = dameRegistrosEntidad();
            List<CAtributo> listaAtributos = dameRegistrosAtributo(listaEntidades[idxEntidad].atrib);
            CAtributo atributo = listaAtributos[indexAtributo];

            atributo.asignombre = transformachar(nombre);
            atributo.tipoat = tipo;
            atributo.longitud = longitud;
            atributo.ind = indide;
            atributo.tipoind = indice;

            escribeRegistroAtributo(atributo);
        }
        public void eliminarAtributo(int idxEntidad, int idxAtributo)
        {
            List<CEntidad> listaEntidades = dameRegistrosEntidad();
            List<CAtributo> listaAtributos = dameRegistrosAtributo(listaEntidades[idxEntidad].atrib);
            CAtributo atributo = listaAtributos[idxAtributo];

            if (idxAtributo == 0)
                escribeCabeceraAtributos(listaEntidades[idxEntidad].asigdireccion,
                                         atributo.atrib);
            else if (idxAtributo == listaAtributos.Count - 1)
                escribeDireccionSiguienteAtributo(listaAtributos[idxAtributo - 2].atrib, (long)-1);
            else
                escribeDireccionSiguienteAtributo(listaAtributos[idxAtributo - 2].atrib,
                                                  listaAtributos[idxAtributo].atrib);
        }

        public int verificaSiExisteIdxbusqueda(List<CAtributo> atributos)
        {
            int i = 0;
            foreach (CAtributo atributo in atributos)
            {
                if (atributo.tipoind == 1)
                    return i;
                i++;
            }
            return -1;
        }

        //es indice primario
        public int verificaSiExisteIdxPrimario(List<CAtributo> atributos)
        {
            int i = 0;
            foreach (CAtributo atributo in atributos)
            {
                if (atributo.tipoind == 2)
                    return i;
                i++;
            }
            return -1;
        }
        //es indice secundario
        public int existeIdxSecundario(List<CAtributo> atributos)
        {
            int i = 0;
            foreach (var item in atributos)
            {
                if (item.tipoind == 3)
                    return i;
                i++;
            }
            return -1;
        }
        public int existeIdxArbolPrimario(List<CAtributo> atributos)
        {
            int i = 0;
            foreach (var item in atributos)
            {
                if (item.tipoind == 4)
                    return i;
                i++;
            }
            return -1;
        }

        /*
        public void llenalistaatribut()
        {
            List<CAtributo> listaAtributos = new List<CAtributo>();
            BinaryReader lectorBinario = new BinaryReader(archivo);
            long direcicon = cab;
            for (int i = 0; direcicon > 0; i++)
            {
                try
                {
                    lectorBinario.BaseStream.Position = direcicon;
                    listaAtributos.Add(new CAtributo());
                    listaAtributos[i].dimeIdAtributo = lectorBinario.ReadBytes(5);
                    listaAtributos[i].dimeNombreAtributo = System.Text.Encoding.UTF8.GetString(lectorBinario.ReadBytes(35)).ToCharArray();
                    listaAtributos[i].dimeTipoDatoAtributo = System.Convert.ToChar(lectorBinario.ReadByte());
                    listaAtributos[i].dimeLongitudDatoAtributo = lectorBinario.ReadInt32();
                    listaAtributos[i].DimeDireccionAtributo = lectorBinario.ReadInt64();
                    listaAtributos[i].DimeTipoIndiceAtributo = lectorBinario.ReadInt32();
                    listaAtributos[i].dimeDireccionIndiceAtributo = lectorBinario.ReadInt64();
                    direcicon = listaAtributos[i].dimeDireccionSiguienteAtributo = lectorBinario.ReadInt64();
                }
                catch
                {
                    direcicon = -1;
                }
            }
        }
        */
        #endregion
        /////////////////////////////////////Fin Atributos////////////////////////////////////////////7
        ///
        #region datos
        public List<Registro> dameRegistrosDatos(CEntidad entidad)
        {
            List<Registro> listaRegistrosDatos = new List<Registro>();
            List<CAtributo> istaAtributos = dameRegistrosAtributo(entidad.atrib);

            abreArchivoDatos(entidad);
            long direccion = entidad.asigdat;
            BinaryReader reader = new BinaryReader(archivoDeDatos);
            reader.BaseStream.Position = 0;
            for (int i = 0; direccion >= 0; i++)
            {
                reader.BaseStream.Position = direccion;
                listaRegistrosDatos.Add(new Registro());
                listaRegistrosDatos[i].dimeDireccionRegstro = reader.ReadInt64();
                listaRegistrosDatos[i].dimeAtributosRegistro = new List<Byte[]>();
              
                for (int j = 0; j < istaAtributos.Count; j++)
                {
                    listaRegistrosDatos[i].dimeAtributosRegistro.Add(reader.ReadBytes(istaAtributos[j].longitud));
                }
    
                direccion = listaRegistrosDatos[i].dimeDireccionSiguienteRegstro = reader.ReadInt64();
            }
            cierraArchivoDatos();
            return listaRegistrosDatos;
        }

        public void abreArchivoDatos(CEntidad entidad)
        {
            archivoDeDatos = new FileStream(this.rutada + "\\" + new string(entidad.asignombre).Replace(" ", "") + ".dat", FileMode.Open, FileAccess.ReadWrite);
        }

        public void cierraArchivoDatos()
        {
            archivoDeDatos.Close();
        }

        public bool agregaRegistroDatos(int index, List<string> datos, Registro anterior)
        {
            CEntidad entidad = dameRegistrosEntidad()[index];
            archind = entidad;
            List<CAtributo> listaAtributos = dameRegistrosAtributo(entidad.atrib);
            Registro registro = null;
            CAtributo atributoPrimario;
            //Crea Archivo de Datos si es necesario
            if (entidad.asigdat == -1)
            {
                crearArchivoDatos(entidad);
            }

            //Crea el registro a insertar y checa si fue válido
            registro = creaRegistroDatos(entidad, datos, anterior);
            if (registro == null)
            {
                return false;
            }

            //Checa si en los atributos existe uno con índice primario, si sí entonces procede con sus acciones
            int idxAtrPrim = verificaSiExisteIdxPrimario(listaAtributos);
            int idxAtrSec = existeIdxSecundario(listaAtributos);
            int idxAtriArbolPrim = existeIdxArbolPrimario(listaAtributos);
         //   int idxAtriArbolSec = existeIdxArbolsecundario(listaAtributos);

            if (idxAtrPrim != -1)
            {
                 atributoPrimario = listaAtributos[idxAtrPrim];
                //Como sí existe índice primario, tenemos que garantizar que se cumplan sus condiciones
                if (condicionesIndPrimario(atributoPrimario, registro.dimeAtributosRegistro[idxAtrPrim]))
                {
                    if (idxAtriArbolPrim != -1)
                    {
                        CAtributo atributoArbolPrimario = listaAtributos[idxAtriArbolPrim];
                        if (!condicionesIndPrimarioArbol(atributoArbolPrimario, BitConverter.ToInt32(registro.dimeAtributosRegistro[idxAtriArbolPrim], 0),entidad))
                        {
                            return false;
                        }
                        accionesArbolPrimario(atributoArbolPrimario, registro, idxAtriArbolPrim);
                    }
                    //Si es así procedemos a escribir el registro y a terminar con las acciones al Arreglo 
                    accionesArregloPrimario(atributoPrimario, registro, idxAtrPrim);
                }
                else
                {
                    return false;
                }
            }
            else if (idxAtriArbolPrim != -1)
            {
                CAtributo atributoArbolPrimario = listaAtributos[idxAtriArbolPrim];
                if (condicionesIndPrimarioArbol(atributoArbolPrimario, BitConverter.ToInt32(registro.dimeAtributosRegistro[idxAtriArbolPrim], 0)))
                {
                    accionesArbolPrimario(atributoArbolPrimario, registro, idxAtriArbolPrim);
                }
                else
                {
                    return false;
                }
            }
            atributoPrimario = listaAtributos[idxAtrPrim];

            if (idxAtrSec != -1)
            {
                int conte = 0;int sec=0;
                long dir=0;
                for(int i=0; i <listaAtributos.Count;i++)
                {
                    if (listaAtributos[i].tipoind == 3)
                    {
                        if (listaAtributos[i].ind == -1)
                        { 
                        if (conte == 0)
                        {
                                dir += valordirec(atributoPrimario);
                            inicializaArchivoIndice(listaAtributos[i], dir);
                            escribeCabeceraIndice(listaAtributos[i].asigdireccion, dir);
                                conte++;
                        }
                            else
                            {
                                dir += valordirec(listaAtributos[i]);
                                inicializaArchivoIndice(listaAtributos[i], dir);
                                escribeCabeceraIndice(listaAtributos[i].asigdireccion, dir);
                            }
                        }
                    }

                }
                conte = 0;
                dir = 0;
                for (int i = 0; i < listaAtributos.Count; i++)
                {
                    if (listaAtributos[i].tipoind == 3)
                    {

                            if (conte == 0)
                             {
                            dir += valordirec(atributoPrimario);
                            conte++;
                            accionesArregloSecundario(listaAtributos[i], registro, sec, dir);
                            }
                            else
                        {
                            dir += valordirec(listaAtributos[i]);
                            accionesArregloSecundario(listaAtributos[i], registro, sec, dir);
                        }
                 
          
                    }
       sec++;
                }
                //    CAtributo atributoSecundario = listaAtributos[idxAtrSec];
                // accionesArregloSecundario(atributoSecundario, registro, idxAtrSec, atributoPrimario);
            }
            escribeRegistroDatos(entidad, registro);
            return true;
        }

        public bool agregaRegistroDatos(int index, List<string> datos, Registro anterior,int organi)
        {
            CEntidad entidad = dameRegistrosEntidad()[index];
            archind = entidad;
            List<CAtributo> listaAtributos = dameRegistrosAtributo(entidad.atrib);
            Registro registro = null;
            CAtributo atributoPrimario;
            List<Registro> listaRegistros = dameRegistrosDatos(entidad);
          //  listaRegistros = listaRegistros.OrderBy(x => x.dimeDireccionRegstro).ToList();
            //Crea Archivo de Datos si es necesario
            if (entidad.asigdat == -1)
            {
                crearArchivoDatos(entidad);
            }
            int ar = 0;
           /* foreach (var atributo in listaAtributos)
            {
                if(datos[ar].Length > atributo.longitud)
                    datos[ar] = datos[ar].Substring(0, atributo.longitud);
            }*/
                //Crea el registro a insertar y checa si fue válido
                registro = creaRegistroDatos(entidad, datos, anterior);
            if (registro == null)
            {
                return false;
            }
            int prim = verificaSiExisteIdxPrimario(listaAtributos);

            foreach (var item in listaRegistros)
            {
                int i = 0;
                foreach (var atributo in item.dimeAtributosRegistro)
                {
                    if (i == prim)
                    {
                        if (listaAtributos[prim].tipoat == 'E')
                        {
                            if (BitConverter.ToInt32(atributo, 0).ToString().Trim() == datos[prim])
                                return false;
                        }
                        if (listaAtributos[prim].tipoat == 'C')
                        {
                            if (Encoding.ASCII.GetString(atributo).Trim() == datos[prim])
                                return false;

                        }
                        if (listaAtributos[prim].tipoat == 'F')
                        {
                            if (BitConverter.ToDouble(atributo, 0).ToString().Trim() == datos[prim])
                                return false;
                        }

                    }
                        i++;
                }

            }
            switch(organi)
            {
                case 0:
                    escribeRegistroDatos(entidad, registro,1);
                    break;
                case 1:
                    int idxAtrPrim =  verificaSiExisteIdxPrimario(listaAtributos);
                 
                    int idxAtrSec = existeIdxSecundario(listaAtributos);
                    if (idxAtrPrim != -1)
                    {
                        atributoPrimario = listaAtributos[idxAtrPrim];
                        //Como sí existe índice primario, tenemos que garantizar que se cumplan sus condiciones
                        if (condicionesIndPrimario(atributoPrimario, registro.dimeAtributosRegistro[idxAtrPrim]))
                        {
                            /*if (idxAtriArbolPrim != -1)
                            {
                                CAtributo atributoArbolPrimario = listaAtributos[idxAtriArbolPrim];
                                if (!condicionesIndPrimarioArbol(atributoArbolPrimario, BitConverter.ToInt32(registro.dimeAtributosRegistro[idxAtriArbolPrim], 0), entidad))
                                {
                                    return false;
                                }
                                accionesArbolPrimario(atributoArbolPrimario, registro, idxAtriArbolPrim);
                            }*/
                            //Si es así procedemos a escribir el registro y a terminar con las acciones al Arreglo 
                            accionesArregloPrimario(atributoPrimario, registro, idxAtrPrim);
                        }
                        else
                        {
                            return false;
                        }
                        atributoPrimario = listaAtributos[idxAtrPrim];

                        if (idxAtrSec != -1)
                        {
                            int conte = 0; int sec = 0;
                            long dir = 0;
                            for (int i = 0; i < listaAtributos.Count; i++)
                            {
                                if (listaAtributos[i].tipoind == 3)
                                {
                                    if (listaAtributos[i].ind == -1)
                                    {
                                        if (conte == 0)
                                        {
                                            dir += valordirec(atributoPrimario);
                                            inicializaArchivoIndicesecu(listaAtributos[i], dir);
                                            escribeCabeceraIndice(listaAtributos[i].asigdireccion, dir);
                                            conte++;
                                        }
                                        else
                                        {
                                            dir += valordirec(listaAtributos[i]);
                                            inicializaArchivoIndicesecu(listaAtributos[i], dir);
                                            escribeCabeceraIndice(listaAtributos[i].asigdireccion, dir);
                                        }
                                    }  
                                }

                            }
                            conte = 0;
                            dir = 0;
                            for (int i = 0; i < listaAtributos.Count; i++)
                            {
                                if (listaAtributos[i].tipoind == 3)
                                {

                                    if (conte == 0)
                                    {
                                        dir += valordirec(atributoPrimario);
                                        conte++;
                                        accionesArregloSecundario(listaAtributos[i], registro, sec, dir);
                                    }
                                    else
                                    {
                                        dir += valordirec(listaAtributos[i]);
                                        accionesArregloSecundario(listaAtributos[i], registro, sec, dir);
                                    }


                                }
                                sec++;
                            }
                            //    CAtributo atributoSecundario = listaAtributos[idxAtrSec];
                            // accionesArregloSecundario(atributoSecundario, registro, idxAtrSec, atributoPrimario);
                        }
                    }
                    escribeRegistroDatos(entidad, registro, 2);
                    break;
                case 2:
                    int idxAtriArbolPrim = existeIdxArbolPrimario(listaAtributos);
          if (idxAtriArbolPrim != -1)
            {
                CAtributo atributoArbolPrimario = listaAtributos[idxAtriArbolPrim];
                if (condicionesIndPrimarioArbol(atributoArbolPrimario, BitConverter.ToInt32(registro.dimeAtributosRegistro[idxAtriArbolPrim], 0)))
                {
                    accionesArbolPrimario(atributoArbolPrimario, registro, idxAtriArbolPrim);
                            escribeRegistroDatos(entidad, registro, 4);
                        }
                else
                {
                    return false;
                }
            }
                    break;
            }
         return true;
        }


        public void crearArchivoDatos(CEntidad entidad)
        {
            string ruta = this.rutada + "\\" + new string(entidad.asignombre).Replace(" ", "") + ".dat";
            archivoDeDatos = new FileStream(ruta, FileMode.Create);
            archivoDeDatos.Close();
        }


     
        public Registro creaRegistroDatos(CEntidad entidad, List<string> datos, Registro anterior)
        {

            Registro registro = new Registro();
            List<Registro> listaRegistros = dameRegistrosDatos(entidad);
           
            List<CAtributo> listaAtributos = dameRegistrosAtributo(entidad.atrib);
            int i = 0;
            abreArchivoDatos(entidad);
            registro.dimeAtributosRegistro = new List<byte[]>();
            foreach (var item in listaAtributos)
            {
                if (item.tipoat == 'E')
                {
                    registro.dimeAtributosRegistro.Add(BitConverter.GetBytes(Convert.ToInt32(datos[i])));
                }
                if (item.tipoat == 'C')
                {
                    char[] temp = convierteStringACharN(item.longitud, datos[i]);
                    registro.dimeAtributosRegistro.Add(Encoding.ASCII.GetBytes(temp));
                }
                if (item.tipoat == 'F')
                {
                    registro.dimeAtributosRegistro.Add(BitConverter.GetBytes(Convert.ToDouble(datos[i])));
                }

                i++;
            }
            if (anterior == null)
                registro.dimeDireccionRegstro = listaRegistros.Count * registro.dameTamañoRegistro();//archivoDeDatos.Length;
            else
                registro.dimeDireccionRegstro = anterior.dimeDireccionRegstro;

            cierraArchivoDatos();

            registro.dimeDireccionSiguienteRegstro = -1;
            return registro;

        }
        public void escribeRegistroDatos(CEntidad entidad, Registro registro, int clave)
        {
            int dameClave = dameClavePrimaria(entidad,clave); //Indice del atributo de la clave de búsqueda
            registro.dimeDireccionSiguienteRegstro = dameSiguienteDireccionDatos(entidad, registro, dameClave);

            abreArchivoDatos(entidad);

            BinaryWriter escritorBinario = new BinaryWriter(archivoDeDatos);
            escritorBinario.BaseStream.Position = registro.dimeDireccionRegstro;
            escritorBinario.Write(registro.dimeDireccionRegstro);
            foreach (var atributo in registro.dimeAtributosRegistro)
            {
                escritorBinario.Write(atributo);
            }
            escritorBinario.Write(registro.dimeDireccionSiguienteRegstro);

            cierraArchivoDatos();
        }

        public void escribeRegistroDatos(CEntidad entidad, Registro registro)
             {
                int dameClave = dameClavePrimaria(entidad); //Indice del atributo de la clave de búsqueda
                registro.dimeDireccionSiguienteRegstro = dameSiguienteDireccionDatos(entidad, registro, dameClave);

                abreArchivoDatos(entidad);

                BinaryWriter escritorBinario = new BinaryWriter(archivoDeDatos);
                escritorBinario.BaseStream.Position = registro.dimeDireccionRegstro;
                escritorBinario.Write(registro.dimeDireccionRegstro);
                foreach (var atributo in registro.dimeAtributosRegistro)
                {
                    escritorBinario.Write(atributo);
                }
                escritorBinario.Write(registro.dimeDireccionSiguienteRegstro);

                cierraArchivoDatos();
            }
        public int dameClavePrimaria(CEntidad entidad)
        {
            int i = 0;
            foreach (CAtributo atributo in dameRegistrosAtributo(entidad.atrib))
            {
                if (atributo.tipoind ==2)
                    return i;
                i++;
            }
            return -1;
        }
        public int dameClavePrimaria(CEntidad entidad,int clave)
        {
            int i = 0;
            foreach (CAtributo atributo in dameRegistrosAtributo(entidad.atrib))
            {
                if (atributo.tipoind == clave)
                    return i;
                i++;
            }
            return -1;
        }
        public long dameSiguienteDireccionDatos(CEntidad entidad, Registro registro, int dameClavePrimar)
        {
            List<Registro> listaRegistros = dameRegistrosDatos(entidad);

            if (listaRegistros.Count == 0)
            {
                escribeCabeceraDatos(entidad.asigdireccion, registro.dimeDireccionRegstro);
                return -1;
            }
            else
            {
                CAtributo atributo;
                int i;
                if (dameClavePrimar != -1)
                {
                    atributo = dameRegistrosAtributo(entidad.atrib)[dameClavePrimar];
                    i = dameIndiceOrdenDatos(listaRegistros, dameClavePrimar, registro.dimeAtributosRegistro[dameClavePrimar], atributo.tipoat);
                }
                else
                {
                    i = -1;
                }
                if (i == 0)
                {
                    //Principio
                    escribeCabeceraDatos(entidad.asigdireccion, registro.dimeDireccionRegstro);
                    return listaRegistros[i].dimeDireccionRegstro;
                }
                else if (i == -1)
                {
                    //Final
                    escribeDirSigRegistro(entidad, listaRegistros[listaRegistros.Count -1].dimeDireccionRegstro,
                                                registro.dameTamañoRegistro(), registro.dimeDireccionRegstro);

                    return -1;
                }
                else
                {
                    //Intermedio
                    escribeDirSigRegistro(entidad, listaRegistros[i - 1].dimeDireccionRegstro,
                                            registro.dameTamañoRegistro(), registro.dimeDireccionRegstro);
                    return listaRegistros[i].dimeDireccionRegstro;
                }
            }
        }

        public int dameIndiceOrdenDatos(List<Registro> datos, int claveBusqueda, byte[] insertar, char tipo)
        {
            for (int i = 0; i < datos.Count; i++)
            {
                if (tipo == 'C')
                {
                    string num1 = Encoding.ASCII.GetString(insertar);
                    string num2 = Encoding.ASCII.GetString(datos[i].dimeAtributosRegistro[claveBusqueda]);
                    if (string.Compare(num1, num2) <= 0)
                    {
                        return i;
                    }
                }
                if (tipo == 'E')
                {
                    int num1 = BitConverter.ToInt32(insertar, 0);
                    int num2 = BitConverter.ToInt32(datos[i].dimeAtributosRegistro[claveBusqueda], 0);
                    if (num1 <= num2)
                    {
                        return i;
                    }
                }
                if (tipo == 'F')
                {
                    Double num1 = BitConverter.ToDouble(insertar, 0);
                    Double num2 = BitConverter.ToDouble(datos[i].dimeAtributosRegistro[claveBusqueda], 0);
                    if (num1 <= num2)
                    {
                        return i;
                    }
                }

            }
            return -1;
        }
        public void escribeCabeceraDatos(long direccion, long direccionDatos)
        {
            BinaryWriter escritorBinario = new BinaryWriter(Archivo.asignarchivo);
            escritorBinario.BaseStream.Position = direccion + 46;
            escritorBinario.Write(direccionDatos);
        }
        public void escribeDirSigRegistro(CEntidad entidad, long direccion, int tam, long siguienteDireccion)
        {
            abreArchivoDatos(entidad);
            BinaryWriter escritorBinario = new BinaryWriter(archivoDeDatos);
            escritorBinario.BaseStream.Position = direccion + tam - 8;
            escritorBinario.Write(siguienteDireccion);
            cierraArchivoDatos();
        }


        public void modificaRegistroDatos(int idxEntidad, int idxRegistro, List<string> datos)
        {
            CEntidad entidad = dameRegistrosEntidad()[idxEntidad];
            List<Registro> listaRegistros = dameRegistrosDatos(entidad);
          //  listaRegistros = listaRegistros.OrderBy(x => x.dimeDireccionRegstro).ToList();
            List<CAtributo> atributos = dameRegistrosAtributo(entidad.atrib);
            Registro registro = listaRegistros[idxRegistro];
            eliminaRegistroDatos(idxEntidad, idxRegistro);
            agregaRegistroDatos(idxEntidad, datos, registro);
        }
        public bool modificaRegistroDatos(int idxEntidad, int idxRegistro, List<string> datos,int organi)
        {

            CEntidad entidad = dameRegistrosEntidad()[idxEntidad];
            List<Registro> listaRegistros = dameRegistrosDatos(entidad);
           // listaRegistros = listaRegistros.OrderBy(x => x.dimeDireccionRegstro).ToList();
            List<CAtributo> atributos = dameRegistrosAtributo(entidad.atrib);
          int prim=  verificaSiExisteIdxPrimario(atributos);
            Registro registro = listaRegistros[idxRegistro];
            eliminaRegistroDatos(idxEntidad, idxRegistro, organi);
            agregaRegistroDatos(idxEntidad, datos, registro, organi);
            int j = 0;


            return true;
        }
        public void eliminaRegistroDatos(int idxEntidad, int idxRegistro,int ogani)
        {
            CEntidad entidad = dameRegistrosEntidad()[idxEntidad];
            List<CAtributo> listaAtributos = dameRegistrosAtributo(entidad.atrib);
            List<Registro> listaRegistros = dameRegistrosDatos(entidad);

            archind = entidad;

            Registro registro = null;
            CAtributo atributoPrimario;

            //  listaRegistros = listaRegistros.OrderBy(x => x.dimeDireccionRegstro).ToList();
            switch (ogani)
            {
                case 0:
                    if (idxRegistro == 0)
                    {
                        abreArchivoDatos(entidad);
                        escribeCabeceraDatos(entidad.asigdireccion, listaRegistros[idxRegistro].dimeDireccionSiguienteRegstro);
                        cierraArchivoDatos();
                    }
                    else if (idxRegistro == listaRegistros.Count - 1)
                        escribeDirSigRegistro(entidad, listaRegistros[idxRegistro - 1].dimeDireccionRegstro,
                                                    listaRegistros[idxRegistro].dameTamañoRegistro(), -1);
                    else
                        escribeDirSigRegistro(entidad, listaRegistros[idxRegistro - 1].dimeDireccionRegstro,
                                                    listaRegistros[idxRegistro].dameTamañoRegistro(), listaRegistros[idxRegistro].dimeDireccionSiguienteRegstro);

                    break;
                case 1:

                    //índice Primario
                    int idxAtrPrim = verificaSiExisteIdxPrimario(listaAtributos);
                    if (idxAtrPrim != -1)
                    {
                        eliminaDeIndicePrimario(listaAtributos[idxAtrPrim], listaRegistros[idxRegistro].dimeAtributosRegistro[idxAtrPrim]);
                    }

                    //índice Secundario
                    int idxAtrSec = existeIdxSecundario(listaAtributos);
                    if (idxAtrSec != -1)
                    {
                        atributoPrimario = listaAtributos[idxAtrPrim];

                        int conte = 0, sec = 0 ;
                    long    dir = 0;
                        for (int i = 0; i < listaAtributos.Count; i++)
                        {
                            if (listaAtributos[i].tipoind == 3)
                            {

                                if (conte == 0)
                                {
                                    dir += valordirec(atributoPrimario);
                                    conte++;
                                   // accionesArregloSecundario(listaAtributos[i], registro, sec, dir);
eliminaIndiceSecundario(listaAtributos[i], listaRegistros[idxRegistro].dimeAtributosRegistro[i], dir, listaRegistros[idxRegistro].dimeDireccionRegstro);

                                }
                                else
                                {
                                    dir += valordirec(listaAtributos[i]);
                               //     accionesArregloSecundario(listaAtributos[i], registro, sec, dir);
    eliminaIndiceSecundario(listaAtributos[i], listaRegistros[idxRegistro].dimeAtributosRegistro[i], dir, listaRegistros[idxRegistro].dimeDireccionRegstro);

                                }
                            }
                            sec++;
                        }



                    }


                    if (idxRegistro == 0)
                    {
                        abreArchivoDatos(entidad);
                        escribeCabeceraDatos(entidad.asigdireccion, listaRegistros[idxRegistro].dimeDireccionSiguienteRegstro);
                        cierraArchivoDatos();
                    }
                    else if (idxRegistro == listaRegistros.Count - 1)
                        escribeDirSigRegistro(entidad, listaRegistros[idxRegistro - 1].dimeDireccionRegstro,
                                                    listaRegistros[idxRegistro].dameTamañoRegistro(), -1);
                    else
                        escribeDirSigRegistro(entidad, listaRegistros[idxRegistro - 1].dimeDireccionRegstro,
                                                    listaRegistros[idxRegistro].dameTamañoRegistro(), listaRegistros[idxRegistro].dimeDireccionSiguienteRegstro);
                    break;
                case 2:
            int idxAtrArbolPrim = existeIdxArbolPrimario(listaAtributos);
            if (idxAtrArbolPrim != -1)
            {
                eliminaArbolPrimario(listaAtributos[idxAtrArbolPrim], BitConverter.ToInt32(listaRegistros[idxRegistro].
                    dimeAtributosRegistro[idxAtrArbolPrim], 0));
            }
                    if (idxRegistro == 0)
                    {
                        abreArchivoDatos(entidad);
                        escribeCabeceraDatos(entidad.asigdireccion, listaRegistros[idxRegistro].dimeDireccionSiguienteRegstro);
                        cierraArchivoDatos();
                    }
                    else if (idxRegistro == listaRegistros.Count - 1)
                        escribeDirSigRegistro(entidad, listaRegistros[idxRegistro - 1].dimeDireccionRegstro,
                                                    listaRegistros[idxRegistro].dameTamañoRegistro(), -1);
                    else
                        escribeDirSigRegistro(entidad, listaRegistros[idxRegistro - 1].dimeDireccionRegstro,
                                                    listaRegistros[idxRegistro].dameTamañoRegistro(), listaRegistros[idxRegistro].dimeDireccionSiguienteRegstro);

                    break;
            }
  }

        public void eliminaRegistroDatos(int idxEntidad, int idxRegistro)
        {
            CEntidad entidad = dameRegistrosEntidad()[idxEntidad];
            List<CAtributo> listaAtributos = dameRegistrosAtributo(entidad.atrib);
            List<Registro> listaRegistros = dameRegistrosDatos(entidad);

            listaRegistros= listaRegistros.OrderBy(x => x.dimeDireccionRegstro).ToList();
            //índice Primario
            int idxAtrPrim = verificaSiExisteIdxPrimario(listaAtributos);
            if (idxAtrPrim != -1)
            {
                eliminaDeIndicePrimario(listaAtributos[idxAtrPrim], listaRegistros[idxRegistro].
                    dimeAtributosRegistro[idxAtrPrim]);
            }

            //índice Secundario
            int idxAtrSec = existeIdxSecundario(listaAtributos);
            if (idxAtrSec != -1)
            {
          //      eliminaIndiceSecundario(listaAtributos[idxAtrSec], listaRegistros[idxRegistro].
            //        dimeAtributosRegistro[idxAtrSec], listaRegistros[idxRegistro].dimeDireccionRegstro);
            }

            // Árbol Primario
            int idxAtrArbolPrim = existeIdxArbolPrimario(listaAtributos);
            if (idxAtrArbolPrim != -1)
            {
                eliminaArbolPrimario(listaAtributos[idxAtrArbolPrim], BitConverter.ToInt32(listaRegistros[idxRegistro].
                    dimeAtributosRegistro[idxAtrArbolPrim], 0));
            }

            //índice Secundario
            /*int idxAtrArbolSec = existeIdxArbolsecundario(listaAtributos);
            if (idxAtrArbolSec != -1)
            {
                eliminaArbolSecundario(listaAtributos[idxAtrArbolSec], BitConverter.ToInt32(listaRegistros[idxRegistro].
                    dimeAtributosRegistro[idxAtrArbolSec], 0), listaRegistros[idxRegistro].dimeDireccionRegstro);
            }*/

            // Registros de Datos
            if (idxRegistro == 0)
                escribeCabeceraDatos(entidad.asigdireccion, listaRegistros[idxRegistro].dimeDireccionSiguienteRegstro);
            else if (idxRegistro == listaRegistros.Count - 1)
                escribeDirSigRegistro(entidad, listaRegistros[idxRegistro - 1].dimeDireccionRegstro,
                                            listaRegistros[idxRegistro].dameTamañoRegistro(), -1);
            else
                escribeDirSigRegistro(entidad, listaRegistros[idxRegistro - 1].dimeDireccionRegstro,
                                            listaRegistros[idxRegistro].dameTamañoRegistro(), listaRegistros[idxRegistro].dimeDireccionSiguienteRegstro);
        }

        #endregion



        #region indices

        private CEntidad archind;
        private long dirarchinid;
        private int primario = 0;
        /*índice Primario*/
        public bool condicionesIndPrimario(CAtributo atributo, byte[] datoIndice)
        {
            Dictionary<byte[], long> arregloPrim = new Dictionary<byte[], long>();
            
            //Si no tiene registros se crea el archivo y regresamos
            if (atributo.ind == -1)
            {
                crearArchivoIndice();
                inicializaArchivoIndice(atributo, 0);

                //Escribimos la cabecera del atributo en 0 para que apunte al primero
                escribeCabeceraIndice(atributo.asigdireccion, 0);
                return true;
            }
            //Si sí hay datos la condición tiene que cumplir es que el arreglo no tenga el dato
            else
            {
                arregloPrim = dameArreglo(atributo);
                if (arregloPrim.ContainsKey(datoIndice))
                {
                    return false;
                }
                return true;
            }
        }
        public void inicializaArchivoIndice(CAtributo atributo, long direccion)
        {
            abreArchivoIndice();
            BinaryWriter escritorBinario = new BinaryWriter(archivoIndice);
            int numero = calculaNumeroRegistro(atributo);
            int numeroBytes = atributo.longitud;
            byte[] bytes;

            escritorBinario.BaseStream.Position = direccion;
            for (int i = 0; i < numero; i++)
            {
                bytes = new byte[numeroBytes];
                new Random().NextBytes(bytes);
                escritorBinario.Write(bytes);
                escritorBinario.Write((long)-1);
            }
    //        escritorBinario.BaseStream.Position = tamañoBloque - 8;
            escritorBinario.Write((long)-1);
            if (primario == 0)
            {
                dirarchinid = archivoIndice.Length;
                primario = 1;
            }
            cierraArchivoIndice();
        }
        public void asigarchi(CEntidad ent)
        {
            archind = ent;
        //    dirarchinid = ent.atrib;
        }


        public void crearArchivoIndice()
        {

             string ruta = this.rutada + "\\" + new string(archind.asignombre).Replace(" ", "") + ".idx";
           // string ruta = dameRutaArchivoIndice(atributo.dimeIdAtributo);
            archivoIndice = new FileStream(ruta, FileMode.Create);
            archivoIndice.Close();
        }
        public void abreArchivoIndice()
        {
            archivoIndice = new FileStream(this.rutada + "\\" + new string(archind.asignombre).Replace(" ", "") + ".idx", FileMode.Open, FileAccess.ReadWrite);
        }
        public void cierraArchivoIndice()
        {
            archivoIndice.Close();
        }


        public void escribeCabeceraIndice(long direccion, long cabecera)
        {
            BinaryWriter escritorBinario = new BinaryWriter(Archivo.asignarchivo);
            escritorBinario.BaseStream.Position = direccion + 47;
            escritorBinario.Write(cabecera);
        }


        public void accionesArregloPrimario(CAtributo atributo, Registro registro, int idxAtributoPrim)
        {
            Dictionary<byte[], long> arregloPrim = dameArreglo(atributo);

            //Insertar en Diccionario
            arregloPrim.Add(registro.dimeAtributosRegistro[idxAtributoPrim], registro.dimeDireccionRegstro);
            arregloPrim = ordenaArreglo(arregloPrim);
            //Escribir en Archivo
            escribeArreglo(atributo, arregloPrim,(long)0);
        }
        public void escribeArreglo(CAtributo atributo, Dictionary<byte[], long> arreglo,long dir)
        {
            inicializaArchivoIndice(atributo, dir);
            abreArchivoIndice();
            BinaryWriter escritorBinario = new BinaryWriter(archivoIndice);
            escritorBinario.BaseStream.Position = dir;
            foreach (var item in arreglo)
            {
                escritorBinario.Write(item.Key);
                escritorBinario.Write(item.Value);
            }
        //    escritorBinario.BaseStream.Position = tamañoBloque - 8;
            escritorBinario.Write((long)-1);
            cierraArchivoIndice();
        }



        /// ///////////////////////////////////////7777obtine el contenido del archivo//////////////////////////////////////

        public Dictionary<byte[], long> dameArreglo(CAtributo atributo)
        {
            abreArchivoIndice();
            Dictionary<byte[], long> arreglo = new Dictionary<byte[], long>();
            BinaryReader lectorBinario = new BinaryReader(archivoIndice);
            int numeroRegistros = calculaNumeroRegistro(atributo);
            lectorBinario.BaseStream.Position = 0;
            for (int i = 0; i < numeroRegistros; i++)
            {
                byte[] bytes = lectorBinario.ReadBytes(atributo.longitud);
                long direccion = lectorBinario.ReadInt64();
                if (direccion == -1)
                    break;
                arreglo.Add(bytes, direccion);
            }
            cierraArchivoIndice();
            return arreglo;

        }
        public Dictionary<byte[], long> dameArreglo(CAtributo atributo,long dir)
        {

            Dictionary<byte[], long> arreglo = new Dictionary<byte[], long>();
            try
            {
                abreArchivoIndice();
                BinaryReader lectorBinario = new BinaryReader(archivoIndice);
                int numeroRegistros = calculaNumeroRegistro(atributo);
                lectorBinario.BaseStream.Position = dir;
                for (int i = 0; i < numeroRegistros; i++)
                {
                    byte[] bytes = lectorBinario.ReadBytes(atributo.longitud);
                    long direccion = lectorBinario.ReadInt64();
                    if (direccion == -1)
                        break;
                    arreglo.Add(bytes, direccion);
                }
                cierraArchivoIndice();
                return arreglo;
            }
            catch (Exception e) { }
            return arreglo;
        }
        public Dictionary<byte[], long> ordenaArreglo(Dictionary<byte[], long> arreglo)
        {
            List<byte[]> indices = new List<byte[]>();
            foreach (var item in arreglo)
                indices.Add(item.Key);
            indices.Sort(new ByteArrayComparer1());

            Dictionary<byte[], long> nuevo = new Dictionary<byte[], long>();
            foreach (var item in indices)
            {
                nuevo.Add(item, arreglo[item]);
            }
            return nuevo;
        }

        public int calculaNumeroRegistro(CAtributo atributo)
        {
            return (tamañoBloque - 8) / (atributo.longitud + 8);
        }
///)///////////////////////////////////////////////////////////////////////////////////////////////////////////
        /*Índice Secundario*////)///////////////////////////////////////////////////////////////////////
        public bool accionesArregloSecundario(CAtributo atributo, Registro registro, int idxAtributo,long dir)
        {

            List<CAtributo> listaAtributos = dameRegistrosAtributo(archind.atrib);
            CAtributo atributoSecundario = listaAtributos[idxAtributo];
            Dictionary<byte[], long> arreglo = dameArreglo(atributoSecundario, atributoSecundario.ind);///////////////
            byte[] datoSecundario = registro.dimeAtributosRegistro[idxAtributo];
            bool existe=false;
            long dire=0;
            foreach (var item in arreglo)
            {
                if (atributo.tipoat == 'E')
                {
                    string x = BitConverter.ToInt32(item.Key, 0).ToString().Trim();
                    if (BitConverter.ToInt32(datoSecundario, 0).ToString().Trim() == BitConverter.ToInt32(item.Key, 0).ToString().Trim())
                    {
                        existe = true;
                        dire = item.Value;
                        break;
                    }
                }
                else
                {
                    string y = Encoding.ASCII.GetString(item.Key).Trim();
                    string y2 = Encoding.ASCII.GetString(datoSecundario).Trim();
                    if (Encoding.ASCII.GetString(item.Key).Trim()== Encoding.ASCII.GetString(datoSecundario).Trim())
                    {
                        existe = true;
                        dire = item.Value;
                        break;
                    }


                }
                if (item.Key== datoSecundario)
                {
                    int k=0;

                }
            }
            if (existe)
            {
                //Sólo inserta la dirección del registro en el bloque      [datoSecundario]
                List<long> bloqueDirecciones = dameBloqueDeDirecciones(atributo, dire);
                bloqueDirecciones.Add(registro.dimeDireccionRegstro);
                bloqueDirecciones.Sort();
                escribeBloqueDeDirecciones(atributo, bloqueDirecciones, dire);
            }
            else
            {
                //Crea nuevo bloque denso con la dirección del registro dentro
                long direccionBloque = dameTamañoArchivoIndice(atributo);
                inicializaBloqueDirecciones(atributo, direccionBloque);
                List<long> bloqueDirecciones = dameBloqueDeDirecciones(atributo, direccionBloque);
                bloqueDirecciones.Add(registro.dimeDireccionRegstro);
                escribeBloqueDeDirecciones(atributo, bloqueDirecciones, direccionBloque);

                //Insertar en Diccionario
                arreglo.Add(registro.dimeAtributosRegistro[idxAtributo], direccionBloque);
                //Escribir en Archivo
                escribeArreglo(atributo, arreglo, dir);
            }
            return true;
        }
        public long valordirec(CAtributo atributo)
        {
            int numero = calculaNumeroRegistro(atributo);
            int numeroBytes = atributo.longitud;
            int calc = numeroBytes + 8;
            int final = (calc * numero)+8;


            return final;
        }
        public void inicializaArchivoIndicesecu(CAtributo atributo, long direccion)
        {
            abreArchivoIndice();
            BinaryWriter escritorBinario = new BinaryWriter(archivoIndice);
            int numero = calculaNumeroRegistro(atributo);
            int numeroBytes = atributo.longitud;
            byte[] bytes;

            escritorBinario.BaseStream.Position = direccion;
            for (int i = 0; i < numero; i++)
            {
                bytes = new byte[numeroBytes];
               // new Random().NextBytes(bytes);
                escritorBinario.Write((bytes));
                escritorBinario.Write((long)-1);
            }
            //        escritorBinario.BaseStream.Position = tamañoBloque - 8;
            escritorBinario.Write((long)-1);
            if (primario == 0)
            {
                dirarchinid = archivoIndice.Length;
                primario = 1;
            }
            cierraArchivoIndice();
        }

        public List<long> dameBloqueDeDirecciones(CAtributo atributo, long cabecera)
        {
            List<long> listaDirecciones = new List<long>();
            abreArchivoIndice();
            BinaryReader lectorBinario = new BinaryReader(archivoIndice);
            lectorBinario.BaseStream.Position = cabecera;
            int numeroRegistros = calculaNumeroRegistro(atributo);
            for (int i = 0; i < numeroRegistros; i++)
            {
          //      byte[] bytes = lectorBinario.ReadBytes(atributo.longitud);
                long direccion = lectorBinario.ReadInt64();
                if (direccion != -1)
                    listaDirecciones.Add(direccion);
            }
            cierraArchivoIndice();
            return listaDirecciones;
        }
        public List<long> dameBloqueDeDireccionessecu(CAtributo atributo, long cabecera)
        {
            List<long> listaDirecciones = new List<long>();
            abreArchivoIndice();
            Dictionary<byte[], long> arregloPrim = dameArreglo(atributo,cabecera);//////////////////////////
            BinaryReader lectorBinario = new BinaryReader(archivoIndice);
            lectorBinario.BaseStream.Position = cabecera;
            int numeroRegistros = calculaNumeroRegistro(atributo);
            for (int i = 0; i < numeroRegistros; i++)
            {
                byte[] x = lectorBinario.ReadBytes(atributo.longitud);
                long direccion = lectorBinario.ReadInt64();
                if (direccion != -1)
                    listaDirecciones.Add(direccion);
            }
            cierraArchivoIndice();
            return listaDirecciones;
        }
        public void escribeBloqueDeDirecciones(CAtributo atributo, List<long> direcciones, long cabecera)
        {
            inicializaBloqueDirecciones(atributo, cabecera);
            abreArchivoIndice();
            BinaryWriter escritorBinario = new BinaryWriter(archivoIndice);
            escritorBinario.BaseStream.Position = cabecera;
            foreach (var item in direcciones)
                escritorBinario.Write(item);
            cierraArchivoIndice();
        }
        public void inicializaBloqueDirecciones(CAtributo atributo, long direccion)
        {
            abreArchivoIndice();

            BinaryWriter escritorBinario = new BinaryWriter(archivoIndice);
            escritorBinario.BaseStream.Position = direccion;
            for (int i = 0; i < numeroRegistros; i++)
            {
                escritorBinario.Write((long)-1);
            }
            escritorBinario.BaseStream.Position = direccion + tamañoBloque - 8;
            escritorBinario.Write((long)-1);
            cierraArchivoIndice();
        }
        public long dameTamañoArchivoIndice(CAtributo atributo)
        {
            abreArchivoIndice();
            long tamaño = archivoIndice.Length;
            
            cierraArchivoIndice();
            return tamaño;
        }
     //////////////////////////////////////////////////////////////////////////////////////////////////////////////////  
        
        
        /// eliminacionea)/////////////////////////////////////////////////////////////////////////////////////////////
        public void eliminaDeIndicePrimario(CAtributo atributo, byte[] llave)
        {
            Dictionary<byte[], long> arreglo = dameArreglo(atributo,(long)0);
            Dictionary<byte[], long> arregl = new Dictionary<byte[], long>();
            arreglo.Remove(llave);
            foreach (var item in arreglo)
            {
                if (atributo.tipoat == 'E')
                {
                    string x = BitConverter.ToInt32(item.Key, 0).ToString().Trim();
                        int p = BitConverter.ToInt32(item.Key, 0);
                    int p2 = BitConverter.ToInt32(llave, 0);
                    if(p2!=p)
                    {
                    byte[] intBytes = BitConverter.GetBytes(p);

                    byte[] result = intBytes;
                        arregl.Add(result,item.Value);
                    }

                }
                else
                {
                    string y = Encoding.ASCII.GetString(item.Key).Trim();
                    string y2 = Encoding.ASCII.GetString(llave).Trim();
                    if (y2 != y)
                    {
                        arregl.Add(Encoding.ASCII.GetBytes(y), item.Value);
                    }
                    byte[] intBytes = Encoding.ASCII.GetBytes(y);

                }

            }


            if (arregl.Count == 0)
            {
                escribeCabeceraIndice(atributo.asigdireccion, (long)-1);
            }
            escribeArreglo(atributo, arregl, (long)0);
        }

        public void eliminaIndiceSecundario(CAtributo atributo, byte[] llave, long direccion,long direccions)
        {
           // int p2 = BitConverter.ToInt32(llave, 0);
            Dictionary<byte[], long> arreglo = dameArreglo(atributo,atributo.ind);
            Dictionary<byte[], long> arregl = new Dictionary<byte[], long>();
            bool existe = false;
            long dire = 0;
            foreach (var item in arreglo)
            {
                if (atributo.tipoat == 'E')
                {
                    //string x = BitConverter.ToInt32(item.Key, 0).ToString().Trim();
                    if (BitConverter.ToInt32(llave, 0).ToString().Trim() == BitConverter.ToInt32(item.Key, 0).ToString().Trim())
                    {
                        existe = true;
                        dire = item.Value;
                        break;
                    }
                }
                else
                {
                    string y = Encoding.ASCII.GetString(item.Key).Trim();
                    string y2 = Encoding.ASCII.GetString(llave).Trim();
                    if (Encoding.ASCII.GetString(item.Key).Trim() == Encoding.ASCII.GetString(llave).Trim())
                    {
                        existe = true;
                        dire = item.Value;
                        break;
                    }


                }
            }
                List<long> denso = dameBloqueDeDirecciones(atributo, dire);


            denso.Remove(direccions);
            if (denso.Count == 0)
            {
                foreach (var item in arreglo)
                {
                    if (atributo.tipoat == 'E')
                    {
                        string x = BitConverter.ToInt32(item.Key, 0).ToString().Trim();
                        int p = BitConverter.ToInt32(item.Key, 0);
                      int   p2 = BitConverter.ToInt32(llave, 0);
                        if (p2 != p)
                        {
                            byte[] intBytes = BitConverter.GetBytes(p);

                            byte[] result = intBytes;
                            arregl.Add(result, item.Value);
                        }

                    }
                    else
                    {
                        string y = Encoding.ASCII.GetString(item.Key).Trim();
                        string y2 = Encoding.ASCII.GetString(llave).Trim();
                        if (y2 != y)
                        {
                            arregl.Add(Encoding.ASCII.GetBytes(y), item.Value);
                        }
                        byte[] intBytes = Encoding.ASCII.GetBytes(y);

                    }

                }

              arreglo.Remove(llave);
                if (arregl.Count == 0)
                {
                    escribeCabeceraIndice(atributo.asigdireccion, -1);
                    escribeArreglo(atributo, arregl, direccion);
                    cierraArchivoIndice();
                    return;
                }
               escribeArreglo(atributo, arregl, direccion);
                return;
            }
            escribeBloqueDeDirecciones(atributo, denso, dire);
          //  escribeArreglo(atributo, arreglo,dire);
        }


        #region arbol
        /////////////////////////////////////////  /*Árbol Primario*////////////////////////////////////
        ///        /*indice Primario*/
        public bool condicionesIndPrimarioArbol(CAtributo atributoArbolPrim, int datoIndice, CEntidad ent)
        {
            //Si no tiene registros insertados, sólo creamos el archivo y nos regresamos
            if (atributoArbolPrim.ind == -1)
            {
                // crearArchivoIndice(atributoArbolPrim);
                crearArchivoIndice();
                //Escribimos la cabecera del atributo en 0 para que apunte al primero
                escribeCabeceraIndice(atributoArbolPrim.asigdireccion, 0);
                return true;
            }
            //Si sí hay datos la única condición que tiene que cumplir es que el arreglo no tenga el dato
            else
            {
                Arbol arbol = new Arbol(dameNodos(atributoArbolPrim), atributoArbolPrim);
                if (arbol.contieneClave(datoIndice))
                {
                    return false;
                }
                return true;
            }
        }
        public bool condicionesIndPrimarioArbol(CAtributo atributoArbolPrim, int datoIndice)
        {
            //Si no tiene registros insertados, sólo creamos el archivo y nos regresamos
            if (atributoArbolPrim.atrib == -1)
            {
                crearArchivoIndice();

                //Escribimos la cabecera del atributo en 0 para que apunte al primero
                escribeCabeceraIndice(atributoArbolPrim.atrib, 0);
                return true;
            }
            //Si sí hay datos la única condición que tiene que cumplir es que el arreglo no tenga el dato
            else
            {
                Arbol arbol = new Arbol(dameNodos(atributoArbolPrim), atributoArbolPrim);
                if (arbol.contieneClave(datoIndice))
                {
                    return false;
                }
                return true;
            }
        }
        //Función que elimina de árbol primario
        public void eliminaArbolPrimario(CAtributo atributo, int dato)
        {
            Arbol arbolPrim = new Arbol(dameNodos(atributo), atributo);
            Nodo nodo = arbolPrim.dameNodoDeLaLlave(dato);
            long direccion = nodo.dameApuntadorHoja(dato);
            eliminaDeArbol(arbolPrim, nodo, dato, direccion);
        }
        //Eliminación en el árbol
        bool eliminaDeArbol(Arbol arbol, Nodo nodo, int dato, long direccion)
        {
            int tamañoMinimo = (gradoArbol - 1) / 2;
            char tipo = nodo.tipo;

            if (tipo == 'H')
            {
                if (!nodo.eliminaEnHoja(dato))
                    return false;
            }
            else
            {
                if (!nodo.eliminaNodoRaizOIntermedio(dato, direccion))
                    return false;
            }

            escribeNodoArchivo(arbol.atributo, nodo);

            if (tipo != 'R')
            {
                if (nodo.numeroClaves() < tamañoMinimo)
                {
                    Nodo padre = arbol.dameNodoPadre(nodo);
                    Nodo vecinoDerecho = arbol.dameNodoVecinoDerecho(nodo);
                    Nodo vecinoIzquierdo = arbol.dameNodoVecinoIzquierdo(nodo);

                    if (vecinoDerecho != null && arbol.tieneMismoNodoPadre(nodo, vecinoDerecho) && vecinoDerecho.numeroClaves() - 1 >= tamañoMinimo)
                    {
                        if (tipo == 'H')
                        {
                            long direccionDePrestado = vecinoDerecho.listaDirecciones[0];
                            int claveBusquedaDePrestado = vecinoDerecho.listaClaves[0];

                            if (!vecinoDerecho.eliminaEnHoja(claveBusquedaDePrestado))
                                return false;
                            escribeNodoArchivo(arbol.atributo, vecinoDerecho);

                            nodo.insertaEnHoja(claveBusquedaDePrestado, direccionDePrestado);
                            escribeNodoArchivo(arbol.atributo, nodo);

                            int idxActualizarPadre = padre.listaDirecciones.IndexOf(nodo.direccion);
                            padre.listaClaves[idxActualizarPadre] = vecinoDerecho.listaClaves[0];
                            escribeNodoArchivo(arbol.atributo, padre);
                        }
                        else
                        {
                            long direccionVecino = vecinoDerecho.listaDirecciones[0];
                            int claveBusquedaVecino = vecinoDerecho.listaClaves[0];
                            int idxClaveBusquedaPadre = padre.listaDirecciones.IndexOf(nodo.direccion);
                            int claveBusquedaPadre = padre.listaClaves[idxClaveBusquedaPadre];

                            if (!vecinoDerecho.eliminaNodoRaizOIntermedio(claveBusquedaVecino, direccionVecino))
                                return false;
                            escribeNodoArchivo(arbol.atributo, vecinoDerecho);

                            padre.listaClaves[idxClaveBusquedaPadre] = claveBusquedaVecino;
                            escribeNodoArchivo(arbol.atributo, padre);

                            nodo.insertaEnRaizOIntermedio(claveBusquedaPadre, direccionVecino);
                            escribeNodoArchivo(arbol.atributo, nodo);
                        }
                    }
                    else if (vecinoIzquierdo != null && arbol.tieneMismoNodoPadre(nodo, vecinoIzquierdo) && vecinoIzquierdo.numeroClaves() - 1 >= tamañoMinimo)
                    {
                        if (tipo == 'H')
                        {
                            long direccionDePrestado = vecinoIzquierdo.listaDirecciones[vecinoIzquierdo.numeroClaves() - 1];
                            int claveBusquedaDePrestado = vecinoIzquierdo.listaClaves[vecinoIzquierdo.numeroClaves() - 1];

                            if (!vecinoIzquierdo.eliminaEnHoja(claveBusquedaDePrestado))
                                return false;
                            escribeNodoArchivo(arbol.atributo, vecinoIzquierdo);

                            nodo.insertaEnHoja(claveBusquedaDePrestado, direccionDePrestado);
                            escribeNodoArchivo(arbol.atributo, nodo);

                            int idxActualizarPadre = padre.listaDirecciones.IndexOf(vecinoIzquierdo.direccion);
                            padre.listaClaves[idxActualizarPadre] = claveBusquedaDePrestado;
                            escribeNodoArchivo(arbol.atributo, padre);
                        }
                        else
                        {
                            long direccionVecino = vecinoIzquierdo.listaDirecciones[vecinoIzquierdo.numeroClaves()];
                            int claveBusquedaVecino = vecinoIzquierdo.listaClaves[vecinoIzquierdo.numeroClaves() - 1];
                            int idxClaveBusquedaPadre = padre.listaDirecciones.IndexOf(vecinoIzquierdo.direccion);
                            int claveBusquedaPadre = padre.listaClaves[idxClaveBusquedaPadre];

                            if (!vecinoIzquierdo.eliminaNodoRaizOIntermedio(claveBusquedaVecino, direccionVecino))
                                return false;
                            escribeNodoArchivo(arbol.atributo, vecinoIzquierdo);

                            padre.listaClaves[idxClaveBusquedaPadre] = claveBusquedaVecino;
                            escribeNodoArchivo(arbol.atributo, padre);

                            nodo.insertaEnRaizOIntermedio(claveBusquedaPadre, direccionVecino);
                            escribeNodoArchivo(arbol.atributo, nodo);
                        }
                    }
                    else if (vecinoDerecho != null && arbol.tieneMismoNodoPadre(nodo, vecinoDerecho))
                    {
                        if (tipo == 'H')
                        {
                            for (int i = 0; i < vecinoDerecho.numeroClaves(); i++)
                                nodo.insertaEnHoja(vecinoDerecho.listaClaves[i], vecinoDerecho.listaDirecciones[i]);
                            escribeNodoArchivo(arbol.atributo, nodo);
                            if (padre.tipo == 'R' && padre.numeroClaves() == 1)
                            {
                                escribeCabeceraIndice(arbol.atributo.atrib, vecinoDerecho.direccion);
                            }
                            else
                            {
                                int idxEliminarPadre = padre.listaDirecciones.IndexOf(vecinoDerecho.direccion);
                                int nuevoDato = padre.listaClaves[idxEliminarPadre - 1];
                                long nuevaDireccion = padre.listaDirecciones[idxEliminarPadre];

                                return eliminaDeArbol(arbol, padre, nuevoDato, nuevaDireccion);
                            }
                        }
                        else
                        {
                            int claveBusquedaPadre = padre.listaClaves[padre.listaDirecciones.IndexOf(nodo.direccion)];
                            long direccionCeroVecino = vecinoDerecho.listaDirecciones[0];

                            vecinoDerecho.insertaEnRaizOIntermedio(claveBusquedaPadre, direccionCeroVecino);

                            for (int i = 0; i < nodo.numeroClaves(); i++)
                                vecinoDerecho.insertaEnRaizOIntermedio(nodo.listaClaves[i], nodo.listaDirecciones[i + 1]);
                            vecinoDerecho.listaDirecciones[0] = nodo.listaDirecciones[0];

                            if (padre.tipo == 'R' && padre.numeroClaves() == 1)
                            {
                                vecinoDerecho.tipo = 'R';
                                escribeNodoArchivo(arbol.atributo, vecinoDerecho);
                                escribeCabeceraIndice(arbol.atributo.atrib, vecinoDerecho.direccion);
                            }
                            else
                            {
                                escribeNodoArchivo(arbol.atributo, vecinoDerecho);
                                return eliminaDeArbol(arbol, padre, claveBusquedaPadre, nodo.direccion);
                            }
                        }
                    }
                    else if (vecinoIzquierdo != null && arbol.tieneMismoNodoPadre(nodo, vecinoIzquierdo))
                    {
                        if (tipo == 'H')
                        {
                            for (int i = 0; i < nodo.numeroClaves(); i++)
                                vecinoIzquierdo.insertaEnHoja(nodo.listaClaves[i], nodo.listaDirecciones[i]);
                            escribeNodoArchivo(arbol.atributo, vecinoIzquierdo);
                            if (padre.tipo == 'R' && padre.numeroClaves() == 1)
                            {
                                vecinoIzquierdo.tipo = 'R';
                                escribeCabeceraIndice(arbol.atributo.atrib, vecinoIzquierdo.direccion);
                            }
                            else
                            {
                                int idxEliminarPadre = padre.listaDirecciones.IndexOf(nodo.direccion);
                                int nuevoDato = padre.listaClaves[idxEliminarPadre - 1];
                                long nuevaDireccion = padre.listaDirecciones[idxEliminarPadre];

                                return eliminaDeArbol(arbol, padre, nuevoDato, nuevaDireccion);
                            }
                        }
                        else
                        {
                            int claveBusquedaPadre = padre.listaClaves[padre.listaDirecciones.IndexOf(vecinoIzquierdo.direccion)];
                            long direccionCeroNodo = nodo.listaDirecciones[0];

                            vecinoIzquierdo.insertaEnRaizOIntermedio(claveBusquedaPadre, direccionCeroNodo);

                            for (int i = 0; i < nodo.numeroClaves(); i++)
                                vecinoIzquierdo.insertaEnRaizOIntermedio(nodo.listaClaves[i], nodo.listaDirecciones[i + 1]);

                            if (padre.tipo == 'R' && padre.numeroClaves() == 1)
                            {
                                vecinoIzquierdo.tipo = 'R';
                                escribeNodoArchivo(arbol.atributo, vecinoIzquierdo);
                                escribeCabeceraIndice(arbol.atributo.atrib, vecinoIzquierdo.direccion);
                            }
                            else
                            {
                                escribeNodoArchivo(arbol.atributo, vecinoIzquierdo);
                                return eliminaDeArbol(arbol, padre, claveBusquedaPadre, nodo.direccion);
                            }
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        // Función que prepara y llama a la Inserción del Árbol primario
        public void accionesArbolPrimario(CAtributo atributo, Registro registro, int index_atributo)
        {
            Arbol arbolPrim = new Arbol(dameNodos(atributo), atributo);
            int dato = BitConverter.ToInt32(registro.dimeAtributosRegistro[index_atributo], 0);
            insertaEnArbol(arbolPrim, dato, registro.dimeDireccionRegstro);
        }
        /*Funciones del árbol*/

        public List<Nodo> dameNodos(CAtributo atributo)
        {
            List<Nodo> listaNodos = new List<Nodo>();
            abreArchivoIndice();

            BinaryReader lectorBinario = new BinaryReader(archivoIndice);
            long direccionIndice = atributo.ind;

            if (direccionIndice != -1)
            {
                lectorBinario.BaseStream.Position = direccionIndice;
                Nodo nodo = new Nodo();
                nodo.tipo = System.Convert.ToChar(lectorBinario.ReadByte());
                nodo.direccion = lectorBinario.ReadInt64();
                nodo.listaClaves = new List<int>();
                nodo.listaDirecciones = new List<long>();

                for (int i = 0; i < gradoArbol - 1; i++)
                {
                    nodo.listaDirecciones.Add(lectorBinario.ReadInt64());
                    nodo.listaClaves.Add(lectorBinario.ReadInt32());
                }

                nodo.listaDirecciones.Add(lectorBinario.ReadInt64());
                listaNodos.Add(nodo);
                lectorBinario.Close();
                cierraArchivoIndice();
            }
            else
            {
                cierraArchivoIndice();
                return listaNodos;
            }

            if (listaNodos[0].tipo == 'H')
                return listaNodos;
            else
            {
                List<Nodo> listaDeNodos = dameNodoRaizoIntermedio(atributo, listaNodos[0]);
                foreach (Nodo nodo in listaDeNodos)
                {
                    listaNodos.Add(nodo);
                }
            }

            return listaNodos;
        }


        public List<Nodo> dameNodoRaizoIntermedio(CAtributo atributo, Nodo denso)
        {
            List<Nodo> listaNodos = new List<Nodo>();
            abreArchivoIndice();

            BinaryReader lectorBinario = new BinaryReader(archivoIndice);

            foreach (long direccion in denso.listaDirecciones)
            {
                if (direccion == -1) break;
                lectorBinario.BaseStream.Position = direccion;

                Nodo nodo = new Nodo();

                nodo.tipo = System.Convert.ToChar(lectorBinario.ReadByte());
                nodo.direccion = lectorBinario.ReadInt64();
                nodo.listaClaves = new List<int>();
                nodo.listaDirecciones = new List<long>();

                for (int i = 0; i < gradoArbol - 1; i++)
                {
                    nodo.listaDirecciones.Add(lectorBinario.ReadInt64());
                    nodo.listaClaves.Add(lectorBinario.ReadInt32());
                }
                nodo.listaDirecciones.Add(lectorBinario.ReadInt64());
                listaNodos.Add(nodo);
            }
            lectorBinario.Close();
            cierraArchivoIndice();

            int tamañoDenso = listaNodos.Count;
            for (int i = 0; i < tamañoDenso; i++)
            {
                Nodo item = listaNodos[i];
                if (item.tipo != 'H')
                {
                    List<Nodo> temp;
                    temp = dameNodoRaizoIntermedio(atributo, item);
                    foreach (var item2 in temp)
                    {
                        listaNodos.Add(item2);
                    }
                }
            }
            return listaNodos;
        }


        public void insertaEnArbol(Arbol arbol, int dato, long direccion)
        {
            // Si no existen nodos en el árbol, se crea la primera hoja y se le inserta la primera clave
            if (arbol.listaDeNodos.Count == 0)
            {
                Nodo nuevo = creaNodo(arbol.atributo, 'H');
                nuevo.listaDirecciones[0] = direccion;
                nuevo.listaClaves[0] = dato;
                escribeNodoArchivo(arbol.atributo, nuevo);
                escribeCabeceraIndice(arbol.atributo.asigdireccion, nuevo.direccion);
            }
            // Si no contiene raíz se inserta en la única hoja que se tiene en el árbol
            else if (!arbol.contieneRaiz())
            {
                Nodo unicaHoja = arbol.listaDeNodos[0];
                List<Nodo> nodos = insertaDatoEnHoja(arbol.atributo, unicaHoja, dato, direccion);
                if (nodos.Count == 2)
                {
                    Nodo raiz = creaNodo(arbol.atributo, 'R');
                    raiz.listaDirecciones[0] = nodos[0].direccion;
                    raiz.listaDirecciones[1] = nodos[1].direccion;
                    raiz.listaClaves[0] = nodos[1].listaClaves[0];
                    escribeCabeceraIndice(arbol.atributo.asigdireccion, raiz.direccion);
                    escribeNodoArchivo(arbol.atributo, raiz);
                }
            }
            else
            {
                Nodo nodoPadre;
                Nodo nodoHoja = arbol.dameNodoRaiz();

                do
                {
                    nodoPadre = nodoHoja;
                    int index = nodoPadre.damePosicionActualDato(dato);
                    nodoHoja = arbol.dameNodo(nodoPadre.listaDirecciones[index]);
                } while (nodoHoja.tipo != 'H');

                List<Nodo> nodos = insertaDatoEnHoja(arbol.atributo, nodoHoja, dato, direccion);
                if (nodos.Count == 2)
                {
                    actualizaInfoNodoPadre(arbol, nodoPadre, nodos[1].listaClaves[0], nodos[1].direccion);
                }
            }
        }
        public Nodo creaNodo(CAtributo atributo, char tipo)
        {
            abreArchivoIndice();
            Nodo nodo = new Nodo();
            nodo.tipo = tipo;
            nodo.direccion = archivoIndice.Length;
            cierraArchivoIndice();
            return nodo;
        }
        public void escribeNodoArchivo(CAtributo atributo, Nodo nodo)
        {
            abreArchivoIndice();
            BinaryWriter escritorBinario = new BinaryWriter(archivoIndice);
            escritorBinario.BaseStream.Position = nodo.direccion;
            escritorBinario.Write(nodo.tipo);
            escritorBinario.Write(nodo.direccion);
            for (int i = 0; i < gradoArbol; i++)
            {
                escritorBinario.Write(nodo.listaDirecciones[i]);
                if (i != gradoArbol - 1)
                    escritorBinario.Write(nodo.listaClaves[i]);
            }
            cierraArchivoIndice();
        }
        public List<Nodo> insertaDatoEnHoja(CAtributo atributo, Nodo nodoHoja, int dato, long direccion)
        {
            // Si no se pudo insertar significa que se tiene que dividir el nodo
            if (!nodoHoja.insertaEnHoja(dato, direccion))
            {
                List<Nodo> res = divideHoja(nodoHoja, creaNodo(atributo, 'H'), dato, direccion);
                escribeNodoArchivo(atributo, res[0]);
                escribeNodoArchivo(atributo, res[1]);
                return res;
            }
            // Si sí se insertó simplemente se actualiza en el archivo
            else
            {
                escribeNodoArchivo(atributo, nodoHoja);
                List<Nodo> res = new List<Nodo> { nodoHoja };
                return res;
            }
        }
        public List<Nodo> divideHoja(Nodo nodoLleno, Nodo nodoVacio, int dato, long direccion)
        {
            int idxMedio = (gradoArbol - 1) / 2 - 1;

            // Se mueven los datos según si es derecha o izquierda
            List<long> apuntadores = new List<long>();
            List<int> claves = new List<int>();

            foreach (var item in nodoLleno.listaDirecciones)
                apuntadores.Add(item);

            foreach (var item in nodoLleno.listaClaves)
                claves.Add(item);

            claves.Add(dato);
            claves.Sort();

            Nodo nodoNuevo = new Nodo();
            nodoNuevo.tipo = nodoLleno.tipo;
            nodoNuevo.direccion = nodoLleno.direccion;
            nodoNuevo.listaDirecciones[gradoArbol - 1] = nodoLleno.listaDirecciones[gradoArbol - 1];

            int idxAnterior = 0, idx_nuevo = 0, idx_temp = 0;
            for (int i = 0; i < gradoArbol; i++)
            {
                if (i <= idxMedio)
                {
                    if (claves[i] == dato)
                        nodoNuevo.listaDirecciones[idxAnterior] = direccion;
                    else
                        nodoNuevo.listaDirecciones[idxAnterior] = apuntadores[idx_temp++];
                    nodoNuevo.listaClaves[idxAnterior++] = claves[i];
                }
                else
                {
                    if (claves[i] == dato)
                        nodoVacio.listaDirecciones[idx_nuevo] = direccion;
                    else
                        nodoVacio.listaDirecciones[idx_nuevo] = apuntadores[idx_temp++];
                    nodoVacio.listaClaves[idx_nuevo++] = claves[i];
                }
            }

            nodoVacio.listaDirecciones[gradoArbol - 1] = nodoNuevo.listaDirecciones[gradoArbol - 1];
            nodoNuevo.listaDirecciones[gradoArbol - 1] = nodoVacio.direccion;

            return new List<Nodo> { nodoNuevo, nodoVacio };
        }
        void actualizaInfoNodoPadre(Arbol arbol, Nodo nodoPadre, int dato, long direccion)
        {
            if (nodoPadre.insertaEnRaizOIntermedio(dato, direccion))
                escribeNodoArchivo(arbol.atributo, nodoPadre);
            else
            {
                int tipo = nodoPadre.tipo;
                if (tipo == 'R') nodoPadre.tipo = 'I';

                List<int> indicesOrdenados = new List<int>();
                foreach (var item in nodoPadre.listaClaves)
                    indicesOrdenados.Add(item);

                indicesOrdenados.Add(dato);
                indicesOrdenados.Sort();
                int idxMitad = (gradoArbol - 1) / 2;
                int claveArriba = indicesOrdenados[idxMitad];

                List<Nodo> listaNodosIntermedios = divideHoja(indicesOrdenados, nodoPadre, creaNodo(arbol.atributo, 'I'), dato, direccion);

                escribeNodoArchivo(arbol.atributo, listaNodosIntermedios[0]);
                escribeNodoArchivo(arbol.atributo, listaNodosIntermedios[1]);

                if (tipo == 'R')
                {
                    Nodo nuevaRaiz = creaNodo(arbol.atributo, 'R');
                    nuevaRaiz.listaDirecciones[0] = listaNodosIntermedios[0].direccion;
                    nuevaRaiz.listaDirecciones[1] = listaNodosIntermedios[1].direccion;
                    nuevaRaiz.listaClaves[0] = claveArriba;
                    escribeCabeceraIndice(arbol.atributo.asigdireccion, nuevaRaiz.direccion);
                    escribeNodoArchivo(arbol.atributo, nuevaRaiz);
                }
                else
                {
                    Nodo padre = arbol.dameNodoPadre(listaNodosIntermedios[0]);
                    actualizaInfoNodoPadre(new Arbol(dameNodos(arbol.atributo), arbol.atributo), padre, claveArriba, listaNodosIntermedios[1].direccion);
                }
            }
        }
        public List<Nodo> divideHoja(List<int> claves, Nodo nodoLleno, Nodo nodoVacio, int dato, long direccion)
        {
            int idxMedio = (gradoArbol - 1) / 2;

            List<long> direcciones = new List<long>();

            foreach (var item in nodoLleno.listaDirecciones)
                direcciones.Add(item);

            Nodo nodoNuevo1 = new Nodo();
            nodoNuevo1.tipo = nodoLleno.tipo;
            nodoNuevo1.direccion = nodoLleno.direccion;

            int idxClaveMedio = nodoLleno.listaClaves.IndexOf(claves[idxMedio]) + 1;
            nodoNuevo1.listaDirecciones[0] = direcciones[0];
            nodoVacio.listaDirecciones[0] = direcciones[idxClaveMedio];

            direcciones.RemoveAt(idxClaveMedio);
            direcciones.RemoveAt(0);

            int idxViejo = 0, idx_nuevo = 0, idxTemp = 0;
            for (int i = 0; i < gradoArbol; i++)
            {
                if (i < idxMedio)
                {
                    if (claves[i] == dato)
                        nodoNuevo1.listaDirecciones[idxViejo + 1] = direccion;
                    else
                        nodoNuevo1.listaDirecciones[idxViejo + 1] = direcciones[idxTemp++];
                    nodoNuevo1.listaClaves[idxViejo++] = claves[i];
                }
                else if (i > idxMedio)
                {
                    if (claves[i] == dato)
                        nodoVacio.listaDirecciones[idx_nuevo + 1] = direccion;
                    else
                        nodoVacio.listaDirecciones[idx_nuevo + 1] = direcciones[idxTemp++];
                    nodoVacio.listaClaves[idx_nuevo++] = claves[i];
                }
            }
            return new List<Nodo> { nodoNuevo1, nodoVacio };
        }
      
        #endregion
        #endregion


        #region funciones independientes

        public class ByteArrayComparer2 : IEqualityComparer<byte[]>
        {
            public bool Equals(byte[] izquierda, byte[] derecha)
            {
                if (izquierda == null || derecha == null)
                {
                    return izquierda == derecha;
                }
                return izquierda.SequenceEqual(derecha);
            }
            public int GetHashCode(byte[] llave)
            {
                if (llave == null)
                    throw new ArgumentNullException("key");
                return llave.Sum(b => b);
            }
        }
        public class ByteArrayComparer1 : IComparer<byte[]>
        {
            public int Compare(byte[] x, byte[] y)
            {
                int iguales;
                for (int index = 0; index < x.Length; index++)
                {
                    iguales = x[index].CompareTo(y[index]);
                    if (iguales != 0) return iguales;
                }
                return x.Length.CompareTo(y.Length);
            }
        }
        #endregion
    }
    //////////////////////////////////Atributos///////////////////////////////////////////////////

}

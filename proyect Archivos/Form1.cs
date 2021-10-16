using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO; // No olvidar este using.

namespace proyect_Archivos
{
    public partial class Form1 : Form
    {

        private CDiccionario dicio;// = new CDiccionario();
        private string rutacarp;
        public Form1()
        {
            InitializeComponent();

        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void abrirArchivoToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        //cambio del panel principal
        private void btninicio_Click_1(object sender, EventArgs e)
        {
            panelprin.SelectedIndex = 1;
        }
        // metodo que permite abri un archi ya existente
        private void abrirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Diccionario de Datos|*.BD";
            openFileDialog.Title = "Abrir Diccionario";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (dicio == null)
                {
                    dicio = new CDiccionario();
                    MessageBox.Show(openFileDialog.FileName);
                    String fileName = openFileDialog.FileName;
                    this.rutacarp = Path.GetDirectoryName(fileName);
                    rutacarpa();
                    if (dicio.abrirarchi(openFileDialog.FileName))
                    {
                        Invalidate();
                        lb1.Text = "Archivo: " + openFileDialog.FileName;
                        nombreruta.Text = openFileDialog.FileName;
                        String cadena = openFileDialog.FileName;
                        FileInfo fi = new FileInfo(cadena);
                        nombrerutafin = fi.Name;
                        inicio();
                    }
                    else
                        MessageBox.Show("Error al abrir archiov.", "Error");
                }
                else
                { MessageBox.Show("Error al abrir archiov.", "Error por existencia de apertura de archivo."); }
                //   MessageBox.Show(openFileDialog.FileName);
            }

        }
        private void rutacarpa()
        {
            dicio.asignarutacarpet(this.rutacarp);
        }
        private void cambiarNombreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dicio == null)
            {

                MessageBox.Show("Error al abrir archiov.", "Error por inexistencia de apertura de archivo.");
            }
            else
            {
                panelprin.SelectedIndex = 2;
                actualnom.Text = "Nombre actual: " + nombrerutafin;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string message = "Camio de nombre?";
            string caption = "Cambio de nombre";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result;

            // Displays the MessageBox.
            result = MessageBox.Show(message, caption, buttons);
            if (result == System.Windows.Forms.DialogResult.Yes && nuevon.Text != "")
            {

                string rutacarp = this.rutacarp + "\\" + nuevon.Text;

                dicio.cierre();
                dicio = null;
                dicio = new CDiccionario();
                System.IO.File.Move(nombreruta.Text, rutacarp + ".BD");
                rutacarpa();
                if (dicio.abrirarchi(rutacarp + ".BD"))
                {
                    Invalidate();
                    lb1.Text = "Archivo: " + rutacarp + ".BD";
                    nombreruta.Text = rutacarp + ".BD";
                    String cadena = rutacarp + ".BD";
                    FileInfo fi = new FileInfo(cadena);
                    nombrerutafin = fi.Name;
                    inicio();
                    panelprin.SelectedIndex = 1;
                    lb1.Text = "Archivo: " + rutacarp + ".BD";
                }
                else
                    MessageBox.Show("Error al abrir archiov.", "Error");
                restura();
                inicio();
                nuevon.Text = "";
            }
            else
            {
                MessageBox.Show("ERROR");
            }


        }
        private void button2_Click(object sender, EventArgs e)
        {
            panelprin.SelectedIndex = 1;
            nuevon.Text = "";
        }
        String nombrerutafin;
        //creacion de un nuevo archivo mientras no exita
        private void NuevoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dicio == null)
            {
                dicio = new CDiccionario();
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Diccionario de Datos|*.BD";
                saveFileDialog.Title = "Guardar Como";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    dicio.creaArchivoDiccionario(saveFileDialog.FileName);
                    dicio.abrirarchi(saveFileDialog.FileName);
                    lb1.Text = "Archivo: " + saveFileDialog.FileName;
                    nombreruta.Text = saveFileDialog.FileName;
                    String cadena = saveFileDialog.FileName;
                    FileInfo fi = new FileInfo(cadena);
                    nombrerutafin = fi.Name;
                    String fileName = saveFileDialog.FileName;
                    this.rutacarp = Path.GetDirectoryName(fileName);
                    rutacarpa();
                    inicio();
                }
            }
            else
            { MessageBox.Show("Error al abrir archiov.", "Error por inexistencia de apertura de archivo."); }
        }
        //cierra archivo si es que esta abierto
        private void cerrar_Click(object sender, EventArgs e)
        {
            if (dicio != null)
            {
                dicio.cierre();
                dicio = null;
                restura();
                MessageBox.Show("Cierre de Archivo", "Cierre exitoso de archivo.");
            }
            else
            { MessageBox.Show("Error al cerrar archiov.", "Error por inexistencia de apertura de archivo."); }
        }

        private void eliminarBaseDeDatosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string message = "Eliminar Base?";
            string caption = "Eliminar";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result;

            // Displays the MessageBox.
            result = MessageBox.Show(message, caption, buttons);
            if (result == System.Windows.Forms.DialogResult.Yes )
            {
                if (dicio != null)
                {
                    dicio.eliminaciontotal();
                    dicio.cierre();
                    File.Delete(rutacarp + "\\" + nombrerutafin);
                    dicio = null;
                    restura();
                    MessageBox.Show("Eliminacion de Archivo", "Eliminacion exitoso de archivo.");
                }
                else
                { MessageBox.Show("Error al cerrar archiov.", "Error por inexistencia de apertura de archivo."); }

            }
        } 
        //regresa a la normalidad atributos del form
        private void restura()
        {
            lb1.Text = "Archivo: ";
            lbcab.Text = "Cabecera: ";
            nuevon.Text = "";
            Panelsecundario.Enabled = false;
            Panelsecundario.SelectedIndex = 0;
            btmodif.Enabled = false;
            btelimina.Enabled = false;
            btguardar.Enabled = false;
            //dicio.orden();
            restaucombo();
            //iniciagrod();
            data1.Rows.Clear();

        }
        private void restaucombo()
        {
            texID.Text = "";
            texdt.Text = "";
            texatri.Text = "";
            texsig.Text = "";
            texnomb.Text = "";
            comboenti.Items.Clear();
            comboenti.Text = "";

        }
        //pone los datos correspondientes para crecion de entidades y atributos asi como se muestra e
        private void inicio()
        {
            Panelsecundario.SelectedIndex = 0;
            Panelsecundario.Enabled = true;
            dicio.llenalista();
            iniciagrod();
            dicio.orden();

            //  MessageBox.Show(dicio.buscacabecer().ToString());
            if (dicio.numdeentida() == 0)
            {
                lbcab.Text = "Cabecera: -1";

                comboenti.Items.Add("Nueva Entidad");
            }
            else
            {
                List<CEntidad> axui = dicio.dameRegistrosEntidad();

                for (int i = 0; i < axui.Count; i++)
                {
                    comboenti.Items.Add(new string(axui[i].asignombre).Replace(" ", ""));
                }
                comboenti.Items.Add("Nueva Entidad");
                lbcab.Text = "Cabecera: " + dicio.buscacabecer();
            }

        }
        #region panel0

        //evento del combobox para las entidades
        private void comboenti_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dicio.numdeentida() == 0)
            {
                texID.Text = "8";
                texdt.Text = "-1";
                texatri.Text = "-1";
                texsig.Text = "-1";
                btguardar.Enabled = true;
            }
            else
            { if (comboenti.Text == "Nueva Entidad")
                {
                    texnomb.Text = "";
                    texID.Text = dicio.idreturn().ToString();
                    texatri.Text = "-1";
                    texdt.Text = "-1";
                    texsig.Text = "-1";
                    btmodif.Enabled = false;
                    btelimina.Enabled = false;
                    btguardar.Enabled = true;
                    botonregenti.Enabled = false;
                }
                else
                {
                    texnomb.Text = comboenti.Text;
                    texID.Text = dicio.idreturn2(comboenti.Text).ToString();
                    texatri.Text = dicio.atreturn(comboenti.Text).ToString(); ;
                    texdt.Text = dicio.dateturn(comboenti.Text).ToString();
                    texsig.Text = dicio.sigeturn(comboenti.Text).ToString();
                    btmodif.Enabled = true;
                    btelimina.Enabled = true;
                    btguardar.Enabled = false;
                    botonregenti.Enabled = true;

                }
            }
        }

        private void btguardar_Click(object sender, EventArgs e)
        {
            if (dicio.existenoment(texnomb.Text) == true)
            {
                MessageBox.Show("No se puede guardar una Entidad con el mismo nombre de una ya existente", "ERROR");
            }
            else
            {
                if (texnomb.Text != "")
                {
                    dicio.insertaEntidad(texnomb.Text);

                    string s1 = this.rutacarp + "\\" + texnomb.Text + ".dat";
                    FileStream archivo = new FileStream(s1, FileMode.Create);
                    BinaryWriter escritorBinario = new BinaryWriter(archivo);
                    archivo.Close();
                   // string s2 = this.rutacarp + "\\" + texnomb.Text + ".idx";
                    //FileStream archivo2 = new FileStream(s2, FileMode.Create);
                    //BinaryWriter escritorBinario2 = new BinaryWriter(archivo2);
                    //archivo2.Close();
                    List<CEntidad> axui = dicio.dameRegistrosEntidad();
                    restaucombo();
                    iniciagrod();
                    for (int i = 0; i < axui.Count; i++)
                    {
                        comboenti.Items.Add(new string(axui[i].asignombre).Replace(" ", ""));
                    }
                    comboenti.Items.Add("Nueva Entidad");
                    lbcab.Text = "Cabecera: " + dicio.buscacabecer();
                }
            }
        }

        private void btelimina_Click(object sender, EventArgs e)
        {
            CEntidad entidad = dicio.dameRegistrosEntidad()[comboenti.SelectedIndex];
            if (texnomb.Text != "" && entidad.asigdat == (long)-1)
            {
                dicio.eliminaEntidad(comboenti.SelectedIndex);
                restaucombo();
                dicio.llenalista();
                List<CEntidad> axui = dicio.dameRegistrosEntidad();

                for (int i = 0; i < axui.Count; i++)
                {
                    comboenti.Items.Add(new string(axui[i].asignombre).Replace(" ", ""));
                }
                comboenti.Items.Add("Nueva Entidad");
                lbcab.Text = "Cabecera: " + dicio.buscacabecer();
                //restaucombo();

            }
            else
            {
                MessageBox.Show("Error no puedes nombrar como otra Entidad o la entidad ya contiene datos");
            }
        }

        private void btmodif_Click(object sender, EventArgs e)
        {
            CEntidad entidad = dicio.dameRegistrosEntidad()[comboenti.SelectedIndex];

            if (texnomb.Text != "" && dicio.cambiono(texnomb.Text, comboenti.SelectedIndex) && entidad.asigdat == (long)-1)
            {
                dicio.actualizaEntidad(comboenti.SelectedIndex, texnomb.Text);
                restaucombo();
                dicio.llenalista();

                lbcab.Text = "Cabecera: " + dicio.buscacabecer();


            }
            else
            {
                MessageBox.Show("Error no puedes eliminar la entidad ya contiene datos o esta mal los datos");
            }
        }

        private void botonregenti_Click(object sender, EventArgs e)
        {

            duda();
            Panelsecundario.SelectedIndex = 2;
        }

        #endregion
        private int auorizo = 0;

        private void duda()
        {
            auorizo = 1;
        }


        private void Panelsecundario_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (dicio != null)
            {
                try
                {
                    switch (Panelsecundario.SelectedIndex)
                    {
                        case 0:
                            restaucombo();
                            dicio.llenalista();
                            List<CEntidad> axui0 = dicio.dameRegistrosEntidad();

                            for (int i = 0; i < axui0.Count; i++)
                            {
                                comboenti.Items.Add(new string(axui0[i].asignombre).Replace(" ", ""));
                            }
                            comboenti.Items.Add("Nueva Entidad");
                            lbcab.Text = "Cabecera: " + dicio.buscacabecer();
                            break;
                        case 1:
                            comboEnat.Items.Clear();
                            comboEnat.Text = "";
                            List<CEntidad> axui = dicio.dameRegistrosEntidad();
                            for (int i = 0; i < axui.Count; i++)
                            {
                                comboEnat.Items.Add(new string(axui[i].asignombre).Replace(" ", ""));
                            }
                            texat.Text = "";
                            textdirat.Text = "";
                            Cobti.Text = "";
                            Cobti.Enabled = false;
                            textlong.Text = "";
                            comoindi.Text = "";
                            comoindi.Enabled = false;
                            tesdirind.Text = "";
                            texdirsigat.Text = "";

                            Comoatri.Text = "";
                            Comoatri.Enabled = false;
                            break;
                        case 2:
                            camboregsi.Items.Clear();
                            List<CEntidad> axui2 = dicio.dameRegistrosEntidad();

                            for (int i = 0; i < axui2.Count; i++)
                            {
                                camboregsi.Items.Add(new string(axui2[i].asignombre).Replace(" ", ""));
                            }
                            camboregsi.Items.Add("Todos");
                            camboregsi.SelectedIndex = camboregsi.Items.Count - 1;

                            busca();


                            break;
                        case 3:
                            CBDatos.Items.Clear();
                            List<CEntidad> axui3 = dicio.dameRegistrosEntidad();
                            for (int i = 0; i < axui3.Count; i++)
                            {
                                CBDatos.Items.Add(new string(axui3[i].asignombre).Replace(" ", ""));

                            }
                            CBDatos.Text = "";
                            btInsertarR.Enabled = true;
                            btModificarR.Enabled = true;
                            btEliminarR.Enabled = true;
                            textbusq.Enabled = true;
                            comboatru.Text = "";
                            textbusq.Text = "";
                            //  dgInsertaR.Columns.Clear();
                            // dgRegistrosR.Columns.Clear();

                            break;
                        case 4:
                            List<CEntidad> registros = dicio.dameRegistrosEntidad();
                            int index3 = cbEntidadIndices.SelectedIndex;
                            int index4 = cbEntidadIndiceSecundario.SelectedIndex;
                            cbEntidadIndices.Items.Clear();
                            cbEntidadIndiceSecundario.Items.Clear();

                            foreach (var item in registros)
                            {
                                //cbEntidadA.Items.Add(new string(item.dimeNombreEntidad).Trim());
                                //cbEntidadR.Items.Add(new string(item.dimeNombreEntidad).Trim());
                                cbEntidadIndices.Items.Add(new string(item.asignombre).Trim());
                                cbEntidadIndiceSecundario.Items.Add(new string(item.asignombre).Trim());
                                //cbEntidadArbolPrimario.Items.Add(new string(item.dimeNombreEntidad).Trim());
                                //cbEntidadesArbolSecundario.Items.Add(new string(item.dimeNombreEntidad).Trim());

                            }
                            cbEntidadIndices.SelectedIndex = index3;
                            cbEntidadIndiceSecundario.SelectedIndex = index4;
                            //cbEntidadArbolPrimario.SelectedIndex = index4;
                            //cbEntidadesArbolSecundario.SelectedIndex = index4;

                            if (index3 > 0)
                                cargaAtributosIdxPrim(registros[index3]);
                            if (index4 > 0)
                                cargaAtributosIdxSecu(registros[index4]);
                            break;

                        case 5:
                            List<CEntidad> registros2 = dicio.dameRegistrosEntidad();
                            cbEntidadArbolPrimario.Items.Clear();
                            int index5 = cbEntidadArbolPrimario.SelectedIndex;
                            foreach (var item in registros2)
                            {

                                cbEntidadArbolPrimario.Items.Add(new string(item.asignombre).Trim());

                            }

                            if (index5 > 0)
                                cargaAtributosIdxPrimArbol(registros2[index5]);

                            break;

                    }
                    if (Panelsecundario.SelectedIndex == 1)
                    {

                    }

                }
                catch
                {

                }
            }
        }
        #region Panel 1 Atributos
        private void comboEnat_SelectedIndexChanged(object sender, EventArgs e)
        {
            Comoatri.Enabled = true;

            Comoatri.Items.Clear();
            List<CEntidad> axui = dicio.dameRegistrosEntidad();
            for (int i = 0; i < axui.Count; i++)
            {
                if (comboEnat.Text == new string(axui[i].asignombre).Replace(" ", ""))
                {
                    List<CAtributo> axui2 = dicio.dameRegistrosAtributo(axui[i].atrib);
                    for (int j = 0; j < axui2.Count; j++)
                    {
                        Comoatri.Items.Add(new string(axui2[j].asignombre).Replace(" ", ""));
                    }
                }
            }

            Comoatri.Items.Add("Nuevo Atributo");
        }

        bool buscarfora()
        {
            List<CEntidad> axui = dicio.dameRegistrosEntidad();
            List<CAtributo> axui2, auxfor;
            for (int i = 0; i < axui.Count; i++)
            {
                if (comboEnat.Text == new string(axui[i].asignombre).Replace(" ", ""))
                {
                     axui2 = dicio.dameRegistrosAtributo(axui[i].atrib);
                  //  List<CAtributo> axui3 = dicio.regretlieatribut();

                        for (int a = 0; a < axui.Count; a++)
                        {  
                        for (int p = 0; p < axui.Count; p++)
                    {                                 
                            auxfor = dicio.dameRegistrosAtributo(axui[p].atrib);
                            for (int p3 = 0; p3 < auxfor.Count; p3++)
                            {
                                if ((axui[a].asigdireccion == auxfor[p3].ind)&& a==i)
                                {
                                    return true;
                                }
                  }

                       

                            }
                        }


                }
            }

            return false;


        }


        private void Comoatri_SelectedIndexChanged(object sender, EventArgs e)
        {
            texat.Enabled = true;
            Cobti.Enabled = true;
            textlong.Enabled = true;
            comoindi.Enabled = true;

            if (Comoatri.Text == "Nuevo Atributo")
            {
                btguardatrib.Enabled = true;
                botmodiat.Enabled = false;
                botelimiatrib.Enabled = false;
                List<CEntidad> axui = dicio.dameRegistrosEntidad();
                for (int i = 0; i < axui.Count; i++)
                {
                    if (comboEnat.Text == new string(axui[i].asignombre).Replace(" ", ""))
                    {
                        List<CAtributo> axu4 = dicio.dameRegistrosAtributo(axui[i].atrib);
                        if (axu4.Count == 0)
                        {
                            texat.Text = "";
                            texat.Enabled = true;
                        }
                        else
                        {
                            texat.Enabled = true;
                            texat.Text = "";
                        }
                        break;
                    }
                }
                btguardatrib.Enabled = true;

                textdirat.Text = dicio.regresatamarch().ToString();
                Cobti.Text = "Selecione tipo dato";
                textlong.Text = "";
                comoindi.Text = "Selecione tipo indice";
                tesdirind.Text = "-1";
                texdirsigat.Text = "-1";
                exitimos = false;
            }
            else
            {
                btguardatrib.Enabled = false;
                botmodiat.Enabled = true;
                botelimiatrib.Enabled = true;
                texat.Enabled = true;
                exitimos = true;

                List<CEntidad> axui1 = dicio.dameRegistrosEntidad();
                for (int j = 0; j < axui1.Count; j++)
                {
                    if (comboEnat.Text == new string(axui1[j].asignombre).Replace(" ", ""))
                    {
                        List<CAtributo> axui = dicio.dameRegistrosAtributo(axui1[j].atrib);
                        for (int i = 0; i < axui.Count; i++)
                        {
                         /*   if ("Cve_" + new string(axui[i].asignombre).Replace(" ", "") == Comoatri.Text)
                            {
                                texat.Text = "Cve_" + new string(axui[i].asignombre).Replace(" ", "");
                                textdirat.Text = axui[i].asigdireccion.ToString();
                                if (axui[i].tipoat == 'C')
                                {
                                    Cobti.SelectedIndex = 0;
                                }
                                else
                                {
                                    Cobti.SelectedIndex = 1;
                                }
                                //comoindi.Enabled = false;
                                textlong.Text = axui[i].longitud.ToString();
                                comoindi.SelectedIndex = axui[i].tipoind;
                                tesdirind.Text = axui[i].ind.ToString();
                                texdirsigat.Text = axui[i].atrib.ToString();
                                break;
                            }

                            else
                            {*/
                                if (new string(axui[i].asignombre).Replace(" ", "") == Comoatri.Text)
                                {
                                    texat.Text = new string(axui[i].asignombre).Replace(" ", "");
                                    textdirat.Text = axui[i].asigdireccion.ToString();



                                char op = axui[i].tipoat;
                                if (op == 'C')
                                {
                                    Cobti.SelectedIndex = 0;                                    
                                }
                                if ( op == 'E')
                                {Cobti.SelectedIndex = 1;
                                }
                                if (  op == 'F')
                                {Cobti.SelectedIndex = 2;
                                }

                                    comoindi.Enabled = true;
                                    textlong.Text = axui[i].longitud.ToString();
                                    comoindi.SelectedIndex = axui[i].tipoind;
                                    tesdirind.Text = axui[i].ind.ToString();
                                    texdirsigat.Text = axui[i].atrib.ToString();
                                    break;
                                }
                            }
                       // }
                        break;
                    }
                }


            }
        }
        private void btguardatrib_Click(object sender, EventArgs e)
        {
            CEntidad entidad = dicio.dameRegistrosEntidad()[comboEnat.SelectedIndex];
            List<CAtributo> istaAtributos = dicio.dameRegistrosAtributo(entidad.atrib);
            bool exit = false;
            for (int i = 0; i < istaAtributos.Count; i++)
            {
                string du = new string(istaAtributos[i].asignombre).Replace(" ", "");
                if (du == texat.Text)
                {
                    exit = true;
                    break;
                }
                else
                {

                }
            }
            //  for(int i=0;i<)
            if (entidad.asigdat == (long)-1 && !exit)
            {
                char op = ' ';
                if(Cobti.SelectedIndex==0)
                {
                    op = 'C';
                }
                if (Cobti.SelectedIndex == 1)
                {
                    op = 'E';
                }
                if (Cobti.SelectedIndex == 2)
                {
                    op = 'F';
                }


                if (comoindi.SelectedIndex < 8)
                    dicio.insertaAtributo(comboEnat.SelectedIndex, texat.Text, op , comoindi.SelectedIndex, Convert.ToInt32(textlong.Text), retorindi(tesdirind.Text));
                else
                {
                    dicio.insertaAtributo(comboEnat.SelectedIndex, texat.Text, op, comoindi.SelectedIndex, Convert.ToInt32(textlong.Text), retorindi(tesdirind.Text));
                }
                Panelsecundario.SelectedIndex = 0;
                Panelsecundario.SelectedIndex = 1;
            }
            else
            {
                MessageBox.Show("Error no puedes agregar la entidad ya contiene datos o esta mal los datos");
            }
        }

        private long retorindi(String p)
        {
            if (p == (-1).ToString())
            {
                return (long)-1;
            }
            else
            {
                List<CEntidad> axui1 = dicio.dameRegistrosEntidad();
                for (int j = 0; j < axui1.Count; j++)
                {
                    if (new string(axui1[j].asignombre).Replace(" ", "") == p)
                        return axui1[j].asigdireccion;
                }

            }
            return -1;
        }

        //combo de caracter o entero
        private void Cobti_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Cobti.SelectedIndex == 0)
            {
                textlong.Enabled = true;
                textlong.Text = "";
            }
            if (Cobti.SelectedIndex == 1)
            {
                textlong.Enabled = true;
                textlong.Text = "4";
            }
            if (Cobti.SelectedIndex == 2)
            {
                textlong.Enabled = true;
                textlong.Text = "8";
            }

        }


        bool exitimos = false;
        private void comoindi_SelectedIndexChanged(object sender, EventArgs e)
        { int fun = 0;

            if (exitimos)
            {
                if (comoindi.SelectedIndex == 8)
                {
                    tesdirind.Enabled = true;
                    tesdirind.Items.Clear();

                    List<CEntidad> axui = dicio.dameRegistrosEntidad();
                    for (int i = 0; i < axui.Count; i++)
                    {
                        if (axui[i].atrib != 1 && new string(axui[i].asignombre).Replace(" ", "") != comboEnat.Text)
                        {
                            tesdirind.Items.Add(new string(axui[i].asignombre).Replace(" ", ""));
                            fun = 1;
                        }

                    }
                    if (fun == 0)
                    {
                        tesdirind.Enabled = false;
                        tesdirind.Items.Clear();
                        tesdirind.Text = "-1";
                        comoindi.SelectedIndex = 0;
                        MessageBox.Show("Ninguan entidad cumple con esta condicion");
                    }
                }
                else
                {
                    tesdirind.Enabled = false;
                    tesdirind.Items.Clear();
                    tesdirind.Text = "-1";
                    // comoindi.SelectedIndex = 0;
                }
            }
            else
            {
                if (comoindi.SelectedIndex == 8)
                {
                    tesdirind.Enabled = true;
                    tesdirind.Items.Clear();

                    List<CEntidad> axui = dicio.dameRegistrosEntidad();
                    for (int i = 0; i < axui.Count; i++)
                    {
                        if (axui[i].atrib != 1 && new string(axui[i].asignombre).Replace(" ", "") != comboEnat.Text)
                        {
                            tesdirind.Items.Add(new string(axui[i].asignombre).Replace(" ", ""));
                            fun = 1;
                        }

                    }
                    if (fun == 0)
                    {
                        tesdirind.Enabled = false;
                        tesdirind.Items.Clear();
                        tesdirind.Text = "-1";
                        comoindi.SelectedIndex = 0;
                        MessageBox.Show("Ninguan entidad cumple con esta condicion");
                    }
                }
                else
                {
                    tesdirind.Enabled = false;
                    tesdirind.Items.Clear();
                    tesdirind.Text = "-1";
                    // comoindi.SelectedIndex = 0;
                }
            }
        }

        private void botmodiat_Click(object sender, EventArgs e)
        {
            CEntidad entidad = dicio.dameRegistrosEntidad()[comboEnat.SelectedIndex];
       bool puedo  =   buscarfora();
            if (!puedo)
            {
                List<CAtributo> istaAtributos = dicio.dameRegistrosAtributo(entidad.atrib);
                bool exit = false;
                for (int i = 0; i < istaAtributos.Count; i++)
                {
                    string du = new string(istaAtributos[i].asignombre).Replace(" ", "");
                    if (du == texat.Text)
                    {
                        exit = true;
                        break;
                    }
                    else
                    {

                    }
                }
                if (entidad.asigdat == (long)-1 && !exit)
                {
                    char op = ' ';
                    if (Cobti.SelectedIndex == 0)
                    {
                        op = 'C';
                    }
                    if (Cobti.SelectedIndex == 1)
                    {
                        op = 'E';
                    }
                    if (Cobti.SelectedIndex == 2)
                    {
                        op = 'F';
                    }


                    int pp = comoindi.SelectedIndex;
                    dicio.actualizaAtributo(comboEnat.SelectedIndex,
                                    Comoatri.SelectedIndex,
                                    texat.Text,
                                   op,
                                    comoindi.SelectedIndex,
                                    Convert.ToInt32(textlong.Text), retorindi(tesdirind.Text));
                    Panelsecundario.SelectedIndex = 0;
                    Panelsecundario.SelectedIndex = 1;
                }
                else
                {
                    char op = ' ';
                    if (Cobti.SelectedIndex == 0)
                    {
                        op = 'C';
                    }
                    if (Cobti.SelectedIndex == 1)
                    {
                        op = 'E';
                    }
                    if (Cobti.SelectedIndex == 2)
                    {
                        op = 'F';
                    }

                    if (entidad.asigdat == (long)-1 && exit && Comoatri.Text != "Nuevo Atributo")
                    {

                        int pp = comoindi.SelectedIndex;
                        dicio.actualizaAtributo(comboEnat.SelectedIndex,
                                        Comoatri.SelectedIndex,
                                        texat.Text,
                                        op,
                                        comoindi.SelectedIndex,
                                        Convert.ToInt32(textlong.Text), retorindi(tesdirind.Text));
                        Panelsecundario.SelectedIndex = 0;
                        Panelsecundario.SelectedIndex = 1;
                    }
                    else
                        MessageBox.Show("Error no puedes modificar el atributo ya contiene datos o esta mal los datos");
                } 
            }
            else
                 MessageBox.Show("Error no puedes modificar por clave foranea");

        }
        private void botelimiatrib_Click(object sender, EventArgs e)
        {
            CEntidad entidad = dicio.dameRegistrosEntidad()[comboEnat.SelectedIndex];
            bool puedo = buscarfora();
            if (!puedo)
            {
                if (entidad.asigdat == (long)-1)
                {
                    dicio.eliminarAtributo(comboEnat.SelectedIndex,
                                     Comoatri.SelectedIndex);
                    Panelsecundario.SelectedIndex = 0;
                    Panelsecundario.SelectedIndex = 1;
                }
                else
                {
                    MessageBox.Show("Error no puedes eliminar la entidad ya contiene datos o esta mal los datos");
                }
            }
            else
                MessageBox.Show("Error no puedes eliminar por clave foranea debes eliminar esa relacion con los atributos de la entidad");

        }
        private void tesdirind_SelectedIndexChanged(object sender, EventArgs e)
        {
            CEntidad entidad = dicio.dameRegistrosEntidad()[comboEnat.SelectedIndex];

            List<CEntidad> axui = dicio.dameRegistrosEntidad();
            for (int i = 0; i < axui.Count; i++)
            {
                if (axui[i].atrib != 1 && new string(axui[i].asignombre).Replace(" ", "") == tesdirind.Text)
                {
                    List<CAtributo> axu2 = dicio.dameRegistrosAtributo(axui[i].atrib);

                    if (axu2.Count > 0)
                    {
                        if (axu2[0].tipoat == 'C')
                        {
                            Cobti.SelectedIndex = 0;
                        }
                        if (axu2[0].tipoat == 'E')
                        {
                            Cobti.SelectedIndex = 1;
                        }
                        if (axu2[0].tipoat == 'F')
                        {
                            Cobti.SelectedIndex = 2;
                        }


                        textlong.Text = axu2[0].longitud.ToString();
                        break;
                    }
                    else
                    {
                        MessageBox.Show("Hubo un error la entidad no contiene atributos");
                    }
                }
                else
                {

               //     MessageBox.Show("Hubo un error en los datos no puede agregar clave foranea");
                }

            }

        }
        #endregion

        #region panel 2
        private void camboregsi_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (camboregsi.SelectedIndex == camboregsi.Items.Count - 1)
                iniciagrod();
            else
                iniciagrod2();
        }
        private void iniciagrod()
        {
            dicio.llenalista();
            data1.Rows.Clear();
            data1.Columns.Clear();
            DataGridViewTextBoxColumn mp = new DataGridViewTextBoxColumn();
            mp.HeaderText = "Cab: " + dicio.buscacabecer();
            data1.Columns.Add(mp);
            for (int i = 0; i < 8; i++)
            {
                DataGridViewTextBoxColumn n = new DataGridViewTextBoxColumn();
                n.HeaderText = "";
                data1.Columns.Add(n);
            }

            //data1.Rows.Add();
            //  DataGridViewTextBoxColumn  m = new DataGridViewTextBoxColumn();
            // m.HeaderText = "Nombre";
            //data1.Rows.Add("Cabfff: " + dicio.buscacabecer());
            //data1.Columns.Add(m);

            int e = data1.Rows.Add();
            data1.Rows[e].Cells[0].Value = "Nombre";
            data1.Rows[e].Cells[1].Value = "DE/DA";
            data1.Rows[e].Cells[2].Value = "DA/TD";
            data1.Rows[e].Cells[3].Value = "DD/LD";
            data1.Rows[e].Cells[4].Value = "DSE/TI";
            data1.Rows[e].Cells[5].Value = "/DI";
            data1.Rows[e].Cells[6].Value = "/DSA";

            dicio.orden();
            int j = dicio.regresaorde();

            for (int i = 0; i < j; i++)
            {
                int p = data1.Rows.Add();
                if (dicio.soy(dicio.miposi(i)) == 0)
                {
                    data1.Rows[p].Cells[0].Value = dicio.regenom(dicio.miposi(i)).ToString();
                    data1.Rows[p].Cells[1].Value = dicio.regreente(dicio.miposi(i)).ToString();
                    data1.Rows[p].Cells[2].Value = dicio.regreatribe(dicio.miposi(i)).ToString();
                    data1.Rows[p].Cells[3].Value = dicio.regdato(dicio.miposi(i)).ToString();
                    data1.Rows[p].Cells[4].Value = dicio.regsige(dicio.miposi(i)).ToString();
                    //data1.Rows[p].Cells[5].Value = "/DI";
                    //data1.Rows[p].Cells[6].Value = "/DSA";
                    long arch = dicio.regreatribe(dicio.miposi(i));
                    List<CAtributo> aux = dicio.dameRegistrosAtributo(arch);
                    for (int x = 0; x < aux.Count; x++)
                    {
                        int po = data1.Rows.Add();
                        data1.Rows[po].Cells[0].Value = new string(aux[x].asignombre).Replace(" ", "");
                        data1.Rows[po].Cells[1].Value = aux[x].asigdireccion.ToString();
                        data1.Rows[po].Cells[2].Value = aux[x].tipoat.ToString();
                        data1.Rows[po].Cells[3].Value = aux[x].longitud.ToString();
                        data1.Rows[po].Cells[4].Value = aux[x].tipoind.ToString();
                        data1.Rows[po].Cells[5].Value = aux[x].ind.ToString();
                        data1.Rows[po].Cells[6].Value = aux[x].atrib.ToString();
                    }
                }
                else
                {
                    if (dicio.soy(dicio.miposi(i)) == 1)
                    {

                    }
                }
            }



        }
        private void iniciagrod2()
        {
            dicio.llenalista();
            data1.Rows.Clear();
            data1.Columns.Clear();
            DataGridViewTextBoxColumn mp = new DataGridViewTextBoxColumn();
            mp.HeaderText = "Cab: " + dicio.buscacabecer();
            data1.Columns.Add(mp);
            for (int o = 0; o < 8; o++)
            {
                DataGridViewTextBoxColumn n = new DataGridViewTextBoxColumn();
                n.HeaderText = "";
                data1.Columns.Add(n);
            }
            int e = data1.Rows.Add();
            data1.Rows[e].Cells[0].Value = "Nombre";
            data1.Rows[e].Cells[1].Value = "DE/DA";
            data1.Rows[e].Cells[2].Value = "DA/TD";
            data1.Rows[e].Cells[3].Value = "DD/LD";
            data1.Rows[e].Cells[4].Value = "DSE/TI";
            data1.Rows[e].Cells[5].Value = "/DI";
            data1.Rows[e].Cells[6].Value = "/DSA";

            dicio.orden();
            int j = dicio.regresaorde();
            int i = camboregsi.SelectedIndex;

            int p = data1.Rows.Add();
            if (dicio.soy(dicio.miposi(i)) == 0)
            {
                data1.Rows[p].Cells[0].Value = dicio.regenom(dicio.miposi(i)).ToString();
                data1.Rows[p].Cells[1].Value = dicio.regreente(dicio.miposi(i)).ToString();
                data1.Rows[p].Cells[2].Value = dicio.regreatribe(dicio.miposi(i)).ToString();
                data1.Rows[p].Cells[3].Value = dicio.regdato(dicio.miposi(i)).ToString();
                data1.Rows[p].Cells[4].Value = dicio.regsige(dicio.miposi(i)).ToString();
                //data1.Rows[p].Cells[5].Value = "/DI";
                //data1.Rows[p].Cells[6].Value = "/DSA";
                long arch = dicio.regreatribe(dicio.miposi(i));
                List<CAtributo> aux = dicio.dameRegistrosAtributo(arch);
                for (int x = 0; x < aux.Count; x++)
                {
                    int po = data1.Rows.Add();
                    data1.Rows[po].Cells[0].Value = new string(aux[x].asignombre).Replace(" ", "");
                    data1.Rows[po].Cells[1].Value = aux[x].asigdireccion.ToString();
                    data1.Rows[po].Cells[2].Value = aux[x].tipoat.ToString();
                    data1.Rows[po].Cells[3].Value = aux[x].longitud.ToString();
                    data1.Rows[po].Cells[4].Value = aux[x].tipoind.ToString();
                    data1.Rows[po].Cells[5].Value = aux[x].ind.ToString();
                    data1.Rows[po].Cells[6].Value = aux[x].atrib.ToString();
                }
            }
            else
            {
                if (dicio.soy(dicio.miposi(i)) == 1)
                {

                }
            }
        }

        private void busca()
        {
            if (auorizo == 1)
            {
                camboregsi.SelectedIndex = comboenti.SelectedIndex;
                auorizo = 0;
            }
        }


        #endregion Registro de datos

        #region Panel 3
        private void CBDatos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CBDatos.Text != "")
            {

                comoorden.Enabled = true;
                CEntidad entidad = dicio.dameRegistrosEntidad()[CBDatos.SelectedIndex];
                List<CAtributo> atributos = dicio.dameRegistrosAtributo(entidad.atrib);
                comboatru.Items.Clear();
                comboatru.Text = "";
                textbusq.Text = "";



                foreach (var item in atributos)
                    comboatru.Items.Add(new string(item.asignombre).Trim());  
                llenaGridRegistros();
            }


        }
        private void comboatru_SelectedIndexChanged(object sender, EventArgs e)
        {
            textbusq.Enabled = true;
        }
        private void textbusq_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (CBDatos.Text != "" && comboatru.Text != "")
                {

                    CEntidad entidad = dicio.dameRegistrosEntidad()[CBDatos.SelectedIndex];
                    List<CAtributo> atributos = dicio.dameRegistrosAtributo(entidad.atrib);
                    dgRegistrosR.Columns.Clear();
                    dgRegistrosR.DataSource = null;
                    dgRegistrosR.Rows.Clear();
                    dgRegistrosR.Refresh();
                    foreach (var item in atributos)
                    {
                        string columna;
                        columna = new string(item.asignombre).Trim();
                        dgRegistrosR.Columns.Add(columna, columna);
                    }
                    dgRegistrosR.Columns.Add("direccion", "DD");
                    dgRegistrosR.Columns.Add("sigDir", "DSD");

                    if (dgInsertaR.Columns.Count != 0)
                    {
                        if (entidad.asigdireccion != -1)
                        {
                            List<Registro> datos = dicio.dameRegistrosDatos(entidad);
                            datos = datos.OrderBy(x => x.dimeDireccionRegstro).ToList();
                            foreach (var item in datos)
                            {
                                string[] columna = new string[dgRegistrosR.Columns.Count];

                                int i = 0;
                                foreach (var atributo in item.dimeAtributosRegistro)
                                {
                                    if (atributos[i].tipoat == 'E')
                                    {
                                        columna[i] = BitConverter.ToInt32(atributo, 0).ToString().Trim();
                                    }
                                    if (atributos[i].tipoat == 'C')
                                    {
                                        columna[i] = Encoding.ASCII.GetString(atributo).Trim();
                                    }
                                    if (atributos[i].tipoat == 'F')
                                    {
                                        columna[i] = BitConverter.ToDouble(atributo, 0).ToString().Trim();
                                    }

                                    i++;
                                }
                                columna[i] = item.dimeDireccionRegstro.ToString();
                                columna[dgRegistrosR.Columns.Count - 1] = item.dimeDireccionSiguienteRegstro.ToString();
                                string resu = columna[comboatru.SelectedIndex].Substring(0, textbusq.Text.Length);
                                if (resu == textbusq.Text.ToString())
                                    dgRegistrosR.Rows.Add(columna);
                            }

                        }

                    }
                    else
                    {
                        dgRegistrosR.Columns.Clear();
                    }
                }
                if (CBDatos.Text != "" && comboatru.Text == "")
                {
                    llenaGridRegistros();
                }
            }
            catch (Exception) { }
        }

        public int dameregistro()
        {
            CEntidad entidad = dicio.dameRegistrosEntidad()[CBDatos.SelectedIndex];
            List<CAtributo> atributos = dicio.dameRegistrosAtributo(entidad.atrib);
            List<Registro> datos = dicio.dameRegistrosDatos(entidad);
            List<List<string>> aui = new List<List<string>>();
            foreach (var item in datos)
            {
                string[] columna = new string[dgRegistrosR.Columns.Count];

                int i = 0;
                List<string> conte = new List<string>();
                foreach (var atributo in item.dimeAtributosRegistro)
                {
                    if (atributos[i].tipoat == 'E')
                    {
                        conte.Add(BitConverter.ToInt32(atributo, 0).ToString().Trim());
                    }
                    if (atributos[i].tipoat == 'C')
                    {
                        conte.Add(Encoding.ASCII.GetString(atributo).Trim());
                    }
                    if (atributos[i].tipoat == 'F')
                    {
                        conte.Add(BitConverter.ToDouble(atributo, 0).ToString().Trim());
                    }

                    i++;
                }
                aui.Add(conte);
            }

            List<string> separa = new List<string>();
            for (int j = 0; j < cadenaor.Count - 2; j++)
            {
                separa.Add(cadenaor[j]);
            }

            for (int j = 0; j < aui.Count; j++)
            {
                if (aui[j].SequenceEqual(separa))
                    return j;
            }

            return -1;
        }
        List<List<string>> aui2 = new List<List<string>>();

        public void llenareg()
        {
            CEntidad entidad = dicio.dameRegistrosEntidad()[CBDatos.SelectedIndex];
            List<CAtributo> atributos = dicio.dameRegistrosAtributo(entidad.atrib);
            List<Registro> datos = dicio.dameRegistrosDatos(entidad);
            aui2.Clear();
            foreach (var item in datos)
            {
                string[] columna = new string[dgRegistrosR.Columns.Count];

                int i = 0;
                List<string> conte = new List<string>();
                foreach (var atributo in item.dimeAtributosRegistro)
                {
                    if (atributos[i].tipoat == 'E')
                    {
                        conte.Add(BitConverter.ToInt32(atributo, 0).ToString().Trim());
                    }
                    if (atributos[i].tipoat == 'C')
                    {
                        conte.Add(Encoding.ASCII.GetString(atributo).Trim());
                    }
                    if (atributos[i].tipoat == 'F')
                    {
                        conte.Add(BitConverter.ToDouble(atributo, 0).ToString().Trim());
                    }

                    i++;
                }
                aui2.Add(conte);
            }
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////

        }
        public int reger()
        {
            List<string> separa = new List<string>();
            for (int j = 0; j < cadenaor2.Count - 2; j++)
            {
                separa.Add(cadenaor2[j]);
            }

            for (int j = 0; j < aui2.Count; j++)
            {
                if (aui2[j].SequenceEqual(separa))
                    return j;
            }

            return -1;
        }
        public int primarioexi(int x)
        {
            List<string> sep = new List<string>();
          sep = dameInformacionGridDatos();

            CEntidad entidad = dicio.dameRegistrosEntidad()[CBDatos.SelectedIndex];
            List<CAtributo> atributos = dicio.dameRegistrosAtributo(entidad.atrib);
            int q = dicio.verificaSiExisteIdxPrimario(atributos);
            for (int j = 0; j < aui2.Count; j++)
            {
                if (aui2[j][q].SequenceEqual(sep[q])&& j!=x)
                    return j;
            }

            return -1;
        }
        public int arbolexi(int x)
        {
            List<string> sep = new List<string>();
            sep = dameInformacionGridDatos();

            CEntidad entidad = dicio.dameRegistrosEntidad()[CBDatos.SelectedIndex];
            List<CAtributo> atributos = dicio.dameRegistrosAtributo(entidad.atrib);
            int q = dicio.existeIdxArbolPrimario(atributos);
            for (int j = 0; j < aui2.Count; j++)
            {
                if (aui2[j][q].SequenceEqual(sep[q]) && j != x)
                    return j;
            }

            return -1;
        }
        public void llenaGridRegistros()
        {
            CEntidad entidad = dicio.dameRegistrosEntidad()[CBDatos.SelectedIndex];
            List<CAtributo> atributos = dicio.dameRegistrosAtributo(entidad.atrib);
           // CEntidad entidad2 = dicio.dameRegistrosEntidad()[1];
            dgInsertaR.Columns.Clear();
            dgRegistrosR.Columns.Clear();


            foreach (var item in atributos)
            {
                string columna;
                columna = new string(item.asignombre).Trim();
                if (item.tipoind == 8)
                 {
                    //if()
                    DataGridViewComboBoxColumn cmb = new DataGridViewComboBoxColumn();
                    cmb.HeaderText = columna;
                    cmb.Name = "cmb";
                    //  cmb.MaxDropDownItems = 4;
                    List<Registro> datos = dicio.dameRegistrosDatos(rtornporin(item.ind));
                    foreach (var itemdat in datos)
                    {   }
                    List<CAtributo> atributos2 = dicio.dameRegistrosAtributo(rtornporin(item.ind).atrib);
                    for (int ip=0; ip<datos.Count;ip++)
                    {
                        foreach (var atributo in datos[ip].dimeAtributosRegistro)
                        {
                            if (atributos2[0].tipoat == 'E')
                            {
                                cmb.Items.Add( BitConverter.ToInt32(atributo, 0).ToString().Trim());
                                break;
                            }
                            if (atributos2[0].tipoat == 'C')
                            {
                                cmb.Items.Add(Encoding.ASCII.GetString(atributo).Trim());
                                break;
                            }
                            if (atributos2[0].tipoat == 'F')
                            {
                                cmb.Items.Add(BitConverter.ToSingle(atributo, 0).ToString().Trim());
                                break;
                            }


                         
                        }

                    }     
                    if(cmb.Items.Count==0)
                    {
                        MessageBox.Show("Un atributo contine clave foranea pero no hay datos en la entidad a la que apunta agrea datos");
                        Panelsecundario.SelectedIndex = 4;
                        Panelsecundario.SelectedIndex = 3;
                        dgInsertaR.Columns.Clear();
                        dgRegistrosR.Columns.Clear();
                        break;
                    }
                    dgInsertaR.Columns.Add(cmb);

                }
                else
                { 
                    dgInsertaR.Columns.Add(columna, columna);
                }
            }

            foreach (var item in atributos)
            {
                string columna;
                columna = new string(item.asignombre).Trim();
                dgRegistrosR.Columns.Add(columna, columna);

            }
            dgRegistrosR.Columns.Add("direccion", "DD");
            dgRegistrosR.Columns.Add("sigDir", "DSD");

            if (dgInsertaR.Columns.Count != 0)
            {
                if (entidad.asigdireccion != -1)
                {
                    List<Registro> datos = dicio.dameRegistrosDatos(entidad);
               //    datos= datos.OrderBy(x => x.dimeDireccionRegstro).ToList();
                    foreach (var item in datos)
                    {
                        string[] columna = new string[dgRegistrosR.Columns.Count];

                        int i = 0;
                        foreach (var atributo in item.dimeAtributosRegistro)
                        {

                            if (atributos[i].tipoat == 'E')
                            {
                                columna[i ] = BitConverter.ToInt32(atributo, 0).ToString().Trim();
                            }
                            if (atributos[i].tipoat == 'C')
                            {
                                columna[i] = Encoding.ASCII.GetString(atributo).Trim();
                            }
                            if (atributos[i].tipoat == 'F')
                            {
                                columna[i] = BitConverter.ToDouble(atributo, 0).ToString().Trim();
                            }

                            i++;
                        }
                        columna[i] = item.dimeDireccionRegstro.ToString();
                        columna[dgRegistrosR.Columns.Count - 1] = item.dimeDireccionSiguienteRegstro.ToString();
                        dgRegistrosR.Rows.Add(columna);
                    }
                }
            }
            else
            {
                dgRegistrosR.Columns.Clear();
            }
            float x = 2500000.8778f;
        }
        private CEntidad rtornporin(long dire)
        {
            List<CEntidad> axui = dicio.dameRegistrosEntidad();
            for (int i = 0; i < axui.Count; i++)
            {
                if (axui[i].asigdireccion == dire)
                    return axui[i];
            }
            return null;
        }

        private void comoorden_SelectedIndexChanged(object sender, EventArgs e)
        {
            btInsertarR.Enabled = true;
            btModificarR.Enabled = true;
            btEliminarR.Enabled = true;
        }

        private void btInsertarR_Click(object sender, EventArgs e)
        {
            int idx_entidad = CBDatos.SelectedIndex;
            comoorden.SelectedIndex = 0;
            if (!dicio.agregaRegistroDatos(idx_entidad, dameInformacionGridDatos(), null, comoorden.SelectedIndex))
            {
                MessageBox.Show("Error al ingresar Registro: \n Pudo haber sido:" +
                    "   -   Mal ingreso de datos" +
                    "   -   Registro con el valor del índice primario ya existe." , "Error");
                // +
                 //   "   -   Registro con el valor del índice secundario ya existe."
            }
            else
                llenaGridRegistros();
        }
        bool buscarfora2()
        {
            List<CEntidad> axui = dicio.dameRegistrosEntidad();
            List<CAtributo> axui2, auxfor;
            CEntidad entidad = dicio.dameRegistrosEntidad()[CBDatos.SelectedIndex];
            List<CAtributo> atributos = dicio.dameRegistrosAtributo(entidad.atrib);
            List<List<string>> buscador = new List<List<string>>();
            if (entidad.asigdireccion != -1)
            {
                List<Registro> datos = dicio.dameRegistrosDatos(entidad);

                //    datos= datos.OrderBy(x => x.dimeDireccionRegstro).ToList();
                foreach (var item in datos)
                {
                    // string[] columna = new string[dgRegistrosR.Columns.Count];

                          List<string> auxiliar1 = new List<string>();                int i = 0;
                    foreach (var atributo in item.dimeAtributosRegistro)
                    {
 
                        if (atributos[i].tipoat == 'E')
                        {
                            auxiliar1.Add(BitConverter.ToInt32(atributo, 0).ToString().Trim());
                        }
                        if (atributos[i].tipoat == 'C')
                        {
                            auxiliar1.Add(Encoding.ASCII.GetString(atributo).Trim());
                        }
                        if (atributos[i].tipoat == 'F')
                        {
                            auxiliar1.Add(BitConverter.ToDouble(atributo, 0).ToString().Trim());
                        }

                        i++;
                    }
                    buscador.Add(auxiliar1);
                }
            }
            else
                MessageBox.Show("Error vuelve a selecionar");
            int obstaculo=-1;
            for (int i = 0; i < axui.Count; i++)
            {
                if (CBDatos.Text == new string(axui[i].asignombre).Replace(" ", ""))///encontrar entidad actual
                {
               //     axui2 = dicio.dameRegistrosAtributo(axui[i].atrib);//atributos actuales

                    //  List<CAtributo> axui3 = dicio.regretlieatribut();

                    for (int a = 0; a < axui.Count; a++)//encontrar entidad que me apunta
                    { }
                        for (int p = 0; p < axui.Count; p++)//probar todas las entidades
                        {
                            auxfor = dicio.dameRegistrosAtributo(axui[p].atrib);
                            for (int p3 = 0; p3 < auxfor.Count; p3++)
                            {
                                if ((axui[i].asigdireccion == auxfor[p3].ind))// && a == i)
                                {
                                    obstaculo = p;
                                    break;
                                }
                            }



                        }
                   


                }
            }
            if (obstaculo > -1)
            {
                CEntidad entidad2 = dicio.dameRegistrosEntidad()[obstaculo];

                long punto = entidad.asigdireccion;
                List<CAtributo> atributos2 = dicio.dameRegistrosAtributo(entidad2.atrib);
                List<List<string>> buscador2 = new List<List<string>>();
                List<Registro> datos2 = dicio.dameRegistrosDatos(entidad2);

                //    datos= datos.OrderBy(x => x.dimeDireccionRegstro).ToList();
                foreach (var item in datos2)
                {
                    // string[] columna = new string[dgRegistrosR.Columns.Count];

                    List<string> auxiliar1 = new List<string>();
                    int i = 0;
                    foreach (var atributo in item.dimeAtributosRegistro)
                    {

                        if (atributos2[i].tipoat == 'E')
                        {
                            auxiliar1.Add(BitConverter.ToInt32(atributo, 0).ToString().Trim());
                        }
                        if (atributos2[i].tipoat == 'C')
                        {
                            auxiliar1.Add(Encoding.ASCII.GetString(atributo).Trim());
                        }
                        if (atributos2[i].tipoat == 'F')
                        {
                            auxiliar1.Add(BitConverter.ToSingle(atributo, 0).ToString().Trim());
                        }

                        i++;
                    }
                    buscador2.Add(auxiliar1);

                }
                int fin=-1;
                for (int i = 0; i < atributos2.Count; i++)
                {

                    if (atributos2[i].ind == punto)
                    {
                        fin = i;
                        break;
                    }
                }
                if (fin > -1)
                {
                    for (int i = 0; i < buscador.Count; i++)
                    {  }
                        for (int j = 0; j < buscador2.Count; j++)
                        {
                      //     for (int k = 0; k < buscador.Count; k++)
                        //    {   }
                                if (buscador[dameregistro()][0] == buscador2[j][fin])
                                {
                                    return true;
                                }

                         
                        }
                  
                }
            }
            int p89988;
            return false;


        }

        private void btModificarR_Click(object sender, EventArgs e)
        {
            try
            {
                comoorden.SelectedIndex = 0;
                bool puedo = buscarfora2();
                if (!puedo)
                {
                    switch (comoorden.SelectedIndex)
                    {
                        case 0:
                            int reetido = reger();

                            int res = primarioexi(reetido);
                            if (res == -1)
                            {
                                dicio.asigarchi(dicio.dameRegistrosEntidad()[CBDatos.SelectedIndex]);
                                dicio.modificaRegistroDatos(CBDatos.SelectedIndex, reger(), dameInformacionGridDatos(), comoorden.SelectedIndex);
                                reseteaGridDatos();
                                llenaGridRegistros();
                            }
                            else
                            {
                                MessageBox.Show("Duplicidad en clave primaria");
                            }
                            break;
                        case 1:
                            int reetido1 = reger();

                            int res1 = primarioexi(reetido1);
                            if (res1 == -1)
                            {
                                dicio.asigarchi(dicio.dameRegistrosEntidad()[CBDatos.SelectedIndex]);
                                dicio.modificaRegistroDatos(CBDatos.SelectedIndex, reger(), dameInformacionGridDatos(), comoorden.SelectedIndex);
                                reseteaGridDatos();
                                llenaGridRegistros();
                            }
                            else
                            {
                                MessageBox.Show("Duplicidad en clave primaria");
                            }
                            break;
                        case 2:
                            int reetido2 = reger();

                            int res2 = arbolexi(reetido2);
                            if (res2 == -1)
                            {
                                dicio.asigarchi(dicio.dameRegistrosEntidad()[CBDatos.SelectedIndex]);
                                dicio.modificaRegistroDatos(CBDatos.SelectedIndex, reger(), dameInformacionGridDatos(), comoorden.SelectedIndex);
                                reseteaGridDatos();
                                llenaGridRegistros();
                            }
                            else
                            {
                                MessageBox.Show("Duplicidad en clave primaria de árbol");
                            }
                            break;
                    }

                }
                else
                {
                    MessageBox.Show("Error por clave foranea");
                }
            }
            catch { }
        }
        //Al presionar eliminar en registros

        private void btEliminarR_Click(object sender, EventArgs e)
        {
            try
            {
                bool puedo = buscarfora2();
                if (!puedo)
                {
                    comoorden.SelectedIndex = 0;
                    dicio.asigarchi(dicio.dameRegistrosEntidad()[CBDatos.SelectedIndex]);
                    dicio.eliminaRegistroDatos(CBDatos.SelectedIndex, dameregistro(), comoorden.SelectedIndex);
                    reseteaGridDatos();
                    llenaGridRegistros();
                }
                else
                {
                    MessageBox.Show("Error por clave foranea");
                }
            }
            catch { }
        }
        private List<string> cadenaor;
        private List<string> cadenaor2;
        private void dgRegistrosR_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                CEntidad registro = dicio.dameRegistrosEntidad()[CBDatos.SelectedIndex];
                List<CAtributo> atributos = dicio.dameRegistrosAtributo(registro.atrib);
                List<Registro> datos = dicio.dameRegistrosDatos(registro);

                List<string> cadenas = dameInformacionGridRegistros(e.RowIndex);
                cadenaor = dameInformacionGridRegistros(e.RowIndex);
                cadenaor2 = dameInformacionGridRegistros(e.RowIndex);
                llenareg();
                for (int i = 1; i < cadenas.Count - 1; i++)
                {
                    dgInsertaR.Rows[0].Cells[i - 1].Value = cadenas[i - 1];
                }
            }
            catch { }
        }
        //Poner el dataGrid de los registros en blanco
        private void reseteaGridDatos()
        {
            foreach (DataGridViewCell item in dgInsertaR.Rows[0].Cells)
            {
                item.Value = "";
            }
        }
        List<string> dameInformacionGridDatos()
        {
            List<string> temporal = new List<string>();
            foreach (DataGridViewCell item in dgInsertaR.Rows[0].Cells)
            {
                try
                {
                    temporal.Add(item.Value.ToString());
                }
                catch
                {
                    temporal.Add("");
                }
            }
            return temporal;
        }
        List<string> dameInformacionGridRegistros(int index)
        {
            List<string> temporal = new List<string>();
            foreach (DataGridViewCell item in dgRegistrosR.Rows[index].Cells)
            {
                try
                {
                    temporal.Add(item.Value.ToString());
                }
                catch
                {
                    temporal.Add("");
                }
            }
            return temporal;
        }

        #endregion

        private void cbEntidadIndices_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargaAtributosIdxPrim(dicio.dameRegistrosEntidad()[cbEntidadIndices.SelectedIndex]);
        }

   
        public void cargaAtributosIdxPrim(CEntidad registro)
        {
            dicio.asigarchi(registro);
            int index_atributos1 = cbAtributoIndicePrimarioI.SelectedIndex;
            cbAtributoIndicePrimarioI.Items.Clear();
            try
            {
                foreach (var item in dicio.dameRegistrosAtributo(registro.atrib))
                {
                    if (item.tipoind == 2)
                        cbAtributoIndicePrimarioI.Items.Add(new string(item.asignombre).Trim());
                }
                cbAtributoIndicePrimarioI.SelectedIndex = index_atributos1;
            }
            catch (Exception)
            {

            }
        }

        private void cbAtributoIndicePrimarioI_SelectedIndexChanged(object sender, EventArgs e)
        {
            llenaGridPrimario();
        }
        public void llenaGridPrimario()
        {
            if (cbAtributoIndicePrimarioI.Items.Count > 0)
            {
                int idxEntidad = cbEntidadIndices.SelectedIndex;
                List<CAtributo> atributos = dicio.dameRegistrosAtributo(dicio.dameRegistrosEntidad()[idxEntidad].atrib);
                int idxAtributo = dameIndicePrimario(atributos, cbAtributoIndicePrimarioI.SelectedIndex);
                Dictionary<byte[], long> diccionario = dicio.dameArreglo(atributos[idxAtributo],(long)0);

                List<byte[]> zlk = new List<byte[]>(diccionario.Keys);

                List<long> zlv = new List<long>();


                Dictionary<byte[], long> diccionario2 = new Dictionary<byte[], long>();

                List<int> listaentera = new List<int>();
                List<string> listastring= new List<string>();
                foreach (var item in diccionario)
                {
                    if (atributos[idxAtributo].tipoat == 'E')
                    {
                        string x = BitConverter.ToInt32(item.Key, 0).ToString().Trim();
     
                        int p = BitConverter.ToInt32(item.Key, 0);
                        // if (BitConverter.ToInt32(datoSecundario, 0).ToString().Trim() == BitConverter.ToInt32(item.Key, 0).ToString().Trim())
                        {
                            byte[] intBytes = BitConverter.GetBytes(p);
                            listaentera.Add(p);
                            byte[] result = intBytes;
                        }
                    }
                    else
                    {
                        string y = Encoding.ASCII.GetString(item.Key).Trim();
                        // string y2 = Encoding.ASCII.GetString(datoSecundario).Trim();

                        listastring.Add(y);

                    }

                }
                if (atributos[idxAtributo].tipoat == 'E')
                {
                    listaentera.Sort();
                    List<byte[]> axi = new List<byte[]>();
                    foreach (var item in listaentera)
                    {
                        axi.Add(BitConverter.GetBytes(item));
                    }
                    foreach (var item in axi)
                    {
                        foreach (var ax in diccionario)
                        {
                            string s1, s2;
                            s1 = BitConverter.ToInt32(item, 0).ToString().Trim();
                            s2 = BitConverter.ToInt32(ax.Key, 0).ToString().Trim();
                            if (s1==s2)
                     zlv.Add(ax.Value);
                        }
       

                    }
                    for (int i = 0; i < axi.Count; i++)
                    {
                        diccionario2.Add(axi[i], zlv[i]);
                    }

                }
                else
                {
                    listastring.Sort();
                    List<byte[]> axi = new List<byte[]>();
                    foreach (var item in listastring)
                    {
                        axi.Add(Encoding.ASCII.GetBytes(item));
                    }
                    foreach (var item in axi)
                    {
                        foreach (var ax in diccionario)
                        {
                            string s1, s2;
                            s1 = BitConverter.ToInt32(item, 0).ToString().Trim();
                            s2 = BitConverter.ToInt32(ax.Key, 0).ToString().Trim();
                            if (s1 == s2)
                                zlv.Add(ax.Value);
                        }


                    }
                    for (int i = 0; i < axi.Count; i++)
                    {
                        diccionario2.Add(axi[i], zlv[i]);
                    }

                }

                    char tipo = atributos[idxAtributo].tipoat;
                dgIndicePrimarioI.Rows.Clear();

                string[] filas = new string[2];
                foreach (var item in diccionario2)
                {
                    if (tipo == 'E')
                        filas[0] = BitConverter.ToInt32(item.Key, 0).ToString().Trim();
                    else
                        filas[0] = Encoding.ASCII.GetString(item.Key).Trim();
                    filas[1] = item.Value.ToString().Trim();

                    dgIndicePrimarioI.Rows.Add(filas);
                }
            }
        }
        //Obtener el index del atributo con indice primario
        public int dameIndicePrimario(List<CAtributo> atributos, int index)
        {
            int res = 0, cuenta = 0;
            foreach (CAtributo atributo in atributos)
            {
                if (atributo.tipoind == 2)
                {
                    if (cuenta == index)
                        break;
                    else
                        cuenta++;
                }
                res++;
            }
            return res;
        }
        //Método para cargar los aatributos con indice primario


        private void Form1_Paint(object sender, PaintEventArgs e)
        {
         
        }

        private void cbEntidadIndiceSecundario_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargaAtributosIdxSecu(dicio.dameRegistrosEntidad()[cbEntidadIndiceSecundario.SelectedIndex]);
        }
        public void cargaAtributosIdxSecu(CEntidad registro)
        {
            int index_atributos1 = cbAtributoIndiceSecundario.SelectedIndex;
            cbAtributoIndiceSecundario.Items.Clear();
            try
            {
                //List<CAtributo> art = dicio.dameRegistrosAtributo(registro.atrib);
              //  art.Sort();
                foreach (var item in dicio.dameRegistrosAtributo(registro.atrib))
                {
                    if (item.tipoind == 3)
                        cbAtributoIndiceSecundario.Items.Add(new string(item.asignombre).Trim());
                }
                cbAtributoIndiceSecundario.SelectedIndex = index_atributos1;
            }
            catch (Exception)
            {

            }
        }

        private void cbAtributoIndiceSecundario_SelectedIndexChanged(object sender, EventArgs e)
        {
            CEntidad registro = dicio.dameRegistrosEntidad()[cbEntidadIndiceSecundario.SelectedIndex];
            dicio.asigarchi(registro);
            cargaDirecciones(registro,
                dameIndiceSecundario(
                    dicio.dameRegistrosAtributo(registro.atrib),
                    cbEntidadIndiceSecundario.SelectedIndex));
            llenaGridSecundario();
        }
        //Método para cargar las direcciones del bloque en el indice secundario
        public void cargaDirecciones(CEntidad registro, int idexAtributo)
        {
            int indexAtributos1 = cbAtributoIndices.SelectedIndex;
            CAtributo atributo = dicio.dameRegistrosAtributo(registro.atrib)[idexAtributo];
            cbAtributoIndices.Items.Clear();
            Dictionary<byte[], long> bloque = dicio.dameArreglo(atributo,atributo.ind);
            try
            {
                List<int> listaentera = new List<int>();
                List<string> listastring = new List<string>();



                foreach (var item in bloque)
                {
                    if (atributo.tipoat == 'E')
                    {
                        int dato = BitConverter.ToInt32(item.Key, 0);
                        listaentera.Add(dato);
                   //     if (!cbAtributoIndices.Items.Contains(dato))
                     //       cbAtributoIndices.Items.Add(dato);
                        
                    }
                    else
                    {
                        string dato = Encoding.ASCII.GetString(item.Key);
                        listastring.Add(dato);
                     //   if (!cbAtributoIndices.Items.Contains(dato))
                      //      cbAtributoIndices.Items.Add(dato);
                    }
                }
                listaentera.Sort();
                listastring.Sort();
                if (atributo.tipoat == 'E')
                {
                    foreach (var item in listaentera)
                    {
                        cbAtributoIndices.Items.Add(item);
                    }
                }
                else
                {
                    foreach (var item in listastring)
                    {
                        cbAtributoIndices.Items.Add(item);
                    }
                }
                    cbAtributoIndices.SelectedIndex = indexAtributos1;
            }
            catch (Exception)
            {

            }
        }
        //Obtener el index del atributo con indice secundario
        public int dameIndiceSecundario(List<CAtributo> atributos, int index)
        {
            int res = 0, cuenta = 0;
            foreach (var item in atributos)
            {
                if (item.tipoind == 3)
                {
                    if (cuenta == index)
                        break;
                    else
                        cuenta++;
                }
                res++;
            }
            return res;
        }
        //Llenar el dataGrid secundario
        public void llenaGridSecundario()
        {
            if (cbAtributoIndiceSecundario.Items.Count > 0)
            {
                int idxEntidad = cbEntidadIndiceSecundario.SelectedIndex;
                CEntidad entidad = dicio.dameRegistrosEntidad()[idxEntidad];

                dicio.asigarchi(entidad);
                List<CAtributo> atributos = dicio.dameRegistrosAtributo(entidad.atrib);
                int idxAtributo = dameIndiceSecundario(atributos, cbAtributoIndiceSecundario.SelectedIndex);
                Dictionary<byte[], long> diccionario = dicio.dameArreglo(atributos[idxAtributo], atributos[idxAtributo].ind);

                char tipo = atributos[idxAtributo].tipoat;
                dgIndiceSecundarioI.Rows.Clear();

                List<byte[]> zlk = new List<byte[]>(diccionario.Keys);

                List<long> zlv = new List<long>();


                Dictionary<byte[], long> diccionario2 = new Dictionary<byte[], long>();
                List<int> listaentera = new List<int>();
                List<string> listastring = new List<string>();
                foreach (var item in diccionario)
                {
                    if (atributos[idxAtributo].tipoat == 'E')
                    {
                        string x = BitConverter.ToInt32(item.Key, 0).ToString().Trim();

                        int p = BitConverter.ToInt32(item.Key, 0);
                        // if (BitConverter.ToInt32(datoSecundario, 0).ToString().Trim() == BitConverter.ToInt32(item.Key, 0).ToString().Trim())
                        {
                            byte[] intBytes = BitConverter.GetBytes(p);
                            listaentera.Add(p);
                            byte[] result = intBytes;
                        }
                    }
                    else
                    {
                        string y = Encoding.ASCII.GetString(item.Key).Trim();
                        // string y2 = Encoding.ASCII.GetString(datoSecundario).Trim();

                        listastring.Add(y);

                    }

                }
                if (atributos[idxAtributo].tipoat == 'E')
                {
                    listaentera.Sort();
                    List<byte[]> axi = new List<byte[]>();
                    foreach (var item in listaentera)
                    {
                        axi.Add(BitConverter.GetBytes(item));
                    }
                    foreach (var item in axi)
                    {
                        foreach (var ax in diccionario)
                        {
                            string s1, s2;
                            s1 = BitConverter.ToInt32(item, 0).ToString().Trim();
                            s2 = BitConverter.ToInt32(ax.Key, 0).ToString().Trim();
                            if (s1 == s2)
                                zlv.Add(ax.Value);
                        }


                    }
                    for (int i = 0; i < axi.Count; i++)
                    {
                        diccionario2.Add(axi[i], zlv[i]);
                    }

                }
                else
                {
                    listastring.Sort();
                    List<byte[]> axi = new List<byte[]>();
                    foreach (var item in listastring)
                    {
                        axi.Add(Encoding.ASCII.GetBytes(item));
                    }
                    foreach (var item in axi)
                    {
                        foreach (var ax in diccionario)
                        {
                            string s1, s2;
                            s1 = BitConverter.ToInt32(item, 0).ToString().Trim();
                            s2 = BitConverter.ToInt32(ax.Key, 0).ToString().Trim();
                            if (s1 == s2)
                                zlv.Add(ax.Value);
                        }


                    }
                    for (int i = 0; i < axi.Count; i++)
                    {
                        diccionario2.Add(axi[i], zlv[i]);
                    }

                }



                string[] filas = new string[2];
                foreach (var item in diccionario2)
                {
                    if (tipo == 'E')
                        filas[0] = BitConverter.ToInt32(item.Key, 0).ToString().Trim();
                    else
                        filas[0] = Encoding.ASCII.GetString(item.Key).Trim();
                    filas[1] = item.Value.ToString().Trim();

                    dgIndiceSecundarioI.Rows.Add(filas);
                }

                if (cbAtributoIndices.SelectedIndex >= 0)
                {
                    List<long> bloque_denso = dicio.dameBloqueDeDirecciones(atributos[idxAtributo],
                                diccionario2.ElementAt(cbAtributoIndices.SelectedIndex).Value);
                    dgDireccionesI.Rows.Clear();
                    foreach (var item in bloque_denso)
                    {
                        dgDireccionesI.Rows.Add(item.ToString());
                    }
                }
                else
                    dgDireccionesI.Rows.Clear();
            }

        }
        public class ByteListComparer : IComparer<IList<byte>>
        {
            public int Compare(IList<byte> x, IList<byte> y)
            {
                int result;
                for (int index = 0; index < Math.Min(x.Count, y.Count); index++)
                {
                    result = x[index].CompareTo(y[index]);
                    if (result != 0) return result;
                }
                return x.Count.CompareTo(y.Count);
            }
        }

        private void cbAtributoIndices_SelectedIndexChanged(object sender, EventArgs e)
        {
            llenaGridSecundario(); 
        }

        private void cbEntidadArbolPrimario_SelectedIndexChanged(object sender, EventArgs e)
        {

                cargaAtributosIdxPrimArbol(dicio.dameRegistrosEntidad()[cbEntidadArbolPrimario.SelectedIndex]);
         
        }
        //Método para cargar los aatributos con indice árbol primario
        public void cargaAtributosIdxPrimArbol(CEntidad registro)
        {
            int index_atributos1 = cbSeleccionArbolPrimario.SelectedIndex;
            dicio.asigarchi(registro);
            cbSeleccionArbolPrimario.Items.Clear();
            try
            {
                foreach (var item in dicio.dameRegistrosAtributo(registro.atrib))
                {
                    if (item.tipoind == 4)// || item.tipoind == 2)
                        cbSeleccionArbolPrimario.Items.Add(new string(item.asignombre).Trim());
                }
                cbSeleccionArbolPrimario.SelectedIndex = index_atributos1;
            }
            catch (Exception)
            {

            }
        }

        private void cbSeleccionArbolPrimario_SelectedIndexChanged(object sender, EventArgs e)
        {
            llenaGridArbolPrimario();
        }
        //Llenar el dataGrid del árbol primario
        public void llenaGridArbolPrimario()
        {


            if (cbSeleccionArbolPrimario.Items.Count > 0)
            {
                int idxEntidad = cbEntidadArbolPrimario.SelectedIndex;

                List<CAtributo> atributos = dicio.dameRegistrosAtributo(
                                            dicio.dameRegistrosEntidad()[idxEntidad].atrib);
                int idxAtributo = dameIndicePrimarioArbol(atributos, cbSeleccionArbolPrimario.SelectedIndex);
                List<Nodo> nodos = dicio.dameNodos(atributos[idxAtributo]);
                nodos = nodos.OrderBy(x => x.direccion).ToList();
                dgArbolPrimarioA.Rows.Clear();

                string[] filas = new string[11];
                foreach (Nodo nodo in nodos)
                {
                    filas[0] = nodo.direccion.ToString();
                    filas[1] = nodo.tipo.ToString();

                    for (int i = 0; i < 4; i++)
                    {
                        filas[2 * (i + 1)] = nodo.listaDirecciones[i].ToString();
                        filas[2 * (i + 1) + 1] = nodo.listaClaves[i].ToString();
                    }
                    filas[10] = nodo.listaDirecciones[CDiccionario.gradoArbol - 1].ToString();
                    dgArbolPrimarioA.Rows.Add(filas);
                }


            }
        }
        private void orenacolumna()
        {
            // Check which column is selected, otherwise set NewColumn to null.
            DataGridViewColumn newColumn =
                dgArbolPrimarioA.Columns.GetColumnCount(
                DataGridViewElementStates.Selected) == 1 ?
                dgArbolPrimarioA.SelectedColumns[1] : null;

            DataGridViewColumn oldColumn = dgArbolPrimarioA.SortedColumn;
            ListSortDirection direction;

            // If oldColumn is null, then the DataGridView is not currently sorted.
            if (oldColumn != null)
            {
                // Sort the same column again, reversing the SortOrder.
                if (oldColumn == newColumn &&
                    dgArbolPrimarioA.SortOrder == SortOrder.Ascending)
                {
                    direction = ListSortDirection.Descending;
                }
                else
                {
                    // Sort a new column and remove the old SortGlyph.
                    direction = ListSortDirection.Ascending;
                    oldColumn.HeaderCell.SortGlyphDirection = SortOrder.None;
                }
            }
            else
            {
                direction = ListSortDirection.Ascending;
            }

            // If no column has been selected, display an error dialog  box.
            if (newColumn == null)
            {
                MessageBox.Show("Select a single column and try again.",
                    "Error: Invalid Selection", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            else
            {
                dgArbolPrimarioA.Sort(newColumn, direction);
                newColumn.HeaderCell.SortGlyphDirection =
                    direction == ListSortDirection.Ascending ?
                    SortOrder.Ascending : SortOrder.Descending;
            }
        }

        //Obtener el index del atributo con indice árbol primario
        public int dameIndicePrimarioArbol(List<CAtributo> atributos, int index)
        {
            int res = 0, cuenta = 0;
            foreach (var item in atributos)
            {
                if (item.tipoind == 4)
                {
                    if (cuenta == index)
                        break;
                    else
                        cuenta++;
                }
                res++;
            }
            return res;
        }

        private void RegistroDatos_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Consulta nueva = new Consulta(textBox1.Text);
            /*foreach (String prueba in nueva.tablas)
            {
                MessageBox.Show("Las tablas son " + prueba);
            }/*
            foreach (String prueba in nueva.atributos)
            {
                MessageBox.Show("Los atributos son " + prueba+"holi");
            }
            foreach (String[] prueba in nueva.condOpe)
            {
                MessageBox.Show("Las condiciones son " + prueba[0]+prueba[1]+prueba[2]);
            }
            foreach (String[] prueba in nueva.condJoin)
            {
                MessageBox.Show("Las condiciones iner join son " + prueba[0] + prueba[1] + prueba[2]);
            }*/
            llenaConsultas(nueva);
        }

        public void llenaConsultas(Consulta con)
        {
            List<CEntidad> entidades = dicio.dameRegistrosEntidad();
            dataGridView1.Columns.Clear();
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
            if (!con.multiTablas)
            {
                foreach (String nombres in con.tablas)
                {
                    foreach (CEntidad entidad in entidades)
                    {
                        String cambiado = new String(entidad.asignombre);
                        cambiado = cambiado.Replace(" ", "");
                        if (con.tablas.Any(endad => endad == cambiado))
                        {
                            List<CAtributo> atributos = dicio.dameRegistrosAtributo(entidad.atrib);

                            foreach (var item in atributos)
                            {
                                String nomAt = new String(item.asignombre);
                                nomAt = nomAt.Replace(" ", "");
                                if (con.atributos.Any(endad => endad == nomAt) || con.atributos.Any(endad => endad == "*"))
                                {

                                    string columna;
                                    columna = new string(item.asignombre).Trim();
                                    dataGridView1.Columns.Add(columna, columna);
                                }

                            }


                            if (entidad.asigdireccion != -1)
                            {
                                List<Registro> datos = dicio.dameRegistrosDatos(entidad);
                                datos = datos.OrderBy(x => x.dimeDireccionRegstro).ToList();
                                foreach (var item in datos)
                                {
                                    string[] columna = new string[dataGridView1.Columns.Count];

                                    int i = 0;
                                    int n = 0;
                                    double numero1; 
                                    double numero2;
                                    foreach (var atributo in item.dimeAtributosRegistro)
                                    {
                                        String nomAt = new String(atributos[i].asignombre);
                                        nomAt = nomAt.Replace(" ", "");
                                        //MessageBox.Show("Los meros buenos son" + nomAt);
                                        if (con.atributos.Any(endad => endad == nomAt) || con.atributos.Any(endad => endad == "*"))
                                        {
                                            if (atributos[i].tipoat == 'E')
                                            {
                                                columna[n] = BitConverter.ToInt32(atributo, 0).ToString().Trim();
                                            }
                                            else
                                            {
                                                columna[n] = Encoding.ASCII.GetString(atributo).Trim();
                                            }
                                            n++;
                                        }
                                        i++;
                                    }
                                    if (!double.TryParse(con.condOpe[0], out numero1))
                                    {
                                        i = 0;
                                        foreach (var atributo in item.dimeAtributosRegistro)
                                        {
                                            String nomAt = new String(atributos[i].asignombre);
                                            nomAt = nomAt.Replace(" ", "");
                                            //MessageBox.Show("Los meros buenos son" + nomAt);
                                            if (con.condOpe[0] == nomAt)
                                            {
                                                if (atributos[i].tipoat == 'E')
                                                {
                                                    numero1 = BitConverter.ToInt32(atributo, 0);
                                                }
                                                else
                                                {
                                                    if (atributos[i].tipoat == 'F')
                                                    {
                                                        numero1 = BitConverter.ToDouble(atributo, 0);
                                                    }
                                                    else
                                                    {
                                                        numero1 = Convert.ToInt32(Encoding.ASCII.GetString(atributo).Trim());
                                                    }
                                                }

                                            }
                                            i++;
                                        }

                                    }
                                    if (!double.TryParse(con.condOpe[2], out numero2))
                                    {
                                        i = 0;
                                        foreach (var atributo in item.dimeAtributosRegistro)
                                        {
                                            String nomAt = new String(atributos[i].asignombre);
                                            nomAt = nomAt.Replace(" ", "");
                                            //MessageBox.Show("Los meros buenos son" + nomAt);
                                            if (con.condOpe[2] == nomAt)
                                            {
                                                if (atributos[i].tipoat == 'E')
                                                {
                                                    numero2 = BitConverter.ToInt32(atributo, 0);
                                                }
                                                else
                                                {
                                                    if (atributos[i].tipoat == 'F')
                                                    {
                                                        numero2 = BitConverter.ToDouble(atributo, 0);
                                                    }
                                                    else
                                                    {
                                                        numero2 = Convert.ToInt32(Encoding.ASCII.GetString(atributo).Trim());
                                                    }
                                                }

                                            }
                                            i++;
                                        }
                                    }
                                    switch (con.condOpe[1])
                                    {
                                        case "<":
                                            if (numero1 < numero2)
                                                dataGridView1.Rows.Add(columna);
                                            break;
                                        case ">":
                                            if (numero1 > numero2)
                                                dataGridView1.Rows.Add(columna);
                                            break;
                                        case "<>":
                                            if (numero1 != numero2)
                                                dataGridView1.Rows.Add(columna);
                                            break;
                                        case "=":
                                            if (numero1 == numero2)
                                                dataGridView1.Rows.Add(columna);
                                            break;
                                        case "<=":
                                            if (numero1 <= numero2)
                                                dataGridView1.Rows.Add(columna);
                                            break;
                                        case ">=":
                                            if (numero1 >= numero2)
                                                dataGridView1.Rows.Add(columna);
                                            break;
                                        default:
                                            dataGridView1.Rows.Add(columna);
                                            break;

                                    }

                                }

                            }


                        }
                    }

                }
            }
            else
            {
                List<String> fila = new List<String>();
                CEntidad primera = null;
                List<CAtributo> atributosPrimera = null;
                List<Registro> combinados = null;
                List<String[]> registrosNuevos = new List<String[]>();
                List<int> rechazados = new List<int>();

                int pos1 = 0, pos2 = 0;
                foreach (String nombres in con.tablas)
                {
                    foreach (CEntidad entidad in entidades)
                    {
                        String cambiado = new String(entidad.asignombre);
                        cambiado = cambiado.Replace(" ", "");

                        if (nombres == cambiado)
                        {
                            if (primera == null)
                            {
                                primera = entidad;
                                atributosPrimera = dicio.dameRegistrosAtributo(entidad.atrib);
                                combinados = dicio.dameRegistrosDatos(entidad);
                            }
                            List<CAtributo> atributos = dicio.dameRegistrosAtributo(entidad.atrib);

                            foreach (var item in atributos)
                            {
                                String nomAt = new String(item.asignombre);
                                nomAt = nomAt.Replace(" ", "");
                                String columna;
                                columna = new String(item.asignombre).Trim();
                                columna = cambiado + "." + columna;
                                dataGridView1.Columns.Add(columna, columna);

                            }

                        }
                    }

                }
                for (int j = 1; j < con.tablas.Count; j++)
                {
                    List<CAtributo> atributos = null;
                    foreach (CEntidad entidad in entidades)
                    {
                        String cambiado = new String(entidad.asignombre);
                        cambiado = cambiado.Replace(" ", "");
                        if (con.tablas[j] == cambiado)
                        {

                            atributos = dicio.dameRegistrosAtributo(entidad.atrib);
                            if (entidad.asigdireccion != -1)
                            {
                                List<Registro> datos = dicio.dameRegistrosDatos(entidad);
                                datos = datos.OrderBy(x => x.dimeDireccionRegstro).ToList();
                                foreach (var pri in combinados)
                                {

                                    foreach (var item in datos)
                                    {
                                        String[] columna = new String[atributos.Count + atributosPrimera.Count];

                                        int i = 0;
                                        int n = 0;
                                        foreach (var atributo in pri.dimeAtributosRegistro)
                                        {
                                            String nomAt = new String(atributosPrimera[i].asignombre);
                                            nomAt = nomAt.Replace(" ", "");

                                            if (atributosPrimera[i].tipoat == 'E')
                                            {
                                                columna[n] = BitConverter.ToInt32(atributo, 0).ToString().Trim();
                                            }
                                            else
                                            {

                                                if (atributosPrimera[i].tipoat == 'F')
                                                {
                                                    columna[n] = BitConverter.ToDouble(atributo, 0).ToString().Trim();
                                                }
                                                else
                                                {
                                                    columna[n] = Encoding.ASCII.GetString(atributo).Trim();
                                                }
                                                
                                            }

                                            n++;
                                            i++;
                                        }
                                        i = 0;
                                        foreach (var atributo in item.dimeAtributosRegistro)
                                        {
                                            String nomAt = new String(atributos[i].asignombre);
                                            nomAt = nomAt.Replace(" ", "");
                                            //MessageBox.Show("Los meros buenos son" + nomAt);
                                            if (atributos[i].tipoat == 'E')
                                            {
                                                columna[n] = BitConverter.ToInt32(atributo, 0).ToString().Trim();
                                            }
                                            else
                                            {
                                                if (atributos[i].tipoat == 'F')
                                                {
                                                    columna[n] = BitConverter.ToDouble(atributo, 0).ToString().Trim();
                                                }
                                                else
                                                {
                                                    columna[n] = Encoding.ASCII.GetString(atributo).Trim();
                                                }
                                            }
                                            n++;
                                            i++;
                                        }

                                        registrosNuevos.Add(columna);

                                    }
                                }

                            }


                        }
                    }
                    if (atributos != null)
                    {
                        atributosPrimera.AddRange(atributos);

                    }

                }

                foreach (String[] condicion in con.condJoin)
                {
                    int renB = 0;
                    double numero1, numero2;
                    foreach (String[] regi in registrosNuevos)
                    {
                        if (!double.TryParse(condicion[0], out numero1))
                        {
                            int ap = 0;
                            for (int r = 0; r < dataGridView1.Columns.Count; r++)
                            {
                                String nomAt = dataGridView1.Columns[r].Name;
                                //MessageBox.Show("Los meros buenos son" + nomAt);
                                if (condicion[0] == nomAt)
                                {
                                    if (atributosPrimera[r].tipoat == 'E')
                                    {
                                        numero1 = int.Parse(regi[ap]);
                                    }
                                    else
                                    {
                                        if (atributosPrimera[r].tipoat == 'F')
                                        {
                                            numero1 = double.Parse(regi[ap]);
                                        }
                                        else
                                        {
                                            numero2 = Convert.ToInt32(regi[ap]);
                                        }
                                    }

                                }
                                ap++;
                            }
                        }
                        if (!double.TryParse(condicion[2], out numero2))
                        {
                            int ap = 0;

                            for (int r = 0; r < dataGridView1.Columns.Count; r++)
                            {
                                String nomAt = dataGridView1.Columns[r].Name;
                                //MessageBox.Show("Los meros buenos son" + nomAt);
                                if (condicion[2] == nomAt)
                                {
                                    if (atributosPrimera[r].tipoat == 'E')
                                    {
                                        numero2 = int.Parse(regi[ap]);
                                    }
                                    else
                                    {
                                        if (atributosPrimera[r].tipoat == 'F')
                                        {
                                            numero2 = double.Parse(regi[ap]);
                                        }
                                        else
                                        {
                                            numero2 = Convert.ToInt32(regi[ap]);
                                        }
                                    }

                                }
                                ap++;
                            }
                        }

                        switch (condicion[1])
                        {
                            case "<":
                                if (!(numero1 < numero2))
                                    rechazados.Add(renB);

                                break;
                            case ">":
                                if (!(numero1 > numero2))
                                    rechazados.Add(renB);
                                break;
                            case "<>":
                                if (!(numero1 != numero2))
                                    rechazados.Add(renB);
                                break;
                            case "=":
                                if (!(numero1 == numero2))
                                    rechazados.Add(renB);
                                break;
                            case "<=":
                                if (!(numero1 <= numero2))
                                    rechazados.Add(renB);
                                break;
                            case ">=":
                                if (!(numero1 >= numero2))
                                    rechazados.Add(renB);
                                break;


                        }
                        renB++;
                    }
                }
                int numerito = 0;

                foreach (String[] regi in registrosNuevos)
                {
                    if (!(rechazados.Any(endad => endad == numerito)))
                    {
                        dataGridView1.Rows.Add(regi);
                    }
                    else
                    {
                        //MessageBox.Show(regi[1]);
                    }
                    numerito++;
                }
                for (int final = 0; final < dataGridView1.Columns.Count; final++)
                {
                    String[] cad = dataGridView1.Columns[final].Name.Split('.');
                    if (!(con.atributos.Any(endad => endad == cad[1]) || con.atributos.Any(endad => endad == "*")))
                    {
                        dataGridView1.Columns[final].Visible = false;
                    }
                }
            }
        }
    }
}

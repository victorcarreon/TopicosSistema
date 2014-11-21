using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SistemaTopicos
{
    public partial class Form1 : Form
    {
        SistemaTopicosEntities db;
        List<TiposTablilla> tipos;
        List<String> codigosValidos;
       
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            db = new SistemaTopicosEntities();
            //Obtener los tipos de tablilla validos
            tipos = (from t in db.TiposTablilla
                         select t).ToList();

            //Los codigos validos para la validacion.
            codigosValidos = (from t in tipos
                                 select t.Id).ToList();
            
            serialPort1 = new SerialPort("COM1", 9600, Parity.None, 8, StopBits.One);
            serialPort1.ReadTimeout = 1000;
            serialPort1.WriteTimeout = 1000;
            serialPort1.Encoding = Encoding.ASCII;
            serialPort1.DataReceived += new SerialDataReceivedEventHandler(handler);
            serialPort1.Open();

            //Inicializa lista
            listView1.View = View.Details;
            listView1.GridLines = true;
            listView1.FullRowSelect = true;

            listView1.Columns.Add("Codigo", 70);
            listView1.Columns.Add("Nombre", 100);
            listView1.Columns.Add("Descripcion", 200);
        }

        private void handler(object sender, SerialDataReceivedEventArgs e)
        {

            string lectura = serialPort1.ReadLine();
            lectura = lectura.Remove(lectura.Length - 1);
            //Validar que la lectura del codigo es de una pieza esperada
            if (codigosValidos.Contains(lectura))
            {
                var tempItem = (from d in tipos
                                 where d.Id == lectura
                                 select d).First();

                string[] itemData = { tempItem.Id, tempItem.Nombre, tempItem.Descripcion };

                ListViewItem item = new ListViewItem(itemData);
                listView1.Items.Add(item);


                //Guardar en inventario
                Piezas nuevaPieza = new Piezas();
                nuevaPieza.CodigoProveedor = tempItem.Id;
                nuevaPieza.CodigoProceso = tempItem.Id + "I";
                nuevaPieza.Descripcion = tempItem.Descripcion + " En Inventario";
                nuevaPieza.timestamp = DateTime.Now;
                nuevaPieza.EstadoFk = 1;
                nuevaPieza.TipoFk = tempItem.Id;
                nuevaPieza.Nombre = tempItem.Nombre + " En inventario";

                db.Piezas.Add(nuevaPieza);
                db.SaveChanges();

            }
            else
            {
                lblError.Text = "Elemento desconocido!, Escanear objeto valido.";
               
            }
               
                
                
        }
    }

}

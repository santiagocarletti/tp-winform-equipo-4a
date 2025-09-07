using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using dominio;
using negocio;

namespace TpWindowsForms
{
    public partial class FormAgregar : Form
    {
        public FormAgregar()
        {
            InitializeComponent();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            Articulo Arti = new Articulo();
            ArticuloNegocio negocio = new ArticuloNegocio();
            try
            {
                Arti.Codigo = txtNumero.Text;
                Arti.Nombre = txtNombre.Text;
                Arti.Descripcion = txtDescripcion.Text;

                negocio.agregar(Arti);
                MessageBox.Show("Agregado correctamente");
                Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }
    }
}

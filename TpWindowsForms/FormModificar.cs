using dominio;
using negocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TpWindowsForms
{
    public partial class FormModificar : Form
    {
        private Articulo articulo = null;

        public FormModificar(Articulo articulo)
        {
            InitializeComponent();
            this.articulo = articulo;
            CargarControles();
            CargarDatos(articulo);
        }

        private void CargarControles()
        {
            MarcaNegocio mNegocio = new MarcaNegocio();
            CategoriaNegocio cNegocio = new CategoriaNegocio();

            cboMarca.DataSource = mNegocio.listar();
            cboMarca.ValueMember = "Id";
            cboMarca.DisplayMember = "Descripcion";

            cboCategoria.DataSource = cNegocio.listar();
            cboCategoria.ValueMember = "Id";
            cboCategoria.DisplayMember = "Descripcion";
        }
        private void CargarDatos(Articulo articulo)
        {
            txtCodigo.Text = articulo.Codigo.ToString();
            txtNombre.Text = articulo.Nombre.ToString();
            txtDescripcion.Text = articulo.Descripcion.ToString();
            if (articulo.IdCategoria != null)
                cboCategoria.SelectedValue = articulo.IdCategoria.Id;

            if (articulo.marca != null)
                cboMarca.SelectedValue = articulo.marca.Id;

            txtPrecio.Text = articulo.Precio.ToString();
        }
    }
}

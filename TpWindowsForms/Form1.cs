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
    public partial class Form1 : Form
    {
        private List<Articulo> listaArticulos;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cargar();
        }
        private void cargar()
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            try
            {
                listaArticulos = negocio.listar();
                dgvArticulos.DataSource = listaArticulos;
                dgvArticulos.Columns["Id"].Visible = false;
                //dgvArticulos.Columns["Imagen"].Visible = false;
                //pbxArticulos.Load(listaArticulos[0].Imagen.ImagenUrl);
                //jueves
                pbxArticulos.Load(listaArticulos[0].Imagen[0]);

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }
        private void dgvArticulos_SelectionChanged(object sender, EventArgs e)
        {
           Articulo seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
            //cargarImagen(seleccionado.Imagen.ImagenUrl);
            //jueves
            cargarImagen(seleccionado.Imagen[0]);
            //viernes
            CargarListaImagenes();
            if (cboImagenes.Items.Count > 0)
            {
                cboImagenes.SelectedIndex = 0;
                cargarImagen((string)cboImagenes.Items[0]);
            }
        }
        private void CargarListaImagenes()
        {
            if (dgvArticulos.CurrentRow == null)
            { return; }

            Articulo seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;

            cboImagenes.Items.Clear();

            foreach (string Image in seleccionado.Imagen)
            {
                cboImagenes.Items.Add(Image);
            }
        }
        private void cargarImagen(string imagen)
        {
            try
            {
                pbxArticulos.Load(imagen);
            }
            catch (Exception ex)
            {
                pbxArticulos.Load("https://png.pngtree.com/png-vector/20230407/ourmid/pngtree-placeholder-line-icon-vector-png-image_6691835.png");
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            FormAgregar alta = new FormAgregar();
            alta.ShowDialog();
            cargar();
        }

        private void dgvArticulos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (dgvArticulos.CurrentRow != null)
            {
                Articulo seleccionado;
                seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
                FormModificar modificar = new FormModificar(seleccionado);
                modificar.ShowDialog();
            }
        }

        private void cboImagenes_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargarImagen((string)cboImagenes.SelectedItem);
        }

        private void btnBorrar_Click(object sender, EventArgs e)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            Articulo seleccionado;
            try
            {
                DialogResult respuesta = MessageBox.Show("¿Estas seguro que querer eliminarlo?", "Eliminando", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if(respuesta == DialogResult.Yes)
                {
                    seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
                    negocio.eliminar(seleccionado.Id);
                    cargar();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }
    }
}

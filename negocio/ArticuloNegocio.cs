using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dominio;

namespace negocio
{
    public class ArticuloNegocio
    {
        public List<Articulo> listar()
        {
            List<Articulo> lista = new List<Articulo>();
            SqlConnection conexion = new SqlConnection();
            SqlCommand comando = new SqlCommand();
            SqlDataReader lector;

            try
            {
                conexion.ConnectionString = "server=.\\SQLEXPRESS; database=CATALOGO_P3_DB; integrated security=true";
                comando.CommandType = System.Data.CommandType.Text;
                //comando.CommandText = "SELECT A.Id, Codigo, Nombre, A.Descripcion, M.Descripcion AS Marca, IdCategoria, Precio, I.ImagenUrl FROM Articulos A, IMAGENES I, MARCAS AS M WHERE A.Id = I.Id AND M.Id = A.IdMarca";
                comando.CommandText = "SELECT A.Id, Codigo, Nombre, A.Descripcion, M.Descripcion AS Marca, M.Id AS IdMarca, IdCategoria, Precio, I.ImagenUrl FROM Articulos A, IMAGENES I, MARCAS AS M WHERE A.Id = I.Id AND M.Id = A.IdMarca";

                comando.Connection = conexion;

                conexion.Open();
                lector = comando.ExecuteReader();

                while (lector.Read())
                {
                    Articulo aux = new Articulo();
                    aux.Id = (int)lector["Id"];
                    aux.Codigo = (string)lector["Codigo"];
                    aux.Nombre = (string)lector["Nombre"];
                    aux.Descripcion = (string)lector["Descripcion"];
                    aux.marca = new Marca();
                    aux.marca.Descripcion = (string)lector["Marca"];
                    //
                    aux.marca.Id = (int)lector["IdMarca"];
                    //
                    aux.IdCategoria = (int)lector["IdCategoria"];
                    aux.Precio = (decimal)lector["Precio"];
                    aux.Imagen = new Imagen();
                    aux.Imagen.ImagenUrl = (string)lector["ImagenUrl"];

                    lista.Add(aux);

                }

                conexion.Close();
                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void agregar(Articulo nuevo)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("Insert into ARTICULOS (Codigo, Nombre, Descripcion)values(" + nuevo.Codigo + ", '" + nuevo.Nombre + "', '" + nuevo.Descripcion +"')");
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
    }

}

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
                //comando.CommandText = "SELECT A.Id, Codigo, Nombre, A.Descripcion, M.Descripcion AS Marca, M.Id AS IdMarca, IdCategoria, Precio, I.ImagenUrl FROM Articulos A, IMAGENES I, MARCAS AS M WHERE A.Id = I.Id AND M.Id = A.IdMarca";
                comando.CommandText = "SELECT A.Id IdArticulo, A.Codigo codigoArticulo, A.Nombre nombreArticulo, A.Descripcion articuloDescripcion, M.Descripcion marcaDescripcion, CA.Descripcion categoriaDescripcion, Precio precioArticulo, I.ImagenUrl ImagenUrl FROM ARTICULOS A, IMAGENES I, MARCAS M, CATEGORIAS CA WHERE M.Id = A.IdMarca AND CA.Id = A.IdCategoria AND I.Id = A.Id";
                comando.Connection = conexion;

                conexion.Open();
                lector = comando.ExecuteReader();

                while (lector.Read())
                {
                    Articulo aux = new Articulo();
                    aux.Id = (int)lector["IdArticulo"];
                    aux.Codigo = (string)lector["codigoArticulo"];
                    aux.Nombre = (string)lector["nombreArticulo"];
                    aux.Descripcion = (string)lector["articuloDescripcion"];
                    aux.marca = new Marca();
                    aux.marca.Descripcion = (string)lector["marcaDescripcion"];
                    //
                    //aux.marca.Id = (int)lector["IdMarca"];
                    //
                    aux.IdCategoria = new Categoria();
                    //aux.IdCategoria.Id = (int)lector["IdCategoria"];
                    aux.IdCategoria.Descripcion = (string)lector["categoriaDescripcion"];
                    aux.Precio = (decimal)lector["precioArticulo"];
                    aux.Imagen = new Imagen();
                    if (!(lector["ImagenUrl"] is DBNull))
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
                datos.setearConsulta("Insert into ARTICULOS (Codigo, Nombre, Descripcion, IdMarca, IdCategoria, Precio)values(" + nuevo.Codigo + ", '" + nuevo.Nombre + "', '" + nuevo.Descripcion + "' , @IdMarca, @IdCategoria, @Precio)");
                datos.setearParametro("@IdMarca", nuevo.marca.Id);
                datos.setearParametro("@IdCategoria", nuevo.IdCategoria.Id);
                datos.setearParametro("@Precio", nuevo.Precio);
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

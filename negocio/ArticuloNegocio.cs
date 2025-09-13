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
                //comando.CommandText = "SELECT A.Id, Codigo, Nombre, A.Descripcion, M.Descripcion AS Marca, M.Id AS IdMarca, IdCategoria, Precio, I.ImagenUrl FROM Articulos A, IMAGENES I, MARCAS AS M WHERE A.Id = I.Id AND M.Id = A.IdMarca";
                //comando.CommandText = "SELECT A.Id IdArticulo, A.Codigo codigoArticulo, A.Nombre nombreArticulo, A.Descripcion articuloDescripcion, M.Descripcion marcaDescripcion, CA.Descripcion categoriaDescripcion, Precio precioArticulo, I.ImagenUrl ImagenUrl FROM ARTICULOS A, IMAGENES I, MARCAS M, CATEGORIAS CA WHERE M.Id = A.IdMarca AND CA.Id = A.IdCategoria AND I.Id = A.Id";
                //viernes
                //comando.CommandText = "SELECT A.Id AS IdArticulo, A.Codigo AS codigoArticulo, A.Nombre  AS nombreArticulo, A.Descripcion AS articuloDescripcion, M.Descripcion AS marcaDescripcion, CA.Descripcion AS categoriaDescripcion, A.Precio AS precioArticulo, I.ImagenUrl AS ImagenUrl, I.Id AS IdImagen FROM ARTICULOS A LEFT JOIN IMAGENES I ON I.IdArticulo = A.Id LEFT JOIN MARCAS M ON M.Id = A.IdMarca LEFT JOIN CATEGORIAS CA ON CA.Id = A.IdCategoria ORDER BY A.Id, I.Id;";
                //sabado
                comando.CommandText = "SELECT A.Id AS IdArticulo, A.Codigo AS codigoArticulo, A.Nombre AS nombreArticulo,\r\n       A.Descripcion AS articuloDescripcion, \r\n       M.Id AS IdMarca, M.Descripcion AS marcaDescripcion, \r\n       CA.Id AS IdCategoria, CA.Descripcion AS categoriaDescripcion, \r\n       A.Precio AS precioArticulo, I.ImagenUrl AS ImagenUrl, I.Id AS IdImagen\r\nFROM ARTICULOS A\r\nLEFT JOIN IMAGENES I ON I.IdArticulo = A.Id\r\nLEFT JOIN MARCAS M ON M.Id = A.IdMarca\r\nLEFT JOIN CATEGORIAS CA ON CA.Id = A.IdCategoria\r\nORDER BY A.Id, I.Id;\r\n";
                comando.Connection = conexion;

                conexion.Open();
                lector = comando.ExecuteReader();
                //jueves
                Articulo ultCarga = null;

                while (lector.Read())
                {
                    //int idArtBD = Convert.ToInt32(lector["Id"]);
                    int idArtBD = lector["IdArticulo"] != DBNull.Value ? Convert.ToInt32(lector["IdArticulo"]) : 0;


                    //jueves. Si es el mismo Articulo, solo agrego imagen para no cargar duplicado
                    if (ultCarga != null && ultCarga.Id == idArtBD)
                    {
                        string imagenUrl = Convert.ToString(lector["ImagenUrl"]);
                        ultCarga.Imagen.Add(imagenUrl);
                        continue;
                    }
                    //

                    Articulo aux = new Articulo();
                    aux.Id = (int)lector["IdArticulo"];
                    aux.Codigo = (string)lector["codigoArticulo"];
                    aux.Nombre = (string)lector["nombreArticulo"];
                    aux.Descripcion = (string)lector["articuloDescripcion"];
                    aux.marca = new Marca();
                    //sabado
                    aux.marca.Id = (int)lector["IdMarca"];
                    //
                    aux.marca.Descripcion = (string)lector["marcaDescripcion"];
                    aux.IdCategoria = new Categoria();
                    //sabado
                    aux.IdCategoria.Id = (int)lector["IdCategoria"];
                    //
                    aux.IdCategoria.Descripcion = (string)lector["categoriaDescripcion"];
                    aux.Precio = (decimal)lector["precioArticulo"];
                    aux.Imagen = new List<string>();
                    aux.Imagen.Add(Convert.ToString(lector["ImagenUrl"]));

                    lista.Add(aux);
                    ultCarga = aux;
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
                datos.abrirConexion();
                datos.setearConsulta(
                    "INSERT INTO ARTICULOS (Codigo, Nombre, Descripcion, Precio, IdMarca, IdCategoria) " +
                    "VALUES (@codigo, @nombre, @descripcion, @precio, @idMarca, @idCategoria); " +
                    "SELECT SCOPE_IDENTITY();"
                );
                datos.setearParametro("@codigo", nuevo.Codigo);
                datos.setearParametro("@nombre", nuevo.Nombre);
                datos.setearParametro("@descripcion", nuevo.Descripcion);
                datos.setearParametro("@precio", nuevo.Precio);
                datos.setearParametro("@idMarca", nuevo.marca.Id);
                datos.setearParametro("@idCategoria", nuevo.IdCategoria.Id);

                int idArticulo = datos.ejecutarAccionconreturn();
                nuevo.Id = idArticulo;

                datos.limpiarParametros();

                
                if (nuevo.Imagen != null && nuevo.Imagen.Count > 0)
                {
                    foreach (string url in nuevo.Imagen)
                    {
                        if (!string.IsNullOrEmpty(url))
                        {
                            datos.setearConsulta(
                                "INSERT INTO IMAGENES (IdArticulo, ImagenUrl) VALUES (@IdArticulo, @ImagenUrl)"
                            );
                            datos.setearParametro("@IdArticulo", idArticulo);
                            datos.setearParametro("@ImagenUrl", url);
                            datos.ejecutarAccion();
                            datos.limpiarParametros();
                        }
                    }
                }
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

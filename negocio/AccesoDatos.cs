﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace negocio
{
    internal class AccesoDatos
    {
        private SqlConnection conexion;
        private SqlCommand comando;
        private SqlDataReader lector;

        public SqlDataReader Lector
        {
            get { return lector; }
        }
        public AccesoDatos()
        {
            conexion = new SqlConnection("server=.\\SQLEXPRESS; database=CATALOGO_P3_DB; integrated security=true");
            comando = new SqlCommand();
        }
        public void setearConsulta(string consulta)
        {
            comando.CommandType = System.Data.CommandType.Text;
            comando.CommandText = consulta;
        }
        public void ejecutarLectura()
        {
            comando.Connection = conexion;
            try
            {
                conexion.Open();
                lector = comando.ExecuteReader();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ejecutarAccion()
        {
            comando.Connection = conexion;
            conexion.Open();
            comando.ExecuteNonQuery();
            //conexion.Close();
            //try
            //{
            //
            //}
            //catch (Exception)
            //{

            //    throw;
            //}
        }

        public void setearParametro(string nombre, object valor)
        {
            comando.Parameters.AddWithValue(nombre, valor);
        }

        public void cerrarConexion()
        {
            if (lector != null)
                lector.Close();
            conexion.Close();
        }
        //jueves
        public int ejecutarAccionconreturn()
        {
            comando.Connection = conexion;
            conexion.Open();
            return int.Parse(comando.ExecuteScalar().ToString());
            conexion.Close();
            //try
            //{
            //    conexion.Open();
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
        }
        public void limpiarParametros()
        {
            comando.Parameters.Clear();
        }
        public void abrirConexion()
        {
            if (conexion.State != System.Data.ConnectionState.Open)
                conexion.Open();
        }
        //
        public void ejecutarMasAcciones()
        {
            try
            {
                comando.Connection = conexion;
                abrirConexion();
                comando.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

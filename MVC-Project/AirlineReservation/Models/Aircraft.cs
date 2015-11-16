using Npgsql;

using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace AirlineReservation.Models
{
    public class Aircraft
    {
        #region [Propiedades]
        public long ID { get; set; }

        [Required(ErrorMessage = "El campo nombre es requerido")]
        [DisplayName("Nombre")]
        [StringLength(55)]
        public string Name { get; set; }

        [Required(ErrorMessage = "El campo capacidad es requerido")]
        [DisplayName("Capacidad")]
        public int Capacity { get; set; }

        [Required(ErrorMessage = "El campo código es requerido")]
        [DisplayName("Código")]
        [StringLength(3)]
        public string Code { get; set; }
        #endregion

        #region [Metodos]
        /// <summary>
        /// Establece las propiedades del Aircraft dado un NpgsqlDataReader
        /// (miturriaga)
        /// </summary>
        /// <param name="dr">El NpgsqlDataReader que contiene los datos del aircraft</param>
        public void SetDesde(NpgsqlDataReader dr)
        {
            this.ID = Convert.ToInt64(dr["aircraftID"]);
            this.Name = dr["aircraftName"].ToString();
            this.Capacity = Convert.ToInt32(dr["aircraftCapacity"]);
            this.Code = dr["aircraftCode"].ToString();
        }

        /// <summary>
        /// Selecciona los aircraft dada la pagina y cantidad de resultados por pagina
        /// (miturriaga)
        /// </summary>
        /// <param name="cantidadResultados">Cantidad de resultado por pagina</param>
        /// <param name="pagina">Pagina que se necesita ver</param>
        /// <returns>La lista de aircraft</returns>
        public List<Aircraft> Todos(int cantidadResultados, int pagina)
        {

            var aircrafts = new List<Aircraft>();
            int index = cantidadResultados * (pagina - 1);

            try
            {
                string connString = ConfigurationManager.ConnectionStrings["PostgresConnection"].ConnectionString;

                var comando = new NpgsqlCommand() { CommandText = "Aircraft_Todos", CommandType = CommandType.StoredProcedure };
                comando.Parameters.Add(new NpgsqlParameter("inIndex", NpgsqlDbType.Integer));
                comando.Parameters[0].Value = index;
                comando.Parameters.Add(new NpgsqlParameter("inNext", NpgsqlDbType.Integer));
                comando.Parameters[1].Value = cantidadResultados;

                using (var conn = new NpgsqlConnection(connString))
                {
                    conn.Open();
                    comando.Connection = conn;
                    NpgsqlDataReader ds = comando.ExecuteReader();

                    while (ds.Read())
                    {
                        var aircraft = new Aircraft();
                        aircraft.SetDesde(ds);
                        aircrafts.Add(aircraft);
                    }
                    conn.Close();
                }
            }
            catch (Exception ex) { }

            return aircrafts;
        }

        /// <summary>
        /// Selecciona un aircraft dado el id que se entrega como parametro 
        /// (miturriaga)
        /// </summary>
        /// <param name="id">el id del aircraft a seleccionar</param>
        /// <returns>True si el aircraft fue encontrado, false en caso contrario</returns>
        public bool Seleccionar(long id)
        {
            try
            {
                string connString = ConfigurationManager.ConnectionStrings["PostgresConnection"].ConnectionString;

                var comando = new NpgsqlCommand() { CommandText = "Aircraft_Seleccionar", CommandType = CommandType.StoredProcedure };
                comando.Parameters.Add(new NpgsqlParameter("inID", NpgsqlDbType.Integer));
                comando.Parameters[0].Value = id;

                using (var conn = new NpgsqlConnection(connString))
                {
                    conn.Open();
                    comando.Connection = conn;
                    NpgsqlDataReader ds = comando.ExecuteReader();

                    if (ds.Read())
                    {
                        this.SetDesde(ds);
                        conn.Close();
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception ex) { }

            return false;
        }

        /// <summary>
        /// Crea este nuevo aircrat en la base de datos
        /// (miturriaga)
        /// </summary>
        public void Crear()
        {
            try
            {
                string connString = ConfigurationManager.ConnectionStrings["PostgresConnection"].ConnectionString;

                var comando = new NpgsqlCommand() { CommandText = "Aircraft_Crear", CommandType = CommandType.StoredProcedure };
                comando.Parameters.Add(new NpgsqlParameter("outID", NpgsqlDbType.Integer));
                comando.Parameters[0].Direction = ParameterDirection.Output;
                comando.Parameters[0].Value = 0;
                comando.Parameters.Add(new NpgsqlParameter("inName", NpgsqlDbType.Text));
                comando.Parameters[1].Value = this.Name;
                comando.Parameters.Add(new NpgsqlParameter("inCapacity", NpgsqlDbType.Integer));
                comando.Parameters[2].Value = this.Capacity;
                comando.Parameters.Add(new NpgsqlParameter("inCode", NpgsqlDbType.Text));
                comando.Parameters[3].Value = this.Code;

                using (var conn = new NpgsqlConnection(connString))
                {
                    conn.Open();
                    comando.Connection = conn;
                    NpgsqlDataReader ds = comando.ExecuteReader();

                    this.ID = Convert.ToInt64(comando.Parameters[0].Value);

                    conn.Close();
                }
            }
            catch (Exception ex) { }
        }

        /// <summary>
        /// Modifica este aircraft en la base de datos
        /// (miturriaga)
        /// </summary>
        public void Modificar()
        {
            try
            {
                string connString = ConfigurationManager.ConnectionStrings["PostgresConnection"].ConnectionString;

                var comando = new NpgsqlCommand() { CommandText = "Aircraft_Modificar", CommandType = CommandType.StoredProcedure };
                comando.Parameters.Add(new NpgsqlParameter("inID", NpgsqlDbType.Integer));
                comando.Parameters[0].Value = this.ID;
                comando.Parameters.Add(new NpgsqlParameter("inName", NpgsqlDbType.Text));
                comando.Parameters[1].Value = this.Name;
                comando.Parameters.Add(new NpgsqlParameter("inCapacity", NpgsqlDbType.Integer));
                comando.Parameters[2].Value = this.Capacity;
                comando.Parameters.Add(new NpgsqlParameter("inCode", NpgsqlDbType.Text));
                comando.Parameters[3].Value = this.Code;

                using (var conn = new NpgsqlConnection(connString))
                {
                    conn.Open();
                    comando.Connection = conn;
                    NpgsqlDataReader ds = comando.ExecuteReader();
                    conn.Close();
                }
            }
            catch (Exception ex) { }
        }

        /// <summary>
        /// Elimina este aircraft de la base de datos
        /// (miturriaga)
        /// </summary>
        public void Eliminar()
        {
            try
            {
                string connString = ConfigurationManager.ConnectionStrings["PostgresConnection"].ConnectionString;

                var comando = new NpgsqlCommand() { CommandText = "Aircraft_Eliminar", CommandType = CommandType.StoredProcedure };
                comando.Parameters.Add(new NpgsqlParameter("inID", NpgsqlDbType.Integer));
                comando.Parameters[0].Value = this.ID;

                using (var conn = new NpgsqlConnection(connString))
                {
                    conn.Open();
                    comando.Connection = conn;
                    NpgsqlDataReader ds = comando.ExecuteReader();
                    conn.Close();
                }
            }
            catch (Exception ex) { }
        }

        /// <summary>
        /// Retorna un string que representa brevemente esta entidad
        /// (miturriaga)
        /// </summary>
        /// <returns>Un string con la representacion del aircraft</returns>
        public override string ToString()
        {
            return string.Format("Aircraft (ID: {0}, Name: {1})", this.ID, this.Name);
        }

        #endregion
    }
}
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace AirlineReservation.Models
{
    public class State
    {
        #region [Propiedades]

        public long ID { get; set; }

        public string Name { get; set; }

        #endregion

        #region [Metodos]
        /// <summary>
        /// Establece las propiedades del state dado un NpgsqlDataReader
        /// (miturriaga)
        /// </summary>
        /// <param name="dr">El NpgsqlDataReader que contiene los datos del state</param>
        public void SetDesde(NpgsqlDataReader dr)
        {
            this.ID = Convert.ToInt64(dr["flightStateID"]);
            this.Name = dr["stateName"].ToString();
        }

        /// <summary>
        /// Selecciona un state dado el id que se entrega como parametro 
        /// (miturriaga)
        /// </summary>
        /// <param name="id">el id del state a seleccionar</param>
        /// <returns>True si el state fue encontrado, false en caso contrario</returns>
        public bool Seleccionar(long id)
        {
            try
            {
                string connString = ConfigurationManager.ConnectionStrings["PostgresConnection"].ConnectionString;

                var comando = new NpgsqlCommand() { CommandText = "FlightState_Seleccionar", CommandType = CommandType.StoredProcedure };
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
        #endregion
    }
}
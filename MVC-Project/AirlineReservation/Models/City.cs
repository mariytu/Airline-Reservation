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
    public class City
    {
        #region [Propiedades]

        public long ID { get; set; }

        public string Name { get; set; }

        #endregion

        #region [Metodos]
        /// <summary>
        /// Establece las propiedades de la ciudad dado un NpgsqlDataReader
        /// (miturriaga)
        /// </summary>
        /// <param name="dr">El NpgsqlDataReader que contiene los datos de la ciudad</param>
        public void SetDesde(NpgsqlDataReader dr)
        {
            this.ID = Convert.ToInt64(dr["cityID"]);
            this.Name = dr["cityName"].ToString();
        }

        /// <summary>
        /// Selecciona una ciudad dado el id que se entrega como parametro 
        /// (miturriaga)
        /// </summary>
        /// <param name="id">el id de la ciudad a seleccionar</param>
        /// <returns>True si la ciudad fue encontrado, false en caso contrario</returns>
        public bool Seleccionar(long id)
        {
            try
            {
                string connString = ConfigurationManager.ConnectionStrings["PostgresConnection"].ConnectionString;

                var comando = new NpgsqlCommand() { CommandText = "City_Seleccionar", CommandType = CommandType.StoredProcedure };
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
        /// Selecciona todas las ciudades
        /// (miturriaga)
        /// </summary>
        /// <returns>La lista de ciudades</returns>
        public static List<City> Todos()
        {

            var cities = new List<City>();

            try
            {
                string connString = ConfigurationManager.ConnectionStrings["PostgresConnection"].ConnectionString;

                var comando = new NpgsqlCommand() { CommandText = "City_Todos", CommandType = CommandType.StoredProcedure };
                
                using (var conn = new NpgsqlConnection(connString))
                {
                    conn.Open();
                    comando.Connection = conn;
                    NpgsqlDataReader ds = comando.ExecuteReader();

                    while (ds.Read())
                    {
                        var city = new City();
                        city.SetDesde(ds);
                        cities.Add(city);
                    }
                    conn.Close();
                }
            }
            catch (Exception ex) { }

            return cities;
        }
        #endregion
    }
}
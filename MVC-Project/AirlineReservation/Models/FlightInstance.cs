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
    public class FlightInstance
    {
        #region [Propiedades]
        public long ID { get; set; }

        [Required(ErrorMessage = "El campo estado es requerido")]
        [DisplayName("Estado")]
        public long StateID { get; set; }

        public string State
        {
            get
            {
                if (this.StateID == 1)
                {
                    return "In Flight";
                }
                else if (this.StateID == 2)
                {
                    return "Canceled";
                }
                else if (this.StateID == 3)
                {
                    return "Scheduled";
                }
                else
                {
                    return "Finished";
                }
            }
        }

        [Required(ErrorMessage = "El campo costo es requerido")]
        [DisplayName("Costo")]
        public int Cost { get; set; }

        [Required(ErrorMessage = "El campo vuelo es requerido")]
        [DisplayName("Vuelo")]
        public long FlightNumberID { get; set; }

        [DisplayName("Fecha de Salida Real")]
        public DateTime RealDeparture { get; set; }

        public string RealDepartureString
        {
            get
            {
                if (DateTime.MinValue == RealDeparture)
                {
                    return "-";
                }
                return RealDeparture.ToString("dd-MM-yyyy HH:mm");
            }
        }

        [DisplayName("Fecha de Llegada Real")]
        public DateTime RealArrival { get; set; }

        public string RealArrivalString
        {
            get
            {
                if (DateTime.MinValue == RealArrival)
                {
                    return "-";
                }
                return RealArrival.ToString("dd-MM-yyyy HH:mm");
            }
        }

        [Required(ErrorMessage = "El campo fecha de salida estimada es requerido")]
        [DisplayName("Fecha de Salida Estimada")]
        public DateTime EstimatedDeparture { get; set; }

        public string EstimatedDepartureString
        {
            get
            {
                if (DateTime.MinValue == EstimatedDeparture)
                {
                    return "-";
                }
                return EstimatedDeparture.ToString("dd-MM-yyyy HH:mm");
            }
        }

        [Required(ErrorMessage = "El campo fecha de llegada estimada es requerido")]
        [DisplayName("Fecha de Llegada Estimada")]
        public DateTime EstimatedArrival { get; set; }

        public string EstimatedArrivalString
        {
            get
            {
                if (DateTime.MinValue == EstimatedArrival)
                {
                    return "-";
                }
                return EstimatedArrival.ToString("dd-MM-yyyy HH:mm");
            }
        }

        [Required(ErrorMessage = "El campo avión es requerido")]
        [DisplayName("Avión ID")]
        public long AircraftID { get; set; }
        #endregion

        #region [Metodos]
        /// <summary>
        /// Establece las propiedades de la flight instance dado un NpgsqlDataReader
        /// (miturriaga)
        /// </summary>
        /// <param name="dr">El NpgsqlDataReader que contiene los datos de la flight instance</param>
        public void SetDesde(NpgsqlDataReader dr)
        {
            this.ID = Convert.ToInt64(dr["flightInstanceID"]);
            this.StateID = Convert.ToInt64(dr["state"]);

            this.Cost = Convert.ToInt32(dr["cost"]);
            this.FlightNumberID = Convert.ToInt64(dr["flightNumber"]);
            
            if (dr["realDeparture"] != DBNull.Value)
            {
                this.RealDeparture = Convert.ToDateTime(dr["realDeparture"]);
            }
            else
            {
                this.RealDeparture = DateTime.MinValue;
            }

            if (dr["realArrival"] != DBNull.Value)
            {
                this.RealArrival = Convert.ToDateTime(dr["realArrival"]);
            }
            else
            {
                this.RealArrival = DateTime.MinValue;
            }

            if (dr["estimatedDeparture"] != DBNull.Value)
            {
                this.EstimatedDeparture = Convert.ToDateTime(dr["estimatedDeparture"]);
            }
            else
            {
                this.EstimatedDeparture = DateTime.MinValue;
            }

            if (dr["estimatedArrival"] != DBNull.Value)
            {
                this.EstimatedArrival = Convert.ToDateTime(dr["estimatedArrival"]);
            }
            else
            {
                this.EstimatedArrival = DateTime.MinValue;
            }
            
            this.AircraftID = Convert.ToInt64(dr["aircraftID"]);
        }

        /// <summary>
        /// Selecciona los flight instance dada la pagina y cantidad de resultados por pagina
        /// (miturriaga)
        /// </summary>
        /// <param name="cantidadResultados">Cantidad de resultado por pagina</param>
        /// <param name="pagina">Pagina que se necesita ver</param>
        /// <returns>La lista de flight instance</returns>
        public List<FlightInstance> Todos(int cantidadResultados, int pagina)
        {

            var flightInstances = new List<FlightInstance>();
            int index = cantidadResultados * (pagina - 1);

            try
            {
                string connString = ConfigurationManager.ConnectionStrings["PostgresConnection"].ConnectionString;

                var comando = new NpgsqlCommand() { CommandText = "FlightInstace_Todos", CommandType = CommandType.StoredProcedure };
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
                        var flightInstance = new FlightInstance();
                        flightInstance.SetDesde(ds);
                        flightInstances.Add(flightInstance);
                    }
                    conn.Close();
                }
            }
            catch (Exception ex) { }

            return flightInstances;
        }

        /// <summary>
        /// Cuenta la cantidad de pasajeros en esta instancia de vuelo
        /// (miturriaga)
        /// </summary>
        /// <returns>La cantidad de pasajeros en esta instancia de vuelo</returns>
        public int PasajerosCount()
        {
            try
            {
                string connString = ConfigurationManager.ConnectionStrings["PostgresConnection"].ConnectionString;

                var comando = new NpgsqlCommand() { CommandText = "Passenger_Count", CommandType = CommandType.StoredProcedure };
                comando.Parameters.Add(new NpgsqlParameter("flightInstanceID", NpgsqlDbType.Integer));
                comando.Parameters[0].Value = this.ID;

                using (var conn = new NpgsqlConnection(connString))
                {
                    conn.Open();
                    comando.Connection = conn;
                    NpgsqlDataReader ds = comando.ExecuteReader();

                    /*while (ds.Read())
                    {
                        var flightInstance = new FlightInstance();
                        flightInstance.SetDesde(ds);
                        flightInstances.Add(flightInstance);
                    }*/
                    conn.Close();
                    return 1;
                }
            }
            catch (Exception ex) { }

            return 0;
        }
        #endregion
    }
}
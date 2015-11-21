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
        /// Selecciona una flight instance dado el id que se entrega como parametro 
        /// (miturriaga)
        /// </summary>
        /// <param name="id">el id de la flight instance a seleccionar</param>
        /// <returns>True si la flight instance fue encontrado, false en caso contrario</returns>
        public bool Seleccionar(long id)
        {
            try
            {
                string connString = ConfigurationManager.ConnectionStrings["PostgresConnection"].ConnectionString;

                var comando = new NpgsqlCommand() { CommandText = "FlightInstance_Seleccionar", CommandType = CommandType.StoredProcedure };
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
        /// Cuenta la cantidad de pasajeros en esta instancia de vuelo
        /// (miturriaga)
        /// </summary>
        /// <returns>La cantidad de pasajeros en esta instancia de vuelo</returns>
        public int PasajerosCount()
        {
            int count = 0;
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

                    if (ds.Read())
                    {
                        count = Convert.ToInt32(ds["passenger_count"]);
                    }
                    conn.Close();
                }
            }
            catch (Exception ex) { }

            return count;
        }

        public List<Aircraft> AircraftTodos()
        {
            return Aircraft.Todos();
        }

        /// <summary>
        /// Realiza el check-in de esta reserva de vuelo como transaccion
        /// (miturriaga)
        /// </summary>
        public string ChangeAirplane()
        {
            try
            {
                string connString = ConfigurationManager.ConnectionStrings["PostgresConnection"].ConnectionString;

                using (var conn = new NpgsqlConnection(connString))
                {
                    conn.Open();
                    NpgsqlTransaction t = conn.BeginTransaction();

                    try
                    {
                        var comando = new NpgsqlCommand()
                        {
                            CommandText = "select \"aircraftCapacity\" from \"Aircraft\" where \"aircraftID\" = :airplaneID"
                        };

                        comando.Parameters.Add(new NpgsqlParameter("airplaneID", NpgsqlDbType.Integer));
                        comando.Parameters[0].Value = this.AircraftID;
                        comando.Connection = conn;
                        comando.Transaction = t;

                        int newCapacity = (int)comando.ExecuteScalar();

                        comando = new NpgsqlCommand()
                        {
                            CommandText = "select \"Aircraft\".\"aircraftCapacity\" from \"Aircraft\",\"FlightInstance\" " +
                                          "where \"FlightInstance\".\"flightInstanceID\" = :id AND " +
                                          "\"Aircraft\".\"aircraftID\" = \"FlightInstance\".\"aircraftID\""
                        };

                        comando.Parameters.Add(new NpgsqlParameter("id", NpgsqlDbType.Integer));
                        comando.Parameters[0].Value = this.ID;
                        comando.Connection = conn;
                        comando.Transaction = t;

                        int oldCapacity = (int)comando.ExecuteScalar();
                        long diff = 0;

                        if (newCapacity < oldCapacity) //Hay que verificar la cantidad de pasajeros
                        {
                            comando = new NpgsqlCommand()
                            {
                                CommandText = "select count(*) from \"FlightReservation\" " +
                                              "where \"FlightReservation\".\"flightInstanceID\" = :id"
                            };

                            comando.Parameters.Add(new NpgsqlParameter("id", NpgsqlDbType.Integer));
                            comando.Parameters[0].Value = this.ID;
                            comando.Connection = conn;
                            comando.Transaction = t;

                            long pasajeros = Convert.ToInt64(comando.ExecuteScalar());

                            if (pasajeros > newCapacity) //Hay que cancelar algunas reservas!!!
                            {
                                diff = pasajeros - newCapacity;

                                comando = new NpgsqlCommand()
                                {
                                    CommandText = "SELECT * FROM \"FlightReservation\" WHERE \"flightInstanceID\" = :inID LIMIT :inTotal"
                                };
                                comando.Parameters.Add(new NpgsqlParameter("inID", NpgsqlDbType.Integer));
                                comando.Parameters[0].Value = this.ID;
                                comando.Parameters.Add(new NpgsqlParameter("inTotal", NpgsqlDbType.Integer));
                                comando.Parameters[1].Value = diff;
                                comando.Connection = conn;
                                comando.Transaction = t;

                                List<String> identificadores = new List<string>();
                                NpgsqlDataReader reader = comando.ExecuteReader();

                                while (reader.Read())
                                {
                                    string IDReservacion = reader.GetString(reader.GetOrdinal("reservationID"));
                                    identificadores.Add(IDReservacion);
                                }

                                reader.Close();

                                foreach (string iD in identificadores)
                                {
                                    comando = new NpgsqlCommand()
                                    {
                                        CommandText = "SELECT \"paymentID\" FROM \"ItineraryReservation\" WHERE \"reservationID\" = :id"
                                    };
                                    comando.Parameters.Add(new NpgsqlParameter("id", NpgsqlDbType.Integer));
                                    comando.Parameters[0].Value = iD;
                                    comando.Connection = conn;
                                    comando.Transaction = t;

                                    long paymentID = (long)comando.ExecuteScalar();

                                    comando = new NpgsqlCommand()
                                    {
                                        CommandText = "UPDATE \"Payment\" SET \"paymentAmount\" = 0 WHERE \"paymentID\" = :paymentID"
                                    };
                                    comando.Parameters.Add(new NpgsqlParameter("paymentID", NpgsqlDbType.Integer));
                                    comando.Parameters[0].Value = paymentID;
                                    comando.Connection = conn;
                                    comando.Transaction = t;
                                    comando.ExecuteNonQuery();

                                    comando = new NpgsqlCommand()
                                    {
                                        CommandText = "DELETE FROM \"FlightReservation\" WHERE :id = \"reservationID\" AND \"flightInstanceID\" = :inID"
                                    };
                                    comando.Parameters.Add(new NpgsqlParameter("id", NpgsqlDbType.Integer));
                                    comando.Parameters[0].Value = iD;
                                    comando.Parameters.Add(new NpgsqlParameter("inID", NpgsqlDbType.Integer));
                                    comando.Parameters[1].Value = this.ID;
                                    comando.Connection = conn;
                                    comando.Transaction = t;
                                    comando.ExecuteNonQuery();

                                    comando = new NpgsqlCommand()
                                    {
                                        CommandText = "UPDATE \"ItineraryReservation\" SET \"reservationState\" = 4 WHERE \"reservationID\" = :id"
                                    };
                                    comando.Parameters.Add(new NpgsqlParameter("id", NpgsqlDbType.Integer));
                                    comando.Parameters[0].Value = iD;
                                    comando.Connection = conn;
                                    comando.Transaction = t;
                                    comando.ExecuteNonQuery();
                                }
                            }
                        }

                        //Listos ahora actualizar el aircraftID en la tabla
                        comando = new NpgsqlCommand()
                        {
                            CommandText = "update \"FlightInstance\" " +
                                          "set \"aircraftID\" = :airplaneID " +
                                          "where \"FlightInstance\".\"flightInstanceID\" = :id"
                        };
                        comando.Parameters.Add(new NpgsqlParameter("airplaneID", NpgsqlDbType.Integer));
                        comando.Parameters[0].Value = this.AircraftID;
                        comando.Parameters.Add(new NpgsqlParameter("id", NpgsqlDbType.Integer));
                        comando.Parameters[1].Value = this.ID;
                        comando.Connection = conn;
                        comando.Transaction = t;
                        comando.ExecuteNonQuery();

                        t.Commit();
                        conn.Close();
                        return "Se ha cambiado el avión, y se han cancelado " + diff + " reservas.";
                    }
                    catch (Exception ex)
                    {
                        t.Rollback();
                        return ex.Message;
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// Cancela una instancia de vuelo
        /// (miturriaga)
        /// </summary>
        public string CancelInstance()
        {
            try
            {
                string connString = ConfigurationManager.ConnectionStrings["PostgresConnection"].ConnectionString;

                using (var conn = new NpgsqlConnection(connString))
                {
                    conn.Open();
                    NpgsqlTransaction t = conn.BeginTransaction();

                    try
                    {
                        var comando = new NpgsqlCommand()
                        {
                            CommandText = "select \"FlightState\".\"stateName\" from \"FlightState\", \"FlightInstance\" " + 
                                          "where \"FlightInstance\".\"flightInstanceID\" = :id AND " + 
                                          "\"FlightInstance\".\"state\" = \"FlightState\".\"flightStateID\""
                        };

                        comando.Parameters.Add(new NpgsqlParameter("id", NpgsqlDbType.Integer));
                        comando.Parameters[0].Value = this.ID;
                        comando.Connection = conn;
                        comando.Transaction = t;

                        string estado = (string)comando.ExecuteScalar();
                        string mensaje = "";

                        if (estado.Equals("Scheduled"))
                        {
                            comando = new NpgsqlCommand()
                            {
                                CommandText = "SELECT * FROM \"FlightReservation\" WHERE \"flightInstanceID\" = :id"
                            };
                            comando.Parameters.Add(new NpgsqlParameter("id", NpgsqlDbType.Integer));
                            comando.Parameters[0].Value = this.ID;
                            comando.Connection = conn;
                            comando.Transaction = t;

                            List<String> identificadores = new List<string>();
                            NpgsqlDataReader reader = comando.ExecuteReader();

                            while (reader.Read())
                            {
                                string IDReservacion = reader.GetString(reader.GetOrdinal("reservationID"));
                                identificadores.Add(IDReservacion);
                            }

                            reader.Close();

                            foreach (string iD in identificadores)
                            {
                                comando = new NpgsqlCommand()
                                {
                                    CommandText = "SELECT \"paymentID\" FROM \"ItineraryReservation\" WHERE \"reservationID\" = :id"
                                };
                                comando.Parameters.Add(new NpgsqlParameter("id", NpgsqlDbType.Integer));
                                comando.Parameters[0].Value = iD;
                                comando.Connection = conn;
                                comando.Transaction = t;

                                long paymentID = (long)comando.ExecuteScalar();

                                comando = new NpgsqlCommand()
                                {
                                    CommandText = "UPDATE \"Payment\" SET \"paymentAmount\" = 0 WHERE \"paymentID\" = :paymentID"
                                };
                                comando.Parameters.Add(new NpgsqlParameter("paymentID", NpgsqlDbType.Integer));
                                comando.Parameters[0].Value = paymentID;
                                comando.Connection = conn;
                                comando.Transaction = t;
                                comando.ExecuteNonQuery();

                                comando = new NpgsqlCommand()
                                {
                                    CommandText = "UPDATE \"ItineraryReservation\" SET \"reservationState\" = 4 WHERE \"reservationID\" = :id"
                                };
                                comando.Parameters.Add(new NpgsqlParameter("id", NpgsqlDbType.Integer));
                                comando.Parameters[0].Value = iD;
                                comando.Connection = conn;
                                comando.Transaction = t;
                                comando.ExecuteNonQuery();

                                comando = new NpgsqlCommand()
                                {
                                    CommandText = "DELETE FROM \"FlightReservation\" WHERE :id = \"reservationID\" AND \"flightInstanceID\" = :inID"
                                };
                                comando.Parameters.Add(new NpgsqlParameter("id", NpgsqlDbType.Integer));
                                comando.Parameters[0].Value = iD;
                                comando.Parameters.Add(new NpgsqlParameter("inID", NpgsqlDbType.Integer));
                                comando.Parameters[1].Value = this.ID;
                                comando.Connection = conn;
                                comando.Transaction = t;
                                comando.ExecuteNonQuery();
                            }

                            //Listo... cambiar el estado de la instancia de vuelo a cancelado
                            comando = new NpgsqlCommand()
                            {
                                CommandText = "update \"FlightInstance\" " +
                                              "set \"state\" = 2 " +
                                              "where \"FlightInstance\".\"flightInstanceID\" = :id"
                            };
                            comando.Parameters.Add(new NpgsqlParameter("id", NpgsqlDbType.Integer));
                            comando.Parameters[0].Value = this.ID;
                            comando.Connection = conn;
                            comando.Transaction = t;

                            comando.ExecuteNonQuery();

                            mensaje = "La instancia fue cancelada exitosamente =)";
                        }
                        else
                        {
                            mensaje = "La instancia de vuelo no puede ser cancelada.";
                        }
                        
                        t.Commit();
                        conn.Close();
                        return mensaje;
                    }
                    catch (Exception ex)
                    {
                        t.Rollback();
                        return ex.Message;
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        #endregion
    }
}
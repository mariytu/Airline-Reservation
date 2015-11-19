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
    public class ItineraryReservation
    {
        #region [Propiedades]

        public long ID { get; set; }

        [Required(ErrorMessage = "El campo fecha de reserva es requerido")]
        [DisplayName("Fecha de Reserva")]
        public DateTime DateReservationMade { get; set; }

        public String DateReservationMadeString
        {
            get
            {
                if (DateTime.MinValue == this.DateReservationMade)
                {
                    return "-";
                }
                return DateReservationMade.ToString("dd-MM-yyyy HH:mm");
            }
        }

        public long AgentID { get; set; }

        public long PassengerID { get; set; }

        [Required(ErrorMessage = "El campo estado reserva es requerido")]
        [DisplayName("Estado Reserva")]
        public long ReservationStateID { get; set; }

        public string ReservationState
        {
            get
            {
                if (this.ReservationStateID == 1)
                {
                    return "Reserved";
                }
                else if (this.ReservationStateID == 2)
                {
                    return "Check-in";
                }
                else if (this.ReservationStateID == 3)
                {
                    return "Finished";
                }
                else
                {
                    return "Canceled";
                }
            }
        }

        [Required(ErrorMessage = "El campo pago reserva es requerido")]
        [DisplayName("Pago")]
        public long PaymentID { get; set; }

        public bool Payment
        {
            get
            {
                if (this.PaymentID >= 1)
                {
                    return true;
                }
                return false;
            }
        }

        [DisplayName("Fecha de Salida")]
        public DateTime EstimatedDeparture { get; set; }

        public String EstimatedDepartureString
        {
            get
            {
                if (DateTime.MinValue == this.EstimatedDeparture)
                {
                    return "-";
                }
                return EstimatedDeparture.ToString("dd-MM-yyyy HH:mm");
            }
        }

        [DisplayName("Fecha de Pago")]
        public DateTime PaymentDate { get; set; }

        public String PaymentDateString
        {
            get
            {
                if (DateTime.MinValue == this.PaymentDate)
                {
                    return "-";
                }
                return PaymentDate.ToString("dd-MM-yyyy HH:mm");
            }
        }

        [DisplayName("Monto del Pago")]
        public int PaymentAmount { get; set; }

        #endregion

        #region [Metodos]
        /// <summary>
        /// Establece las propiedades del itinerary reservation dado un NpgsqlDataReader
        /// (miturriaga)
        /// </summary>
        /// <param name="dr">El NpgsqlDataReader que contiene los datos del itinerary reservation</param>
        public void SetDesde(NpgsqlDataReader dr)
        {
            this.ID = Convert.ToInt64(dr["reservationID"]);

            if (dr["dateReservationMade"] != DBNull.Value)
            {
                this.DateReservationMade = Convert.ToDateTime(dr["dateReservationMade"]);
            }
            else
            {
                this.DateReservationMade = DateTime.MinValue;
            }

            this.AgentID = Convert.ToInt64(dr["agentID"]);
            this.PassengerID = Convert.ToInt64(dr["passengerID"]);
            this.ReservationStateID = Convert.ToInt64(dr["reservationState"]);
            this.PaymentID = Convert.ToInt64(dr["paymentID"]);

            if (dr["estimatedDeparture"] != DBNull.Value)
            {
                this.EstimatedDeparture = Convert.ToDateTime(dr["estimatedDeparture"]);
            }
            else
            {
                this.EstimatedDeparture = DateTime.MinValue;
            }

            if (dr["paymentDate"] != DBNull.Value)
            {
                this.PaymentDate = Convert.ToDateTime(dr["paymentDate"]);
            }
            else
            {
                this.PaymentDate = DateTime.MinValue;
            }

            this.PaymentAmount = Convert.ToInt32(dr["paymentAmount"]);
        }

        /// <summary>
        /// Selecciona un itinerary reservation dado el id que se entrega como parametro 
        /// (miturriaga)
        /// </summary>
        /// <param name="id">el id del itinerary reservation a seleccionar</param>
        /// <returns>True si el itinerary reservation fue encontrado, false en caso contrario</returns>
        public bool Seleccionar(long id)
        {
            try
            {
                string connString = ConfigurationManager.ConnectionStrings["PostgresConnection"].ConnectionString;

                var comando = new NpgsqlCommand() { CommandText = "ItineraryReservation_Seleccionar", CommandType = CommandType.StoredProcedure };
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
        /// Selecciona todos los itinerary reservation de un pasajero
        /// (miturriaga)
        /// </summary>
        /// <param name="id">el id del pasajero</param>
        /// <returns>La lista de itinerary reservations del pasajero</returns>
        public static List<ItineraryReservation> TodosPasajero(long id)
        {

            var reservations = new List<ItineraryReservation>();

            try
            {
                string connString = ConfigurationManager.ConnectionStrings["PostgresConnection"].ConnectionString;

                var comando = new NpgsqlCommand() { CommandText = "ItineraryReservation_TodosPasajero", CommandType = CommandType.StoredProcedure };
                comando.Parameters.Add(new NpgsqlParameter("inID", NpgsqlDbType.Integer));
                comando.Parameters[0].Value = id;

                using (var conn = new NpgsqlConnection(connString))
                {
                    conn.Open();
                    comando.Connection = conn;
                    NpgsqlDataReader ds = comando.ExecuteReader();

                    while (ds.Read())
                    {
                        var itineraryReservation = new ItineraryReservation();
                        itineraryReservation.SetDesde(ds);
                        reservations.Add(itineraryReservation);
                    }
                    conn.Close();
                }
            }
            catch (Exception ex) { }

            return reservations;
        }

        /// <summary>
        /// Realiza el check-in de esta reserva de vuelo como SP
        /// (miturriaga)
        /// </summary>
        public string CheckInSP()
        {
            try
            {
                string connString = ConfigurationManager.ConnectionStrings["PostgresConnection"].ConnectionString;

                var comando = new NpgsqlCommand() { CommandText = "ItineraryReservation_CheckIn", CommandType = CommandType.StoredProcedure };
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
            catch (Exception ex)
            {
                return ex.Message;

            }
            return "Se ha realizado el check-in de forma exitosa =)";
        }

        /// <summary>
        /// Realiza el check-in de esta reserva de vuelo como transaccion
        /// (miturriaga)
        /// </summary>
        public string CheckInTrans()
        {
            try
            {
                string connString = ConfigurationManager.ConnectionStrings["PostgresConnection"].ConnectionString;

                using (var conn = new NpgsqlConnection(connString))
                {
                    conn.Open();
                    NpgsqlTransaction t = conn.BeginTransaction();

                    var comando = new NpgsqlCommand()
                    {
                        CommandText = "SELECT COUNT(*) FROM \"ItineraryReservation\" LEFT JOIN \"ReservationState\" ON \"ReservationState\".\"reservationID\" = \"ItineraryReservation\".\"reservationState\" WHERE   \"ItineraryReservation\".\"reservationID\" = :id AND	\"ReservationState\".\"reservationName\" = 'Reserved';"
                    };
                    comando.Parameters.Add(new NpgsqlParameter("id", NpgsqlDbType.Integer));
                    comando.Parameters[0].Value = this.ID;
                    comando.Connection = conn;
                    comando.Transaction = t;

                    var resultado = comando.ExecuteScalar();

                    int resultado2 = Convert.ToInt32(resultado);

                    if (resultado2 > 0)
                    {
                        comando = new NpgsqlCommand()
                        {
                            CommandText = "SELECT  \"Payment\".\"paymentAmount\" FROM \"ItineraryReservation\" LEFT JOIN \"Payment\" ON \"Payment\".\"paymentID\" = \"ItineraryReservation\".\"paymentID\" WHERE \"ItineraryReservation\".\"reservationID\" = :id ;"
                        };
                        comando.Parameters.Add(new NpgsqlParameter("id", NpgsqlDbType.Integer));
                        comando.Parameters[0].Value = this.ID;
                        comando.Connection = conn;
                        comando.Transaction = t;

                        resultado = comando.ExecuteScalar();

                       int  monto = Convert.ToInt32(resultado);
                        if(monto > 0){
                            comando = new NpgsqlCommand()
                            {
                                CommandText = "UPDATE \"ItineraryReservation\" SET \"reservationState\" = 2 WHERE \"ItineraryReservation\".\"reservationID\" = :inID;"
                            };
                            comando.Parameters.Add(new NpgsqlParameter("inID", NpgsqlDbType.Integer));
                            comando.Parameters[0].Value = this.ID;
                            comando.Connection = conn;
                            comando.Transaction = t;
                            comando.ExecuteNonQuery();
                        }
                        else
                        {
                            return "El usuario no puede realizar check-in, no se ha pagado el total del pasaje";
                        }
                    }
                    else
                    {
                        return "El itinerario ya realizo check-in";
                    }
                    t.Commit();
                    conn.Close();
                }
            }
               
            catch (Exception ex)
            {
                
                return ex.Message;
            }
            return "Se ha realizado el check-in de forma exitosa =)";
        }

        /// <summary>
        /// Cancela una reserva de itinerario
        /// (miturriaga)
        /// </summary>
        public string CancelReservation()
        {
            try
            {
                string connString = ConfigurationManager.ConnectionStrings["PostgresConnection"].ConnectionString;

                using (var conn = new NpgsqlConnection(connString))
                {
                    conn.Open();
                    NpgsqlTransaction t = conn.BeginTransaction();

                    var comando = new NpgsqlCommand()
                    {
                        CommandText = "SELECT COUNT(*) FROM \"ItineraryReservation\" LEFT JOIN \"ReservationState\" ON \"ReservationState\".\"reservationID\" = \"ItineraryReservation\".\"reservationState\" WHERE   \"ItineraryReservation\".\"reservationID\" = :id AND	\"ReservationState\".\"reservationName\" = 'Reserved';"
                    };
                    comando.Parameters.Add(new NpgsqlParameter("id", NpgsqlDbType.Integer));
                    comando.Parameters[0].Value = this.ID;
                    comando.Connection = conn;
                    comando.Transaction = t;

                    var resultado = comando.ExecuteScalar();

                    int resultado2 = Convert.ToInt32(resultado);
                    if(resultado2 > 0 ){ //significa que esta en estado reservado

                        comando = new NpgsqlCommand()
                        {
                            CommandText = "SELECT * FROM \"ItineraryReservation\" LEFT JOIN \"ReservationState\" ON \"ReservationState\".\"reservationID\" = \"ItineraryReservation\".\"reservationState\" WHERE   \"ItineraryReservation\".\"reservationID\" = :id AND	\"ReservationState\".\"reservationName\" = 'Reserved';"
                        };
                        comando.Parameters.Add(new NpgsqlParameter("id", NpgsqlDbType.Integer));
                        comando.Parameters[0].Value = this.ID;
                        comando.Connection = conn;
                        comando.Transaction = t;

                        List<String> identificadores = new List<string>() ;

                        using (var reader = comando.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                //ahora debo eliminar la reserva asociado con los vuelos correspondientes 

                                string IDReservacion = reader.GetString(reader.GetOrdinal("reservationID"));
                                identificadores.Add(IDReservacion);

                            }
                        }

                        foreach (string iD in identificadores){
                             comando = new NpgsqlCommand()
                            {
                                CommandText = "DELETE FROM \"FlightReservation\"  WHERE \"FlightReservation\".\"reservationID\"= :id"
                            };
                            comando.Parameters.Add(new NpgsqlParameter("id", NpgsqlDbType.Integer));
                            comando.Parameters[0].Value = iD;
                            comando.Connection = conn;
                            comando.Transaction = t;
                            comando.ExecuteNonQuery();


                            comando = new NpgsqlCommand()
                            {
                                CommandText = "UPDATE \"ItineraryReservation\" SET \"reservationState\" = 4 WHERE \"ItineraryReservation\".\"reservationID\" = :inID;"
                            };
                            comando.Parameters.Add(new NpgsqlParameter("inID", NpgsqlDbType.Integer));
                            comando.Parameters[0].Value = iD;
                            comando.Connection = conn;
                            comando.Transaction = t;
                            comando.ExecuteNonQuery();

                            
                        }

                    }
                    else
                    {
                        return "No se puede cancelar un itinerario si no esta en estado reservado";
                    }
                    
                    t.Commit();
                    conn.Close();
                    return "Itinerario cancelado exitosamente ";
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
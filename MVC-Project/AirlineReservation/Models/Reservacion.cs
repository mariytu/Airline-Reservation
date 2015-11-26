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
    public class Reservacion
    {
        #region [Propiedades]

        [Required(ErrorMessage = "El campo agencia es requerido")]
        [DisplayName("Agencia")]
        public long AgenciaID { get; set; }

        [Required(ErrorMessage = "El campo aeropuerto de origen es requerido")]
        [DisplayName("Aeropuerto de Origen")]
        public long AeropuertoOrigenID { get; set; }

        [Required(ErrorMessage = "El campo aeropuerto de destino es requerido")]
        [DisplayName("Aeropuerto de Destino")]
        public long AeropuertoDestinoID { get; set; }

        [Required(ErrorMessage = "El campo pasajero es requerido")]
        [DisplayName("Pasajero")]
        public long PasajeroID { get; set; }

        [Required(ErrorMessage = "El campo pasajero es requerido")]
        [DisplayName("Pasajero")]
        public DateTime fecha { get; set; }

        #endregion

        #region [Metodos]

        public  List<City> AeropuertosTodos()
        {
            var passengers = new List<City>();

            try
            {
                string connString = ConfigurationManager.ConnectionStrings["PostgresConnection"].ConnectionString;

                var comando = new NpgsqlCommand()
                {
                    CommandText = "SELECT * FROM \"City\""
                };

                using (var conn = new NpgsqlConnection(connString))
                {
                    conn.Open();
                    comando.Connection = conn;
                    NpgsqlDataReader ds = comando.ExecuteReader();

                    while (ds.Read())
                    {
                        var ciudad = new City();
                        ciudad.SetDesde(ds);
                        passengers.Add(ciudad);
                    }
                    conn.Close();
                }
            }
            catch (Exception ex) { }

            return passengers;
        }

        public List<FlightInstance> VuelosPosibles(int pagina, long ciudadOrigen , long CiudadDestino)
        {
            int cantidadResultados = 5;
            var flightInstances = new List<FlightInstance>();
            int index = cantidadResultados * (pagina - 1);

            try
            {
                string connString = ConfigurationManager.ConnectionStrings["PostgresConnection"].ConnectionString;

                var comando = new NpgsqlCommand() { CommandText = "flightinstance_todos_reserva", CommandType = CommandType.StoredProcedure };
                comando.Parameters.Add(new NpgsqlParameter("inIndex", NpgsqlDbType.Integer));
                comando.Parameters[0].Value = index;
                comando.Parameters.Add(new NpgsqlParameter("inNext", NpgsqlDbType.Integer));
                comando.Parameters[1].Value = cantidadResultados;
                comando.Parameters.Add(new NpgsqlParameter("origen", NpgsqlDbType.Bigint));
                comando.Parameters[2].Value = ciudadOrigen;
                comando.Parameters.Add(new NpgsqlParameter("destino", NpgsqlDbType.Bigint));
                comando.Parameters[3].Value = CiudadDestino;

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

        public List<Passenger> PasajerosTodos()
        {

            var passengers = new List<Passenger>();

            try
            {
                string connString = ConfigurationManager.ConnectionStrings["PostgresConnection"].ConnectionString;

                var comando = new NpgsqlCommand()
                {
                    CommandText = "SELECT * FROM \"Passengers\""
                };

                using (var conn = new NpgsqlConnection(connString))
                {
                    conn.Open();
                    comando.Connection = conn;
                    NpgsqlDataReader ds = comando.ExecuteReader();

                    while (ds.Read())
                    {
                        var passenger = new Passenger();
                        passenger.SetDesde(ds);
                        passengers.Add(passenger);
                    }
                    conn.Close();
                }
            }
            catch (Exception ex) { }

            return passengers;
        }

        public String Reservar(DateTime hora, int agentID , int passengerID, int flightInstanceID, int IDAircraft)
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
                            CommandText = "SELECT COUNT(*) FROM \"FlightReservation\" WHERE \"FlightReservation\".\"flightInstanceID\" = :flightInstanceID ;"
                        };

                        comando.Parameters.Add(new NpgsqlParameter("flightInstanceID", NpgsqlDbType.Integer));
                        comando.Parameters[0].Value = flightInstanceID;
                        comando.Connection = conn;
                        comando.Transaction = t;

                        var pasajerostmp = comando.ExecuteScalar();

                        int pasajeros = Convert.ToInt32(pasajerostmp);

                        var comando2 = new NpgsqlCommand()
                        {
                            CommandText = "SELECT \"Aircraft\".\"aircraftCapacity\" FROM \"Aircraft\" WHERE \"Aircraft\".\"aircraftID\" = :IDavion;"
                        };

                        comando2.Parameters.Add(new NpgsqlParameter("IDavion", NpgsqlDbType.Integer));
                        comando2.Parameters[0].Value = IDAircraft;
                        comando2.Connection = conn;
                        comando2.Transaction = t;

                        var capacidadtmp = comando2.ExecuteScalar();
                        int capacidad = Convert.ToInt32(capacidadtmp);

                        if(pasajeros<capacidad){
                            //el avion tiene capacidad para poder reservar
                            try {

                                var comando3 = new NpgsqlCommand()
                                {
                                    CommandText = "SELECT COUNT(*) FROM \"FlightInstance\" WHERE \"FlightInstance\".state = 3 AND 	\"FlightInstance\".\"flightInstanceID\" = :IDvuelo;"
                                };

                                comando3.Parameters.Add(new NpgsqlParameter("IDvuelo", NpgsqlDbType.Integer));
                                comando3.Parameters[0].Value = flightInstanceID;
                                comando3.Connection = conn;
                                comando3.Transaction = t;

                                var estadoAgendado = comando3.ExecuteScalar();
                                int estado = Convert.ToInt32(estadoAgendado);
                                if(estado > 0){

                                    comando = new NpgsqlCommand()
                                    {
                                        CommandText = "INSERT INTO \"ItineraryReservation\"(\"reservationID\",\"dateReservationMade\", \"agentID\", \"passengerID\", \"reservationState\", \"paymentID\") VALUES ( :reservationID,:dateReservation, :agentID, :passengerID, :reservationState, :paymentID);"
                                    };
                                    comando.Parameters.Add(new NpgsqlParameter("reservationID", NpgsqlDbType.Integer));
                                    comando.Parameters[0].Value = 952;
                                    comando.Parameters.Add(new NpgsqlParameter("dateReservation", NpgsqlDbType.Date));
                                    comando.Parameters[1].Value = hora;
                                    comando.Parameters.Add(new NpgsqlParameter("agentID", NpgsqlDbType.Integer));
                                    comando.Parameters[2].Value = agentID;
                                    comando.Parameters.Add(new NpgsqlParameter("passengerID", NpgsqlDbType.Integer));
                                    comando.Parameters[3].Value = passengerID;
                                    comando.Parameters.Add(new NpgsqlParameter("reservationState", NpgsqlDbType.Integer));
                                    comando.Parameters[4].Value = 1;
                                    comando.Parameters.Add(new NpgsqlParameter("paymentID", NpgsqlDbType.Integer));
                                    comando.Parameters[5].Value = 1;
                                    comando.Connection = conn;
                                    comando.Transaction = t;
                                    comando.ExecuteNonQuery();


                                    comando = new NpgsqlCommand()
                                    {
                                        CommandText = "SELECT MAX(\"ItineraryReservation\".\"reservationID\")FROM \"ItineraryReservation\" ;"
                                    };
                                    comando.Connection = conn;
                                    comando.Transaction = t;
                                    var IDtmp = comando3.ExecuteScalar();
                                    int ID = Convert.ToInt32(IDtmp);


                                    comando = new NpgsqlCommand()
                                    {
                                        CommandText = "INSERT INTO \"FlightReservation\"(\"reservationID\", \"flightInstanceID\", \"seatNumber\", \"number\")    VALUES (:reservationID, :flightInstanceID, :seatNumber, :number);"
                                    };
                                    comando.Parameters.Add(new NpgsqlParameter("reservationID", NpgsqlDbType.Integer));
                                    comando.Parameters[0].Value = ID;
                                    comando.Parameters.Add(new NpgsqlParameter("flightInstanceID", NpgsqlDbType.Integer));
                                    comando.Parameters[1].Value = flightInstanceID;
                                    comando.Parameters.Add(new NpgsqlParameter("seatNumber", NpgsqlDbType.Integer));
                                    comando.Parameters[2].Value = pasajeros+1;
                                    comando.Parameters.Add(new NpgsqlParameter("number", NpgsqlDbType.Integer));
                                    comando.Parameters[3].Value = 1;
                                    comando.Connection = conn;
                                    comando.Transaction = t;
                                    comando.ExecuteNonQuery();


                                    t.Commit();
                                    return "ha sido reservado con exito ";





                                }else{

                                    t.Rollback();
                                    return "el vuelo no esta en estado agendado";
                                }
                            }catch (Exception ex){
                                t.Rollback();
                                return ex.Message;
                            }
                        }
                        else
                        {
                            t.Rollback();
                            return "no pudo reservar, avion a su maxima capacidad";
                        }
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
            return "Se ha realizado la reserva de forma exitosa =)";
           
        }

        public List<Agent> AgenciasTodos()
        {

            var agents = new List<Agent>();

            try
            {
                string connString = ConfigurationManager.ConnectionStrings["PostgresConnection"].ConnectionString;

                var comando = new NpgsqlCommand()
                {
                    CommandText = "SELECT * FROM \"BookingAgent\""
                };

                using (var conn = new NpgsqlConnection(connString))
                {
                    conn.Open();
                    comando.Connection = conn;
                    NpgsqlDataReader ds = comando.ExecuteReader();

                    while (ds.Read())
                    {
                        var agent = new Agent();
                        agent.SetDesde(ds);
                        agents.Add(agent);
                    }
                    conn.Close();
                }
            }
            catch (Exception ex) { }

            return agents;
        }



        #endregion
    }
}
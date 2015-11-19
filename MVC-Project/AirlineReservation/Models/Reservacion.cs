﻿using Npgsql;
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

        public  List<Aircraft> AeropuertosTodos()
        {
            return Aircraft.Todos();
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
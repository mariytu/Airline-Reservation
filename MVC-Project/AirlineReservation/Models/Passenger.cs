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
    public class Passenger
    {
        #region [Propiedades]
        public long ID { get; set; }

        [Required(ErrorMessage = "El campo nombre es requerido")]
        [DisplayName("Nombre")]
        [StringLength(30)]
        public string FirstName { get; set; }

        [DisplayName("Segundo Nombre")]
        [StringLength(30)]
        public string SecondName { get; set; }

        [Required(ErrorMessage = "El campo apellido es requerido")]
        [DisplayName("Apellido")]
        [StringLength(30)]
        public string LastName { get; set; }

        [DisplayName("Teléfono")]
        public int PhoneNumber { get; set; }

        [Required(ErrorMessage = "El campo dirección es requerido")]
        [DisplayName("Dirección")]
        [StringLength(40)]
        public string AddressLine { get; set; }

        [Required(ErrorMessage = "El campo correo es requerido")]
        [DisplayName("Correo")]
        [StringLength(45)]
        public string EmailAddress { get; set; }

        public long CityID { get; set; }

        [DisplayName("Ciudad")]
        public City City { get; set; }
        #endregion

        #region [Metodos]
        /// <summary>
        /// Establece las propiedades del passenger dado un NpgsqlDataReader
        /// (miturriaga)
        /// </summary>
        /// <param name="dr">El NpgsqlDataReader que contiene los datos del passenger</param>
        public void SetDesde(NpgsqlDataReader dr)
        {
            this.ID = Convert.ToInt64(dr["passengerID"]);
            this.FirstName = dr["firstName"].ToString();
            this.SecondName = dr["secondName"].ToString();
            this.LastName = dr["lastName"].ToString();
            this.PhoneNumber = Convert.ToInt32(dr["phoneNumber"]);
            this.AddressLine = dr["addressLine"].ToString();
            this.EmailAddress = dr["emailAddress"].ToString();
            this.CityID = Convert.ToInt64(dr["cityID"]);

            this.City = new City();
            this.City.Seleccionar(this.CityID);
        }

        /// <summary>
        /// Selecciona los passenger dada la pagina y cantidad de resultados por pagina
        /// (miturriaga)
        /// </summary>
        /// <param name="cantidadResultados">Cantidad de resultado por pagina</param>
        /// <param name="pagina">Pagina que se necesita ver</param>
        /// <returns>La lista de passenger</returns>
        public List<Passenger> Todos(int cantidadResultados, int pagina)
        {

            var passengers = new List<Passenger>();
            int index = cantidadResultados * (pagina - 1);

            try
            {
                string connString = ConfigurationManager.ConnectionStrings["PostgresConnection"].ConnectionString;

                var comando = new NpgsqlCommand() { CommandText = "Passenger_Todos", CommandType = CommandType.StoredProcedure };
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

        /// <summary>
        /// Selecciona un passenger dado el id que se entrega como parametro 
        /// (miturriaga)
        /// </summary>
        /// <param name="id">el id del passenger a seleccionar</param>
        /// <returns>True si el passenger fue encontrado, false en caso contrario</returns>
        public bool Seleccionar(long id)
        {
            try
            {
                string connString = ConfigurationManager.ConnectionStrings["PostgresConnection"].ConnectionString;

                var comando = new NpgsqlCommand() { CommandText = "Passenger_Seleccionar", CommandType = CommandType.StoredProcedure };
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
        /// Crea este nuevo passenger en la base de datos
        /// (miturriaga)
        /// </summary>
        public void Crear()
        {
            try
            {
                string connString = ConfigurationManager.ConnectionStrings["PostgresConnection"].ConnectionString;

                var comando = new NpgsqlCommand() { CommandText = "Passenger_Crear", CommandType = CommandType.StoredProcedure };
                comando.Parameters.Add(new NpgsqlParameter("outID", NpgsqlDbType.Integer));
                comando.Parameters[0].Direction = ParameterDirection.Output;
                comando.Parameters[0].Value = 0;
                comando.Parameters.Add(new NpgsqlParameter("inFirstName", NpgsqlDbType.Text));
                comando.Parameters[1].Value = this.FirstName;
                comando.Parameters.Add(new NpgsqlParameter("inSecondName", NpgsqlDbType.Text));
                comando.Parameters[2].Value = this.SecondName;
                comando.Parameters.Add(new NpgsqlParameter("inLastName", NpgsqlDbType.Text));
                comando.Parameters[3].Value = this.LastName;
                comando.Parameters.Add(new NpgsqlParameter("inPhoneNumber", NpgsqlDbType.Integer));
                comando.Parameters[4].Value = this.PhoneNumber;
                comando.Parameters.Add(new NpgsqlParameter("inAddressLine", NpgsqlDbType.Text));
                comando.Parameters[5].Value = this.AddressLine;
                comando.Parameters.Add(new NpgsqlParameter("inEmailAddress", NpgsqlDbType.Text));
                comando.Parameters[6].Value = this.EmailAddress;
                comando.Parameters.Add(new NpgsqlParameter("inCityID", NpgsqlDbType.Integer));
                comando.Parameters[7].Value = this.CityID;

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
        /// Modifica este passenger en la base de datos
        /// (miturriaga)
        /// </summary>
        public void Modificar()
        {
            try
            {
                string connString = ConfigurationManager.ConnectionStrings["PostgresConnection"].ConnectionString;

                var comando = new NpgsqlCommand() { CommandText = "Passenger_Modificar", CommandType = CommandType.StoredProcedure };
                comando.Parameters.Add(new NpgsqlParameter("inID", NpgsqlDbType.Integer));
                comando.Parameters[0].Value = this.ID;
                comando.Parameters.Add(new NpgsqlParameter("inFirstName", NpgsqlDbType.Text));
                comando.Parameters[1].Value = this.FirstName;
                comando.Parameters.Add(new NpgsqlParameter("inSecondName", NpgsqlDbType.Text));
                comando.Parameters[2].Value = this.SecondName;
                comando.Parameters.Add(new NpgsqlParameter("inLastName", NpgsqlDbType.Text));
                comando.Parameters[3].Value = this.LastName;
                comando.Parameters.Add(new NpgsqlParameter("inPhoneNumber", NpgsqlDbType.Integer));
                comando.Parameters[4].Value = this.PhoneNumber;
                comando.Parameters.Add(new NpgsqlParameter("inAddressLine", NpgsqlDbType.Text));
                comando.Parameters[5].Value = this.AddressLine;
                comando.Parameters.Add(new NpgsqlParameter("inEmailAddress", NpgsqlDbType.Text));
                comando.Parameters[6].Value = this.EmailAddress;
                comando.Parameters.Add(new NpgsqlParameter("inCityID", NpgsqlDbType.Integer));
                comando.Parameters[7].Value = this.CityID;

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
        /// Elimina este passenger de la base de datos
        /// (miturriaga)
        /// </summary>
        public void Eliminar()
        {
            try
            {
                string connString = ConfigurationManager.ConnectionStrings["PostgresConnection"].ConnectionString;

                var comando = new NpgsqlCommand() { CommandText = "Passenger_Eliminar", CommandType = CommandType.StoredProcedure };
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

        public List<City> CityTodos()
        {
            return City.Todos();
        }

        /// <summary>
        /// Retorna un string que representa brevemente esta entidad
        /// (miturriaga)
        /// </summary>
        /// <returns>Un string con la representacion del passenger</returns>
        public override string ToString()
        {
            return string.Format("Passenger (ID: {0}, Name: {1}, LastName: {2})", this.ID, this.FirstName, this.LastName);
        }

        #endregion
    }
}
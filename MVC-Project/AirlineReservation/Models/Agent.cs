using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AirlineReservation.Models
{
    public class Agent
    {
        #region [Propiedades]

        public long ID { get; set; }

        public string AgentName { get; set; }

        public string AgentDetails { get; set; }

        #endregion

        #region [Metodos]
        /// <summary>
        /// Establece las propiedades del agente dado un NpgsqlDataReader
        /// (miturriaga)
        /// </summary>
        /// <param name="dr">El NpgsqlDataReader que contiene los datos del agente</param>
        public void SetDesde(NpgsqlDataReader dr)
        {
            this.ID = Convert.ToInt64(dr["agentID"]);
            this.AgentName = dr["agentName"].ToString();
            this.AgentDetails = dr["agentDetails"].ToString();
        }
        #endregion
    }
}
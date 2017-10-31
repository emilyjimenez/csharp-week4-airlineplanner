using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;

namespace AirlinePlanner.Models
{
    public class Flight
    {
        public string OriginCity {get; private set;}
        public string DestinationCity {get; private set;}
        public string Depart {get; private set;}
        public string Arrive {get; private set;}
        public string Status {get; private set;}
        public int Id {get; private set;}


        public Flight(string originCity, string destinationCity, string depart, string arrive, string status , int id = 0)
        {
          OriginCity = originCity;
          DestinationCity = destinationCity;
          Depart = depart;
          Arrive = arrive;
          Status = status;
          Id = id;
        }

        public static Flight Find(int id)
        {
          MySqlConnection conn = DB.Connection();
          conn.Open();
          var cmd = conn.CreateCommand() as MySqlCommand;
          cmd.CommandText =@"SELECT * FROM flights WHERE id = (@searchId);";

          MySqlParameter searchId = new MySqlParameter();
          searchId.ParameterName = "@searchId";
          searchId.Value = id;
          cmd.Parameters.Add(searchId);

          var rdr = cmd.ExecuteReader() as MySqlDataReader;
          int flightId = 0;
          string flightOrigin = "";
          string flightDestination = "";
          string flightDepart = "";
          string flightArrive = "";
          string flightStatus = "";

          while(rdr.Read())
          {
            flightId = rdr.GetInt32(0);
            flightOrigin = rdr.GetString(1);
            flightDestination = rdr.GetString(2);
            flightDepart = rdr.GetString(3);
            flightArrive = rdr.GetString(4);
            flightStatus = rdr.GetString(5);

          }
          Flight newFlight = new Flight(flightOrigin, flightDestination, flightDepart, flightArrive, flightStatus, flightId);
          conn.Close();
          if(conn != null)
          {
            conn.Dispose();
          }
          return newFlight;
        }

        public void Save()
        {
          MySqlConnection conn = DB.Connection();
          conn.Open();

          var cmd = conn.CreateCommand() as MySqlCommand;
          cmd.CommandText = @"INSERT INTO flights (origin_city, destination_city, depart, arrive, status) VALUES (@OriginCity, @DestinationCity, @Depart, @Arrive, @Status);";

          MySqlParameter origin_city = new MySqlParameter();
          origin_city.ParameterName = "@OriginCity";
          origin_city.Value = this.OriginCity;
          cmd.Parameters.Add(origin_city);

          MySqlParameter destination_city = new MySqlParameter();
          destination_city.ParameterName = "@DestinationCity";
          destination_city.Value = this.DestinationCity;
          cmd.Parameters.Add(destination_city);

          MySqlParameter depart = new MySqlParameter();
          depart.ParameterName = "@Depart";
          depart.Value = this.Depart;
          cmd.Parameters.Add(depart);

          MySqlParameter arrive = new MySqlParameter();
          arrive.ParameterName = "@Arrive";
          arrive.Value = this.Arrive;
          cmd.Parameters.Add(arrive);

          MySqlParameter status = new MySqlParameter();
          status.ParameterName = "@Status";
          status.Value = this.Status;
          cmd.Parameters.Add(status);

          cmd.ExecuteNonQuery();
          Id = (int) cmd.LastInsertedId;

          conn.Close();
          if (conn != null)
          {
              conn.Dispose();
          }
        }

        public static List<Flight> GetAll()
        {
            List<Flight> allFlights = new List<Flight> {};

            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM flights;";

            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            while(rdr.Read())
            {
                int flightID = rdr.GetInt32(0);
                string flightOrigin = rdr.GetString(1);
                string flightDestination = rdr.GetString(2);
                string flightDeparture = rdr.GetString(3);
                string flightArrival = rdr.GetString(4);
                string flightStatus = rdr.GetString(5);
                Flight newFlight = new Flight(flightOrigin, flightDestination, flightDeparture, flightArrival, flightStatus, flightID);
                allFlights.Add(newFlight);
            }

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return allFlights;
        }


        public static void ClearAll()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM flights;";
            cmd.ExecuteNonQuery();

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public override bool Equals(System.Object otherFlight)
        {
          if (!(otherFlight is Flight))
          {
            return false;
          }
          else
          {
             Flight newFlight = (Flight) otherFlight;
             bool idEquality = this.Id == newFlight.Id;
             bool originEquality = this.OriginCity == newFlight.OriginCity;
             bool destinationEquality = this.DestinationCity == newFlight.DestinationCity;
             bool departEquality = this.Depart == newFlight.Depart;
             bool arriveEquality = this.Arrive == newFlight.Arrive;
             bool statusEquality = this.Status == newFlight.Status;
             return (idEquality && originEquality && destinationEquality && departEquality && arriveEquality && statusEquality);
           }
        }

        public override int GetHashCode()
        {
             return this.Id.GetHashCode();
        }
    }
}

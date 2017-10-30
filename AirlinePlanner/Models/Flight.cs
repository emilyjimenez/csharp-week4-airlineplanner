using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;

namespace AirlinePlanner.Models
{
    public class Flight
    {
        private int _id;
        private string _departureTime;
        private int _originCityId;
        private int _destinationCityId;
        private string _flightStatus;

        public Flight(string depatureTime, string flightStatus, int id = 0)
        {
            _departureTime = depatureTime;
            _flightStatus = flightStatus;
            _id = id;
        }

        public int GetId()
        {
            return _id;
        }

        public string GetDepatureTime()
        {
            return _departureTime;
        }

        public string GetFlightStatus()
        {
            return _flightStatus;
        }

        public int GetOriginCityId()
        {
            return _originCityId;
        }

        public void AddOriginCityId()
        {

        }

        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO flights (time, status) VALUES (@departureTime, @flightStatus);";

            MySqlParameter time = new MySqlParameter();
            time.ParameterName = "@departureTime";
            time.Value = this.GetDepatureTime();
            cmd.Parameters.Add(time);

            MySqlParameter status = new MySqlParameter();
            status.ParameterName = "@flightStatus";
            status.Value = this.GetFlightStatus();
            cmd.Parameters.Add(status);

            cmd.ExecuteNonQuery();
            _id = (int) cmd.LastInsertedId;

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
                string flightDepartureTime = rdr.GetString(1);
                string flightStatus = rdr.GetString(2);
                Flight newFlight = new Flight(flightDepartureTime, flightStatus,  flightID);
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
             bool idEquality = this.GetId() == newFlight.GetId();
             bool timeEquality = this.GetDepatureTime() == newFlight.GetDepatureTime();
             bool statusEquality = this.GetFlightStatus() == newFlight.GetFlightStatus();
             return (idEquality && timeEquality && statusEquality);
           }
        }
        public override int GetHashCode()
        {
             return this.GetDepatureTime().GetHashCode();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;

namespace AirlinePlanner.Models
{
    public class City
    {
        private int _id;
        private string _name;

        public City(string name, int id = 0)
        {
            _name = name;
            _id = id;
        }

        public int GetId()
        {
            return _id;
        }

        public string GetName()
        {
            return _name;
        }

        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO cities (name) VALUES (@name);";

            MySqlParameter name = new MySqlParameter();
            name.ParameterName = "@name";
            name.Value = this._name;
            cmd.Parameters.Add(name);

            cmd.ExecuteNonQuery();
            _id = (int) cmd.LastInsertedId;
            Console.WriteLine("SAVE METHOD ID: " + _id);

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public static List<City> GetAll()
        {
            List<City> allCities = new List<City> {};

            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM cities;";

            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            while(rdr.Read())
            {
                int cityID = rdr.GetInt32(0);
                string cityName = rdr.GetString(1);
                City newCity = new City(cityName, cityID);
                allCities.Add(newCity);
            }

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return allCities;
        }


        public static void ClearAll()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM cities;";
            cmd.ExecuteNonQuery();

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public override bool Equals(System.Object otherCity)
        {
          if (!(otherCity is City))
          {
            return false;
          }
          else
          {
             City newCity = (City) otherCity;
             bool idEquality = this.GetId() == newCity.GetId();
             bool nameEquality = this.GetName() == newCity.GetName();
             return (idEquality && nameEquality);
           }
        }
        public override int GetHashCode()
        {
             return this.GetName().GetHashCode();
        }
    }
}

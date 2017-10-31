using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;

namespace AirlinePlanner.Models
{
    public class City
    {
        public string Code {get; private set;}
        public int Id {get; private set;}

        public City(string code, int id = 0)
        {
            Code = code;
            Id = id;
        }

        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO cities (code) VALUES (@Code);";

            MySqlParameter code = new MySqlParameter();
            code.ParameterName = "@Code";
            code.Value = this.Code;
            cmd.Parameters.Add(code);

            cmd.ExecuteNonQuery();
            Id = (int) cmd.LastInsertedId;

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
                string cityCode = rdr.GetString(1);
                City newCity = new City(cityCode, cityID);
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
             bool idEquality = this.Id == newCity.Id;
             bool codeEquality = this.Code == newCity.Code;
             return (idEquality && codeEquality);
           }
        }
        public override int GetHashCode()
        {
             return this.Id.GetHashCode();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AirlinePlanner.Models;

namespace AirlinePlanner.Models.Tests
{
  [TestClass]
  public class FlightTests : IDisposable
  {
    public void Dispose()
    {
      Flight.ClearAll();
    }

    public FlightTests()
    {
        DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=airline_test;";
    }

    [TestMethod]
    public void GetAll_RetrievesEmptyListOfFlightsFromDatabase_0()
    {
        List<Flight> flights = Flight.GetAll();

        Assert.AreEqual(0, flights.Count);
    }

    [TestMethod]
    public void Save_SavesFlightToDatabase_Flight()
    {
        Flight newFlight = new Flight("7:00", "ON TIME");
        newFlight.Save();

        List<Flight> flights = Flight.GetAll();
        List<Flight> expectedFlights = new List<Flight>{newFlight};
        CollectionAssert.AreEqual(expectedFlights, flights);
    }

    [TestMethod]
    public void Equals_OverrideTrueForSameDepatureTime_Flight()
    {
        Flight flight1 = new Flight("7:00", "ON TIME");
        Flight flight2 = new Flight("7:00", "ON TIME");

        Assert.AreEqual(flight1, flight2);
    }
  }
}

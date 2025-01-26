using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S10267326_PRG2Assignment
{
    abstract class Flight
    {
        public string FlightNumber { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public DateTime ExpectedTime { get; set; }
        public string Status { get; set; }
        public Flight(string flightNumber, string origin, string destination, DateTime expectedTime)
        {
            FlightNumber = flightNumber;
            Origin = origin;
            Destination = destination;
            ExpectedTime = expectedTime;
            Status = "On Time";
        }
        public virtual double CalculateFees()
        {
            double fees = 0;
            if (Destination == "SIN")
            {
                return 500;
            }
            else if (Origin == "SIN")
            {
                return 800;
            }
            return 0;
        }
        public override string ToString()
        {
            return "Flight Number: " + FlightNumber + "\tOrigin: " + Origin + "\tDestination: " + Destination + "\tExpected Time: " + ExpectedTime + "\tStatus: " + Status;
        }
    }
    class NORMFlight : Flight
    {
        public NORMFlight(string flightNumber, string origin, string destination, DateTime expectedTime) : base(flightNumber, origin, destination, expectedTime) { }
        public override double CalculateFees()
        {
            return base.CalculateFees();
        }
        public override string ToString()
        {
            return base.ToString();
        }
    }
    class CFFTFlight : Flight
    {
        public double RequestFee { get; set; }
        public CFFTFlight(string flightNumber, string origin, string destination, DateTime expectedTime) : base(flightNumber, origin, destination, expectedTime)
        {
            RequestFee = 150;
        }
        public override double CalculateFees()
        {
            return base.CalculateFees() + RequestFee;
        }
        public override string ToString()
        {
            return base.ToString();
        }
    }
    class LWTTFlight : Flight
    {
        public double RequestFee { get; set; }
        public LWTTFlight(string flightNumber, string origin, string destination, DateTime expectedTime) : base(flightNumber, origin, destination, expectedTime)
        {
            RequestFee = 500;
        }
        public override double CalculateFees()
        {
            return base.CalculateFees() + RequestFee;
        }
        public override string ToString()
        {
            return base.ToString();
        }
    }
    class DDJBFlight : Flight
    {
        public double RequestFee { get; set; }
        public DDJBFlight(string flightNumber, string origin, string destination, DateTime expectedTime) : base(flightNumber, origin, destination, expectedTime)
        {
            RequestFee = 300;
        }
        public override double CalculateFees()
        {
            return base.CalculateFees() + RequestFee;
        }
        public override string ToString()
        {
            return base.ToString();
        }
    }
}

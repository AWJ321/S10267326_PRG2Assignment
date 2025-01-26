using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S10267326_PRG2Assignment
{
    class Terminal
    {
        public string TerminalName { get; set; }
        public Dictionary<string, Airline> Airlines { get; set; } = new Dictionary<string, Airline>();
        public Dictionary<string, Flight> Flights { get; set; } = new Dictionary<string, Flight> { };
        public Dictionary<string, BoardingGate> BoardingGates { get; set; } = new Dictionary<string, BoardingGate>();
        public Dictionary<string, double> GateFees { get; set; } = new Dictionary<string, double>();
        public Terminal(string terminalName)
        {
            TerminalName = terminalName;
        }
        public bool AddAirline(Airline airline)
        {
            if (!Airlines.ContainsKey(airline.Code))
            {
                Airlines[airline.Code] = airline;
                return true;
            }
            return false;
        }

        public bool AddBoardingGate(BoardingGate gate)
        {
            if (!BoardingGates.ContainsKey(gate.GateName))
            {
                BoardingGates[gate.GateName] = gate;
                return true;
            }
            return false;
        }
        public Airline GetAirlineFromFlight(Flight flight)
        {
            foreach (Airline airline in Airlines.Values)
            {
                if (airline.Flights.ContainsKey(flight.FlightNumber))
                {
                    return airline;
                }
            }
            return null;
        }

        public void PrintAirlineFees()
        {
            foreach (Airline airline in Airlines.Values)
            {
                Console.WriteLine($"Airline: {airline.Name}, Total Fees: {airline.CalculateFees()}");
            }
        }

        public override string ToString()
        {
            return "Terminal: " + TerminalName;
        }
    }
    class Airline
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public Dictionary<string, Flight> Flights { get; set; } = new Dictionary<string, Flight>();
        public Airline(string name, string code)
        {
            Name = name;
            Code = code;
        }
        public bool AddFlight(Flight flight)
        {
            if (!Flights.ContainsKey(flight.FlightNumber))
            {
                Flights[flight.FlightNumber] = flight;
                return true;
            }
            return false;
        }
        public double CalculateFees()
        {
            double totalFees = 0;

            foreach (Flight flight in Flights.Values)
            {
                totalFees += flight.CalculateFees();
            }
            return totalFees;
        }
        public bool RemoveFlight(Flight flight)
        {
            return Flights.Remove(flight.FlightNumber);
        }
        public override string ToString()
        {
            return "Airline: " + Name + "\tCode: " + Code;
        }
    }
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
    class BoardingGate
    {
        public string GateName { get; set; }
        public bool SupportsCFFT { get; set; }
        public bool SupportsDDJB { get; set; }
        public bool SupportsLWTT { get; set; }
        public Flight Flight { get; set; }
        public BoardingGate(string gateName, bool supportsCFFT, bool supportsDDJB, bool supportsLWTT)
        {
            GateName = gateName;
            SupportsCFFT = supportsCFFT;
            SupportsDDJB = supportsDDJB;
            SupportsLWTT = supportsLWTT;
            Flight = null;
        }
        public double CalculateFees()
        {
            return 300;
        }
        public override string ToString()
        {
            return "Gate Name: " + GateName + "\tSupportsCFFT: " + SupportsCFFT + "\tSupportsDDJB: " + SupportsDDJB + "\tSupportsLWTT: " + SupportsLWTT + "\tFlight" + Flight;
        }
    }
}

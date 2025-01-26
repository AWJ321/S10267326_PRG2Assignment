//==========================================================
// Student Number : S10267326
// Student Name : Ang Wei Jun
// Partner Name : Wee Qi Cheng
//==========================================================

using S10267326_PRG2Assignment;
using System.ComponentModel.Design;
using System.Xml.Serialization;

Terminal terminal = new Terminal("Terminal 5");

// Q1
// ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
void LoadFiles()
{
    string[] lines1 = File.ReadAllLines("airlines.csv");

    Console.WriteLine("Loading Airlines...");
    int count = 0;
    for (int i = 1; i < lines1.Length; i++)
    {
        count += 1;
        string[] details = lines1[i].Split(',');
        string name = details[0];
        string code = details[1];
        Airline a = new Airline(name, code);
        terminal.AddAirline(a);
    }
    Console.WriteLine($"{count} Airlines Loaded!");

    string[] lines2 = File.ReadAllLines("boardinggates.csv");

    Console.WriteLine("Loading Boarding Gates...");
    int count2 = 0;
    for (int i = 1; i < lines2.Length; i++)
    {
        count2 += 1;
        string[] details = lines2[i].Split(',');
        string gateName = details[0];
        bool supportsDDJB = Convert.ToBoolean(details[1]);
        bool supportsCFFT = Convert.ToBoolean(details[2]);
        bool supportsLWTT = Convert.ToBoolean(details[3]);
        BoardingGate bg = new BoardingGate(gateName, supportsCFFT, supportsDDJB, supportsLWTT);
        terminal.AddBoardingGate(bg);
    }
    Console.WriteLine($"{count2} Boarding Gates Loaded!");
}
LoadFiles();

// Q2
// ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
void LoadFiles2()
{
    string[] lines3 = File.ReadAllLines("flights.csv");

    Console.WriteLine("Loading Flights...");
    int count = 0;
    for (int i = 1; i < lines3.Length; i++)
    {
        count += 1;
        string[] details = lines3[i].Split(',');
        string flightNo = details[0];
        string origin = details[1];
        string destination = details[2];

        DateTime expectedTime;
        try
        {
            expectedTime = Convert.ToDateTime(details[3]);
        }
        catch (FormatException)
        {
            expectedTime = DateTime.ParseExact(details[3], "d/M/yyyy h:mm tt", null);
        }

        string code = details[4];

        Flight flight;

        if (code == "DDJB")
            flight = new DDJBFlight(flightNo, origin, destination, expectedTime);
        else if (code == "CFFT")
            flight = new CFFTFlight(flightNo, origin, destination, expectedTime);
        else if (code == "LWTT")
            flight = new LWTTFlight(flightNo, origin, destination, expectedTime);
        else
            flight = new NORMFlight(flightNo, origin, destination, expectedTime);

        terminal.Flights[flightNo] = flight;
        string airlineCode = flightNo.Substring(0, 2);
        terminal.Airlines[airlineCode].AddFlight(flight);
    }
    Console.WriteLine($"{count} Flights Loaded!");
}
LoadFiles2();

// Q3
// ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
void ListFlights()
{
    Console.WriteLine(@"=============================================
List of Flights for Changi Airport Terminal 5
=============================================");
    Console.WriteLine("{0,-16}{1,-23}{2,-23}{3,-23}{4}", "Flight Number", "Airline Name", "Origin", "Destination", "Expected Departure/Arrival Time");
    foreach (Flight flight in terminal.Flights.Values)
    {
        // Extract airline code from flight number (e.g., "SQ" from "SQ 115")
        string airlineCode = flight.FlightNumber.Substring(0, 2);

        // Get the airline name using the code
        string airlineName = terminal.Airlines[airlineCode].Name;
        Console.WriteLine("{0,-16}{1,-23}{2,-23}{3,-23}{4}", flight.FlightNumber, airlineName, flight.Origin, flight.Destination, flight.ExpectedTime);
    }
}
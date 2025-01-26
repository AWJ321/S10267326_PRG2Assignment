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

// Q4 
// ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
void ListBoardingGates()
{
    Console.WriteLine(@"=============================================
List of Boarding Gates for Changi Airport Terminal 5
=============================================");
    Console.WriteLine("{0,-20}{1,-20}{2,-20}{3,-20}{4}", "Gate Name", "DDJB", "CFFT", "LWTT", "Flight Number");
    foreach (BoardingGate gate in terminal.BoardingGates.Values)
    {
        string flightNo;
        if (gate.Flight == null)
        {
            flightNo = "Unassigned";
        }
        else
        {
            flightNo = gate.Flight.FlightNumber;
        }
        Console.WriteLine("{0,-20}{1,-20}{2,-20}{3,-20}{4}", gate.GateName, gate.SupportsDDJB, gate.SupportsCFFT, gate.SupportsLWTT, flightNo);
    }
}

// Q5
// ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
void AssignGateToFlight()
{
    Console.WriteLine(@"=============================================
Assign a Boarding Gate to a Flight
=============================================");
    while (true)
    {
        Console.WriteLine("Enter Flight Number: ");
        string flightNo1 = Console.ReadLine();
        if (!terminal.Flights.ContainsKey(flightNo1))
        {
            Console.WriteLine($"Flight {flightNo1} not found. Please try again.");
            continue;
        }
        Flight selectedFlight = terminal.Flights[flightNo1];

        Console.WriteLine("Enter Boarding Gate Name: ");
        string gateName1 = Console.ReadLine();
        if (!terminal.BoardingGates.ContainsKey(gateName1))
        {
            Console.WriteLine($"Boarding Gate {gateName1} not found. Please try again.");
            continue;
        }

        BoardingGate selectedGate = terminal.BoardingGates[gateName1];

        if (selectedGate.Flight != null)
        {
            Console.WriteLine($"Boarding Gate {gateName1} is already assigned to Flight {selectedGate.Flight.FlightNumber}. Please try again.");
            continue;
        }
        selectedGate.Flight = selectedFlight;
        Console.Write($@"
Flight Number: {selectedFlight.FlightNumber}
Origin: {selectedFlight.Origin}
Destination: {selectedFlight.Destination}
Expected Time: {selectedFlight.ExpectedTime}
Special Request Code: ");
        if (selectedFlight is NORMFlight)
            Console.WriteLine("None");
        else if (selectedFlight is DDJBFlight)
            Console.WriteLine("DDJB");
        else if (selectedFlight is CFFTFlight)
            Console.WriteLine("CFFT");
        else
            Console.WriteLine("LWTT");
        Console.WriteLine($@"Boarding Gate Name: {gateName1}
Supports DDJB: {selectedGate.SupportsDDJB}
Supports CFFT: {selectedGate.SupportsCFFT}
Supports LWTT: {selectedGate.SupportsLWTT}");
        while (true)
        {
            Console.Write("Would you like to update the status of the flight? (Y/N): ");
            string updateStatus = Console.ReadLine().ToUpper();
            if (updateStatus == "Y")
            {
                Console.WriteLine(@"1. Delayed
2. Boarding
3. On Time");
                while (true)
                {
                    Console.Write("Please select the new status of the flight: ");
                    string statusOption = Console.ReadLine();
                    if (statusOption == "1")
                    {
                        selectedFlight.Status = "Delayed";
                        break;
                    }
                    else if (statusOption == "2")
                    {
                        selectedFlight.Status = "Boarding";
                        break;
                    }
                    else if (statusOption == "3")
                    {
                        selectedFlight.Status = "On Time";
                        break;
                    }
                    else
                        Console.WriteLine("Invalid input, please try again.");
                }
                break;
            }
            else if (updateStatus == "N")
                break;
            else
                Console.WriteLine("Invalid Input, please try again.");
        }
        Console.WriteLine($"Flight {selectedFlight.FlightNumber} has been assigned to Boarding Gate {gateName1}!");
        break;
    }
}

//Q6
// ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
void CreateFlight()
{
    while (true)
    {
        Console.Write("Enter Flight Number: ");
        string flightNo = Console.ReadLine();

        Console.Write("Enter Origin: ");
        string origin = Console.ReadLine();

        Console.Write("Enter Destination: ");
        string destination = Console.ReadLine();

        DateTime expectedTime;
        while (true)
        {

            try
            {
                Console.Write("Enter Expected Departure/Arrival Time (dd/mm/yyyy hh:mm): ");
                string input = Console.ReadLine();
                expectedTime = DateTime.ParseExact(input, "d/M/yyyy HH:mm", null);
                break;
            }
            catch (FormatException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        Flight flight;
        string specialRequestCode;
        while (true)
        {
            Console.Write("Enter Special Request Code (CFFT/DDJB/LWTT/None): ");
            specialRequestCode = Console.ReadLine().ToUpper();
            if (specialRequestCode == "CFFT")
            {
                flight = new CFFTFlight(flightNo, origin, destination, expectedTime);
                break;
            }
            else if (specialRequestCode == "DDJB")
            {
                flight = new DDJBFlight(flightNo, origin, destination, expectedTime);
                break;
            }
            else if (specialRequestCode == "LWTT")
            {
                flight = new LWTTFlight(flightNo, origin, destination, expectedTime);
                break;
            }
            else if (specialRequestCode == "None")
            {
                flight = new NORMFlight(flightNo, origin, destination, expectedTime);
                break;
            }
            else
                Console.WriteLine("Invalid Input, please try again.");
        }
        terminal.Flights[flightNo] = flight;
        string airlineCode = flightNo.Substring(0, 2);
        terminal.Airlines[airlineCode].AddFlight(flight);

        using (StreamWriter sw = new StreamWriter("flights.csv", true))
        {
            sw.WriteLine($"{flightNo},{origin},{destination},{expectedTime:d/M/yyyy h:mm tt},{specialRequestCode}");
        }

        Console.WriteLine($"Flight {flightNo} has been added!");
        string addFlight = "";
        while (true)
        {
            Console.Write("Would you like to add another flight? (Y/N): ");
            addFlight = Console.ReadLine().ToUpper();
            if (addFlight == "Y" || addFlight == "N")
                break;
            else
                Console.WriteLine("Invalid Input, please try again.");
        }
        if (addFlight == "N")
            break;
        else
            continue;
    }
}
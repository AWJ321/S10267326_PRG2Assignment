﻿//==========================================================
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

// Q2: Load Flights
// ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
void LoadFiles2()
{
    // Load flights from file
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

        // Add flight to terminal and airline dctionaries
        terminal.Flights[flightNo] = flight;
        string airlineCode = flightNo.Substring(0, 2);
        terminal.Airlines[airlineCode].AddFlight(flight);
    }
    Console.WriteLine($"{count} Flights Loaded!");
}
LoadFiles2();

// Q3: List Flights
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

// Q4: List Boarding Gates
// ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
void ListBoardingGates()
{
    Console.WriteLine(@"=============================================
List of Boarding Gates for Changi Airport Terminal 5
=============================================");
    Console.WriteLine("{0,-20}{1,-20}{2,-20}{3,-20}{4}", "Gate Name", "DDJB", "CFFT", "LWTT", "Flight Number");

    // Check if the gate is assigned to a flight
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

// Q5: Assign Gate to a Flight
// ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
void AssignGateToFlight()
{
    Console.WriteLine(@"=============================================
Assign a Boarding Gate to a Flight
=============================================");
    while (true)
    {
        Console.WriteLine("Enter Flight Number: ");
        string flightNo1 = Console.ReadLine().ToUpper();

        // Validate flight number
        if (!terminal.Flights.ContainsKey(flightNo1))
        {
            Console.WriteLine($"Flight {flightNo1} not found. Please try again.");
            continue;
        }
        Flight selectedFlight = terminal.Flights[flightNo1];

        Console.WriteLine("Enter Boarding Gate Name: ");
        string gateName1 = Console.ReadLine().ToUpper();

        // Validate boarding gate name
        if (!terminal.BoardingGates.ContainsKey(gateName1))
        {
            Console.WriteLine($"Boarding Gate {gateName1} not found. Please try again.");
            continue;
        }

        BoardingGate selectedGate = terminal.BoardingGates[gateName1];

        // Check if gate is already assigned
        if (selectedGate.Flight != null)
        {
            Console.WriteLine($"Boarding Gate {gateName1} is already assigned to Flight {selectedGate.Flight.FlightNumber}. Please try again.");
            continue;
        }

        // Assign gate to the flight
        selectedGate.Flight = selectedFlight;

        // Display flight and gate details
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

        // Update flight status
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

// Q6: Create new flight
// ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
void CreateFlight()
{
    while (true)
    {
        // Prompt user for basic flight details
        Console.Write("Enter Flight Number: ");
        string flightNo = Console.ReadLine();
        string airlineCode = flightNo.Substring(0, 2);

        bool isValid = false;
        foreach (Airline airline in terminal.Airlines.Values)
        {
            if (airlineCode == airline.Code)
            {
                isValid = true;
                break;
            }
        }
        if (isValid == false)
        {
            Console.WriteLine("Invalid airline code.");
            continue;
        }

        Console.Write("Enter Origin: ");
        string origin = Console.ReadLine();

        Console.Write("Enter Destination: ");
        string destination = Console.ReadLine();

        // Handle expected time input with validation
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

        // Prompt for and validate special request code
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

        // Add the flight to terminal and airline dictionaries
        terminal.Flights[flightNo] = flight;
        terminal.Airlines[airlineCode].AddFlight(flight);

        // Append new flight to flights.csv file
        using (StreamWriter sw = new StreamWriter("flights.csv", true))
        {
            sw.WriteLine($"{flightNo},{origin},{destination},{expectedTime:d/M/yyyy h:mm tt},{specialRequestCode}");
        }

        Console.WriteLine($"Flight {flightNo} has been added!");

        // Ask if the user wants to add another flight
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

// Q7: Display full flight details from an airline
// ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
void AirlineDetails()
{
    Console.WriteLine(@"=============================================
List of Airlines for Changi Airport Terminal 5
=============================================");

    // Display list of airlines
    foreach (Airline airline in terminal.Airlines.Values)
    {
        Console.WriteLine("{0,-15}{1}", airline.Code, airline.Name);
    }

    // Prompt user to select an airline
    Console.Write("Enter Airline Code: ");
    string airlineCode1 = Console.ReadLine().ToUpper();

    if (!terminal.Airlines.ContainsKey(airlineCode1))
    {
        Console.WriteLine($"Airline Code {airlineCode1} not found. Please try again.");
        return;
    }
    Airline selectedAirline = terminal.Airlines[airlineCode1];

    // Display flights for the selected airline
    Console.WriteLine($@"=============================================
List of Flights for {selectedAirline.Name}
=============================================");
    Console.WriteLine("{0,-20}{1,-23}{2}", "Flight Number", "Origin", "Destination");
    foreach (Flight flight in selectedAirline.Flights.Values)
    {
        Console.WriteLine("{0,-20}{1,-23}{2}", flight.FlightNumber, flight.Origin, flight.Destination);
    }

    // Prompt user to select a flight
    Console.Write("Enter Flight Number: ");
    string flightNumber = Console.ReadLine().ToUpper();
    if (!selectedAirline.Flights.ContainsKey(flightNumber))
    {
        Console.WriteLine($"Flight Number {flightNumber} not found. Please try again.");
        return;
    }
    Flight selectedFlight1 = selectedAirline.Flights[flightNumber];

    // Determine the special request code
    string specialRqcode = "None";
    if (selectedFlight1 is DDJBFlight)
        specialRqcode = "DDJB";
    else if (selectedFlight1 is CFFTFlight)
        specialRqcode = "CFFT";
    else if (selectedFlight1 is LWTTFlight)
        specialRqcode = "LWTT";
    else
        specialRqcode = "None";

    // Check if the flight is assigned to a boarding gate
    string boardingGateName = "Unassigned";
    foreach (BoardingGate gate in terminal.BoardingGates.Values)
    {
        if (gate.Flight == selectedFlight1)
        {
            boardingGateName = gate.GateName;
            break;
        }
    }

    // Display flight details
    Console.WriteLine("{0,-16}{1,-23}{2,-23}{3,-23}{4,-35}{5,-23}{6}", "Flight Number", "Airline Name", "Origin", "Destination", "Expected Departure/Arrival Time", "Special Request Code", "Boarding Gate");
    Console.WriteLine("{0,-16}{1,-23}{2,-23}{3,-23}{4,-35}{5,-23}{6}", selectedFlight1.FlightNumber, selectedAirline.Name, selectedFlight1.Origin, selectedFlight1.Destination, selectedFlight1.ExpectedTime, specialRqcode, boardingGateName);
}


// Q8: Modify Flight Details
// ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

// Display detailed flight information
void DisplayFlightDetails(string flightNumber)
{
    Flight flight = terminal.Flights[flightNumber];
    Console.Write($@"
Flight Number: {flight.FlightNumber}
Origin: {flight.Origin}
Destination: {flight.Destination}
Expected Departure/Arrival Time: {flight.ExpectedTime}
Status: {flight.Status}
Special Request Code: ");
    // Determine special request code (SRC)
    if (flight is NORMFlight)
        Console.WriteLine("None");
    else if (flight is DDJBFlight)
        Console.WriteLine("DDJB");
    else if (flight is CFFTFlight)
        Console.WriteLine("CFFT");
    else
        Console.WriteLine("LWTT");

    // Check and display assigned boarding gate
    string boardingGateName = "Unassigned";
    foreach (BoardingGate gate in terminal.BoardingGates.Values)
    {
        if (gate.Flight == flight)
        {
            boardingGateName = gate.GateName;
            break;
        }
    }
    Console.WriteLine($"Boarding Gate: {boardingGateName}");
}

// Change flight type and update all related dictionaries
void ChangeFlightSRC(Flight flight, string target)
{
    Flight newFlight;

    // Create a new flight object based on the target SRC
    switch (target)
    {
        case "DDJB":
            newFlight = new DDJBFlight(flight.FlightNumber, flight.Origin, flight.Destination, flight.ExpectedTime);
            break;
        case "CFFT":
            newFlight = new CFFTFlight(flight.FlightNumber, flight.Origin, flight.Destination, flight.ExpectedTime);
            break;
        case "LWTT":
            newFlight = new LWTTFlight(flight.FlightNumber, flight.Origin, flight.Destination, flight.ExpectedTime);
            break;
        case "NONE":
            newFlight = new NORMFlight(flight.FlightNumber, flight.Origin, flight.Destination, flight.ExpectedTime);
            break;
        default:
            Console.WriteLine("Invalid SRC type specified.");
            return;
    }

    // Update airline and terminal dictionaries
    string flightNumber = flight.FlightNumber;
    string airlineCode = flightNumber.Substring(0, 2);
    Airline airline = terminal.Airlines[airlineCode];
    airline.RemoveFlight(flight);
    airline.AddFlight(newFlight);
    terminal.Flights[flightNumber] = newFlight;
}

// Display available airlines
void DisplayAirlines()
{
    Console.WriteLine(@"=============================================
List of Airlines for Changi Airport Terminal 5
=============================================");
    foreach (Airline airline in terminal.Airlines.Values)
    {

        Console.WriteLine("{0,-15}{1}", airline.Code, airline.Name);
    }
}

// Display flights for a specific airline
void DisplayFlights(Airline airline)
{
    Console.WriteLine($@"=============================================
List of Flights for {airline.Name}
=============================================");
    Console.WriteLine("{0,-16}{1,-23}{2}", "Flight Number", "Origin", "Destination");
    foreach (Flight flight in airline.Flights.Values)
    {
        Console.WriteLine("{0,-16}{1,-23}{2}", flight.FlightNumber, flight.Origin, flight.Destination);
    }
}

// Modify flight details or delete the flight
void ModifyFlight()
{
    DisplayAirlines();

    Console.Write("Enter Airline Code: ");
    string airlineCode = Console.ReadLine().ToUpper();

    if (!terminal.Airlines.ContainsKey(airlineCode))
    {
        Console.WriteLine($"Airline Code {airlineCode} not found. Please try again.");
        return;
    }

    Airline selectedAirline = terminal.Airlines[airlineCode];
    DisplayFlights(selectedAirline);

    // Prompt user to select a flight
    Console.Write("Choose an existing Flight to modify or delete: ");
    string flightNumber = Console.ReadLine().ToUpper();
    if (!selectedAirline.Flights.ContainsKey(flightNumber))
    {
        Console.WriteLine($"Flight Number {flightNumber} not found. Please try again.");
        return;
    }

    // Display modification options
    Console.WriteLine(@"1. Modify Flight
2. Delete Flight
Choose an option: ");
    string option = Console.ReadLine();

    switch (option)
    {
        case "1":
            ModifySelectedFlight(flightNumber);
            break;

        case "2":
            DeleteSelectedFlight(selectedAirline, flightNumber);
            break;

        default:
            Console.WriteLine("Invalid option.");
            break;
    }
}

// Modify specific flight details
void ModifySelectedFlight(string flightNumber)
{
    Console.WriteLine(@"1. Modify Basic Information
2. Modify Status
3. Modify Special Request Code
4. Modify Boarding Gate
Choose an option: ");
    string option = Console.ReadLine();

    switch (option)
    {
        case "1":
            ModifyBasicInformation(flightNumber);

            break;

        case "2":
            ModifyStatus(flightNumber);
            break;

        case "3":
            ModifySpecialRequestCode(flightNumber);
            break;

        case "4":
            ModifyBoardingGate(flightNumber);
            break;

        default:
            Console.WriteLine("Invalid option.");
            break;
    }

    // Display updated flight details
    DisplayFlightDetails(flightNumber);
}

// Modify basic flight details
void ModifyBasicInformation(string flightNumber)
{
    Flight flight = terminal.Flights[flightNumber];
    Console.Write("Enter new Origin: ");
    string newOrigin = Console.ReadLine();
    Console.Write("Enter new Destination: ");
    string newDestination = Console.ReadLine();
    Console.Write("Enter new Expected Departure/Arrival Time (dd/mm/yyyy hh:mm): ");

    DateTime newExpectedTime = DateTime.ParseExact(Console.ReadLine(), "d/M/yyyy HH:mm", null);

    // Update details   
    flight.Origin = newOrigin;
    flight.Destination = newDestination;
    flight.ExpectedTime = newExpectedTime;

    Console.WriteLine("Flight information updated.");
}

// Modify flight status
void ModifyStatus(string flightNumber)
{
    Flight flight = terminal.Flights[flightNumber];
    while (true)
    {
        Console.WriteLine("Enter new Status (Delayed, Boarding, On Time): ");
        string newStatus = Console.ReadLine();
        if ((newStatus == "Delayed" || newStatus == "Boarding" || newStatus == "On Time") && (newStatus != flight.Status))
        {
            flight.Status = newStatus;
            Console.WriteLine("Flight status updated.");
            break;
        }
        Console.WriteLine("Invalid status, please try again.");
    }
}

// Modify the special request code (SRC) of a flight
void ModifySpecialRequestCode(string flightNumber)
{
    Flight flight = terminal.Flights[flightNumber];
    while (true)
    {
        Console.WriteLine("Enter new SRC (None, DDJB, CFFT, LWTT): ");
        string newSRC = Console.ReadLine().ToUpper();
        if ((newSRC == "NONE" && !(flight is NORMFlight)) || (newSRC == "DDJB" && !(flight is DDJBFlight)) || (newSRC == "CFFT" && !(flight is CFFTFlight)) || (newSRC == "LWTT" && !(flight is LWTTFlight)))
        {
            ChangeFlightSRC(flight, newSRC);
            break;
        }
        else if (newSRC != "NONE" && newSRC != "DDJB" && newSRC != "CFFT" && newSRC != "LWTT")
            Console.WriteLine("Invalid SRC, please try again.");
        else
            Console.WriteLine($"Flight SRC is already {newSRC}");
    }
}

// Modify boarding gate assignment
void ModifyBoardingGate(string flightNumber)
{
    Flight flight = terminal.Flights[flightNumber];
    string newBoardingGate;
    while (true)
    {
        Console.WriteLine("Enter new Boarding Gate: ");
        newBoardingGate = Console.ReadLine().ToUpper();
        if (terminal.BoardingGates.ContainsKey(newBoardingGate))
        {
            break;
        }
        Console.WriteLine("Invalid Boarding Gate, please try again.");
    }

    // Unassign the current gate, if any
    foreach (BoardingGate gate in terminal.BoardingGates.Values)
    {
        if (gate.Flight == flight)
        {
            gate.Flight = null;
            break;
        }
    }

    // Assign new gate
    terminal.BoardingGates[newBoardingGate].Flight = flight;
    Console.WriteLine("Boarding gate updated.");
}

// Delete a flight
void DeleteSelectedFlight(Airline airline, string flightNumber)
{
    Flight flight = terminal.Flights[flightNumber];
    Console.WriteLine($"Confirm again to delete flight {flight.FlightNumber} [Y/N]: ");
    string confirm = Console.ReadLine().ToUpper();
    if (confirm == "Y")
    {
        airline.RemoveFlight(flight);
        terminal.Flights.Remove(flight.FlightNumber);
        Console.WriteLine($"Flight {flight.FlightNumber} deleted.");

        // Remove gate assignment, if any
        foreach (BoardingGate gate in terminal.BoardingGates.Values)
        {
            if (gate.Flight == flight)
            {
                gate.Flight = null;
                break;
            }
        }
    }
    else
    {
        Console.WriteLine("Deletion canceled.");
    }
}


// Q9: Display scheduled flights in chronological order
// ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
void DisplayDayChrono()
{
    // Get today's date (no time component)
    DateTime today = DateTime.Now.Date;

    // List to store flights scheduled for today
    List<Flight> flightList = new List<Flight>();

    // Filter flights for today's date
    foreach (Flight flight in terminal.Flights.Values)
    {
        if (flight.ExpectedTime.Date == today)
        {
            flightList.Add(flight);
        }
    }
    // Sort flights chronologically by ExpectedTime (default sorting for DateTime)
    flightList.Sort();

    // Display table header
    Console.WriteLine("{0,-16}{1,-23}{2,-23}{3,-23}{4,-40}{5,-14}{6,-23}{7}", "Flight Number", "Airline Name", "Origin", "Destination", "Expected Departure/Arrival Time", "Status", "Special Request Code", "Boarding Gate");

    // Iterate through the sorted flight list and display each flight's details
    foreach (Flight flight in flightList)
    {
        string airlineCode = flight.FlightNumber.Substring(0, 2);
        string airlineName = terminal.Airlines[airlineCode].Name;

        string specialRqcode = "None";
        if (flight is DDJBFlight)
            specialRqcode = "DDJB";
        else if (flight is CFFTFlight)
            specialRqcode = "CFFT";
        else if (flight is LWTTFlight)
            specialRqcode = "LWTT";
        else
            specialRqcode = "None";

        // Check if the flight has a boarding gate assigned
        string boardingGateName = "Unassigned";
        foreach (BoardingGate gate in terminal.BoardingGates.Values)
        {
            if (gate.Flight == flight)
            {
                boardingGateName = gate.GateName;
                break;
            }
        }

        // Display flight details
        Console.WriteLine("{0,-16}{1,-23}{2,-23}{3,-23}{4,-40}{5,-14}{6,-23}{7}", flight.FlightNumber, airlineName, flight.Origin, flight.Destination, flight.ExpectedTime, flight.Status, specialRqcode, boardingGateName);
    }
}


// MENU
// ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
Console.WriteLine("\n\n\n");
while (true)
{
    Console.WriteLine();
    Console.Write(@"=============================================
Welcome to Changi Airport Terminal 5
=============================================
1. List All Flights
2. List Boarding Gates
3. Assign a Boarding Gate to a Flight
4. Create Flight
5. Display Airline Flights
6. Modify Flight Details
7. Display Flight Schedule
8. Process Unassigned Flights to Boarding Gates
9. Display Total Fee per Airline
0. Exit");
    Console.WriteLine("\n\nPlease select your option: ");
    string option = Console.ReadLine();
    if (option == "0")
    {
        Console.WriteLine("Goodbye!");
        break;
    }
    else if (option == "1")
    {
        ListFlights();
        continue;
    }
    else if (option == "2")
    {
        ListBoardingGates();
        continue;
    }
    else if (option == "3")
    {
        AssignGateToFlight();
        continue;
    }
    else if (option == "4")
    {
        CreateFlight();
        continue;
    }
    else if (option == "5")
    {
        AirlineDetails();
        continue;
    }
    else if (option == "6")
    {
        ModifyFlight();
        continue;
    }
    else if (option == "7")
    {
        DisplayDayChrono();
        continue;
    }
    else if (option == "8")
    {
        AutoProcessFlightsToGates();
        continue;
    }
    else if (option == "9")
    {
        DisplayFee();
        continue;
    }
    else
    {
        Console.WriteLine("Invalid input, returning to menu.");
    }
}

// Advanced Q1: Automatically assign unassigned flights to unassigned boarding gates
// ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
void AutoProcessFlightsToGates()
{
    Queue<Flight> unassignedFlights = new Queue<Flight>();

    // Counter for flights already assigned to gates
    int flightsAlreadyAssigned = 0;

    // Counter for gates already assigned to flights
    int gatesAlreadyAssigned = 0;

    //Identify unassigned flights and count already assigned flights
    foreach (Flight flight in terminal.Flights.Values)
    {
        bool isAssigned = false;

        // Check if the flight is already assigned to a gate
        foreach (BoardingGate gate in terminal.BoardingGates.Values)
        {
            if (gate.Flight == flight)
            {
                isAssigned = true;
                flightsAlreadyAssigned++;
                break;
            }
        }

        // If unassigned, add to the queue
        if (!isAssigned)
        {
            unassignedFlights.Enqueue(flight);
        }
    }

    // Count unassigned flights
    int totalUnassignedFlights = unassignedFlights.Count;
    Console.WriteLine($"Total number of flights without a boarding gate assigned: {totalUnassignedFlights}");

    // Identify unassigned gates and count already assigned gates
    List<BoardingGate> unassignedGates = new List<BoardingGate>();
    foreach (BoardingGate gate in terminal.BoardingGates.Values)
    {
        // Gate has no flight assigned  
        if (gate.Flight == null)
        {
            unassignedGates.Add(gate);
        }
        else
        {
            gatesAlreadyAssigned++;
        }
    }

    // Count unassigned gates
    int totalUnassignedGates = unassignedGates.Count;
    Console.WriteLine($"Total number of unassigned boarding gates: {totalUnassignedGates}");

    // Count of flights processed
    int totalFlightsProcessed = 0;

    // Count of gates processed
    int totalGatesProcessed = 0;

    //Display heading
    Console.WriteLine("{0,-16}{1,-23}{2,-23}{3,-23}{4,-35}{5,-23}{6}", "Flight Number", "Airline Name", "Origin", "Destination", "Expected Departure/Arrival Time", "Special Request Code", "Boarding Gate");
    while (true)
    {
        Flight flight1;
        try
        {
            flight1 = unassignedFlights.Dequeue();
        }
        // Check if queue has any items 

        catch (InvalidOperationException)
        {
            break;
        }

        string specialRqCode = "None";
        if (flight1 is DDJBFlight)
            specialRqCode = "DDJB";
        else if (flight1 is CFFTFlight)
            specialRqCode = "CFFT";
        else if (flight1 is LWTTFlight)
            specialRqCode = "LWTT";
        else
            specialRqCode = "None";

        BoardingGate gateSearch = null;

        // Find a suitable gate for the flight
        if (specialRqCode != "None") // If the flight has a special request
        {
            foreach (BoardingGate gate in unassignedGates)
            {
                if ((specialRqCode == "DDJB" && gate.SupportsDDJB) || (specialRqCode == "CFFT" && gate.SupportsCFFT) || (specialRqCode == "LWTT" && gate.SupportsLWTT))
                {
                    gateSearch = gate;
                    break;
                }
            }
        }
        else // If the flight has no special request
        {
            foreach (BoardingGate gate in unassignedGates)
            {
                if (!gate.SupportsDDJB && !gate.SupportsCFFT && !gate.SupportsLWTT)
                {
                    gateSearch = gate;
                    break;
                }
            }
        }

        // Assign the gate to the flight and remove the gate from unassigned list
        gateSearch.Flight = flight1;
        unassignedGates.Remove(gateSearch);
        totalFlightsProcessed++;
        totalGatesProcessed++;
    }

    // Print the details of the assigned flight
    foreach (Flight flight in terminal.Flights.Values)
    {
        string airlineCode = flight.FlightNumber.Substring(0, 2);
        string airlineName = terminal.Airlines[airlineCode].Name;

        string specialRqCode = "None";
        if (flight is DDJBFlight)
            specialRqCode = "DDJB";
        else if (flight is CFFTFlight)
            specialRqCode = "CFFT";
        else if (flight is LWTTFlight)
            specialRqCode = "LWTT";
        else
            specialRqCode = "None";

        foreach (BoardingGate gate in terminal.BoardingGates.Values)
        {
            if (gate.Flight == flight)
            {
                Console.WriteLine("{0,-16}{1,-23}{2,-23}{3,-23}{4,-35}{5,-23}{6}", flight.FlightNumber, airlineName, flight.Origin, flight.Destination, flight.ExpectedTime, specialRqCode, gate.GateName);
            }
        }
    }

    // Summary of processing

    Console.WriteLine($"Total Flights Processed and Assigned: {totalFlightsProcessed}");
    Console.WriteLine($"Total Boarding Gates Processed and Assigned: {totalGatesProcessed}");

    if (flightsAlreadyAssigned == 0)
        Console.WriteLine("No flights assigned originally.");
    else
    {
        double percentageFlights = Math.Round((Convert.ToDouble(totalFlightsProcessed) / flightsAlreadyAssigned) * 100, 2);
        Console.WriteLine($"Percentage of Flights Processed Automatically Over Those Already Assigned: {percentageFlights}%");
    }
    if (gatesAlreadyAssigned == 0)
        Console.WriteLine("No gates assigned originally.");
    else
    {
        double percentageGates = Math.Round((Convert.ToDouble(totalGatesProcessed) / gatesAlreadyAssigned) * 100, 2);
        Console.WriteLine($"Percentage of Gates Processed Automatically Over Those Already Assigned: {percentageGates}%");
    }
}


// Advanced Q2: Automatically assign unassigned flights to unassigned boarding gates
// ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
void DisplayFee()
{
    while (true)
    {
        // Check if all flights are assigned
        int countAssigned = 0;
        foreach (Flight flight in terminal.Flights.Values)
        {
            foreach (BoardingGate gate in terminal.BoardingGates.Values)
            {
                if (gate.Flight == flight)
                {
                    countAssigned++;
                }
            }
        }
        if (countAssigned != terminal.Flights.Count)
        {
            Console.WriteLine("Ensure all flights have been assigned before running this feature again.");
            break;
        }

        double termiSubtotal = 0;
        double termiDiscount = 0;
        double termiFinal = 0;

        Console.WriteLine("{0,-20}{1,-20}{2,-20}{3}", "Airline Name", "Original Subtotal", "Discount Total", "Final Subtotal");
        foreach (Airline airline in terminal.Airlines.Values)
        {
            Airline selectedAirline = terminal.Airlines[airline.Code];

            // Calculate airline fee (no discount)
            double airlineFee = 0;
            foreach (Flight flight in selectedAirline.Flights.Values)
            {
                double flightFee = 0;
                if (flight.Origin == "Singapore (SIN)")
                {
                    flightFee += 800;
                }
                if (flight.Destination == "Singapore (SIN)")
                {
                    flightFee += 500;
                }
                double srcFee = flight.CalculateFees();
                flightFee += srcFee;

                airlineFee += flightFee + 300;
            }

            // Calculate airline discount 
            double discountOff = 0;
            int flightCount = airline.Flights.Count();
            int promoCondition1 = (flightCount / 3);

            int promoCondition2 = 0;
            int promoCondition3 = 0;
            int promoCondition4 = 0;
            TimeSpan start = new TimeSpan(21, 0, 0);
            TimeSpan end = new TimeSpan(11, 0, 0);
            foreach (Flight flight in selectedAirline.Flights.Values)
            {
                TimeSpan timeOfFlight = flight.ExpectedTime.TimeOfDay;
                if ((timeOfFlight < start) && (timeOfFlight > end))
                {
                    promoCondition2 += 1;
                }
                if (flight.Origin == "Dubai (DXB)" || flight.Origin == "Bangkok (BKK)" || flight.Origin == "Tokyo (NRT)")
                {
                    promoCondition3 += 1;
                }
                if (flight is NORMFlight)
                {
                    promoCondition4 += 1;
                }
            }

            bool promoCondition5 = false;
            if (flightCount > 5)
            {
                promoCondition5 = true;
            }

            discountOff = (promoCondition1 * 350) + (promoCondition2 * 110) + (promoCondition3 * 25) + (promoCondition4 * 50);

            double originalSubtotal = airlineFee;

            if (promoCondition5 == true)
            {
                airlineFee = airlineFee * 0.97;
            }
            airlineFee = airlineFee - Convert.ToDouble(discountOff);

            // Display values of each airline
            Console.WriteLine("{0,-20}{1,-20:F2}{2,-20:F2}{3:F2}", airline.Name, originalSubtotal, discountOff, airlineFee);

            termiSubtotal += originalSubtotal;
            termiDiscount += discountOff;
            termiFinal += airlineFee;
        }

        // Display Terminal 5 values
        Console.WriteLine($"Subtotal of all airline fees: ${termiSubtotal:F2}");
        Console.WriteLine($"Subtotal of all airline discounts: ${termiDiscount:F2}");
        Console.WriteLine($"Final fee Terminal 5 collects: ${termiFinal:F2}");
        double discOverFinal = Math.Round((termiDiscount / termiFinal) * 100, 2);
        Console.WriteLine($"Percentage of the subtotal discounts over the final total of fees: {discOverFinal:F2}%");

        break;
    }
}
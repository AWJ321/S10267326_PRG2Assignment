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

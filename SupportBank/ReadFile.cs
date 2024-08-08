using System.Text.RegularExpressions;
using NLog;
using NLog.Config;
using NLog.Targets;
using System;
using System.IO;

// Make changes so that when a user runs the program they can see a list of all the transaction files and pick which file they will get a report from.
// Try to do this in a way where you don’t have to hard code the file names.

//list file:
// take the path: C:\Training\w6d2_SupportBank
// list all files .csv .json and .xml at that path (possibly associated with a number)
// user selects the number and program runs appropriate ReadFile method

class ReadFile
{

    public static void ListFiles()
    {
        int fileCount = 0;
        string selectedFile = "";
        List<string> files = new List<string>();

        List<string[]> dirs = new List<string[]>();
        dirs.Add(Directory.GetFiles("C:/Training/w6d2_SupportBank", "*.json"));
        dirs.Add(Directory.GetFiles("C:/Training/w6d2_SupportBank", "*.xml"));
        dirs.Add(Directory.GetFiles("C:/Training/w6d2_SupportBank", "*.csv"));

        Console.WriteLine("The number of files is {0}.", dirs.Count);
        foreach (string[] dir in dirs)
        {
            foreach (string file in dir) {
            fileCount++;
            Console.WriteLine($"{fileCount}) {file}");
            files.Add(file);
            }
        }

        Console.WriteLine("Choose a file (select its number):");
        selectedFile = files[Int32.Parse(Console.ReadLine()) - 1];

        Console.WriteLine($"You selected {selectedFile}");
    }

    // public static void ReadCsv();

    // public static void ReadJson();

}

//do you need to consider the digits after the . in the path?
//".csv" or ".json" or ".xml"
// /.(.*)/g


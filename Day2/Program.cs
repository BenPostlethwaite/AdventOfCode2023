using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace Day2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Process.Start("notepad.exe", "example.txt");
            Process.Start("notepad.exe", "input.txt");


            string[] example = GetLines("example.txt");
            string[] input = GetLines("input.txt");


            Console.WriteLine("Part 1: " + Part1(ref example));
            Console.WriteLine("Part 2: " + Part2(ref example));
            Console.ReadKey();
        }
        static string[] GetLines(string fileLocation)
        {
            //read string from input.txt:
            var sr = new System.IO.StreamReader(fileLocation);
            string lines = sr.ReadToEnd();
            sr.Close();

            string[] linesArray = lines.Split('\n');
            foreach (string line in linesArray)
            {
                line.Trim('\r');
            }
            return linesArray;
        }
        static int Part2(ref string[] lines)
        {
            return 0;
        }
        static int Part1(ref string[] lines)
        {
            return 0;

        }
    }
}

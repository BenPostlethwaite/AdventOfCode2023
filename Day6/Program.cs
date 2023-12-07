using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace Day6
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Process.Start("notepad.exe", "example.txt");
            //Process.Start("notepad.exe", "input.txt");


            string[] example = GetLines("example.txt");
            string[] input = GetLines("input.txt");


            Console.WriteLine("Part 1: " + Part1(ref input));
            Console.WriteLine("Part 2: " + Part2(ref input));
            Console.ReadKey();
        }
        static string[] GetLines(string fileLocation)
        {
            //read string from input.txt:
            var sr = new System.IO.StreamReader(fileLocation);
            string lines = sr.ReadToEnd();
            sr.Close();

            string[] linesArray = lines.Split('\n');
            for (int i = 0; i < linesArray.Length; i++)
            {
                linesArray[i] = linesArray[i].Replace("\r", "");
            }
            return linesArray;
        }
        static ulong Part2(ref string[] lines)
        {
            List<string> times = lines[0].Split(' ').ToList();
            times.RemoveAt(0);
            times.RemoveAll(x => x == "");

            ulong time = ulong.Parse(String.Join("", times));

            List<string> distances = lines[1].Split(' ').ToList();
            distances.RemoveAt(0);
            distances.RemoveAll(x => x == "");
            
            ulong record = ulong.Parse(String.Join("", distances));

            ulong noOfRecords = 0;
            for (ulong timeHeld = 0; timeHeld < time; timeHeld++)
            {
                ulong timeMoving = time - timeHeld;
                ulong distanceTravelled = timeMoving * timeHeld;
                if (distanceTravelled > record)
                {
                    noOfRecords++;
                }
            }
            return noOfRecords;
        }
        static int Part1(ref string[] lines)
        {
            List<string> times = lines[0].Split(' ').ToList();
            times.RemoveAt(0);
            times.RemoveAll(x => x == "");

            List<int> timesInt = times.Select(x => int.Parse(x)).ToList();
            
            List<string> distances = lines[1].Split(' ').ToList();
            distances.RemoveAt(0);
            distances.RemoveAll(x => x == "");

            List<int> distancesInt = distances.Select(int.Parse).ToList();

            (int, int)[] pairs = new (int time, int record)[timesInt.Count];
            for (int i = 0; i < timesInt.Count; i++)
            {
                pairs[i] = (timesInt[i], distancesInt[i]);
            }

            int product = 1;
            foreach ((int time, int record) pair in pairs)
            {
                int noOfRecords = 0;
                for (int timeHeld = 0; timeHeld < pair.time; timeHeld++)
                {
                    int timeMoving = pair.time - timeHeld;
                    int distanceTravelled = timeMoving * timeHeld;
                    if (distanceTravelled > pair.record)
                    {
                        noOfRecords++;
                    }
                }
                product *= noOfRecords;
            }
            return product;
        }
    }
}

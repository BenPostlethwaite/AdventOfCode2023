using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
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
            linesArray = linesArray.Select(x => x.Replace("\r", "")).ToArray();
            return linesArray;
        }
        static int Part2(ref string[] lines)
        {
            int sum = 0;
            int gameNo = 1;
            foreach (string line in lines)
            {
                string[] parts = line.Split(':')[1].Split(';');
                int reds = 0;
                int greens = 0;
                int blues = 0;

                foreach (string part in parts)
                {


                    string[] colors = part.Split(',');
                    foreach (string color in colors)
                    {
                        List<string> split = color.Split(' ').ToList();
                        split.RemoveAll(x => x == "");
                        string colorName = split[1];
                        int colorValue = int.Parse(split[0]);
                        if (colorName == "red")
                        {
                            if (colorValue > reds)
                            {
                                reds = colorValue;
                            }
                        }
                        else if (colorName == "green")
                        {
                            if (colorValue > greens)
                            {
                                greens = colorValue;
                            }
                        }
                        else if (colorName == "blue")
                        {
                            if (colorValue > blues)
                            {
                                blues = colorValue;
                            }
                        }
                    }
                }
       
                sum += reds * greens * blues;                
                gameNo++;
            }
            return sum;
        }
        static int Part1(ref string[] lines)
        {
            int sum = 0;
            int gameNo = 1;            
            foreach (string line in lines)
            {
                string[] parts = line.Split(':')[1].Split(';');
                int reds = 0;
                int greens = 0;
                int blues = 0;

                foreach (string part in parts)
                {
                    

                    string[] colors = part.Split(',');
                    foreach (string color in colors)
                    {
                        List<string> split = color.Split(' ').ToList();
                        split.RemoveAll(x => x == "");
                        string colorName = split[1];
                        int colorValue = int.Parse(split[0]);
                        if (colorName == "red")
                        {
                            if (colorValue > reds)
                            {
                                reds = colorValue;
                            }
                        }
                        else if (colorName == "green")
                        {
                            if (colorValue > greens)
                            {
                                greens = colorValue;
                            }
                        }
                        else if (colorName == "blue")
                        {
                            if (colorValue > blues)
                            {
                                blues = colorValue;
                            }
                        }                        
                    }
                }

                if (reds <= 12 && greens <= 13 && blues <= 14)
                {
                    sum += gameNo;
                }
                gameNo++;
            }
            return sum;
        }
    }       
}

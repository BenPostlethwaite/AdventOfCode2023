using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace Day3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Process.Start("notepad.exe", "example.txt");
            //Process.Start("notepad.exe", "input.txt");


            string[] example = GetLines("example.txt");
            string[] input = GetLines("input.txt");


            Console.WriteLine("Part 1: " + Part1(input));
            Console.WriteLine("Part 2: " + Part2(input));
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
        static int Part2(string[] lines)
        {
            char[,] schematic = GetSchematic(lines);
            List<Number> numbers = new List<Number>();
            for (int y = 0; y < schematic.GetLength(1); y++)
            {
                for (int x = 0; x < schematic.GetLength(0); x++)
                {
                    if (char.IsDigit(schematic[x, y]))
                    {
                        string number = "";
                        while (char.IsDigit(schematic[x, y]))
                        {
                            number += schematic[x, y];
                            x++;
                            if (x >= schematic.GetLength(0))
                            {
                                break;
                            }
                        }
                        Number n = new Number(x - number.Length, y, number);       
                        numbers.Add(n);
                    }
                }                
            }


            int sum = 0;
            for (int y = 0; y < schematic.GetLength(1); y++)
            {
                for (int x = 0; x < schematic.GetLength(0); x++)
                {
                    char c = schematic[x, y];
                    if (c == '*')
                    {
                        List<Number> connectedNumbers = new List<Number>();
                        foreach (Number n in numbers)
                        {
                            if (n.xCoords.Contains(x+1) && n.yCoord == y)
                            {
                                connectedNumbers.Add(n);
                            }
                            else if (n.xCoords.Contains(x+1) && n.yCoord == y+1)
                            {
                                connectedNumbers.Add(n);
                            }
                            else if (n.xCoords.Contains(x+1) && n.yCoord == y-1)
                            {
                                connectedNumbers.Add(n);
                            }
                            else if (n.xCoords.Contains(x-1) && n.yCoord == y)
                            {
                                connectedNumbers.Add(n);
                            }
                            else if (n.xCoords.Contains(x-1) && n.yCoord == y+1)
                            {
                                connectedNumbers.Add(n);
                            }
                            else if (n.xCoords.Contains(x-1) && n.yCoord == y-1)
                            {
                                connectedNumbers.Add(n);
                            }
                            else if (n.xCoords.Contains(x) && n.yCoord == y+1)
                            {
                                connectedNumbers.Add(n);
                            }
                            else if (n.xCoords.Contains(x) && n.yCoord == y-1)
                            {
                                connectedNumbers.Add(n);
                            }
                        }
                        if (connectedNumbers.Count == 2)
                        {
                            sum += connectedNumbers[0].value * connectedNumbers[1].value;
                        }
                    }
                }
            }    
            return sum;
        }
        static int Part1(string[] lines)
        {
            char[,] schematic = GetSchematic(lines);


            int sum = 0;
            for (int y = 0; y < schematic.GetLength(1); y++)
            {
                for (int x = 0; x < schematic.GetLength(0); x++)
                {
                    string number = "";                    
                    while (char.IsDigit(schematic[x, y]))
                    {      
                        number += schematic[x, y];
                        x++;
                        if (x >= schematic.GetLength(0))
                        {
                            break;
                        }
                    }
                    if (number != "")
                    {
                        Number n = new Number(x - number.Length, y, number);
                        
                        if (n.IsPart(schematic))
                        {
                            sum += n.value;
                        }
                    }
                }
            }

            return sum;
        }
        static char[,] GetSchematic(string[] lines)
        {
            char[,] schematic = new char[lines[0].Length, lines.Length];
            for (int x = 0; x < lines[0].Length; x++)
            {
                for (int y = 0; y < lines.Length; y++)
                {
                    schematic[x, y] = lines[y][x];
                }
            }
            return schematic;
        }
    }    
    class Number
    {
        public List<int> xCoords;
        public int yCoord;
        public int value;
        int length { get => value.ToString().Length; }
        public Number(int x, int y, string value)
        {
            this.xCoords = new List<int>();
            for (int i = 0; i < value.Length; i++)
            {
                xCoords.Add(x + i);
            }
            this.yCoord = y;
            this.value = int.Parse(value);
        }

        public bool IsPart(char[,] schematic)
        {
            bool onLeftEdge = xCoords[0] == 0;
            bool onRightEdge = xCoords[0] + (length - 1) == schematic.GetLength(0) - 1;
            bool onTopEdge = yCoord == 0;
            bool onBottomEdge = yCoord == schematic.GetLength(1) - 1;

            if (!onTopEdge)
            {
                foreach (int x in xCoords)
                {
                    char c = schematic[x, yCoord - 1];
                    if (!char.IsDigit(c) && c != '.')
                    {
                        return true;
                    }
                }
            }
            if (!onBottomEdge)
            {
                foreach (int x in xCoords)
                {
                    char c = schematic[x, yCoord + 1];
                    if (!char.IsDigit(c) && c != '.')
                    {
                        return true;
                    }
                }
            }
            if (!onLeftEdge)
            {
                char c = schematic[xCoords[0] - 1, yCoord];
                if (!char.IsDigit(c) && c != '.')
                {
                    return true;
                }
            }
            if (!onRightEdge)
            {
                char c = schematic[xCoords[length-1]+1, yCoord];
                if (!char.IsDigit(c) && c != '.')
                {
                    return true;
                }
            }
            if (!onLeftEdge && !onTopEdge)
            {
                char c = schematic[xCoords[0] - 1, yCoord - 1];
                if (!char.IsDigit(c) && c != '.')
                {
                    return true;
                }
            }
            if (!onRightEdge && !onTopEdge)
            {
                char c = schematic[xCoords[length - 1]+1, yCoord - 1];
                if (!char.IsDigit(c) && c != '.')
                {
                    return true;
                }
            }
            if (!onLeftEdge && !onBottomEdge)
            {
                char c = schematic[xCoords[0]-1, yCoord + 1];
                if (!char.IsDigit(c) && c != '.')
                {
                    return true;
                }
            }
            if (!onRightEdge && !onBottomEdge)
            {
                char c = schematic[xCoords[length-1]+1, yCoord + 1];
                if (!char.IsDigit(c) && c != '.')
                {
                    return true;
                }
            }

            return false;
        }        
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace Day13
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
        static List<char[,]> GetParts(string[] lines)
        {
            List<char[,]> parts = new List<char[,]>();
            List<string> newPartList = new List<string>();
            int start = 0;
            char[,] newPart;
            int i;
            for (i = 0; i <= lines.Length; i++)
            {
                if (i == lines.Length || lines[i] == "")
                {
                    newPart = new char[lines[start].Length, i - start];
                    for (int x = 0; x < newPart.GetLength(0); x++)
                    {
                        for (int y = 0; y < newPart.GetLength(1); y++)
                        {
                            newPart[x, y] = newPartList[y][x];
                        }
                    }
                    parts.Add(newPart);
                    start = i + 1;
                    newPartList = new List<string>();
                }
                else
                {
                    newPartList.Add(lines[i]);
                }
            }
            return parts;
        }
        static ulong Part2(ref string[] lines)
        {
            List<char[,]> parts = GetParts(lines);
            ulong sum = 0;

            for (int p = 0; p < parts.Count; p++)
            {
                char[,] part = parts[p];
                List<Smudge> horisontalSmudges = GetHorisontalSmudge(part);
                List<Smudge> verticalSmudges = GetVerticalSmudge(part);

                //List<int> reflectedColumns = ReflectedHorisontal(part);
                //List<int> reflectedRows = ReflectedVertical(part);

                //for (int s = 0; s < horisontalSmudges.Count; s++)
                //{
                //    Smudge smudge = horisontalSmudges[s];
                //    for (int r = 0; r < reflectedRows.Count; r++)
                //    {
                //        if (smudge.reflectionLine == reflectedRows[r])
                //        {
                //            horisontalSmudges.RemoveAt(s);
                //        }
                //    }
                //}
                //for (int s = 0; s < verticalSmudges.Count; s++)
                //{
                //    Smudge smudge = verticalSmudges[s];
                //    for (int r = 0; r < reflectedColumns.Count; r++)
                //    {
                //        if (smudge.reflectionLine == reflectedColumns[r])
                //        {
                //            verticalSmudges.RemoveAt(s);
                //        }
                //    }
                //}
                
                if (horisontalSmudges.Count + verticalSmudges.Count != 1)
                {
                    throw new Exception("Not 1 smudge");
                }   
                if (horisontalSmudges.Count == 1)
                {
                    sum += (ulong)horisontalSmudges[0].reflectionLine;
                }
                else
                {
                    sum += 100 * (ulong)verticalSmudges[0].reflectionLine;
                }
            }

            return sum;
        }
        static ulong Part1(ref string[] lines)
        {
            List<char[,]> parts = GetParts(lines);

            ulong sum = 0;
            for (int p = 0; p < parts.Count; p++)
            { 
                char[,] part = parts[p];                    
                //PrintPart(part);

                
                List<int> reflectedColumns = ReflectedHorisontal(part);
                if (reflectedColumns.Count > 1)
                {
                    throw new Exception("Not 1 reflection");
                }
                sum += (ulong)reflectedColumns.Sum();   
                

                List<int> reflectedRows = ReflectedVertical(part);
                if (reflectedRows.Count > 1)
                {
                    throw new Exception("Not 1 reflection");
                }
                sum += 100 * (ulong)reflectedRows.Sum();
                

            }
            return sum;
        }

        static void PrintPart(char[,] input)
        {
            for (int y = 0; y < input.GetLength(1); y++)
            {
                for (int x = 0; x < input.GetLength(0); x++)
                {
                    Console.Write(input[x, y]);
                }
                Console.WriteLine();
            }
        }
        
        static List<int> ReflectedHorisontal(char[,] part)
        {
            List<int> reflectedColumns = new List<int>();

            for (int x = 1; x < part.GetLength(0); x++)
            {
                int minLength = Math.Min(x, part.GetLength(0) - x);
                bool isReflected = true;
                
                
                for (int i = 0; i < minLength; i++)
                {                    
                    for (int y = 0; y < part.GetLength(1); y++)
                    {
                        char leftPart = part[x - i - 1, y];
                        char rightPart = part[x + i, y];
                        if (leftPart != rightPart)
                        {
                            isReflected = false;
                            break;
                        }
                    }                    
                    if (!isReflected)
                    {
                        break;
                    }
                }
                if (isReflected)
                {
                    reflectedColumns.Add(x);
                }
            }
            return reflectedColumns;
        }
        static List<int> ReflectedVertical(char[,] part)
        {
            List<int> reflectedColumns = new List<int>();

            for (int y = 1; y < part.GetLength(1); y++)
            {
                int minLength = Math.Min(y, part.GetLength(1) - y);
                bool isReflected = true;

                for (int i = 0; i < minLength; i++)
                {
                    for (int x = 0; x < part.GetLength(0); x++)
                    {
                        char topPart = part[x, y - i - 1];
                        char bottomPart = part[x, y + i];
                        if (topPart != bottomPart)
                        {
                            isReflected = false;
                            break;
                        }
                    }
                    if (!isReflected)
                    {
                        break;
                    }
                }
                if (isReflected)
                {
                    reflectedColumns.Add(y);
                }
            }
            return reflectedColumns;
        }

        static List<Smudge> GetHorisontalSmudge(char[,] part)
        {
            List<Smudge> smudges = new List<Smudge>();
            for (int x = 1; x < part.GetLength(0); x++)
            {
                int minLength = Math.Min(x, part.GetLength(0) - x);

                List<(Coord, Coord)> wrongCoords = new List<(Coord, Coord)>();
                for (int i = 0; i < minLength; i++)
                {
                    for (int y = 0; y < part.GetLength(1); y++)
                    {
                        char leftPart = part[x - i - 1, y];
                        char rightPart = part[x + i, y];
                        if (leftPart != rightPart)
                        {
                            wrongCoords.Add((new Coord(x - i - 1, y), new Coord(x + i, y)));
                            if (wrongCoords.Count > 1)
                            {
                                break;
                            }
                        }                        
                    }
                    if (wrongCoords.Count > 1)
                    {
                        break;
                    }
                }
                if (wrongCoords.Count == 1)
                {
                    smudges.Add(new Smudge(wrongCoords[0], x, true));
                }
            }
            return smudges;
        }
        static List<Smudge> GetVerticalSmudge(char[,] part)
        {
            List<Smudge> smudges = new List<Smudge>();
            for (int y = 1; y < part.GetLength(1); y++)
            {
                int minLength = Math.Min(y, part.GetLength(1) - y);

                List<(Coord, Coord)> wrongCoords = new List<(Coord, Coord)>();
                for (int i = 0; i < minLength; i++)
                {
                    for (int x = 0; x < part.GetLength(0); x++)
                    {
                        char topPart = part[x, y - i - 1];
                        char bottomPart = part[x, y + i];
                        if (topPart != bottomPart)
                        {
                            wrongCoords.Add((new Coord(x, y - i - 1), new Coord(x, y + i)));
                        }
                    }
                }
                if (wrongCoords.Count == 1)
                {
                    smudges.Add(new Smudge(wrongCoords[0], y, false));
                }
            }
            return smudges;
        }
    }
}

public class Coord
{
    public int x;
    public int y;
    public Coord(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
    public override string ToString()
    {
        return "(" + x + "," + y + ")";
    }
    public override int GetHashCode()
    {
        return ToString().GetHashCode();
    }

}
public class Smudge
{
    public (Coord, Coord) pair;
    public int reflectionLine;
    bool isHorisontal;
    public Smudge((Coord, Coord) pair, int reflectionLine, bool isHorisontal)
    {
        this.pair = pair;
        this.reflectionLine = reflectionLine;
        this.isHorisontal = isHorisontal;
    }
    public override string ToString()
    {
        string toReturn = pair.Item1.ToString() + "," + pair.Item2.ToString();
        toReturn += " " + reflectionLine;
        return toReturn;
    }
    public override int GetHashCode()
    {
        return ToString().GetHashCode();
    }
}

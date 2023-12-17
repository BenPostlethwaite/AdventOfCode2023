using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace Day16
{
    internal class Program
    {
        public static Dictionary<(int x, int y, int bearing), HashSet<(int x, int y)>> visitedPositions 
            = new Dictionary<(int x, int y, int bearing), HashSet<(int x, int y)>>();
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
        static int Part2(ref string[] lines)
        {
            char[,] grid = GetGrid(lines);
            int best = 0;
            for (int x = 0; x < grid.GetLength(0); x++)
            {
                int count = GetEnergisedCount(x, -1, 2, grid);
                if (count > best)
                {
                    best = count;
                }
            }
            for (int x = 0; x < grid.GetLength(0); x++)
            {
                int count = GetEnergisedCount(x, grid.GetLength(1), 0, grid);
                if (count > best)
                {
                    best = count;
                }
            }


            for (int y = 0; y < grid.GetLength(1); y++)
            {
                int count = GetEnergisedCount(-1, y, 1, grid);
                if (count > best)
                {
                    best = count;
                }
            }

            for (int y = 0; y < grid.GetLength(1); y++)
            {
                int count = GetEnergisedCount(grid.GetLength(0), y, 3, grid);
                if (count > best)
                {
                    best = count;
                }
            }
            return best;
        }
        static int Part1(ref string[] lines)
        {
            char[,] grid = GetGrid(lines);
            return GetEnergisedCount(-1, 0, 1, grid);
        }
        static int GetEnergisedCount(int x, int y, int bearing, char[,] grid)
        {

            List<Beam> startBeams = new List<Beam>();

            HashSet<(int x, int y)> visitedCoords = new HashSet<(int x, int y)>();
            List<(int x, int y, int bearing)> seemStates = new List<(int x, int y, int bearing)>();

            List<(int x, int y)> startCoords = new List<(int x, int y)>();

            startCoords.Add((x,y));

            Beam startBeam = new Beam(bearing, startCoords);
            startBeams.Add(startBeam);

            Queue<Beam> queue = new Queue<Beam>();
            queue.Enqueue(startBeam);

            while (queue.Count() != 0)
            {
                Beam beam = queue.Dequeue();
                List<Beam> beams = beam.Iterate(grid);
                foreach (Beam b in beams)
                {
                    if (!seemStates.Contains(b.GetState()))
                    {
                        seemStates.Add(b.GetState());
                        visitedCoords.Add(b.coords.Last());
                        queue.Enqueue(b);
                    }
                }
            }

            return visitedCoords.Count;
        }
        static char[,] GetGrid(string[] lines)
        {
            char[,] grid = new char[lines[0].Length, lines.Length];
            for (int y = 0; y < lines.Length; y++)
            {
                for (int x = 0; x < lines.Length; x++)
                {
                    grid[x, y] = lines[y][x];
                }
            }
            return grid;
        }
    }
}

public class Beam
{
    public List<(int x, int y)> coords = new List<(int x, int y)>();

    public int bearing;

    public Beam(int bearing, List<(int x, int y)> coords)
    {
        this.bearing = bearing; // clockwise from north
        this.coords = coords;
    }
    public (int x, int y, int bearing) GetState()
    {
        return (coords.Last().x, coords.Last().y, bearing);
    }

    public bool InGrid((int x, int y) coord, char[,] grid)
    {
        if (coord.x < 0 || coord.y < 0) return false;
        if (coord.x >= grid.GetLength(0)) return false;
        if (coord.y >= grid.GetLength(1)) return false;

        return true;
    }
    public List<Beam> Iterate(char[,] grid)
    {
        (int x, int y) newCoord = (coords.Last().x, coords.Last().y);
        switch (bearing)
        {
            case 0:
                newCoord.y--;
                break;
            case 1:
                newCoord.x++;
                break;
            case 2:
                newCoord.y++;
                break;
            case 3:
                newCoord.x--;
                break;
        }

        if (!InGrid(newCoord, grid))
        {
            return new List<Beam>() {};
        }
        
        coords.Add(newCoord);

        
        
        switch (grid[newCoord.x, newCoord.y])
        {
            case '/':
                switch (bearing)
                {
                    case 0:
                        bearing = 1; break;
                    case 1:
                        bearing = 0; break;
                    case 2:
                        bearing = 3; break;
                    case 3:
                        bearing = 2; break;
                    default:
                        throw new Exception("invalid bearing");
                }
                return new List<Beam>() { this };
                break;

            case '\\':
                switch (bearing)
                {
                    case 0:
                        bearing = 3; break;
                    case 1:
                        bearing = 2; break;
                    case 2:
                        bearing = 1; break;
                    case 3:
                        bearing = 0; break;
                    default:
                        throw new Exception("invalid bearing");
                }
                break;
            case '-':
                if (bearing == 0 || bearing == 2)
                {
                    Beam beam1 = new Beam(1, new List<(int x, int y)>(coords));
                    Beam beam2 = new Beam(3, new List<(int x, int y)>(coords));
                    return new List<Beam>() { beam1, beam2 };
                }
                break;

            case '|':                
                if (bearing == 1 || bearing == 3)
                {
                    Beam beam1 = new Beam(0, new List<(int x, int y)>(coords));
                    Beam beam2 = new Beam(2, new List<(int x, int y)>(coords));
                    return new List<Beam>() { beam1, beam2 };
                }
                break;
                


        }
        return new List<Beam> { this };
    }
}



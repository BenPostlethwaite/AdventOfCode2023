using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace Day14
{
    internal class Program
    {
        public static Dictionary<CharGrid, int> states = new Dictionary<CharGrid, int>();
        public static Dictionary<CharGrid, int> loops = new Dictionary<CharGrid, int>();
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
        static CharGrid GetGrid(string[] lines)
        {
            char[,] grid = new char[lines[0].Length, lines.Length];
            for (int y = 0; y < lines.Length; y++)
            {
                for (int x = 0; x < lines[0].Length; x++)
                {
                    grid[x, y] = lines[y][x];
                }
            }
            return new CharGrid(grid);
        }
        static void PrintGrid(CharGrid grid)
        {
            Console.WriteLine(grid.ToString());
        }        
        static long Part2(ref string[] lines)
        {
            CharGrid grid = GetGrid(lines);
            PrintGrid(grid);
            long numLoops = 1000000000;

            for (int i = 0; i < numLoops; i++)
            {                
                if (states.ContainsKey(grid))
                {
                    long simmilarIndex = states[grid];
                    long loopLength = i - simmilarIndex;
                    long loopsLeft = numLoops - i;
                    long loopsToSkip = loopsLeft / loopLength;
                    i += (int)(loopsToSkip * loopLength);
                    if (i >= numLoops)
                    {
                        break;
                    }
                }
                else
                {
                    states.Add(grid, i);
                }

                TiltUp(ref grid);
                //PrintGrid(grid);

                TiltLeft(ref grid);
                //PrintGrid(grid);

                TiltDown(ref grid);
                //PrintGrid(grid);

                TiltRight(ref grid);
                //PrintGrid(grid);
                
            }
            PrintGrid(grid);
            return grid.GetLoad();
        }
        static ulong Part1(ref string[] lines)
        {
            CharGrid grid = GetGrid(lines);
            //PrintGrid(grid);
            ulong sum = 0;
            for (int x = 0; x < grid.GetLength(0); x++)
            {                                                             
                int start = 0;
                int y = 0;
                bool endHit = false;
                while (!endHit)
                {
                    bool chainComplete = false;
                    int count = 0;
                    while (!chainComplete)
                    {
                        if (y == grid.GetLength(1))
                        {
                            chainComplete = true;
                            endHit = true;
                            continue;
                        }
                        switch (grid[x, y])
                        {
                            case ('O'):
                                grid[x, y] = '.';
                                count++;
                                break;
                            case ('.'):
                                break;
                            case ('#'):
                                chainComplete = true;
                                break;
                            default:
                                throw new Exception("Invalid character");
                                break;
                        }
                        y++;
                    }
                    for (int i = start; i < start + count; i++)
                    {
                        grid[x, i] = 'O';
                        sum += (ulong)grid.GetLength(1) - (ulong)i;
                    }
                    start = y;
                }
            }            
            //PrintGrid(grid);
            return sum;
        }

        static void TiltUp(ref CharGrid grid)
        {
            for (int x = 0; x < grid.GetLength(0); x++)
            {
                int start = 0;
                int y = 0;
                bool endHit = false;
                while (!endHit)
                {
                    bool chainComplete = false;
                    int count = 0;
                    while (!chainComplete)
                    {
                        if (y == grid.GetLength(1))
                        {
                            chainComplete = true;
                            endHit = true;
                            continue;
                        }
                        switch (grid[x, y])
                        {
                            case ('O'):
                                grid[x, y] = '.';
                                count++;
                                break;
                            case ('.'):
                                break;
                            case ('#'):
                                chainComplete = true;
                                break;
                            default:
                                throw new Exception("Invalid character");
                                break;
                        }
                        y++;
                    }
                    for (int i = start; i < start + count; i++)
                    {
                        grid[x, i] = 'O';
                    }
                    start = y;
                }
            }            
        }
        static void TiltDown(ref CharGrid grid)
        {
            for (int x = 0; x < grid.GetLength(0); x++)
            {
                int start = grid.GetLength(1)-1;
                int y = grid.GetLength(1)-1;
                bool endHit = false;
                while (!endHit)
                {
                    bool chainComplete = false;
                    int count = 0;
                    while (!chainComplete)
                    {
                        if (y == -1)
                        {
                            chainComplete = true;
                            endHit = true;
                            continue;
                        }
                        switch (grid[x, y])
                        {
                            case ('O'):
                                grid[x, y] = '.';
                                count++;
                                break;
                            case ('.'):
                                break;
                            case ('#'):
                                chainComplete = true;
                                break;
                            default:
                                throw new Exception("Invalid character");
                                break;
                        }
                        y--;
                    }
                    for (int i = start; i > start - count; i--)
                    {
                        grid[x, i] = 'O';
                    }
                    start = y;
                }
            }
        }
        static void TiltLeft(ref CharGrid grid)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                int start = 0;
                int x = 0;
                bool endHit = false;
                while (!endHit)
                {
                    bool chainComplete = false;
                    int count = 0;
                    while (!chainComplete)
                    {
                        if (x == grid.GetLength(0))
                        {
                            chainComplete = true;
                            endHit = true;
                            continue;
                        }
                        switch (grid[x, y])
                        {
                            case ('O'):
                                grid[x,y] = '.';
                                count++;
                                break;
                            case ('.'):
                                break;
                            case ('#'):
                                chainComplete = true;
                                break;
                            default:
                                throw new Exception("Invalid character");
                                break;
                        }
                        x++;
                    }
                    for (int i = start; i < start + count; i++)
                    {
                        grid[i,y] = 'O';
                    }
                    start = x;
                }
            }
        }
        static void TiltRight(ref CharGrid grid)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                int start = grid.GetLength(0)-1;
                int x = grid.GetLength(0)-1;
                bool endHit = false;
                while (!endHit)
                {
                    bool chainComplete = false;
                    int count = 0;
                    while (!chainComplete)
                    {
                        if (x == -1)
                        {
                            chainComplete = true;
                            endHit = true;
                            continue;
                        }
                        switch (grid[x, y])
                        {
                            case ('O'):
                                grid[x, y] = '.';
                                count++;
                                break;
                            case ('.'):
                                break;
                            case ('#'):
                                chainComplete = true;
                                break;
                            default:
                                throw new Exception("Invalid character");
                                break;
                        }
                        x--;
                    }
                    for (int i = start; i > start - count; i--)
                    {
                        grid[i, y] = 'O';
                    }
                    start = x;
                }
            }
        }
    }
}
public class CharGrid
{
    char[,] grid;
    public int GetLength(int dimension)
    {
        return grid.GetLength(dimension);
    }
    public char this[int x, int y]
    {
        get
        {
            return grid[x, y];
        }
        set
        {
            grid[x, y] = value;
        }
    }
    public CharGrid(char[,] grid)
    {
        this.grid = grid;
    }
    public override string ToString()
    {
        string toReturn = "";
        for (int y = 0; y < grid.GetLength(0); y++)
        {
            for (int x = 0; x < grid.GetLength(1); x++)
            {
                toReturn += grid[x, y];
            }
            toReturn += "\n";
        }
        return toReturn;
    }
    public override int GetHashCode()
    {
        return ToString().GetHashCode();
    }
    public override bool Equals(object obj)
    {
        return obj.ToString() == ToString();
    }
    public long GetLoad()
    {
        long sum = 0;
        for (int y = 0; y < GetLength(1); y++)
        {
            for (int x = 0; x < GetLength(0); x++)
            {
                if (grid[x, y] == 'O')
                {
                    sum += GetLength(1) - y;
                }
            }
        }
        return sum;
    }
}

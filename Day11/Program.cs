using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace Day11
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Process.Start("notepad.exe", "example.txt");
            //Process.Start("notepad.exe", "input.txt");


            string[] example = GetLines("example.txt");
            string[] input = GetLines("input.txt");


            Console.WriteLine("Part 1: " + Part1(ref input ));
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
        static ulong Part2(ref string[] linesArray)
        {
            List<string> lines = new List<string>(linesArray);
            List<long> emptyCollumns = new List<long>();
            List<long> emptyRows = new List<long>();

            for (int x = 0; x < lines[0].Length; x++)
            {
                bool collumnEmpty = true;
                for (int y = 0; y < lines.Count; y++)
                {
                    if (lines[y][x] == '#')
                    {
                        collumnEmpty = false;
                        break;
                    }
                }
                if (collumnEmpty)
                {
                    emptyCollumns.Add(x);
                }
            }
            for (int y = 0; y < lines.Count; y++)
            {
                bool rowEmpty = true;
                for (int x = 0; x < lines[0].Length; x++)
                {
                    if (lines[y][x] == '#')
                    {
                        rowEmpty = false;
                        break;
                    }
                }
                if (rowEmpty)
                {
                    emptyRows.Add(y);
                }
            }

            List<Node> nodes = new List<Node>();
            char[,] chars = new char[lines[0].Length, lines.Count];
            for (int y = 0; y < lines.Count; y++)
            {
                for (int x = 0; x < lines[0].Length; x++)
                {
                    chars[x, y] = lines[y][x];
                    if (lines[y][x] == '#')
                    {
                        nodes.Add(new Node(x, y));
                    }
                }
            }

            ulong sum = 0;
            const ulong expandedSize = 1000000;

            List<Pair> pairs = new List<Pair>();
            for (int i = 0; i < nodes.Count; i++)
            {
                for (int j = i + 1; j < nodes.Count; j++)
                {
                    Pair pair = new Pair(nodes[i], nodes[j]);

                    long minX = Math.Min(pair.a.x, pair.b.x);
                    long maxX = Math.Max(pair.a.x, pair.b.x);
                    long minY = Math.Min(pair.a.y, pair.b.y);
                    long maxY = Math.Max(pair.a.y, pair.b.y);
                    ulong expandedRowsCrossed = (ulong)emptyRows.Where(row => row >= minY && row <= maxY).Count();
                    ulong expandedCollumnsCrossed = (ulong)emptyCollumns.Where(collumn => collumn >= minX && collumn <= maxX).Count();
                    
                    sum += pair.Distance() + (expandedSize-1) * (expandedRowsCrossed + expandedCollumnsCrossed);

                }
            }

            return sum;
            //not 82000210
        }
        static ulong Part1(ref string[] linesArray)
        {            
            List<string> lines = new List<string>(linesArray);
            List<int> emptyCollumns = new List<int>();
            List<int> emptyRows = new List<int>();

            for (int x = 0; x < lines[0].Length; x++)
            {
                bool collumnEmpty = true;
                for (int y = 0; y < lines.Count; y++)
                {
                    if (lines[y][x] == '#')
                    {
                        collumnEmpty = false;
                        break;
                    }
                }
                if (collumnEmpty)
                {
                    emptyCollumns.Add(x);
                }
            }
            for (int y = 0; y < lines.Count; y++)
            {
                bool rowEmpty = true;
                for (int x = 0; x < lines[0].Length; x++)
                {
                    if (lines[y][x] == '#')
                    {
                        rowEmpty = false;
                        break;
                    }
                }
                if (rowEmpty)
                {
                    emptyRows.Add(y);
                }
            }
            
            
            
            for (int y = lines.Count - 1; y >= 0; y--)
            {
                bool rowEmpty = emptyRows.Contains(y);

                for (int x = lines[0].Length - 1; x >= 0; x--)
                {
                    bool collumnEmpty = emptyCollumns.Contains(x);
                    if (collumnEmpty)
                    {
                        lines[y] = lines[y].Insert(x, ".");
                    }
                }
                if (rowEmpty)
                {
                    lines.Insert(y, new string('.', lines[y].Length));
                }
            }

            List<Node> nodes = new List<Node>();
            char[,] chars = new char[lines[0].Length, lines.Count];
            for (int y = 0; y < lines.Count; y++)
            {
                for (int x = 0; x < lines[0].Length; x++)
                {
                    chars[x, y] = lines[y][x];
                    if (lines[y][x] == '#')
                    {
                        nodes.Add(new Node(x,y));
                    }
                }
            }

            ulong sum = 0;
            List<Pair> pairs = new List<Pair>();
            for (int i = 0; i < nodes.Count; i++)
            {
                for (int j = i+1; j < nodes.Count; j++)
                {
                    Pair pair = new Pair(nodes[i], nodes[j]);
                    pairs.Add(pair);
                    sum += pair.Distance();
                }
            }

            return sum;
        }
    }
    struct Node
    {
        public long x;
        public long y;
        public Node(long x, long y)
        {
            this.x = x;
            this.y = y;
        }
    }
    struct Pair
    {
        public Node a;
        public Node b;
        public Pair(Node a, Node b)
        {
            this.a = a;
            this.b = b;
        }
        public ulong Distance()
        {
            return (ulong)(Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y));
        }
    }
}

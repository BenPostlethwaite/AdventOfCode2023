using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Day12
{
    internal class Program
    {
        public static Dictionary<(int hashCount, string sizes, string line), ulong> dict 
            = new Dictionary<(int hashCount, string sizes, string line), ulong>();

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
            ulong sum = 0;
            foreach (string line in lines)
            {
                string[] split = line.Split(' ');

                string part1 = Unfold(split[0]) + ".";
                string part2 = split[1];

                List<int> sizes = part2.Split(',').Select(x => int.Parse(x)).ToList();
                //sizes = sizes repeated 5 times
                List<int> newSizes = new List<int>();
                for (int i = 0; i < 5; i++)
                {
                    newSizes.AddRange(sizes);
                }

                State state = new State(part1, 0, newSizes);
                ulong combs = NumberOfCombinations(state);
                sum += combs;
            }
            // not: 33980939331541
            // not: 24712367628654
            return sum;
        }
        static ulong Part1(ref string[] lines)
        {
            ulong sum = 0;
            foreach (string line in  lines)
            {
                string[] split = line.Split(' ');

                string part1 = split[0] + ".";                
                string part2 = split[1];

                List<int> sizes = part2.Split(',').Select(x => int.Parse(x)).ToList();

                State state = new State(part1, 0, sizes);
                ulong combs = NumberOfCombinations(state);
                sum += combs;
            }
            return sum;
        }
        static string Unfold(string input)
        {
            string toReturn = input;
            for (int i = 0; i < 4; i++)
            {
                toReturn += "?" + input;
            }
            return toReturn;
        }
        static ulong NumberOfCombinations(State state)
        {
            
            string line = state.line;
            int hashCount = state.hashCount;
            List<int> sizes = new List<int>(state.sizes);

            if (dict.ContainsKey((hashCount, string.Join(",",sizes), line)))
            {
                return dict[(hashCount, string.Join(",", sizes), line)];
            }

            State newState;

            ulong toReturn;
            if (line.Length == 0)
            {
                if (sizes.Count == 0)
                {
                    toReturn = 1;
                    if (!dict.ContainsKey((hashCount, string.Join(",", sizes), line)))
                    {
                        dict.Add((hashCount, string.Join(",", sizes), line), toReturn);
                    }
                    return toReturn;
                }
                else
                {
                    toReturn = 0;
                    if (!dict.ContainsKey((hashCount, string.Join(",", sizes), line)))
                    {
                        dict.Add((hashCount, string.Join(",", sizes), line), toReturn);
                    }
                    return toReturn;
                }
            }

            char c = line[0];
            switch (c)
            {
                case '.':

                    if (hashCount > 0)
                    {
                        if (hashCount == sizes[0])
                        {
                            sizes = sizes.GetRange(1, sizes.Count - 1);
                            hashCount = 0;
                        }
                        else
                        {
                            toReturn = 0;
                            if (!dict.ContainsKey((hashCount, string.Join(",", sizes), line)))
                            {
                                dict.Add((hashCount, string.Join(",", sizes), line), toReturn);
                            }
                            return toReturn;
                        }
                    }
                    newState = new State(line.Substring(1), 0, sizes);
                    toReturn = NumberOfCombinations(newState);
                    if (!dict.ContainsKey((hashCount, string.Join(",", sizes), line)))
                    {
                        dict.Add((hashCount, string.Join(",", sizes), line), toReturn);
                    }
                    return toReturn;
                    break;

                case '#':
                    if (sizes.Count == 0)
                    {
                        toReturn = 0;
                        if (!dict.ContainsKey((hashCount, string.Join(",", sizes), line)))
                        {
                            dict.Add((hashCount, string.Join(",", sizes), line), toReturn);
                        }
                        return toReturn;
                    }
                    if (hashCount >= sizes[0])
                    {
                        toReturn = 0;
                        if (!dict.ContainsKey((hashCount, string.Join(",", sizes), line)))
                        {
                            dict.Add((hashCount, string.Join(",", sizes), line), toReturn);
                        }
                        return toReturn;
                    }
                    else
                    {
                        newState = new State(line.Substring(1), hashCount + 1, sizes);
                        toReturn = NumberOfCombinations(newState);
                        if (!dict.ContainsKey((hashCount, string.Join(",", sizes), line)))
                        {
                            dict.Add((hashCount, string.Join(",", sizes), line), toReturn);
                        }
                        return toReturn;
                    }
                    break;

                case '?':
                    string pos1 = "#" + line.Substring(1);
                    string pos2 = "." + line.Substring(1);

                    newState = new State(pos1, hashCount, sizes);
                    ulong combs1 = NumberOfCombinations(newState);

                    newState = new State(pos2, hashCount, sizes);
                    ulong combs2 = NumberOfCombinations(newState);

                    toReturn = combs1 + combs2;
                    if (!dict.ContainsKey((hashCount, string.Join(",", sizes), line)))
                    {
                        dict.Add((hashCount, string.Join(",", sizes), line), toReturn);
                    }
                    return toReturn;
                    break;
                        
                default: 
                    throw new Exception();
            }                                  
        }      
    }
    public class State
    {
        public int hashCount;
        public List<int> sizes;
        public string line;

        public override string ToString()
        {
            return line + " " + hashCount + " " + string.Join(",", sizes);
        }
        public int hashId => ToString().GetHashCode();
        public State(string line, int hashCount, List<int> sizes)
        {
            this.line = line;
            this.hashCount = hashCount;
            this.sizes = sizes;
        }
        public override int GetHashCode()
        {
            return hashId;
        }
        public override bool Equals(object obj)
        {
            return hashId == obj.GetHashCode();
        }         
    }    
}

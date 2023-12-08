using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Day8
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Process.Start("notepad.exe", "example.txt");
            //Process.Start("notepad.exe", "input.txt");


            string[] example = GetLines("example.txt");
            string[] input = GetLines("input.txt");
            Stopwatch sw = new Stopwatch();

            sw.Start();
            Console.WriteLine("Part 1: " + Part1(ref input));
            sw.Stop();
            Console.WriteLine("Time for part1: " + sw.ElapsedMilliseconds + "ms\n");

            sw.Reset();
            sw.Start();
            Console.WriteLine("Part 2: " + Part2(ref input));
            sw.Stop();
            Console.WriteLine("Time for part2: " + sw.ElapsedMilliseconds + "ms");
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
        static Dictionary<string, Node> GetNodes(string[] lines)
        {           
            Dictionary<string, Node> nodes = new Dictionary<string, Node>();
            for (int i = 2; i < lines.Length; i++)
            {
                string line = lines[i];
                string[] split = line.Split('=');

                string name = split[0].Trim();
                split[1] = split[1].Trim();
                string stringLeft = split[1].Substring(1, 3);
                string stringRight = split[1].Substring(6, 3);

                Node n = new Node(name, stringLeft, stringRight);
                nodes.Add(name, n);
            }
            foreach (var pair in nodes)
            {
                pair.Value.AddConnections(nodes);
            }
            return nodes;

        }
        static long Part2(ref string[] lines)
        {
            string InstructionLine = lines[0];
            char[] instructions = InstructionLine.ToCharArray();

            Dictionary<string, Node> nodes = GetNodes(lines);
            List<Node> startNodes = new List<Node>();
            foreach (var pair in nodes)
            {
                if (pair.Key[2] == 'A')
                {
                    startNodes.Add(pair.Value);
                }
            }


            Dictionary<Node, Info> nodeInfoDict = new Dictionary<Node, Info>();
            foreach (Node startNode in startNodes)
            {
                Node current = startNode;

                long instructionIndex = 0;
                long count = 0;
                while (current.ToString()[2] != 'Z')
                {
                    char instruction = instructions[instructionIndex];
                    if (instruction == 'L')
                    {
                        current = current.left;
                    }
                    else if (instruction == 'R')
                    {
                        current = current.right;
                    }
                    count++;
                    instructionIndex = (instructionIndex + 1) % instructions.Length;
                }
                Node endNode = current;

                long statIndex = count;
                long loopCount = 0;

                do
                {
                    char instruction = instructions[instructionIndex];
                    if (instruction == 'L')
                    {
                        current = current.left;
                    }
                    else if (instruction == 'R')
                    {
                        current = current.right;
                    }
                    loopCount++;
                    instructionIndex = (instructionIndex + 1) % instructions.Length;


                } while (!current.Equals(endNode));

                Info info = new Info(statIndex, loopCount);
                nodeInfoDict.Add(startNode, info);
            }

            long maxToGetToZ = 0;
            foreach (var pair in nodeInfoDict)
            {
                if (pair.Value.countToGetToZ > maxToGetToZ)
                {
                    maxToGetToZ = pair.Value.countToGetToZ;
                }
            }

            long lowestCommonMultiple = 1;
            foreach (var pair in nodeInfoDict)
            {
                long n = pair.Value.loopSize;
                if (n == 0)
                {
                    n = pair.Value.loopSize;
                }
                lowestCommonMultiple = LCM(lowestCommonMultiple, n);
            }

            long adder = 1;
            for (long n = maxToGetToZ; n < maxToGetToZ + lowestCommonMultiple; n+=adder)
            {
                bool solutionFound = true;
                for (int i = 0; i < nodeInfoDict.Count; i++)
                {
                    var pair = nodeInfoDict.ElementAt(i);
                    Node node = pair.Key;
                    Info info = pair.Value;

                    long value = info.EvaluateAt(n);
                    
                    value = value % info.loopSize;

                    if (value == info.countToGetToZ % info.loopSize)
                    {
                        // at a z
                        if (adder == 1)
                        {
                            adder = info.loopSize;
                        }
                        else if (adder != info.loopSize)
                        {
                            adder = LCM(adder, pair.Value.loopSize);
                        }
                        nodeInfoDict.Remove(node);
                        i--;
                    }
                    else
                    {
                        //not at a z
                        solutionFound = false;
                        break;
                    }
                }
                if (solutionFound)
                {
                    return n;
                }
            }
            throw new Exception("No solution found");
        }

        public static long LCM(long a, long b)
        {
            return (a * b) / GCD(a, b);
        }
        public static long GCD(long a, long b)
        {
            if (a == 0)
                return b;
            return GCD(b % a, a);
        }
        static int Part1(ref string[] lines)
        {
            string InstructionLine = lines[0];
            char[] instructions = InstructionLine.ToCharArray();

            Dictionary<string, Node> nodes = GetNodes(lines);
            Node current = nodes["AAA"];

            int instructionIndex = 0;
            int count = 0;
            while (current.ToString() != "ZZZ")
            {
                char instruction = instructions[instructionIndex];
                if (instruction == 'L')
                {
                    current = current.left;
                }
                else if (instruction == 'R')
                {
                    current = current.right;
                }
                count++;
                instructionIndex++;
                instructionIndex %= instructions.Length;
            }
            return count;
        }
    }
    public class Node
    {
        public Node left;
        public Node right;

        string stringLeft;
        string stringRight;

        public string name;
        public override string ToString()
        {
            return name;
        }
        public override bool Equals(object obj)
        {
            return name == ((Node)obj).name;
        }
        public Node(string name, string stringLeft, string stringRight)
        {
            this.name = name;    
            this.stringLeft = stringLeft;
            this.stringRight = stringRight;
        }    
        public void AddConnections(Dictionary<string, Node> dict)
        {
            left = dict[stringLeft];
            right = dict[stringRight];
        }
    }
    public struct Info
    {
        public long countToGetToZ;
        public long loopSize;
        public Info(long z, long loopSize)
        {
            countToGetToZ = z;
            this.loopSize = loopSize;
        }        
        public long EvaluateAt(long n)
        {
            return n % loopSize;
        }        
    }
    
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace Day4
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
            for (int i = 0; i < linesArray.Length; i++)
            {
                linesArray[i] = linesArray[i].Replace("\r", "");
            }
            return linesArray;
        }
        static int Part2(string[] lines)
        {
            int[] noOfInstancesOfScratchCard = new int[lines.Length];

            for (int i = 0; i < noOfInstancesOfScratchCard.Length; i++)
            {
                noOfInstancesOfScratchCard[i] = 1;
            }

            for (int lineNo = 0; lineNo < lines.Length; lineNo++)
            {
                string line = lines[lineNo];
                int numMatches = CountMatches(lines, lineNo);                                                
                for (int i = lineNo+1; i <= lineNo + numMatches; i++)
                {
                    noOfInstancesOfScratchCard[i] += noOfInstancesOfScratchCard[lineNo];
                }
            }   
            int sum = 0;
            for (int i = 0; i < noOfInstancesOfScratchCard.Length; i++)
            {
                sum += noOfInstancesOfScratchCard[i];
            }
            return sum;
        }
        static int Part1(string[] lines)
        {
            int sum = 0;
            for (int lineNo = 0; lineNo < lines.Length; lineNo++)
            {
                int score = 0;


                string line = lines[lineNo];
                line = line.Split(':')[1];

                string[] leftAndRight = line.Split('|');
                string left = leftAndRight[0];
                string right = leftAndRight[1];

                List<string> winningNumbers = left.Split(' ').ToList();
                winningNumbers.RemoveAll(x => x == "");
                List<string> numbersIHave = right.Split(' ').ToList();
                numbersIHave.RemoveAll(x => x == "");

                foreach (string winningNumber in winningNumbers)
                {
                    foreach (string numberIHave in numbersIHave)
                    {
                        if (winningNumber == numberIHave)
                        {
                            if (score == 0)
                            {
                                score = 1;
                            }
                            else
                            {
                                score *= 2;
                            }
                        }
                    }
                }
                sum += score;
            }
            return sum;
        }

        static int CountMatches(string[] lines, int lineNo)
        {  
            int count = 0;
            string line = lines[lineNo].Split(':')[1];

            string[] leftAndRight = line.Split('|');
            string left = leftAndRight[0];
            string right = leftAndRight[1];

            List<string> winningNumbers = left.Split(' ').ToList();
            winningNumbers.RemoveAll(x => x == "");
            List<string> numbersIHave = right.Split(' ').ToList();
            numbersIHave.RemoveAll(x => x == "");

            foreach (string winningNumber in winningNumbers)
            {
                foreach (string numberIHave in numbersIHave)
                {
                    if (winningNumber == numberIHave)
                    {
                        count++;
                    }
                }
            }

            return count;
        }
    }
}

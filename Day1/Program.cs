using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Day1
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
            for (int i = 0; i < linesArray.Count(); i++)
            {
                linesArray[i] = linesArray[i].Replace("\r", "");
            }
            return linesArray;
        }
        static int Part2(string[] lines)
        {
            //complete the task as described in the comment at the top of the file
            Dictionary<string, int> digits = new Dictionary<string, int>();
            digits.Add("one", 1);
            digits.Add("two", 2);
            digits.Add("three", 3);
            digits.Add("four", 4);
            digits.Add("five", 5);
            digits.Add("six", 6);
            digits.Add("seven", 7);
            digits.Add("eight", 8);
            digits.Add("nine", 9);

            for (int i = 0; i < lines.Count(); i++)
            {
                foreach (KeyValuePair<string, int> digit in digits)
                {
                    lines[i] = lines[i].Replace(digit.Key, digit.Key+digit.Value.ToString()+digit.Key);
                }
            }      
            return Part1(lines);
        }
        static int Part1(string[] lines)
        {
            //complete the task as described in the comment at the top of the file
            int sum = 0;
            foreach (string line in lines)
            {
                List<string> digits = new List<string>();
                for (int i = 0; i < line.Length; i++)
                {
                    if (char.IsDigit(line[i]))
                    {
                        digits.Add(line[i].ToString());
                    }
                }
                sum += int.Parse(digits[0] + digits[digits.Count - 1]);
            }
            return sum;

        }
    }
}

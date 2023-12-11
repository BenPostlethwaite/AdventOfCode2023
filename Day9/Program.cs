using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace Day9
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
        static long Part2(ref string[] lines)
        {
            long sum = 0;
            foreach (string l in lines)
            {
                Line line = new Line(l);                
                line.ExtendBackWards();
                sum += line.numbers[0];
            }
            return sum;
        }
        static long Part1(ref string[] lines)
        {
            long sum = 0;
            foreach (string l in lines)
            {
                Line line = new Line(l);
                line.Extend();
                sum += line.numbers[line.numbers.Count - 1];
            }
            return sum;
        }
    }
    public class Line
    {
        public List<long> numbers;
        public Line(string line)
        {
            numbers = line.Split(' ').Select(long.Parse).ToList();
        }
        public Line(List<long> numbers)
        {
               this.numbers = numbers;
        }

        public void Extend()
        {
            long next = 0;
            Line differences = GetDifferences();
            if (differences.numbers.All(d => d== differences.numbers[0]))
            {
                next = numbers[numbers.Count - 1] + differences.numbers[0];
            }
            else
            {
                differences.Extend();
                next = numbers[numbers.Count - 1] + differences.numbers[differences.numbers.Count-1];
            }
            numbers.Add(next);
        }
        public void ExtendBackWards()
        {
            long next = 0;
            Line differences = GetDifferences();
            if (differences.numbers.All(d => d == differences.numbers[0]))
            {
                next = numbers[0] - differences.numbers[0];
            }
            else
            {
                differences.ExtendBackWards();
                next = numbers[0] - differences.numbers[0];
            }
            numbers.Insert(0, next);
        }
        public Line GetDifferences()
        {
            List<long> differences = new List<long>(numbers.Count - 1);
            for (int i = 0; i < numbers.Count - 1; i++)
            {
                differences.Add(numbers[i + 1] - numbers[i]);
            }
            return new Line(differences);
        }
    }
}

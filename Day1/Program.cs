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
//Something is wrong with global snow production, and you've been selected to take a look. The Elves have even given you a map; on it, they've used stars to mark the top fifty locations that are likely to be having problems.

//You've been doing this long enough to know that to restore snow operations, you need to check all fifty stars by December 25th.

//Collect stars by solving puzzles.Two puzzles will be made available on each day in the Advent calendar; the second puzzle is unlocked when you complete the first.Each puzzle grants one star. Good luck!

//You try to ask why they can't just use a weather machine ("not powerful enough") and where they're even sending you ("the sky") and why your map looks mostly blank("you sure ask a lot of questions") and hang on did you just say the sky("of course, where do you think snow comes from") when you realize that the Elves are already loading you into a trebuchet("please hold still, we need to strap you in").

//As they're making the final adjustments, they discover that their calibration document (your puzzle input) has been amended by a very young Elf who was apparently just excited to show off her art skills. Consequently, the Elves are having trouble reading the values on the document.

//The newly-improved calibration document consists of lines of text; each line originally contained a specific calibration value that the Elves now need to recover.On each line, the calibration value can be found by combining the first digit and the last digit(in that order) to form a single two-digit number.

//For example:

//1abc2
//pqr3stu8vwx
//a1b2c3d4e5f
//treb7uchet
//In this example, the calibration values of these four lines are 12, 38, 15, and 77. Adding these together produces 142.

//Consider your entire calibration document.What is the sum of all of the calibration values?

//Your puzzle answer was 53651.

//--- Part Two ---
//Your calculation isn't quite right. It looks like some of the digits are actually spelled out with letters: one, two, three, four, five, six, seven, eight, and nine also count as valid "digits".

//Equipped with this new information, you now need to find the real first and last digit on each line.For example:

//two1nine
//eightwothree
//abcone2threexyz
//xtwone3four
//4nineeightseven2
//zoneight234
//7pqrstsixteen
//In this example, the calibration values are 29, 83, 13, 24, 42, 14, and 76. Adding these together produces 281.

//What is the sum of all of the calibration values?
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
            foreach (string line in linesArray)
            {
                line.Trim('\r');
            }
            return linesArray;
        }
        static int Part2(ref string[] lines)
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
                    lines[i] = lines[i].Replace(digit.Key, digit.Value.ToString());
                }
            }      
            return Part1(ref lines);
        }
        static int Part1(ref string[] lines)
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

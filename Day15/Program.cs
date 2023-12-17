using System;
using System.Collections.Generic;
using System.Linq;

namespace Day15
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
        static int Part2(ref string[] lines)
        {
            List<(string, int)>[] boxes = new List<(string, int)>[256];
            for (int i = 0; i < 256; i++)
            {
                boxes[i] = new List<(string, int)>();
            }

            int sum = 0;
            foreach (string line in lines[0].Split(','))
            {
                string label = "";
                int focusLength = -1;
                int hashValue = -1;

                if (line.Contains('='))
                {
                    string[] split = line.Split('=');
                    label = split[0];
                    focusLength = int.Parse(split[1]);

                    hashValue = Hash(label);

                    var box = boxes[hashValue];
                    bool replaced = false;
                    for (int i = 0; i < box.Count; i++)
                    {                       
                        if (box[i].Item1 == label)
                        {
                            box[i] = (label, focusLength);
                            replaced = true;
                            break;
                        }
                    }
                    if (!replaced)
                    {
                        boxes[hashValue].Add((label, focusLength));
                    }
                }
                else if (line.Contains('-'))
                {
                    string[] split = line.Split('-');
                    label = split[0];

                    foreach (var box in boxes)
                    {
                        box.RemoveAll(x => x.Item1 == label);
                    }
                }
            }
            for (int b = 0; b < boxes.Count(); b++)
            {
                var box = boxes[b];
                for (int i = 0; i < box.Count; i++)
                {
                    var item = box[i];
                    int focusingPower = b + 1;
                    focusingPower *= (i + 1);
                    focusingPower *= item.Item2;
                    sum += focusingPower;
                }
            }


            return sum;
        }
        static int Part1(ref string[] lines)
        {
            Dictionary<string, int> dict = new Dictionary<string, int>();

            int sum = 0;
            string[] lineSplit = lines[0].Split(',');
            foreach (string part in lineSplit)
            {
                sum += Hash(part);
            }

            return sum;

        }

        static int Hash(string input)
        {
            int currentValue = 0;
            foreach (char c in input)
            {
                int ascii = c;
                currentValue += ascii;
                currentValue *= 17;
                currentValue %= 256;
            }
            return currentValue;
        }
    }
}

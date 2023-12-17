using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace Day5
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
        static Int64 Part2(ref string[] lines)
        {
            Almanac almanac = new Almanac(lines);
            almanac.ParsePart2Seeds(lines);
            List<Range> outputs = almanac.GetAllOutputs(almanac.ranges);
            long min = outputs.Min(x => x.start);
            return min;
        }
        static Int64 Part1(ref string[] lines)
        {
            Almanac almanac = new Almanac(lines);
            almanac.ParsePart1Seeds(lines);
            Int64[] outputs = almanac.GetAllOutputs(almanac.startSeeds);
            return outputs.Min();
        }
    }

    public class Almanac
    {
        public List<Int64> startSeeds = new List<Int64>();
        public List<Range> ranges = new List<Range>();
        public List<Map> maps = new List<Map>();
        public Almanac(string[] input)
        {
            GetMaps(input);
        }
        public void ParsePart1Seeds(string[] input)
        {
            string seedLine = input[0];
            seedLine = seedLine.Split(':')[1].Trim();
            string[] startSeedsString = seedLine.Split(' ');
            startSeeds = startSeedsString.Select(x => Int64.Parse(x)).ToList();
        }
        public void ParsePart2Seeds(string[] input)
        {
            string seedLine = input[0];
            seedLine = seedLine.Split(':')[1].Trim();
            string[] startSeedsString = seedLine.Split(' ');
            Int64[] intInputSeeds = startSeedsString.Select(x => Int64.Parse(x)).ToArray();
            for (Int64 i = 0; i < intInputSeeds.Length; i+=2)
            {
                Range r = new Range(start:intInputSeeds[i], length:intInputSeeds[i + 1]);
                ranges.Add(r);
            }
        }

        private void GetMaps(string[] input)
        {                        
            List<Map> parsers = new List<Map>();

            int i = 3;
            while (i < input.Length)
            {
                List<string> parserLines = new List<string>();
                while (input[i] != "")
                {
                    parserLines.Add(input[i]);
                    i++;
                    if (i >= input.Length) break;
                }
                Map parser = new Map(parserLines);
                this.maps.Add(parser);
                i += 2;
            }                
        }
        public List<Range> GetAllOutputs(List<Range> inputRanges)
        {
            List<Range> outputRanges = inputRanges;
            for (int i = 0; i < maps.Count; i++)
            {
                outputRanges = MapOnce(outputRanges, maps[i]);
            }
            return outputRanges;
        }
        public List<Range> MapOnce(List<Range> inputRanges, Map parser)
        {
            List<Range> outputRanges = new List<Range>();

            while (inputRanges.Count > 0)
            {
                Range range = inputRanges[0];
                bool releventRuleExists = false;
                foreach (Rule rule in parser.rules)
                {
                    bool ruleContainsStart = rule.range.Contains(range.start);
                    bool ruleContainsEnd = rule.range.Contains(range.end);

                    if (!ruleContainsStart && !ruleContainsEnd) continue;
                    else if (ruleContainsStart && ruleContainsEnd)
                    {
                        //the whole range is in the rule
                        long difference = rule.outputStart - rule.inputStart;
                        Range newRange = new Range(start:range.start + difference, end:range.end + difference);
                        inputRanges.RemoveAt(0);
                        outputRanges.Add(newRange);
                        releventRuleExists = true;
                        break;
                    }
                    else if (ruleContainsStart && !ruleContainsEnd)
                    {
                        //range 1 is the part in the rule
                        Range range1 = new Range(start: range.start,end:rule.range.end);
                        // range 2 is part not in the rule
                        Range range2 = new Range(start:rule.range.end + 1, end:range.end);
                        inputRanges.RemoveAt(0);

                        outputRanges.AddRange(MapOnce(new List<Range>() { range1, range2 }, parser));
                        releventRuleExists = true;
                        break;
                    }
                    else if (!ruleContainsStart && ruleContainsEnd)
                    {
                        //range 1 is the part not in the rule
                        Range range1 = new Range(start:range.start, end:rule.range.start - 1);
                        // range 2 is part in the rule
                        Range range2 = new Range(start:rule.range.start,end:range.end);
                        inputRanges.RemoveAt(0);

                        outputRanges.AddRange(MapOnce(new List<Range>() { range1, range2 }, parser));                        
                        releventRuleExists = true;
                        break;
                    }
                    else
                    {
                        throw new Exception("This should never happen");
                    }
                }
                if (!releventRuleExists)
                {
                    outputRanges.Add(range);
                    inputRanges.RemoveAt(0);
                }
            }

            
            return outputRanges;
        }
        public Int64 GetOutput(Int64 input)
        {
            Int64 output = input;
            foreach (Map parser in maps)
            {
                output = parser.DoMap(output);
            }
            return output;
        }
        public Int64[] GetAllOutputs(List<Int64> inputs)
        {
            Int64[] outputs = new Int64[inputs.Count];
            for (int i = 0; i < inputs.Count; i++)
            {
                outputs[i] = GetOutput(inputs[i]);
            }
            return outputs;
        }
        
    }

    public class Map
    {
        public List<Rule> rules = new List<Rule>();
        public Map(List<string> lines)
        {
            for (int i = 0; i < lines.Count; i++)
            {
                rules.Add(new Rule(lines[i]));
            }
        }

        public Int64 DoMap(Int64 input)
        {
            foreach (Rule rule in rules)
            {
                if (input >= rule.inputStart && input < rule.inputStart + rule.length)
                {
                    return input - rule.inputStart + rule.outputStart;
                }
            }
            return input;
        }
    }

    public struct Rule
    {
        public Int64 inputStart;
        public Int64 length;
        public Range range;
        public Int64 outputStart;
        
        public Rule(string line)
        {
            string[] items = line.Split(' ');
            List<Int64> intItems = new List<Int64>();
            for (Int64 i = 0; i < items.Length; i++)
            {
                intItems.Add(Int64.Parse(items[i]));
            }

            inputStart = intItems[1];
            length = intItems[2];
            outputStart = intItems[0];
            range = new Range(start: inputStart, length:length);
        }

        public override string ToString()
        {
            return range.ToString();
        }
    }
    public struct Range
    {
        public Int64 start;
        public Int64 end;
        public override string ToString()
        {
            return start.ToString() + ":" + end.ToString();
        }        

        public Range(Int64 start, Int64 end = -1, Int64 length = -1)
        {
            this.start = start;
            if (end == -1 && length == -1)
            {
                throw new Exception("You must provide either an end or a length");
            }
            if (end == -1)
            {
                this.end = start + length - 1;                
            }
            else if (length == -1)
            {
                this.end = end;
            }
            else
            {
                throw new Exception("You must provide either an end or a length, not both");
            }
        }

        public bool Contains(Int64 value)
        {
            return value >= start && value <= end;
        }

    }
}

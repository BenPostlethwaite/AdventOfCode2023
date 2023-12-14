using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Day10
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
        static char[,] GetCharMatrix(string[] lines)
        {
            char[,] matrix = new char[lines[0].Length+2, lines.Length+2];
            for (int y = 0; y < lines.Length; y++)
            {
                for (int x = 0; x < lines[0].Length; x++)
                {
                    matrix[x+1, y+1] = lines[y][x];
                }
            }
            for (int x = 0; x < matrix.GetLength(0); x++)
            {
                matrix[x, 0] = '.';
                matrix[x, matrix.GetLength(1) - 1] = '.';
            }   
            for (int y = 0; y < matrix.GetLength(1); y++)
            {
                matrix[0, y] = '.';
                matrix[matrix.GetLength(0) - 1, y] = '.';
            }
            return matrix;
        }
        static int[] GetStartPos(char[,] matrix)
        {
            int[] startPos = new int[2];
            for (int x = 0; x < matrix.GetLength(0); x++)
            {
                for (int y = 0; y < matrix.GetLength(1); y++)
                {
                    if (matrix[x, y] == 'S')
                    {
                        startPos[0] = x;
                        startPos[1] = y;
                        return startPos;
                    }
                }
            }
            throw new Exception("No start position found");
        }
        static List<Vector> GetLoopPositions(char[,] matrix)
        {
            int[] startPos = GetStartPos(matrix);

            Dictionary<char, Vector[]> directionDict = new Dictionary<char, Vector[]>();

            Vector[] northToSouth = new Vector[] { new Vector(0, -1), new Vector(0, 1) };
            Vector[] eastToWest = new Vector[] { new Vector(1, 0), new Vector(-1, 0) };
            Vector[] northToEast = new Vector[] { new Vector(0, -1), new Vector(1, 0) };
            Vector[] northToWest = new Vector[] { new Vector(0, -1), new Vector(-1, 0) };
            Vector[] southToEast = new Vector[] { new Vector(0, 1), new Vector(1, 0) };
            Vector[] southToWest = new Vector[] { new Vector(0, 1), new Vector(-1, 0) };

            directionDict.Add('|', northToSouth);
            directionDict.Add('-', eastToWest);
            directionDict.Add('L', northToEast);
            directionDict.Add('J', northToWest);
            directionDict.Add('7', southToWest);
            directionDict.Add('F', southToEast);



            Vector direction = new Vector();
            Vector[] possibleDirections = new Vector[]
            {
                new Vector(0, -1),
                new Vector(0, 1),
                new Vector(1, 0),
                new Vector(-1, 0)
            };

            for (int i = 0; i < 4; i++)
            {
                List<Vector> positions = new List<Vector>();
                Vector currentPos = new Vector(startPos[0], startPos[1]);
                positions.Add(currentPos);

                direction = possibleDirections[i];
                char currentChar;
                while (true)
                {
                    currentPos += direction;

                    if (currentPos.x < 0 || currentPos.x >= matrix.GetLength(0) || currentPos.y < 0 || currentPos.y >= matrix.GetLength(1))
                    {
                        break;
                    }
                    positions.Add(currentPos);
                    currentChar = matrix[currentPos.x, currentPos.y];
                    if (currentChar == 'S')
                    {
                        positions.RemoveAt(positions.Count - 1);
                        return positions;
                    }
                    else if (directionDict.ContainsKey(currentChar))
                    {
                        Vector[] connectedDirectons = directionDict[currentChar];
                        if (new Vector() - connectedDirectons[0] == direction)
                        {
                            direction = connectedDirectons[1];
                        }
                        else if (new Vector() - connectedDirectons[1] == direction)
                        {
                            direction = connectedDirectons[0];
                        }
                        else
                        {
                            break;
                        }
                    }
                    else break;
                }
            }
            return null;
        }
        static int Part2(ref string[] lines)
        {
            char[,] charMatrix = GetCharMatrix(lines);
            List<Vector> loopPositions = GetLoopPositions(charMatrix);
            List<Vector> stuckCharacters = GetStuckPositions(charMatrix, loopPositions);
            VisualiseFoundPositions(charMatrix, stuckCharacters, loopPositions);

            return stuckCharacters.Count;
        }                
        static List<Vector> GetStuckPositions(char[,] grid, List<Vector> loopPositions)
        {
            bool inLoop = false;
            bool? aboveIsInLoop = null;

            List<Vector> stuckCharaters = new List<Vector>();
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                for (int x = 0; x < grid.GetLength(0); x++)
                {
                    char c = grid[x, y];
                    if (loopPositions.Contains(new Vector(x, y)))
                    {
                        switch (c)
                        {
                            case '|':
                                aboveIsInLoop = null;
                                inLoop = !inLoop;
                                break;

                            case '-':
                                break;

                            case 'L':                                
                                if (inLoop)
                                {
                                    aboveIsInLoop = false;
                                }
                                else
                                {
                                    aboveIsInLoop = true;
                                }
                                break;

                            case 'J':
                                if (aboveIsInLoop == null)
                                {
                                    throw new Exception("Not connected");
                                }
                                else if (aboveIsInLoop == true)
                                {
                                    inLoop = false;
                                }
                                else if (aboveIsInLoop == false)
                                {
                                    inLoop = true;
                                }

                                break;

                            case '7':
                                if (aboveIsInLoop == null)
                                {
                                    throw new Exception("Not connected");
                                }
                                else if (aboveIsInLoop == true)
                                {
                                    inLoop = true;
                                }
                                else if (aboveIsInLoop == false)
                                {
                                    inLoop = false;
                                }
                                break;

                            case 'F':
                                if (inLoop)
                                {
                                    aboveIsInLoop = true;
                                }
                                else
                                {
                                    aboveIsInLoop = false;
                                }
                                break;                           
                        }
                    }
                    else
                    {
                        if (inLoop)
                        {
                            stuckCharaters.Add(new Vector(x, y));
                        }
                    }                                        
                }
            }
            return stuckCharaters;
        }
        static int Part1(ref string[] lines)
        {
            char[,] matrix = GetCharMatrix(lines);
            int[] startPos = GetStartPos(matrix);

            Dictionary<char, Vector[]> directionDict = new Dictionary<char, Vector[]>();

            Vector[] northToSouth = new Vector[] { new Vector( 0, -1 ), new Vector( 0, 1 ) };
            Vector[] eastToWest = new Vector[] { new Vector ( 1, 0 ), new Vector ( -1, 0) };
            Vector[] northToEast = new Vector[] { new Vector(0, -1), new Vector(1, 0) };
            Vector[] northToWest = new Vector[] { new Vector ( 0, -1), new Vector ( -1, 0) };
            Vector[] southToEast = new Vector[] { new Vector (0, 1), new Vector(1, 0) };
            Vector[] southToWest = new Vector[] { new Vector ( 0, 1 ), new Vector ( -1, 0) };

            directionDict.Add('|', northToSouth);
            directionDict.Add('-', eastToWest);
            directionDict.Add('L', northToEast);
            directionDict.Add('J', northToWest);
            directionDict.Add('7', southToWest);
            directionDict.Add('F', southToEast);
                


            Vector direction = new Vector();
            Vector[] possibleDirections = new Vector[]
            {
                new Vector(0, -1),
                new Vector(0, 1),
                new Vector(1, 0),
                new Vector(-1, 0)
            };

            for (int i = 0; i < 4; i++)
            {
                int steps = 0;
                Vector currentPos = new Vector(startPos[0], startPos[1]);
                direction = possibleDirections[i];
                char currentChar;
                while (true)
                {
                    currentPos += direction;

                    if (currentPos.x < 0 || currentPos.x >= matrix.GetLength(0) || currentPos.y < 0 || currentPos.y >= matrix.GetLength(1))
                    {
                        break;
                    }
                    steps++;
                    currentChar = matrix[currentPos.x, currentPos.y];
                    if (currentChar == 'S')
                    {
                        return steps / 2;
                    }
                    else if (directionDict.ContainsKey(currentChar))
                    {
                        Vector[] connectedDirectons = directionDict[currentChar];
                        if (new Vector()-connectedDirectons[0] == direction)
                        {
                            direction = connectedDirectons[1];
                        }
                        else if (new Vector() - connectedDirectons[1] == direction)
                        {
                            direction = connectedDirectons[0];
                        }
                        else
                        {
                            break;
                        }                        
                    }
                    else break;                    
                }
            }
            return 0;
        }
        static void VisualiseFoundPositions(char[,] matrix, List<Vector> stuckPositions, List<Vector> positions)
        {
            for (int y = 0; y < matrix.GetLength(1); y++)
            {
                for (int x = 0; x < matrix.GetLength(0); x++)
                {
                    if (matrix[x, y] == 'S')
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                    }
                    else if (stuckPositions.Contains(new Vector(x, y)))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    else if (positions.Contains(new Vector(x,y)))
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    Console.Write(matrix[x, y]);
                }
                Console.WriteLine();
            }    
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
    class Vector
    {
        public int x => vector[0];
        public int y => vector[1];

        public int[] vector = new int[2];
        public override string ToString()
        {
            return ("(" + x + ", " + y + ")");
        }
        public Vector()
        {
            vector[0] = 0;
            vector[1] = 0;
        }
        public Vector(int x, int y)
        {
            vector[0] = x;
            vector[1] = y;
        }
        public override bool Equals(object obj)
        {
            if (obj is Vector)
            {
                Vector v = (Vector)obj;
                return (v.x == x && v.y == y);
            }
            return false;
        }
        public override int GetHashCode()
        {
            return vector.GetHashCode();
        }
        public static Vector operator +(Vector v1, Vector v2)
        {
            return new Vector(v1.x + v2.x, v1.y + v2.y);
        }
        public static Vector operator -(Vector v1, Vector v2)
        {
            return new Vector(v1.x - v2.x, v1.y - v2.y);
        }
        public static bool operator ==(Vector v1, Vector v2)
        {
            return v1.Equals(v2);
        }
        public static bool operator !=(Vector v1, Vector v2)
        {
            return !v1.Equals(v2);
        }
        public int GetAngle()
        {
            if (x == 0 && y == -1)
            {
                return 0;
            }
            else if (x == 0 && y == 1)
            {
                return 180;
            }
            else if (x == 1 && y == 0)
            {
                return 90;
            }
            else if (x == -1 && y == 0)
            {
                return 270;
            }
            else
            {
                throw new Exception("Not a valid direction");
            }
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
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
            Console.WriteLine("Part 2: " + Part2(ref example));
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
        static List<Vector> GetPositions(char[,] matrix)
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
        static List<Vector> BFS(Vector startPos, List<Vector> loopPositions, ref List<Vector> visitedPositions, List<Vector> directions, char[,] charMatrix)
        {
            List<Vector> positionsExplored = new List<Vector>();
            Queue<Vector> queue = new Queue<Vector>();
            queue.Enqueue(startPos);
            positionsExplored.Add(startPos);

            while (queue.Count > 0)
            {
                Vector currentPos = queue.Dequeue();
                foreach (Vector direction in directions)
                {
                    Vector newPos = currentPos + direction;
                    if (newPos.x < 0 || newPos.x >= charMatrix.GetLength(0) || newPos.y < 0 || newPos.y >= charMatrix.GetLength(1))
                    {
                        continue;
                    }
                    else if (!positionsExplored.Contains(newPos) && !loopPositions.Contains(newPos) && !visitedPositions.Contains(newPos))
                    {
                        queue.Enqueue(newPos);
                        positionsExplored.Add(newPos);
                    }
                }
            }
            visitedPositions.AddRange(positionsExplored);
            return positionsExplored;
        }
        static int Part2(ref string[] lines)
        {             
            List<Vector> directions = new List<Vector>();
            directions.Add(new Vector(0, -1));
            directions.Add(new Vector(0, 1));
            directions.Add(new Vector(1, 0));
            directions.Add(new Vector(-1, 0));

            char[,] charMatrix = GetCharMatrix(lines);
            bool clockWiseLoop;

            List<Vector> loopPositions = GetPositions(charMatrix);
            loopPositions.Add(loopPositions[0]);
            loopPositions.Add(loopPositions[1]);

            Vector direction = loopPositions[1] - loopPositions[0];
            int r = 0;
            for (int i = 1; i < loopPositions.Count; i++)
            {
                Vector newDirection = loopPositions[i] - loopPositions[i-1];
                if (newDirection != direction)
                {
                    //angle is clockwise from vertical
                    int oldAngle = direction.GetAngle();
                    int newAngle = newDirection.GetAngle();
                    int angleChange = newAngle - oldAngle;
                    if (angleChange > 180)
                    {
                        angleChange -= 360;
                    }
                    else if (angleChange < -180)
                    {
                        angleChange += 360;
                    }
                    r = r + angleChange;
                    direction = newDirection;
                }
                direction = newDirection;
            }

            loopPositions.RemoveAt(loopPositions.Count-1);
            loopPositions.RemoveAt(loopPositions.Count-1);

            if (r > 0)
            {
                clockWiseLoop = true;
            }
            else
            {
                clockWiseLoop = false;
            }

            Queue<Vector> queue = new Queue<Vector>();
            List<Vector> visitedPositions = new List<Vector>();
            List<Vector> stuckPositions = new List<Vector>();

            List<Vector> possiblyStuck = new List<Vector>();
            for (int y = 0; y < charMatrix.GetLength(1); y++)
            {
                for (int x = 0; x < charMatrix.GetLength(0); x++)
                {
                    possiblyStuck.Add(new Vector(x, y));
                }
            }
            possiblyStuck = possiblyStuck.Where(pos => !loopPositions.Contains(pos)).ToList();

            direction = loopPositions[1] - loopPositions[0];
            for (int i = 1; i < loopPositions.Count; i++)
            {
                direction = loopPositions[i] - loopPositions[i - 1];
                Vector startPos = loopPositions[i];
                switch (direction.GetAngle())
                {
                    case 0:
                        if (clockWiseLoop)
                        {
                            startPos += new Vector(1, 0);
                        }
                        else
                        {
                            startPos += new Vector(-1, 0);
                        }
                        break;
                    case 90:
                        if (clockWiseLoop)
                        {
                            startPos += new Vector(0, 1);
                        }
                        else
                        {
                            startPos += new Vector(0, -1);
                        }
                        break;
                    case 180:
                        if (clockWiseLoop)
                        {
                            startPos += new Vector(-1, 0);
                        }
                        else
                        {
                            startPos += new Vector(1, 0);
                        }
                        break;
                    case 270:
                        if (clockWiseLoop)
                        {
                            startPos += new Vector(0, -1);
                        }
                        else
                        {
                            startPos += new Vector(0, 1);
                        }
                        break;
                    default:
                        throw new Exception("Not a valid direction");                    
                }
                if (possiblyStuck.Contains(startPos))
                {
                    List<Vector> explored = BFS(startPos, loopPositions, ref visitedPositions, directions, charMatrix);
                    stuckPositions.AddRange(explored);    
                    possiblyStuck = possiblyStuck.Where(pos => !explored.Contains(pos)).ToList();
                }                
            }


            int totalCharacters = charMatrix.GetLength(0) * charMatrix.GetLength(1);
            VisualiseFoundPositions(charMatrix, stuckPositions, loopPositions);
            return stuckPositions.Count;
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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace Day7
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
        static int Part2(ref string[] lines)
        {
            return 0;
        }
        static ulong Part1(ref string[] lines)
        {
            List<Hand> hands = new List<Hand>();
            foreach (string line in lines)
            {
                hands.Add(new Hand(line));
            }

            Hand.SetRanks(ref hands);
            ulong sum = 0;
            foreach (Hand hand in hands)
            {
                sum += (ulong)hand.rank * (ulong)hand.bid;
            }
            return sum;
        }
    }

    public class Card : IComparable<Card>
    {
        static Dictionary<char, int> cardsDictionary = new Dictionary<char, int>()
        {
            {'A', 13},
            {'K', 12},
            {'Q', 11},
            {'T', 10},
            {'9', 9},
            {'8', 8},
            {'7', 7},
            {'6', 6},
            {'5', 5},
            {'4', 4},
            {'3', 3},
            {'2', 2},
            {'J', 1 }
        };
        public override string ToString()
        {
            return value.ToString();
        }
        public char value;
        public Card(char value)
        {
            this.value = value;
        }
        public int CompareTo(Card other)
        {
            return cardsDictionary[value].CompareTo(cardsDictionary[other.value]);
        }
        public override bool Equals(object obj)
        {
            return value == ((Card)obj).value;
        }
        public bool EqualsOrJoker(Card card)
        {
            return card.value == 'J' || Equals(card);
        }
        public override int GetHashCode()
        {
            return value.GetHashCode();
        }
        
    }
    public class Hand : IComparable<Hand>
    {
        public Card[] cards;
        public int rank;
        public int bid;
        public Hand(string line)
        {
            string[] split = line.Split(' ');
            this.cards = new Card[5]
            {
                new Card(split[0][0]),
                new Card(split[0][1]),
                new Card(split[0][2]),
                new Card(split[0][3]),
                new Card(split[0][4])
            };

            this.bid = int.Parse(split[split.Length - 1]);
        }        
        public bool FiveOfKind
        {
            get
            {
                Card card0 = cards[0];
                return cards.All(x => x.EqualsOrJoker(card0));
            }
        }
        public bool FourOfKind
        {
            get
            {
                for (int i = 0; i < cards.Length; i++)
                {
                    Card card = cards[i];              
                    if (card.value == 'J')
                    {
                        if (cards.Count(x => card.Equals(x)) == 4)
                        {
                            return true;
                        }
                    }
                    if (cards.Count(x => card.EqualsOrJoker(x)) == 4)
                    {
                        return true;
                    }
                }
                return false;
            }
        }
        public bool ThreeOfAKind
        {
            get
            {
                for (int i = 0; i < cards.Length; i++)
                {
                    Card card = cards[i];
                    if (card.value == 'J')
                    {
                        if (cards.Count(x => card.Equals(x)) == 3)
                        {
                            return true;
                        }
                    }
                    if (cards.Count(x => card.EqualsOrJoker(x)) == 3)
                    {
                        return true;
                    }
                }
                return false;
            }
        }
        public bool FullHouse
        {
            get
            {
                return ThreeOfAKind && OnePair;
            }
        }
        public List<Card> Pairs
        {
            get
            {
                int jokersLeft = cards.Count(x => x.value == 'J');
                List<Card> pairs = new List<Card>();
                for (int i = 0; i < cards.Length; i++)
                {
                    Card card = cards[i];
                    if (cards.Count(x => x.Equals(card)) == 2)
                    {
                        if (!pairs.Contains(card))
                        {
                            pairs.Add(card);
                        }
                    }
                    else if (cards.Count(x => card.EqualsOrJoker(x)) == 2)
                    {
                        if (!pairs.Contains(card))
                        {
                            pairs.Add(card);
                        }
                    }
                }
                return pairs;
            }            
        }
        public bool OnePair => Pairs.Count == 1;
        public bool TwoPairs => Pairs.Count == 2;
        public bool HighCard
        {
            get
            {
                Card card = cards[0];
                int i = 0;
                for (i = 0; i < cards.Length; i++)
                {
                    card = cards[i];
                    if (card.value != 'J')
                    {
                        break;
                    }
                }
                return cards.All(x => card.EqualsOrJoker(x));           
            }                      
        }        
              
        public double Strength
        {
            get
            {
                List<Card> cardsCopy = new List<Card>(cards);
                Dictionary<char, int> cardsDictionary = new Dictionary<char, int>()
                {
                    {'A', 0},
                    {'K', 0},
                    {'Q', 0},
                    {'T', 0},
                    {'9', 0},
                    {'8', 0},
                    {'7', 0},
                    {'6', 0},
                    {'5', 0},
                    {'4', 0},
                    {'3', 0},
                    {'2', 0},
                    {'J', 0 }
                };
                
                double bestStrength = 0;
                char bestCard = ' ';

                for (int i = 0; i < cards.Length; i++)
                {
                    cardsDictionary[cards[i].value]++;                    
                }

                foreach (KeyValuePair<char, int> cardPair in cardsDictionary)
                {
                    if (cardPair.Key != 'J' && cardPair.Value > bestStrength)
                    {
                        bestStrength = cardPair.Value;
                        bestCard = cardPair.Key;
                    }
                }
                if (bestCard != 'J')
                {
                    bestStrength += cardsDictionary['J'];
                }
                cardsCopy.RemoveAll(x => x.value == bestCard || x.value == 'J');

                if (bestStrength == 3)
                {
                    if (cardsCopy[0].value == cardsCopy[1].value)
                    {
                        bestStrength += 0.5;
                    }                    
                }
                else if (bestStrength == 2)
                {
                    if (cardsCopy[0].value == cardsCopy[1].value
                        || cardsCopy[0].value == cardsCopy[2].value
                        || cardsCopy[1].value == cardsCopy[2].value)
                    {
                        bestStrength += 0.5;
                    }                   
                }
                return bestStrength;
            }
        }
        public int CompareTo(Hand other)
        {
            double thisStrength = Strength;
            double otherStrength = other.Strength;

            if (thisStrength == other.Strength)
            {
                for (int i = 0; i < cards.Length; i++)
                {
                    int comparison = cards[i].CompareTo(other.cards[i]);
                    if (comparison != 0)
                    {
                        return comparison;
                    }
                }
                return 0; //same card
            }
            else if (thisStrength > otherStrength)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }
        public override string ToString()
        {
            return String.Join("", cards.Select(x => x.ToString()));
        }
        public static void Sort(ref List<Hand> hands)
        {
            // do quicksort using CompareTo
            if (hands.Count <= 1)
            {
                return;
            }
            List<Hand> sorted = new List<Hand>();
            
            Hand pivot = hands[0];
            hands.RemoveAt(0);
            List<Hand> smaller = new List<Hand>();
            List<Hand> bigger = new List<Hand>();
            foreach (Hand hand in hands)
            {
                if (hand.CompareTo(pivot) == -1)
                {
                    smaller.Add(hand);
                }
                else
                {
                    bigger.Add(hand);
                }
            }
            Hand.Sort(ref smaller);
            Hand.Sort(ref bigger);

            hands.Clear();
            hands.AddRange(smaller);
            hands.Add(pivot);
            hands.AddRange(bigger);

        }
        public static void SetRanks(ref List<Hand> hands)
        {            
            Hand.Sort(ref hands);
            int rank = 0;


            for (int i = 0; i < hands.Count; i++)
            {
                if (i == 0 || hands[i].ToString() != hands[i - 1].ToString())
                {
                    rank++;
                }
                else
                {
                    rank = rank;
                }
                hands[i].rank = rank;

            }

        }
    }
}

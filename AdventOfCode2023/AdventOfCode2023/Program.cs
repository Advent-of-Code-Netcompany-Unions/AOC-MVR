using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Timers;

namespace AdventOfCode2023
{
    class Program
    {
        static void Main(string[] args)
        {

            //DayOne();

            //DayTwo();

            //DayThree();

            //DayFour();

            //DayFive();

            //DaySix();

            //DaySeven();

            DayEight();
        }


        private static List<string> GetInput(string path)
        {
            var allLines = new List<string>();
            StreamReader sr = new StreamReader(path);
            var line = "";

            while (line != null)
            {

                //Console.WriteLine(line);

                line = sr.ReadLine();

                if (line != null)
                {
                    allLines.Add(line);
                }
            }

            sr.Close();

            return allLines;
        }

        private static void DayOne()
        {
            var totalSum = 0;
            var allLines = new List<string>();
            StreamReader sr = new StreamReader("D:\\Code\\AdventOfCode2023\\AdventOfCode2023\\Day1Input.txt");

            var line = "";

            while (line != null)
            {

                //Console.WriteLine(line);

                line = sr.ReadLine();

                if (line != null)
                {
                    allLines.Add(line);
                }
            }

            sr.Close();

            foreach(var l in allLines)
            {
                string firstNum = "";
                string lastNum = "";

                if(l == null)
                {
                    continue;
                }

                var numbersInLine = ExtractNumbers(l.ToLower());


                for (int i = 0; i < l.Length; i++)
                {

                    if (string.IsNullOrEmpty(firstNum) == false && string.IsNullOrEmpty(lastNum) == false)
                    {
                        break;
                    }

                    var firstTextTest = numbersInLine.Where(x => x.Index <= i).Select(x => x.Number).FirstOrDefault();

                    if(firstTextTest > 0 && string.IsNullOrEmpty(firstNum))
                    {
                        firstNum = firstTextTest.ToString();
                    }

                    var lastTextIndex = numbersInLine.Where(x => x.Index >= (l.Length - i - 1)).Select(x => x.Number).FirstOrDefault();

                    if (lastTextIndex > 0 && string.IsNullOrEmpty(lastNum))
                    {
                        lastNum = lastTextIndex.ToString();
                    }

                    if (int.TryParse(l[i].ToString(), out _) && string.IsNullOrEmpty(firstNum))
                    {
                        firstNum = l[i].ToString();
                    }

                    if (int.TryParse(l[l.Length - i - 1].ToString(), out _) && string.IsNullOrEmpty(lastNum))
                    {
                        lastNum = l[l.Length - i - 1].ToString();
                    }
                }

                string num = firstNum.ToString() + lastNum;

                Console.WriteLine(l + " " + num);

                totalSum = totalSum + int.Parse(num);
            }

            Console.WriteLine("Total sum: " + totalSum);
        }

        private static List<(int Number, int Index)> ExtractNumbers(string input)
        {
            var list = new List<(int Number, int Index)>();
      

            var wordToNumber = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
            {
                {"one", 1},
                {"two", 2},
                {"three", 3},
                {"four", 4},
                {"five", 5},
                {"six", 6},
                {"seven", 7},
                {"eight", 8},
                {"nine", 9},
            };


            foreach (var word in wordToNumber)
            {
                if (input.Contains(word.Key))
                {
                    var firstOccurence = input.IndexOf(word.Key);
                    var lastOccurence = input.LastIndexOf(word.Key);

                    if(firstOccurence == lastOccurence)
                    {
                        list.Add((wordToNumber[word.Key], firstOccurence));
                    }
                    else
                    {
                        list.Add((wordToNumber[word.Key], firstOccurence));
                        list.Add((wordToNumber[word.Key], lastOccurence));
                    }
                }
            }

            return list;
        }

        private static void DayTwo()
        {
            var totalSum = 0;
            var totalPower = 0;
            var allLines = new List<string>();
            StreamReader sr = new StreamReader("D:\\Code\\AdventOfCode2023\\AdventOfCode2023\\Day2Input.txt");
            //StreamReader sr = new StreamReader("D:\\Code\\AdventOfCode2023\\AdventOfCode2023\\test.txt");
            var line = "";

            while (line != null)
            {

                //Console.WriteLine(line);

                line = sr.ReadLine();

                if (line != null)
                {
                    allLines.Add(line);
                }
            }

            sr.Close();

            Dictionary<string, Dictionary<string, int>> games = new Dictionary<string, Dictionary<string, int>>();

            Dictionary<string, int> gameRules = new Dictionary<string, int>();
            gameRules.Add("red", 12);
            gameRules.Add("green", 13);
            gameRules.Add("blue", 14);

            foreach (var l in allLines)
            {
                var invalidGame = false;
                Dictionary<string, int> maxNums = new Dictionary<string, int>();
                var gameSplit = l.Split(':');

                var game = gameSplit[0].Split(' ');

                var gameNum = game[1];

                var handShowings = gameSplit[1].Split(';');

                foreach(var showing in handShowings)
                {
                    var colors = showing.Split(',');

                    foreach(var colorNum in colors)
                    {
                        var split = colorNum.Split(" ");

                        var num = split[1];
                        var color = split[2];

                        if(gameRules.ContainsKey(color) == true && gameRules[color] < int.Parse(num) && invalidGame == false)
                        {
                            invalidGame = true;
                        }

                        if (maxNums.ContainsKey(color) == false || maxNums[color] < int.Parse(num))
                        {
                            maxNums[color] = int.Parse(num);
                        }
                    }

                }
                games.Add(gameNum,maxNums);
                if(invalidGame == false)
                {
                    totalSum += int.Parse(gameNum);
                }

                var power = 1;
                foreach(var max in maxNums)
                {
                    power *= max.Value;
                }

                totalPower += power;
            }

            Console.WriteLine("Total sum: " + totalSum);
            Console.WriteLine("Total power: " + totalPower);

        }

        private static void DayThree()
        {
            var input = GetInput("D:\\Code\\AdventOfCode2023\\AdventOfCode2023\\Day3Input.txt");

            int lineNum = 0;
            string[,] grid = new string[input.Count(), input[0].Length];

            var gridMap = new Dictionary<(int,int), Node>();
            var indexMap = new Dictionary<(int, int), List<(int,int)>>();

            foreach (var l in input)
            {
                char[] charArray = l.ToCharArray();

                string[] line = Array.ConvertAll(charArray, c => c.ToString());

                for(int i = 0; i<line.Length; i++)
                {
                    grid[lineNum, i] = line[i];

                    var node = new Node
                    {
                        index = (lineNum, i),
                        value = line[i],
                        IsNumber = int.TryParse(line[i], out _),
                        IsSymbol = string.Equals(line[i], ".") == false && int.TryParse(line[i], out _),
                        IsGear = line[i].Equals("*"),
                        FullNumber = 0
                    };

                    if (node.IsNumber == true)
                    {
                        node.FullNumber = GetFullNumber(line, node);
                    }

                    int top = lineNum - 1;
                    int bottom = lineNum + 1;
                    int left = i - 1;
                    int right = i + 1;

                    List<(int, int)> neighbors = new List<(int, int)>();

                    if(top >= 0)
                    {
                        neighbors.Add((top, i));

                        if (left >= 0)
                        {
                            neighbors.Add((top, left));
                        }

                        if (right < line.Length)
                        {
                            neighbors.Add((top, right));
                        }
                    }

                    if (bottom < input.Count())
                    {
                        neighbors.Add((bottom, i));

                        if (left >= 0)
                        {
                            neighbors.Add((bottom, left));
                        }

                        if (right < line.Length)
                        {
                            neighbors.Add((bottom, right));
                        }
                    }

                    if (left >= 0)
                    {
                        neighbors.Add((lineNum, left));
                    }

                    if (right < line.Length)
                    {
                        neighbors.Add((lineNum, right));
                    }

                    gridMap.Add((lineNum, i), node);
                    indexMap.Add((lineNum,i), neighbors);

                }

                lineNum++;
            }

            //var symbolNodes = gridMap.Where(x => x.Value.IsSymbol == true).Select(x => x).ToList();
            //var totalSum = 0;
            //foreach(var node in symbolNodes)
            //{
            //    foreach(var neighbor in indexMap[node.Key])
            //    {
            //        totalSum += gridMap[(neighbor.Item1, neighbor.Item2)].FullNumber;
            //    }
            //}

            var totalSumPar1 = CalculateSum(grid);

            Console.WriteLine(totalSumPar1);

            var totalSumPart2 = Day3Part2(gridMap, indexMap);
            Console.WriteLine(totalSumPart2);
        }

        private static object Day3Part2(Dictionary<(int, int), Node> gridMap, Dictionary<(int, int), List<(int, int)>> indexMap)
        {
            var gearNodes = gridMap.Where(n => n.Value.IsGear).Select(n => n);
            var totalSum = 0;

            foreach(var gearNode in gearNodes)
            {
                var numberOfNumNeighbours = 0;
                var neighBours = indexMap[gearNode.Key];
                var currentNeighboursToCount = new List<Node>();

                foreach(var n in neighBours)
                {
                    var isCovered = currentNeighboursToCount.Where(x => indexMap[x.index].Contains(n)).Select(x => x).ToList();
                    if (gridMap[n].IsNumber && (isCovered == null || isCovered.Any() == false))
                    {
                        numberOfNumNeighbours++;
                        currentNeighboursToCount.Add(gridMap[n]);
                    }
                }

                if(numberOfNumNeighbours >= 2)
                {
                    var mul = 1;
                    foreach(var n in currentNeighboursToCount)
                    {
                        mul *= n.FullNumber;
                    }

                    totalSum += mul;
                }
            }

            return totalSum;
        }


        private static int GetFullNumber(string[] line, Node node)
        {   
            var num = "";

            Dictionary<int,int> numberIndexes = new Dictionary<int, int>();
            List<int> indexes = new List<int>();

            for (int i = 0; i<line.Length; i++)
            {

                if(int.TryParse(line[i], out _))
                {
                    indexes.Add(i);
                    num += line[i];

                    if(i == line.Length - 1 && string.IsNullOrEmpty(num) == false)
                    {
                        var fullNum = int.Parse(num);

                        foreach (var index in indexes)
                        {
                            numberIndexes.Add(index, fullNum);
                        }

                        indexes.Clear();
                        num = "";
                    }
                }
                else if (string.IsNullOrEmpty(num) == false)
                {
                    var fullNum = int.Parse(num);

                    foreach (var index in indexes)
                    {
                        numberIndexes.Add(index, fullNum);
                    }

                    indexes.Clear();
                    num = "";
                }
            }

            return numberIndexes[node.index.Item2];
            
        }

        private static int CalculateSum(string[,] map)
        {
            var totalSum = 0;
            var currentNum = "";
            var currentIndexesOnLine = new List<int>();

            for(int i = 0; i<map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    if (int.TryParse(map[i,j], out _))
                    {
                        currentNum += map[i,j];
                        currentIndexesOnLine.Add(j);

                        if (j == map.GetLength(1) - 1 && string.IsNullOrEmpty(currentNum) == false)
                        {
                            if(HasSymbolNeighbour(map, i, currentIndexesOnLine))
                            {
                                totalSum += int.Parse(currentNum);
                            }

                            currentNum = "";
                            currentIndexesOnLine.Clear();
                        }
                    }
                    else if (string.IsNullOrEmpty(currentNum) == false)
                    {
                        if (HasSymbolNeighbour(map, i, currentIndexesOnLine))
                        {
                            totalSum += int.Parse(currentNum);
                        }

                        currentNum = "";
                        currentIndexesOnLine.Clear();
                    }
                }
            }

            return totalSum;
        }

        private static bool HasSymbolNeighbour(string[,] map, int i, List<int> jIndexes)
        {

            var topIndex = i - 1;
            var bottomIndex = i + 1;

            foreach (var index in jIndexes)
            {

                var leftIndex = index - 1;
                var rightIndex = index + 1;

                if (topIndex >= 0)
                {  
                    if(map[topIndex,index].Equals(".") == false && int.TryParse(map[topIndex,index], out _) == false)
                    {
                        return true;
                    }

                    if (leftIndex >= 0)
                    {
                        if (map[topIndex,leftIndex].Equals(".") == false && int.TryParse(map[topIndex,leftIndex], out _) == false)
                        {
                            return true;
                        }
                    }

                    if (rightIndex <= map.GetLength(1) - 1)
                    {
                        if (map[topIndex,rightIndex].Equals(".") == false && int.TryParse(map[topIndex,rightIndex], out _) == false)
                        {
                            return true;
                        }
                    }

                }

                if (bottomIndex <= map.GetLength(0) - 1)
                {
                    if (map[bottomIndex,index].Equals(".") == false && int.TryParse(map[bottomIndex,index], out _) == false)
                    {
                        return true;
                    }

                    if (leftIndex >= 0)
                    {
                        if (map[bottomIndex,leftIndex].Equals(".") == false && int.TryParse(map[bottomIndex,leftIndex], out _) == false)
                        {
                            return true;
                        }
                    }

                    if (rightIndex <= map.GetLength(1) - 1)
                    {
                        if (map[bottomIndex,rightIndex].Equals(".") == false && int.TryParse(map[bottomIndex,rightIndex], out _) == false)
                        {
                            return true;
                        }
                    }
                }

                if(leftIndex >= 0)
                {
                    if (map[i,leftIndex].Equals(".") == false && int.TryParse(map[i,leftIndex], out _) == false)
                    {
                        return true;
                    }
                }

                if(rightIndex <= map.GetLength(1) - 1)
                {
                    if (map[i,rightIndex].Equals(".") == false && int.TryParse(map[i,rightIndex], out _) == false)
                    {
                        return true;
                    }
                }
            }

            return false;
        }


        private static void DayFour()
        {
            var totalSum = 0;
            var input = GetInput("D:\\Code\\AdventOfCode2023\\AdventOfCode2023\\Day4Input.txt");
            //var input = GetInput("D:\\Code\\AdventOfCode2023\\AdventOfCode2023\\Test.txt");

            DayFourPartOne(input);
            DayFourPartTwo(input);
            
        }

        private static void DayFourPartOne(List<string> input)
        {
            var totalSum = 0;

            foreach (var line in input)
            {
                var cardScore = 0;
                var lineSplit = line.Split(":");
                var cardNum = lineSplit[0];
                var cardValues = lineSplit[1];

                var cardValuesSplit = cardValues.Split("|");
                var winningNumbers = cardValuesSplit[0];
                var myNumbers = cardValuesSplit[1];

                var winningNumbersArray = winningNumbers.Split(" ");
                winningNumbersArray = winningNumbersArray.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                var myNumbersArray = myNumbers.Split(" ");
                myNumbersArray = myNumbersArray.Where(x => !string.IsNullOrEmpty(x)).ToArray();

                var intersection = winningNumbersArray.Intersect(myNumbersArray);

                if (intersection.Count() > 0)
                {
                    cardScore = 1;

                    for (int i = 0; i < intersection.Count() - 1; i++)
                    {
                        cardScore *= 2;
                    }


                    totalSum += cardScore;
                }

            }

            Console.WriteLine(totalSum);
        }

        private static void DayFourPartTwo(List<string> input)
        {
            var totalSum = 0;

            var winningsPerCard = new Dictionary<int, int>();
            var cardInstances = new Dictionary<int, int>();
            var queue = new Queue<int>();

            int i = 1;

            foreach (var line in input)
            {
                var lineSplit = line.Split(":");
                var cardNum = lineSplit[0];
                var cardValues = lineSplit[1];

                var cardValuesSplit = cardValues.Split("|");
                var winningNumbers = cardValuesSplit[0];
                var myNumbers = cardValuesSplit[1];

                var winningNumbersArray = winningNumbers.Split(" ");
                winningNumbersArray = winningNumbersArray.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                var myNumbersArray = myNumbers.Split(" ");
                myNumbersArray = myNumbersArray.Where(x => !string.IsNullOrEmpty(x)).ToArray();

                var intersection = winningNumbersArray.Intersect(myNumbersArray);

                if (intersection.Count() > 0)
                {
                    winningsPerCard.Add(i,intersection.Count());
                }
                else
                {
                    winningsPerCard.Add(i, 0);
                }

                cardInstances.Add(i, 1);
                queue.Enqueue(i);
                i++;
            }

            while(queue.Count() > 0)
            {
                var card = queue.Dequeue();
                var numWinnings = winningsPerCard[card];

                for(int j = 1; j <= numWinnings; j++)
                {
                    cardInstances[card + j] = cardInstances[card + j] + 1;
                    queue.Enqueue(card+j);
                }
            }

            foreach(var card in cardInstances)
            {
                totalSum += card.Value;
            }

            Console.WriteLine(totalSum);

        }


        private static void DayFive()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var input = GetInput("D:\\Code\\AdventOfCode2023\\AdventOfCode2023\\Day5Input.txt");
            //var input = GetInput("D:\\Code\\AdventOfCode2023\\AdventOfCode2023\\Test.txt");
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Console.WriteLine("Elapsed time getting input: " + elapsedMs + "ms");

            //DayFivePartOne(input);
            DayFivePartOneNew(input);
            DayFivePartTwo(input);

        }

        private static void FillMap(string line, Dictionary<long,long> map)
        {
            var split = line.Split(" ");
            var target = Int64.Parse(split[0]);
            var source = Int64.Parse(split[1]);
            var aheads = Int64.Parse(split[2]);

            for(int i = 0; i <= aheads; i++)
            {
                map[source + i] = target + i;
            }
        }


        private static void DayFivePartOneNew(List<string> input)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var seedLine = input[0];
            var seedLineSplit = seedLine.Split(":");
            var seeds = seedLineSplit[1].Split(" ");
            seeds = seeds.Where(x => !string.IsNullOrEmpty(x)).ToArray();

            var currentString = "";

            var lowestLocation = long.MaxValue;

            foreach (var seed in seeds)
            {
                var value = Int64.Parse(seed);
                var foundNextValue = false;

                int firstLine = 0;

                foreach (var line in input)
                {
                    if (string.IsNullOrEmpty(line))
                    {
                        continue;
                    }

                    if(firstLine == 0)
                    {
                        firstLine++;
                        continue;
                    }

                    if (line.Equals("seed-to-soil map:"))
                    {
                        currentString = "seed-to-soil map:";
                        continue;
                    }
                    else if (line.Equals("soil-to-fertilizer map:"))
                    {
                        foundNextValue = false;
                        currentString = "soil-to-fertilizer map:";
                        continue;
                    }
                    else if (line.Equals("fertilizer-to-water map:"))
                    {
                        foundNextValue = false;
                        currentString = "fertilizer-to-water map:";
                        continue;
                    }
                    else if (line.Equals("water-to-light map:"))
                    {
                        foundNextValue = false;
                        currentString = "water-to-light map:";
                        continue;
                    }
                    else if (line.Equals("light-to-temperature map:"))
                    {
                        foundNextValue = false;
                        currentString = "light-to-temperature map:";
                        continue;
                    }
                    else if (line.Equals("temperature-to-humidity map:"))
                    {
                        foundNextValue = false;
                        currentString = "temperature-to-humidity map:";
                        continue;
                    }
                    else if (line.Equals("humidity-to-location map:"))
                    {
                        foundNextValue = false;
                        currentString = "humidity-to-location map:";
                        continue;
                    }

                    var split = line.Split(" ");
                    var target = Int64.Parse(split[0]);
                    var source = Int64.Parse(split[1]);
                    var aheads = Int64.Parse(split[2]);

                    if (currentString.Equals("seed-to-soil map:") && foundNextValue == false)
                    {
                        if(value >= source && value <= source + aheads)
                        {
                            var diff = value - source;
                            value = target + diff;
                            foundNextValue = true;
                        }
                    }
                    else if (currentString.Equals("soil-to-fertilizer map:") && foundNextValue == false)
                    {
                        if (value >= source && value <= source + aheads)
                        {
                            var diff = value - source;
                            value = target + diff;
                            foundNextValue = true;
                        }
                    }
                    else if (currentString.Equals("fertilizer-to-water map:") && foundNextValue == false)
                    {
                        if (value >= source && value <= source + aheads)
                        {
                            var diff = value - source;
                            value = target + diff;
                            foundNextValue = true;
                        }
                    }
                    else if (currentString.Equals("water-to-light map:") && foundNextValue == false)
                    {
                        if (value >= source && value <= source + aheads)
                        {
                            var diff = value - source;
                            value = target + diff;
                            foundNextValue = true;
                        }
                    }
                    else if (currentString.Equals("light-to-temperature map:") && foundNextValue == false)
                    {
                        if (value >= source && value <= source + aheads)
                        {
                            var diff = value - source;
                            value = target + diff;
                            foundNextValue = true;
                        }
                    }
                    else if (currentString.Equals("temperature-to-humidity map:") && foundNextValue == false)
                    {
                        if (value >= source && value <= source + aheads)
                        {
                            var diff = value - source;
                            value = target + diff;
                            foundNextValue = true;
                        }
                    }
                    else if (currentString.Equals("humidity-to-location map:") && foundNextValue == false)
                    {
                        if (value >= source && value <= source + aheads)
                        {
                            var diff = value - source;
                            value = target + diff;
                            foundNextValue = true;
                        }
                    }

                }

                if(value < lowestLocation)
                {
                    lowestLocation = value;
                }
            }

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Console.WriteLine("Day5 Part one solution: " + lowestLocation + "\nElapsed time: " + elapsedMs + "ms");
        }

        private static void DayFivePartTwo(List<string> input)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            var seedLine = input[0];
            var seedLineSplit = seedLine.Split(":");
            var seedArray = seedLineSplit[1].Split(" ");
            seedArray = seedArray.Where(x => !string.IsNullOrEmpty(x)).ToArray();

            var ranges = GetAllSeedNumbers(seedArray);

            var currentString = "";

            var lowestLocation = long.MaxValue;

            /*
            Make a list/dictionary? "ranges" (start by the seed ranges) which contains a list of (value, aheads) where aheads = length of the range
            on each line from the map-input look for range intersections store (lowest value, aheads) and extract this range from the range that were used for the intersection
            when done we will have a list of (lowest values, aheads) and then just have to compare the lowest values
             */

            int firstLine = 0;
            var lines = input.Where(x => !string.IsNullOrEmpty(x)).ToList();
            foreach (var line in lines)
            {
                if (string.IsNullOrEmpty(line))
                {
                    continue;
                }

                if (firstLine == 0)
                {
                    firstLine++;
                    continue;
                }

                if (line.Equals("seed-to-soil map:"))
                {
                    currentString = "seed-to-soil map:";
                    var keys = new List<long>(ranges.Keys);
                    foreach (var key in keys)
                    {
                        var value = ranges[key];

                        value.Item2 = false;

                        ranges[key] = value;
                    }

                    continue;
                }
                else if (line.Equals("soil-to-fertilizer map:"))
                {
                    currentString = "soil-to-fertilizer map:";
                    var keys = new List<long>(ranges.Keys);
                    foreach (var key in keys)
                    {
                        var value = ranges[key];

                        value.Item2 = false;

                        ranges[key] = value;
                    }
                    continue;
                }
                else if (line.Equals("fertilizer-to-water map:"))
                {
                    currentString = "fertilizer-to-water map:";
                    var keys = new List<long>(ranges.Keys);
                    foreach (var key in keys)
                    {
                        var value = ranges[key];

                        value.Item2 = false;

                        ranges[key] = value;
                    }
                    continue;
                }
                else if (line.Equals("water-to-light map:"))
                {
                    currentString = "water-to-light map:";
                    var keys = new List<long>(ranges.Keys);
                    foreach (var key in keys)
                    {
                        var value = ranges[key];

                        value.Item2 = false;

                        ranges[key] = value;
                    }
                    continue;
                }
                else if (line.Equals("light-to-temperature map:"))
                {
                    currentString = "light-to-temperature map:";
                    var keys = new List<long>(ranges.Keys);
                    foreach (var key in keys)
                    {
                        var value = ranges[key];

                        value.Item2 = false;

                        ranges[key] = value;
                    }
                    continue;
                }
                else if (line.Equals("temperature-to-humidity map:"))
                {
                    currentString = "temperature-to-humidity map:";
                    var keys = new List<long>(ranges.Keys);
                    foreach (var key in keys)
                    {
                        var value = ranges[key];

                        value.Item2 = false;

                        ranges[key] = value;
                    }
                    continue;
                }
                else if (line.Equals("humidity-to-location map:"))
                {
                    currentString = "humidity-to-location map:";
                    var keys = new List<long>(ranges.Keys);
                    foreach (var key in keys)
                    {
                        var value = ranges[key];

                        value.Item2 = false;

                        ranges[key] = value;
                    }
                    continue;
                }

                var split = line.Split(" ");
                var target = Int64.Parse(split[0]);
                var source = Int64.Parse(split[1]);
                var aheads = Int64.Parse(split[2]);

                var relevantRanges = ranges.Where(x => x.Value.Item2 == false).Select(x => x).ToList();

                foreach(var range in relevantRanges)
                {
                    var startRange = range.Key;
                    var endRange = startRange + range.Value.Item1;

                    var startMap = source;
                    var endMap = startMap + aheads;

                    var intersection = FindIntersection(startRange, endRange, startMap, endMap);

                    if(intersection == (0, 0))
                    {
                        continue;
                    }
                    else
                    {
                        
                        var newRange = (intersection.Item1, (intersection.Item2, true));
                        var oldRangeBeginning = (intersection.Item1 - startRange - 1, false);
                        var numItemsInLastRange = range.Value.Item1 - newRange.Item2.Item1 - oldRangeBeginning.Item1 - 1;
                        var diff = newRange.Item1 - source;
                        var newTarget = target + diff;

                        if (range.Key == newRange.Item1)
                        {

                            ranges.Remove(startRange);

                            if (ranges.ContainsKey(newTarget))
                            {
                                ranges[newTarget] = (Math.Max(ranges[newTarget].Item1, intersection.Item2),true);
                            }
                            else
                            {
                                ranges.Add(newTarget, (intersection.Item2, true));
                            }
                        }
                        else
                        {
                            ranges[startRange] = oldRangeBeginning;
                            if (ranges.ContainsKey(newTarget))
                            {
                                ranges[newTarget] = (Math.Max(ranges[newTarget].Item1, intersection.Item2), true);
                            }
                            else
                            {
                                ranges.Add(newTarget, (intersection.Item2, true));
                            }
                        }
                           
                        if (numItemsInLastRange > 0 )
                        {

                            if (ranges.ContainsKey(intersection.Item1 + intersection.Item2 + 1))
                            {
                                ranges[newTarget] = (Math.Max(ranges[newTarget].Item1, numItemsInLastRange), true);
                            }
                            else
                            {
                                ranges.Add(intersection.Item1 + intersection.Item2 + 1, (numItemsInLastRange, false));
                            }
                        }
                    }
                }

            }

            var minKey = ranges.Keys.Min();
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Console.WriteLine("Day5 Part two solution: " + minKey + "\nElapsed time: " + elapsedMs +"ms");

        }

        private static Dictionary<long, (long, bool)> GetAllSeedNumbers(string[] seedArray)
        {

            var dictionary = new Dictionary<long, (long, bool)>();

            for(int i = 0; i<seedArray.Length - 1; i++)
            {
                var seedBegin = Int64.Parse(seedArray[i]);
                var seedAhead = Int64.Parse(seedArray[i + 1]);

                dictionary.Add(seedBegin, (seedAhead, false));
               
                i++;
            }

            return dictionary;
        }

        static (long,long) FindIntersection(long start1, long end1, long start2, long end2)
        {
            if (start1 <= end2 && end1 >= start2)
            {
                long intersectionStart = Math.Max(start1, start2);
                long intersectionEnd = Math.Min(end1, end2);

                return (intersectionStart, intersectionEnd - intersectionStart);
            }

            return (0,0);
        }

        static List<long> GenerateRange(long start, long end)
        {
            int capacity = (int)(end - start + 1);
            List<long> result = new List<long>(capacity);

            for (long i = start; i <= end; i++)
            {
                result.Add(i);
            }

            return result;
        }

        private static void DaySix()
        {
           var input = GetInput("D:\\Code\\AdventOfCode2023\\AdventOfCode2023\\Day6Input.txt");
           //var input = GetInput("D:\\Code\\AdventOfCode2023\\AdventOfCode2023\\Test.txt");

            DaySixPartOne(input);
            DaySixPartTwo(input);
        }

        private static void DaySixPartOne(List<string> input)
        {
            var times = input[0];
            var distances = input[1];

            var timesSplit = times.Split(":");
            var distanceSplit = distances.Split(":");

            var timeMatches = Regex.Split(timesSplit[1], @"\D+").ToList();
            var distanceMatches = Regex.Split(distanceSplit[1], @"\D+").ToList();
            var totalProduct = 1;

            for(int i = 0; i<timeMatches.Count(); i++)
            {
                if (string.IsNullOrEmpty(timeMatches[i])) continue;

                var time = int.Parse(timeMatches[i]);
                var distance = int.Parse(distanceMatches[i]);
                var maxDistanceHoldTime = Math.Ceiling((double)time / 2);
                //var t = BinarySearch(int.Parse(timeMatches[i]), int.Parse(distanceMatches[i]), maxTimeHold, new List<double>(), -1, maxTimeHold);
                var minHoldTime = GetLowestHoldTime(time, distance, maxDistanceHoldTime);
                var maxHoldTime = time - minHoldTime;

                var numberWinnings = maxHoldTime - minHoldTime + 1;

                totalProduct *= numberWinnings;

            }

            Console.WriteLine("Day Six Part one solution: " + totalProduct);
        }

        private static void DaySixPartTwo(List<string> input)
        {
            var times = input[0];
            var distances = input[1];

            var timesSplit = times.Split(":");
            var distanceSplit = distances.Split(":");

            var timeMatches = Regex.Split(timesSplit[1], @"\D+").ToList();
            var distanceMatches = Regex.Split(distanceSplit[1], @"\D+").ToList();

            var totalTime = "";
            var totalDistance = "";

            for (int i = 0; i < timeMatches.Count(); i++)
            {
                if (string.IsNullOrEmpty(timeMatches[i])) continue;

                totalTime += timeMatches[i];
                totalDistance += distanceMatches[i];
            }

            var time = Int64.Parse(totalTime);
            var distance = Int64.Parse(totalDistance);
            var maxDistanceHoldTime = Math.Ceiling((double)time / 2);
            //var t = BinarySearch(int.Parse(timeMatches[i]), int.Parse(distanceMatches[i]), maxTimeHold, new List<double>(), -1, maxTimeHold);
            var minHoldTime = GetLowestHoldTime(time, distance, maxDistanceHoldTime);
            var maxHoldTime = time - minHoldTime;

            var numberWinnings = maxHoldTime - minHoldTime + 1;


            Console.WriteLine("Day Six Part two solution: " + numberWinnings);
        }

        private static int GetLowestHoldTime(double time, double distance, double maxTimeHold)
        {

            for(int i = 1; i<maxTimeHold; i++)
            {
                var remainingTime = time - i;
                var travelDistance = remainingTime * i;

                if (travelDistance > distance)
                {
                    return i;
                }
            }

            return 0;
        }


        private static void DaySeven()
        {
            var input = GetInput("D:\\Code\\AdventOfCode2023\\AdventOfCode2023\\Day7Input.txt");
            //var input = GetInput("D:\\Code\\AdventOfCode2023\\AdventOfCode2023\\Test.txt");

            DaySevenPartOne(input);
            DaySevenPartTwo(input);
        }

        static Dictionary<string, int> cardRank = new Dictionary<string, int>()
            {
                { "A", 14 },
                { "K", 13 },
                { "Q", 12 },
                { "T", 10 },
                { "9", 9 },
                { "8", 8 },
                { "7", 7 },
                { "6", 6 },
                { "5", 5 },
                { "4", 4 },
                { "3", 3 },
                { "2", 2 },
                { "J", 1 },
            };

        private static void DaySevenPartOne(List<string> input)
        {
            var handDict = new Dictionary<string, string>();
            var maxMultiplier = input.Count();
            var fiveOfAKind = new List<string>();
            var fourOfAKind = new List<string>();
            var fullHouse = new List<string>();
            var threeOfAKind = new List<string>();
            var twoPair = new List<string>();
            var onePair = new List<string>();
            var highCard = new List<string>();

            
            foreach (var line in input)
            {
                var split = line.Split();
                var hand = split[0];
                var bid = split[1];

                handDict.Add(hand, bid);

                if (hand.GroupBy(x=>x).Any(g=>g.Count() == 5))
                {
                    fiveOfAKind.Add(hand);
                }
                else if(hand.GroupBy(x => x).Any(g => g.Count() == 4))
                {
                    fourOfAKind.Add(hand);
                }
                else if (CheckOccurences(hand, 3, 2))
                {
                    fullHouse.Add(hand);
                }
                else if (hand.GroupBy(x => x).Any(g => g.Count() == 3))
                {
                    threeOfAKind.Add(hand);
                }
                else if (CheckForTwoPairs(hand))
                {
                    twoPair.Add(hand);
                }
                else if (hand.GroupBy(x => x).Any(g => g.Count() == 2))
                {

                    onePair.Add(hand);
                }
                else
                {
                    highCard.Add(hand);
                }
            }

            fiveOfAKind.Sort(CompareHands);
            fourOfAKind.Sort(CompareHands);
            fullHouse.Sort(CompareHands);
            threeOfAKind.Sort(CompareHands);
            twoPair.Sort(CompareHands);
            onePair.Sort(CompareHands);
            highCard.Sort(CompareHands);

            var fullList = fiveOfAKind;
            fullList.AddRange(fourOfAKind);
            fullList.AddRange(fullHouse);
            fullList.AddRange(threeOfAKind);
            fullList.AddRange(twoPair);
            fullList.AddRange(onePair);
            fullList.AddRange(highCard);

            var totalSum = 0;

            for(int i = 0; i < fullList.Count(); i++)
            {
                var multiplier = maxMultiplier - i;
                var handScore = int.Parse(handDict[fullList[i]]) * multiplier;
                totalSum += handScore;
            }

            Console.WriteLine("Solution day 7 part 1 = "+ totalSum);

        }

        private static void DaySevenPartTwo(List<string> input)
        {
            var handDict = new Dictionary<string, string>();
            var maxMultiplier = input.Count();
            var fiveOfAKind = new List<string>();
            var fourOfAKind = new List<string>();
            var fullHouse = new List<string>();
            var threeOfAKind = new List<string>();
            var twoPair = new List<string>();
            var onePair = new List<string>();
            var highCard = new List<string>();


            foreach (var line in input)
            {
                var split = line.Split();
                var orgiginalHand = split[0];
                var bid = split[1];

                handDict.Add(orgiginalHand, bid);

                var jokerHand = orgiginalHand;
                if (orgiginalHand.Contains("J"))
                {
                    jokerHand = HandleJokers(orgiginalHand);
                }

                if (jokerHand.GroupBy(x => x).Any(g => g.Count() == 5))
                {
                    fiveOfAKind.Add(orgiginalHand);
                }
                else if (jokerHand.GroupBy(x => x).Any(g => g.Count() == 4))
                {
                    fourOfAKind.Add(orgiginalHand);
                }
                else if (CheckOccurences(jokerHand, 3, 2))
                {
                    fullHouse.Add(orgiginalHand);
                }
                else if (jokerHand.GroupBy(x => x).Any(g => g.Count() == 3))
                {
                    threeOfAKind.Add(orgiginalHand);
                }
                else if (CheckForTwoPairs(jokerHand))
                {
                    twoPair.Add(orgiginalHand);
                }
                else if (jokerHand.GroupBy(x => x).Any(g => g.Count() == 2))
                {

                    onePair.Add(orgiginalHand);
                }
                else
                {
                    highCard.Add(orgiginalHand);
                }
            }

            fiveOfAKind.Sort(CompareHands);
            fourOfAKind.Sort(CompareHands);
            fullHouse.Sort(CompareHands);
            threeOfAKind.Sort(CompareHands);
            twoPair.Sort(CompareHands);
            onePair.Sort(CompareHands);
            highCard.Sort(CompareHands);

            var fullList = fiveOfAKind;
            fullList.AddRange(fourOfAKind);
            fullList.AddRange(fullHouse);
            fullList.AddRange(threeOfAKind);
            fullList.AddRange(twoPair);
            fullList.AddRange(onePair);
            fullList.AddRange(highCard);

            var totalSum = 0;

            for (int i = 0; i < fullList.Count(); i++)
            {
                var multiplier = maxMultiplier - i;
                var handScore = int.Parse(handDict[fullList[i]]) * multiplier;
                totalSum += handScore;
            }

            Console.WriteLine("Solution day 7 part 1 = " + totalSum);

        }

        private static string HandleJokers(string hand)
        {

            var numberOfJokers = hand.Where(c => c.Equals("J")).Count();
            var jokerLessHand = hand.Replace("J", string.Empty);

            if (string.IsNullOrEmpty(jokerLessHand))
            {
                return "AAAAA";
            }

            var charOccurringMost = ' ';

            charOccurringMost = jokerLessHand.GroupBy(x => x).OrderByDescending(x => x.Count()).First().Key;

            var newHand = hand.Replace('J', charOccurringMost);

            return newHand;

        }

        private static int CompareHands(string h1, string h2)
        {
            for (int i = 0; i < h1.Length; i++)
            {
                if (cardRank[h1[i].ToString()] > cardRank[h2[i].ToString()])
                {
                    return -1;
                }

                if (cardRank[h1[i].ToString()] < cardRank[h2[i].ToString()])
                {
                    return 1;
                }
            }

            return 0;
        }

        static bool CheckOccurences(string input, int count1, int count2)
        {
            var charCounts = input.GroupBy(c => c)
                                  .ToDictionary(group => group.Key, group => group.Count());

            return charCounts.Count(kv => kv.Value == count1) == 1 &&
                   charCounts.Count(kv => kv.Value == count2) == 1;
        }

        static bool CheckForTwoPairs(string input)
        {
            var charCounts = input.GroupBy(c => c)
                                  .ToDictionary(group => group.Key, group => group.Count());

            return charCounts.Values.Where(count => count == 2).Count() == 2;
        }

        private static void DayEight()
        {
            //var input = GetInput("D:\\Code\\AdventOfCode2023\\AdventOfCode2023\\Day8Input.txt");
            var input = GetInput("D:\\Code\\AdventOfCode2023\\AdventOfCode2023\\Test.txt");

            //DayEightPartOne(input);
            DayEightPartTwoNew(input);
        }

        private static void DayEightPartOne(List<string> input)
        {
            var instructions = input[0];
            var instructionModulu = instructions.Length;
            var map = new Dictionary<string, (string, string)>();

            for(int i = 2; i < input.Count(); i++)
            {
                var split = input[i].Split(" = ");
                var key = split[0];

                var signs = split[1].Replace("(", "").Replace(")", "");
                var individualSigns = signs.Split(", ");

                map.Add(key, (individualSigns[0], individualSigns[1]));
            }

            var start = map["AAA"];
            int step = 0;
            var currentLocation = "AAA"; 

            while(currentLocation.Equals("ZZZ") == false)
            {

                var test = step % instructionModulu;
                var test2 = instructions[0];
                var test3 = instructions[1];

                if (instructions[step % instructionModulu].Equals('L'))
                {
                    step++;
                    currentLocation = map[currentLocation].Item1;
                }
                else
                {
                    step++;
                    currentLocation = map[currentLocation].Item2;
                }
            }

            Console.WriteLine("Day eight part one solution: " + step);

        }

        private static void DayEightPartTwo(List<string> input)
        {
            var instructions = input[0];
            var instructionModulu = instructions.Length;
            var map = new Dictionary<string, (string, string)>();

            var currentState = new List<string>();

            for (int i = 2; i < input.Count(); i++)
            {
                var split = input[i].Split(" = ");
                var key = split[0];

                var signs = split[1].Replace("(", "").Replace(")", "");
                var individualSigns = signs.Split(", ");

                map.Add(key, (individualSigns[0], individualSigns[1]));

                if (key.EndsWith("A"))
                {
                    currentState.Add(key);
                }
            }
            int step = 0;

            while (IsGoal(currentState) == false)
            {

                var newState = new List<string>();
                if (instructions[step % instructionModulu].Equals('L'))
                {
                    step++;

                    foreach(var loc in currentState)
                    {
                        newState.Add(map[loc].Item1);
                    }

                }
                else
                {
                    step++;

                    foreach (var loc in currentState)
                    {
                        newState.Add(map[loc].Item2);
                    }
                }

                currentState = newState;
            }

            Console.WriteLine("Day eight part two solution: " + step);

        }

        private static bool IsGoal(List<string> currentState)
        {
            var endingWithZ = currentState.Where(x => x.EndsWith("Z")).ToList();

            //Console.WriteLine("Endin with Z: " + endingWithZ?.Count() + " currentState" + currentState.Count());

            var test = endingWithZ?.Count() >= 1;
            if (test)
            {
                var t = "";
            }

            return endingWithZ?.Count() == currentState.Count();
        }

        private static void DayEightPartTwoNew(List<string> input)
        {
            var instructions = input[0];
            var instructionModulu = instructions.Length;
            var map = new Dictionary<string, (string, string)>();

            var startNodes = new List<string>();
            //var steps = new Dictionary<string, int>();

            for (int i = 2; i < input.Count(); i++)
            {
                var split = input[i].Split(" = ");
                var key = split[0];

                var signs = split[1].Replace("(", "").Replace(")", "");
                var individualSigns = signs.Split(", ");

                map.Add(key, (individualSigns[0], individualSigns[1]));

                if (key.EndsWith("A"))
                {
                    startNodes.Add(key);
                    //steps.Add(key,0);
                }
            }


            var foundGoal = false;
            var listCycles = new List<Dictionary<int, bool>>();

            foreach(var node in startNodes)
            {
                listCycles.Add(FindCyckes(node, map, instructions, instructionModulu));
            }

            var stepCount = 0;
            while(foundGoal == false)
            {
                var numberEndingWithZ = listCycles.Where(x=>
                {
                    var moduluStepCount = stepCount % x.Count();
                    if(moduluStepCount == 0)
                    {
                        return x[1];
                    }
                    else
                    {
                        return x[moduluStepCount];
                    }
                }
                ).Count();

                if(numberEndingWithZ == startNodes.Count())
                {
                    foundGoal = true;
                }

                stepCount++;
            }

            Console.WriteLine("Day eight part one solution: " + stepCount);

        }

        private static Dictionary<int, bool> FindCyckes(string loc, Dictionary<string, (string, string)> map, string instructions, int instructionModulu)
        {
            int step = 0;
            var currentLocation = loc;
            var cycles = new Dictionary<int, bool>();
            var visitedLocations = new Dictionary<int, string>();

            while (visitedLocations.ContainsValue(currentLocation) == false)
            {
                
                var endsWithZ = currentLocation.EndsWith("Z");
                visitedLocations.Add(step, currentLocation);
                cycles.Add(step, endsWithZ);

                if (instructions[step % instructionModulu].Equals('L'))
                {
                    step++;
                    currentLocation = map[currentLocation].Item1;
                }
                else
                {
                    step++;
                    currentLocation = map[currentLocation].Item2;
                }
            }

            return cycles;
        }
    }

    public class Node
    {
        public (int,int) index { get; set; }
        public string value { get; set; }
        public bool IsSymbol { get; set; }
        public bool IsGear { get; set; }
        public bool IsNumber { get; set; }
        public int FullNumber { get; set; }

        public override  bool Equals(object n)
        {
            var node = (Node)n;
            return this.index.Item1 == node.index.Item1 && this.index.Item2 == node.index.Item2;
        }
    }
}

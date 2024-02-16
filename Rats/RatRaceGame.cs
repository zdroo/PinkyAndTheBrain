using Rats;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class RatRaceGame
{
    private readonly IConsoleInputRepository _consoleRepository;
    public char[,] Map;  // 2D array to hold the map
    public Rat Pinky = new Rat("Pinky");
    public Rat Brain = new Rat("Brain");

    public RatRaceGame(IConsoleInputRepository consoleRepository)
    {
        _consoleRepository = consoleRepository;
    }
    public bool IsValidMap(string path)
    {
        List<char> mapSymbols = new List<char> { '.', '#', '@', 'P', 'B', 'p', 'b' };
        string[] lines = File.ReadAllLines(path);

        if (lines.Any() == false) return false; //check if map file is empty
        else
        {
            if (lines.Count() < 4) return false; // check if there are enough lines to play the game

            int len = lines[0].Length;
            foreach (var line in lines) //check if all lines have the same length
            {
                if (line.Length != len) return false;
            }
        }

        for (int i = 0; i < lines.Length; i++)
        {
            if (i == 0 || i == lines.Length - 1)
            {
                for (int j = 0; j < lines[i].Length; j++) //check if first and last lines are only #####
                {
                    if (lines[i][j] != '#') return false;
                }
            }
            else
            {
                if (lines[i][0] != '#' || lines[i][lines[i].Length - 1] != '#') //check if first and last element of each row is # 
                    return false;

                for (int j = 1; j < lines[i].Length - 1; j++)
                {
                    if (mapSymbols.Contains(lines[i][j]) == false) //check if the file only contains map symbols
                    {
                        Console.WriteLine("Map contains invalid symbols.");
                        return false;
                    }
                    if (lines[i][j].Equals('P') || lines[i][j].Equals('p'))
                    {
                        if (Pinky.IsOnMap) //check if pinky is already on the map before adding it
                        {
                            Console.WriteLine($"Too many appearences of {Pinky.Name} on the map.");
                            return false;
                        }
                        Pinky.IsOnMap = true;
                    }
                    if (lines[i][j].Equals('B') || lines[i][j].Equals('b'))
                    {
                        if (Brain.IsOnMap) //check if pinky is already on the map before adding it
                        {
                            Console.WriteLine($"Too many appearences of {Brain.Name} on the map.");
                            return false;
                        }
                        Brain.IsOnMap = true;
                    }
                }
            }
        }
        return true;
    }
    public void LoadMap(string filename)
    {
        string[] lines = File.ReadAllLines(filename);
        int rows = lines.Length;  // add two rows for the outer walls
        int cols = lines[0].Length;  // add two columns for the outer walls
        Map = new char[rows, cols];

        for (int i = 0; i < lines.Length; i++)
        {
            for (int j = 0; j < lines[0].Length; j++)
            {
                Map[i, j] = lines[i][j];
                if (lines[i][j] == 'P' || lines[i][j] == 'p')
                {
                    Pinky.Row = i;
                    Pinky.Column = j;
                    Pinky.IsOnMap = true;
                }
                else if (lines[i][j] == 'B' || lines[i][j] == 'b')
                {
                    Brain.Row = i;
                    Brain.Column = j;
                    Brain.IsOnMap = true;
                }
            }
        }
        Console.WriteLine();
    }
    public void AddRatOnMap(Rat rat)
    {
        if (rat.IsOnMap == false)
        {
            Console.WriteLine($"You have to add {rat.Name} on the map.");
            Console.WriteLine($"Write {rat.Name}'s row then press enter. Rows and columns are counted starting from 0.");

            while (rat.IsOnMap == false)
            {
                try
                {
                    int row = Convert.ToInt32(_consoleRepository.GetNextConsoleInput());
                    Console.WriteLine($"Write {rat.Name}'s column then press enter.");
                    int col = Convert.ToInt32(_consoleRepository.GetNextConsoleInput());

                    if (Map[row, col] != '#' && Map[row, col] != '@')
                    {
                        Map[row, col] = rat.Initial;
                        rat.IsOnMap = true;
                        rat.Row = row;
                        rat.Column = col;
                    }
                    else
                    {
                        Console.WriteLine("That is a wall or a sprout. Try again.");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Invalid input. Coordinate does not exist or it is a wall or sprout. Try again." + "\n" + e.Message);
                }
            }
        }
    }
    public void PrintMap()
    {
        Console.Clear();
        for (int i = 0; i < Map.GetLength(0); i++)
        {
            for (int j = 0; j < Map.GetLength(1); j++)
            {
                Console.Write(Map[i, j]);
            }
            Console.WriteLine();
        }
    }
    public int CountSprouts()
    {
        int count = 0;
        for (int i = 0; i < Map.GetLength(0); i++)
        {
            for (int j = 0; j < Map.GetLength(1); j++)
            {
                if (Map[i, j] == '@')
                {
                    count++;
                }
            }
        }
        return count;
    }
    public void SetOldSpace(Rat currentRat, Rat otherRat) //Set the old space of the rat to '.' or the other rat's name
    {
        if (Pinky.Row == Brain.Row && Pinky.Column == Brain.Column)
        {
            Map[currentRat.Row, currentRat.Column] = otherRat.Initial;
        }
        else
        {
            Map[currentRat.Row, currentRat.Column] = '.';
        }
    }
    public bool IsValidNextSpace(char nextSpace) //Check if next space is a wall and add score to a rat if next space is a sprout
    {
        if (nextSpace != '#')
        {
            return true;
        }
        return false;
    }
    public void ModifyRatScore(char nextSpace, Rat rat)
    {
        if (nextSpace == '@')
        {
            rat.Score++;
        }
    }
    public void MoveRat(Rat currentRat)
    {
        Console.WriteLine();

        Rat otherRat = new Rat();
        bool hasMoved = false;

        if (currentRat == Pinky)
        {
            Console.WriteLine($"{currentRat.Name}'s turn");
            otherRat = Brain;

            Console.WriteLine($"{currentRat.Initial} choose your direction" + "\n" + $"Up: {Map[currentRat.Row - 1, currentRat.Column]}" + "\n" + $"Down: {Map[currentRat.Row + 1, currentRat.Column]}");
            Console.WriteLine($"Left: {Map[currentRat.Row, currentRat.Column - 1]}" + "\n" + $"Right: {Map[currentRat.Row, currentRat.Column + 1]}");

            while (hasMoved == false)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.W:
                        if (IsValidNextSpace(Map[currentRat.Row - 1, currentRat.Column]))
                        {
                            ModifyRatScore(Map[currentRat.Row - 1, currentRat.Column], currentRat);
                            Map[currentRat.Row - 1, currentRat.Column] = currentRat.Initial;
                            SetOldSpace(currentRat, otherRat);
                            currentRat.Row--;
                            hasMoved = true;
                        }
                        break;

                    case ConsoleKey.S:
                        if (IsValidNextSpace(Map[currentRat.Row + 1, currentRat.Column]))
                        {
                            ModifyRatScore(Map[currentRat.Row + 1, currentRat.Column], currentRat);
                            Map[currentRat.Row + 1, currentRat.Column] = currentRat.Initial;
                            SetOldSpace(currentRat, otherRat);
                            currentRat.Row++;
                            hasMoved = true;
                        }
                        break;

                    case ConsoleKey.A:
                        if (IsValidNextSpace(Map[currentRat.Row, currentRat.Column - 1]))
                        {
                            ModifyRatScore(Map[currentRat.Row, currentRat.Column - 1], currentRat);
                            Map[currentRat.Row, currentRat.Column - 1] = currentRat.Initial;
                            SetOldSpace(currentRat, otherRat);
                            currentRat.Column--;
                            hasMoved = true;
                        }
                        break;

                    case ConsoleKey.D:
                        if (IsValidNextSpace(Map[currentRat.Row, currentRat.Column + 1]))
                        {
                            ModifyRatScore(Map[currentRat.Row, currentRat.Column + 1], currentRat);
                            Map[currentRat.Row, currentRat.Column + 1] = currentRat.Initial;
                            SetOldSpace(currentRat, otherRat);
                            currentRat.Column++;
                            hasMoved = true;
                        }
                        break;
                }

                if (!hasMoved)
                {
                    Console.WriteLine("You cannot move in this direction or you pressed a wrong key. Try again.");
                }
            }
        }
        else
        {
            Console.WriteLine($"{currentRat.Name}'s turn");
            otherRat = Pinky;

            Console.WriteLine($"{currentRat.Initial} choose your direction" + "\n" + $"Up: {Map[currentRat.Row - 1, currentRat.Column]}" + "\n" + $"Down: {Map[currentRat.Row + 1, currentRat.Column]}");
            Console.WriteLine($"Left: {Map[currentRat.Row, currentRat.Column - 1]}" + "\n" + $"Right: {Map[currentRat.Row, currentRat.Column + 1]}");

            while (hasMoved == false)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.I:
                        if (IsValidNextSpace(Map[currentRat.Row - 1, currentRat.Column]))
                        {
                            ModifyRatScore(Map[currentRat.Row - 1, currentRat.Column], currentRat);
                            Map[currentRat.Row - 1, currentRat.Column] = currentRat.Initial;
                            SetOldSpace(currentRat, otherRat);
                            currentRat.Row--;
                            hasMoved = true;
                        }
                        break;

                    case ConsoleKey.K:
                        if (IsValidNextSpace(Map[currentRat.Row + 1, currentRat.Column]))
                        {
                            ModifyRatScore(Map[currentRat.Row + 1, currentRat.Column], currentRat);
                            Map[currentRat.Row + 1, currentRat.Column] = currentRat.Initial;
                            SetOldSpace(currentRat, otherRat);
                            currentRat.Row++;
                            hasMoved = true;
                        }
                        break;

                    case ConsoleKey.J:
                        if (IsValidNextSpace(Map[currentRat.Row, currentRat.Column - 1]))
                        {
                            ModifyRatScore(Map[currentRat.Row, currentRat.Column - 1], currentRat);
                            Map[currentRat.Row, currentRat.Column - 1] = currentRat.Initial;
                            SetOldSpace(currentRat, otherRat);
                            currentRat.Column--;
                            hasMoved = true;
                        }
                        break;

                    case ConsoleKey.L:
                        if (IsValidNextSpace(Map[currentRat.Row, currentRat.Column + 1]))
                        {
                            ModifyRatScore(Map[currentRat.Row, currentRat.Column + 1], currentRat);
                            Map[currentRat.Row, currentRat.Column + 1] = currentRat.Initial;
                            SetOldSpace(currentRat, otherRat);
                            currentRat.Column++;
                            hasMoved = true;
                        }
                        break;
                }

                if (!hasMoved)
                {
                    Console.WriteLine("You cannot move in this direction or you pressed a wrong key. Try again.");
                }
            }
        }
    }
}
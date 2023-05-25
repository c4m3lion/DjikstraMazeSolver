using System;
using System.Collections.Generic;
using System.IO;

namespace MyApplication
{
    public class Cell
    {
        public int X, Y;
        public int Coins;
        public bool IsWall;
    }

    class Program
    {
        private static int[] dx = { -1, 0, 1, 0 };
        private static int[] dy = { 0, 1, 0, -1 };
        static void Main(string[] args)
        {
            
            // Enter here your folder name to your txts
            // pls add txt to exe file location otherwise dumb C# cannot take files
            string folderPath = "inputs";

            // Load all files from folderPath
            string[] fileNames = Directory.GetFiles(folderPath, "*.txt");// got this idea from phind.com

            // Read each maze file and solve the maze
            foreach (string fileName in fileNames)
            {
                string[] maze = File.ReadAllLines(fileName);
                //Here we go for Djikstra
                startSearch(maze);
            }
            Console.ReadLine();
        }
        static void startSearch(string[] maze)
        {
            //fill template costs with max value
            int[,] costs = new int[maze.Length, maze[0].Length];
            (int, int)[,] parents = new (int, int)[maze.Length, maze[0].Length];
            for (int i = 0; i < maze.Length; i++)
            {
                for (int j = 0; j < maze[0].Length; j++)
                {
                    costs[i, j] = int.MaxValue;
                }
            }

            //Find start and end points
            int startX = 0, startY = 0, goalX = 0, goalY = 0;
            for (int i = 0; i < maze.Length; i++)
            {
                for (int j = 0; j < maze[0].Length; j++)
                {
                    if (maze[i][j] == 'S')
                    {
                        startX = i;
                        startY = j;
                        costs[i, j] = 0;
                    }
                    if (maze[i][j] == 'G')
                    {
                        goalX = i;
                        goalY = j;
                    }
                }
            }

            var pq = new SortedSet<(int, int, int)>();
            pq.Add((0, startX, startY));
            while (pq.Count > 0)//till there roads we gonna loooooop
            {
                var current = pq.Min;
                pq.Remove(current);
                int currentCost = current.Item1;
                int currentX = current.Item2;
                int currentY = current.Item3;

                if (currentX == goalX && currentY == goalY)//we reach end so print
                {
                    PrintPath(maze, parents, startX, startY, goalX, goalY, currentCost);
                    return;
                }

                for (int i = 0; i < 4; i++)//there are 4 different way to go up and down and left and right
                {
                    int newX = currentX + dx[i];
                    int newY = currentY + dy[i];
                    if (newX < 0 || newY < 0 || newX >= maze.Length || newY >= maze[0].Length || maze[newX][newY] == 'X')
                    {
                        continue;
                    }

                    int newCost = currentCost;
                    if (maze[newX][newY] != 'X' && maze[newX][newY] != 'S' && maze[newX][newY] != 'G') // check if it is not wall or stasrt or end
                    {
                        newCost += maze[newX][newY] - '0';
                    }

                    if (newCost < costs[newX, newY]) //if cost in this node is smaller it means we find shorter path and we replace it
                    {
                        pq.Remove((costs[newX, newY], newX, newY));
                        costs[newX, newY] = newCost;
                        parents[newX, newY] = (currentX, currentY); // I stored path I completly forget we don't needed coordinates before uploading so I Just kept it one way is to add string 
                        pq.Add((newCost, newX, newY));
                    }
                }
            }

            Console.WriteLine("No path to the goal found!");
        }

        static void PrintPath(string[] maze, (int, int)[,] parents, int startX, int startY, int goalX, int goalY, int currentCost)
        {
            var path = new Stack<(int, int)>();
            var current = (goalX, goalY);
            List<char> pathstr = new List<char>();
            while (current != (startX, startY))//loop till start from goal
            {
                path.Push(current);
                current = parents[current.Item1, current.Item2];
                pathstr.Insert(0,maze[current.Item1][current.Item2]);//insert from behind because we stored data reversed from goal
            }
            //pathstr.Reverse();
            pathstr.RemoveAt(0);

            Console.WriteLine($"Path: {String.Join("+", pathstr)} = {currentCost}\n");
            Console.WriteLine("-----------------------------------\n");
        }
    }
}

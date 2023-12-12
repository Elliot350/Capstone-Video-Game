using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.Tilemaps;

public class Astar
{
    public Spot[,] spots;

    public Astar(Vector3Int[,] grid, int columns, int rows)
    {
        spots = new Spot[columns, rows];
    }

    private bool IsValidPath(Vector3Int[,] grid, Spot start, Spot end)
    {
        // Debug.Log($"Checking from ({start.X}, {start.Y}) to ({end.X}, {end.Y})");
        // Debug.Log($"Start {start}, End {end}");
        if (end == null || start == null || end.height >= 1)
        {
            Debug.LogWarning($"No valid path from ({start.X}, {start.Y}) to ({end.X}, {end.Y})");
            return false;
        }
        return true;
    }

    public List<Spot> CreatePath(Vector3Int[,] grid, Vector2Int start, Vector2Int end, int length)
    {
        // Debug.Log("Generating Astar path...");
        Spot End = null;
        Spot Start = null;
        var columns = spots.GetUpperBound(0) + 1;
        var rows = spots.GetUpperBound(1) + 1;
        spots = new Spot[columns, rows];

        // Debug.Log($"Columns: {columns}, rows {rows}");
        
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                spots[i, j] = new Spot(grid[i, j].x, grid[i, j].y, grid[i, j].z);
            }
        }

        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                spots[i, j].AddNeighbours(spots, i, j);
                if (spots[i, j].X == start.x && spots[i, j].Y == start.y)
                    Start = spots[i, j];
                else if (spots[i, j].X == end.x && spots[i, j].Y == end.y)
                    End = spots[i, j];
            }
        }
        
        if (Start == null || End == null || !IsValidPath(grid, Start, End))
            return null;
        
        List<Spot> OpenSet = new List<Spot>();
        List<Spot> ClosedSet = new List<Spot>();

        OpenSet.Add(Start);

        while (OpenSet.Count > 0)
        {
            // Find shortest step distance in the direction of your goal within the open set
            int winner = 0;
            for (int i = 0; i < OpenSet.Count; i++)
            {
                if (OpenSet[i].F < OpenSet[winner].F)
                    winner = i;
                else if (OpenSet[i].F == OpenSet[winner].F) // Tie breaking for faster routing
                    if (OpenSet[i].H < OpenSet[winner].H) 
                        winner = i;
            }

            var current = OpenSet[winner];

            // Found the path, create and returns the path
            if (End != null && OpenSet[winner] == End)
            {
                List<Spot> Path = new List<Spot>();
                var temp = current;
                Path.Add(temp);
                while (temp.previous != null)
                {
                    Path.Add(temp.previous);
                    temp = temp.previous;
                }
                if (length - (Path.Count - 1) < 0)
                {
                    Path.RemoveRange(0, (Path.Count - 1) - length);
                }
                return Path;
            }

            OpenSet.Remove(current);
            ClosedSet.Add(current);

            // Finds the next closest step on the grid
            var neighbours = current.neighbours;

            // Look through our current spot neighbours (current spot is the shortest F distance in OpenSet)
            for (int i = 0; i < neighbours.Count; i++) 
            {
                var n = neighbours[i];
                // Checks to make sure the neighbour of our current tile is not within the OpenSet
                if (!ClosedSet.Contains(n) && n.height < 1)
                {
                    // Gets a temp comparision interger for seeing if a route is shorter that our current path
                    var tempG = current.G + 1;

                    bool newPath = false;
                    if (OpenSet.Contains(n))
                    {
                        // Checks if the neighbour we are checking is within the OpenSet
                        if (tempG < n.G)
                        {
                            n.G = tempG;
                            newPath = true;
                        }
                    }
                    // If its not in OpenSet or ClosedSet, then it IS a new path and it should be added to OpenSet
                    else
                    {
                        n.G = tempG;
                        newPath = true;
                        OpenSet.Add(n);
                    }

                    // If it is a newPath, calculate the H and F and set current to the neighbours previous
                    if (newPath)
                    {
                        n.H = Heuristic(n, End);
                        n.F = n.G + n.H;
                        n.previous = current;
                    }
                }
            }

        }
        return null;
        
    }

    private int Heuristic(Spot a, Spot b)
    {
        // Manhattan
        var dx = Mathf.Abs(a.X - b.X);
        var dy = Mathf.Abs(a.Y - b.Y);
        return 1 * (dx + dy);
    }
}

public class Spot
{
    public int X;
    public int Y;
    public int F;
    public int G;
    public int H;
    public int height = 0;
    public List<Spot> neighbours;
    public Spot previous = null;
    
    public Spot(int x, int y, int height)
    {
        X = x;
        Y = y;
        F = 0;
        G = 0;
        H = 0;
        neighbours = new List<Spot>();
        this.height = height;
    }

    public bool isEqual(Spot spot)
    {
        return X == spot.X && Y == spot.Y;
    }

    public void AddNeighbours(Spot[,] grid, int x, int y)
    {
        
        if (x < grid.GetUpperBound(0)) 
            neighbours.Add(grid[x + 1, y]);
        if (x > 0) 
            neighbours.Add(grid[x - 1, y]);
        if (y < grid.GetUpperBound(1)) 
            neighbours.Add(grid[x, y + 1]);
        if (y > 0) 
            neighbours.Add(grid[x, y - 1]);
    
        // Tilemap tiles = RoomPlacer.GetInstance().tilemap;
        // Vector3Int pos = new Vector3Int(x, y), up = new Vector3Int(x + 1, y), down = new Vector3Int(x - 1, y), left = new Vector3Int(x, y - 1), right = new Vector3Int(x, y + 1);

        // Debug.Log($"Positions: ({x}, {y})");
        
        // if (tiles.GetTile(pos) != null)
        // {
        //     Debug.Log(tiles.GetTile(pos));
        //     Debug.Log(tiles.GetInstantiatedObject(pos));
        //     Debug.Log(tiles.GetInstantiatedObject(pos).GetComponent<Room>());
        //     Debug.Log(tiles.GetInstantiatedObject(pos).GetComponent<Room>().room);
        //     Debug.Log(tiles.GetInstantiatedObject(pos).GetComponent<Room>().room is Hallway);
        // }
        // // If this position is within bounds
        // if (x < grid.GetUpperBound(0)) 
        // {
        //     // And either it is a hallway or the adjacent ones are hallways
        //     if ((tiles.GetTile(pos) != null && tiles.GetInstantiatedObject(pos).GetComponent<Room>().room is Hallway) || (tiles.GetTile(up) != null && tiles.GetInstantiatedObject(up).GetComponent<Room>().room is Hallway))
        //     {
        //         neighbours.Add(grid[x + 1, y]);
        //     }
        // }
        // if (x > 0) 
        // {
        //     if ((tiles.GetTile(pos) != null && tiles.GetInstantiatedObject(pos).GetComponent<Room>().room is Hallway) || (tiles.GetTile(down) != null && tiles.GetInstantiatedObject(down).GetComponent<Room>().room is Hallway))
        //     {
        //         neighbours.Add(grid[x - 1, y]);
        //     }
        // }
        // if (y < grid.GetUpperBound(1)) 
        // {
        //     if ((tiles.GetTile(pos) != null && tiles.GetInstantiatedObject(pos).GetComponent<Room>().room is Hallway) || (tiles.GetTile(right) != null && tiles.GetInstantiatedObject(right).GetComponent<Room>().room is Hallway))
        //     {
        //         neighbours.Add(grid[x, y + 1]);
        //     }
        // }
        // if (y > 0) 
        // {
        //     if ((tiles.GetTile(pos) != null && tiles.GetInstantiatedObject(pos).GetComponent<Room>().room is Hallway) || (tiles.GetTile(left) != null && tiles.GetInstantiatedObject(left).GetComponent<Room>().room is Hallway))
        //     {
        //         neighbours.Add(grid[x, y - 1]);
        //     }
        // }
        

    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astar
{
    public Spot[,] spots;

    public Astar(Vector3Int[,] grid, int columns, int rows)
    {
        spots = new Spot[columns, rows];
    }

    private bool IsValidPath(Vector3Int[,] grid, Spot start, Spot end)
    {
        // Debug.Log("Checking for valid path...");
        // Debug.Log(grid);
        // Debug.Log(start);
        // Debug.Log(end);
        // Debug.Log(end.height);
        if (end == null || start == null || end.height >= 1)
        {
            Debug.LogWarning("No valid path");
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
        
        if (!IsValidPath(grid, Start, End))
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

                    // If ir is a newPath, calculate the H and F and set current to the neighbours previous
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
    
        
        /*
        // If this spot is a room, don't add other rooms
        // If either this tile is a hallway or the other tiles are hallways
        AdvancedRuleTile tile = RoomPlacer.GetInstance().tilemap.GetTile<AdvancedRuleTile>(new Vector3Int(x, y));
        if (tile != null)
            Debug.Log($"Spot {tile} ({x}, {y}), is {tile.isHallway}");
        else 
            Debug.Log($"Spot {x}, {y} is null");
        if (tile == null || (tile != null && tile.isHallway))
        {
            if (x < grid.GetUpperBound(0)) 
                neighbours.Add(grid[x + 1, y]);
            if (x > 0) 
                neighbours.Add(grid[x - 1, y]);
            if (y < grid.GetUpperBound(1)) 
                neighbours.Add(grid[x, y + 1]);
            if (y > 0) 
                neighbours.Add(grid[x, y - 1]);
        }
        else
        {
            if (x < grid.GetUpperBound(0)) {
                if (RoomPlacer.GetInstance().tilemap.GetTile<AdvancedRuleTile>(new Vector3Int(x + 1, y)) != null && RoomPlacer.GetInstance().tilemap.GetTile<AdvancedRuleTile>(new Vector3Int(x + 1, y)).isHallway)
                    neighbours.Add(grid[x + 1, y]);
            }
            if (x > 0) {
                if (RoomPlacer.GetInstance().tilemap.GetTile<AdvancedRuleTile>(new Vector3Int(x - 1, y)) != null && RoomPlacer.GetInstance().tilemap.GetTile<AdvancedRuleTile>(new Vector3Int(x - 1, y)).isHallway)
                    neighbours.Add(grid[x - 1, y]);
            }
            if (y < grid.GetUpperBound(1)) {
                if (RoomPlacer.GetInstance().tilemap.GetTile<AdvancedRuleTile>(new Vector3Int(x, y + 1)) != null && RoomPlacer.GetInstance().tilemap.GetTile<AdvancedRuleTile>(new Vector3Int(x, y + 1)).isHallway)
                    neighbours.Add(grid[x, y + 1]);
            }
            if (y > 0) {
                if (RoomPlacer.GetInstance().tilemap.GetTile<AdvancedRuleTile>(new Vector3Int(x, y - 1)) != null && RoomPlacer.GetInstance().tilemap.GetTile<AdvancedRuleTile>(new Vector3Int(x, y - 1)).isHallway)
                    neighbours.Add(grid[x, y - 1]);
            }
        }
        */

    }
}
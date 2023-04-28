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
}

public class Spot
{
    public int X;
    public int Y;
    public int F;
    public int G;
    public int H;
    public int height = 0;
    public List<Spot> nieghbours;
    public Spot previous = null;
    
    public Spot(int x, int y, int height)
    {
        X = x;
        Y = y;
        F = 0;
        G = 0;
        H = 0;
        nieghbours = new List<Spot>();
        this.height = height;
    }
}
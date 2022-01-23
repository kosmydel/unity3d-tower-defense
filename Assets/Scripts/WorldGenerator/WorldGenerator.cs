using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{

    public Vector2 startPosition, endPosition;
    public int height, width;

    private int[,] _map;
    bool[,] visited;

    // Kierunek górny lewo -> prawy doł
    
    // Start is called before the first frame update
    void Start()
    {
        _map = new int[width, height];
        visited = new bool[width, height];
        PrintMap(); 
        MakePath();
        Debug.Log(visited);
        PrintMap();
    }

    void MakePath()
    {
        
        Visit((int)startPosition.x, (int)startPosition.y);
        
    }

    void Visit(int x, int y)
    {
        Debug.Log("Visiting " + x + " / " + y);
        visited[x, y] = true;
        if (x > 0 && !visited[x - 1, y])
        {
            Visit(x-1, y);
        }
        if (x < width-1 && !visited[x + 1, y])
        {
            Visit(x+1, y);
        }
        if (y > 1 && !visited[x, y-1])
        {
            Visit(x, y-1);
        }
        if (y < height -1 && !visited[x, y+1])
        {
            Visit(x, y+1);
        }
    }

    void PrintMap()
    {
        string result = "";
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                result += " " + visited[x, y];
            }

            result += "\n";
        }
        Debug.Log(result);
    }
}

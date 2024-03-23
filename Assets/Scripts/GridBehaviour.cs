using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;

public class GridBehaviour : MonoBehaviour
{
    public GameObject[] inputTiles;
    public GameObject[] inputPlayers;
    private GameObject[,] tiles = new GameObject[6, 6];
    private GameObject redPlayer;
    private GameObject purplePlayer;
    private GameObject greenPlayer;
    private GameObject bluePlayer;
    
    public bool IsRedChamber(int x, int y)
    {
        return tiles[x, y].GetComponent<TileAttribs>().IsRedChamber;
    }
    
    public bool IsPurpleChamber(int x, int y)
    {
        return tiles[x, y].GetComponent<TileAttribs>().IsPurpleChamber;
    }
    
    public bool IsGreenChamber(int x, int y)
    {
        return tiles[x, y].GetComponent<TileAttribs>().IsGreenChamber;
    }
    
    public bool IsBlueChamber(int x, int y)
    {
        return tiles[x, y].GetComponent<TileAttribs>().IsBlueChamber;
    }

    public int MinLength(int x1, int y1, int x2, int y2)
    {
        // BFS
        Queue<Tuple<int, int>> q = new Queue<Tuple<int, int>>();
        bool[,] visited = new bool[6, 6];
        int dist = 0;
        q.Enqueue(new Tuple<int, int>(x1, y1));
        visited[x1, y1] = true;
        while (q.Count > 0)
        {
            int size = q.Count;
            for (int i = 0; i < size; i++)
            {
                Tuple<int, int> cur = q.Dequeue();
                if (cur.Item1 == x2 && cur.Item2 == y2)
                {
                    return dist;
                }
                if (cur.Item1 > 0 && !visited[cur.Item1 - 1, cur.Item2] && CanMove(cur.Item1, cur.Item2, 0)) // up
                {
                    // Debug.Log("Can move up from {" + cur.Item1 + ", " + cur.Item2 + "} to {" + (cur.Item1 - 1) + ", " + cur.Item2 + "}");
                    q.Enqueue(new Tuple<int, int>(cur.Item1 - 1, cur.Item2));
                    visited[cur.Item1 - 1, cur.Item2] = true;
                }
                if (cur.Item2 > 0 && !visited[cur.Item1, cur.Item2 - 1] && CanMove(cur.Item1, cur.Item2, 1)) // left
                {
                    // Debug.Log("Can move left from {" + cur.Item1 + ", " + cur.Item2 + "} to {" + cur.Item1 + ", " + (cur.Item2 - 1) + "}");
                    q.Enqueue(new Tuple<int, int>(cur.Item1, cur.Item2 - 1));
                    visited[cur.Item1, cur.Item2 - 1] = true;
                }
                if (cur.Item1 < 5 && !visited[cur.Item1 + 1, cur.Item2] && CanMove(cur.Item1, cur.Item2, 2)) // down
                {
                    // Debug.Log("Can move down from {" + cur.Item1 + ", " + cur.Item2 + "} to {" + (cur.Item1 + 1) + ", " + cur.Item2 + "}");
                    q.Enqueue(new Tuple<int, int>(cur.Item1 + 1, cur.Item2));
                    visited[cur.Item1 + 1, cur.Item2] = true;
                }
                if (cur.Item2 < 5 && !visited[cur.Item1, cur.Item2 + 1] && CanMove(cur.Item1, cur.Item2, 3)) // right
                {
                    // Debug.Log("Can move right from {" + cur.Item1 + ", " + cur.Item2 + "} to {" + cur.Item1 + ", " + (cur.Item2 + 1) + "}");
                    q.Enqueue(new Tuple<int, int>(cur.Item1, cur.Item2 + 1));
                    visited[cur.Item1, cur.Item2 + 1] = true;
                }
            }
            dist++;
        }
        
        return -1;
    }

    private bool CanMove(int x, int y, int toward) // toward=0 up, 1 left, 2 down, 3 right 
    {
        if (toward == 0) // up
        {
            return tiles[x, y].GetComponent<TileAttribs>().upConnected() && tiles[x - 1, y].GetComponent<TileAttribs>().downConnected();
        }
        else if (toward == 1) // left
        {
            return tiles[x, y].GetComponent<TileAttribs>().leftConnected() && tiles[x, y - 1].GetComponent<TileAttribs>().rightConnected();
        }
        else if (toward == 2) // down
        {
            return tiles[x, y].GetComponent<TileAttribs>().downConnected() && tiles[x + 1, y].GetComponent<TileAttribs>().upConnected();
        }
        else // right
        {
            return tiles[x, y].GetComponent<TileAttribs>().rightConnected() && tiles[x, y + 1].GetComponent<TileAttribs>().leftConnected();
        }
    }
    
    private void RotateTile(int x, int y, bool clockwise)
    {
        if (clockwise)
        {
            tiles[x, y].GetComponent<TileAttribs>().rotate = (tiles[x, y].GetComponent<TileAttribs>().rotate + 3) % 4;
        }
        else
        {
            tiles[x, y].GetComponent<TileAttribs>().rotate = (tiles[x, y].GetComponent<TileAttribs>().rotate + 1) % 4;
        }
    }
    
    private void SwapTiles(int x1, int y1, int x2, int y2)
    {
        Assert.IsTrue(x1 >= 0 && x1 < 6);
        Assert.IsTrue(y1 >= 0 && y1 < 6);
        Assert.IsTrue(x2 >= 0 && x2 < 6);
        Assert.IsTrue(y2 >= 0 && y2 < 6);
        
        Vector3 tempPosition = tiles[x1, y1].transform.position;
        tiles[x1, y1].transform.position = tiles[x2, y2].transform.position;
        tiles[x2, y2].transform.position = tempPosition;
        
        GameObject temp = tiles[x1, y1];
        tiles[x1, y1] = tiles[x2, y2];
        tiles[x2, y2] = temp;
    }

    private void UpdateTileRotations()
    {
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                switch (tiles[i, j].GetComponent<TileAttribs>().rotate)
                {
                    case 0:
                        tiles[i, j].transform.eulerAngles = new Vector3(0, 0, 0);
                        break;
                    case 1:
                        tiles[i, j].transform.eulerAngles = new Vector3(0, 0, 90);
                        break;
                    case 2:
                        tiles[i, j].transform.eulerAngles = new Vector3(0, 0, 180);
                        break;
                    case 3:
                        tiles[i, j].transform.eulerAngles = new Vector3(0, 0, 270);
                        break;
                    default:
                        Debug.LogError("Invalid direction value.");
                        break;
                }
            }
        }
    }
    
    private void UpdatePlayerPositions()
    {
        Vector3 redOffset = new Vector3(-0.15f, -0.15f, -1.0f);
        Vector3 purpleOffset = new Vector3(0.15f, -0.15f, -1.0f);
        Vector3 greenOffset = new Vector3(-0.15f, 0.15f, -1.0f);
        Vector3 blueOffset = new Vector3(0.15f, 0.15f, -1.0f);
        
        redPlayer.transform.position = tiles[redPlayer.GetComponent<PlayerAttribs>().X, redPlayer.GetComponent<PlayerAttribs>().Y].transform.position + redOffset;
        purplePlayer.transform.position = tiles[purplePlayer.GetComponent<PlayerAttribs>().X, purplePlayer.GetComponent<PlayerAttribs>().Y].transform.position + purpleOffset;
        greenPlayer.transform.position = tiles[greenPlayer.GetComponent<PlayerAttribs>().X, greenPlayer.GetComponent<PlayerAttribs>().Y].transform.position + greenOffset;
        bluePlayer.transform.position = tiles[bluePlayer.GetComponent<PlayerAttribs>().X, bluePlayer.GetComponent<PlayerAttribs>().Y].transform.position + blueOffset;
    }

    public bool GetSelectedTile(Vector3 mouseWorldPos, ref int tileX, ref int tileY)
    {
        // Tile is row major
        tileY = (int) Math.Floor((mouseWorldPos.x + 1.5f) / 3.5f);
        tileX = (int) Math.Floor((-mouseWorldPos.y + 1.5f) / 3.5f);
            
        // outside the grid
        if (tileY < 0 || tileY >= 6 || tileX < 0 || tileX >= 6) 
        {
            return false;
        }
        // outside the tile
        if (Math.Abs(tileY * 3.5f - mouseWorldPos.x) > 1.5f || Math.Abs(-tileX * 3.5f - mouseWorldPos.y) > 1.5f) 
        {
            return false;
        }

        return true;
    }
    
    public void PlayerSwapTiles(int x1, int y1, int x2, int y2)
    {
        SwapTiles(x1, y1, x2, y2);
    }
    
    public void PlayerRotateTile(int x, int y, bool clockwise)
    {
        RotateTile(x, y, clockwise);
    }

    public void PlayerPushTiles(bool horizontal, bool positive, int ind)
    {
        Assert.IsTrue(ind >= 0 && ind < 6);
        var pushedTiles = new List<Tuple<int, int>>();
        if (horizontal)
        {
            if (positive)
            {
                for (int i = 0; i < 6; i++)
                {
                    pushedTiles.Add(new Tuple<int, int>(ind, i));
                }
            }
            else
            {
                for (int i = 0; i < 6; i++)
                {
                    pushedTiles.Add(new Tuple<int, int>(ind, 5 - i));
                }
            }
        }
        else
        {
            if (positive)
            {
                for (int i = 0; i < 6; i++)
                {
                    pushedTiles.Add(new Tuple<int, int>(i, ind));
                }
            }
            else
            {
                for (int i = 0; i < 6; i++)
                {
                    pushedTiles.Add(new Tuple<int, int>(5 - i, ind));
                }
            }
        }

        for (int i = 1; i < 6; i++)
        {
            SwapTiles(pushedTiles[0].Item1, pushedTiles[0].Item2, pushedTiles[i].Item1, pushedTiles[i].Item2);
        }

        for (int i = 0; i < 6; i++)
        {
            if (redPlayer.GetComponent<PlayerAttribs>().X == pushedTiles[i].Item1 && 
                redPlayer.GetComponent<PlayerAttribs>().Y == pushedTiles[i].Item2)
            {
                redPlayer.GetComponent<PlayerAttribs>().X = pushedTiles[(i + 1) % 6].Item1;
                redPlayer.GetComponent<PlayerAttribs>().Y = pushedTiles[(i + 1) % 6].Item2;
                break;
            }
        }
        for (int i = 0; i < 6; i++)
        {
            if (purplePlayer.GetComponent<PlayerAttribs>().X == pushedTiles[i].Item1 && 
                purplePlayer.GetComponent<PlayerAttribs>().Y == pushedTiles[i].Item2)
            {
                purplePlayer.GetComponent<PlayerAttribs>().X = pushedTiles[(i + 1) % 6].Item1;
                purplePlayer.GetComponent<PlayerAttribs>().Y = pushedTiles[(i + 1) % 6].Item2;
                break;
            }
        }
        for (int i = 0; i < 6; i++)
        {
            if (greenPlayer.GetComponent<PlayerAttribs>().X == pushedTiles[i].Item1 && 
                greenPlayer.GetComponent<PlayerAttribs>().Y == pushedTiles[i].Item2)
            {
                greenPlayer.GetComponent<PlayerAttribs>().X = pushedTiles[(i + 1) % 6].Item1;
                greenPlayer.GetComponent<PlayerAttribs>().Y = pushedTiles[(i + 1) % 6].Item2;
                break;
            }
        }
        for (int i = 0; i < 6; i++)
        {
            if (bluePlayer.GetComponent<PlayerAttribs>().X == pushedTiles[i].Item1 &&
                bluePlayer.GetComponent<PlayerAttribs>().Y == pushedTiles[i].Item2)
            {
                bluePlayer.GetComponent<PlayerAttribs>().X = pushedTiles[(i + 1) % 6].Item1;
                bluePlayer.GetComponent<PlayerAttribs>().Y = pushedTiles[(i + 1) % 6].Item2;
                break;
            }
        }
    }

    public GameObject GetCurrentPlayer(int ind)
    {
        switch (ind)
        {
            case 0:
                return redPlayer;
            case 1:
                return purplePlayer;
            case 2:
                return greenPlayer;
            case 3:
                return bluePlayer;
            default:
                return null;
        }
    }

    public bool IsOccupied(int x, int y)
    {
        return redPlayer.GetComponent<PlayerAttribs>().X == x && redPlayer.GetComponent<PlayerAttribs>().Y == y ||
               purplePlayer.GetComponent<PlayerAttribs>().X == x && purplePlayer.GetComponent<PlayerAttribs>().Y == y ||
               greenPlayer.GetComponent<PlayerAttribs>().X == x && greenPlayer.GetComponent<PlayerAttribs>().Y == y ||
               bluePlayer.GetComponent<PlayerAttribs>().X == x && bluePlayer.GetComponent<PlayerAttribs>().Y == y ||
               tiles[x, y].GetComponent<TileAttribs>().IsRedChamber ||
               tiles[x, y].GetComponent<TileAttribs>().IsPurpleChamber ||
               tiles[x, y].GetComponent<TileAttribs>().IsGreenChamber ||
               tiles[x, y].GetComponent<TileAttribs>().IsBlueChamber;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        // Debug.Log("GridBehaviour started.");
        
        Assert.AreEqual(inputTiles.Length, 36);
        Assert.AreEqual(inputPlayers.Length, 4);
        for (int i = 0; i < 36; i++)
        {
            int x = i / 6;
            int y = i % 6;
            tiles[x, y] = inputTiles[i];
        }
        redPlayer = inputPlayers[0];
        purplePlayer = inputPlayers[1];
        greenPlayer = inputPlayers[2];
        bluePlayer = inputPlayers[3];
        
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlayerPositions();
        UpdateTileRotations();
    }
}

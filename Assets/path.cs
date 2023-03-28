using UnityEngine;
using System.Collections.Generic;

public class path : MonoBehaviour
{
    public GameObject wallPrefab;
    public float cellSize = 2f;
    public int mazeWidth = 20;
    public int mazeHeight = 20;

    private bool[,] visited;
    private List<GameObject> walls = new List<GameObject>();

    private void Start()
    {
        visited = new bool[mazeWidth, mazeHeight];
        GenerateMaze(0, 0);
    }

    private void GenerateMaze(int x, int y)
    {
        visited[x, y] = true;

        // Generate walls for this cell
        Vector3 cellPos = new Vector3(x * cellSize, 0, y * cellSize);
        GameObject rightWall = Instantiate(wallPrefab, cellPos + new Vector3(cellSize / 2, 0, 0), Quaternion.identity);
        walls.Add(rightWall);
        GameObject leftWall = Instantiate(wallPrefab, cellPos + new Vector3(-cellSize / 2, 0, 0), Quaternion.identity);
        walls.Add(leftWall);
        GameObject topWall = Instantiate(wallPrefab, cellPos + new Vector3(0, 0, cellSize / 2), Quaternion.identity);
        walls.Add(topWall);
        GameObject bottomWall = Instantiate(wallPrefab, cellPos + new Vector3(0, 0, -cellSize / 2), Quaternion.identity);
        walls.Add(bottomWall);

        // Generate neighboring cells
        List<int> directions = new List<int> { 0, 1, 2, 3 };
        while (directions.Count > 0)
        {
            int index = Random.Range(0, directions.Count);
            int direction = directions[index];
            directions.RemoveAt(index);

            int nextX = x;
            int nextY = y;

            if (direction == 0 && x > 0 && !visited[x - 1, y]) // left
            {
                nextX--;
                GameObject.Destroy(leftWall);
            }
            else if (direction == 1 && x < mazeWidth - 1 && !visited[x + 1, y]) // right
            {
                nextX++;
                GameObject.Destroy(rightWall);
            }
            else if (direction == 2 && y > 0 && !visited[x, y - 1]) // down
            {
                nextY--;
                GameObject.Destroy(bottomWall);
            }
            else if (direction == 3 && y < mazeHeight - 1 && !visited[x, y + 1]) // up
            {
                nextY++;
                GameObject.Destroy(topWall);
            }

            if (nextX != x || nextY != y)
            {
                GenerateMaze(nextX, nextY);
            }
        }
    }

    private void OnDestroy()
    {
        foreach (GameObject wall in walls)
        {
            Destroy(wall);
        }
    }
}

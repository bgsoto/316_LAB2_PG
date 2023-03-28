using System.Collections;
using System.Collections.Generic;
using UnityEngine;






[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]



//public GameObject objectPrefab;




public class level2 : MonoBehaviour
{
    // The width and height of the terrain mesh
    public int width = 100;
    public int height = 100;

    // The scale of the Perlin noise used to generate the terrain
    public float scale = 20.0f;

    // The maximum and minimum heights of the terrain
    public float maxHeight = 10.0f;
    public float minHeight = -10.0f;

    // The texture used to define the colors of the terrain
    public Texture2D colorMap;

    // The GameObject to spawn on the terrain
    public GameObject prefab;

    // The number of GameObjects to spawn
    public int numObjects = 40;

    private Vector3[] vertices;
    private Color[] colors;
    private int[] triangles;
    private List<GameObject> spawnedObjects = new List<GameObject>();

    void Start()
    {

        // Get the MeshFilter and MeshRenderer components on this GameObject
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();

        // Create a new mesh
        Mesh mesh = new Mesh();

        // Create arrays to hold the vertices, colors, and triangles of the mesh
        vertices = new Vector3[width * height];
        colors = new Color[width * height];
        triangles = new int[(width - 1) * (height - 1) * 6];
        int triangleIndex = 0;

        // Loop through each vertex in the mesh
        for (int z = 0; z < height; z++)
        {
            for (int x = 0; x < width; x++)
            {
                // Calculate the height of the vertex using Perlin noise
                float y = Mathf.PerlinNoise((float)x / width * scale, (float)z / height * scale);

                // Scale the height to fit within the specified range
                y = Mathf.Lerp(minHeight, maxHeight, y);

                // Set the position of the vertex
                vertices[z * width + x] = new Vector3(x, y, z);

                // Get the color of the current pixel in the color map
                Color color = colorMap.GetPixel(x % colorMap.width, z % colorMap.height);

                // Set the color of the vertex
                colors[z * width + x] = color;

                // If the vertex is not on the edge of the mesh, create two triangles using it
                if (x > 0 && z > 0)
                {
                    int a = (z - 1) * width + x - 1;
                    int b = (z - 1) * width + x;
                    int c = z * width + x;
                    int d = z * width + x - 1;

                    triangles[triangleIndex++] = a;
                    triangles[triangleIndex++] = b;
                    triangles[triangleIndex++] = c;

                    triangles[triangleIndex++] = c;
                    triangles[triangleIndex++] = d;
                    triangles[triangleIndex++] = a;
                }
            }
        }

        // Set the vertices, colors, and triangles of the mesh and recalculate its normals
        mesh.vertices = vertices;
        mesh.colors = colors;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        // Set the mesh on the MeshFilter component
        meshFilter.mesh = mesh;

        // Set the color map on the MeshRenderer component
        meshRenderer.material.mainTexture = colorMap;

        // Generate a random map or maze to create a path through the terrain
        GeneratePath();

        float minDistanceBetweenObjects = .05f;
        SpawnObjects(numObjects, minDistanceBetweenObjects);

        // SpawnObjects(numObjects);

    }



    private void GeneratePath()
    {
        // Create a list of visited points on the path
        List<Vector3> pathPoints = new List<Vector3>();

        // Choose a random starting point on the terrain
        int startX = Random.Range(0, width);
        int startZ = Random.Range(0, height);

        // Add the starting point to the visited list
        pathPoints.Add(vertices[startZ * width + startX]);

        // Loop until all points on the path have been visited
        while (pathPoints.Count < 100)
        {
            // Find the nearest neighbor of the last visited point on the path
            Vector3 lastPoint = pathPoints[pathPoints.Count - 1];
            float minDistance = float.MaxValue;
            Vector3 nearestNeighbor = Vector3.zero;

            for (int z = 0; z < height; z++)
            {
                for (int x = 0; x < width; x++)
                {
                    Vector3 currentPoint = vertices[z * width + x];
                    if (!pathPoints.Contains(currentPoint))
                    {
                        float distance = Vector3.Distance(lastPoint, currentPoint);
                        if (distance < minDistance)
                        {
                            minDistance = distance;
                            nearestNeighbor = currentPoint;
                        }
                    }
                }
            }

            // Add the nearest neighbor to the visited list and color it to show the path
            pathPoints.Add(nearestNeighbor);
            int index = System.Array.IndexOf(vertices, nearestNeighbor);

            // Make sure the index is valid before coloring the vertex
            if (index >= 0 && index < colors.Length)
            {
                colors[index] = Color.HSVToRGB(0.1f, 0.8f, 0.4f);
            }
        }

        // Update the mesh with the new colors
        //mesh.colors = colors;
    }

    private void SpawnObjects(int count, float minDistance)
    {
        int maxAttempts = 100;

        for (int i = 0; i < count; i++)
        {
            bool validPosition = false;
            int attempts = 0;
            Vector3 position = Vector3.zero;

            while (!validPosition && attempts < maxAttempts)
            {
                // Choose a random position on the terrain
                int x = Random.Range(0, width);
                int z = Random.Range(0, height);
                position = vertices[z * width + x];

                validPosition = true;

                // Check the distance to all previously spawned objects
                foreach (GameObject spawnedObject in spawnedObjects)
                {
                    if (Vector3.Distance(position, spawnedObject.transform.position) < minDistance)
                    {
                        validPosition = false;
                        break;
                    }
                }

                attempts++;
            }

            // Instantiate the prefab at the chosen position if it's valid and add it to the list of spawned objects
            if (validPosition)
            {
                GameObject spawnedObject = Instantiate(prefab, position, Quaternion.identity);
                spawnedObjects.Add(spawnedObject);
            }
        }
    }
}



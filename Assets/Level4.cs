using System.Collections;
using System.Collections.Generic;
using UnityEngine;






[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]


public class Level4 : MonoBehaviour
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

    public GameObject objectPrefab;

    void Start()
    {
        // Get the MeshFilter and MeshRenderer components on this GameObject
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();

        // Create a new mesh
        Mesh mesh = new Mesh();

        // Create arrays to hold the vertices, colors, and triangles of the mesh
        Vector3[] vertices = new Vector3[width * height];
        Color[] colors = new Color[width * height];
        int[] triangles = new int[(width - 1) * (height - 1) * 6];
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
            SpawnObjectOnTerrain();
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
    }
    void SpawnObjectOnTerrain()
    {
        if (objectPrefab == null)
        {
            Debug.LogError("No object prefab assigned.");
            return;
        }
        // Choose a random position on the terrain
        int x = Random.Range(0, width);
        int z = Random.Range(0, height);

        // Get the height of the terrain at the chosen position
        float y = Mathf.PerlinNoise((float)x / width * scale, (float)z / height * scale);
        y = Mathf.Lerp(minHeight, maxHeight, y);

        // Instantiate the prefab at the chosen position
        Vector3 position = new Vector3(x, y, z);
        Instantiate(objectPrefab, position, Quaternion.identity);
    }
}


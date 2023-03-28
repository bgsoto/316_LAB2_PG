using UnityEngine;

public class TerrainChunk : MonoBehaviour
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

    public void Generate(Vector2 position)
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
                float y = Mathf.PerlinNoise((position.x + x) / width * scale, (position.y + z) / height * scale);

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
        
    }
}

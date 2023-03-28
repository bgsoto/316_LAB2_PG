using System.Collections.Generic;
using UnityEngine;

public class level5 : MonoBehaviour
{
    public GameObject player;
    public TerrainChunk terrainChunkPrefab;
    public int chunkSize = 100;
    public int viewDistance = 200;

    private Dictionary<Vector2, TerrainChunk> terrainChunks = new Dictionary<Vector2, TerrainChunk>();

    void Update()
    {
        LoadTerrainChunks();
    }

    void LoadTerrainChunks()
    {
        Vector2 playerChunkPosition = new Vector2(Mathf.Floor(player.transform.position.x / chunkSize), Mathf.Floor(player.transform.position.z / chunkSize));

        for (int x = -viewDistance / chunkSize; x < viewDistance / chunkSize; x++)
        {
            for (int z = -viewDistance / chunkSize; z < viewDistance / chunkSize; z++)
            {
                Vector2 chunkPosition = playerChunkPosition + new Vector2(x, z);

                if (Vector2.Distance(playerChunkPosition, chunkPosition) * chunkSize < viewDistance)
                {
                    if (!terrainChunks.ContainsKey(chunkPosition))
                    {
                        TerrainChunk newChunk = Instantiate(terrainChunkPrefab, new Vector3(chunkPosition.x * chunkSize, 0, chunkPosition.y * chunkSize), Quaternion.identity);
                        newChunk.Generate(chunkPosition);
                        terrainChunks.Add(chunkPosition, newChunk);
                    }
                }
                else
                {
                    if (terrainChunks.ContainsKey(chunkPosition))
                    {
                        Destroy(terrainChunks[chunkPosition].gameObject);
                        terrainChunks.Remove(chunkPosition);
                    }
                }
            }
        }
    }
}

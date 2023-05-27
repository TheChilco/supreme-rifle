using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    public int width = 64;
    public int depth = 64;
    public float heightScale = 5f;
    public float detailScale = 5f;
    public GameObject voxelPrefab; // Assign a Cube prefab from the inspector

    private float xOffset, zOffset;

    void Start()
    {
        xOffset = Random.Range(0f, 9999f);
        zOffset = Random.Range(0f, 9999f);

        for (int z = 0; z < depth; z++)
        {
            for (int x = 0; x < width; x++)
            {
                Vector2Int chunkCoord = currentChunkCoord + new Vector2Int(x, z);
                if (!chunks.ContainsKey(chunkCoord))
                {
                    GenerateChunk(chunkCoord);
                }
            }
        }

        // Unload chunks that are outside the render distance
        List<Vector2Int> chunksToRemove = new List<Vector2Int>();
        foreach (var chunk in chunks)
        {
            if (Vector2Int.Distance(currentChunkCoord, chunk.Key) > renderDistance)
            {
                chunksToRemove.Add(chunk.Key);
            }
        }

        foreach (var chunkCoord in chunksToRemove)
        {
            Destroy(chunks[chunkCoord]);
            chunks.Remove(chunkCoord);
        }
    }

    void GenerateChunk(Vector2Int coord)
    {
        GameObject chunk = new GameObject("Chunk " + coord.x + ", " + coord.y);
        chunk.transform.parent = transform;

        for (int z = 0; z < chunkSize; z++)
        {
            for (int x = 0; x < chunkSize; x++)
            {
                float y = Mathf.PerlinNoise((x + coord.x * chunkSize + xOffset) / detailScale, (z + coord.y * chunkSize + zOffset) / detailScale) * heightScale;
                int yRounded = Mathf.FloorToInt(y);

                Vector3 position = new Vector3(x + coord.x * chunkSize, yRounded, z + coord.y * chunkSize);
                Instantiate(voxelPrefab, position, Quaternion.identity, chunk.transform);
            }
        }

        chunks.Add(coord, chunk);
    }
}

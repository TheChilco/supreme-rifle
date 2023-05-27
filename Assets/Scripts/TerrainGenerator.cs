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

    bool CheckVisibility(int x, int y, int z, Vector2Int coord)
    {
        // Check if any neighboring voxel is present and if it is higher than the current voxel
        int[] offsets = { -1, 1 };
        foreach (int xOffset in offsets)
        {
            Vector3Int neighborPosition = new Vector3Int(x + xOffset, y, z);
            if (!IsVoxelPresent(neighborPosition.x, neighborPosition.y, neighborPosition.z, coord))
                return true;
            if (GetVoxelHeight(neighborPosition.x, neighborPosition.y, neighborPosition.z, coord) <= y)
                return true;
        }
        foreach (int zOffset in offsets)
        {
            Vector3Int neighborPosition = new Vector3Int(x, y, z + zOffset);
            if (!IsVoxelPresent(neighborPosition.x, neighborPosition.y, neighborPosition.z, coord))
                return true;
            if (GetVoxelHeight(neighborPosition.x, neighborPosition.y, neighborPosition.z, coord) <= y)
                return true;
        }

        return false;
    }

    bool IsVoxelPresent(int x, int y, int z, Vector2Int coord)
    {
        // Check if the neighboring voxel is present in the chunk or in neighboring chunks
        if (x >= 0 && x < chunkSize && z >= 0 && z < chunkSize)
        {
            // Check within the current chunk
            return true; // Modify this logic based on your actual voxel data structure or source
        }
        else
        {
            // Check in neighboring chunks
            Vector2Int neighborChunkCoord = new Vector2Int(coord.x, coord.y);
            if (x < 0)
                neighborChunkCoord.x--;
            else if (x >= chunkSize)
                neighborChunkCoord.x++;

            if (z < 0)
                neighborChunkCoord.y--;
            else if (z >= chunkSize)
                neighborChunkCoord.y++;

            // Check within the neighboring chunk
            if (chunks.ContainsKey(neighborChunkCoord))
            {
                GameObject neighborChunk = chunks[neighborChunkCoord];
                foreach (Transform voxel in neighborChunk.transform)
                {
                    // Modify this logic based on your actual voxel data structure or source
                    Vector3Int voxelLocalPosition = Vector3Int.RoundToInt(voxel.localPosition);
                    if (voxelLocalPosition.x == x && voxelLocalPosition.y == y && voxelLocalPosition.z == z)
                        return true;
                }
            }
        }

        return false;
    }

    int GetVoxelHeight(int x, int y, int z, Vector2Int coord)
    {
        // Implement your logic to retrieve the height of the neighboring voxel
        // Modify this based on your actual voxel data structure or source
        return 0;
    }

}

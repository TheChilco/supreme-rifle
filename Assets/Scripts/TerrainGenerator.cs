using UnityEngine;
using System.Collections.Generic;

public class TerrainGenerator : MonoBehaviour
{
    public int chunkSize = 16;
    public float heightScale = 5f;
    public float detailScale = 5f;
    public GameObject voxelPrefab; // Assign a Cube prefab from the inspector

    private float xOffset, zOffset;
    private Dictionary<Vector2Int, GameObject> chunks = new Dictionary<Vector2Int, GameObject>();

    // Assume this is your PlayerController script which holds the render distance
    public PlayerController playerController;
    public GameObject player;

    void Start()
    {
        xOffset = Random.Range(0f, 9999f);
        zOffset = Random.Range(0f, 9999f);
    }

    void Update()
    {
        // Calculate the chunk coordinate based on the player's position, not the TerrainGenerator's position
        Vector3 playerPosition = playerController.transform.position;
        Vector2Int currentChunkCoord = new Vector2Int(Mathf.FloorToInt(playerPosition.x / chunkSize), Mathf.FloorToInt(playerPosition.z / chunkSize));

        // Get the render distance from the PlayerController
        int renderDistance = playerController.renderDistance;

        // Generate chunks in a square grid around the player, based on the render distance
        for (int z = -renderDistance; z <= renderDistance; z++)
        {
            for (int x = -renderDistance; x <= renderDistance; x++)
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

        Camera mainCamera = Camera.main;
        Transform playerTransform = playerController.transform; // Replace "playerController" with the actual reference to your player controller script or component

        // Get the visibility bounds based on the camera frustum
        Plane[] frustumPlanes = GeometryUtility.CalculateFrustumPlanes(mainCamera);

        for (int z = 0; z < chunkSize; z++)
        {
            for (int x = 0; x < chunkSize; x++)
            {
                float y = Mathf.PerlinNoise((x + coord.x * chunkSize + xOffset) / detailScale, (z + coord.y * chunkSize + zOffset) / detailScale) * heightScale;
                int yRounded = Mathf.FloorToInt(y);

                // Check if the voxel is visible to the camera and within the player's view
                Vector3 position = new Vector3(x + coord.x * chunkSize, yRounded, z + coord.y * chunkSize);
                if (IsVoxelVisible(position, frustumPlanes) && IsVoxelWithinPlayerView(position, playerTransform))
                {
                    for (int yIndex = 0; yIndex <= yRounded; yIndex++)
                    {
                        position = new Vector3(x + coord.x * chunkSize, yIndex, z + coord.y * chunkSize);
                        Instantiate(voxelPrefab, position, Quaternion.identity, chunk.transform);
                    }
                }
            }
        }

        chunks.Add(coord, chunk);
    }

    bool IsVoxelVisible(Vector3 position, Plane[] frustumPlanes)
    {
        // Check if the voxel is visible to the camera based on the frustum planes
        return GeometryUtility.TestPlanesAABB(frustumPlanes, new Bounds(position, Vector3.one));
    }

    bool IsVoxelWithinPlayerView(Vector3 position, Transform playerTransform)
    {
        // Check if the voxel is within the player's view
        Vector3 playerToVoxel = position - playerTransform.position;
        Camera playerCamera = playerTransform.GetChild(0).GetComponent<Camera>(); // Assuming the player's camera is the first child of the player object
        float angle = Vector3.Angle(playerTransform.forward, playerToVoxel);
        return angle <= playerCamera.fieldOfView;
    }

}

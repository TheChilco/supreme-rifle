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
                float y = Mathf.PerlinNoise((x + xOffset) / detailScale, (z + zOffset) / detailScale) * heightScale;
                int yRounded = Mathf.FloorToInt(y);

                // Create voxel for each height level
                for (int i = 0; i <= yRounded; i++)
                {
                    Vector3 position = new Vector3(x, i, z);
                    Instantiate(voxelPrefab, position, Quaternion.identity, transform);
                }
            }
        }
    }
}

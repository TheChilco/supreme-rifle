using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshCollider), typeof(MeshRenderer))]
public class TerrainGenerator : MonoBehaviour
{
    public int width = 256;
    public int depth = 256;
    public float heightScale = 20f;
    public float detailScale = 25f;
    public Material terrainMaterial; // Assign this from the inspector
    public float voxelSize = 3f; // The size of a single "voxel"

    private float xOffset, zOffset;

    void Start()
    {
        xOffset = Random.Range(0f, 9999f);
        zOffset = Random.Range(0f, 9999f);

        MeshFilter terrainMesh = this.GetComponent<MeshFilter>();
        MeshCollider terrainCollider = this.GetComponent<MeshCollider>();
        MeshRenderer terrainRenderer = this.GetComponent<MeshRenderer>();
        
        Mesh mesh = new Mesh();
        mesh.name = "Procedural Terrain";
        terrainMesh.mesh = mesh;

        Vector3[] vertices = new Vector3[width * depth];
        int[] triangles = new int[(width - 1) * (depth - 1) * 6];
        Vector2[] uvs = new Vector2[vertices.Length];

        int triangleIndex = 0;
        float halfWidth = width / 2f;
        float halfDepth = depth / 2f;

        for (int z = 0; z < depth; z++)
        {
            for (int x = 0; x < width; x++)
            {
                int vertexIndex = z * width + x;
                float y = Mathf.PerlinNoise((x + halfWidth + xOffset) / detailScale, (z + halfDepth + zOffset) / detailScale) * heightScale;
                
                // Snap y to the nearest voxelSize increment to create a voxel effect
                y = Mathf.Round(y / voxelSize) * voxelSize;

                vertices[vertexIndex] = new Vector3(x - halfWidth, y, z - halfDepth);
                uvs[vertexIndex] = new Vector2(x / (float)width, z / (float)depth);

                if (x < width - 1 && z < depth - 1)
                {
                    triangles[triangleIndex] = vertexIndex;
                    triangles[triangleIndex + 1] = vertexIndex + width;
                    triangles[triangleIndex + 2] = vertexIndex + width + 1;

                    triangles[triangleIndex + 3] = vertexIndex;
                    triangles[triangleIndex + 4] = vertexIndex + width + 1;
                    triangles[triangleIndex + 5] = vertexIndex + 1;
                    triangleIndex += 6;
                }
            }
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();

        terrainCollider.sharedMesh = mesh;
        terrainRenderer.material = terrainMaterial;
    }
}

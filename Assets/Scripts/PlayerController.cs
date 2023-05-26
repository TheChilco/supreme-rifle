using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public TerrainGenerator terrainGenerator;

    void Start()
    {
        SetTerrainHeight();
    }

    void Update()
    {
        //Every Frame
    }

    private void SetTerrainHeight()
    {
        // Get the terrain height at the player's x and z coordinates
        float terrainHeight = GetTerrainHeight(transform.position.x, transform.position.z);

        // Set the player's y position to the terrain height plus some offset (e.g., 1 unit)
        transform.position = new Vector3(transform.position.x, terrainHeight + 1, transform.position.z);
    }

    float GetTerrainHeight(float x, float z)
    {
        // Ensure x and z are within terrain boundaries
        x = Mathf.Clamp(x, 0, terrainGenerator.width - 1);
        z = Mathf.Clamp(z, 0, terrainGenerator.depth - 1);

        // Get terrain height at x and z
        float perlin = Mathf.PerlinNoise(x / terrainGenerator.detailScale, z / terrainGenerator.detailScale);
        float y = Mathf.Pow(perlin, 2) * terrainGenerator.heightScale; // taking the square of the Perlin noise output

        return y;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public TerrainGenerator terrainGenerator;
    public float maxRaycastDistance = 1000f;
    public int renderDistance = 10;

    void Start()
    {
        PositionPlayerOnTerrain();
    }

    void Update()
    {
        //Every Frame
    }

    private void PositionPlayerOnTerrain()
    {
        // First, we'll try raycasting downwards
        Ray downRay = new Ray(transform.position, -Vector3.up);
        RaycastHit hitInfo;
        if (Physics.Raycast(downRay, out hitInfo, maxRaycastDistance))
        {
            // If we hit something, move the player to the surface
            transform.position = hitInfo.point;
        }
        else
        {
            // If we didn't hit anything downwards, try upwards
            Ray upRay = new Ray(transform.position, Vector3.up);
            if (Physics.Raycast(upRay, out hitInfo, maxRaycastDistance))
            {
                // If we hit something, move the player to the surface
                transform.position = hitInfo.point;
            }
        }
    }
}
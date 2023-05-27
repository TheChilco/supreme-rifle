using UnityEngine;

public class Voxel : MonoBehaviour
{
    public bool[] renderFaces = new bool[6]; // One for each face (up, down, left, right, front, back)

    // This function will be called after the voxel is instantiated, it'll disable faces that should not be rendered
    public void UpdateFaces()
    {
        for (int i = 0; i < 6; i++)
        {
            transform.GetChild(i).gameObject.SetActive(renderFaces[i]);
        }
    }
}

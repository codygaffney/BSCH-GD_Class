using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialChange : MonoBehaviour
{
    // List to hold all materials
    private List<Material> materials = new List<Material>();

    // Start is called before the first frame update
    void Start()
    {
        // Find all MeshRenderers in this GameObject and its children
        MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();

        // Iterate over each renderer and add its materials to the list
        foreach (MeshRenderer renderer in renderers)
        {
            foreach (Material mat in renderer.materials) // Access all materials of the renderer
            {
                materials.Add(mat);
            }
        }
    }

    // This method is called when the GameObject collides with another GameObject
    public void OnCollisionEnter(Collision collision)
    {
        // Change the color of each material in the list
        foreach (Material mat in materials)
        {
            mat.color = Random.ColorHSV(); // Set to a random color
        }
    }
}

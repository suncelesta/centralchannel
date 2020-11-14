using UnityEngine;

public class SideCanal : MonoBehaviour
{
    public Material[] redMaterials;
    public Material[] whiteMaterials;

    public void MakeRed()
    {
        SetMaterials(redMaterials);
    }

    public void MakeWhite()
    {
        SetMaterials(whiteMaterials);
    }
    
    private void SetMaterials(Material[] materials)
    {
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.materials = materials;
    }
}

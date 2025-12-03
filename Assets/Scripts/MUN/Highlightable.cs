using UnityEngine;
using System.Collections.Generic;

public class Highlightable : MonoBehaviour
{
    private Renderer myRenderer;
    private Material[] originalMaterials; 

    void Awake()
    {

        myRenderer = GetComponentInChildren<Renderer>();
        if (myRenderer != null)
        {

            originalMaterials = myRenderer.materials;
        }
    }

    // F키 눌렀을 때 아웃라인 추가
    public void Highlight(Material outlineMat)
    {
        if (myRenderer == null) return;

        List<Material> matList = new List<Material>(myRenderer.materials);


        if (!matList.Contains(outlineMat))
        {
            matList.Add(outlineMat); 
            myRenderer.materials = matList.ToArray(); 
        }
    }

    public void Unhighlight()
    {
        if (myRenderer == null) return;

        myRenderer.materials = originalMaterials;
    }
}
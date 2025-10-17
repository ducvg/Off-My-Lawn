using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class MeshCombiner : MonoBehaviour
{
    public MeshFilter[] meshFilters;
    public string excludeObjectName = "Grass";
    public string unparentObjectName = "Grass";

    [ContextMenu("Combine")]
    void Combine()
    {
        CombineInstance[] instances = new CombineInstance[meshFilters.Length];

        for (int i = 0; i < meshFilters.Length; i++)
        {
            var meshFilter = meshFilters[i];
            if(meshFilter == null || meshFilter.sharedMesh == null)
                continue;
            var renderer = meshFilters[i].GetComponent<MeshRenderer>();
            instances[i] = new CombineInstance
            {
                mesh = meshFilter.sharedMesh,
                transform = meshFilter.transform.localToWorldMatrix,
            };

            meshFilter.gameObject.SetActive(false);
        }

        Mesh combinedMesh = new Mesh();
        combinedMesh.CombineMeshes(instances);
        gameObject.GetComponent<MeshFilter>().sharedMesh = combinedMesh;
        gameObject.SetActive(true);
    }

    [ContextMenu("Fetch meshFilters")]
    void FetchMeshFilters()
    {
        meshFilters = GetComponentsInChildren<MeshFilter>();
        List<MeshFilter> filtered = new List<MeshFilter>();
        foreach (var meshFilter in meshFilters)
        {
            if (meshFilter.gameObject.name != excludeObjectName)
                filtered.Add(meshFilter);
        }
        meshFilters = filtered.ToArray();
    }

    [ContextMenu("Enable all children")]
    void EnableAllChildren()
    {
        meshFilters = GetComponentsInChildren<MeshFilter>(includeInactive: true);
        foreach (var meshFilter in meshFilters)
        {
            meshFilter.gameObject.SetActive(true);
        }
    }

    [ContextMenu("Unparent children")]
    void UnparentChildren()
    {
        foreach (var meshFilter in meshFilters)
        {
            if (meshFilter.gameObject.name == unparentObjectName)
                meshFilter.transform.parent = transform;
        }
    }
}
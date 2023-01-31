using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshBakerObject : MonoBehaviour
{
    public NavMeshSurface[] surfaces;
    void Start()
    {
        //UpdateMesh();
    }

    public void UpdateMesh()
    {
        for (int i = 0; i < surfaces.Length; i++) 
        {
            surfaces [i].BuildNavMesh ();    
        }    
    }
}

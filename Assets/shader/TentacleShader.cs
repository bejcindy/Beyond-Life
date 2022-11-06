using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentacleShader : MonoBehaviour
{
    [SerializeField] Color color_1;

    [SerializeField] Color color_2;

    [SerializeField] MeshRenderer[] CubeMat;

    public float smoothness = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        for ( int i = 0; i < CubeMat.Length; i++)
        {
            smoothness += 1f / CubeMat.Length;
            CubeMat[i].material.color = Color.Lerp(color_1, color_2, smoothness);  
        }
             
    }

    
}

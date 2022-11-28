using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelOneNPC : MonoBehaviour
{

    public int spawnGauge = 0;
    // Start is called before the first frame update
    void Start()
    {
        spawnGauge = Random.Range(0, 5);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

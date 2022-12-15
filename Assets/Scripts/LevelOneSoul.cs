using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelOneSoul : MonoBehaviour
{
    public GameObject core;
    public bool arrivedAtPort = false;
    // Start is called before the first frame update
    void Start()
    {
        core = GameObject.Find("level1_orb");
    }

    // Update is called once per frame
    void Update()
    {
        if (arrivedAtPort)
        {
            StartCoroutine(lerpToCore(core.transform.position, 10.0f));
        }
    }

    IEnumerator lerpToCore(Vector3 corePos, float duration)
    {
        GetComponent<ParticleSystem>().Play();
        float time = 0;
        Vector3 startPos = transform.position;
        while(time < duration)
        {
            transform.position = Vector3.Lerp(startPos, corePos, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = corePos;
    }
}

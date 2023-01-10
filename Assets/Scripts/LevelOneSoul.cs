using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelOneSoul : MonoBehaviour
{
    public GameObject core;
    public bool arrivedAtPort = false;

    Vector3 originalScale;
    public float targetScaleMulti;
    float scaleModifier = 1;


    // Start is called before the first frame update
    void Start()
    {
        core = GameObject.Find("level1_orb");
        originalScale = gameObject.transform.localScale;
        targetScaleMulti = 10f;
    }

    // Update is called once per frame
    void Update()
    {
        if (arrivedAtPort)
        {
            StartCoroutine(lerpToCore(core.transform.position, 10.0f));
        }
        //if(transform.parent == null)
        //{
        //    if (LevelOneFirstCheck.soulCount >= 8)
        //    {
        //        if (transform.localScale.x < originalScale.x * targetScaleMulti)
        //        {
        //            StartCoroutine(soulGrow());
        //        }
        //    }
        //}
        //else if(gameObject.transform.parent.gameObject.tag == "Receiver")
        //{
        //    StartCoroutine(lockSoul());
        //}
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

    public IEnumerator SoulGrow(float endVal, float duration)
    {
        float time = 0;
        float startValue = scaleModifier;
        Vector3 startScale = transform.localScale;
        while(time < duration)
        {
            scaleModifier = Mathf.Lerp(startValue, endVal, time / duration);
            transform.localScale = startScale *  scaleModifier;
            time += Time.deltaTime;
            yield return null;
        }

        transform.localScale = startScale * endVal;
        scaleModifier = endVal;

    }



    IEnumerator lockSoul()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, originalScale, Time.deltaTime * 2f);
        yield return null;
    }


}

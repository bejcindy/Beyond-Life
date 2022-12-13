using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L1SoulLock : MonoBehaviour
{
    public bool hasSoul = false;
    public GameObject myBase;
    public GameObject myPort;
    public GameObject mySoul;
    public GameObject player;

    public bool hasPlayer;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        myBase = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(player.GetComponent<CurvePlayerController>().toPort)
        {
            if (hasPlayer)
            {
                player.transform.parent = transform;
            }
            StartCoroutine(lerpToPort(myPort.transform.position, 30.0f));

        }
        if(Vector3.Distance(transform.position, myPort.transform.position) < 0.5f)
        {
            if(mySoul != null)
            {
                mySoul.GetComponent<LevelOneSoul>().arrivedAtPort = true;
            }
            
        }
    }

    IEnumerator lerpToPort(Vector3 portPos, float duration)
    {
        float time = 0;
        Vector3 startPos = transform.position;
        while(time < duration)
        {
            transform.position = Vector3.Lerp(startPos, portPos, time/duration);
            time += Time.deltaTime;
            yield return null;
        }

        transform.position = portPos;
    }

    public void unlockAll()
    {
        myBase.transform.parent = null;
        if(mySoul != null)
        {
            mySoul.transform.parent = null;
        }

    }

    public void lockAll()
    {
        myBase.transform.parent = transform;
        if(mySoul != null)
        {
            mySoul.transform.parent = transform;
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name == "Player")
        {
            hasPlayer = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.name == "Player")
        {
            hasPlayer = false;
        }
    }
}

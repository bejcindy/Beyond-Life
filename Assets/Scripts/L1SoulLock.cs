using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L1SoulLock : MonoBehaviour
{
    public bool hasSoul = false;
    public GameObject myBoard;
    public GameObject myPort;

    public GameObject player;

    public bool hasPlayer;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(player.GetComponent<CurvePlayerController>().toPort)
        {
            if (hasPlayer)
            {
                player.transform.parent = myBoard.transform;
            }
            StartCoroutine(lerpToPort(myPort.transform.position, 30.0f));

        }
    }

    IEnumerator lerpToPort(Vector3 portPos, float duration)
    {
        float time = 0;
        Vector3 startPos = myBoard.transform.position;
        while(time < duration)
        {
            myBoard.transform.position = Vector3.Lerp(startPos, portPos, time/duration);
            time += Time.deltaTime;
            yield return null;
        }

        myBoard.transform.position = portPos;
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

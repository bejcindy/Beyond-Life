using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelOneFirstCheck   : MonoBehaviour
{
    public GameObject player;
    public GameObject checkKey;
    public GameObject mainTrack;
    //public GameObject[] cpLights;
    //public GameObject wakeUpLogic;

    public bool inCheck = false;
    public bool inSecondCheck = false;

    public float holdTime = 10.0f;
    public float holdFailCountDown = 2.0f;
    public float holdTimeSpeed = 2.0f;

    //public float pressCD = 3.0f;
    //float pressCDVal;
    public static int unpressedAmt = 0;

    //new stuff
    public bool keyPressed = false;
    public GameObject myTunnel;
    public GameObject nextDoor;
    public Material currentMat;


    //New Logic


    public bool failedFirstCheck = false;
    public bool failedSecondCheck = false;

    [SerializeField] Material greenMat;
    [SerializeField] Material yellowMat;
    [SerializeField] Material redMat;
    [SerializeField] AudioClip greenSound;
    [SerializeField] AudioClip yellowSound;
    [SerializeField] AudioClip redSound;

    AudioSource AS;
    // Start is called before the first frame update
    void Start()
    {
        //pressCDVal = pressCD;
        AS = GetComponent<AudioSource>();
        currentMat = greenMat;
        mainTrack = GameObject.Find("MainTrack");
    }

    // Update is called once per frame
    void Update()
    {

        //if (Input.anyKey)
        //{
        //    if (Input.GetKey(KeyCode.E))
        //    {
        //        Debug.Log("pressed E");
        //    }
        //    else
        //    {
        //        Debug.Log("wrong key");
        //    }
        //}

        if (inCheck)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("passing check");
                keyPressed = true;
            }
        }

        if (inSecondCheck)
        {
            holdTime -= Time.deltaTime * holdTimeSpeed;
            holdFailCountDown -= Time.deltaTime;

            if (holdFailCountDown <= 0)
            {
                if (!Input.GetKey(KeyCode.E))
                {
                    failedSecondCheck = true;
                }

            }
            if (Input.GetKey(KeyCode.E))
            {
                Debug.Log("pressing and holding E");
            }

            if (Input.GetKeyUp(KeyCode.E))
            {
                Debug.Log("you failed second check");
                failedSecondCheck = true;
            }

            if(holdTime <= 0)
            {
                if (failedSecondCheck)
                {
                    triggerDropPlayer();
                }
                else
                {
                    resumePlayer();
                }
                inSecondCheck = false;
                checkKey.SetActive(false);
            }
        }



        
    }  

    void OnTriggerEnter(Collider coll)
    {
        if(coll.gameObject.tag == "Player")
        {
            //wakeUpLogic.SetActive(true);
            inCheck = true;
            checkKey.SetActive(true);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            checkKey.SetActive(false);
            inCheck = false;
            if (!keyPressed)
            {
                failedFirstCheck = true;
            }
            enterSecondCheck();
        }
    }

    public void enterSecondCheck()
    {
        if (failedFirstCheck)
        {
            Debug.Log("you failed first check");
            mainTrack.GetComponent<Animator>().enabled = false;
            player.transform.parent = myTunnel.transform;
            myTunnel.GetComponent<Animator>().enabled = true;
            inSecondCheck = true;
            checkKey.SetActive(true);
        }
    }

    void triggerDropPlayer()
    {

    }

    void resumePlayer()
    {
        player.transform.parent = mainTrack.transform;
        mainTrack.GetComponent<Animator>().enabled = true;
    }
    IEnumerator resetDoor()
    {
        yield return new WaitForSeconds(5.0f);
        nextDoor.GetComponent<Renderer>().material = greenMat;
    }
}

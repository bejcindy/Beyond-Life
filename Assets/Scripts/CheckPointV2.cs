using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointV2   : MonoBehaviour
{
    public GameObject player;
    public GameObject checkKey;
    //public GameObject[] cpLights;
    //public GameObject wakeUpLogic;

    public bool inCheck = false;


    //public float pressCD = 3.0f;
    //float pressCDVal;
    public float timerSpeed = 0.1f;
    public static int unpressedAmt = 0;

    //new stuff
    public bool keyPressed = false;
    public GameObject myDoor;
    public GameObject nextDoor;
    public Material currentMat;


    //New Logic


    public bool firstCheckPass = false;

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
            if (Input.GetKey(KeyCode.E))
            {
                Debug.Log("passing check");
            }
            if (Input.GetKeyUp(KeyCode.E))
            {
                firstCheckPass = false;
                Debug.Log("you failed");
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
            enterSecondCheck();
        }
    }

    public void enterSecondCheck()
    {
        if (firstCheckPass)
        {
            //continue
        }
        else
        {
            //pause and start second check
        }
    }

    void changeLight()
    {
        //switch (unpressedAmt)
        //{
        //    case 0:
        //        break;
        //    case 1:
        //        foreach(GameObject light in cpLights)
        //        {
        //            light.GetComponent<Renderer>().material = yellowMat;
        //        }
        //        AS.PlayOneShot(yellowSound);

        //        break;
        //    case 2:
        //        foreach (GameObject light in cpLights)
        //        {
        //            light.GetComponent<Renderer>().material = redMat;
        //        }
        //        AS.PlayOneShot(redSound);
        //        GetComponent<BoxCollider>().enabled = false;
        //        wakeUpLogic.SetActive(true);
        //        wakeUpLogic.GetComponent<WakeUp>().offTrack = true;
        //        break;
        //    default:
        //        break;
        //}
        //unpressedAmt += 1;
        //switch (unpressedAmt)
        //{
        //    case 0:
        //        break;
        //    case 1:
        //        nextDoor.GetComponent<Renderer>().material = yellowMat;
        //        AS.PlayOneShot(yellowSound);
        //        break;
        //    case 2:
        //        nextDoor.GetComponent<Renderer>().material = redMat;
        //        AS.PlayOneShot(redSound);
        //        break;
        //    default:
        //        break;
        //}
        //StartCoroutine(resetDoor());
    }

    IEnumerator resetDoor()
    {
        yield return new WaitForSeconds(5.0f);
        nextDoor.GetComponent<Renderer>().material = greenMat;
    }
}

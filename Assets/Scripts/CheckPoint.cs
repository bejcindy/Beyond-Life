using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public GameObject player;
    public GameObject checkKey;
    //public GameObject[] cpLights;
    public GameObject wakeUpLogic;
    [SerializeField] private GameObject myDoor;

    public bool inCheck = false;
    public bool keyPressed = false;


    public float pressCD = 3.0f;
    float pressCDVal;
    public float timerSpeed = 0.1f;
    public static int unpressedAmt = 0;


    [SerializeField] Material yellowMat;
    [SerializeField] Material redMat;
    [SerializeField] Material greenMat;
    [SerializeField] AudioClip greenSound;
    [SerializeField] AudioClip yellowSound;
    [SerializeField] AudioClip redSound;
    [SerializeField] AudioClip buttonSound;

    AudioSource AS;
    AudioSource playerAS;
    // Start is called before the first frame update
    void Start()
    {
        pressCDVal = pressCD;
        AS = myDoor.GetComponent<AudioSource>();
        playerAS = player.GetComponent<AudioSource>();
        greenMat = myDoor.GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(unpressedAmt);
        if (inCheck)
        {
            if (!Input.GetKeyDown(KeyCode.E))
            {

                pressCD -= timerSpeed * Time.deltaTime;
                if(pressCD <=0)
                {
                    inCheck = false;
                    wakeUpLogic.SetActive(false);
                    checkKey.SetActive(false);
                    unpressedAmt += 1;
                    

                    changeLight();


                }
            }
            else
            {
                //AS.PlayOneShot(greenSound);
                playerAS.PlayOneShot(buttonSound);
                keyPressed = true;
                inCheck = false;
                pressCD = pressCDVal;
                
            }
        }
        if (Vector3.Distance(player.transform.position, myDoor.transform.position) < 6.0f)
        {
            AS.enabled = true;
            if (keyPressed)
            {
                keyPressed = false;
                AS.PlayOneShot(greenSound);
                
            }
            else
            {
                if (unpressedAmt == 1)
                {
                    if (!AS.isPlaying)
                    {
                        AS.PlayOneShot(yellowSound);
                    }
          
                }
                else if (unpressedAmt == 2)
                {
                    if (!AS.isPlaying)
                    {
                        AS.PlayOneShot(redSound);
                    }
                   
                }
            }

        }
    }

    void OnTriggerEnter(Collider coll)
    {
        if(coll.gameObject.tag == "Player")
        {
            wakeUpLogic.SetActive(true);
            checkKey.SetActive(true);
            inCheck = true;
        }

    }

    void changeLight()
    {
        switch (unpressedAmt)
        {
            case 0:
                break;
            case 1:
                //foreach(GameObject light in cpLights)
                //{
                //    light.GetComponent<Renderer>().material = yellowMat;
                //}
                //AS.PlayOneShot(yellowSound);
                myDoor.GetComponent<Renderer>().material = yellowMat;
                StartCoroutine(resetDoor());
                
                
                break;
            case 2:
                //foreach (GameObject light in cpLights)
                //{
                //    light.GetComponent<Renderer>().material = redMat;
                //}
                //AS.PlayOneShot(redSound);
                
                myDoor.GetComponent<Renderer>().material = redMat;
                StartCoroutine(resetDoor());
                GetComponent<BoxCollider>().enabled = false;
                wakeUpLogic.SetActive(true);
                wakeUpLogic.GetComponent<WakeUp>().offTrack = true;
                break;
            default:
                break;
        }
    }

    IEnumerator resetDoor()
    {
        yield return new WaitForSeconds(5.0f);
        myDoor.GetComponent<Renderer>().material = greenMat;

    }
}

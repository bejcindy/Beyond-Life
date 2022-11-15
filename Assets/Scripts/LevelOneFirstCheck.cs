using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelOneFirstCheck   : MonoBehaviour
{
    public GameObject player;
    public GameObject checkKey;
    public GameObject mainTrack;

    public GameObject playerTile;
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
    public GameObject myLights;
    public Material currentMat;


    //New Logic


    public bool failedFirstCheck = false;
    public bool failedSecondCheck = false;

    Animator tunnelAnim;

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

        tunnelAnim = myTunnel.transform.parent.gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        if (inCheck)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("passing check");
                keyPressed = true;
                myLights.GetComponent<Animator>().SetBool("Checked", true);
            }
        }

        if (inSecondCheck)
        {
            
            //holdFailCountDown -= Time.deltaTime;

            //if (holdFailCountDown <= 0)
            //{
            //    if (!Input.GetKey(KeyCode.E))
            //    {
            //        failedSecondCheck = true;
            //    }

            //}
            if (Input.GetKeyDown(KeyCode.E))
            {
                //holdTime -= Time.deltaTime * holdTimeSpeed;
                //myLights.GetComponent<Animator>().enabled = true;
                myLights.GetComponent<Animator>().SetBool("Charging", true);
                myLights.GetComponent<Animator>().speed = 0.1f;
                if(myLights.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
                {
                    myLights.GetComponent<AudioSource>().enabled = true;
                    myLights.GetComponent<AudioSource>().Play();
                }

                Debug.Log("pressing and holding E");
            }

            if (Input.GetKeyUp(KeyCode.E))
            {
                //if(holdTime > 0)
                //{
                //    Debug.Log("you failed second check");
                //    failedSecondCheck = true;
                //}
                myLights.GetComponent<Animator>().speed = 0;
                myLights.GetComponent<AudioSource>().Pause();

            }

            if(tunnelAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
            {
                //if(holdTime <= 0)
                //{
                //    if (failedSecondCheck)
                //    {
                //        triggerDropPlayer();
                //    }
                //    else
                //    {
                //        resumePlayer();
                //    }
                //}
                //else
                //{
                //    failedSecondCheck = true;
                //    triggerDropPlayer();
                //}
                if (myLights.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
                {
                    failedSecondCheck = true;
                    triggerDropPlayer();
                }
                else
                {
                    resumePlayer();
                }
                checkKey.SetActive(false);
            }
            else
            {
                if (myLights.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
                {
                    checkKey.SetActive(false);
                }
            }


            //if(holdTime <= 0)
            //{
            //    if (failedSecondCheck)
            //    {
            //        triggerDropPlayer();
            //    }
            //    else
            //    {
            //        if (tunnelAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
            //        {
            //            resumePlayer();
            //        }
                        
            //    }
            //    checkKey.SetActive(false);
            //}
        }



        
    }  

    void OnTriggerEnter(Collider coll)
    {
        if(coll.gameObject.tag == "Player")
        {
            //wakeUpLogic.SetActive(true);
            inCheck = true;
            checkKey.SetActive(true);
            myLights.GetComponent<Animator>().SetBool("Fading", true);
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
            StartCoroutine(enterSecondCheck());
        }
    }

    public IEnumerator enterSecondCheck()
    {
        if (failedFirstCheck)
        {
            Debug.Log("you failed first check");
            mainTrack.GetComponent<Animator>().speed = 0;
            playerTile.SetActive(false);
            player.transform.parent = myTunnel.transform;
            player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
            yield return new WaitForSeconds(1.0f);
            tunnelAnim.enabled = true;
            inSecondCheck = true;
            checkKey.SetActive(true);
        }
    }



    void triggerDropPlayer()
    {

    }

    void resumePlayer()
    {

        Debug.Log("done rotating");
        inSecondCheck = false;
        player.transform.parent = mainTrack.transform;
        player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | 
                                                        RigidbodyConstraints.FreezeRotationZ | 
                                                        RigidbodyConstraints.FreezeRotationY |
                                                        RigidbodyConstraints.FreezePositionY;
        playerTile.SetActive(true);
        mainTrack.GetComponent<Animator>().speed = 1;
        



    }

}

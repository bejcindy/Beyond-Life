using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelOneFirstCheck   : MonoBehaviour
{
    public GameObject player;
    public GameObject checkKey;
    public GameObject mainTrack;

    public GameObject playerTile;

    public GameObject spawnedBall;
    //public GameObject wakeUpLogic;

    public bool inCheck = false;
    public bool inSecondCheck = false;

    //public float holdTime = 10.0f;
    //public float holdFailCountDown = 2.0f;
    //public float holdTimeSpeed = 2.0f;

    //public float pressCD = 3.0f;
    //float pressCDVal;

    //new stuff
    public bool keyPressed = false;
    public bool secondKeyPressed = false;
    public GameObject myTunnel;
    public GameObject myLights;
    public GameObject buttonSound;
    public Material currentMat;


    //New Logic


    public bool failedFirstCheck = false;
    public bool failedSecondCheck = false;

    public AudioClip chargingSound;
    public AudioClip failedSound;
    public AudioClip ventCloseClip;

    Animator tunnelAnim;
    Animator checkAnim;
    Animator lightAnim;
    AudioSource AS;
    AudioSource lightAS;



    // Start is called before the first frame update
    void Start()
    {
        AS = GetComponent<AudioSource>();
        lightAS = myLights.GetComponent<AudioSource>();
        lightAnim = myLights.GetComponent<Animator>();
        
        buttonSound = gameObject.transform.GetChild(0).gameObject;

        tunnelAnim = myTunnel.transform.parent.gameObject.GetComponent<Animator>();
        checkAnim = checkKey.GetComponent<Animator>();
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
                lightAnim.SetBool("Checked", true);
                buttonSound.GetComponent<AudioSource>().enabled = true;

                checkAnim.SetBool("FirstPress", true);
            }

            if (Input.GetKeyUp(KeyCode.E))
            {
                checkAnim.SetBool("ButtonUp", true);
                checkAnim.SetBool("FirstPress", false);
            }
            if (!buttonSound.GetComponent<AudioSource>().isPlaying)
            {
                buttonSound.GetComponent<AudioSource>().enabled = false;
            }
        }

        if (inSecondCheck)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                
                lightAnim.SetBool("Charging", true);
                secondKeyPressed = true;
                lightAnim.speed = 0.1f;
                if(lightAnim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
                {
                    lightAS.clip = chargingSound;
                    lightAS.enabled = true;
                    lightAS.Play();
                }

                Debug.Log("pressing and holding E");
            }

            if (Input.GetKeyUp(KeyCode.E))
            {
                lightAnim.speed = 0;
                lightAS.Pause();

            }

            if(tunnelAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
            {


                if (lightAnim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1 || !secondKeyPressed)
                {
                    failedSecondCheck = true;
                    StartCoroutine(triggerDropPlayer());
                }
                else
                {
                    resumePlayer();
                }
                checkKey.SetActive(false);
            }
            else
            {
                if (lightAnim.GetCurrentAnimatorClipInfo(0)[0].clip.name == "ChargingLight" &&
                   lightAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 && secondKeyPressed)
                {
                    checkKey.SetActive(false);
                    Debug.Log("faklse now");
                }
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
            lightAnim.enabled = true;
            lightAnim.SetBool("Fading", true);
        }



    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            Debug.Log("checking if multiple checks");
            checkAnim.SetBool("FirstPress", false);
            checkKey.SetActive(false);
            inCheck = false;
            if (!keyPressed)
            {
                failedFirstCheck = true;
            }
            else
            {
                resetTrack();
            }
            StartCoroutine(enterSecondCheck());
        }

        //if (other.gameObject.tag == "NPC")
        //{
        //    Vector3 spawnPos = new Vector3(myTunnel.transform.position.x, myTunnel.transform.position.y - 5.0f, myTunnel.transform.position.z);
        //    Instantiate(spawnedBall, spawnPos, Quaternion.identity);
        //}
    }

    public IEnumerator enterSecondCheck()
    {
        if (failedFirstCheck)
        {
            mainTrack.GetComponent<Animator>().speed = 0;
            //playerTile.SetActive(false);
            player.transform.parent = myTunnel.transform;
            playerTile.transform.parent = myTunnel.transform;
            player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY ;
            GetComponent<BoxCollider>().enabled = false;
            yield return new WaitForSeconds(1.0f);
            checkKey.SetActive(true);
            tunnelAnim.enabled = true;
            inSecondCheck = true;
            
        }
    }



    IEnumerator triggerDropPlayer()
    {
        inSecondCheck = false;
        player.transform.parent = null;
        lightAnim.speed = 1;
        lightAnim.SetBool("SecondFail", true);
        lightAS.clip = failedSound;
        lightAS.enabled = true;
        if (!lightAS.isPlaying)
        {
            lightAS.Play();
        }
        lightAS.loop = true;
        playerTile.GetComponent<Animator>().enabled = true;
        player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None | RigidbodyConstraints.FreezeRotationZ
                                                    | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationX;
        yield return new WaitForSeconds(3.0f);
        playerTile.GetComponent<AudioSource>().PlayOneShot(ventCloseClip);
        lightAS.enabled = false;
        



        yield return new WaitForSeconds(5.0f);
        resetTrack();
        GetComponent<BoxCollider>().enabled = true;
        player.GetComponent<CurvePlayerController>().enabled = true;
        Camera.main.GetComponent<CameraController>().enabled = true;
    }

    void resumePlayer()
    {

        Debug.Log("done rotating");
        inSecondCheck = false;
        player.transform.parent = mainTrack.transform;
        playerTile.transform.parent = mainTrack.transform;
        player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | 
                                                        RigidbodyConstraints.FreezeRotationZ | 
                                                        RigidbodyConstraints.FreezeRotationY |
                                                        RigidbodyConstraints.FreezePositionY;

        //resetting
        resetTrack();



    }


    void resetTrack()
    {
        GetComponent<BoxCollider>().enabled = true;
        failedFirstCheck = false;
        failedSecondCheck = false;
        inCheck = false;
        keyPressed = false;
        lightAnim.SetBool("Checked", false);
        lightAnim.SetBool("Charging", false);
        lightAnim.SetBool("Fading", false);
        lightAnim.SetBool("SecondFail", false);
        lightAnim.enabled = false;
        tunnelAnim.enabled = false;
        playerTile.transform.parent = mainTrack.transform;
        mainTrack.GetComponent<Animator>().speed = 1;
    }



}

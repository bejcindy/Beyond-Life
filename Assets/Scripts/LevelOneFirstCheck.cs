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

    public bool onTrack = true;

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
    Animator playerAnim;
    AudioSource AS;
    AudioSource lightAS;



    // Start is called before the first frame update
    void Start()
    {
        AS = GetComponent<AudioSource>();
        lightAS = myLights.GetComponent<AudioSource>();
        lightAnim = myLights.GetComponent<Animator>();
        playerAnim = player.GetComponent<Animator>();
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


        //rolling tunnel check
        if (inSecondCheck)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
               //start light charging up animation
                lightAnim.SetBool("Charging", true);

                secondKeyPressed = true;
                lightAnim.speed = 0.1f;

                if(lightAnim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
                {
                    lightAS.Play();
                }
            }

            if (Input.GetKeyUp(KeyCode.E))
            {
                lightAnim.speed = 0;
                lightAS.Pause();

            }

            //when tunnel is done rolling for one cycle 
            if(tunnelAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
            {
                checkKey.SetActive(false);

                //if the light animation hasnt played fully or no key is pressed
                if (lightAnim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1 || !secondKeyPressed)
                {
                    failedSecondCheck = true;
                    StartCoroutine(triggerDropPlayer());
                }
                else
                {
                    resumePlayer();
                }
                
            }
            else
            {
                if (lightAnim.GetCurrentAnimatorClipInfo(0)[0].clip.name == "ChargingLight" &&
                   lightAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 && secondKeyPressed)
                {
                    checkKey.SetActive(false);
                }
            }


        }


        //play shaking animation when player is still stuck on track
        if (player.GetComponent<CurvePlayerController>().onTrack)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                if (!playerAnim.isActiveAndEnabled)
                {
                    Debug.Log("trigger player anim");
                    playerAnim.enabled = true;
                }
                else
                {
                    playerAnim.Play("chara_idle", 0, 0);
                }
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                if (!playerAnim.isActiveAndEnabled)
                {
                    playerAnim.enabled = true;
                }
                else
                {
                    playerAnim.Play("chara_idle", 0, 0);
                }
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                if (!playerAnim.isActiveAndEnabled)
                {
                    playerAnim.enabled = true;
                }
                else
                {
                    playerAnim.Play("chara_idle", 0, 0);
                }
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                if (!playerAnim.isActiveAndEnabled)
                {
                    playerAnim.enabled = true;
                }
                else
                {
                    playerAnim.Play("chara_idle", 0, 0);
                }
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (!playerAnim.isActiveAndEnabled)
                {
                    playerAnim.enabled = true;
                }
                else
                {
                    playerAnim.Play("chara_idle", 0, 0);
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
            lightAnim.SetBool("Fading", true);
        }



    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            checkAnim.SetBool("FirstPress", false);
            checkKey.SetActive(false);
            inCheck = false;
            if (!keyPressed)
            {
                failedFirstCheck = true;
                lightAnim.SetBool("Fading", false);
            }
            else
            {
                resetTrack();
            }
            StartCoroutine(enterSecondCheck());
        }

        if (other.gameObject.tag == "NPC")
        {
            other.gameObject.GetComponent<LevelOneNPC>().spawnGauge += 1;
            if(other.gameObject.GetComponent<LevelOneNPC>().spawnGauge == 5)
            {
                other.gameObject.GetComponent<LevelOneNPC>().spawnGauge = 0;
                float xPosDif = Random.Range(-1, 1);
                Vector3 spawnPos = new Vector3(myTunnel.transform.position.x + xPosDif, myTunnel.transform.position.y - 4.5f, myTunnel.transform.position.z);
                Instantiate(spawnedBall, spawnPos, Quaternion.identity);

            }

        }
    }

    public IEnumerator enterSecondCheck()
    {
        if (failedFirstCheck)
        {
            mainTrack.GetComponent<Animator>().speed = 0;
            //playerTile.SetActive(false);
            player.transform.parent = myTunnel.transform;
            playerTile.transform.parent = myTunnel.transform;
            player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            GetComponent<BoxCollider>().enabled = false;
            yield return new WaitForSeconds(1.0f);
            checkKey.SetActive(true);
            tunnelAnim.enabled = true;
            inSecondCheck = true;
            
        }
    }



    IEnumerator triggerDropPlayer()
    {
        player.GetComponent<CurvePlayerController>().onTrack = false;
        playerAnim.enabled = true;
        inSecondCheck = false;
        lightAnim.speed = 1;
        lightAnim.SetBool("SecondFail", true);
        lightAS.clip = failedSound;
        if (!lightAS.isPlaying)
        {
            lightAS.Play();
        }
        lightAS.loop = true;
        playerTile.GetComponent<Animator>().enabled = true;
        player.transform.parent = null;
        player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None | RigidbodyConstraints.FreezeRotationZ
                                                    | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationX;
        yield return new WaitForSeconds(3.5f);
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
        //lightAnim.enabled = false;
        tunnelAnim.enabled = false;
        playerTile.transform.parent = mainTrack.transform;
        mainTrack.GetComponent<Animator>().speed = 1;
    }



}

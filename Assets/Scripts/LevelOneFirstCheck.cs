using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.UI;
using TMPro;

public class LevelOneFirstCheck   : MonoBehaviour
{
    public GameObject player;
    public GameObject checkKey;
    public GameObject mainTrack;

    public GameObject playerTile;

    public GameObject spawnedBall;
    public WakeUp wakeUpMaster;
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
    public AudioClip buttonCheck;

    Animator tunnelAnim;
    Animator checkAnim;
    Animator lightAnim;
    Animator playerAnim;
    AudioSource AS;
    AudioSource lightAS;
    AudioSource tunnelButtonAS;

    public static string codeKeys = "AQWEDCXZ";
    public static string codeKeys1 = "0P;LKI89";
    public static string codeKeys2 = "78IKJHY6";
    public static string codeKeys3 = "KIOP;/><";
    public static string[] codeKeyGroup = new string[] { codeKeys, codeKeys1, codeKeys2, codeKeys3 };

    public static int groupIndex = 0;
    public static int codeIndex = 1;
    public static KeyCode currentKey;

    public GameObject tunnelCheckKey;
    public GameObject messageInput;
    public KeyCode tunnelKey;
    public int tunnelKeyAmt;
    public char tunnelChar;
    public float tunnelThreshold;
    public static float trackCurrentSpeed;

    public bool messageEntered = false;
    public static int soulCount = 0;
    public static bool messageCorrect = false;
    int tnHelper;

    public AudioSource ambienceAS;
    bool startSound = false;


    // Start is called before the first frame update
    void Start()
    {
        LevelOneFirstCheck.currentKey = KeyCode.I;
        AS = GetComponent<AudioSource>();
        lightAS = myLights.GetComponent<AudioSource>();
        tunnelButtonAS = tunnelCheckKey.GetComponent<AudioSource>();
        lightAnim = myLights.GetComponent<Animator>();
        playerAnim = player.GetComponent<Animator>();
        buttonSound = gameObject.transform.GetChild(0).gameObject;

        tunnelAnim = myTunnel.transform.parent.gameObject.GetComponent<Animator>();
        checkAnim = checkKey.GetComponent<Animator>();

        ambienceAS = GameObject.Find("Ambience").GetComponent<AudioSource>();
        LevelOneFirstCheck.trackCurrentSpeed = mainTrack.GetComponent<Animator>().speed;
    }

    // Update is called once per frame
    void Update()
    {

        if(LevelOneFirstCheck.codeIndex < 8)
        {
            LevelOneFirstCheck.currentKey = convertKey(LevelOneFirstCheck.codeKeys[LevelOneFirstCheck.codeIndex]);
        }
        


        if (startSound)
        {
            bgSoundRaise();
        }
        if (inCheck)
        {
            if (Input.GetKeyDown(currentKey)) 
            {
                startSound = true;
                inCheck = false;

                keyPressed = true;
                lightAnim.SetBool("Checked", true);
                lightAnim.SetBool("Fading", false);
                checkAnim.SetBool("FirstPress", true);
                buttonSound.GetComponent<AudioSource>().Play();
                //mainTrack.GetComponent<Animator>().speed = LevelOneFirstCheck.trackCurrentSpeed * 1.25f;
                LevelOneFirstCheck.trackCurrentSpeed *= 1.125f;
                
                
            }
            Debug.Log(currentKey);

            if (Input.GetKeyUp(currentKey))
            {
                Debug.Log("key is up");
                checkAnim.SetBool("ButtonUp", true);
                checkAnim.SetBool("FirstPress", false);

            }
            //if (!buttonSound.GetComponent<AudioSource>().isPlaying)
            //{
            //    buttonSound.GetComponent<AudioSource>().enabled = false;
            //}
        }


        //rolling tunnel check
        if (inSecondCheck)
        {
            if (Input.GetKeyDown(tunnelKey))
            {
               //start light charging up animation
                //lightAnim.SetBool("Charging", true);
                secondKeyPressed = true;
                if (tunnelKeyAmt > 0)
                {
                    tunnelButtonAS.PlayOneShot(buttonCheck);
                }

                //lightAS.Play();
                //lightAnim.speed = 1;

                //if(lightAnim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
                //{
                //    lightAS.Play();
                //}
            }

            if (Input.GetKeyUp(tunnelKey))
            {
                if (tunnelKeyAmt > 0)
                {
                    tunnelKeyAmt -= 1;
                    tnHelper += 1;
                }

                
                tunnelCheckKey.GetComponent<TextMeshProUGUI>().text = new string(tunnelChar, tunnelKeyAmt);
                //lightAnim.speed = 0;
                //lightAS.Pause();

            }

            if(tunnelKeyAmt <= 0)
            {
                if (!lightAS.isPlaying)
                {
                    lightAS.Play();
                }
                lightAnim.SetBool("Charging", true);
                lightAnim.speed = 0.3f;
            }

            //when tunnel is done rolling for one cycle 
            if(tunnelAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
            {
                tunnelCheckKey.SetActive(false);

                //if keys not all pressed
                if (tunnelKeyAmt>0 || !secondKeyPressed)
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
                //if (lightAnim.GetCurrentAnimatorClipInfo(0)[0].clip.name == "ChargingLight" &&
                //   lightAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 && secondKeyPressed)
                if(tunnelKeyAmt<=0 && !tunnelButtonAS.isPlaying)
                {
                    tunnelCheckKey.SetActive(false);
                }
                //if (lightAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= tunnelThreshold*tnHelper)
                //{
                //    lightAnim.speed = 0;
                //}
                //Debug.Log("time can play is " + lightAS.clip.length * tunnelThreshold * tnHelper);
                //Debug.Log("time is at "+ lightAS.time);
                //if (lightAS.time >= (5f *tunnelThreshold * tnHelper))
                //{
                //    Debug.Log("we should pause");
                //    lightAS.Pause();
                //}
            }


        }


        //play shaking animation when player is still stuck on track
        if (player.GetComponent<CurvePlayerController>().onTrack)
        {
            if (Input.GetKeyDown(KeyCode.W))
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


        if (messageInput.GetComponent<LevelOneSecret>().done)
        {
            messageInput.GetComponent<LevelOneSecret>().done = false;
            if (LevelOneFirstCheck.messageCorrect)
            {
                nextMessage();
            }
            else
            {
                Debug.Log("repeating message");
                repeatMessage();
            }
            resetTrack();
            resetLightAfterInput();
        }
    }  

    void nextMessage()
    {

    }

    void repeatMessage()
    {
        int splitn = UnityEngine.Random.Range(1, 7);
        string[] mgroup = codeKeys.Split(codeKeys[splitn]);
        string output = string.Format(codeKeys[splitn] + "{0}{1}", mgroup[1], mgroup[0]);
        LevelOneFirstCheck.codeKeys = output;
        LevelOneFirstCheck.codeIndex = 0;
        Debug.Log(codeKeys);
    }



    void bgSoundRaise()
    {
        if(mainTrack.GetComponent<AudioSource>().volume < 1)
        {
            mainTrack.GetComponent<AudioSource>().volume += 0.1f * Time.deltaTime;
        }
        if (ambienceAS.volume < 1)
        {
            ambienceAS.volume += 0.1f * Time.deltaTime;
        }

    }
    void resetLightAfterInput()
    {
        messageInput.GetComponent<LevelOneSecret>().currentLight = null;
        messageInput.GetComponent<LevelOneSecret>().lightAS = null;
        Debug.Log("we should reset");        
        messageInput.GetComponent<LevelOneSecret>().messageEntered = false;
        messageInput.GetComponent<TMP_InputField>().text = "";
        messageInput.SetActive(false);
        messageInput.GetComponent<LevelOneSecret>().done = false;


    }


    KeyCode convertKey(Char c)
    {
        switch (c)
        {
            case 'K':
                return KeyCode.K;
            case ';':
                return KeyCode.Semicolon;
            case '/':
                return KeyCode.Slash;
            case '>':
                return KeyCode.Period;
            case '<':
                return KeyCode.Comma;
            case 'I':
                return KeyCode.I;
            case 'O':
                return KeyCode.O;
            case 'P':
                return KeyCode.P;

            default:
                return KeyCode.E;
        }
    }

    void OnTriggerEnter(Collider coll)
    {
        if(coll.gameObject.tag == "Player")
        {
            //wakeUpLogic.SetActive(true);
            inCheck = true;
            checkKey.GetComponent<TextMeshProUGUI>().text = Char.ToString(LevelOneFirstCheck.codeKeys[LevelOneFirstCheck.codeIndex]);
            checkKey.SetActive(true);
            lightAnim.SetBool("Fading", true);
        }



    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            inCheck = false;
            checkAnim.SetBool("FirstPress", false);
            checkKey.SetActive(false);
            if (!keyPressed)
            {
                failedFirstCheck = true;
                lightAnim.SetBool("Fading", false);
                StartCoroutine(enterSecondCheck());
                
            }
            else
            {
                LevelOneFirstCheck.codeIndex++;
                player.GetComponent<CurvePlayerController>().levelOneTunnelPassed += 1;
                if((player.GetComponent<CurvePlayerController>().levelOneTunnelPassed) % 8 == 0)
                {
                    messagePrompt();
                }
                else
                {
                    resetTrack();
                }
                
            }
            
            
        }

        if (other.gameObject.tag == "NPC")
        {
            other.gameObject.GetComponent<LevelOneNPC>().spawnGauge += 1;
            if(other.gameObject.GetComponent<LevelOneNPC>().spawnGauge == 8)
            {
                other.gameObject.GetComponent<LevelOneNPC>().spawnGauge = 0;
                if (LevelOneFirstCheck.soulCount < 8)
                {
                    float xPosDif = UnityEngine.Random.Range(-2, 2);
                    Vector3 spawnPos = new Vector3(myTunnel.transform.position.x + xPosDif, myTunnel.transform.position.y - 4.5f, myTunnel.transform.position.z);
                    Instantiate(spawnedBall, spawnPos, Quaternion.identity);
                    LevelOneFirstCheck.soulCount++;
                }


            }

        }
    }

    void messagePrompt()
    {
        mainTrack.GetComponent<Animator>().speed = 0;
        messageInput.GetComponent<LevelOneSecret>().currentLight = lightAnim;
        messageInput.GetComponent<LevelOneSecret>().lightAS = lightAS;
        messageInput.SetActive(true);
        messageInput.GetComponent<TMP_InputField>().ActivateInputField();
        
    }

    IEnumerator trackDash()
    {
        mainTrack.GetComponent<Animator>().speed = 3;
        yield return new WaitForSeconds(1.0f);
        if((player.GetComponent<CurvePlayerController>().levelOneTunnelPassed) % 8 != 0)
        { 
            mainTrack.GetComponent<Animator>().speed = 1;
        }
        
    }

    IEnumerator enterSecondCheck()
    {

        mainTrack.GetComponent<Animator>().speed = 0;
        player.transform.parent = myTunnel.transform;
        playerTile.transform.parent = myTunnel.transform;
        player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        GetComponent<BoxCollider>().enabled = false;
        yield return new WaitForSeconds(1.0f);
        //checkKey.SetActive(true);
        generateTunnelText();
        tunnelCheckKey.SetActive(true);
        tunnelAnim.enabled = true;
        inSecondCheck = true;
            
    }


    public void generateTunnelText()
    {
        int checker = UnityEngine.Random.Range(0, 2);
        int amt = UnityEngine.Random.Range(3, 5);
        tunnelKeyAmt = amt;
        if (checker == 0)
        {
            tunnelCheckKey.GetComponent<TextMeshProUGUI>().text = new string('F', amt);
            tunnelKey = KeyCode.F;
            tunnelChar = 'F';
        }
        else
        {
            tunnelCheckKey.GetComponent<TextMeshProUGUI>().text = new string('T', amt);
            tunnelKey = KeyCode.T;
            tunnelChar = 'T';
        }
        tunnelThreshold = 1f/amt;
 

    }
    IEnumerator triggerDropPlayer()
    {
        player.GetComponent<CurvePlayerController>().onTrack = false;
        playerAnim.enabled = false;
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
        



        //yield return new WaitForSeconds(5.0f);
        resetTrack();
        GetComponent<BoxCollider>().enabled = true;
        player.GetComponent<CurvePlayerController>().enabled = true;
        Camera.main.GetComponent<CameraController>().enabled = true;
    }

    void resumePlayer()
    {

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
        Debug.Log("resetting track");
        GetComponent<BoxCollider>().enabled = true;
        failedFirstCheck = false;
        failedSecondCheck = false;
        inCheck = false;
        keyPressed = false;
        lightAnim.SetBool("Checked", false);
        lightAnim.SetBool("Charging", false);
        lightAnim.SetBool("Fading", false);
        lightAnim.SetBool("SecondFail", false);
        lightAnim.SetBool("wrongMessage", false);
        //lightAnim.enabled = false;
        tunnelAnim.enabled = false;
        playerTile.transform.parent = mainTrack.transform;
        mainTrack.GetComponent<Animator>().speed = LevelOneFirstCheck.trackCurrentSpeed;
    }



}

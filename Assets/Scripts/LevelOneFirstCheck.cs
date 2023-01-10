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

    public bool inCheck = false;
    public bool inSecondCheck = false;

    public bool onTrack = true;


    //new stuff
    public bool keyPressed = false;
    public bool secondKeyPressed = false;
    public GameObject myTunnel;
    public GameObject myLights;
    public GameObject mySoul;
    public GameObject buttonSound;
    public GameObject tunnelCheck;


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

    Color greenLightColor;
    Color emissionGreen;
    Color yellowLightColor;
    Color emissionYellow;
    Color redLightColor;
    Color emissionRed;

    public bool changingGreen = false;
    public bool changingYellow = false;
    public bool changingRed = false;
    



    // Start is called before the first frame update
    void Start()
    {
        tunnelCheck = GameObject.Find("LevelOneTunnelCheck");
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

        //defaultWhite = Color.white;
        //emissionWhite = new Color(6.7819f, 6.5866f, 5.0795f, 1f);
        greenLightColor = new Color32(23, 255, 0, 255);
        emissionGreen = new Color(0.025f, 0.75f, 0.025f, 1f);
        yellowLightColor = Color.yellow;
        emissionYellow = new Color(0.75f, 0.57f, 0.05f, 1f);
        redLightColor = Color.red;
        emissionRed = new Color(0.87f, 0f, 0f, 1f);

    }

    // Update is called once per frame
    void Update()
    {

        if (startSound)
        {
            bgSoundRaise();
        }

        if (changingGreen)
        {
            foreach (Transform lightPart in myLights.transform)
            {
                Material lightMat = lightPart.gameObject.GetComponent<Renderer>().material;
                lightMat.EnableKeyword("_EMISSION");
                StartCoroutine(changeLight(lightMat, greenLightColor, emissionGreen, 2f));

            }
            
            changingGreen = false;

        }

        if (changingYellow)
        {
            foreach (Transform lightPart in myLights.transform)
            {
                Material lightMat = lightPart.gameObject.GetComponent<Renderer>().material;
                lightMat.EnableKeyword("_EMISSION");
                StartCoroutine(changeLight(lightMat, yellowLightColor, emissionYellow, 2f));

            }
            
            changingYellow = false;
        }

        if (changingRed)
        {
            foreach (Transform lightPart in myLights.transform)
            {
                Material lightMat = lightPart.gameObject.GetComponent<Renderer>().material;
                lightMat.EnableKeyword("_EMISSION");
                StartCoroutine(changeLight(lightMat, redLightColor, emissionRed, 2f));

            }

            changingYellow = false;
        }
        if (inCheck)
        {
            if (Input.GetKeyDown(LevelOneKeys.currentKey)) 
            {
                startSound = true;
                changingGreen = true;
                inCheck = false;
                keyPressed = true;
                buttonSound.GetComponent<AudioSource>().Play();
                StartCoroutine(trackDash());
                StartCoroutine(mySoul.GetComponent<LevelOneSoul>().SoulGrow(1.25f, 1));
                playerAnim.SetBool("PassCheck", true);

            }

            if (Input.GetKeyUp(LevelOneKeys.currentKey))
            {
                Debug.Log("key is up");
                checkAnim.SetBool("ButtonUp", true);

            }
        }


        //rolling tunnel check
        if (inSecondCheck)
        {
            tunnelCheckKey.SetActive(true);
            tunnelCheckKey.GetComponent<TextMeshProUGUI>().text = Char.ToString(tunnelCheck.GetComponent<LevelOneTunnelKeys>().currentChar);

            if (!tunnelCheck.GetComponent<LevelOneTunnelKeys>().inCheck)
            {
                tunnelCheckKey.SetActive(false);
                tunnelCheck.GetComponent<LevelOneTunnelKeys>().resetIndex();
            }
            //if (Input.GetKeyDown(tunnelKey))
            //{
            //    secondKeyPressed = true;
            //    if (tunnelKeyAmt > 0)
            //    {
            //        tunnelButtonAS.PlayOneShot(buttonCheck);
            //    }

            //}

            //if (Input.GetKeyUp(tunnelKey))
            //{
            //    if (tunnelKeyAmt > 0)
            //    {
            //        tunnelKeyAmt -= 1;
            //        tnHelper += 1;
            //    }

                
            //    tunnelCheckKey.GetComponent<TextMeshProUGUI>().text = new string(tunnelChar, tunnelKeyAmt);

            //}

            //if(tunnelKeyAmt <= 0)
            //{
            //    if (!lightAS.isPlaying)
            //    {
            //        lightAS.Play();
            //    }
            //    lightAnim.SetBool("Charging", true);
            //    lightAnim.speed = 0.3f;
            //}

            //when tunnel is done rolling for one cycle 
            if(tunnelAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
            {
                tunnelCheckKey.SetActive(false);

                //if keys not all pressed
                if (tunnelCheck.GetComponent<LevelOneTunnelKeys>().inCheck)
                {
                    failedSecondCheck = true;
                    changingRed = true;
                    StartCoroutine(triggerDropPlayer());
                }
                else
                {
                    changingYellow = true;
                    resumePlayer();
                    resetTrack();
                    resetTrackSpeed();
                    resumeTrackSpeed();
                }
                tunnelCheck.GetComponent<LevelOneTunnelKeys>().resetIndex();
                
            }
            else //pressed all keys before tunnel rolling over 
            {
                if (!tunnelCheck.GetComponent<LevelOneTunnelKeys>().inCheck)
                {
                    changingYellow = true;
                }
            }
            //else
            //{
            //    if(tunnelKeyAmt<=0 && !tunnelButtonAS.isPlaying)
            //    {
            //        tunnelCheckKey.SetActive(false);
            //    }
            //}


        }


        //play shaking animation when player is still stuck on track
        //if (player.GetComponent<CurvePlayerController>().onTrack)
        //{
        //    if (Input.GetKeyDown(KeyCode.W))
        //    {
        //        if (!playerAnim.isActiveAndEnabled)
        //        {
        //            playerAnim.enabled = true;
        //        }
        //        else
        //        {
        //            playerAnim.Play("chara_idle", 0, 0);
        //        }
        //    }
        //    if (Input.GetKeyDown(KeyCode.A))
        //    {
        //        if (!playerAnim.isActiveAndEnabled)
        //        {
        //            playerAnim.enabled = true;
        //        }
        //        else
        //        {
        //            playerAnim.Play("chara_idle", 0, 0);
        //        }
        //    }
        //    if (Input.GetKeyDown(KeyCode.S))
        //    {
        //        if (!playerAnim.isActiveAndEnabled)
        //        {
        //            playerAnim.enabled = true;
        //        }
        //        else
        //        {
        //            playerAnim.Play("chara_idle", 0, 0);
        //        }
        //    }
        //    if (Input.GetKeyDown(KeyCode.D))
        //    {
        //        if (!playerAnim.isActiveAndEnabled)
        //        {
        //            playerAnim.enabled = true;
        //        }
        //        else
        //        {
        //            playerAnim.Play("chara_idle", 0, 0);
        //        }
        //    }
        //    if (Input.GetKeyDown(KeyCode.Space))
        //    {
        //        if (!playerAnim.isActiveAndEnabled)
        //        {
        //            playerAnim.enabled = true;
        //        }
        //        else
        //        {
        //            playerAnim.Play("chara_idle", 0, 0);
        //        }
        //    }
        //}


        if (messageInput.GetComponent<LevelOneSecret>().done)
        {
            messageInput.GetComponent<LevelOneSecret>().done = false;

            resetTrack();
            if (!LevelOneSecret.inputCorrect)
            {
                resetTrackSpeed();
            }
            resumeTrackSpeed();
            resetLightAfterInput();
            LevelOneSecret.inputCorrect = false;
        }
    }  



    IEnumerator changeLight(Material mat, Color endColor, Color emissionColor, float duration)
    {

        //foreach (Transform lightPart in myLights.transform)
        //{
        //    Material lightMat = lightPart.gameObject.GetComponent<Renderer>().material;
        //    lightMat.EnableKeyword("_EMISSION");
            float time = 0;
            while(time < duration)
            {
                mat.color = Color.Lerp(mat.color, endColor, time/duration);
                //lightMat.SetColor("_BaseColor", Color.Lerp(lightMat.color, new Color32(23, 255, 0, 255), Time.deltaTime*2f));
                mat.SetColor("_EmissionColor", Color.Lerp(mat.color, emissionColor * 10f, time/duration));
                time += Time.deltaTime;
                yield return null;


            }
            mat.color = endColor;

        //}


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




    void OnTriggerEnter(Collider coll)
    {
        if(coll.gameObject.tag == "Player")
        {
            //wakeUpLogic.SetActive(true);
            inCheck = true;
            checkKey.GetComponent<TextMeshProUGUI>().text = Char.ToString(LevelOneKeys.currentChar);
            checkKey.SetActive(true);
            lightAnim.SetBool("Fading", true);
        }



    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            inCheck = false;
            playerAnim.SetBool("PassCheck", false);
            checkKey.SetActive(false);
            if (!keyPressed)
            {
                failedFirstCheck = true;
                lightAnim.SetBool("Fading", false);
                StartCoroutine(enterSecondCheck());
                
            }
            else
            {
                LevelOneKeys.codeIndex++;
                
                player.GetComponent<CurvePlayerController>().levelOneTunnelPassed += 1;
                if((player.GetComponent<CurvePlayerController>().levelOneTunnelPassed) % 8 == 0)
                {
                    messagePrompt();
                }
                else
                {
                    resetTrack();
                    resumeTrackSpeed();
                }
                
            }
            
            
        }

        //if (other.gameObject.tag == "NPC")
        //{
        //    other.gameObject.GetComponent<LevelOneNPC>().spawnGauge += 1;
        //    if(other.gameObject.GetComponent<LevelOneNPC>().spawnGauge == 8)
        //    {
        //        other.gameObject.GetComponent<LevelOneNPC>().spawnGauge = 0;
        //        if (LevelOneFirstCheck.soulCount < 8)
        //        {
        //            float xPosDif = UnityEngine.Random.Range(-2, 2);
        //            Vector3 spawnPos = new Vector3(myTunnel.transform.position.x + xPosDif, myTunnel.transform.position.y - 4.5f, myTunnel.transform.position.z);
        //            Instantiate(spawnedBall, spawnPos, Quaternion.identity);
        //            LevelOneFirstCheck.soulCount++;
        //        }


        //    }

        //}
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
        if ((player.GetComponent<CurvePlayerController>().levelOneTunnelPassed) % 8 != 0)
        {
            LevelOneFirstCheck.trackCurrentSpeed *= 1.125f;
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
        //generateTunnelText();
        tunnelCheck.GetComponent<LevelOneTunnelKeys>().inCheck = true;
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
        resetTrackSpeed();
        resumeTrackSpeed();
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
    }


    void resetTrack()
    {
        Debug.Log("resetting track");
        GetComponent<BoxCollider>().enabled = true;
        failedFirstCheck = false;
        failedSecondCheck = false;
        inCheck = false;
        keyPressed = false;
        foreach (AnimatorControllerParameter parameter in lightAnim.parameters)
        {
            lightAnim.SetBool(parameter.name, false);
        }
        //lightAnim.SetBool("Checked", false);
        //lightAnim.SetBool("Charging", false);
        //lightAnim.SetBool("Fading", false);
        //lightAnim.SetBool("SecondFail", false);
        //lightAnim.SetBool("wrongMessage", false);
        tunnelAnim.enabled = false;
        playerTile.transform.parent = mainTrack.transform;
        
    }

    void resumeTrackSpeed()
    {
        mainTrack.GetComponent<Animator>().speed = LevelOneFirstCheck.trackCurrentSpeed;
    }

    void resetTrackSpeed()
    {
        LevelOneFirstCheck.trackCurrentSpeed = 1;
    }



}

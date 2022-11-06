using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WakeUp : MonoBehaviour
{
    public Image blackImage;
    public GameObject J;

    public GameObject player;
    public GameObject ringTrack;

    public AudioClip wakeSound;

    [Header("Time Interval Between J")]
    public float timeInterval;
    [Space(10)]
    public float wakeSpeed;
    public float sleepRate;


    public float newAlpha;
    public float alphaDiff = 0.05f;

    AudioSource a;
    float t;
    bool fallingAsleep;
    int iPressedTimes = 0;

    bool eyesOpen = false;
    public bool offTrack = false;

    // Start is called before the first frame update
    void Start()
    {
        blackImage.color = Color.black;
        t = 0;
        fallingAsleep = false;
        a = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (J.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                //if player hasn't gained full view yet
                if (!eyesOpen)
                {
                    newAlpha = blackImage.color.a - alphaDiff;
                    alphaDiff += 0.05f;
                    iPressedTimes += 1;

                    //Pressed J 4 times
                    if (iPressedTimes == 4)
                    {
                        newAlpha = 0;
                        eyesOpen = true;
                        //activatePlayer();

                    }

                    blackImage.color = new Color(0, 0, 0, blackImage.color.a - wakeSpeed);

                    if (blackImage.color.a > 0)
                    {
                        fallingAsleep = true;
                    }
                    else
                    {
                        blackImage.gameObject.SetActive(false);
                    }
                }

                //if (wakeSound)
                //{
                //    a.PlayOneShot(wakeSound);
                //}
                t = 0;
                J.SetActive(false);
            }
        }
        

        if (fallingAsleep)
        {
            if (blackImage.color.a < 1)
            {
                if(blackImage.color.a < newAlpha)
                {
                    blackImage.color += new Color(0, 0, 0, sleepRate);
                }
                
            }
            else
            {
                blackImage.color = Color.black;
                fallingAsleep = false;
            }
        }

        if (offTrack)
        {
            
            StartCoroutine(activatePlayer());
        }
    }

    IEnumerator activatePlayer()
    {
        ringTrack.GetComponent<Animator>().speed = 0;
        player.transform.parent = null;
        yield return new WaitForSeconds(2.0f);
        ringTrack.GetComponent<MeshCollider>().enabled = false;
        //Camera.main.GetComponent<CameraController>().enabled = true;
        //Camera.main.GetComponent<CameraController>().wakeUp = true;
        yield return new WaitForSeconds(2.0f);
        //yield return 1.0f;

        player.transform.GetChild(0).gameObject.GetComponent<CurvePlayerController>().enabled = true;
        Camera.main.GetComponent<CameraController>().enabled = true;
        Camera.main.GetComponent<CameraController>().wakeUp = true;
        ringTrack.GetComponent<Animator>().speed = 1;
        J.SetActive(false);
        
    }


    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPanel : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject startButton;
    public GameObject wakeUpMaster;
    public AudioClip buttonSound;

    public GameObject mainTrack;
    public AudioSource ambienceAS;

    public float pressWaitTime = 3.0f;
    public float pressWaitTimeVal;

    public bool inButtonWait = false;
    public bool inCheck = false;

    public int pressAmt = 0;

    AudioSource AS;
    Animator buttonAnim;
    void Start()
    {
        startButton = transform.GetChild(0).gameObject;
        buttonAnim = startButton.GetComponent<Animator>();
        AS = gameObject.GetComponent<AudioSource>();

        pressWaitTimeVal = pressWaitTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (inCheck)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                //GetComponent<Animator>().enabled = true;
                buttonAnim.SetBool("FirstPress", true);
                
                AS.PlayOneShot(buttonSound);
            }


            if (Input.GetKeyUp(KeyCode.E))
            {
                buttonAnim.SetBool("ButtonUp", true);
                inButtonWait = true;
                inCheck = false;
                pressAmt += 1;
            }
        }


        if (inButtonWait)
        {
            StartCoroutine(restartButton());

        }

        switch (pressAmt)
        {
            case 0:
                break;
            case 1:
                mainTrack.GetComponent<AudioSource>().volume += 0.1f * Time.deltaTime;
                break;
            case 2:
                ambienceAS.volume += 0.1f * Time.deltaTime;
                wakeUpMaster.SetActive(true);
                mainTrack.GetComponent<Animator>().enabled = true;
                startButton.SetActive(false);
                break;
            default:
                
                break;
        }

        
    }

    IEnumerator restartButton()
    {
        if(pressAmt == 2)
        {
            yield return new WaitForSeconds(2.0f);
            this.gameObject.SetActive(false);
        }
        inButtonWait = false;
        yield return new WaitForSeconds(3.0f);
        buttonAnim.SetBool("FirstPress", false);
        buttonAnim.SetBool("ButtonUp", false);
        buttonAnim.SetBool("Reset", true);
        yield return new WaitForSeconds(2.0f);
        buttonAnim.SetBool("Reset", false);
        inCheck = true;
    }
}

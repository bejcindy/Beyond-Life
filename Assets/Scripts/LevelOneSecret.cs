using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelOneSecret : MonoBehaviour
{
    string secret = "LIFE";
    static int enterIndex = 0;

    public GameObject player;
    public Animator currentLight;
    public AudioSource lightAS;
    public AudioClip failedSound;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void checkAnswer(string answer)
    {
        if(answer == Char.ToString(secret[LevelOneSecret.enterIndex]))
        {
            Debug.Log("answer is correct");
        }
        else
        {
            Debug.Log("answer is incorrect");
        }
    }

    public void submitAnswer(string answer)
    {
        player.GetComponent<CurvePlayerController>().messageEntered = true;
        if (answer == Char.ToString(secret[LevelOneSecret.enterIndex]))
        {
            Debug.Log("answer is correct");
            LevelOneSecret.enterIndex++;
        }
        else
        {
            Debug.Log("answer is incorrect");
            StartCoroutine(failedCheck());
            
        }
    }

    IEnumerator failedCheck()
    {
        currentLight.SetBool("messageFailed", true);
        lightAS.PlayOneShot(failedSound);
        yield return new WaitForSeconds(0.5f);
    }
}

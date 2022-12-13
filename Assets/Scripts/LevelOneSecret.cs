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
    public AudioClip messageCorrect;

    public bool messageEntered = false;
    public bool done = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //public void checkAnswer(string answer)
    //{
    //    if(answer == Char.ToString(secret[LevelOneSecret.enterIndex]))
    //    {
    //        Debug.Log("answer is correct");
    //    }
    //    else
    //    {
    //        Debug.Log("answer is incorrect");
    //    }
    //}

    public void submitAnswer(string answer)
    {
        messageEntered = true;
        
        if (answer == Char.ToString(secret[LevelOneSecret.enterIndex]))
        {
            Debug.Log("answer is correct");
            lightAS.PlayOneShot(messageCorrect);
            LevelOneSecret.enterIndex++;
            done = true;
        }
        else
        {
            LevelOneFirstCheck.codeIndex = 0;
            Debug.Log("answer is incorrect");
            StartCoroutine(failedCheck());
            
        }
    }

    IEnumerator failedCheck()
    {
        Debug.Log("setting message failed to true");
        currentLight.SetBool("wrongMessage", true);
        lightAS.PlayOneShot(failedSound);
        yield return new WaitForSeconds(0.5f);
        currentLight.SetBool("wrongMessage", false);
        currentLight.SetBool("Checked", false);
        done = true;

    }
}

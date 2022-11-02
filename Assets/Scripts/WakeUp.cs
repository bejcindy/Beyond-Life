using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WakeUp : MonoBehaviour
{
    public Image blackImage;
    public GameObject J;

    public AudioClip wakeSound;

    [Header("Time Interval Between J")]
    public float timeInterval;
    [Space(10)]
    public float wakeSpeed;
    public float sleepRate;

    AudioSource a;
    float t;
    bool fallingAsleep;

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
            if (Input.GetKeyDown(KeyCode.J))
            {
                blackImage.color = new Color(0, 0, 0, blackImage.color.a - wakeSpeed);
                if (blackImage.color.a > 0)
                {
                    fallingAsleep = true;
                }
                else
                {
                    blackImage.gameObject.SetActive(false);
                }
                if (wakeSound)
                {
                    a.PlayOneShot(wakeSound);
                }
                t = 0;
                J.SetActive(false);
            }
        }
        else
        {
            if (t < timeInterval)
            {
                t += Time.deltaTime;
            }
            else
            {
                t = timeInterval;
                J.SetActive(true);
            }
            
            
        }

        if (fallingAsleep)
        {
            if (blackImage.color.a < 1)
            {
                blackImage.color += new Color(0, 0, 0, sleepRate);
            }
            else
            {
                blackImage.color = Color.black;
                fallingAsleep = false;
            }
        }
    }
}

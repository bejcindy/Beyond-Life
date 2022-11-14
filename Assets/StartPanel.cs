using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPanel : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject startButton;

    public AudioClip buttonSound;

    AudioSource AS;
    void Start()
    {
        startButton = transform.GetChild(0).gameObject;
        AS = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            GetComponent<Animator>().enabled = true;
            startButton.GetComponent<Animator>().SetBool("FirstPress", true);
            AS.PlayOneShot(buttonSound);
        }

        if (Input.GetKeyUp(KeyCode.E))
        {
            startButton.GetComponent<Animator>().SetBool("ButtonUp", true);
        }
    }
}

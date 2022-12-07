using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGame : MonoBehaviour

    
{
    public GameObject startButton;
    public Animator buttonAnim;
    public AudioClip buttonSound;


    AudioSource AS;
    // Start is called before the first frame update
    void Start()
    {
        startButton = transform.GetChild(0).gameObject;
        buttonAnim = startButton.GetComponent<Animator>();
        AS = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            GetComponent<Animator>().enabled = true;
            buttonAnim.SetBool("FirstPress", true);

            AS.PlayOneShot(buttonSound);
        }


        if (Input.GetKeyUp(KeyCode.E))
        {
            buttonAnim.SetBool("ButtonUp", true);
            StartCoroutine(loadFirstLevel());
        }
    }

    IEnumerator loadFirstLevel()
    {
        yield return new WaitForSeconds(2.0f);
        AS.enabled = false;
        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadScene("Level1_v1.3");
    }
}

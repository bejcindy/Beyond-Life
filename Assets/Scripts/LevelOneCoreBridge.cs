using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class LevelOneCoreBridge : MonoBehaviour
{
    public GameObject core;
    public GameObject player;
    public Volume cameraVolume;

    private ColorAdjustments CA;
    private VolumeParameter<float> postexposure= new VolumeParameter<float>();
    private VolumeParameter<float> colorfilter= new VolumeParameter<float>();

    public bool endingStart = false;
    // Start is called before the first frame update
    void Start()
    {
        core = GameObject.Find("level1_orb");

        cameraVolume.profile.TryGet<ColorAdjustments>(out CA);

        if (CA == null)
            Debug.LogError("No ColorAdjustments found on profile");
    }

    // Update is called once per frame
    void Update()
    {
        if (endingStart)
        {
            lerpLight();
        }

        if(core.transform.localScale.x >= 45)
        {
            if(CA == null)
            {
                return;
            }
            StartCoroutine(increaseLight());
        }

        if(core.transform.localScale.x >= 200)
        {
            player.GetComponent<CurvePlayerController>().enabled = false;
            Camera.main.GetComponent<CameraController>().enabled = false;
        }

        if(postexposure.value >= 30f)
        {
            StartCoroutine(enterLevelTwo());
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            endingStart = true;
            player = other.gameObject;
        }
    }

    public void lerpLight()
    {
        float distl = Mathf.InverseLerp(12f, 8.5f, Vector3.Distance(player.gameObject.transform.position, core.transform.position));
        float coreScaleVal = Mathf.Lerp(7.492139f, 50f, distl);

        core.transform.localScale = new Vector3(coreScaleVal, coreScaleVal, coreScaleVal);
        Debug.Log(coreScaleVal);
    }

    IEnumerator increaseLight()
    {
        postexposure.value += 3f * Time.deltaTime;
        CA.postExposure.SetValue(postexposure);
        yield return null;
    }

    public IEnumerator enterLevelTwo()
    {
        player.GetComponent<Rigidbody>().isKinematic = true;
        yield return new WaitForSeconds(5.0f);
        SceneManager.LoadScene("Level2.0");
    }
}

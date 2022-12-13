using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CurvePlayerController : MonoBehaviour
{
    public float speed;
    public float lookSpeed = 5.0f;
    public Transform[] legPos;
    public bool onTrack = true;

    bool isLevel1;
    bool[] groundCheck;
    Rigidbody rb;
    float x, z;
    float g = -9.18f;
    float dropX, dropZ;
    Camera cam;
    bool cantMove;
    bool cantLook;

    public int levelOneTunnelPassed = 0;
    public bool messageEntered = false;
    public bool toPort = false;

    public int levelOneSoulFound = 0;
    public GameObject[] levelOneBoards;
    public GameObject[] levelOneSoulFrame;
    public AudioClip boardShrinkSound;
    public Vector3 levelOneBoardScale;
    public Vector3 levelOneFrameScale;

    public GameObject worldAS;
    public GameObject blackImage;
    bool boardShrinkPlayed;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        groundCheck = new bool[4];
        isLevel1 = true;
        cam = Camera.main;

        levelOneBoardScale = levelOneBoards[0].transform.localScale;
        levelOneFrameScale = levelOneSoulFrame[0].transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (!cantMove)
        {
            x = Input.GetAxisRaw("Horizontal");
            z = Input.GetAxisRaw("Vertical");
        }
        else
        {
            x = 0;
            z = 0;
        }
        

        Vector3 direction = (transform.forward * z + transform.right * x).normalized;
        rb.velocity = direction * speed;

        if(rb.velocity.x != 0 || rb.velocity.z != 0)
        {
            GetComponent<Animator>().Play("player_jump", 0, 0);
        }

        if (!cantLook)
        {
            transform.Rotate(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }
        else
        {
            Quaternion toRotation = Quaternion.LookRotation(-transform.position);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, Time.deltaTime*10f);
        }
        

        RaycastHit hit;
        if (Physics.Raycast(transform.position, -transform.up, out hit, 3))
        {
            Physics.gravity = hit.normal * g;
        }

        //Using four corners of the robot to detect whether it's falling or not
        for (int i = 0; i < legPos.Length; i++)
        {
            RaycastHit h;
            if (Physics.Raycast(legPos[i].position, -transform.up, out h, 3))
            {
                Physics.gravity = h.normal * g;
                groundCheck[i] = true;
            }
            else
            {
                groundCheck[i] = false;
            }
        }

        //Add extra force to robot because falling speed is too slow under regular gravity
        if (!groundCheck[0] && !groundCheck[1] && !groundCheck[2] && !groundCheck[3])
        {
            if (!isLevel1)
            {
                rb.AddForce(-transform.up * -g * 4000 * Time.deltaTime, ForceMode.Acceleration);
            }
            else
            {
                //    if (GetComponent<Collider>().enabled)
                //    {
                //        GetComponent<Collider>().enabled = false;
                //    }
                //    cantMove = true;
                //    if (transform.position.y > 50)
                //    {
                        rb.AddForce(-transform.up * -g * 4000 * Time.deltaTime, ForceMode.Acceleration);
                //        dropX = transform.position.x;
                //        dropZ = transform.position.z;
                //        //Debug.Log(dropX);
                //    }
                //    else
                //    {
                //        cantLook = true;
                //        //lerp Y position
                //        float lerpSpeed = Mathf.Lerp(-1f, 4000f, Mathf.Clamp(transform.position.y / 50, 0, 1));
                //        rb.AddForce(-transform.up * -g * lerpSpeed * Time.deltaTime, ForceMode.Acceleration);
                //        cam.GetComponent<CameraController>().Level1End = true;
                //        //lerp X & Z position
                //        //float xClamp = Mathf.Clamp(Mathf.Abs(dropX - transform.position.x) /Mathf.Abs( dropX), 0.001f, 1);
                //        //float zClamp= Mathf.Clamp(Mathf.Abs(dropZ - transform.position.z) /Mathf.Abs( dropZ), 0.001f, 1);
                //        float xLerp = Mathf.Lerp(transform.position.x, 0, 0.001f);
                //        float zLerp= Mathf.Lerp(transform.position.z, 0, 0.001f);
                //        Debug.Log(Mathf.Abs(dropX - transform.position.x) );

                //        transform.position = new Vector3(xLerp, transform.position.y, zLerp);
                //    }
                }
            }


        if(levelOneSoulFound == 8)
        {
            if (!boardShrinkPlayed)
            {
                worldAS.GetComponent<AudioSource>().PlayOneShot(boardShrinkSound);
                boardShrinkPlayed = true;
            }
            StartCoroutine(shrinkLevelOneBoards());

        }


        
    }

    


    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Curve"))
        {
            Vector3 newUp;
            RaycastHit hit;
            if (Physics.Raycast(transform.position, -transform.up, out hit, 3))
            {                
                newUp = hit.normal;
                Vector3 left = Vector3.Cross(transform.forward, newUp);
                Vector3 newForward = Vector3.Cross(newUp, left);
                Quaternion newRotation = Quaternion.LookRotation(newForward, newUp);
                transform.rotation = newRotation;                
            }
        }

        if(other.gameObject.name == "Stop")
        {
            rb.isKinematic = true;
            StartCoroutine(startEnding());

        }
        
    }

    IEnumerator startEnding()
    {
        yield return new WaitForSeconds(5.0f);
        blackImage.GetComponent<Animator>().enabled = true;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Core"))
        {
            SceneTransition("Level2.0");
        }
        if (other.CompareTag("Receiver"))
        {
            GameObject board = other.gameObject.transform.parent.parent.gameObject;
            if (!board.GetComponent<L1SoulLock>().hasSoul)
            {
                if (transform.childCount != 10)
                {
                    transform.GetChild(10).GetComponent<Collider>().enabled = false;
                    transform.GetChild(10).GetComponent<SphereController>().enabled = false;
                    GameObject sphere = transform.GetChild(10).gameObject;
                    other.gameObject.GetComponent<AudioSource>().enabled = true;

                    board.GetComponent<L1SoulLock>().mySoul = sphere;
                    sphere.transform.position = other.transform.position + Vector3.up * 1;
                    sphere.transform.parent = other.gameObject.transform;
                    board.gameObject.GetComponent<L1SoulLock>().hasSoul = true;

                    levelOneSoulFound++;
                }
            }

        }
    }
    IEnumerator SceneTransition(string sceneName)
    {
        yield return new WaitForSeconds(.5f);
        SceneManager.LoadScene(sceneName);
    }


    public IEnumerator shrinkLevelOneBoards()
    {

        
        foreach (GameObject board in levelOneBoards)
        {
            board.GetComponent<L1SoulLock>().unlockAll();
            board.transform.localScale = Vector3.Lerp(board.transform.localScale, 
                                                        new Vector3 (levelOneBoardScale.x * 0.3f, levelOneBoardScale.y, levelOneBoardScale.z * 0.3f), 
                                                        Time.deltaTime*0.5f);
            
        }
        foreach (GameObject frame in levelOneSoulFrame)
        {
            frame.transform.localScale = Vector3.Lerp(frame.transform.localScale, levelOneFrameScale * 0.5f, Time.deltaTime * 1);
        }
        foreach (GameObject board in levelOneBoards) {
            board.GetComponent<L1SoulLock>().lockAll();
        }
        yield return new WaitForSeconds(3f);
        
        toPort = true;
    }
}

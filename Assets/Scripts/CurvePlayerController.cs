using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        groundCheck = new bool[4];
        isLevel1 = true;
        cam = Camera.main;
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
                if (GetComponent<Collider>().enabled)
                {
                    GetComponent<Collider>().enabled = false;
                }
                cantMove = true;
                if (transform.position.y > 50)
                {
                    rb.AddForce(-transform.up * -g * 4000 * Time.deltaTime, ForceMode.Acceleration);
                    dropX = transform.position.x;
                    dropZ = transform.position.z;
                    //Debug.Log(dropX);
                }
                else
                {
                    cantLook = true;
                    //lerp Y position
                    float lerpSpeed = Mathf.Lerp(-1f, 4000f, Mathf.Clamp(transform.position.y / 50, 0, 1));
                    rb.AddForce(-transform.up * -g * lerpSpeed * Time.deltaTime, ForceMode.Acceleration);
                    cam.GetComponent<CameraController>().Level1End = true;
                    //lerp X & Z position
                    //float xClamp = Mathf.Clamp(Mathf.Abs(dropX - transform.position.x) /Mathf.Abs( dropX), 0.001f, 1);
                    //float zClamp= Mathf.Clamp(Mathf.Abs(dropZ - transform.position.z) /Mathf.Abs( dropZ), 0.001f, 1);
                    float xLerp = Mathf.Lerp(transform.position.x, 0, 0.001f);
                    float zLerp= Mathf.Lerp(transform.position.z, 0, 0.001f);
                    Debug.Log(Mathf.Abs(dropX - transform.position.x) );

                    transform.position = new Vector3(xLerp, transform.position.y, zLerp);
                }
            }
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
    }

    
}

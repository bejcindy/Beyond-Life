using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurvePlayerController : MonoBehaviour
{
    public float speed;
    float x, z;
    float rotationX;
    public float lookSpeed = 5.0f;
    public float lookXLimit = 45.0f;
    public float g;
    public Transform[] legPos;
    public Transform cam;

    bool[] groundCheck;
    Rigidbody rb;

    bool offground;
    bool parallelToGround;
    Vector3 currentDirection;
    Vector3 zeroX, zeroY, zeroZ;
    bool zX, zY, zZ;
    bool activeAxisX;
    float rot;
    Vector3 realUp;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        parallelToGround = true;
        //legPos = new Transform[4];
        groundCheck = new bool[4];
        
    }

    // Update is called once per frame
    void Update()
    {
        x = Input.GetAxisRaw("Horizontal");
        z = Input.GetAxisRaw("Vertical");
        Debug.Log(transform.up);
        Vector3 direction = (transform.forward * z + transform.right * x).normalized;
        //zeroY = new Vector3(direction.x, 0, direction.z);
        //zeroZ = new Vector3(direction.x, direction.y, 0);
        //zeroX = new Vector3(0, direction.y, direction.z);

        rb.velocity = direction * speed;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, -transform.up, out hit, 3))
        {
            Physics.gravity = hit.normal * g;
            offground = false;
        }

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
        //Debug.DrawRay(legPos[0].position, -transform.up, Color.blue);
        //Debug.Log(groundCheck[0] + " " + groundCheck[1] + " " + groundCheck[2] + " " + groundCheck[3] + " ");
        if (!groundCheck[0] && !groundCheck[1] && !groundCheck[2] && !groundCheck[3])
        {
            rb.AddForce(-transform.up * -g * 4000 * Time.deltaTime, ForceMode.Acceleration);
            Debug.Log("here");
        }
        

        transform.Rotate(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
    }

  
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Curve"))
        {
            //cam.GetComponent<CameraController>().waitForCurve = true;
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
                //Physics.gravity = (transform.position - hit.point).normalized * -9.81f;
                //Physics.gravity = hit.normal * -9.81f;
                //transform.rotation *= Quaternion.FromToRotation(transform.up, hit.normal);
                //transform.up += hit.normal;
                newUp = hit.normal;
                Vector3 left = Vector3.Cross(transform.forward, newUp);
                Vector3 newForward = Vector3.Cross(newUp, left);
                Quaternion newRotation = Quaternion.LookRotation(newForward, newUp);
                transform.rotation = newRotation;
                //Debug.Log(hit.normal);
                //realUp = (hit.point - transform.position).normalized;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Curve"))
        {
            //cam.GetComponent<CameraController>().waitForCurve = false;
        }
    }
}

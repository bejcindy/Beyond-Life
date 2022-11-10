using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurvePlayerController : MonoBehaviour
{
    public float speed;
    public float lookSpeed = 5.0f;
    public Transform[] legPos;

    bool[] groundCheck;
    Rigidbody rb;
    float x, z;
    float g = -9.18f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        groundCheck = new bool[4];
        
    }

    // Update is called once per frame
    void Update()
    {
        x = Input.GetAxisRaw("Horizontal");
        z = Input.GetAxisRaw("Vertical");
        Vector3 direction = (transform.forward * z + transform.right * x).normalized;
        rb.velocity = direction * speed;
        transform.Rotate(0, Input.GetAxis("Mouse X") * lookSpeed, 0);

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
            rb.AddForce(-transform.up * -g * 4000 * Time.deltaTime, ForceMode.Acceleration);
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

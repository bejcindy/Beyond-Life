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

    public Transform cam;

    Rigidbody rb;

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
    }

    // Update is called once per frame
    void Update()
    {
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");
        Vector3 direction = (transform.forward * z + transform.right * x).normalized;
        //zeroY = new Vector3(direction.x, 0, direction.z);
        //zeroZ = new Vector3(direction.x, direction.y, 0);
        //zeroX = new Vector3(0, direction.y, direction.z);

        rb.velocity = direction * speed;

        //transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        //transform.LookAt(mouseWorldSpace, realUp);

        //forward
        //Vector3 newUp;
        //RaycastHit hit;
        //if (Physics.Raycast(transform.position, -transform.up, out hit, 3))
        //{
        //    //transform.rotation *= (Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0) * Quaternion.FromToRotation(transform.up, hit.normal));
        //    //transform.rotation *= Quaternion.FromToRotation(transform.up, hit.normal);
        //    //transform.Rotate(Quaternion.FromToRotation(transform.up, hit.normal).eulerAngles.x, 0, Quaternion.FromToRotation(transform.up, hit.normal).eulerAngles.z);
        //    newUp = hit.normal;
        //    Vector3 left = Vector3.Cross(transform.forward, newUp);
        //    Vector3 newForward = Vector3.Cross(newUp, left);
        //    Quaternion newRotation = Quaternion.LookRotation(newForward, newUp);
        //    transform.rotation = newRotation;
        //    //Debug.Log(hit.normal);
        //    //realUp = (hit.point - transform.position).normalized;
        //    //Debug.DrawRay(transform.position, -transform.right*3f, Color.red);
        //}
        
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
                Physics.gravity = hit.normal * -9.81f;
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

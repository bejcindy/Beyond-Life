using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerNav : MonoBehaviour
{
    NavMeshAgent nav;

    // Start is called before the first frame update
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 wasd = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if (wasd.y != 0||wasd.x!=0)
        {
            nav.destination = transform.position + transform.right * wasd.x + transform.forward * wasd.y;
        }
        else
        {
            nav.destination = transform.position;
        }
        if (Input.GetAxis("Mouse X") != 0)
        {
            transform.Rotate(0, Input.GetAxis("Mouse X") * nav.angularSpeed * Time.deltaTime, 0);
        }

        //RaycastHit hit;
        //if (Physics.Raycast(transform.position, -Vector3.up, out hit, 3))
        //{
        //    //Physics.gravity = (transform.position - hit.point).normalized * -9.81f;
        //    Physics.gravity = hit.normal * -9.81f;
        //    Debug.Log(Physics.gravity);
        //}
    }
}

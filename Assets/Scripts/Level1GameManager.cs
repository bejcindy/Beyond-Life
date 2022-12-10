using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1GameManager : MonoBehaviour
{
    public Transform[] boards;

    public int topBoard;

    Vector3[] newPos;

    Vector3 topBoardPos;
    Vector3 destination = new Vector3(-3, -1.5f, -25.8f);
    Vector3 posDiff = new Vector3(7.7f, 4.4f, 10.55f);
    Vector3 posDiffx = new Vector3(7.7f, 0, 0);
    Vector3 posDiffy = new Vector3(0, 4.4f, 0);
    Vector3 posDiffz = new Vector3(0, 0, 10.55f);
    Vector3 angleDiff = new Vector3(0, -8, -8);

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //move to new pos
            for(int i = 0; i < boards.Length; i++)
            {
                
                Vector3 direction = Vector3.Normalize(destination-boards[i].position);
                Vector3 offset = boards[i].position + direction * Vector3.Distance(destination, boards[0].position)/7 * i;
                boards[i].position = offset;
                boards[i].eulerAngles -= angleDiff;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [HideInInspector]public Transform player;
    [Header("Camera offset by Y and Z")]
    public float y;
    public float z;
    //public float x;

    public float cameraSmooth;

    void Start()
    {
        player = GameObject.Find("run").GetComponent<Transform>();
    }

    void FixedUpdate()
    {
        Vector3 newPos = new Vector3(transform.position.x, player.position.y+2 + y, player.position.z + z+3.5f);
        transform.position = Vector3.Lerp(transform.position, newPos, cameraSmooth); //The camera follows the player, y and x is the offset
    } 

    public void Finished()
    {
        y = 4;
        z = -8;
        //x = -11;
    }
}

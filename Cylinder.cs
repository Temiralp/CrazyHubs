using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cylinder : MonoBehaviour
{
    [Header("Animation Cylinder/Animation")]
    public Transform anim;

    public Vector3 rotationAngle;
    public float rotationSpeed;

    [HideInInspector]public bool Ground = true, Dead = false;


    void FixedUpdate()
    {
        if (PlayerPrefs.GetInt("isStarted") == 1)
        {
            anim.transform.Rotate(rotationAngle * rotationSpeed * Time.deltaTime); //Cylinder rotating animation
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "GroundOn") //The last cylinder passes the values ​​to the first - the first master
        {
            Ground = true;
        }
    }
    private void OnTriggerEnter(Collider target)
    {
        if (target.tag == "GroundOff")
        {
            Ground = false;
        }
        if (target.tag == "Dead")
        {
            Dead = true;
        }
    }
}
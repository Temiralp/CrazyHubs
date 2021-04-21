using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPos : MonoBehaviour
{
    [HideInInspector]public CylinderCont cylinder;

    void Start()
    {
        cylinder = GameObject.Find("/Players/Cylinder").GetComponent<CylinderCont>();
    }

    void LateUpdate()
    {
        transform.position = new Vector3(cylinder.transform.position.x, transform.position.y, cylinder.transform.position.z); //Player movement behind the cylinder from above
    }

    void OnCollisionEnter(Collision collision) 
    {
        if (collision.collider.tag == "GroundOn")
        {
            if (transform.position.z >= cylinder.posStart + cylinder.distance)
            {
                cylinder.animator.Play("dead");
                cylinder.playerrb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionX;
            }
        }
    }
}
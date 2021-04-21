using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnOnTrigger : MonoBehaviour
{
    [HideInInspector]public float distance, posStart;

    [Header("The platform on which to place the spawner.")]
    public Transform thisHouse;
    [Header("The platform to which the road will be built.")]
    public Transform nextHouse;

    [Header("If this is a spawn road at the finish line. Finish = true.")]
    public bool Finish = false;

    [HideInInspector]public CylinderCont cont;
    [HideInInspector]public bool check = false;

    void Awake()
    {
        posStart = transform.position.z; //We calculate the place of the beginning of the spawn of the road.

        cont = GameObject.Find("Players/Cylinder").GetComponent<CylinderCont>();

        if (Finish != true)
        {
            float pos_1;
            float pos_2;

            pos_1 = thisHouse.localPosition.z + thisHouse.localScale.z / 2;
            pos_2 = nextHouse.localPosition.z - nextHouse.localScale.z / 2;

            distance = pos_2 - pos_1;
        }
        else
        {
            distance = 250;
        }
    }

    private void OnTriggerEnter(Collider target)
    {
        if (target.tag == "Player")
        {
            if (check == false) //When touching the player, we pass him the coordinates of the start of the spawn, the distance, turn on the spawn, turn off the Ground, if this is a spawn at the finish, then Finish = true;
            {
                cont.posStart = posStart;
                cont.distance = distance;
                cont.Spawn = true;
                cont.Ground = false;
                cont.Finish = Finish;

                check = true;
            }
        }
    }
}

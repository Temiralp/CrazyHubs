using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemy : MonoBehaviour
{
    public Transform target;
    public NavMeshAgent nav;


    // Start is called before the first frame update
    void Start()
    {

        nav = GetComponent<NavMeshAgent>();

    }

    // Update is called once per frame
    void Update()
    {
        //nav.SetDestination(target.position);
    }


    void OnCollisionEnter(Collision col)
    {
        //When enemy touch main player destroy main player child object
        if (col.gameObject.tag.Equals("mainplayer"))
        {
            Destroy(col.gameObject);
            Debug.Log("hit");

        //We will code here for die main player



        }

    }
}

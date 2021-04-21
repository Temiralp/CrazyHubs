using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDistance : MonoBehaviour
{
    [HideInInspector]public Transform finish;

    [HideInInspector]public float levelDistance;
    [HideInInspector]public float oneProcent;
    [HideInInspector]public float Procent;

    [HideInInspector]public Transform cylinder;

    void Start()
    {
        finish = GameObject.Find("Finish").GetComponent<Transform>();
        cylinder = GameObject.Find("Players/Cylinder").GetComponent<Transform>();

        levelDistance = finish.localPosition.z;

        oneProcent = levelDistance / 100; //Determine the distance to the finish line as a percentage
    }

    
    void Update()
    {
        Procent = cylinder.transform.position.z / oneProcent;
        PlayerPrefs.SetFloat("LevelLine", Procent);
    }
}

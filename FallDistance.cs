using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDistance : MonoBehaviour
{
    [Header("The distance a player can fall after touching an obstacle")]
    public float fallDistance;
    [HideInInspector]public bool check;

    private void OnTriggerEnter(Collider target)
    {
        if (target.tag == "Player")
        {
            if (check == false)
            {
                PlayerPrefs.SetFloat("FallDistance", fallDistance); //Sets how far the player must fall in order to fall not on obstacles. In general, you can delete, but then don't forget to delete the objects on the first level.
                check = true;
            }
        }
    }
}
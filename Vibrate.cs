using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vibrate : MonoBehaviour
{
    private void OnTriggerEnter(Collider target)
    {
        if (target.tag == "Player")
        {
            if (PlayerPrefs.GetInt("VibrationOn") == 1)
            {
                Vibration.Vibrate(60);

            }
        }
    }
}

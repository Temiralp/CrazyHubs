using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coins : MonoBehaviour
{
    [HideInInspector]public int coins; //Total number of coins
    [HideInInspector]public bool check = false;


    private void OnTriggerEnter(Collider target)
    {
        if (target.tag == "OffAll") //The same as in Adders Script, when you touch the OffAll object, we delete the coin for optimization
        {
            Destroy(gameObject);
        }

        if (target.tag == "Player") //Add coins, you can put any value, 5, 10, by default I have 3.
        {
            if (check == false)
            {
                coins = PlayerPrefs.GetInt("Coins");
                coins = coins + 3;
                PlayerPrefs.SetInt("Coins", coins);

                if (PlayerPrefs.GetInt("VibrationOn") == 1) //If vibration is not turned off, the phone vibrates.
                {
                    Vibration.Vibrate(60);
                }

                check = true;



                Destroy(this.gameObject);
            }
        }
    }
}

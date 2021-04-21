using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoBehaviour
{
    [Header("X score: 1,2,3..")]
    public int x;
    [Header("The number of coins that the player will receive")]
    public int addCoins;
    private int Coin;

    [HideInInspector]public CylinderCont cylinder;
    [HideInInspector]public AudioSource sound;
    private bool check = false;

     void Start()
    {
        cylinder = GameObject.Find("Players/Cylinder").GetComponent<CylinderCont>();
        sound = GameObject.Find("Finish").GetComponent<AudioSource>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player_") //When the player, namely the Player, touches the finish, add more to the total number of coins, set the finish points equal to X, victory = 1. Turn on the animation of victory.
        {
            if (check == false)
            {
                Coin = PlayerPrefs.GetInt("Coins");
                Coin = Coin + addCoins;
                PlayerPrefs.SetInt("Coins", Coin);

                PlayerPrefs.SetInt("xScore", x);
                PlayerPrefs.SetInt("isWin", 1);

                cylinder.animator.SetBool("win", true);

                sound.Play();

                Vibration.Vibrate(100);

                check = true;
            }
        }
    }
}
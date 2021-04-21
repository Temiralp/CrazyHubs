using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Adders : MonoBehaviour
{
    [HideInInspector] public float Scale; //Player's last cylinder size
    [Header("How much the last cylinder increases when one Add'r is touched")]
    public float addScale; //How much the last cylinder increases when one Add'r is touched

    [HideInInspector]public float Score; //Total score

    private void OnTriggerEnter(Collider target)
    {
        if (target.tag == "OffAll") //When you touch the OffAll object, turn off Adder for optimization
        {
            Destroy(gameObject);
        }

        if (target.tag == "Player") //When the player touches Adder, we increase the cylinder, add points and turn on vibration if the player has not turned it off
        {
            Scale = PlayerPrefs.GetFloat("Scale");
            Scale = Scale + addScale;
            PlayerPrefs.SetFloat("Scale", Scale);

            Score = PlayerPrefs.GetFloat("Score");
            Score = Score + 0.10714f;
            PlayerPrefs.SetFloat("Score", Score);

            Destroy(this.gameObject); //Removing an object from the scene
        }
    }
}

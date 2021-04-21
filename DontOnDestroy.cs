using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class DontOnDestroy : MonoBehaviour
{
    [HideInInspector]public int levelId;

    void Start()
    {
        levelId = PlayerPrefs.GetInt("LevelId");

        DontDestroyOnLoad(gameObject); //When you turn on the game, load the current level in the menu.
        SceneManager.LoadScene(levelId);
    }

}

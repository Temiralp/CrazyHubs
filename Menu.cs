using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class Menu : MonoBehaviour
{
    [Header("Test Mode")]
    public bool TestingMode = false;
    public int TestingLevelId;

    [Header("Settings Files path:Sprites/Buttons")]
    public Image sound;
    public Image vibration;

    public Sprite soundOn;
    public Sprite soundOff;
    public Sprite vibrationOn;
    public Sprite vibrationOff;

    public GameObject SettingsPanel;

    [Header("Setting button animator")]
    public Animator anim;

    [Header("StartMenu")]
    public GameObject StartMenu;

    [Header("Canvas/StartMenu/Coins CoinsNumber")]
    public Text CoinsNumber_One;

    [Header("Canvas/StartMenu/Menu/Levels")]
    public Image level_1;
    public Image level_2;
    public Image level_3;
    public Image level_4;

    [Header("Path:Sprites/Levels")]
    public Sprite levelOpen;
    public Sprite levelClose;

    [Header("Canvas/StartMenu/Menu/Levels/level..")]
    public Text level1;
    public Text level2;
    public Text level3;
    public Text level4;

    [Header("InGame")]
    public Text CoinsNumber_Two;
    public Text Score_One;
    public GameObject InGame;

    public Text levelNumber_One;
    public Slider levelLine_One;

    [HideInInspector]public int GamesPlayed;

    public Animator score_;

    [Header("GameOver")]
    public Text Score_Two;
    public GameObject GameOver;

    public Text levelNumber_Two;
    public Slider levelLine_Two;

    [Header("Win")]
    public Text xScore;
    public Text Score_Three;
    public Text CoinsNumber_Three;
    public GameObject Win;

    public Animator win_l;
    public Animator win_r;
    public Animator W_xScore;
    public Animator W_Score, W_Coins;

    [Header("Pause")]
    [HideInInspector]public bool Pause;
    public GameObject panelPause;

    [Header("Сompliment")]
    [HideInInspector] public bool ComplimentComplete;
    [HideInInspector] public bool TimerOn;
    public Text[] compliments;
    [HideInInspector] public int num, numMax = 6;

    [HideInInspector] public int time;

    [Header("The time that the compliment is shown")]
    public int timeMax;

    [Header("Sounds")]
    public AudioSource audio_2;

    [HideInInspector] public int score;
    [HideInInspector] public int levelId;

    [Header("Vibration length when assembling Adders. Default = 40")]
    public int vibrationPower = 40;

    [Header("Ads")]
    public AdsManager adScript;

    void Awake()
    {
        StartUp(); //Loading data
        numMax = compliments.Length; //Compliments numbers

        levelId = PlayerPrefs.GetInt("LevelId");
    }

    void Update()
    {
        if (score != (int) PlayerPrefs.GetFloat("Score")) //If the account has changed, change it on the screen
        {
            score = (int)PlayerPrefs.GetFloat("Score"); //Convert the count from float to int and display it on the screen.
            score_.Play("Score");

            audio_2.Play();

            if (PlayerPrefs.GetInt("VibrationOn") == 1)
            {
                Vibration.Vibrate(vibrationPower);
            }
        }

        if (PlayerPrefs.GetInt("isDeaded") == 1) //When the player has lost, turn on the losing scene.
        {
                InGame.SetActive(false);
                GameOver.SetActive(true);
        }
        if (PlayerPrefs.GetInt("isWin") == 1)//When the player has won, we turn on the victory scene.
        {
            InGame.SetActive(false);
            Win.SetActive(true);

            adScript.HideBanner();
        }

        if (StartMenu.active)//If this is the start screen, set the level icons and the number of coins.
        {
            CoinsNumber_One.text = "" + PlayerPrefs.GetInt("Coins");
          
            Levels();
        }

        if (InGame.active)//If the game is on
        {
            CoinsNumber_Two.text = "" + PlayerPrefs.GetInt("Coins");
            Score_One.text = "" + score; //We update the score.

            LevelNow(); //Updating level progress

            if (PlayerPrefs.GetInt("GamesPlayed") != 1) //If this is not the first game
            {
                if (PlayerPrefs.GetInt("CylinderNum") == 7)
                {
                    if (ComplimentComplete == false)
                    {
                        DoCompliment(); //If the player scored seven cylinders, include a compliment
                    }
                }
                else
                {
                    if (PlayerPrefs.GetInt("CylinderNum") <= 4)//If there are fewer cylinders or = 4, the complement can be turned on again if there are 7 cylinders
                    {
                        ComplimentComplete = false;
                    }
                }
                if (TimerOn == true) //Compliment Off Timer
                {
                    time++;
                    if (time > timeMax)
                    {
                        compliments[num].enabled = false;
                        TimerOn = false;
                    }
                }
            }
        }
        if (GameOver.active) // If the game fails and a losing scene is open
        {
            Score_Two.text = "" + score;

            LevelLast();
        }
        if (Win.active) //When you win 
        {
            CoinsNumber_Three.text = "" + PlayerPrefs.GetInt("Coins");
            Score_Three.text = "" + score;
            xScore.text = "x" + PlayerPrefs.GetInt("xScore");

        }
    }

    void OnApplicationPause(bool pauseStatus) //Pause while playing
    {
        if (InGame.active == true)
        {
            Pause = true;
            panelPause.SetActive(true);

            PlayerPrefs.SetInt("isStarted", 0);
        }
    }

    void DoCompliment() //Turn off all compliment texts, choose one randomly and turn on the text and timer
    {
        compliments[1].enabled = false;
        compliments[2].enabled = false;
        compliments[3].enabled = false;
        compliments[4].enabled = false;
        compliments[5].enabled = false;

        num = Random.Range(1, numMax);
        compliments[num].enabled = true;

        ComplimentComplete = true;
        TimerOn = true;
        time = 0;
    }
    void Levels() //We set the level icons in the menu, depending on the level
    {
        switch (PlayerPrefs.GetInt("LevelId"))
        {
            case 1:
                level_1.sprite = levelClose;
                level_2.sprite = levelClose;
                level_3.sprite = levelClose;
                level_4.sprite = levelClose;
                break;
            case 2:
                level_1.sprite = levelOpen;
                level_2.sprite = levelClose;
                level_3.sprite = levelClose;
                level_4.sprite = levelClose;
                break;
            case 3:
                level_1.sprite = levelOpen;
                level_2.sprite = levelOpen;
                level_3.sprite = levelClose;
                level_4.sprite = levelClose;
                break;
            case 4:
                level_1.sprite = levelOpen;
                level_2.sprite = levelOpen;
                level_3.sprite = levelOpen;
                level_4.sprite = levelClose;
                break;
            case 5:
                level_1.sprite = levelOpen;
                level_2.sprite = levelOpen;
                level_3.sprite = levelOpen;
                level_4.sprite = levelOpen;
                break;
    

        }

        if (PlayerPrefs.GetInt("LevelId") < 6)
        {
            level1.text = "1";
            level2.text = "2";
            level3.text = "3";
            level4.text = "4";
        }
    }

     void LevelNow() //Set the value of the current level
    {
        levelNumber_One.text = "" + PlayerPrefs.GetInt("LevelId");
        levelLine_One.value = PlayerPrefs.GetFloat("LevelLine");
    }

     void LevelLast() //Setting the value of the previous level
    {
        levelNumber_Two.text = "" + PlayerPrefs.GetInt("LevelId");
        levelLine_Two.value = PlayerPrefs.GetFloat("LevelLine");
    }

    public void OnClick_Settings()
    {
        if (SettingsPanel.active == false) //Turn settings on or off
        {
            anim.Play("SettingsOpen");

            SettingsPanel.SetActive(true);

         
        }
        else
        {
            anim.Play("SettingsClose");

            SettingsPanel.SetActive(false);

        
        }
    }
    public void OnClick_Sound() //Turn the sound on or off
    {
        if (PlayerPrefs.GetInt("SoundOn") == 1)
        {
            AudioListener.volume = 0;
            PlayerPrefs.SetInt("SoundOn", 0);

            sound.sprite = soundOff;

       
        }
        else
        {
            AudioListener.volume = 1;
            PlayerPrefs.SetInt("SoundOn", 1);

            sound.sprite = soundOn;

        
        }
    }
    public void OnClick_Vibration() //Turn vibration on or off
    {
        if (PlayerPrefs.GetInt("VibrationOn") == 1)
        {

            PlayerPrefs.SetInt("VibrationOn", 0);
            vibration.sprite = vibrationOff;

        
        }
        else
        {
            Vibration.Vibrate(100);
            PlayerPrefs.SetInt("VibrationOn", 1);
            vibration.sprite = vibrationOn;

        
        }
    }

    public void OnClick_Play() //When you press the play button, set everything to default and start the game
    {
        StartMenu.SetActive(false);
        InGame.SetActive(true);

        PlayerPrefs.SetFloat("LevelLine", 0); //Set to null
        PlayerPrefs.SetFloat("Score", 0); // Set to null
        PlayerPrefs.SetInt("xScore", 0); //Set to null

        GamesPlayed = PlayerPrefs.GetInt("GamesPlayed");
        GamesPlayed = GamesPlayed + 1;
        PlayerPrefs.SetInt("GamesPlayed", GamesPlayed);

        PlayerPrefs.SetInt("isStarted", 1);
    }

    public void OnClick_Gameover() //When you press the game over button, set everything to default and start the game
    {
        GameOver.SetActive(false);
        StartMenu.SetActive(true);

        PlayerPrefs.SetInt("isDeaded", 0); //Set to null
        PlayerPrefs.SetFloat("LevelLine", 0); //Set to null
        PlayerPrefs.SetFloat("Score", 0); // Set to null
        PlayerPrefs.SetInt("xScore", 0); //Set to null

        if (levelId > adScript.levelId && adScript.everyLose)
        {
            adScript.ShowInterstitialAd();
            Debug.Log("1" + levelId);
        }
        else
        {
            Debug.Log("0" + levelId);
        }

        SceneManager.LoadScene(PlayerPrefs.GetInt("LevelId"));
    }
    public void OnClick_Win()   //When you press the win button on finish, set everything to default and start the game
    {
        Win.SetActive(false);
        StartMenu.SetActive(true);

        PlayerPrefs.SetInt("isWin", 0); //Set to null
        PlayerPrefs.SetFloat("LevelLine", 0); //Set to null
        PlayerPrefs.SetFloat("Score", 0); // Set to null
        PlayerPrefs.SetInt("xScore", 0); //Set to null

        levelId = PlayerPrefs.GetInt("LevelId");

        if (TestingMode)
        {

        }
        else
        {
            levelId = levelId + 1;
            if (levelId > adScript.levelId && adScript.everyVictory)
            {
                adScript.ShowInterstitialAd();
            }
        }
        PlayerPrefs.SetInt("LevelId", levelId);

        SceneManager.LoadScene(PlayerPrefs.GetInt("LevelId"));
    }

    public void OnClick_ContinueGame() //When you press the continue button in pause, set everything to default and start the game
    {
        Pause = false;
        panelPause.SetActive(false);

        PlayerPrefs.SetInt("isStarted", 1);

    }

    void StartUp() //Default saves
    {
        if (PlayerPrefs.GetInt("DataSetted") == 1)
        {
            PlayerPrefs.SetInt("isDeaded", 0); //Set to null
            PlayerPrefs.SetInt("isWin", 0); //Set to null
            PlayerPrefs.SetInt("isStarted", 0); //Set to null

            PlayerPrefs.SetFloat("LevelLine", 0); //Set to null
            PlayerPrefs.SetFloat("Score", 0); // Set to null
            PlayerPrefs.SetInt("xScore", 0); //Set to null

            if (TestingMode)
            {
                PlayerPrefs.SetInt("LevelId", TestingLevelId); //Set to 1
                PlayerPrefs.SetInt("GamesPlayed", 2);
            }

        }
        else //Если игра запускается первый раз
        {
            PlayerPrefs.SetInt("DataSetted", 1);

            PlayerPrefs.SetInt("SoundOn", 1);
            PlayerPrefs.SetInt("VibrationOn", 1);

            PlayerPrefs.SetInt("NoAds", 0);

            if (TestingMode)
            {
                PlayerPrefs.SetInt("LevelId", TestingLevelId); //Set to 1
                PlayerPrefs.SetInt("GamesPlayed", 2);
            }
            else
            {
                PlayerPrefs.SetInt("LevelId", 1); //Set to 1
                PlayerPrefs.SetInt("GamesPlayed", 0);
            }

            PlayerPrefs.SetInt("Coins", 0); // Set to null

            PlayerPrefs.SetInt("isStarted", 0); //Set to null
            PlayerPrefs.SetInt("isDeaded", 0); //Set to null
            PlayerPrefs.SetInt("isWin", 0); //Set to null

            PlayerPrefs.SetFloat("LevelLine", 0); //Set to null
            PlayerPrefs.SetFloat("Score", 0); // Set to null
            PlayerPrefs.SetInt("xScore", 0); //Set to null
            
        }
    }
  
}

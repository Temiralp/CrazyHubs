using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Input = InputWrapper.Input; //This is needed to simulate touches on a computer. Remove the comment if you are testing the game on a pc, not on a smartphone.

public class CylinderCont : MonoBehaviour
{
    [Header("Cylinders Number at this level")]
    public int cylindersNum;
    public List<Cylinder> cylinders = new List<Cylinder>();

    [Header("Cylinders prefab. Path:Prefabs/Players/Cylinder 2")]
    public Cylinder prefab;

    [HideInInspector]public Transform parent;

    [HideInInspector]public float maxScale = 0.6f;
    [HideInInspector] public int isStarted;

    [Header("Player Speed")]
    public float speed;

    [HideInInspector] public float nowSpeed = 0.025f;
    [HideInInspector] public float smooth = 8.0f;
    private Touch touch;

    [Header("Touch speed")]
    public float defaultSpeed = 0.025f;
    [HideInInspector] public float WidthNow;
    [HideInInspector] public float factor;

    [HideInInspector] public float Scale;

    [HideInInspector] public Animator animator;
    [HideInInspector] public PlayerPos playerpos;
    [HideInInspector] public Rigidbody playerrb;

    [Header("SpawnBlock Prefab")]
    public SpawnBlock SpawnBlock;

    private List<SpawnBlock> spawnedBlocks = new List<SpawnBlock>();
    [HideInInspector] public bool Spawn, Stretch, Finish;
    [HideInInspector] public float zScale, minusScale;
    private Vector3 offset;
    private Rigidbody rb;

    [HideInInspector] public float distance, hasDistance = 0;
    [HideInInspector] public float posWill;

    [HideInInspector] public float zDistance, posStart;
    [HideInInspector] public float xPosOfSpawn, xPosM, xPosA;
    [HideInInspector] public float xPosChange, yPos;

    [HideInInspector]public CameraFollow cam;

    [HideInInspector]public bool Ground;

    [HideInInspector] public int score;

    [HideInInspector] public int time;

    [HideInInspector]public int timeMax;
    [HideInInspector] public bool timerOn;

    public int platformNum;
    public Transform[] secondChance;

    void Start()
    {
        PlayerPrefs.SetFloat("Scale", maxScale); //Setting the default cylinder dimensions
        Scale = maxScale;
        ChangeScale();
        
        isStarted = 0; //The game is not started by default
        score = 0;

        PlayerPrefs.SetInt("CylinderNum", cylindersNum);

        WidthNow = Screen.width; //The default is 720
        factor = 1440 / WidthNow; //Calculating the screen resolution ratio
        nowSpeed = defaultSpeed * factor; //The speed of movement of the character on different phones will be different, due to different screen resolutions. With this function, it will be the same

        parent = GameObject.Find("Players").GetComponent<Transform>();

        animator = GameObject.Find("Player").GetComponent<Animator>();
        playerpos = GameObject.Find("Player").GetComponent<PlayerPos>();
        playerrb = GameObject.Find("Player").GetComponent<Rigidbody>();

        rb = gameObject.GetComponent<Rigidbody>();

        cam = GameObject.Find("MainCamera").GetComponent<CameraFollow>();
    }


    void FixedUpdate()
    {

        if (isStarted != PlayerPrefs.GetInt("isStarted")) //If the variable changes
        {
            isStarted = PlayerPrefs.GetInt("isStarted");

            if (isStarted == 1)  
            {
                if (Ground == true) //If on the ground, it means running
                {
                    animator.Play("run");
                }
            }
            else
            {
                if (PlayerPrefs.GetInt("isDeaded") != 1 & Finish != true) //If the player has not lost and is not at the finish line, then he simply stands
                {
                    animator.Play("idle");
                }
            }
        }

        if (isStarted == 1) // If game is not started
        {
            if (Input.touchCount > 0) //Controlling the character left-right
            {
                touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Moved)
                {
                    if (Finish != true)
                    {
                        if (Ground == true)
                        {
                            if (Stretch == false)
                            {
                                Vector3 newPos = new Vector3(Mathf.Clamp(transform.position.x + touch.deltaPosition.x * nowSpeed, -1.9f, 1.9f), transform.position.y, transform.position.z); //The player's position change is limited, maximum -1.9f and 1.9f by x
                                transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime * smooth); //We change the coordinates of the player, to those that we just calculated
                            }
                            else
                            {
                                Vector3 newPos = new Vector3(Mathf.Clamp(transform.position.x + touch.deltaPosition.x * nowSpeed, -4.2f, 4.2f), transform.position.y, transform.position.z); //The player's position change is limited, maximum -4.2f and 4.2f by x
                                transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime * smooth); //We change the coordinates of the player, to those that we just calculated
                            }
                        }
                        else
                        {
                            Vector3 newPos = new Vector3(Mathf.Clamp(transform.position.x + touch.deltaPosition.x * nowSpeed, -4.2f, 4.2f), transform.position.y, transform.position.z); //The player's position change is limited, maximum -4.2f and 4.2f by x
                            transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime * smooth); //We change the coordinates of the player, to those that we just calculated
                        }
                    }
                }
            }

            transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.World); //Forward movement

            if (Scale != PlayerPrefs.GetFloat("Scale")) //If the cylinder size has changed
            {
                Scale = PlayerPrefs.GetFloat("Scale"); //Get it

                if (Scale <= 0) 
                {
                    if (cylindersNum > 1) //If there are more than 1 cylinders, then delete the last one
                    {
                        cylinders[cylinders.Count - 1].transform.localScale = new Vector3(0, 0.6f, 0);
                        ClearLastCylinder();
                    }
                    else //if he is alone
                    {

                        if (Stretch == false) //If the road spawns
                        {
                            IsDeadForScale();

                        }
                        else
                        {
                            IsDeadForScaleTwo();
                            Stretch = false;
                        }
                    }
                }
                ChangeScale(); //Change Scale
            }
            if (Spawn == true) //If spawn = true, spawn block
            {
                SpawnBl();
            }
            if (Stretch == true) //If the road is every 0.0625f we decrease the cylinder by minusScale
            {
                if (zDistance - hasDistance >= 0.0625f)
                {
                    PlayerPrefs.SetFloat("Scale", Scale - minusScale);

                    if (zDistance - hasDistance >= 0.0625f)
                    {
                        PlayerPrefs.SetFloat("Scale", Scale - minusScale);

                        if (zDistance - hasDistance >= 0.0625f)
                        {
                            PlayerPrefs.SetFloat("Scale", Scale - minusScale);

                            if (zDistance - hasDistance >= 0.0625f)
                            {
                                PlayerPrefs.SetFloat("Scale", Scale - minusScale);

                                if (zDistance - hasDistance >= 0.0625f)
                                {
                                    PlayerPrefs.SetFloat("Scale", Scale - minusScale);
                                }
                                else
                                {
                                    hasDistance = zDistance;
                                }
                            }
                            else
                            {
                                hasDistance = zDistance;
                            }
                        }
                        else
                        {
                            hasDistance = zDistance;
                        }
                    }
                    else
                    {
                        hasDistance = zDistance;
                    }
                }

                if (transform.position.x < xPosM || transform.position.x > xPosA) //If the player leaves the road to the right or left, the road is no longer built
                {
                    Stretch = false;
                }

                if (transform.position.z >= posWill) //If the player has built a road to the end, turn off road building
                {
                    Stretch = false;
                }
                if (transform.position.z > posStart) //We calculate the coordinates of where to spawn the road
                {
                    zDistance = transform.position.z - posStart;
                    if (zDistance > distance)
                    {
                        zDistance = distance;
                    }
                    spawnedBlocks[spawnedBlocks.Count - 1].transform.position = new Vector3(spawnedBlocks[spawnedBlocks.Count - 1].transform.position.x, spawnedBlocks[spawnedBlocks.Count - 1].transform.position.y, posStart + zDistance / 2);
                    spawnedBlocks[spawnedBlocks.Count - 1].transform.localScale = new Vector3(1.2f, 0.1f, zDistance);
                }
            }

            yPos = transform.position.y + transform.localScale.z / 2; //Player position y

            if (playerrb.transform.position.y <= yPos)
            {
                playerrb.transform.position = new Vector3(playerrb.transform.position.x, yPos, playerrb.transform.position.z);
            }
            if (Finish == true) //Increased speed at the finish
            {
                speed = 16;
            }
            if (Ground == false) //If there is more than one cylinder and the value Ground! = The value of the last cylinder,
                                 //we get this value and if Ground = true, turn on the running animation.
            {
                if (cylindersNum > 1)
                {
                    if (Ground != cylinders[cylinders.Count - 1].Ground && animator.GetCurrentAnimatorStateInfo(0).IsName("run"))
                    {
                        Ground = cylinders[cylinders.Count - 1].Ground;
                        if (Ground == true)
                        {
                            Ground = true;
                            animator.Play("run");
                            playerrb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
                        }
                    }
                }
            }
            else
            {
                if (cylindersNum > 1) //If Ground = false, check if after timeMax seconds Ground = true does not become, then the player has lost
                {
                    if (cylinders[cylinders.Count - 1].Dead == true)
                    {
                        timerOn = true;
                        time = 0;
                    }
                }
            }
            if (timerOn == true) //After the last cylinder Ground has become false, we wait for the time and delete it, 
                //so that if the player jumps and some of the cylinders fall off the house, he will not lose
            {
                time++;
                if (time > timeMax)
                {
                    if (cylinders[cylinders.Count - 2].Dead == true)
                    {

                        if (cylinders[cylinders.Count - 3].Dead == true)
                        {
                            if (cylinders[cylinders.Count - 4].Dead == true)
                            {
                                if (cylinders[cylinders.Count - 5].Dead == true)
                                {
                                    if (cylinders[cylinders.Count - 6].Dead == true)
                                    {
                                        if (cylinders[cylinders.Count - 7].Dead == true)
                                        {
                                            if (cylinders[cylinders.Count - 8].Dead == true)
                                            {
                                                IsDeadForFall();
                                                timerOn = false;
                                                time = 0;
                                            }
                                            else
                                            {
                                                timerOn = false;
                                                time = 0;
                                                ClearLastCylinder();
                                                ClearLastCylinder();
                                                ClearLastCylinder();
                                                ClearLastCylinder();
                                                ClearLastCylinder();
                                                ClearLastCylinder();
                                            }
                                        }
                                        else
                                        {
                                            timerOn = false;
                                            time = 0;
                                            ClearLastCylinder();
                                            ClearLastCylinder();
                                            ClearLastCylinder();
                                            ClearLastCylinder();
                                            ClearLastCylinder();
                                            ClearLastCylinder();
                                        }
                                    }
                                    else
                                    {
                                        timerOn = false;
                                        time = 0;
                                        ClearLastCylinder();
                                        ClearLastCylinder();
                                        ClearLastCylinder();
                                        ClearLastCylinder();
                                        ClearLastCylinder();
                                    }
                                }
                                else
                                {
                                    timerOn = false;
                                    time = 0;
                                    ClearLastCylinder();
                                    ClearLastCylinder();
                                    ClearLastCylinder();
                                    ClearLastCylinder();
                                }
                            }
                            else
                            {
                                timerOn = false;
                                time = 0;
                                ClearLastCylinder();
                                ClearLastCylinder();
                                ClearLastCylinder();
                            }
                        }
                        else
                        {
                            timerOn = false;
                            time = 0;
                            ClearLastCylinder();
                            ClearLastCylinder();
                        }
                    }
                    else
                    {
                        timerOn = false;
                        time = 0;
                        ClearLastCylinder();
                    }
                }
                if (score != (int)PlayerPrefs.GetFloat("Score"))
                {
                    score = (int)PlayerPrefs.GetFloat("Score");
                }
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("dead"))
                {
                    PlayerPrefs.SetInt("isDeaded", 1);
                    PlayerPrefs.SetInt("isStarted", 0);
                }
            }

            switch (cylindersNum) //Depending on the number of cylinders, we set coordinates for them so that they are always together
            {
                case 2:
                    cylinders[1].transform.position = new Vector3(transform.position.x, cylinders[1].transform.position.y, transform.position.z);//Main
                    cylinders[2].transform.position = new Vector3(transform.position.x, cylinders[2].transform.position.y, transform.position.z);//Second
                    break;
                case 3:
                    cylinders[1].transform.position = new Vector3(transform.position.x, cylinders[1].transform.position.y, transform.position.z);//Главный
                    cylinders[2].transform.position = new Vector3(transform.position.x, cylinders[2].transform.position.y, transform.position.z);//Второй
                    cylinders[3].transform.position = new Vector3(transform.position.x, cylinders[3].transform.position.y, transform.position.z);//Третий
                    break;
                case 4:
                    cylinders[1].transform.position = new Vector3(transform.position.x, cylinders[1].transform.position.y, transform.position.z);//Главный
                    cylinders[2].transform.position = new Vector3(transform.position.x, cylinders[2].transform.position.y, transform.position.z);//Второй
                    cylinders[3].transform.position = new Vector3(transform.position.x, cylinders[3].transform.position.y, transform.position.z);//Третий
                    cylinders[4].transform.position = new Vector3(transform.position.x, cylinders[4].transform.position.y, transform.position.z);//Четвертый
                    break;
                case 5:
                    cylinders[1].transform.position = new Vector3(transform.position.x, cylinders[1].transform.position.y, transform.position.z);//Главный
                    cylinders[2].transform.position = new Vector3(transform.position.x, cylinders[2].transform.position.y, transform.position.z);//Второй
                    cylinders[3].transform.position = new Vector3(transform.position.x, cylinders[3].transform.position.y, transform.position.z);//Третий
                    cylinders[4].transform.position = new Vector3(transform.position.x, cylinders[4].transform.position.y, transform.position.z);//Четвертый
                    cylinders[5].transform.position = new Vector3(transform.position.x, cylinders[5].transform.position.y, transform.position.z);
                    break;
                case 6:
                    cylinders[1].transform.position = new Vector3(transform.position.x, cylinders[1].transform.position.y, transform.position.z);//Главный
                    cylinders[2].transform.position = new Vector3(transform.position.x, cylinders[2].transform.position.y, transform.position.z);//Второй
                    cylinders[3].transform.position = new Vector3(transform.position.x, cylinders[3].transform.position.y, transform.position.z);//Третий
                    cylinders[4].transform.position = new Vector3(transform.position.x, cylinders[4].transform.position.y, transform.position.z);//Четвертый
                    cylinders[5].transform.position = new Vector3(transform.position.x, cylinders[5].transform.position.y, transform.position.z);
                    cylinders[6].transform.position = new Vector3(transform.position.x, cylinders[6].transform.position.y, transform.position.z);
                    break;
                case 7:
                    cylinders[1].transform.position = new Vector3(transform.position.x, cylinders[1].transform.position.y, transform.position.z);//Главный
                    cylinders[2].transform.position = new Vector3(transform.position.x, cylinders[2].transform.position.y, transform.position.z);//Второй
                    cylinders[3].transform.position = new Vector3(transform.position.x, cylinders[3].transform.position.y, transform.position.z);//Третий
                    cylinders[4].transform.position = new Vector3(transform.position.x, cylinders[4].transform.position.y, transform.position.z);//Четвертый
                    cylinders[5].transform.position = new Vector3(transform.position.x, cylinders[5].transform.position.y, transform.position.z);
                    cylinders[6].transform.position = new Vector3(transform.position.x, cylinders[6].transform.position.y, transform.position.z);
                    cylinders[7].transform.position = new Vector3(transform.position.x, cylinders[7].transform.position.y, transform.position.z);
                    break;
                case 8:
                    cylinders[1].transform.position = new Vector3(transform.position.x, cylinders[1].transform.position.y, transform.position.z);
                    cylinders[2].transform.position = new Vector3(transform.position.x, cylinders[2].transform.position.y, transform.position.z);
                    cylinders[3].transform.position = new Vector3(transform.position.x, cylinders[3].transform.position.y, transform.position.z);
                    cylinders[4].transform.position = new Vector3(transform.position.x, cylinders[4].transform.position.y, transform.position.z);
                    cylinders[5].transform.position = new Vector3(transform.position.x, cylinders[5].transform.position.y, transform.position.z);
                    cylinders[6].transform.position = new Vector3(transform.position.x, cylinders[6].transform.position.y, transform.position.z);
                    cylinders[7].transform.position = new Vector3(transform.position.x, cylinders[7].transform.position.y, transform.position.z);
                    cylinders[8].transform.position = new Vector3(transform.position.x, cylinders[8].transform.position.y, transform.position.z);
                    break;
            }

        }
    }

    public void ChangeScale() 
    {
        if (Scale > maxScale + 0.05f) //If the size is larger than the maximum, set the maximum size
        {
            cylinders[cylinders.Count - 1].transform.localScale = new Vector3(maxScale, 0.6f, maxScale);
            if (cylindersNum < 8) //If there are less than eight cylinders, spawn a new one, if eight, simply set the maximum size
            {
                SpawnNewCylinder();
            }
            else
            {
                Scale = maxScale;
                PlayerPrefs.SetFloat("Scale", Scale);
            }
        }
        else
        {
            cylinders[cylinders.Count - 1].transform.localScale = new Vector3(Scale, 0.6f, Scale);
        }
    }
    public void SpawnBl()
    {
        if (Scale >= 0.01f || cylindersNum > 1) //If the size is more than 0.01 or more than one cylinder, you can spawn
        {
            SpawnBlock newSpawnBlock = Instantiate(SpawnBlock); //Spawn the road block
            offset = new Vector3(0, -cylinders[cylinders.Count - 1].transform.localScale.z / 2 - 0.05f, 0); //We calculate its mixing relative to the last cylinder

            newSpawnBlock.transform.localPosition = new Vector3(transform.position.x, cylinders[cylinders.Count - 1].transform.position.y, posStart) + offset; //Set coordinates
            spawnedBlocks.Add(newSpawnBlock);

            zScale = spawnedBlocks[spawnedBlocks.Count - 1].transform.localScale.z; //zScale - road length

            Spawn = false;
            Stretch = true;

            xPosOfSpawn = transform.position.x;
            xPosM = xPosOfSpawn - xPosChange;
            xPosA = xPosOfSpawn + xPosChange; //We calculate the extreme points of the road to the right and left

            posWill = posStart + distance; //Finish point

            zDistance = 0; //All distance

            hasDistance = 0; //Distance now
        }
    }
    public void SpawnNewCylinder() //Save the cylinder to the past and add it to the cylinder sheet
    {
        Cylinder newCylinder = Instantiate(prefab);
        Vector3 position = new Vector3(transform.position.x, cylinders[cylinders.Count - 1].transform.position.y - cylinders[cylinders.Count - 1].transform.localScale.z, transform.position.z);
        newCylinder.transform.localPosition = position;

        newCylinder.transform.localScale = new Vector3(0.01f, 0.6f, 0.01f);

        Scale = 0.01f;
        PlayerPrefs.SetFloat("Scale", Scale);

        newCylinder.transform.SetParent(parent);

        cylinders.Add(newCylinder);
        cylindersNum = cylindersNum + 1;

        PlayerPrefs.SetInt("CylinderNum", cylindersNum);
    }
    public void ClearLastCylinder() //Remove the last cylinder from the end
    {
            Destroy(this.cylinders[cylinders.Count - 1].gameObject);
            cylinders.RemoveAt(cylindersNum);
            cylindersNum = cylindersNum - 1; //Removing and decreasing the variable

            Scale = maxScale; //The size is equal to the maximum, because the penultimate became the last, and its size cannot be equal to not the maximum
            PlayerPrefs.SetFloat("Scale", Scale);

            Rigidbody rb = cylinders[cylindersNum].GetComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.FreezeRotation;

            PlayerPrefs.SetInt("CylinderNum", cylindersNum);
    }
    private void OnTriggerEnter(Collider target)
    {
        if (target.tag == "Dead")
        {
            IsDeadForFall();
        }
        if (target.tag == "GroundOff")
        {
            Ground = false;

            animator.Play("jump");
            playerrb.AddForce(0, 3.5f, 0, ForceMode.Impulse); //The player jumps and does somersaults
        }

        if (target.tag == "SecondChance")
        {
            platformNum = platformNum + 1;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "GroundOn") //When the player is back on the ground
        {
            if (isStarted == 1)
            {
                Ground = true;
                animator.Play("run");
                playerrb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
            }
        }

    }

    public void IsDeadForScale() //Death on collision with a conventional obstacle
    {
        PlayerPrefs.SetInt("isDeaded", 1);
        PlayerPrefs.SetInt("isStarted", 0);

        playerpos.enabled = false;

        playerrb.constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

        Scale = 0;
        PlayerPrefs.SetFloat("Scale", Scale);

        animator.Play("dead");

        playerrb.AddForce(0, 3f, PlayerPrefs.GetFloat("FallDistance"), ForceMode.Impulse);

        cylinders[cylindersNum].transform.localScale = new Vector3(0, 0.6f, 0);

        Debug.Log("Dead for scale");  
    }
    public void IsDeadForScaleTwo() //Death when the cylinders run out when spawning
    {
        if (Finish == false)
        {
            PlayerPrefs.SetInt("isDeaded", 1);
        }
        else
        {
            cam.Finished();
        }
        PlayerPrefs.SetInt("isStarted", 0);

        playerpos.enabled = false;
        animator.Play("jump-up");

        Scale = 0;
        PlayerPrefs.SetFloat("Scale", Scale);

        cylinders[cylindersNum].transform.localScale = new Vector3(0, 0.6f, 0);

        playerrb.constraints = RigidbodyConstraints.FreezeRotation;

        playerrb.AddForce(0, 8f, 3f, ForceMode.Impulse);

        Rigidbody rb = cylinders[cylindersNum].GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeAll;

        Debug.Log("Dead for scale of spawn");
        
    }
    public void IsDeadForFall() //Death by falling into the abyss
    {
        PlayerPrefs.SetInt("isDeaded", 1);
        PlayerPrefs.SetInt("isStarted", 0);

        animator.Play("jump-up");

        playerpos.enabled = false;
        cam.enabled = false;

        Debug.Log("Dead for fall");

    }

}

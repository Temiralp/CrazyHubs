using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Test Trigger
public class AllOnTrigger : MonoBehaviour
{
    [Header("Insert these objects into the script in the prefab")]
    public GameObject Adds;
    public MeshRenderer mesh;

    private void OnTriggerEnter(Collider target)
    {

        if (target.tag == "AllOn") //When the player touches this object, turn on all Adders, this optimizes the game, since Adders are not turned on all at once, but in turn, while one turns on, the other is already removed from the scene.
        {
            mesh.enabled = false;
            Adds.SetActive(true);
        }
    }
}

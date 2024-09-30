using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionHandler : MonoBehaviour
{
    public GameObject victoryEffectPrefab;
    public GameObject nightmareController;

    CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // all collisions are checked with a tag
        // nightmareController is not null during nightmare mode
        if (hit.gameObject.CompareTag("Start") && nightmareController == null) Destroy(hit.gameObject);


        else if (hit.gameObject.CompareTag("Start"))
        {
            Destroy(hit.gameObject);
            nightmareController.GetComponent<NightmareModeController>().StartMapSwitches(); // starts the maze switches during nightmare mode
        }

        else if (hit.gameObject.CompareTag("Finish") && nightmareController == null)
        {
            Vector3 effectPosition = new Vector3(transform.position.x, -.5f, transform.position.z);
            Instantiate(victoryEffectPrefab, effectPosition, victoryEffectPrefab.transform.rotation);
            Destroy(hit.gameObject);
            Debug.Log("Finished");
        }

        else if (hit.gameObject.CompareTag("Finish"))
        {
            nightmareController.GetComponent<NightmareModeController>().playActive = false; // stops maps from switching when the player finishes
            Vector3 effectPosition = new Vector3(transform.position.x, -.5f, transform.position.z);
            Instantiate(victoryEffectPrefab, effectPosition, victoryEffectPrefab.transform.rotation);
            Destroy(hit.gameObject);
            Debug.Log("Finished");
        }
    }
}

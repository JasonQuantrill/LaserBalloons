using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;


public class InstantiatePrefabWithController : MonoBehaviour
{
    public SteamVR_Input_Sources handType; // Hand type (left or right)
    public SteamVR_Action_Boolean spawnAction; // The action to trigger prefab instantiation
    public GameObject prefabToSpawn; // Assign in Inspector
    public Vector3 spawnOffset;
    public Vector3 fullScale = Vector3.one; // Full size of the balloon
    public float scaleTime = 2.0f; // Time to scale up

    private void Awake()
    {
        spawnAction.AddOnStateDownListener(TriggerDown, handType);
    }


    private GameObject currentBalloon;

    private void TriggerDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        currentBalloon = Instantiate(prefabToSpawn, transform.position + transform.TransformDirection(spawnOffset), Quaternion.identity);
        currentBalloon.transform.localScale = Vector3.zero; // Start small
        currentBalloon.transform.SetParent(transform); // Attach to controller
        StartCoroutine(ScaleAndDetach(currentBalloon));
    }

    private IEnumerator ScaleAndDetach(GameObject balloon)
    {
        float currentTime = 0;

        while (currentTime <= scaleTime)
        {
            balloon.transform.localScale = Vector3.Lerp(Vector3.zero, fullScale, currentTime / scaleTime);
            currentTime += Time.deltaTime;
            yield return null;
        }

        balloon.transform.SetParent(null); // Detach from controller

        // Attach the balloon behaviour script so that they disappear at a given height
        var balloonBehaviour = balloon.AddComponent<BalloonBehaviour>();

        // Ensure the balloon has a Rigidbody component
        Rigidbody rb = balloon.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false; // Allow physics to take over
        }
    }


    private void OnDestroy()
    {
        if (spawnAction != null)
        {
            spawnAction.RemoveOnStateDownListener(TriggerDown, handType);
        }
    }
}

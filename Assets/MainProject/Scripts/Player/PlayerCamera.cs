using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerCamera : MonoBehaviour
{
    // Camera Attributes
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float maxAngleLimit = 90;
    [SerializeField] private float minAngleLimit = -45;

    // Notify weapon and other stuff when player open fire
    // Be called in LaunchRayCast()
    public delegate void RayCastLaunchEvent(Vector3 hitPosition);
    public static event RayCastLaunchEvent OnRayCastFire;
    // Raycast Attributes
    [SerializeField] private float rayCastDistance = 1000;

    private void Awake()
    {
        // Bind Level Start and End event
        LevelState_Level1.OnGameStart += GameStart;
        LevelState_Level1.OnGameEnd += GameEnd;
    }

    private void GameStart()
    {
        // Enable player to control camera when game start
        PlayerController.OnLookChange += RotateCameraOnX;
        // Bind fire input
        PlayerController.OnFireStart += LaunchRayCast;
    }

    private void GameEnd() 
    {
        // Disable player to control camera when game start
        PlayerController.OnLookChange -= RotateCameraOnX;

        // Unbind fire input
        PlayerController.OnFireStart -= LaunchRayCast;

        LevelState_Level1.OnGameStart -= GameStart;
        LevelState_Level1.OnGameEnd -= GameEnd;
    }

    // This script is only responsible to the x rotation of camera
    // Camera is child of Player
    // Player controller's script is responsible to Y rotation of player, and rotate camera simutaneously
    private void RotateCameraOnX(Vector2 input)
    {
        float currentRotationOnX = transform.eulerAngles.x;
        float newRotationOnX = Mathf.Clamp(minAngleLimit, currentRotationOnX - input.y * rotationSpeed, maxAngleLimit);
        transform.eulerAngles = new Vector3(newRotationOnX, transform.eulerAngles.y, transform.eulerAngles.z);
    }

    private void LaunchRayCast()
    {
        Vector3 selfPosition = transform.position;
        Vector3 selfForward = transform.forward;

        if (Physics.Raycast(selfPosition, selfForward, out RaycastHit hit, rayCastDistance))
        {
            // Check if hitted object possesses the script
            // Call be hurted event if object has the script
            DesrtuctibleObjectScript TempDestructibleComponent = hit.collider.transform.GetComponent<DesrtuctibleObjectScript>();

            if (TempDestructibleComponent != null)
            {
                TempDestructibleComponent.BeHurted();
            }
            
            // Notify other class which bind the delegate
            OnRayCastFire?.Invoke(hit.point);
        }
        else
        {
            OnRayCastFire?.Invoke(selfPosition + selfForward * rayCastDistance);
        }
    }
}

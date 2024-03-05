using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerCharacter : MonoBehaviour
{
    // GameObject Component
    private CharacterController _characterController;

    // Jump Attributes
    [SerializeField] private float JumpAccelerate;
    [SerializeField] private int continuousJumpLimit = 1;
    [SerializeField] private float fallDecelerate = -9.81f;
    [SerializeField] private float maxFallSpeed;
    private int _currentJumpTimes = 0;

    // Move Attributes
    [SerializeField] private float moveSpeedOnGround;
    [SerializeField] private float moveSpeedInFalling;
    
    // Rotate Attributes
    [SerializeField] private float RotationSpeed;

    // Player's realtime velocity
    private Vector3 _presentVelocity;

    /*[SerializeField] private GameObject rigidbodyDetectPoint;
    [SerializeField] private float detectRadius;
    [SerializeField] private float detectDistance;*/

    private void Awake()
    {
        // Find component
        _characterController = GetComponent<CharacterController>();

        // Bind start and end event
        LevelState_Level1.OnGameStart += GameStartEvent;
        LevelState_Level1.OnGameEnd += GameEndEvent;

        _characterController.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Move and add gravity to character
        _characterController.Move(_presentVelocity);
        UpdateGravityInfluenceOnY();
    }

    private void GameStartEvent()
    {
        _characterController.enabled = true;
        // Enable Input
        PlayerController.OnMoveEnter += UpdateVelocityOnXZ;
        PlayerController.OnJumpStart += Jump;
        PlayerController.OnLookChange += RotateCharacter;
    }

    private void GameEndEvent()
    {
        _characterController.enabled = false;
        // Disable Input
        PlayerController.OnMoveEnter -= UpdateVelocityOnXZ;
        PlayerController.OnJumpStart -= Jump;
        PlayerController.OnLookChange -= RotateCharacter;

        LevelState_Level1.OnGameStart -= GameStartEvent;
        LevelState_Level1.OnGameEnd -= GameEndEvent;
    }

    // This function would be called by player controller through action OnMoveEnter
    private void UpdateVelocityOnXZ(Vector2 input)
    {
        Vector3 forward;
        Vector3 right;

        forward = _characterController.isGrounded ? input.x * moveSpeedOnGround * Time.deltaTime * transform.right : input.x * moveSpeedInFalling * Time.deltaTime * transform.right;
        right = _characterController.isGrounded ? input.y * moveSpeedOnGround * Time.deltaTime * transform.forward : input.y * moveSpeedInFalling * Time.deltaTime * transform.forward;

        _presentVelocity.x = forward.x + right.x;
        _presentVelocity.z = forward.z + right.z;
    }

    // This function would be called by Updated to update influence of fallSpeed
    private void UpdateGravityInfluenceOnY()
    {
        if(_characterController.isGrounded )
        {
            _presentVelocity.y = 0;
            _currentJumpTimes = 0;
        }

        _presentVelocity.y += fallDecelerate * Time.deltaTime;

        // Judge if fall speed would be less the the max fall speed
        _presentVelocity.y = Mathf.Max(maxFallSpeed, _presentVelocity.y);

        // It seems to use too much time and it is never executed in update
        /*if (Physics.SphereCast(rigidbodyDetectPoint.transform.position, detectRadius, -transform.up, out RaycastHit hit, detectDistance))
        {
            if (hit.transform.GetComponent<Rigidbody>() != null)
            {
                _presentVelocity.y = 0;
            }
        }
        else
        {
            _presentVelocity.y += fallDecelerate * Time.deltaTime;

        // Judge if fall speed would be less the the max fall speed
        _presentVelocity.y = Mathf.Max(maxFallSpeed, _presentVelocity.y);
        }*/
    }

    private void Jump()
    {
        if (_currentJumpTimes < continuousJumpLimit)
        {
            _presentVelocity.y = JumpAccelerate;
            _currentJumpTimes++;
        }
    }

    // This script is only responsible to the y rotation of player
    // And it will rotate camera at the same time since camera is child
    // X roation of camera and weapon is changed in player camera script
    private void RotateCharacter(Vector2 input)
    {
        float newRotation = input.x * RotationSpeed;
        transform.eulerAngles += new Vector3(0, newRotation, 0);
    }

    // The func will be called when player touch lava's collider
    public void Respawn()
    {
        _characterController.enabled= false;
        transform.position = RespawnPoint.respawnLocation.transform.position;
        transform.eulerAngles = RespawnPoint.respawnLocation.transform.eulerAngles;
        _characterController.enabled = true;
    }
}

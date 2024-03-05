using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private InputActions _input;
    // Event for move, jump and look rotation
    public delegate void MoveEnterEvent(Vector2 input);
    public static event MoveEnterEvent OnMoveEnter;

    public delegate void LookChangeEvent(Vector2 input);
    public static event LookChangeEvent OnLookChange;

    public static Action OnJumpStart;
    public static Action OnFireStart;

    private void Awake()
    {
        _input = new InputActions();
        LevelState_Level1.OnGameStart += GameStart;
        LevelState_Level1.OnGameEnd += GameEnd;

        _input.Gameplay.Jump.started += UpdatePlayerJump;
        _input.Gameplay.Fire.started += NotifyFireStartEvent;
    }

    void Update()
    {
        UpdatePlayerMove();
        UpdatePlayerLookRotation();
    }

    void GameStart()
    {
        EnableInput();
    }

    void GameEnd()
    {
        DisableInput();
        _input.Gameplay.Jump.started -= UpdatePlayerJump;
        _input.Gameplay.Fire.started -= NotifyFireStartEvent;
        LevelState_Level1.OnGameStart -= GameStart;
        LevelState_Level1.OnGameEnd -= GameEnd;
    }

    void EnableInput()
    {
        _input.Enable();
    }

    private void DisableInput()
    {
        _input.Disable();
    }

    private void UpdatePlayerMove()
    {
        Vector2 input = _input.Gameplay.Move.ReadValue<Vector2>();
        OnMoveEnter?.Invoke(input);
    }

    private void UpdatePlayerJump(InputAction.CallbackContext obj)
    {
        OnJumpStart?.Invoke();
    }

    private void UpdatePlayerLookRotation()
    {
        Vector2 input = _input.Gameplay.Look.ReadValue<Vector2>();
        OnLookChange?.Invoke(input);
    }

    private void NotifyFireStartEvent(InputAction.CallbackContext obj)
    {
        OnFireStart?.Invoke();
    }
}

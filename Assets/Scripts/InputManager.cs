using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{

    private PlayerControls playerControls;

    public InputAction horizontalMovement;

    public InputAction roll;

    public InputAction jump;


    private void Awake() {

        playerControls = new PlayerControls();

        horizontalMovement = playerControls.InGame.HorizontalMovement;
        horizontalMovement.Enable();

        roll = playerControls.InGame.Roll;
        roll.Enable();

        jump = playerControls.InGame.Jump;
        jump.Enable();

    }

    private void OnDisable() {

        horizontalMovement.Disable();
        roll.Disable();
        jump.Disable();

    }

}

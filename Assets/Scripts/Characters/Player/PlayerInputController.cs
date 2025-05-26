using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;


class PlayerInputController : MonoBehaviour, ICharacterInput {
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction grabAction;
    private InputAction dashAction;

    private const float jumpBufferTime = 0.2f;
    private float jumpBufferCounter;
    public float HorizontalMovement { get; set; }
    public float VerticalMovement { get; set; }
    public bool Jump { get; set; }
    public bool Grab { get; set; }
    public bool CancelJump { get; set; }
    public bool Dash { get; set; }

    void Start() {
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        grabAction = InputSystem.actions.FindAction("Grab");
        dashAction = InputSystem.actions.FindAction("Dash");
        jumpAction.started += OnJumpPressed;
        jumpAction.canceled += OnJumpReleased;
    }

    void Update() {
        Vector2 movementInput = moveAction.ReadValue<Vector2>();
        HorizontalMovement = movementInput.x;
        VerticalMovement = movementInput.y;
        Grab = grabAction.ReadValue<float>() > 0;
        Dash = dashAction.ReadValue<float>() > 0;

        jumpBufferCounter -= Time.deltaTime;

        if(jumpBufferCounter <= 0) {
            Jump = false;
        }
    }

    private void OnJumpPressed(InputAction.CallbackContext context) {
        Jump = true;
        jumpBufferCounter = jumpBufferTime;
    }
    private void OnJumpReleased(InputAction.CallbackContext context) {
        StartCoroutine(EndJumpEarly());
    }

    private IEnumerator EndJumpEarly() {
        CancelJump = true;
        yield return new WaitForEndOfFrame();
        CancelJump = false;
    }

}

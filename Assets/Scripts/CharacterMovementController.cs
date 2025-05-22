using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class CharacterMovementController : MonoBehaviour {
    [Header("Core Components")]
    public Rigidbody2D Body;
    public SpriteRenderer Sprite;
    public Animator Animator;
    public Collider2D HitBox;

    [Header("Sensors")]
    public TerrainSensor GroundCheck;
    public TerrainSensor LeftWallCheck;
    public TerrainSensor RightWallCheck;

    [Header("Movement")]
    public CharacterMovementParams Movement;
    public CharacterInput Input;

    protected StateMachine stateMachine;

    // BLACKBOARD INFO

    public bool IsGrounded => GroundCheck.IsTouching;
    public bool FallingThroughPlatform;
    public float TimeSinceGrounded => GroundCheck.TimeSinceTouched;
    public float HorizontalDrag;
    public int IsTouchingWall => LeftWallCheck.IsTouching ? -1 : RightWallCheck.IsTouching ? 1 : 0;

    #region Movement Methods
    public void HandleMoveInput() {
        ApplyAccelerationX(Input.HorizontalMovement * Movement.AccelerationX);
    }

    public void SetVelocityX(float velocity) {
        Body.linearVelocityX = velocity;
    }

    public void SetVelocityY(float velocity) {
        Body.linearVelocityY = velocity;
    }
    public void ApplyAccelerationX(float acceleration) {
        Body.linearVelocityX += acceleration;
    }

    public void ApplyAccelerationY(float acceleration) {
        Body.linearVelocityY += acceleration;
    }

    public void LookTowards(float directionX) {
        if (directionX == 0) return;

        Sprite.transform.rotation = directionX > 0 ? Quaternion.identity : Quaternion.Euler(0, 180, 0);
    }

    public void FaceMovementDirection() {
        LookTowards(Input.HorizontalMovement);
    }

    public void ApplyHorizontalGroundDrag() {
        if(Input.HorizontalMovement == 0) {
            Body.linearVelocityX *= (1 - Movement.GroundHorizontalDrag);
        }
    }

    public void ApplyHorizontalAirDrag() {
        if (Input.HorizontalMovement == 0) {
            Body.linearVelocityX *= (1 - Movement.AirHorizontalDrag);
        }
    }

    public void LimitWalkingSpeed() {
        Body.linearVelocityX = Mathf.Clamp(Body.linearVelocityX, -Movement.TopSpeedX, Movement.TopSpeedX);
    }
    #endregion

    #region Airborne Methods
    public void ApplyJumpingGravity() {
        Body.gravityScale = Movement.RisingGravity;
    }

    public void ApplyFallingGravity() {
        Body.gravityScale = Movement.FallingGravity;
    }

    public void DisableGravity() {
        Body.gravityScale = 0;
    }

    public void ApplyAdaptiveGravity() {
        if(Body.linearVelocityY > 0) {
            ApplyJumpingGravity();
        }
        else {
            ApplyFallingGravity();
        }
    }
    #endregion

    #region Jump Methods
    public void HandleJumpInput() {
        if (!Input.Jump || FallingThroughPlatform) return;
        if(Input.VerticalMovement < 0 && GroundCheck.IsTouchingLayer("Platforms") && Body.linearVelocityY <= 0.1f) {
            JumpThroughPlatform();
        }
        else if (CanJump()) {
            Jump();
        }
    }


    public bool CanJump() {
        return (IsGrounded || CanCoyoteJump()) && Body.linearVelocityY <= 0.1f && !FallingThroughPlatform;
    }

    public bool CanCoyoteJump() {
        return TimeSinceGrounded <= Movement.CoyoteTime;
    }

    public void Jump() {
        SetVelocityY(Movement.JumpSpeed);
    }
    private void JumpThroughPlatform() {
        StartCoroutine(JumpThroughPlatformCoroutine());
    }

    private IEnumerator JumpThroughPlatformCoroutine() {
        SetVelocityY(4f);
        HitBox.enabled = false;
        FallingThroughPlatform = true;

        yield return new WaitForSeconds(.25f);

        HitBox.enabled = true;
        FallingThroughPlatform = false;
    }
    #endregion
}
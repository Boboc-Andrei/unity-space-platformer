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
    public TerrainSensor LeftLedgeDetector;
    public TerrainSensor RightLedgeDetector;

    [Header("Movement")]
    public CharacterMovementParams Movement;
    public CharacterInput Input;
    public float CurrentWallSlideStamina;
    public float MaxWallSlideStamina = 1f;

    protected StateMachine stateMachine;

    // BLACKBOARD INFO

    public bool IsGrounded => GroundCheck.IsTouching;
    public bool IsJumping;
    public bool FallingThroughPlatform;
    public float MoveSpeedFactor = 1;
    public float TimeSinceGrounded => GroundCheck.TimeSinceTouched;
    public float HorizontalDrag;
    public bool DisableTurning;
    public bool CanGrabWall = true;
    public bool DisableMovementInput = false;
    private bool DisableHorizontalDrag = false;

    public int IsTouchingWall => LeftWallCheck.IsTouching ? -1 : RightWallCheck.IsTouching ? 1 : 0;
    public int IsTouchingGrabbableLedge {
        get {
            if(GrabbableLedge == null) return 0;
            var ledge = GrabbableLedge.GetComponent<LedgeGrabPoint>();
            if (Mathf.Sign(GrabbableLedge.position.x - transform.position.x) == ledge.GrabDirection) return ledge.GrabDirection;
            else return 0;
        }
    }
    public Transform GrabbableLedge {
        get {
            if (LeftLedgeDetector.LastTouched != null) return LeftLedgeDetector.LastTouched.transform;
            if (RightLedgeDetector.LastTouched != null) return RightLedgeDetector.LastTouched.transform;
            return null;
        }
    }

    #region Movement Methods
    public void HandleMoveInput() {
        if (DisableMovementInput) return;
        ApplyAccelerationX(Input.HorizontalMovement * Movement.AccelerationX * MoveSpeedFactor);
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
        if (DisableTurning) return;
        if (Input.HorizontalMovement != 0) {
            LookTowards(Input.HorizontalMovement);
        }
        else if (Body.linearVelocityX > .1f) {
            LookTowards(Body.linearVelocityX);
        }
    }

    public void ApplyHorizontalGroundDrag() {
        if (Input.HorizontalMovement == 0) {
            Body.linearVelocityX *= (1 - Movement.GroundHorizontalDrag);
        }
    }

    public void ApplyHorizontalAirDrag() {
        if (DisableHorizontalDrag) return;
        if (Input.HorizontalMovement == 0) {
            Body.linearVelocityX *= (1 - Movement.AirHorizontalDrag);
        }
    }

    public void LimitWalkingSpeed() {
        Body.linearVelocityX = Mathf.Clamp(Body.linearVelocityX, -Movement.TopSpeedX, Movement.TopSpeedX);
    }

    public void HandleStaminaRegen() {
        if (IsGrounded) CurrentWallSlideStamina = MaxWallSlideStamina;
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
        if (Body.linearVelocityY > 0) {
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
        if (Input.VerticalMovement < 0 && GroundCheck.IsTouchingLayer("Platforms") && Body.linearVelocityY <= 0.1f) {
            JumpThroughPlatform();
        }
        else if (CanJump()) {
            Jump();
        }
        else if (CanWallJump()) {
            WallJump();
        }
    }

    public void WallJump() {
        IsJumping = true;
        var wallJumpDirection = -IsTouchingWall;
        SetVelocityY(Movement.JumpSpeed);
        SetVelocityX(Movement.TopSpeedX * wallJumpDirection);
        LerpMovementAcceleration(.3f);
    }

    public void DisableMovementInputForSeconds(float time) {
        StartCoroutine(DisableMovementInputForSecondsCoroutine(time));
    }

    public IEnumerator DisableMovementInputForSecondsCoroutine(float time) {
        DisableMovementInput = true;
        DisableHorizontalDrag = true;
        yield return new WaitForSeconds(time);
        DisableMovementInput = false;
        DisableHorizontalDrag = false;
    }

    public void LerpMovementAcceleration(float time) {
        StartCoroutine(LerpMovementAccelerationCoroutine(time));
    }

    private IEnumerator LerpMovementAccelerationCoroutine(float time) {
        float elapsed = 0;
        DisableHorizontalDrag = true;
        while (elapsed < time) {
            MoveSpeedFactor = Mathf.Lerp(0f, .75f, elapsed / time);
            elapsed += Time.deltaTime;
            yield return null;
        }
        DisableHorizontalDrag = false;
        MoveSpeedFactor = 1;
    }

    public bool CanWallJump() {
        return !IsGrounded && IsTouchingWall != 0 && Body.linearVelocityY < .1f;
    }

    public bool CanJump() {
        return (IsGrounded || CanCoyoteJump()) && Body.linearVelocityY <= 0.1f && !FallingThroughPlatform;
    }

    public bool CanCoyoteJump() {
        return TimeSinceGrounded <= Movement.CoyoteTime;
    }

    public void Jump() {
        IsJumping = true;
        SetVelocityY(Movement.JumpSpeed);
    }
    protected void JumpThroughPlatform() {
        StartCoroutine(JumpThroughPlatformCoroutine());
    }

    protected IEnumerator JumpThroughPlatformCoroutine() {
        SetVelocityY(4f);
        HitBox.enabled = false;
        FallingThroughPlatform = true;

        yield return new WaitForSeconds(.25f);

        HitBox.enabled = true;
        FallingThroughPlatform = false;
    }
    #endregion

    #region Wall Slide Methods

    public void StartWallGrabCooldown() {
        StartCoroutine(DisableWallGrabbingForSeconds(.2f));
    }

    private IEnumerator DisableWallGrabbingForSeconds(float time) {
        CanGrabWall = false;
        yield return new WaitForSeconds(time);
        CanGrabWall = true;
    }

    #endregion
}
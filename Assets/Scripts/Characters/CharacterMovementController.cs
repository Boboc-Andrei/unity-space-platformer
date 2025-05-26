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
    public ICharacterInput Input;
    public float CurrentWallSlideStamina { get; set; }
    public float MaxWallSlideStamina { get; set; } = 1f;

    protected StateMachine stateMachine;

    // ABILITIES
    public JumpComponent Jump;
    public WallJumpComponent WallJump;
    public DashComponent Dash;

    // BLACKBOARD INFO
    public bool IsJumping;
    public bool IsGrounded => GroundCheck.IsTouching;
    public bool FallingThroughPlatform;
    public float MoveSpeedFactor = 1;
    public float TimeSinceGrounded => GroundCheck.TimeSinceTouched;
    public float HorizontalDrag;
    public bool DisableTurning;
    public bool DisableMovementInput = false;
    public bool DisableHorizontalDrag = false;
    public int FacingDirection => Sprite.transform.rotation.y == 0 ? 1 : -1;
    public bool CanGrabWall { get; set; } = true;

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
        var topSpeed = Input.HorizontalMovement == 0 ? Movement.TopSpeedX : Mathf.Abs(Input.HorizontalMovement) * Movement.TopSpeedX;
        Body.linearVelocityX = Mathf.Clamp(Body.linearVelocityX, -topSpeed, topSpeed);
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
    public virtual void HandleJumpInput() {
        if (!Input.Jump || FallingThroughPlatform) return;
        if (Input.VerticalMovement < 0 && GroundCheck.IsTouchingLayer("Platforms") && Body.linearVelocityY <= 0.1f) {
            JumpThroughPlatform();
        }
        else if (Jump.CanJump()) {
            Jump.Jump();
        }
        else if (WallJump.CanWallJump()) {
            WallJump.WallJump();
        }
    }

    public IEnumerator DisableMovementInputForSecondsCoroutine(float time) {
        DisableMovementInput = true;
        DisableHorizontalDrag = true;
        yield return new WaitForSeconds(time);
        DisableMovementInput = false;
        DisableHorizontalDrag = false;
    }


    protected void JumpThroughPlatform() {
        StartCoroutine(JumpThroughPlatformCoroutine());
    }

    protected IEnumerator JumpThroughPlatformCoroutine() {
        Body.linearVelocityY = 4f;
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
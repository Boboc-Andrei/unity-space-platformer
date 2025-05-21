using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.TextCore.Text;
public abstract class BaseState<T> : IState {

    protected T subject;

    public BaseState(T subject) {
        this.subject = subject;
    }
    public virtual void FixedUpdate() { }

    public virtual void OnEnter() { }

    public virtual void OnExit() { }

    public virtual void Update() { }
}

public class IdleState : BaseState<CharacterMovementController> {
    public IdleState(CharacterMovementController subject) : base(subject) {
    }

    public override void OnEnter() {
        subject.Animator.Play("Idle");
        Debug.Log("Entered idle state");

    }

    public override void Update() {
        subject.FaceMovementDirection();
    }

    public override void FixedUpdate() {
        subject.HandleMoveInput();
        subject.HandleJumpInput();
        subject.ApplyHorizontalGroundDrag();
    }

    public override void OnExit() {
        Debug.Log("Exited idle state");
    }
}

public class WalkingState : BaseState<CharacterMovementController> {
    public WalkingState(CharacterMovementController subject) : base(subject) {

    }

    public override void OnEnter() {
        subject.Animator.Play("Walk");
        Debug.Log("Entered walking state");

    }

    public override void Update() {
        subject.FaceMovementDirection();
        ModulateAnimatorSpeed();
    }

    public override void FixedUpdate() {
        subject.HandleMoveInput();
        subject.HandleJumpInput();
        subject.ApplyHorizontalGroundDrag();
        subject.LimitWalkingSpeed();
    }

    public override void OnExit() {
        Debug.Log("Exited walking state");
    }

    private void ModulateAnimatorSpeed() {
        subject.Animator.speed = Helpers.Map(Mathf.Abs(subject.Body.linearVelocityX), 0, subject.Movement.TopSpeedX, 0, 0.999f, true);
    }
}

public class AirborneState : BaseState<CharacterMovementController> {
    private bool IsJumping;

    public AirborneState(CharacterMovementController subject) : base(subject) {
        
    }

    public override void OnEnter() {
        Debug.Log("Entered airborne state");
        subject.Animator.Play("Jump");
        IsJumping = subject.Input.Jump;
    }

    public override void Update() {
        subject.FaceMovementDirection();
        MapVelocityToFrames();

        if (subject.Body.linearVelocityY <= 0) IsJumping = false;
        if(IsJumping && subject.Input.CancelJump) subject.Body.linearVelocityY *= subject.Movement.JumpCutoffFactor;
    }

    public override void FixedUpdate() {
        subject.HandleMoveInput();
        if(!IsJumping && subject.CanCoyoteJump() && subject.Input.Jump){
            subject.Jump();
            IsJumping = true;
        }

        subject.ApplyHorizontalAirDrag();
        subject.ApplyAdaptiveGravity();
        subject.LimitWalkingSpeed();
    }

    public override void OnExit() {
        Debug.Log("Exited airborne state");
    }

    private void MapVelocityToFrames() {
        float time = Helpers.Map(subject.Body.linearVelocity.y, subject.Movement.JumpSpeed, -subject.Movement.TerminalVelocity, 0, 0.999f, true);
        subject.Animator.Play("Jump", 0, time);
    }
}
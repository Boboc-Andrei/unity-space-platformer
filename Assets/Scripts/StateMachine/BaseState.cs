using System.Runtime.InteropServices;
using UnityEngine.TextCore.Text;
using UnityEngine;

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
    }

    public override void Update() {
        subject.FaceMovementDirection();
    }

    public override void FixedUpdate() {
        subject.HandleMoveInput();
        subject.HandleJumpInput();
        subject.ApplyHorizontalGroundDrag();
    }
}

public class WalkingState : BaseState<CharacterMovementController> {
    public WalkingState(CharacterMovementController subject) : base(subject) {

    }

    public override void OnEnter() {
        subject.Animator.Play("Walk");
    }

    public override void Update() {
        subject.FaceMovementDirection();
    }

    public override void FixedUpdate() {
        subject.HandleMoveInput();
        subject.HandleJumpInput();
        subject.ApplyHorizontalGroundDrag();
        subject.LimitWalkingSpeed();
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
        subject.Animator.Play("Jump");
        subject.Animator.speed = 0;
        IsJumping = subject.Input.Jump;
    }

    public override void Update() {
        subject.FaceMovementDirection();
        MapVelocityToFrames();

        if (subject.Body.linearVelocityY <= 0) IsJumping = false;
        if (IsJumping && subject.Input.CancelJump) subject.Body.linearVelocityY *= subject.Movement.JumpCutoffFactor;
    }

    public override void FixedUpdate() {
        subject.HandleMoveInput();
        subject.ApplyHorizontalAirDrag();
        subject.ApplyAdaptiveGravity();
        subject.LimitWalkingSpeed();
    }

    public override void OnExit() {
        subject.Animator.speed = 1;
    }

    private void MapVelocityToFrames() {
        float time = Helpers.Map(subject.Body.linearVelocity.y, subject.Movement.JumpSpeed, -subject.Movement.TerminalVelocity, 0, 0.999f, true);
        subject.Animator.Play("Jump", 0, time);
    }

}
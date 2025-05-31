using UnityEngine;

public class AirborneState : BaseState<CharacterMovementController> {

    public AirborneState(CharacterMovementController subject) : base(subject) {
        
    }

    public override void OnEnter() {
        subject.Animator.Play("Jump");
        subject.Animator.speed = 0;
    }

    public override void Update() {
        subject.FaceMovementDirection();
        MapVelocityToFrames();

        if (subject.Body.linearVelocityY <= 0) subject.IsJumping = false;
        if(subject.IsJumping && subject.Input.CancelJump) { 
            subject.Body.linearVelocityY *= subject.Jump.InterruptionFactor;
            subject.IsJumping = false;
        }
    }

    public override void FixedUpdate() {
        subject.HandleMoveInput();
        subject.HandleJumpInput();
        subject.HandleDashInput();

        subject.ApplyHorizontalAirDrag();
        subject.ApplyAdaptiveGravity();
        subject.LimitWalkingSpeed();
    }

    public override void OnExit() {
        subject.Animator.speed = 1;
    }

    private void MapVelocityToFrames() {
        float time = Helpers.Map(subject.Body.linearVelocity.y, subject.Jump.Speed, -subject.Movement.TerminalVelocity, 0, 0.999f, true);
        subject.Animator.Play("Jump", 0, time);
    }
}
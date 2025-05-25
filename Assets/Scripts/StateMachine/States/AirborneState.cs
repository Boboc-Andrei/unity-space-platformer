using UnityEngine;

public class AirborneState : BaseState<CharacterMovementController> {

    public AirborneState(CharacterMovementController subject) : base(subject) {
        
    }

    public override void OnEnter() {
        Debug.Log("Entered airborne state");
        subject.Animator.Play("Jump");
    }

    public override void Update() {
        subject.FaceMovementDirection();
        MapVelocityToFrames();

        if (subject.Body.linearVelocityY <= 0) subject.IsJumping = false;
        if(subject.IsJumping && subject.Input.CancelJump) { 
            subject.Body.linearVelocityY *= subject.Movement.JumpCutoffFactor;
            subject.IsJumping = false;
        }
    }

    public override void FixedUpdate() {
        subject.HandleMoveInput();
        subject.HandleJumpInput();

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
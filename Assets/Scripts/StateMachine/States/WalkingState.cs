using UnityEngine;

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

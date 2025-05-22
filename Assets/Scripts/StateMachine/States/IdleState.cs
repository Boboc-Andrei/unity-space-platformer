using UnityEngine;

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

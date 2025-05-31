using UnityEngine;

public class IdleState : BaseState<CharacterMovementController> {
    public IdleState(CharacterMovementController subject) : base(subject) {
    }

    public override void OnEnter() {
        subject.Animator.Play("Idle");

    }

    public override void Update() {
        subject.HandleGroundedFlagsReset();
        subject.FaceMovementDirection();
    }

    public override void FixedUpdate() {
        subject.HandleMoveInput();
        subject.HandleJumpInput();
        subject.HandleDashInput();

        subject.ApplyHorizontalGroundDrag();
    }

    public override void OnExit() {
    }
}

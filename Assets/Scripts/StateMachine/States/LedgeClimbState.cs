using System.Security.Cryptography.X509Certificates;
using UnityEngine;
internal class LedgeClimbState : BaseState<CharacterMovementController> {
    private int ledgeDirection;
    private float startTime;
    private float climbDuration;
    public LedgeClimbState(CharacterMovementController subject) : base(subject) {
        
    }
    public override void OnEnter() {
        subject.Animator.Play("Ledge Climb");
        climbDuration = subject.Animator.GetCurrentAnimatorStateInfo(0).length;
        subject.DisableGravity();
        ledgeDirection = subject.IsTouchingGrabbableLedge;
        subject.LookTowards(ledgeDirection);
        subject.DisableTurning = true;
        subject.Body.linearVelocityX = 0;
        subject.Body.linearVelocityY = 0;
    }
    public override void Update() {
        //if (Time.time - startTime >= climbDuration) {
        //    subject.Animator.Play("Idle");
        //}
    }

    public override void FixedUpdate() {

    }

    public override void OnExit() {
        subject.ApplyFallingGravity();
        subject.DisableTurning = false;

        var offset = new Vector2(
            subject.GrabbableLedge.position.x + .375f * ledgeDirection,
            subject.GrabbableLedge.position.y + 1.125f
            );
        subject.Body.transform.position = offset;
    }
}
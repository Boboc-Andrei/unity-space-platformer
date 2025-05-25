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
        subject.Animator.speed = 1;
        climbDuration = subject.Animator.GetCurrentAnimatorStateInfo(0).length;
        subject.DisableGravity();
        ledgeDirection = subject.IsTouchingGrabbableLedge;
        Debug.Log($"Entered ledge climb state. Climbing {ledgeDirection} ledge");
        subject.LookTowards(ledgeDirection);
        subject.DisableTurning = true;
        subject.SetVelocityX(0);
        subject.SetVelocityX(0);
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
        Debug.Log("Climb animation finished. moving towards " + offset);
        subject.Body.transform.position = offset;
    }
}
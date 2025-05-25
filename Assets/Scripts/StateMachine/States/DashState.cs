using UnityEngine;
class DashState : BaseState <CharacterMovementController> {
    public DashState(CharacterMovementController subject) : base(subject) {
        
    }
    public bool IsFinished = false;
    private float startTime;

    public override void OnEnter() {
        IsFinished = false;
        startTime = Time.time;
        Debug.Log("Entered dash state");
        subject.Animator.Play("Dash");
        subject.DisableGravity();
        subject.SetVelocityY(0);
    }

    public override void Update() {
        if(Time.time - startTime > subject.Movement.DashDuration) {
            IsFinished = true;
        }
    }
    public override void FixedUpdate() {
        if(!IsFinished)
            subject.SetVelocityX(subject.Movement.DashSpeed * subject.FacingDirection);   
    }
    public override void OnExit() {
        Debug.Log("Exited dash state");
        subject.StartDashCooldown();
        subject.ApplyFallingGravity();   
    }
}

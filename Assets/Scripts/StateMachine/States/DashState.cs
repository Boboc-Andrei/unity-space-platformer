using UnityEngine;
class DashState : BaseState <CharacterMovementController> {
    public DashState(CharacterMovementController subject) : base(subject) {
        
    }


    public override void OnEnter() {
        subject.Animator.Play("Dash");
        subject.Dash.StartDash();
    }

    public override void Update() {

    }
    public override void FixedUpdate() {
        
    }
    public override void OnExit() {
        subject.ApplyFallingGravity();   
    }
}

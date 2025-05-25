using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


class LedgeHangState : BaseState<CharacterMovementController> {
    public LedgeHangState(CharacterMovementController subject) : base(subject){
        
    }

    public override void OnEnter() {
        subject.Animator.Play("Ledge Hang");
        subject.DisableGravity();
        subject.LookTowards(subject.IsTouchingGrabbableLedge);
        subject.DisableTurning = true;
        subject.SetVelocityX(0);
        subject.SetVelocityY(0);
        subject.DisableMovementInputForSeconds(.2f);
        

        var ledgeDetector = subject.IsTouchingGrabbableLedge == -1 ? subject.LeftLedgeDetector : subject.RightLedgeDetector;
        var offset = ledgeDetector.transform.position - subject.transform.position;
        subject.transform.position = ledgeDetector.LastTouched.transform.position - offset;
    }

    public override void Update() {
        
    }

    public override void FixedUpdate() {
        subject.HandleMoveInput();
        subject.HandleJumpInput();
    }

    public override void OnExit() {
        subject.DisableTurning = false;
        subject.ApplyAdaptiveGravity();
        subject.StartWallGrabCooldown();
    }
}

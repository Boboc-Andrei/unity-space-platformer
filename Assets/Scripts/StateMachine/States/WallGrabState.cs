using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class WallGrabState : BaseState<CharacterMovementController> {

    public WallGrabState(CharacterMovementController subject) : base(subject) {

    }

    public override void OnEnter() {
        subject.Animator.Play("Wall Hang");
        subject.WallGrab.Grab();
    }

    public override void Update() {
    }

    public override void FixedUpdate() {
        subject.HandleMoveInput();
        subject.HandleJumpInput();
        subject.WallGrab.CurrentStamina -= Time.deltaTime;
        if (subject.WallGrab.CurrentStamina <= 0) {
            subject.WallGrab.Slide();
        }
    }

    public override void OnExit() {
        subject.WallGrab.Release();
    }
}

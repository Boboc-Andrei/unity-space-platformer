using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


class WallGrabState : BaseState<CharacterMovementController> {
    
    public WallGrabState(CharacterMovementController subject) : base(subject) {
        
    }

    public override void OnEnter() {
        subject.Animator.Play("Wall Hang");
        subject.DisableGravity();
        subject.LookTowards(subject.IsTouchingWall);
        subject.DisableTurning = true;
        subject.Body.linearVelocityX = subject.IsTouchingWall * 2f;
        subject.Body.linearVelocityY = 0;
    }

    public override void Update() {
    }

    public override void FixedUpdate() {
        subject.HandleMoveInput();
        subject.HandleJumpInput();
        subject.CurrentWallSlideStamina -= Time.deltaTime;
        if(subject.CurrentWallSlideStamina <= 0) {
            subject.ApplyAccelerationY(-subject.Movement.WallSlideAcceleration);
            subject.Body.linearVelocityY = MathF.Max(subject.Body.linearVelocityY, -subject.Movement.WallSlideMaximumVelocity);
        }
    }

    public override void OnExit() {
        subject.DisableTurning = false;
        subject.ApplyAdaptiveGravity();
        subject.StartWallGrabCooldown();
    }
}

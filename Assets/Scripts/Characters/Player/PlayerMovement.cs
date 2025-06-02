using NUnit.Framework.Constraints;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

internal class PlayerMovement : CharacterMovementController {
    private void Awake() {
        Input = GetComponent<PlayerInputController>();
        stateMachine = new StateMachine();

        // Initialize abilities
        Jump = GetComponent<JumpComponent>();
        Jump.Character = this;
        WallJump = GetComponent<WallJumpComponent>();
        WallJump.Character = this;
        Dash = GetComponent<DashComponent>();
        Dash.Character = this;
        WallGrab = GetComponent<WallGrabComponent>();
        WallGrab.Character = this;

        // Initialize states
        var idleState = new IdleState(this);
        var walkState = new WalkingState(this);
        var airborneState = new AirborneState(this);
        var wallSlideState = new WallGrabState(this);
        var ledgeHangState = new LedgeHangState(this);
        var ledgeClimbState = new LedgeClimbState(this);
        var dashState = new DashState(this);

        // Transitions
        At(idleState, walkState, new FuncPredicate(
            () => Mathf.Abs(Body.linearVelocityX) > 0.1f));
        At(idleState, airborneState, new FuncPredicate(
            () => !IsGrounded));

        At(airborneState, walkState, new FuncPredicate(
            () => IsGrounded && Input.HorizontalMovement != 0 && Mathf.Abs(Body.linearVelocityX) > 0.1f));
        At(airborneState, idleState, new FuncPredicate(
            () => IsGrounded));
        At(airborneState, wallSlideState, new FuncPredicate(
            () => WallGrab.IsEnabled && Input.Grab && IsTouchingWall != 0 && Body.linearVelocityY <= 0f && Input.HorizontalMovement != -IsTouchingWall));
        At(airborneState, ledgeHangState, new FuncPredicate(
            () => Input.Grab && IsTouchingGrabbableLedge != 0 && Body.linearVelocityY <= 0.1f && WallGrab.IsEnabled));


        At(walkState, idleState, new FuncPredicate(
            () => IsGrounded && Mathf.Abs(Body.linearVelocityX) <= 0.1f));
        At(walkState, airborneState, new FuncPredicate(
            () => !IsGrounded));


        At(wallSlideState, idleState, new FuncPredicate(
            () => IsGrounded));
        At(wallSlideState, airborneState, new FuncPredicate(
            () => !Input.Grab || IsTouchingWall == 0 || (Mathf.Abs(Input.HorizontalMovement) > .1f && Mathf.Sign(Input.HorizontalMovement) == -IsTouchingWall)));
        At(wallSlideState, ledgeHangState, new FuncPredicate(
            () => IsTouchingGrabbableLedge != 0));

        At(ledgeHangState, airborneState, new FuncPredicate(
            () => !Input.Grab || IsTouchingGrabbableLedge == 0 || Body.linearVelocityX != 0));

        At(ledgeHangState, ledgeClimbState, new FuncPredicate(
            () => Input.VerticalMovement > 0));

        At(ledgeClimbState, idleState, new FuncPredicate(
            () => Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1));

        At(dashState, idleState, new FuncPredicate(
            () => !Dash.IsActive));

        Any(dashState, new FuncPredicate(
            () => Dash.IsActive));

        stateMachine.SetState(idleState);
    }

    private void Update() {
        stateMachine.Update();
    }

    private void FixedUpdate() {
        stateMachine.FixedUpdate();
    }
    void At(IState from, IState to, IPredicate condition) => stateMachine.AddTransition(from, to, condition);
    void Any(IState to, IPredicate condition) => stateMachine.AddAnyTransition(to, condition);

    internal void EnterStage(int direction) {
        IEnumerator ApplyContinuousVelocityOverDuration(float velocity, float time) {
            float startTime = Time.time;
            while (Time.time - startTime <= time) {
                Body.linearVelocityX = velocity;
                yield return null;
            }
        }
        StartCoroutine(ApplyContinuousVelocityOverDuration(Movement.TopSpeedX * .8f * direction, .3f));
        StartCoroutine(DisableMovementInputForSecondsCoroutine(.3f));
    }

}
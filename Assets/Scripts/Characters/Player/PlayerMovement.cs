using System;
using UnityEngine;
using UnityEngine.Rendering;

internal class PlayerMovement : CharacterMovementController {
    private void Awake() {
        stateMachine = new StateMachine();

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
            () => Input.HorizontalMovement != 0 && Mathf.Abs(Body.linearVelocityX) > 0.1f));
        At(idleState, airborneState, new FuncPredicate(
            () => !IsGrounded));
        At(idleState, dashState, new FuncPredicate(
            () => Input.Dash && DashAvailable));

        At(airborneState, walkState, new FuncPredicate(
            () => IsGrounded && Input.HorizontalMovement != 0 && Mathf.Abs(Body.linearVelocityX) > 0.1f));
        At(airborneState, idleState, new FuncPredicate(
            () => IsGrounded));
        At(airborneState, wallSlideState, new FuncPredicate(
            () => CanGrabWall && Input.Grab && IsTouchingWall != 0 && Body.linearVelocityY <= 0f && Input.HorizontalMovement !=  -IsTouchingWall));
        At(airborneState, ledgeHangState, new FuncPredicate(
            () => Input.Grab && IsTouchingGrabbableLedge != 0 && Body.linearVelocityY <= 0.1f && CanGrabWall));
        At(airborneState, dashState, new FuncPredicate(
    () => Input.Dash && DashAvailable));

        At(walkState, idleState, new FuncPredicate(
            () => IsGrounded && Mathf.Abs(Body.linearVelocityX) <= 0.1f));
        At(walkState, airborneState, new FuncPredicate(
            () => !IsGrounded));
        At(walkState, dashState, new FuncPredicate(
    () => Input.Dash && DashAvailable));

        At(wallSlideState, idleState, new FuncPredicate(
            () => IsGrounded));
        At(wallSlideState, airborneState, new FuncPredicate(
            () => !Input.Grab || IsTouchingWall == 0 || (Mathf.Abs(Input.HorizontalMovement) > .1f && Mathf.Sign(Input.HorizontalMovement) ==  -IsTouchingWall)));
        At(wallSlideState, ledgeHangState, new FuncPredicate(
            () => IsTouchingGrabbableLedge != 0));

        At(ledgeHangState, airborneState, new FuncPredicate(
            () => !Input.Grab || IsTouchingGrabbableLedge == 0 || Body.linearVelocityX != 0));

        At(ledgeHangState, ledgeClimbState, new FuncPredicate(
            () => Input.VerticalMovement > 0));

        At(ledgeClimbState, idleState, new FuncPredicate(
            () => Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1));

        At(dashState, idleState, new FuncPredicate(
            () => dashState.IsFinished));

        stateMachine.SetState(idleState);
    }

    void At(IState from, IState to, IPredicate condition) => stateMachine.AddTransition(from, to, condition);
    void Any(IState to, IPredicate condition) => stateMachine.AddAnyTransition(to, condition);

    private void Update() {
        //Debug.Log($"Dash input: {Input.Dash}, Dash available: {DashAvailable}");
        stateMachine.Update();
    }

    private void FixedUpdate() {
        stateMachine.FixedUpdate();
    }
}
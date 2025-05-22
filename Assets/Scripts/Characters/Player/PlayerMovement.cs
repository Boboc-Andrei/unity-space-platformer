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

        // Transitions
        At(idleState, walkState, new FuncPredicate(
            () => Input.HorizontalMovement != 0 && Mathf.Abs(Body.linearVelocityX) > 0.1f));
        At(idleState, airborneState, new FuncPredicate(
            () => !IsGrounded));

        At(airborneState, walkState, new FuncPredicate(
            () => IsGrounded && Input.HorizontalMovement != 0 && Mathf.Abs(Body.linearVelocityX) > 0.1f));
        At(airborneState, idleState, new FuncPredicate(
            () => IsGrounded));
        At(airborneState, wallSlideState, new FuncPredicate(
            () => CanGrabWall && Input.Grab && IsTouchingWall != 0 && Body.linearVelocityY <= 0f && Input.HorizontalMovement !=  -IsTouchingWall));

        At(walkState, idleState, new FuncPredicate(
            () => IsGrounded && Input.HorizontalMovement == 0 && Mathf.Abs(Body.linearVelocityX) <= 0.1f));
        At(walkState, airborneState, new FuncPredicate(
            () => !IsGrounded));


        At(wallSlideState, idleState, new FuncPredicate(
            () => IsGrounded));
        At(wallSlideState, airborneState, new FuncPredicate(
            () => !Input.Grab || IsTouchingWall == 0 || Input.HorizontalMovement ==  -IsTouchingWall));

        stateMachine.SetState(idleState);
    }

    void At(IState from, IState to, IPredicate condition) => stateMachine.AddTransition(from, to, condition);
    void Any(IState to, IPredicate condition) => stateMachine.AddAnyTransition(to, condition);

    private void Update() {
        stateMachine.Update();
    }

    private void FixedUpdate() {
        stateMachine.FixedUpdate();
    }
}
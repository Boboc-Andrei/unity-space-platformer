using System;
using UnityEngine;
using UnityEngine.Rendering;

internal class PlayerMovement : CharacterMovementController {
    private void Awake() {
        stateMachine = new StateMachine();

        // Initialize states
        var idleState = new IdleState(this);
        var walkState = new WalkingState(this);
        var jumpState = new AirborneState(this);

        // Transitions
        At(idleState, walkState, new FuncPredicate(
            () => Input.HorizontalMovement != 0 && Mathf.Abs(Body.linearVelocityX) > 0.1f));
        At(idleState, jumpState, new FuncPredicate(
            () => !IsGrounded));

        //At(jumpState, walkState, new FuncPredicate(
        //    () => IsGrounded && Input.HorizontalMovement != 0 && Mathf.Abs(Body.linearVelocityX) > 0.1f));
        At(jumpState, idleState, new FuncPredicate(
            () => IsGrounded));

        At(walkState, idleState, new FuncPredicate(
            () => IsGrounded && !(Input.HorizontalMovement != 0 && Mathf.Abs(Body.linearVelocityX) > 0.1f)));
        At(walkState, jumpState, new FuncPredicate(
            () => !IsGrounded));

        stateMachine.SetState(idleState);
    }

    void At(IState from, IState to, IPredicate condition) => stateMachine.AddTransition(from, to, condition);
    void Any(IState to, IPredicate condition) => stateMachine.AddAnyTransition(to, condition);

    private void Update() {
        stateMachine.Update();
        Debug.Log(IsGrounded);
    }

    private void FixedUpdate() {
        stateMachine.FixedUpdate();
    }
}
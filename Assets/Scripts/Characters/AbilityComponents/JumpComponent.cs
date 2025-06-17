using UnityEngine;

public class JumpComponent : CharacterAbilityComponent, IJumpable {
    public float Speed;
    public float InterruptionFactor;

    public void Jump() {
        Character.IsJumping = true;
        Character.Body.linearVelocityY = Character.Jump.Speed;
    }

    public bool CanJump() {
        return (Character.IsGrounded || CanCoyoteJump()) && Character.Body.linearVelocityY <= 0.1f && !Character.FallingThroughPlatform;
    }

    public bool CanCoyoteJump() {
        return Character.TimeSinceGrounded <= Character.CoyoteTime;
    }
}

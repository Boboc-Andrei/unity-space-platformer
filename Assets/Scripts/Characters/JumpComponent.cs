public class JumpComponent : CharacterAbilityComponent, IJumpable {
    public void Jump() {
        Context.IsJumping = true;
        Context.Body.linearVelocityY = Context.Movement.JumpSpeed;
    }

    public bool CanJump() {
        return (Context.IsGrounded || CanCoyoteJump()) && Context.Body.linearVelocityY <= 0.1f && !Context.FallingThroughPlatform;
    }

    public bool CanCoyoteJump() {
        return Context.TimeSinceGrounded <= Context.Movement.CoyoteTime;
    }
}

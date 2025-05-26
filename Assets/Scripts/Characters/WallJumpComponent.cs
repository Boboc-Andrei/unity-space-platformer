using System.Collections;
using UnityEngine;

public class WallJumpComponent : CharacterAbilityComponent, IWallJumpable {
    public bool CanWallJump() {
        return !Context.IsGrounded && Context.IsTouchingWall != 0 && Context.Body.linearVelocityY < .1f;
    }

    public void WallJump() {
        Context.IsJumping = true;
        var wallJumpDirection = -Context.IsTouchingWall;
        Context.Body.linearVelocityY = Context.Movement.JumpSpeed;
        Context.Body.linearVelocityX = Context.Movement.TopSpeedX * wallJumpDirection;
        StartCoroutine(LerpMovementAccelerationCoroutine(.3f));
    }

    private IEnumerator LerpMovementAccelerationCoroutine(float time) {
        float elapsed = 0;
        Context.DisableHorizontalDrag = true;
        while (elapsed < time) {
            Context.MoveSpeedFactor = Mathf.Lerp(0f, .75f, elapsed / time);
            elapsed += Time.deltaTime;
            yield return null;
        }
        Context.DisableHorizontalDrag = false;
        Context.MoveSpeedFactor = 1;
    }
}

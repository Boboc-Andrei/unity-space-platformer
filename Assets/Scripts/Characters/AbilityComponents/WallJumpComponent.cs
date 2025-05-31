using System.Collections;
using UnityEngine;

public class WallJumpComponent : CharacterAbilityComponent, IWallJumpable {
    public bool CanWallJump() {
        return !Character.IsGrounded && Character.IsTouchingWall != 0 && Character.Body.linearVelocityY < .1f;
    }

    public void WallJump() {
        Character.IsJumping = true;
        var wallJumpDirection = -Character.IsTouchingWall;
        Character.Body.linearVelocityY = Character.Jump.Speed;
        Character.Body.linearVelocityX = Character.Movement.TopSpeedX * wallJumpDirection;
        StartCoroutine(LerpMovementAccelerationCoroutine(.3f, wallJumpDirection));
    }

    private IEnumerator LerpMovementAccelerationCoroutine(float time, int direction) {
        float elapsed = 0;
        Character.DisableHorizontalDrag = true;
        while (elapsed < time) {
            Character.MoveSpeedFactor = Mathf.Lerp(0f, .75f, elapsed / time);
            Character.LookTowards(direction);
            elapsed += Time.deltaTime;
            yield return null;
        }
        Character.DisableHorizontalDrag = false;
        Character.MoveSpeedFactor = 1;
    }
}

using System;
using System.Collections;
using UnityEngine;

public class WallGrabComponent : CharacterAbilityComponent, IWallGrabbable {
    public bool IsEnabled { get; set; } = true;
    public float CurrentStamina { get; set; } = 1f;
    public float WallSlideAcceleration;
    public float WallSlideMaximumVelocity;
    [field:SerializeField] public float MaxStamina { get; set; } = 1f;

    public void Grab() {
        Context.DisableGravity();
        Context.LookTowards(Context.IsTouchingWall);
        Context.DisableTurning = true;
        Context.Body.linearVelocityX = Context.IsTouchingWall * 2f;
        Context.Body.linearVelocityY = 0;
    }
    public void Slide() {
        Context.ApplyAccelerationY(-WallSlideAcceleration);
        Context.Body.linearVelocityY = MathF.Max(Context.Body.linearVelocityY, -WallSlideMaximumVelocity);
    }

    public void Release() {
        Context.DisableTurning = false;
        Context.ApplyAdaptiveGravity();
        StartCooldown();
    }

    public void StartCooldown() {
        StartCoroutine(DisableWallGrabbingForSeconds(.2f));
    }

    private IEnumerator DisableWallGrabbingForSeconds(float time) {
        IsEnabled = false;
        yield return new WaitForSeconds(time);
        IsEnabled = true;
    }

    public void ResetStamina() {
        CurrentStamina = MaxStamina;
    }
}

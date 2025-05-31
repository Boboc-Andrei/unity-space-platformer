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
        Character.DisableGravity();
        Character.LookTowards(Character.IsTouchingWall);
        Character.DisableTurning = true;
        Character.Body.linearVelocityX = Character.IsTouchingWall * 2f;
        Character.Body.linearVelocityY = 0;
    }
    public void Slide() {
        Character.ApplyAccelerationY(-WallSlideAcceleration);
        Character.Body.linearVelocityY = MathF.Max(Character.Body.linearVelocityY, -WallSlideMaximumVelocity);
    }

    public void Release() {
        Character.DisableTurning = false;
        Character.ApplyAdaptiveGravity();
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

using System.Collections;
using UnityEngine;

public class DashComponent : CharacterAbilityComponent, IDashable {
    public float Speed;
    public float Duration;
    public bool IsActive { get; set; } = false;
    public bool IsEnabled { get; set; } = true;
    public void StartDashCooldown() {
        StartCoroutine(DisableDashingForSeconds(.4f));
    }

    private IEnumerator DisableDashingForSeconds(float time) {
        IsEnabled = false;
        yield return new WaitForSeconds(time);
        IsEnabled = true;
    }

    public void StartDash() {
        StartCoroutine(ApplyVelocityForDuration(Speed, Duration));
    }
    
    private IEnumerator ApplyVelocityForDuration(float velocity, float duration) {
        IsActive = true;
        Context.DisableGravity();

        float startTime = Time.time;
        while(Time.time - startTime <= duration){
            Context.Body.linearVelocity = new Vector3(velocity * Context.FacingDirection, 0);
            yield return null;
        }

        IsActive = false;
        StartDashCooldown();
    }
}

using System.Collections;
using UnityEngine;

public class DashComponent : CharacterAbilityComponent, IDashable {
    public float Speed;
    public float Duration;
    public bool IsActive { get; set; } = false;
    public bool IsAvailable { get; set; } = true;
    public bool IsOffCooldown { get; set; } = true;
    public void StartDashCooldown() {
        StartCoroutine(DisableDashingForSeconds(.4f));
    }

    private IEnumerator DisableDashingForSeconds(float time) {
        IsOffCooldown = false;
        yield return new WaitForSeconds(time);
        IsOffCooldown = true;
    }

    public void StartDash(int direction) {
        direction = Mathf.Clamp(direction, -1, 1);
        StartCoroutine(DashCoroutine(direction));
    }

    private IEnumerator DashCoroutine(int direction) {
        IsActive = true;
        Character.DisableGravity();

        float startTime = Time.time;
        while (Time.time - startTime <= Duration) {
            Character.Body.linearVelocity = new Vector3(Speed * direction, 0);
            Character.LookTowards(direction);
            yield return null;
        }

        IsActive = false;
        IsAvailable = false;
        StartDashCooldown();
    }
}

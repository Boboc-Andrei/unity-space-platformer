using UnityEngine;

[CreateAssetMenu(fileName = "CharacterMovementParams", menuName = "Scriptable Objects/CharacterMovementParams")]
public class CharacterMovementParams : ScriptableObject {
    [Header("Horizontal Movement")]
    public float TopSpeedX = 9f;
    public float AccelerationX = .75f;

    [Header("Jumping")]
    public float JumpSpeed = 21f;
    [Range(0,1)] public float JumpCutoffFactor = 0.5f;
    [Range(0, 0.5f)] public float CoyoteTime = 0.15f;

    [Header("Environment")]
    public float RisingGravity = 4f;
    public float FallingGravity = 6f;
    public float TerminalVelocity = 30f;
    [Range(0, 1)] public float GroundHorizontalDrag = .8f;
    [Range(0, 1)] public float AirHorizontalDrag = .8f;

    [Header("Wall Sliding")]
    public float WallSlideAcceleration = 1f;
    public float WallSlideMaximumVelocity = 2f;

    [Header("Dash")]
    public float DashSpeed = 20f;
    public float DashDuration = .25f;
}

using UnityEngine;

[CreateAssetMenu(fileName = "CharacterMovementParams", menuName = "Scriptable Objects/CharacterMovementParams")]
public class CharacterMovementParams : ScriptableObject {
    [Header("Horizontal Movement")]
    public float TopSpeedX = 9f;
    public float AccelerationX = .75f;

    [Header("Environment")]
    public float RisingGravity = 4f;
    public float FallingGravity = 6f;
    public float TerminalVelocity = 30f;
    [Range(0, 1)] public float GroundHorizontalDrag = .8f;
    [Range(0, 1)] public float AirHorizontalDrag = .8f;

}

using UnityEngine;

internal class PlayerMovement : CharacterMovementController {
    private void Start() {
        LookTowards(-1);
    }
}
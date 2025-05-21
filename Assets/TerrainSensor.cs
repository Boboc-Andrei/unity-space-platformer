using UnityEngine;

public class TerrainSensor : MonoBehaviour {
    [field: SerializeField] public bool IsTouching => TouchingCount != 0;
    public int TouchingCount = 0;
    public LayerMask Mask;
    public float TimeSinceTouched;

    private void FixedUpdate() {
        if(IsTouching) {
            TimeSinceTouched = 0;
        }
        else {
            TimeSinceTouched += Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (((1 << collision.gameObject.layer) & Mask.value) == 0) return;
        TouchingCount += 1;
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (((1 << collision.gameObject.layer) & Mask.value) == 0) return;
        TouchingCount -= 1;
    }
}

using UnityEngine;

public class TerrainSensor : MonoBehaviour {
    [field:SerializeField] public bool IsTouching { get; set; }
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
        IsTouching = true;
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (((1 << collision.gameObject.layer) & Mask.value) == 0) return;
        IsTouching = false;
    }
}

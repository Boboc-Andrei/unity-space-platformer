using System.Collections.Generic;
using UnityEngine;

public class TerrainSensor : MonoBehaviour {
    [field: SerializeField] public bool IsTouching => TouchingCount != 0;
    public int TouchingCount => touchingColliders.Count;
    public LayerMask Mask;
    public float TimeSinceTouched;
    private HashSet<Collider2D> touchingColliders = new HashSet<Collider2D>();

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
        touchingColliders.Add(collision);
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (((1 << collision.gameObject.layer) & Mask.value) == 0) return;
        touchingColliders.Remove(collision);
    }

    public bool IsTouchingLayer(string layerName) {
        int layerIndex = LayerMask.NameToLayer(layerName);

        if(layerIndex < 0) {
            Debug.LogWarning($"Layer {layerName} does not exist!");
        }
        
        foreach(var collider in touchingColliders) {
            if(collider.gameObject.layer == layerIndex) {
                return true;
            }
        }
        return false;
    }
}

using Unity.Cinemachine;
using UnityEngine;

public class ConfinerShapeFinder : MonoBehaviour
{
    private CinemachineConfiner2D confiner;
    void Start()
    {
        confiner = GetComponent<CinemachineConfiner2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(confiner.BoundingShape2D == null) {
            FindConfiner();
        }
    }

    private void FindConfiner() {
        var collider = GameObject.Find("LevelBounds");

        if (collider == null) Debug.LogWarning("Could not find level confiner shape");
        else confiner.BoundingShape2D = collider.GetComponent<Collider2D>();
    }
}

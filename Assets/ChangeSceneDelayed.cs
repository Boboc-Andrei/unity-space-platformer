using UnityEngine;

public class ChangeSceneDelayed : MonoBehaviour
{
    private float startTime;
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - startTime > 10f) {
            SceneHelper.LoadScene("TestScene2", true, true);
            SceneHelper.UnloadScene("TestScene1");
        }
    }
}

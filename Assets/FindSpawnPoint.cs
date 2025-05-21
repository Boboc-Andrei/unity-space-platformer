using UnityEngine;

public class FindSpawnPoint : MonoBehaviour
{
    private void Update() {
        var spawnPosition = GameObject.Find("SpawnPoint");

        if (spawnPosition == null) return;

        transform.position = spawnPosition.transform.position;
        Destroy(this);
    }
}

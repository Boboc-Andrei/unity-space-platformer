using UnityEngine;

public class FindSpawnPoint : MonoBehaviour
{
    private void Update() {
        var spawnPosition = GameObject.Find("SpawnPoint");

        if (spawnPosition == null) return;

        Debug.Log("Player FindSpawnPoint: spawn point found at " + spawnPosition.transform.position);
        transform.position = spawnPosition.transform.position;
        Destroy(this);
    }
}

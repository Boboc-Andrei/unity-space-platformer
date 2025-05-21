using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerPrefab;
    [SerializeField] private bool spawnPlayerOnStart = true;
    private PlayerMovement Player;
    void Awake()
    {
        if (spawnPlayerOnStart) EnsurePlayerSpawn();
        CameraSetup();
    }

    private void EnsurePlayerSpawn() {
        var playerSpawnPosition = GameObject.Find("SpawnPoint").transform.position;
        if (playerSpawnPosition == null) playerSpawnPosition = Vector3.zero;
         
        if(Player == null) {
            Player = Instantiate(playerPrefab, playerSpawnPosition, Quaternion.identity);
        }
    }

    private void CameraSetup() {
        var currentCamera = FindFirstObjectByType<CinemachineCamera>();
        currentCamera.Target.TrackingTarget = Player.transform;
        currentCamera.transform.position = Player.transform.position;
    }
}

using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerPrefab;
    [SerializeField] private CinemachineCamera cameraPrefab;
    [SerializeField] private bool spawnPlayerOnStart = true;
    private PlayerMovement Player;
    private CinemachineCamera Camera;
    void Awake()
    {
        if (spawnPlayerOnStart) EnsurePlayerSpawn();
        CameraSetup();
    }

    private void EnsurePlayerSpawn() {
        var playerSpawnGameObject = GameObject.Find("SpawnPoint");
        Vector3 playerSpawnPosition = playerSpawnGameObject == null ? Vector3.zero : playerSpawnGameObject.transform.position;
         
        if(Player == null) {
            Player = Instantiate(playerPrefab, playerSpawnPosition, Quaternion.identity);
        }
    }


    private void CameraSetup() {
        var currentCamera = FindFirstObjectByType<CinemachineCamera>();
        if (currentCamera == null) {
            Camera = Instantiate(cameraPrefab, Player.transform.position, Quaternion.identity);
        }
        Camera.Target.TrackingTarget = Player.transform;
        Camera.transform.position = Player.transform.position;
    }
}

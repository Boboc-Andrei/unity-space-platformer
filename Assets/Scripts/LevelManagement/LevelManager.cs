using Eflatun.SceneReference;
using System;
using System.Linq;
using Unity.Cinemachine;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {
    private string lastSceneName = "[None]";
    private int enteredFromDirection = 0;
    private LevelTransition currentTransition;

    [SerializeField] private PlayerMovement playerPrefab;
    [SerializeField] private CinemachineCamera cameraPrefab;
    private PlayerMovement Player;
    private CinemachineCamera Camera;

    private Vector3 playerSpawnPoint;
    private GameObject defaultSpawnPoint;

    private bool QueuePlayerSetup = true;

    void Awake() {
    }


    private void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadMode) {
        FindCurrentLevelTransition();
        CallAfterDelay.Create(0, () => { QueuePlayerSetup = true; });
    }
    private void Start() {
    }

    private void Update() {
        if (QueuePlayerSetup) {
            FindDefaultSpawnPoint();
            FindCurrentLevelTransition();
            EnsurePlayerSpawn();
            CameraSetup();

            QueuePlayerSetup = false;
        }
    }

    private void EnsurePlayerSpawn() {

        // Find where to place player on scene load
        if (currentTransition != null) {
            playerSpawnPoint = currentTransition.transform.position + new Vector3(0, 1);
        }
        else {
            if (defaultSpawnPoint == null) return;
            playerSpawnPoint = defaultSpawnPoint.transform.position;
        }

        // Spawn player if it's null and not already spawned
        if (Player == null) {
            var existingPlayer = FindFirstObjectByType<PlayerMovement>();
            if (existingPlayer != null) Player = existingPlayer;
            else Player = Instantiate(playerPrefab, playerSpawnPoint, Quaternion.identity);
        }
        else {
            Player.transform.position = playerSpawnPoint;
        }

        if (currentTransition != null) {
            Player.EnterStage(-currentTransition.ExitDirection);
        }
    }

    private void FindCurrentLevelTransition() {
        currentTransition = FindObjectsByType<LevelTransition>(sortMode: FindObjectsSortMode.None).FirstOrDefault(lt => lt.To.Name == lastSceneName && lt.ExitDirection == enteredFromDirection);
        if (currentTransition == null) {
            return;
        }
        currentTransition.isActive = false;
    }

    private void FindDefaultSpawnPoint() {
        defaultSpawnPoint = GameObject.Find("SpawnPoint");
    }

    private void CameraSetup() {
        if (Player == null) return;
        var currentCamera = FindFirstObjectByType<CinemachineCamera>();
        if (currentCamera == null) {
            Camera = Instantiate(cameraPrefab, Player.transform.position, Quaternion.identity);
        }
        Camera.Target.TrackingTarget = Player.transform;
        Camera.transform.position = Player.transform.position;
    }

    public void TransitionToLevel(string fromScene, string toScene, int exitDirection) {
        lastSceneName = fromScene;
        enteredFromDirection = -exitDirection;
        SceneHelper.UnloadScene(fromScene);
        SceneHelper.LoadScene(toScene, true, true);
    }
}

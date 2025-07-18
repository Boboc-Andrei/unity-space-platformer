using Eflatun.SceneReference;
using System;
using System.Linq;
using Unity.Cinemachine;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour, IDataPersistence {
    private string CurrentLevel;
    private string LastLevel = "[None]";
    private int enteredFromDirection = 0;
    private LevelTransition currentTransition;

    [SerializeField] private PlayerMovement playerPrefab;
    [SerializeField] private CinemachineCamera cameraPrefab;
    private PlayerMovement Player;
    private CinemachineCamera Camera;

    private Vector3 playerSpawnPoint;
    private Vector3 savedPlayerPosition;
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
            FindPlayerSpawnPoint();
            FindCurrentLevelTransition();
            EnsurePlayerSpawn();
            CameraSetup();
        }
    }

    private void EnsurePlayerSpawn() {
        if (playerSpawnPoint == null) return;


        // Spawn player if it's null and place it at spawn position
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

        QueuePlayerSetup = false;
    }

    private void FindCurrentLevelTransition() {
        currentTransition = FindObjectsByType<LevelTransition>(sortMode: FindObjectsSortMode.None).FirstOrDefault(lt => lt.To.Name == LastLevel && lt.ExitDirection == enteredFromDirection);
        if (currentTransition == null) {
            return;
        }
        currentTransition.isActive = false;
    }

    private void FindPlayerSpawnPoint() {
        if (currentTransition != null) {
            playerSpawnPoint = currentTransition.transform.position + new Vector3(0, 1);
        }
        else if (savedPlayerPosition != Vector3.zero) {
            playerSpawnPoint = savedPlayerPosition;
        }
        else {
            var spawnPoint = GameObject.Find("SpawnPoint");
            if (spawnPoint != null) playerSpawnPoint = spawnPoint.transform.position;
        }
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
        LastLevel = fromScene;
        CurrentLevel = toScene;
        enteredFromDirection = -exitDirection;
        SceneHelper.UnloadScene(fromScene);
        SceneHelper.LoadScene(toScene, true, true);
    }

    #region Data Persistence
    public string Scene => gameObject.scene.name;
    public void LoadData(GameData data) {
        if (CurrentLevel == data.CurrentLevel) return;
        CurrentLevel = data.CurrentLevel;
        savedPlayerPosition = data.PlayerPosition;
        SceneHelper.LoadScene(data.CurrentLevel, true, true);
        Debug.Log($"Loaded player at position {data.PlayerPosition} in scene {data.CurrentLevel}");
    }
    public void SaveData(ref GameData data) {
        data.PlayerPosition = Player.transform.position;
        data.CurrentLevel = CurrentLevel;
    }
    #endregion
}

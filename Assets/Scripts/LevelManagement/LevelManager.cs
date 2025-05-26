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

    private bool PlacePlayer = true;

    void Awake() {
    }


    private void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadMode) {
        FindCurrentLevelTransition();
        CallAfterDelay.Create(0, () => { PlacePlayer = true; });
    }
    private void Start() {
    }

    private void Update() {
        if (PlacePlayer) {
            FindDefaultSpawnPoint();
            FindCurrentLevelTransition();
            EnsurePlayerSpawn();
            CameraSetup();

            PlacePlayer = false;
        }
    }

    private void EnsurePlayerSpawn() {

        if (currentTransition != null) {
            playerSpawnPoint = currentTransition.transform.position + new Vector3(0, 1);
            //Debug.Log("LevelManager EnsurePlayerSpawn: transitioned from other stage. Setting spawnPoint at transition area " + playerSpawnPoint);
        }
        else {
            playerSpawnPoint = defaultSpawnPoint.transform.position;
            //Debug.Log("LevelManager EnsurePlayerSpawn: NOT transitioned from other stage. Setting spawnPoint at default point " + playerSpawnPoint);
        }

        if (Player == null) {
            //Debug.Log("LevelManager EnsurePlayerSpawn: Player is null. Instantiating player at spawnPoint" + playerSpawnPoint);
            Player = Instantiate(playerPrefab, playerSpawnPoint, Quaternion.identity);
        }
        else {
            //Debug.Log("LevelManager EnsurePlayerSpawn: Player already exists. Placing player at spawnPoint" + playerSpawnPoint);
            Player.transform.position = playerSpawnPoint;
        }
        if (currentTransition != null) {
            //Debug.Log("Entered from other stage. Playing move sequence");
            Player.EnterStage(-currentTransition.ExitDirection);
        }
    }

    private void FindCurrentLevelTransition() {
        //Debug.Log($"Finding transition to {lastSceneName} with exit direction {enteredFromDirection}");
        currentTransition = FindObjectsByType<LevelTransition>(sortMode: FindObjectsSortMode.None).FirstOrDefault(lt => lt.To.Name == lastSceneName && lt.ExitDirection == enteredFromDirection);
        if (currentTransition == null) {
            //Debug.Log("FindCurrentLevelTransition: No transition from other stage found");
            return;
        }
        currentTransition.isActive = false;
    }

    private void FindDefaultSpawnPoint() {
        defaultSpawnPoint = GameObject.Find("SpawnPoint");
        //if (defaultSpawnPoint == null) Debug.Log("LevelManager FindSpawnPoint: default spawn point NOT found");
        //else Debug.Log("LevelManager FindSpawnPoint: default spawn point found " + defaultSpawnPoint.transform.position);
    }

    private void CameraSetup() {
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

        //Debug.Log($"Exiting scene {fromScene} through {exitDirection}. Entering {toScene} from {-exitDirection}");
    }
}

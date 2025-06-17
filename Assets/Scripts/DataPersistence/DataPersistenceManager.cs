using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;


class DataPersistenceManager : MonoBehaviour {
    [SerializeField] private string SaveFileName;

    private GameData GameData;
    private FileDataHandler dataHandler;
    private Dictionary<string, List<IDataPersistence>> PersistentDataObjects = new();

    private void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }
    private void Awake() {
        dataHandler = new FileDataHandler(SaveFileName);
        GameData = dataHandler.Load();
    }

    private void Start() {
    }

    private void OnSceneUnloaded(Scene scene) {
        SaveSceneState(scene.name);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode) {
        CallAfterDelay.Create(0, () => {
            FindDataPersistentObjectsInScene(scene.name);
            LoadSceneState(scene.name);
        });
    }

    private void SaveSceneState(string scene) {
        foreach (IDataPersistence dataPersistentObject in PersistentDataObjects[scene]) {
            dataPersistentObject.SaveData(ref GameData);
        }
    }

    public void NewGame() {
        GameData = new GameData();
    }

    public void LoadSceneState(string scene) {
        if (GameData == null) NewGame();
        foreach(IDataPersistence dataPersistentObject in PersistentDataObjects[scene]) {
            dataPersistentObject.LoadData(GameData);
        }
    }

    public void SaveGame() {
        dataHandler.Save(GameData);
        Debug.Log($"Saved player position at {GameData.PlayerPosition}in scene {GameData.CurrentLevel}");
    }

    private void FindDataPersistentObjectsInScene(string scene) {
        var dataPersistentObjectsInScene = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).Where(go => go.gameObject.scene.name == scene).OfType<IDataPersistence>().ToList();
        PersistentDataObjects[scene] = dataPersistentObjectsInScene;
        Debug.Log($"Found {dataPersistentObjectsInScene.Count} persistent data objects in scene {scene}");
    }

    private void Update() {

    }

    private void OnApplicationQuit() {
        for (int i = 0; i < SceneManager.sceneCount; i++) {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene.isLoaded && PersistentDataObjects.ContainsKey(scene.name)) {
                SaveSceneState(scene.name);
            }
        }
        SaveGame();
    }
}

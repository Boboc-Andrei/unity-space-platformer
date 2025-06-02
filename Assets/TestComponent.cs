using UnityEngine;

public class TestComponent : MonoBehaviour, IDataPersistence {
    bool isActive = true;

    public string Scene => gameObject.scene.name;
    public void LoadData(GameData data) {

    }

    public void SaveData(ref GameData data) {
        
    }

    private void Start() {
        gameObject.SetActive(isActive);
    }
}

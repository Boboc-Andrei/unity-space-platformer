using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PersistentSingleton<T> : MonoBehaviour where T : Component {
    private static T _instance;
    public static T Instance {
        get {
            if(_instance == null) {
                Instance = FindAnyObjectByType<T>();
                if(Instance == null) {
                    GameObject obj = new GameObject();
                    obj.name = typeof(T).Name + "AutoCreated";
                    _instance = obj.AddComponent<T>();
                }
            }
            return _instance;
        }
        private set { }
    }

    protected virtual void Awake() => SingletonInitialization();

    private void SingletonInitialization() {
        if (_instance == null) {
            _instance = this as T;
            DontDestroyOnLoad(transform.gameObject);
        }
        else {
            if (this != Instance) {
                Destroy(this.gameObject);
            }
        }
        DontDestroyOnLoad(gameObject);
    }
}

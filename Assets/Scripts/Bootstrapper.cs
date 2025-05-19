using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Bootstrapper : PersistentSingleton<Bootstrapper> {
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static async Task Init() {
        await SceneManager.LoadSceneAsync("Bootstrapper", LoadSceneMode.Additive);
    }
}


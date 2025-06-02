using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Bootstrapper {
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Init() {
        SceneHelper.LoadScene("Bootstrapper");
        //SceneHelper.LoadScene("TestScene1", true, true);
    }
}


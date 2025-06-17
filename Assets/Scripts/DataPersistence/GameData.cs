using SuperTiled2Unity.Editor.LibTessDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[System.Serializable]
public class GameData {
    public bool IsNew = true;
    public Vector3 PlayerPosition = Vector3.zero;
    public string CurrentLevel = "TestScene1";

    public GameData() {
    }
}

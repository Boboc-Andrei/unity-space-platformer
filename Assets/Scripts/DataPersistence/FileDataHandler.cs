using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


class FileDataHandler {
    private string dataDirPath = "";
    private string dataFileName = "";

    public FileDataHandler(string dataFileName) {
        this.dataDirPath = Application.persistentDataPath;
        this.dataFileName = dataFileName;
    }

    public GameData Load() {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        GameData loadedData = null;
        if (File.Exists(fullPath)) {
            try {
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open)) {
                    using (StreamReader reader = new StreamReader(stream)) {
                        dataToLoad = reader.ReadToEnd();
                    }
                }
                Debug.Log($"Loaded game from {fullPath}");
                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception e) {
                Debug.LogError($"An error occured while reading file at {fullPath}: {e.Message}");
            }
        }
        return loadedData;
    }

    public void Save(GameData data) {
        string fullPath = Path.Combine(dataDirPath, dataFileName);

        try {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            string dataToStore = JsonUtility.ToJson(data, true);
            using (FileStream stream = new FileStream(fullPath, FileMode.Create)) {
                using (StreamWriter writer = new StreamWriter(stream)) {
                    writer.Write(dataToStore);
                }
            }
            Debug.Log($"Saved game at {fullPath}");
        }
        catch (Exception e) {
            Debug.LogError($"An error occured while saving file at {fullPath}: {e.Message}");
        }
    }
}

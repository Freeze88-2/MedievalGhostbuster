using UnityEngine;
using System.IO;

public class SaveSystem : MonoBehaviour
{
    private const string FILENAME = "ghostbusterSave.dat";    
    private const KeyCode SAVE_KEY = KeyCode.F3;   
    private const KeyCode LOAD_KEY = KeyCode.F4;
    public MovementController movementController;
    public CameraController cameraController;

    private string _filePath;

    [System.Serializable] 
    private struct SaveData
    {
        public PlayerSaveData playerSaveData;
    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        _filePath = Application.persistentDataPath + "/" + FILENAME;
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if(Input.GetKeyDown(SAVE_KEY))
            SaveGame();
        else if (Input.GetKeyDown(LOAD_KEY))
            LoadGame();
    }   

    private void SaveGame()
    {
        SaveData saveData = CreateSaveData();

        StoreSaveData(saveData);
    }

    private SaveData CreateSaveData()
    {
        SaveData saveData;

        saveData.playerSaveData = movementController.CreateSaveData();

        return saveData;
    }

    private void StoreSaveData(SaveData saveData)
    {
        string jsonSaveData = JsonUtility.ToJson(saveData, true);

        File.WriteAllText(_filePath, jsonSaveData);

        print(_filePath);

        print("Game saved successfully!");
    }

    private void LoadGame()
    {
        if (File.Exists(_filePath))
        {
            SaveData saveData = LoadSaveData();

            ProcessSaveData(saveData);
        }
        if (!File.Exists(_filePath))
        {
            print("There is nothing to load...");
        }
    }

    private SaveData LoadSaveData()
    {
        string jsonSaveData = File.ReadAllText(_filePath);

        SaveData saveData = JsonUtility.FromJson<SaveData>(jsonSaveData);

        return saveData;
    }

    private void ProcessSaveData(SaveData saveData)
    {
        movementController.ProcessSaveData(saveData.playerSaveData);

        print("Game loaded successfully!");
    }
}

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveController : MonoBehaviour
{
    [SerializeField] private Saveable[] _objectsToSave = default;

    private const KeyCode SAVE_KEY = KeyCode.F3;
    private const KeyCode LOAD_KEY = KeyCode.F5;
    private const string FILENAME = "ghostbusterSave.dat";
    private const string WORLD_FILENAME = "ghostbusterWorldSave.dat";
    private string _playerFilepath;
    private string _worldFilepath;
    private MovementController movementController;

    private void Awake()
    {
        movementController = GameObject.FindObjectOfType<MovementController>();
        _playerFilepath = Application.persistentDataPath + "/" + FILENAME;
        _worldFilepath = Application.persistentDataPath + "/" + WORLD_FILENAME;
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        ListenInputs();
    }

    public void UpdateUnsetIds()
    {
        for (int i = 0; i < _objectsToSave.Length; i++)
        {
            if (_objectsToSave[i].ID == default)
            {
                _objectsToSave[i].ID = GetNewId();
                Debug.LogWarning("Assigning new ID of " +
                    _objectsToSave[i].name +
                    " to " +
                    _objectsToSave[i].ID);
            }
        }
    }

    public void GetAllSaveables()
    {
        _objectsToSave = GameObject.FindObjectsOfType<Saveable>();
    }

    private int GetNewId()
    {
        int newId = Random.Range(int.MinValue, int.MaxValue);

        for (int i = 0; i < _objectsToSave.Length; i++)
            if (_objectsToSave[i].ID == newId)
                return GetNewId();

        return newId;
    }

    private void ListenInputs()
    {
        if (Input.GetKeyDown(SAVE_KEY))
            SaveGame();
        else if (Input.GetKeyDown(LOAD_KEY))
            LoadGame();
    }

    //!-----------------------------------------------------------------------
    //!-----------------------------------------------------------------------
    //!-----------------------------------------------------------------------
    //!-----------------------------------------------------------------------
    //!-----------------------------------------------------------------------
    //!-----------------------------------------------------------------------

    // Save start
    private void SaveGame()
    {
        (SaveData plr, WorldData wrld) save = GetSaveData();

        StoreSaveData(save.plr, save.wrld);
    }

    private(SaveData, WorldData) GetSaveData()
    {
        SaveData saveData;
        WorldData worldData;

        saveData = movementController.CreateSaveData();
        worldData = new WorldData(_objectsToSave);

        return (saveData, worldData);
    }

    private void StoreSaveData(SaveData saveData, WorldData worldData)
    {
        string jsonPlayerSaveData = JsonUtility.ToJson(saveData, true);
        string jsonWorldSaveData = JsonUtility.ToJson(worldData, true);

        File.WriteAllText(_playerFilepath, jsonPlayerSaveData);
        File.WriteAllText(_worldFilepath, jsonWorldSaveData);

        print(_playerFilepath);
        print(_worldFilepath);

        print("Game saved successfully!");
    }
    // Save end

    // Load start
    private void LoadGame()
    {
        if (File.Exists(_playerFilepath) && File.Exists(_worldFilepath))
        {
            SaveData playerSaveData;
            WorldData worldSaveData;

            playerSaveData = LoadPlayerSaveData();
            worldSaveData = LoadWorldSaveData();

            ProcessSaveData(playerSaveData, worldSaveData);
        }
        else
        {
            print("There is nothing to load...");
        }
    }

    private SaveData LoadPlayerSaveData()
    {
        string jsonSaveData = File.ReadAllText(_playerFilepath);

        SaveData saveData = JsonUtility.FromJson<SaveData>(jsonSaveData);

        return saveData;
    }
    private WorldData LoadWorldSaveData()
    {
        string jsonSaveData = File.ReadAllText(_worldFilepath);

        WorldData worldData = JsonUtility.FromJson<WorldData>(jsonSaveData);

        return worldData;
    }

    private void ProcessSaveData(SaveData playerData, WorldData worldData)
    {
        movementController.ProcessSaveData(playerData);
        ProcessWorldData(worldData);

        print("Game loaded successfully!");
    }

    private void ProcessWorldData(WorldData worldData)
    {
        List<Saveable> saves = new List<Saveable>(_objectsToSave);
        for (int i = 0; i < worldData.saveDatas.Length; i++)
        {
            foreach (Saveable s in _objectsToSave)
            {
                if (s.ID == worldData.saveDatas[i].id)
                {
                    LoadSaveable(s, worldData.saveDatas[i]);
                    saves.Remove(s);
                    break;
                }
            }
        }
    }

    private void LoadSaveable(Saveable saveable, SaveData data)
    {
        saveable.transform.position = data.position;
        saveable.transform.rotation = data.rotation;
    }
    // Load end
}
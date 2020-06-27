using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveController : MonoBehaviour
{
    [SerializeField] private Saveable[] _objectsToSave = default;

    private const KeyCode SAVE_KEY = KeyCode.F3;
    private const KeyCode LOAD_KEY = KeyCode.F5;
    private const string PLAYER_LOCATION_FILENAME = "ghostbusterSave.dat";
    private const string WORLD_FILENAME = "ghostbusterWorldSave.dat";
    private const string HEALTH_FILENAME = "ghostbusterHealthSave.dat";
    private string _playerLocationFilepath;
    private string _worldFilepath;
    private string _healthFilepath;
    private MovementController _movementController;
    private DummyPlayer _dummy;
    private MenuButton _menuMain;

    private void Awake()
    {
        _movementController = GameObject.FindObjectOfType<MovementController>();
        _dummy = GameObject.FindObjectOfType<DummyPlayer>();
        _playerLocationFilepath = Application.persistentDataPath + "/" + PLAYER_LOCATION_FILENAME;
        _worldFilepath = Application.persistentDataPath + "/" + WORLD_FILENAME;
        _healthFilepath = Application.persistentDataPath + "/" + HEALTH_FILENAME;

        _menuMain = FindObjectOfType<MenuButton>();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if(_menuMain != null && _menuMain.gameIsLoaded == true)
        {
            LoadGame();
            Destroy(_menuMain);
        }
        else if (!File.Exists(_playerLocationFilepath) || !File.Exists(_worldFilepath) || !File.Exists(_healthFilepath))
            SaveGame();
        else
            print("There are previous save files");

        Debug.Log("####" + _menuMain.gameIsLoaded);


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
    public void SaveGame()
    {
        (SaveData plr, WorldData wrld, HealthData hlth) save = GetSaveData();

        StoreSaveData(save.plr, save.wrld, save.hlth);
    }

    private(SaveData, WorldData, HealthData) GetSaveData()
    {
        SaveData saveData;
        WorldData worldData;
        HealthData healthData;

        saveData = _movementController.CreatePlayerSaveData();
        worldData = new WorldData(_objectsToSave);
        healthData = _dummy.CreateHealthSaveData();

        return (saveData, worldData, healthData);
    }

    private void StoreSaveData(SaveData saveData, WorldData worldData, HealthData healthData)
    {
        string jsonPlayerSaveData = JsonUtility.ToJson(saveData, true);
        string jsonWorldSaveData = JsonUtility.ToJson(worldData, true);
        string jsonHealthSaveData = JsonUtility.ToJson(healthData, true);

        File.WriteAllText(_playerLocationFilepath, jsonPlayerSaveData);
        File.WriteAllText(_worldFilepath, jsonWorldSaveData);
        File.WriteAllText(_healthFilepath, jsonHealthSaveData);

        print(_playerLocationFilepath);
        print(_worldFilepath);
        print(_healthFilepath);

        print("Game saved successfully!");
    }
    // Save end

    // Load start
    public void LoadGame()
    {
        if (File.Exists(_playerLocationFilepath) && File.Exists(_worldFilepath) && File.Exists(_healthFilepath))
        {
            SaveData playerSaveData;
            WorldData worldSaveData;
            HealthData healthSaveData;

            playerSaveData = LoadPlayerSaveData();
            worldSaveData = LoadWorldSaveData();
            healthSaveData = LoadHealthSaveData();

            ProcessSaveData(playerSaveData, worldSaveData, healthSaveData);
        }
        else
        {
            print("There is nothing to load...");
        }
    }

    private SaveData LoadPlayerSaveData()
    {
        string jsonSaveData = File.ReadAllText(_playerLocationFilepath);

        SaveData saveData = JsonUtility.FromJson<SaveData>(jsonSaveData);

        return saveData;
    }
    private WorldData LoadWorldSaveData()
    {
        string jsonSaveData = File.ReadAllText(_worldFilepath);

        WorldData worldData = JsonUtility.FromJson<WorldData>(jsonSaveData);

        return worldData;
    }

    private HealthData LoadHealthSaveData()
    {
        string jsonSaveData = File.ReadAllText(_healthFilepath);

        HealthData healthData = JsonUtility.FromJson<HealthData>(jsonSaveData);

        return healthData;
    }

    private void ProcessSaveData(SaveData playerData, WorldData worldData, HealthData healthData)
    {
        _movementController.ProcessPlayerSaveData(playerData);
        ProcessWorldData(worldData);
        _dummy.ProcessHealthSaveData(healthData);

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
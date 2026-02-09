using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEditor.Overlays;
using UnityEngine;

public class SaveController : MonoBehaviour
{
    private string saveLocation;
    private InventoryController inventoryController;
    private HotbarController hotbarController;
    private Chest[] chests;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InitializeComponents();
        LoadGame();
    }

    private void InitializeComponents()
    {
        saveLocation = Path.Combine(Application.persistentDataPath, "saveData.json");
        inventoryController = FindFirstObjectByType<InventoryController>();
        hotbarController = FindFirstObjectByType<HotbarController>();
        chests = FindObjectsByType<Chest>(FindObjectsInactive.Include, FindObjectsSortMode.None);
    }
    public void SaveGame()
    {
        SaveData saveData = new SaveData()
        {
            playerPosition = GameObject.FindWithTag("Player").transform.position,
            mapBoundary = FindFirstObjectByType<CinemachineConfiner2D>().BoundingShape2D.gameObject.name,
            inventorySaveData = inventoryController.GetInventoryItems(),
            hotbarSaveData = hotbarController.GetHotbarItems(),
            chestSaveData = GetChestState()
        };
        File.WriteAllText(saveLocation, JsonUtility.ToJson(saveData));
    }

    private List<ChestSaveData> GetChestState()
    {
        List<ChestSaveData> chestStates = new List<ChestSaveData>();
        foreach (Chest chest in chests)
        {
            ChestSaveData chestSaveDate = new ChestSaveData
            {
                chestID = chest.ChestID,
                isOpened = chest.IsOpened
            };
            chestStates.Add(chestSaveDate);
        }
        return chestStates;
    }
    public void LoadGame()
    {
        if (File.Exists(saveLocation))
        {
            SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(saveLocation));

            GameObject.FindWithTag("Player").transform.position = saveData.playerPosition;
            PolygonCollider2D savedMapBound = GameObject.Find(saveData.mapBoundary).GetComponent<PolygonCollider2D>();
            FindFirstObjectByType<CinemachineConfiner2D>().BoundingShape2D = savedMapBound;
            MapController_Manual.Instance?.HighlightArea(saveData.mapBoundary);
            MapController_Dynamic.Instance?.GenerateMap(savedMapBound);
            inventoryController.SetInventoryItems(saveData.inventorySaveData);
            hotbarController.SetHotbarItems(saveData.hotbarSaveData);
            LoadChestStates(saveData.chestSaveData);
        }
        else
        {
            SaveGame();
            inventoryController.SetInventoryItems(new List<InventorySaveData>());
            hotbarController.SetHotbarItems(new List<InventorySaveData>());
            MapController_Dynamic.Instance?.GenerateMap();
        }
    }

    private void LoadChestStates(List<ChestSaveData> chestStates)
    {
        foreach (Chest chest in chests)
        {
            ChestSaveData chestSaveData = chestStates.FirstOrDefault(c => c.chestID == chest.ChestID);
            if (chestSaveData != null)
            {
                chest.SetOpened(chestSaveData.isOpened);
            }
        }
    }
}

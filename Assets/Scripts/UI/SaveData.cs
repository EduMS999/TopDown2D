using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    // La posición del player
    public Vector3 playerPosition;
    // El nombre de la zona dentro del mapa: T1, F1, ...
    public string mapBoundary;
    public List<InventorySaveData> inventorySaveData;
    public List<InventorySaveData> hotbarSaveData;
}

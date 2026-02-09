using System.Collections.Generic;
using UnityEngine;

// El atributo [System.Serializable] permite que esta clase se convierta en datos (como JSON o binario)
// para poder guardarse en un archivo o verse en el Inspector de Unity.
[System.Serializable]
public class SaveData
{
    // Almacena las coordenadas X, Y, Z del jugador para que al cargar aparezca donde dejó la partida.
    public Vector3 playerPosition;

    // Guarda el nombre de la zona actual (ej: "Sala de Boss", "T1") para el sistema de mapa dinámico.
    public string mapBoundary;

    // Lista que guarda los objetos que el jugador tiene en su inventario principal.
    public List<InventorySaveData> inventorySaveData;

    // Lista que guarda los objetos colocados en la barra de acceso rápido (hotbar).
    public List<InventorySaveData> hotbarSaveData;

    // Lista de objetos tipo 'ChestSaveData' para recordar qué cofres han sido abiertos.
    public List<ChestSaveData> chestSaveData;
}

// Esta clase representa la información mínima necesaria para identificar y recordar el estado de un cofre.
[System.Serializable]
public class ChestSaveData
{
    // El identificador único (generado por GlobalHelper) que vincula estos datos con un cofre real en el juego.
    public string chestID;

    // Indica si el cofre estaba abierto (true) o cerrado (false) al momento de guardar.
    public bool isOpened;
}

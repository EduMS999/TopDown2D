using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject menuCanvas;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        menuCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // Edit > Poject Settins > Player > Other Settings > Active Input Handling
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!menuCanvas.activeSelf && PauseController.IsGamePaused)
            {
                return; // Si estamos pausados por otra razón, no queremos alterar esta lógica
            }
            menuCanvas.SetActive(!menuCanvas.activeSelf);
            PauseController.SetPause(menuCanvas.activeSelf);
        }

    }
}

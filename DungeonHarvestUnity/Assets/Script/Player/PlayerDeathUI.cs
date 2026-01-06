using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerDeathUI : MonoBehaviour
{
    public GameObject canvas;
    public Transform player;
    public Vector3 respawnPosition;
    public Button respawn;
    public Button mainMenu;
    private PauseMenu pause;

void Start()
{
    canvas.SetActive(false);
    respawn.onClick.AddListener(Respawn);
    mainMenu.onClick.AddListener(MainMenu);
}


    public void Show()
    {
        Time.timeScale = 0f;
        canvas.SetActive(true);
    }

    public void Respawn()
    {
        Time.timeScale = 1f;
        canvas.SetActive(false);

        player.position = respawnPosition;

        HealthStaminaManager health = player.GetComponent<HealthStaminaManager
        >();
        health.ResetHealth();
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}

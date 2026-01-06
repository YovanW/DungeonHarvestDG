using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerDeathUI : MonoBehaviour
{
    public GameObject canvas;
    public Transform player;
    private Vector3 respawnPosition;
    public Button respawn;
    public Button mainMenu;


    void Start()
    {
        canvas.SetActive(false);
        respawn.onClick.AddListener(Respawn);
        mainMenu.onClick.AddListener(MainMenu);

        // get player spawn coord
        respawnPosition = player.position;
        Debug.Log(respawnPosition);
    }


    public void Show()
    {
        canvas.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Respawn()
    {
        canvas.SetActive(false);

        CharacterController cc = player.GetComponent<CharacterController>();
        if (cc != null)
            cc.enabled = false;

        player.position = respawnPosition;

        FirstPersonController fps = player.GetComponent<FirstPersonController>();
        if (fps != null)
        {
            fps.ResetLook();
        }


        if (cc != null)
            cc.enabled = true;

        HealthStaminaManager health = player.GetComponent<HealthStaminaManager>();
        health.ResetHealth();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void MainMenu()
    {
        GetComponent<PauseMenu>().saveAndExit();
    }
}

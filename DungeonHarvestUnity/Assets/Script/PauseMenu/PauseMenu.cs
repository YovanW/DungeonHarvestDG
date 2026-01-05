using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject settingCanvas;
    private bool isOpen = false;
    public InventoryManager inventoryManager;


    public Button continueBtn;
    public Button settingBtn;
    public Button saveExitBtn;
    public Button backBtn;

    void Start()
    {
        inventoryManager = GameObject.FindGameObjectWithTag("InventoryManager").GetComponent<InventoryManager>();

        pauseMenu.SetActive(false);
        settingCanvas.SetActive(false);

        continueBtn.onClick.AddListener(resume);
        settingBtn.onClick.AddListener(setting);
        saveExitBtn.onClick.AddListener(saveAndExit);
        backBtn.onClick.AddListener(back);
    }

    public void resume()
    {
        isOpen = false;
        pauseMenu.SetActive(false);
        // settingCanvas.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;

        // force the button to return to normal state
        EventSystem.current.SetSelectedGameObject(null);

    }

    public void setting()
    {
        pauseMenu.SetActive(false);
        settingCanvas.SetActive(true);


        // force the button to return to normal state
        EventSystem.current.SetSelectedGameObject(null);
    }
    public void saveAndExit()
    {
        // TODO: implement save functionality
        inventoryManager.SaveInventory();
        PlayerPrefs.SetInt("SavedGame", 1);
        PlayerPrefs.Save();

        SceneManager.LoadScene("Main Menu");
    }

    public void back()
    {
        settingCanvas.SetActive(false);
        pauseMenu.SetActive(true);
    }

    public void pause()
    {
        isOpen = true;
        pauseMenu.SetActive(true);
        settingCanvas.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isOpen && settingCanvas.activeSelf == false) { resume(); }
            else if (settingCanvas.activeSelf == true) { back(); }
            else { pause(); }
        }
    }

}

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject settingCanvas;
    private bool isOpen = false;


    public Button continueBtn;
    public Button settingBtn;
    public Button saveExitBtn;
    public Button backBtn;

    void Start()
    {
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
        Time.timeScale = 1f;

        // force the button to return to normal state
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void setting()
    {
        pauseMenu.SetActive(false);
        settingCanvas.SetActive(true);

        Time.timeScale = 0f;

        // force the button to return to normal state
        EventSystem.current.SetSelectedGameObject(null);
    }
    public void saveAndExit()
    {
        // TODO: implement save functionality


        SceneManager.LoadScene("Main Menu");
    }

    public void back()
    {
        settingCanvas.SetActive(false);
        pauseMenu.SetActive(true);
    }

    void Update()
    {
        if (settingCanvas.activeSelf == false)
        {
            Time.timeScale = 1f;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isOpen && settingCanvas.activeSelf == false) { resume(); }
            else if (isOpen && settingCanvas.activeSelf == true) { back(); }
            else
            {
                isOpen = true;
                pauseMenu.SetActive(true);
                settingCanvas.SetActive(false);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }
}

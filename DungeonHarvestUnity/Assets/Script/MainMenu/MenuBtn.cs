using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;


public class MenuBtn : MonoBehaviour
{

    public GameObject mainCanvas;
    public GameObject playCanvas;
    public GameObject settingCanvas;

    public Button playBtn;
    public Button settingBtn;
    public Button newGameBtn;

    public Button continueBtn;
    public Button normalModeBtn;
    public Button hardModeBtn;
    public Button backBtn;
    public Button settingBackBtn;

    private string dataToLoad;

    void Start()
    {
        // Assign button listeners
        playBtn.onClick.AddListener(play);
        settingBtn.onClick.AddListener(setting);
        continueBtn.onClick.AddListener(continueGame);
        newGameBtn.onClick.AddListener(newGame);
        backBtn.onClick.AddListener(back);
        settingBackBtn.onClick.AddListener(back);


        // Disable Continue button if no save data
        if (!PlayerPrefs.HasKey("SavedGame"))
        {
            continueBtn.interactable = false;
        }

        // //FIXME: test change scene
        continueBtn.interactable = true;

        // Hide other canvases
        playCanvas.SetActive(false);
        settingCanvas.SetActive(false);

        // Hide mode buttons at start
        normalModeBtn.gameObject.SetActive(false);
        hardModeBtn.gameObject.SetActive(false);
    }

    void Update()
    {
        // Esc key to go back to main menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (playCanvas.activeSelf || settingCanvas.activeSelf)
            {
                back();
            }
        }
    }

    // Back to Main Menu
    public void back()
    {
        playCanvas.SetActive(false);
        settingCanvas.SetActive(false);
        mainCanvas.SetActive(true);
    }

    // Main Menu
    public void play()
    {
        mainCanvas.SetActive(false);
        playCanvas.SetActive(true);
    }

    public void setting()
    {
        mainCanvas.SetActive(false);
        settingCanvas.SetActive(true);
    }

    public void quit()
    {
        Application.Quit();
        Debug.Log("Game is exiting");
    }

    // Play Menu
    public void continueGame()
    {
        Debug.Log("Continue Game");

        //TODO: Load saved game data
        if (PlayerPrefs.HasKey("SavedGame"))
        {
            // dataToLoad = PlayerPrefs.GetString("SavedGame");

            // // Load the saved game scene
            // SceneManager.LoadScene("Game");
        }

        // FIXME: test scene load
        // SceneManager.LoadScene("nathanTest");
        SceneManager.LoadScene("Demo");
    }

    public void newGame()
    {
        bool isActive = normalModeBtn.gameObject.activeSelf;

        // toggle both buttons
        normalModeBtn.gameObject.SetActive(!isActive);
        hardModeBtn.gameObject.SetActive(!isActive);

        // force the button to return to normal state
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void normalMode()
    {
        Debug.Log("Normal Mode Selected");
        //TODO: create new game in normal mode

    }

    public void hardMode()
    {
        Debug.Log("Hard Mode Selected");
        //TODO: create new game in hard mode

    }

}

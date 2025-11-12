using UnityEngine;

public class mainMenuBtn : MonoBehaviour
{
    public void play()
    {
        // SceneManager.LoadScene("Game");
    }

    
    public void quit()
    {
        Application.Quit();
        Debug.Log("Game is exiting");
    }
}

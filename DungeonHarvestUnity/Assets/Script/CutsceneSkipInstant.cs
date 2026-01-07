using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

public class CutsceneSkipInstant : MonoBehaviour
{
    public PlayableDirector director;
    public string gameSceneName;

    public void Skip()
    {
        director.Stop();
        SceneManager.LoadScene(gameSceneName);
    }
}

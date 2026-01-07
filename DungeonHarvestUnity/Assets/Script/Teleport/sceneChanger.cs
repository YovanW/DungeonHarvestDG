using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneChanger : MonoBehaviour
{
    public string sceneName;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            spawnPointFinder.isTeleporting = true;
            SceneManager.LoadScene(sceneName);
        }        
    }
}

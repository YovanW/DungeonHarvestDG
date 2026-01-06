using UnityEngine;
using UnityEngine.SceneManagement;

public class spawnPointFinder : MonoBehaviour
{
    [SerializeField] private Transform player;
    private GameObject spawnPoint;
    public static bool isTeleporting = false;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += SceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= SceneLoaded;
    }

    private void SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!isTeleporting)
            return;

        spawnPoint = GameObject.FindWithTag("SpawnPoint");

        if (spawnPoint != null && player != null)
        {
            player.SetPositionAndRotation(
                spawnPoint.transform.position,
                spawnPoint.transform.rotation
            );
            Physics.SyncTransforms();
        }

        isTeleporting = false;
    }

}

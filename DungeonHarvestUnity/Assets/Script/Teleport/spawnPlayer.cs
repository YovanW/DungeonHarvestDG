using UnityEngine;

public class spawnPlayer : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform spawnPoint;

    private static bool spawned = false;

    private void Awake()
    {
        if (spawned)
            return;

        GameObject player = Instantiate(
            playerPrefab,
            spawnPoint.position,
            spawnPoint.rotation
        );

        spawned = true;
    }
}

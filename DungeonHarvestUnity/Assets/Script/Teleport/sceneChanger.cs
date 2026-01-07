using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneChanger : MonoBehaviour
{
    public string sceneName;
    private InventoryManager inventoryManager;

    void Start()
    {
        inventoryManager = GameObject.FindGameObjectWithTag("InventoryManager").GetComponent<InventoryManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            // spawnPointFinder.isTeleporting = true;
            // Save player inventory
            inventoryManager.SaveInventory();

            // Mark save and commit PlayerPrefs
            PlayerPrefs.SetInt("SavedGame", 1);
            PlayerPrefs.Save();
            SceneManager.LoadScene(sceneName);
        }        
    }
}

using UnityEngine;

public class TreeController : MonoBehaviour
{
    public GameObject treeModel;
    public GameObject choppedTreeModel;
    public GameObject hitbox;
    public ItemSO drop;

    private float health = 100f;
    float respawnTime = 30f;
    public bool isChopped = false;

    void Start()
    {
        if (choppedTreeModel == null) { Debug.LogWarning("choppedTreeModel reference is missing in TreeController"); }
        if (treeModel == null) { Debug.LogWarning("treeModel reference is missing in TreeController"); }
        if (hitbox == null) { Debug.LogWarning("hitbox reference is missing in TreeController"); }
        if (drop == null) { Debug.LogWarning("drop ItemSO reference is missing in TreeController"); }
    }

    void Update()
    {
        UpdateModel();
    }

    void UpdateModel()
    {
        if (isChopped)
        {
            treeModel.SetActive(false);
            choppedTreeModel.SetActive(true);
            isChopped = false;
        }
        else
        {
            treeModel.SetActive(true);
            choppedTreeModel.SetActive(false);
        }
    }

    
}

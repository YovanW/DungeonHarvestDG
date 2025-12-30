using UnityEngine;

public class soilHitBox : MonoBehaviour
{
    public GameObject plantArea;
    void Start()
    {
        // hide the plant area
        if (plantArea == null) { Debug.LogWarning("Plant Area is not assigned in the inspector."); }
        plantArea.SetActive(false);


    }

    // Update is called once per frame
    void Update()
    {

    }
}

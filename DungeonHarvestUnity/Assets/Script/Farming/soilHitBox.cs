using UnityEngine;

public class soilHitBox : MonoBehaviour
{
    public GameObject plantArea;
    public bool isRaked = false;
    public int soilQuality = 0;
    void Start()
    {
        if (plantArea == null) { Debug.LogWarning("Plant Area is not assigned in the inspector."); }

        if (isRaked)
            plantArea.SetActive(true);         // show the plant area
        else
            plantArea.SetActive(false);        // hide the plant area
    }

    // Update is called once per frame
    void Update()
    {

    }
}

using UnityEngine;
using UnityEngine.UI;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public ItemSO itemScriptableObject;
    [SerializeField] Image iconImage;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // This is where you would get the second error if the 'using' was missing
        iconImage.sprite = itemScriptableObject.icon;
    }
}
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    [Header("Crosshair Settings")]
    public Texture2D crosshairTexture;
    public Vector2 crosshairSize = new Vector2(20, 20);
    public Color crosshairColor = Color.white;
    
    private Vector2 screenCenter;
    
    void Start()
    {
        // Calculate screen center once
        screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
    }
    
    void OnGUI()
    {
        if (crosshairTexture != null)
        {
            // Save original GUI color
            Color originalColor = GUI.color;
            
            // Set crosshair color
            GUI.color = crosshairColor;
            
            // Draw crosshair at screen center
            Rect crosshairRect = new Rect(
                screenCenter.x - crosshairSize.x / 2,
                screenCenter.y - crosshairSize.y / 2,
                crosshairSize.x,
                crosshairSize.y
            );
            
            GUI.DrawTexture(crosshairRect, crosshairTexture);
            
            // Restore original GUI color
            GUI.color = originalColor;
        }
    }
    
    void Update()
    {
        // Update screen center if resolution changes (rare, but good practice)
        screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
    }
}
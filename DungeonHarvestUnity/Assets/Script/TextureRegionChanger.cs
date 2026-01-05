using UnityEngine;

// Attach this to your prefab
public class ChangeTextureRegionRed : MonoBehaviour
{
    [Header("Surface Inputs Tiling & Offset")]
    public Vector2 tiling = new Vector2(1, 1);
    public Vector2 offset = new Vector2(0, 0);
    
    [Header("Color Settings")]
    public Color baseColor = Color.white;
    public bool applyRedTint = false;
    public float redIntensity = 1.0f;
    
    void Start()
    {
        ApplySurfaceChanges();
    }
    
    void ApplySurfaceChanges()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer == null) return;
        
        MaterialPropertyBlock propBlock = new MaterialPropertyBlock();
        
        // Get current properties
        renderer.GetPropertyBlock(propBlock);
        
        // Set Base Map tiling and offset
        propBlock.SetVector("_BaseMap_ST", new Vector4(tiling.x, tiling.y, offset.x, offset.y));
        
        // Apply the same tiling/offset to other surface inputs
        propBlock.SetVector("_MetallicMap_ST", new Vector4(tiling.x, tiling.y, offset.x, offset.y));
        propBlock.SetVector("_BumpMap_ST", new Vector4(tiling.x, tiling.y, offset.x, offset.y)); // Normal Map
        propBlock.SetVector("_ParallaxMap_ST", new Vector4(tiling.x, tiling.y, offset.x, offset.y)); // Height Map
        propBlock.SetVector("_OcclusionMap_ST", new Vector4(tiling.x, tiling.y, offset.x, offset.y));
        
        // Apply red color tint
        if (applyRedTint)
        {
            Color redTint = new Color(redIntensity, baseColor.g * (1f - redIntensity), baseColor.b * (1f - redIntensity), baseColor.a);
            propBlock.SetColor("_BaseColor", redTint);
            propBlock.SetColor("_Color", redTint);
        }
        else
        {
            propBlock.SetColor("_BaseColor", baseColor);
            propBlock.SetColor("_Color", baseColor);
        }
        
        // Apply changes
        renderer.SetPropertyBlock(propBlock);
    }
    
    // Update in real-time
    public void UpdateTilingOffset(Vector2 newTiling, Vector2 newOffset)
    {
        tiling = newTiling;
        offset = newOffset;
        ApplySurfaceChanges();
    }
    
    // Set red color specifically
    public void SetRedColor(float intensity = 1.0f)
    {
        applyRedTint = true;
        redIntensity = intensity;
        ApplySurfaceChanges();
    }
    
    // Remove red tint
    public void RemoveRedColor()
    {
        applyRedTint = false;
        ApplySurfaceChanges();
    }
    
    // For texture atlas - select specific region
    public void SetTextureRegion(int columns, int rows, int cellIndex)
    {
        float cellWidth = 1f / columns;
        float cellHeight = 1f / rows;
        
        int row = cellIndex / columns;
        int col = cellIndex % columns;
        
        tiling = new Vector2(cellWidth, cellHeight);
        offset = new Vector2(col * cellWidth, row * cellHeight);
        
        ApplySurfaceChanges();
    }
    
    // This makes it work in edit mode without play
    void OnValidate()
    {
        // Apply changes both in edit mode and play mode
        ApplySurfaceChanges();
    }
    
    // Additional method for editor-time updates
    void Reset()
    {
        ApplySurfaceChanges();
    }
}
using System.ComponentModel;
//TODO: Convert to ScriptableObject
/// <summary>
/// AoSettings is a data container class that stores the settings for the ambient occlusion (AO) map generation process.
/// </summary>
public class AoSettings
{
    /// <summary>
    /// Blend is a float value representing the amount of AO map to be blended with the original height map.
    /// </summary>
    [DefaultValue(1.0f)]public float Blend;

    /// <summary>
    /// BlendText is a string representation of the value of Blend.
    /// </summary>
    [DefaultValue("1")] 
    public string BlendText;

    /// <summary>
    /// Depth is a float value representing the depth of the AO map.
    /// </summary>
    [DefaultValue(100.0f)]
    public float Depth;

    /// <summary>
    /// DepthText is a string representation of the value of Depth.
    /// </summary>
    [DefaultValue("100")] 
    public string DepthText;

    /// <summary>
    /// FinalBias is a float value representing the bias of the final AO map.
    /// </summary>
    [DefaultValue(0.0f)] 
    public float FinalBias;

    /// <summary>
    /// FinalBiasText is a string representation of the value of FinalBias.
    /// </summary>
    [DefaultValue("0")] 
    public string FinalBiasText;

    /// <summary>
    /// FinalContrast is a float value representing the contrast of the final AO map.
    /// </summary>
    [DefaultValue(1.0f)] 
    public float FinalContrast;

    /// <summary>
    /// FinalContrastText is a string representation of the value of FinalContrast.
    /// </summary>
    [DefaultValue("1")] 
    public string FinalContrastText;

    /// <summary>
    /// Spread is a float value representing the spread of the AO map.
    /// </summary>
    [DefaultValue(5.0f)] 
    public float Spread;

    /// <summary>
    /// SpreadText is a string representation of the value of Spread.
    /// </summary>
    [DefaultValue("50")] 
    public string SpreadText;

    /// <summary>
    /// AoSettings is a constructor that initializes the default values for the class properties.
    /// </summary>
    public AoSettings()
    {
        Spread = 50.0f;
        SpreadText = "50";

        Depth = 100.0f;
        DepthText = "100";

        FinalBias = 0.0f;
        FinalBiasText = "0";

        FinalContrast = 1.0f;
        FinalContrastText = "1";

        Blend = 1.0f;
        BlendText = "1";
    }
}

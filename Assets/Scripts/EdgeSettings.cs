// TODO: Convert to Scriptable Object

#region

using System.ComponentModel;

#endregion

/// <summary>
/// EdgeSettings class to hold the settings used for Edge Detection
/// </summary>
public class EdgeSettings
{
    /// <summary>
    /// The contrast for the first blur step
    /// </summary>
    [DefaultValue(1.0f)]
    public float Blur0Contrast;

    /// <summary>
    /// The string representation of the Blur0Contrast value
    /// </summary>
    [DefaultValue("1")]
    public string Blur0ContrastText;

    /// <summary>
    /// The weight of the first blur step
    /// </summary>
    [DefaultValue(1.0f)]
    public float Blur0Weight;

    /// <summary>
    /// The weight of the second blur step
    /// </summary>
    [DefaultValue(0.5f)]
    public float Blur1Weight;

    /// <summary>
    /// The weight of the third blur step
    /// </summary>
    [DefaultValue(0.3f)]
    public float Blur2Weight;

    /// <summary>
    /// The weight of the fourth blur step
    /// </summary>
    [DefaultValue(0.5f)]
    public float Blur3Weight;

    /// <summary>
    /// The weight of the fifth blur step
    /// </summary>
    [DefaultValue(0.7f)]
    public float Blur4Weight;

    /// <summary>
    /// The weight of the sixth blur step
    /// </summary>
    [DefaultValue(0.7f)]
    public float Blur5Weight;

    /// <summary>
    /// The weight of the seventh blur step
    /// </summary>
    [DefaultValue(0.3f)]
    public float Blur6Weight;

    /// <summary>
    /// The amount of crevice details to be added
    /// </summary>
    [DefaultValue(1.0f)]
    public float CreviceAmount;

    /// <summary>
    /// The string representation of the CreviceAmount value
    /// </summary>
    [DefaultValue("1")]
    public string CreviceAmountText;

    /// <summary>
    /// The amount of edge details to be added
    /// </summary>
    [DefaultValue(1.0f)]
    public float EdgeAmount;

    /// <summary>
    /// The string representation of the EdgeAmount value
    /// </summary>
    [DefaultValue("1")]
    public string EdgeAmountText;

    /// <summary>
    /// The final bias of the image after processing
    /// </summary>
    [DefaultValue(0.0f)]
    public float FinalBias;

    /// <summary>
    /// The string representation of the FinalBias value
    /// </summary>
    [DefaultValue("0")]
    public string FinalBiasText;

    /// <summary>
    /// The final contrast of the image after processing
    /// </summary>
    [DefaultValue(1.0f)]
    public float FinalContrast;
    /// <summary>
    /// The text value of the final contrast property
    /// </summary>
    [DefaultValue("1")]
    public string FinalContrastText;

    /// <summary>
    /// The pinch property of the edge settings
    /// </summary>
    [DefaultValue(1.0f)]
    public float Pinch;

    /// <summary>
    /// The text value of the pinch property
    /// </summary>
    [DefaultValue("1")]
    public string PinchText;

    /// <summary>
    /// The pillow property of the edge settings
    /// </summary>
    [DefaultValue(1.0f)]
    public float Pillow;

    /// <summary>
    /// The text value of the pillow property
    /// </summary>
    [DefaultValue("1")]
    public string PillowText;

    /// <summary>
    /// Constructor for the EdgeSettings class
    /// Initializes all properties with default values
    /// </summary>
    public EdgeSettings()
    {
        Blur0Contrast = 1.0f;
        Blur0ContrastText = "1";

        Blur0Weight = 1.0f;
        Blur1Weight = 0.5f;
        Blur2Weight = 0.3f;
        Blur3Weight = 0.5f;
        Blur4Weight = 0.7f;
        Blur5Weight = 0.7f;
        Blur6Weight = 0.3f;

        FinalContrast = 1.0f;
        FinalContrastText = "1";

        FinalBias = 0.0f;
        FinalBiasText = "0";

        EdgeAmount = 1.0f;
        EdgeAmountText = "1";

        CreviceAmount = 1.0f;
        CreviceAmountText = "1";

        Pinch = 1.0f;
        PinchText = "1";

        Pillow = 1.0f;
        PillowText = "1";
    }
}

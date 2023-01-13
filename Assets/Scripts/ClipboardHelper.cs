#region

using UnityEngine;

#endregion

/// <summary>
/// ClipboardHelper is a static class that provides a simple way to interact with the clipboard.
/// </summary>
public static class ClipboardHelper
{
    /// <summary>
    /// Get or set the current value of the clipboard.
    /// </summary>
    public static string ClipBoard
    {
        get => GUIUtility.systemCopyBuffer;
        set => GUIUtility.systemCopyBuffer = value;
    }
}
#region

using UnityEngine;

#endregion
/*Description:
 The CubemapPicker class is a simple script that allows the user to cycle through a set of cubemaps using a specified key (default is "C").
 When the key is pressed, the script increments a counter and uses it to select the next cubemap from an array of cubemaps.
 The selected cubemap is then set as the global cubemap for the project, which means it will be applied to any object that uses a shader that references the "_GlobalCubemap" property.
 */

/// <summary>
/// Class to Pick and change Cubemap on a scene
/// </summary>
public class CubemapPicker : MonoBehaviour
{
    private static readonly int GlobalCubemap = Shader.PropertyToID("_GlobalCubemap");
    private int _selectedCubemap;
    public Cubemap[] CubeMaps;
    public KeyCode Key;
    /// <summary>
    /// Update function is called once per frame
    /// it checks if the selected key is pressed, if so the selected cubemap will be incremented
    /// and if the incremented index is out of the range of the array it will be set to 0
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyDown(Key))
        {
            _selectedCubemap += 1;
            if (_selectedCubemap >= CubeMaps.Length) _selectedCubemap = 0;
        }

        Shader.SetGlobalTexture(GlobalCubemap, CubeMaps[_selectedCubemap]);
    }
}

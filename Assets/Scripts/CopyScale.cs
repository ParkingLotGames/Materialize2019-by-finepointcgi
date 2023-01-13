/*Description: The CopyScale class is a Unity MonoBehaviour that updates the local scale of the object it is attached to, to match that of another GameObject,
 * by adding a value that depends on the value of the property _Parallax of the material of that other object, and the x component of the property _Tiling.
 * The class also retrieves the Renderer component of the TargetObject in the Start method.*/

#region

using UnityEngine;

#endregion

public class CopyScale : MonoBehaviour
{
    private static readonly int Parallax = Shader.PropertyToID("_Parallax");
    private static readonly int Tiling1 = Shader.PropertyToID("_Tiling");
    private Renderer _renderer;
    public GameObject TargetObject;

    /// <summary>
    /// On start, gets the renderer component of the TargetObject
    /// </summary>
    private void Start()
    {
        _renderer = TargetObject.GetComponent<Renderer>();
    }

    /// <summary>
    /// On update, updates the scale of the current object based on the scale of the TargetObject and properties of its material
    /// </summary>
    private void Update()
    {
        var tempScale = TargetObject.transform.localScale; // get the local scale of the TargetObject
        var targetMaterial = _renderer.sharedMaterial; // get the shared material of the TargetObject

        if (targetMaterial.HasProperty("_Parallax")) // check if the material has the _Parallax property
        {
            var height = targetMaterial.GetFloat(Parallax); // get the value of _Parallax
            var tiling = targetMaterial.GetVector(Tiling1); // get the value of _Tiling

            tempScale.z += height * (1.0f / tiling.x); // update the scale of the current object
        }

        transform.localScale = tempScale; // set the local scale of the current object
    }
}

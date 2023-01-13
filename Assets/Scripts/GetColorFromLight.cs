#region

using UnityEngine;

#endregion

/* TODO: & Description
This script is responsible for updating the color of a material based on the color of a light. It does this by getting the color of the light component in the "Update" method and then sets the "_Color" property of the material's shader to the color of the light.
It could be split into 2 different scripts

One that has the logic to get the color of the light and store it in a variable
another one that has the logic to apply the color to the material.
*/

public class GetColorFromLight : MonoBehaviour
{
    /// <summary> 
    ///This variable will hold the reference to the _Color property of the shader
    /// </summary>
    private static readonly int Color = Shader.PropertyToID("_Color");
    /// <summary>
    /// This variable will hold the reference to the Light component
    /// </summary>
    private Light _thisLight;
    /// <summary>
    /// This variable will hold the reference to the Material component of the object
    /// </summary>
    private Material _thisMaterial;
    /// <summary>
    /// This variable will hold the reference to the GameObject that has the Light component 
    /// </summary>
    public GameObject LightObject;

    private void Start()
    {
        //Get the Light component from the LightObject variable
        _thisLight = LightObject.GetComponent<Light>();
        //Get the Material component from the object
        _thisMaterial = GetComponent<MeshRenderer>().material;
    }

    private void Update()
    {
        //Set the color of the _Color property of the shader to the color of the light
        _thisMaterial.SetColor(Color, _thisLight.color);
    }
}


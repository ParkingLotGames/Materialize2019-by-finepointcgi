/*So this script is updating the texture of the sample probe and setting it as the texture for the "Tex" property of the skybox material and the "ProbeCubemap" property of the shader. 
 * This allows the skybox material to use the texture from the sample probe, which is a reflection probe in the scene that captures the environment's reflections and lighting, making the skybox appear more realistic. 
 * The script also makes sure that when the script is disabled, the "UseProbeTexture" property of the shader is set to 0, and the "_USE_BAKED_CUBEMAP_ON" keyword is enabled 
 * and "_USE_BAKED_CUBEMAP_OFF" is disabled, which likely controls whether or not to use the sample probe texture in the skybox material.*/
/* TODO: One way to separate this script into single responsibility scripts would be to create two separate scripts: one for handling the reflection probe and one for handling the skybox material.

The script for handling the reflection probe could have the following responsibilities:

Updating the texture of the sample probe
Setting the texture as the "ProbeCubemap" property of the shader
Enabling and disabling the "UseProbeTexture" keyword in the shader
The script for handling the skybox material could have the following responsibilities:

Setting the texture of the sample probe as the "Tex" property of the skybox material
Managing the skybox material's properties and settings
*/

#region

using UnityEngine;

#endregion
[ExecuteInEditMode]
public class SampleProbeTex : MonoBehaviour
{
   //A unique identifier for the "UseProbeTexture" property in the shader.
   private static readonly int UseProbeTexture = Shader.PropertyToID("_UseProbeTexture");
   //A unique identifier for the "Tex" property in the shader.
   private static readonly int Tex = Shader.PropertyToID("_Tex");
   //A unique identifier for the "ProbeCubemap" property in the shader.
   private static readonly int ProbeCubemap = Shader.PropertyToID("_ProbeCubemap");
   //A reflection probe component that is used to sample the environment's lighting.
   public ReflectionProbe SampleProbe;
   //Skybox material that use to display the environment
   public Material SkyboxMaterial;

   private void Start()
   {
       //Start the render of the reflection probe
       SampleProbe.RenderProbe();
   }

   //This function is called when the script component is disabled.
   private void OnDisable()
   {
       //set the global useProbeTexture value to 0
       Shader.SetGlobalFloat(UseProbeTexture, 0);
       //enable the _USE_BAKED_CUBEMAP_ON keyword
       Shader.EnableKeyword("_USE_BAKED_CUBEMAP_ON");
       //disable the _USE_BAKED_CUBEMAP_OFF keyword
       Shader.DisableKeyword("_USE_BAKED_CUBEMAP_OFF");
   }

   private void Update()
   {
       // sets the texture of the "Tex" property of the skybox material to the texture of the sample probe
       SkyboxMaterial.SetTexture(Tex, SampleProbe.texture);

       //set the "ProbeCubemap" property of the shader to the texture of the sample probe
       Shader.SetGlobalTexture(ProbeCubemap, SampleProbe.texture);
   }
}

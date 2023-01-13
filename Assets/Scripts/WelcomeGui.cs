/*TODO:
Separate this script into single responsibility scripts

WelcomeScreenControl: This script would handle the logic for controlling the welcome screen, such as checking whether to skip the welcome screen, activating the necessary objects, and starting the intro sequence.

FadeControl: This script would handle the logic for fading the background and logo, including the FadeBackground(), FadeLogo(), and Intro() methods.

OnGUIControl: This script would handle the logic for drawing the background and logo textures on the GUI, including the OnGUI() method.

StartupControl: This script would handle the logic for setting the target frame rate and vSync settings, as well as setting the global cubemap texture.

WelcomeGui: This script would act as a wrapper for all the other scripts and would be attached to the object in the scene that you want to use as the Welcome Screen.
*/

#region

using System.Collections;
using UnityEngine;

#endregion

public class WelcomeGui : MonoBehaviour
{
    /// <summary>
    /// TODO: Use to replace GlobalCubemap
    /// Cubemap uniform shader variable index used to set the Skybox from script
    /// </summary>
    private static readonly int globalCubemapUniformInt = Shader.PropertyToID("_Cubemap");
    /// <summary>
    /// Fade time for the background texture
    /// </summary>
    private float backgroundFade = 1.0f;
    /// <summary>
    /// Fade time for the logo
    /// </summary>
    private float logoFade = 1.0f;
    /// <summary>
    /// Background texture for the splash screen
    /// </summary>
    public Texture2D background;
    /// <summary>
    /// Logo texture to be drawn on the GUI
    /// </summary>
    public Texture2D logo;
    /// <summary>
    /// //TODO: EXPLAIN CommandListExecutorObject to be activated when the welcome screen is skipped
    /// </summary>
    public GameObject commandListExecutorObject;
    /// <summary>
    /// controlsGuiObject to be activated when the welcome screen is skipped
    /// </summary>
    public GameObject controlsGuiObject;
    /// <summary>
    /// mainGuiObject to be activated when the welcome screen is skipped
    /// </summary>
    public GameObject mainGuiObject;
    /// <summary>
    /// settingsGuiObject to be activated when the welcome screen is skipped
    /// </summary>
    public GameObject settingsGuiObject;

    /// <summary>
    /// Whether or not to skip the welcome screen and activate objects immediately
    /// </summary>
    public bool skipWelcomeScreen;
    /// <summary>
    /// Cubemap to be set as the global cubemap
    /// </summary>
    public Cubemap initialCubeMap;
    /// <summary>
    /// testObject to be activated when the welcome screen is skipped
    /// </summary>
    public GameObject testObject;

    private void Start()
    {
        // Allow the application to run in the background
        Application.runInBackground = true;
        // Set the target frame rate and vSync count based on player preferences
        if (PlayerPrefs.HasKey("targetFrameRate"))
        {
            Application.targetFrameRate = PlayerPrefs.GetInt("targetFrameRate");
            QualitySettings.vSyncCount = PlayerPrefs.GetInt("Vsync");
        }
        // If the welcome screen should be skipped or the application is running in the editor
        if (skipWelcomeScreen || Application.isEditor)
        {
        // Activate objects and deactivate this gameobject
            ActivateObjects();
            gameObject.SetActive(false);
        }
        // Otherwise play the intro animation
        else
        {
            // Start the intro coroutine
            StartCoroutine(Intro());
        }

        // Set the global cubemap
        //Shader.SetGlobalTexture(GlobalCubemap, initialCubeMap);
        Shader.SetGlobalTexture(globalCubemapUniformInt, initialCubeMap);
    }

    /// <summary>
    /// Activates the necessary objects
    /// </summary>
    private void ActivateObjects()
    {
        //Activates all the game objects that are set in the inspector 
        testObject.SetActive(true);
        //mainGuiObject.SetActive(true);
        //settingsGuiObject.SetActive(true);
        controlsGuiObject.SetActive(true);
        commandListExecutorObject.SetActive(true);
    }

    private void OnGUI()
    {
        //Sets the color of the GUI elements to have a full alpha channel
        GUI.color = new Color(1, 1, 1, backgroundFade);
        //Draws the background texture on the whole screen
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), background);
        //Calculates the size and position of the logo texture to be drawn in the center of the screen
        var logoWidth = Mathf.FloorToInt(Screen.width * 0.75f);
        var logoHeight = Mathf.FloorToInt(logoWidth * 0.5f);
        var logoPosX = Mathf.FloorToInt(Screen.width * 0.5f - logoWidth * 0.5f);
        var logoPosY = Mathf.FloorToInt(Screen.height * 0.5f - logoHeight * 0.5f);
        //Sets the color of the logo to have a full alpha channel
        GUI.color = new Color(1, 1, 1, logoFade);
        //Draws the logo texture on the calculated position and size
        GUI.DrawTexture(new Rect(logoPosX, logoPosY, logoWidth, logoHeight), logo);
    }

    private IEnumerator Fadelogo(float target, float overTime)
    {
        //Fades the logo texture to the target alpha over the specified time
        var timer = overTime;
        var original = logoFade;

        while (timer > 0.0f)
        {
            timer -= Time.deltaTime;
            logoFade = Mathf.Lerp(target, original, timer / overTime);
            yield return new WaitForEndOfFrame();
        }

        logoFade = target;

        //yield return new WaitForEndOfFrame();
    }

    private IEnumerator FadeBackground(float target, float overTime)
    {
        //Fades the background texture to the target alpha over the specified time
        var timer = overTime;
        var original = backgroundFade;

        while (timer > 0.0f)
        {
            timer -= Time.deltaTime;
            backgroundFade = Mathf.Lerp(target, original, timer / overTime);
            yield return new WaitForEndOfFrame();
        }

        backgroundFade = target;

        //Deactivates the WelcomeGui game object after the background has been faded
        gameObject.SetActive(false);
    }

    private IEnumerator Intro()
    {
        // Fade in the logo
        StartCoroutine(Fadelogo(1.0f, 0.5f));

        // Wait for 3 seconds
        yield return new WaitForSeconds(3.0f);

        // Fade out the logo
        StartCoroutine(Fadelogo(0.0f, 1.0f));

        // Wait for 1 second 
        yield return new WaitForSeconds(1.0f);

        // Fades out the background
        StartCoroutine(FadeBackground(0.0f, 1.0f));
        
        // Activate the objects
        ActivateObjects();
    }
}
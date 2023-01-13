/*TODO:
 separate this script into single responsibility scripts, you could consider breaking it down into the following parts:

A settings manager script that handles loading, saving, and updating the settings.
A GUI script that handles displaying and updating the settings in the Unity editor.
A script that handles communication with other objects in the scene, such as the MainGui and OBJRotator objects.
Each of these scripts would have a specific responsibility and would be easier to maintain and understand.

For the Settings Manager script, it could have methods like LoadSettings(), SaveSettings() and SetSettings().
For the GUI script, it could have methods like DoMyWindow(), and it would be responsible for displaying the settings in the Unity editor and updating them.
For the communication script, it could have methods like SetNormalMode(), SetFormat() that would handle the communication between the SettingsGui and other objects.

You could also consider refactoring other scripts that are dependent on the SettingsGui class to remove their dependency on it, so that they only need to know about the settings they require and not the whole SettingsGui class.
*/

/*Description:
Settings: A class that contains several fields such as FileFormat, NormalMapMaxStyle and NormalMapMayaStyle.

SettingsGui: A MonoBehaviour that stores an instance of itself, Settings object, several GUI elements and some properties for the application.

Start(): Loads the settings from PlayerPrefs when the script is started.

LoadSettings(): Loads the settings from PlayerPrefs, if there are any. If not, it sets default values and saves it to PlayerPrefs.

SaveSettings(): Serializes the current Settings object to a string and saves it to the PlayerPrefs.

SetNormalMode(): Uses the NormalMapMayaStyle setting to set the global flip normal y variable in the shader.

SetSettings(): Sets the settings to the elements in the application.

DoMyWindow(int windowId): A callback function that creates a GUI window, it contains several toggles, labels and buttons to change the values of the settings.

SetFrameRate(int FrameRate): This function sets the frame rate of the game to the value passed in as an argument.

SetVsyncOn(): This function enables the vsync setting in the game.

SaveDefaultFileFormat(): This function saves the default file format used to export textures.

SaveAndClose(): This function saves the current settings and closes the settings menu.

setDefaultPropertyMap(): This function sets the default property maps for the red, green, and blue channels.

NormalMapMaxStyle(bool Bool): This function sets the normal map style to 'Max Style' if the passed in boolean is true, otherwise, it sets it to 'Maya Style'.

NormalMapMayaStyle(bool Bool): This function sets the normal map style to 'Maya Style' if the passed in boolean is true, otherwise, it sets it to 'Max Style'.

SetPostProcessing(bool Bool): This function enables or disables post processing effects in the game based on the passed in boolean.

SetHideUI(bool Bool): This function allows or disallows hiding the UI when the object is rotated based on the passed in boolean.
*/
#region

using System.IO;
using System.Xml.Serialization;
using UnityEngine;

#endregion

public class Settings
{
    public FileFormat FileFormat;
    public bool NormalMapMaxStyle;
    public bool NormalMapMayaStyle;

    //public bool PostProcessEnabled;
    public PropChannelMap PropBlue;
    public PropChannelMap PropGreen;

    public PropChannelMap PropRed;
}


public class SettingsGui : MonoBehaviour
{
    private const string SettingsKey = "Settings";
    public static SettingsGui Instance;
    private static readonly int FlipNormalY = Shader.PropertyToID("_FlipNormalY");
    //private bool _windowOpen;

    private Rect _windowRect = new Rect(Screen.width - 300, Screen.height - 320, 280, 600);
    //public PostProcessGui PostProcessGui;
    [HideInInspector] public Settings Settings = new Settings();
    public ObjRotator OBJRotator;
    public GameObject SettingsMenu;

    private void Start()
    {
        Instance = this;
        
        LoadSettings();
    }

    public void LoadSettings()
    {
        if (PlayerPrefs.HasKey(SettingsKey))
        {
            var set = PlayerPrefs.GetString(SettingsKey);
            var serializer = new XmlSerializer(typeof(Settings));
            using (TextReader sr = new StringReader(set))
            {
                Settings = serializer.Deserialize(sr) as Settings;
            }
        }
        else
        {
            Settings.NormalMapMaxStyle = true;
            Settings.NormalMapMayaStyle = false;
            //Settings.PostProcessEnabled = true;
            Settings.PropRed = PropChannelMap.None;
            Settings.PropGreen = PropChannelMap.None;
            Settings.PropBlue = PropChannelMap.None;
            Settings.FileFormat = FileFormat.Png;
            SaveSettings();
        }

        SetSettings();
    }

    public  void SaveSettings()
    {
        var serializer = new XmlSerializer(typeof(Settings));
        using (TextWriter sw = new StringWriter())
        {
            serializer.Serialize(sw, Settings);
            PlayerPrefs.SetString(SettingsKey, sw.ToString());
        }
    }

    public void SetNormalMode()
    {
        var flipNormalY = 0;
        if (Settings.NormalMapMayaStyle) flipNormalY = 1;

        Shader.SetGlobalInt(FlipNormalY, flipNormalY);
    }

    public void SetSettings()
    {
        SetNormalMode();

        //if (Settings.PostProcessEnabled)
            //PostProcessGui.PostProcessOn();
        //else
            //PostProcessGui.PostProcessOff();

        var mainGui = MainGui.Instance;
        mainGui.PropRed = Settings.PropRed;
        mainGui.PropGreen = Settings.PropGreen;
        mainGui.PropBlue = Settings.PropBlue;

        mainGui.SetFormat(Settings.FileFormat);
    }

    private void DoMyWindow(int windowId)
    {
        const int offsetX = 10;

        var offsetY = 30;

        OBJRotator.AllowHideUI =  GUI.Toggle(new Rect(offsetX, offsetY, 150, 30), OBJRotator.AllowHideUI, "Hide UI On Rotate");

        offsetY += 20;

        GUI.Label(new Rect(offsetX, offsetY, 250, 30), "Normal Map Style");

        offsetY += 30;

        Settings.NormalMapMaxStyle =
            GUI.Toggle(new Rect(offsetX, offsetY, 100, 30), Settings.NormalMapMaxStyle, " Max Style");
        Settings.NormalMapMayaStyle = !Settings.NormalMapMaxStyle;


        Settings.NormalMapMayaStyle = GUI.Toggle(new Rect(offsetX + 100, offsetY, 100, 30), Settings.NormalMapMayaStyle,
            " Maya Style");
        Settings.NormalMapMaxStyle = !Settings.NormalMapMayaStyle;

        offsetY += 30;

        //Settings.PostProcessEnabled = GUI.Toggle(new Rect(offsetX, offsetY, 280, 30), Settings.PostProcessEnabled,
            //" Enable Post Process By Default");

        offsetY += 20;
        GUI.Label(new Rect(offsetX, offsetY, 250, 30), "Limit Frame Rate");

        offsetY += 20;

        if (GUI.Button(new Rect(offsetX + 40, offsetY, 30, 30), "30"))
        {
            Application.targetFrameRate = 30;
            QualitySettings.vSyncCount = 0;
            PlayerPrefs.SetInt("targetFrameRate", 30);
            PlayerPrefs.SetInt("Vsync", 0);
        }
        if (GUI.Button(new Rect(offsetX + 80, offsetY, 30, 30), "60"))
        {
            Application.targetFrameRate = 60;
            QualitySettings.vSyncCount = 0;
            PlayerPrefs.SetInt("targetFrameRate", 60);
            PlayerPrefs.SetInt("Vsync", 0);
        }
        if (GUI.Button(new Rect(offsetX + 120, offsetY, 30, 30), "120"))
        {
            Application.targetFrameRate = 120;
            QualitySettings.vSyncCount = 0;
            PlayerPrefs.SetInt("targetFrameRate", 120);
            PlayerPrefs.SetInt("Vsync", 0);
        }

        if (GUI.Button(new Rect(offsetX + 160, offsetY, 40, 30), "None"))
        {
            //Application.targetFrameRate = 120;
            QualitySettings.vSyncCount = 1;
           // PlayerPrefs.SetInt("targetFrameRate", 30);
            PlayerPrefs.SetInt("Vsync", 1);
        }

        offsetY += 40;


        if (GUI.Button(new Rect(offsetX, offsetY, 260, 25), "Set Default Property Map Channels"))
        {
            Settings.PropRed = MainGui.Instance.PropRed;
            Settings.PropGreen = MainGui.Instance.PropGreen;
            Settings.PropBlue = MainGui.Instance.PropBlue;
        }

        offsetY += 30;

        if (GUI.Button(new Rect(offsetX, offsetY, 260, 25), "Set Default File Format"))
            Settings.FileFormat = FileFormat.Png;

        offsetY += 40;

        if (GUI.Button(new Rect(offsetX + 140, offsetY, 120, 30), "Save and Close"))
        {
            SaveSettings();
            SetNormalMode();
            //_windowOpen = false;
        }

        GUI.DragWindow();
    }
    public void SetFrameRate(int FrameRate)
    {
        Application.targetFrameRate = FrameRate;
        QualitySettings.vSyncCount = 0;
        PlayerPrefs.SetInt("targetFrameRate", FrameRate);
        PlayerPrefs.SetInt("Vsync", 0);
    }
    public void SetVsyncOn()
    {
        //Application.targetFrameRate = 120;
        QualitySettings.vSyncCount = 1;
        // PlayerPrefs.SetInt("targetFrameRate", 30);
        PlayerPrefs.SetInt("Vsync", 1);
    }
    public void SaveDefaultFileFormat()
    {
        Settings.FileFormat = FileFormat.Png;
    }

    public void SaveAndClose()
    {
        SaveSettings();
        SetNormalMode();
    }

    public void setDefaultPropertyMap()
    {
        Settings.PropRed = MainGui.Instance.PropRed;
        Settings.PropGreen = MainGui.Instance.PropGreen;
        Settings.PropBlue = MainGui.Instance.PropBlue;
        
    }

    public void NormalMapMaxStyle(bool Bool)
    {
        Settings.NormalMapMaxStyle = Bool;
        Settings.NormalMapMayaStyle = !Bool;
    }

    public void NormalMapMayaStyle(bool Bool)
    {
        Settings.NormalMapMayaStyle = Bool;
        Settings.NormalMapMaxStyle = !Bool;
    }

    public void SetPostProcessing(bool Bool)
    {
        //Settings.PostProcessEnabled = Bool;
    }
    public void SetHideUI(bool Bool) {
        OBJRotator.AllowHideUI = Bool;
     }
    /*
    private void OnGUI()
    {
        _windowRect = new Rect(Screen.width - 300, Screen.height - 360, 280, 300);

        if (_windowOpen) _windowRect = GUI.Window(20, _windowRect, DoMyWindow, "Setting and Preferences");

        if (!GUI.Button(new Rect(Screen.width - 280, Screen.height - 40, 80, 30), "Settings")) return;
        if (_windowOpen)
        {
            SaveSettings();
            SettingsMenu.SetActive(false);
        }
        else
        {
            SettingsMenu.SetActive(true);
        }
    }*/
}
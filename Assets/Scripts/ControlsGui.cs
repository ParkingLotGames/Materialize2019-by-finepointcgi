/* This class is a simple GUI class that displays a window with control information on it.
 * It is using Unity's GUI system to draw the window and its contents.
 * The window will display the controls of the application, such as how to rotate the model, move the model, zoom in/out, rotate the light, and rotate the background.
 * The window will be closed when the user clicks the "Close" button.*/
#region

using UnityEngine;

#endregion

public class ControlsGui : MonoBehaviour
{
    /// <summary>
    /// A flag indicating whether the window is currently open or not
    /// </summary>
    //private bool _windowOpen;
    /// <summary>
    /// A rectangle representing the position and size of the window
    /// </summary>
    private Rect _windowRect = new Rect(Screen.width - 520, Screen.height - 320, 300, 600);

    /// <summary>
    /// Draws the window in OnGUI method
    /// </summary>
    /// <param name="windowId"></param>
    private void DoMyWindow(int windowId)
    {
        const int offsetX = 10;
        var offsetY = 30;

        GUI.Label(new Rect(offsetX, offsetY, 250, 30), "Rotate Model");
        offsetY += 20;
        GUI.Label(new Rect(offsetX, offsetY, 250, 30), "Right Mouse Button");
        offsetY += 30;

        GUI.Label(new Rect(offsetX, offsetY, 250, 30), "Move Model");
        offsetY += 20;
        GUI.Label(new Rect(offsetX, offsetY, 250, 30), "Middle Mouse Button");
        offsetY += 30;

        GUI.Label(new Rect(offsetX, offsetY, 250, 30), "Zoom In/Out");
        offsetY += 20;
        GUI.Label(new Rect(offsetX, offsetY, 250, 30), "Mouse Scroll Wheel");
        offsetY += 30;

        GUI.Label(new Rect(offsetX, offsetY, 250, 30), "Rotate Light");
        offsetY += 20;
        GUI.Label(new Rect(offsetX, offsetY, 250, 30), "Middle Mouse Button + L");
        offsetY += 30;

        GUI.Label(new Rect(offsetX, offsetY, 250, 30), "Rotate Background");
        offsetY += 20;
        GUI.Label(new Rect(offsetX, offsetY, 250, 30), "Middle Mouse Button + B");
        offsetY += 30;

       // if (GUI.Button(new Rect(offsetX + 160, offsetY, 120, 30), "Close")) _windowOpen = false;
    }

    private void OnGUI()
    {
        //_windowRect = new Rect(Screen.width - 480, Screen.height - 370, 170, 280);

        GUI.Window(22, _windowRect, DoMyWindow, "Controls");

        //if (!GUI.Button(new Rect(Screen.width - 370, Screen.height - 40, 80, 30), "Controls")) return;
        //_windowOpen = !_windowOpen;
    }
}
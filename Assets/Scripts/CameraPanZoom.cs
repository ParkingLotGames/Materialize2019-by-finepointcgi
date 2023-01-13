#region

using UnityEngine;

#endregion
/*Description: This script is used to allow the user to pan and zoom the camera in the scene by moving the mouse while holding down certain keys and/or pressing certain mouse buttons.
 * The script is attached to a camera object in the scene and it uses the UnityEngine.Input class to detect mouse and keyboard input.
 * The script uses the mouse position, mouse offset, mouse button state, and mouse scroll wheel input to update the target 
 * position of the camera, and then gradually moves the camera towards the target position in the Update() method.*/
public class CameraPanZoom : MonoBehaviour
{
    private Vector2 _lastMousePos; // A variable to store the previous mouse position to calculate the offset of the mouse movement
    private Vector2 _mousePos; // A variable to store the current mouse position
    private float _targetFov; // A variable to store the target field of view for the camera
    private Vector3 _targetPos; // A variable to store the target position for the camera
    public KeyCode[] KeyToHold; // An array of key codes representing keys that need to be held down to enable panning
    public int MouseButtonPan; // An integer representing the mouse button that needs to be pressed to enable panning
    
    // Use this for initialization
    private void Start()
    {
        _targetPos = transform.position; // Set the target position to the current position of the camera
    }

    // Update is called once per frame
    private void Update()
    {
        if (!MainGui.Instance) return; // Check if the MainGui instance is null and return if it is
        _mousePos = Input.mousePosition; // Get the current mouse position
        var mouseOffset = _mousePos - _lastMousePos; // Calculate the offset of the mouse movement based on the current and last mouse position

        var keyHeld = false; // A flag to check if any of the required keys are held down
        foreach (var t in KeyToHold)
        {
            if (Input.GetKey(t))
            {
                keyHeld = true;
            }
        }

        var mouseDown = Input.GetMouseButton(MouseButtonPan);
        if ((KeyToHold.Length > 0 && keyHeld || KeyToHold.Length == 0) && mouseDown)
        {
            MainGui.Instance.SaveHideStateAndHideAndLock(this);

            _targetPos -= new Vector3(1, 0, 0) * mouseOffset.x * 0.025f; // Update the target position based on the horizontal mouse offset
            _targetPos -= new Vector3(0, 1, 0) * mouseOffset.y * 0.025f; // Update the target position based on the vertical mouse offset
        }
        else
        {
            MainGui.Instance.HideGuiLocker.Unlock(this);
        }

        _targetPos += new Vector3(0, 0, 1) * Input.GetAxis("Mouse ScrollWheel") * 3.0f; // Update the target position based on the mouse scroll wheel input

        var trf = transform;
        var position = trf.position;
        position += (_targetPos - position) * 0.05f; // Update the camera position based on the target position
        trf.position = position;

        _lastMousePos = _mousePos; // Update the last mouse position
    }
}

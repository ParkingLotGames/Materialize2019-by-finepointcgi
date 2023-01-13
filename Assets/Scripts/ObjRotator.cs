#region

using UnityEngine;

#endregion
public class ObjRotator : MonoBehaviour
{
    // variable to store the last mouse position 
    private Vector2 _lastMousePos;
    // variable to store the lerped rotation
    private Vector3 _lerpRotation;
    // variable to store the number of times the mouse button is pressed
    private int _mouseDownCount;
    // variable to store the current mouse position
    private Vector2 _mousePos;
    // variable to store the rotation of the object
    private Vector3 _rotation;

    // public variable to allow rotation on the x-axis
    public bool AllowX = true;
    // public variable to allow rotation on the y-axis
    public bool AllowY = true;
    // public variable to determine if a key needs to be held to rotate the object
    public bool HoldKey;
    // public variable to invert the rotation on the x-axis
    public bool InvertX;
    // public variable to invert the rotation on the y-axis
    public bool InvertY;
    // public variable to determine if the UI should be hidden while rotating
    public bool AllowHideUI;
    // public variable to store the key that needs to be held to rotate the object
    public KeyCode KeyToHold = KeyCode.L;
    // public variable to store the mouse button used for rotation
    public int MouseButton;

    // Use this for initialization
    private void Start()
    {
        // initialize the mouse position and last mouse position variables
        _mousePos = Input.mousePosition;
        _lastMousePos = _mousePos;

        // initialize the rotation variable
        _rotation = transform.eulerAngles;
        // initialize the lerped rotation variable
        _lerpRotation = _rotation;
    }

    // public method to reset the rotation of the object
    public void Reset()
    {
        _rotation = new Vector3(0, 0, 0);
        _lerpRotation = _rotation;
        transform.eulerAngles = _lerpRotation;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        // check if the main GUI instance exists
        if (!MainGui.Instance) return;
        // update the mouse position variable
        _mousePos = Input.mousePosition;

        // calculate the difference between the current and last mouse positions
        var mouseOffset = _mousePos - _lastMousePos;

        // check if the mouse button is pressed
        if (Input.GetMouseButton(MouseButton))
            // increment the mouse down count
            _mouseDownCount++;
        else
            // reset the mouse down count
            _mouseDownCount = 0;

        // check if the key needs to be held and if it is held or if it doesn't need to be held and the mouse has been pressed for more than one frame
        if ((HoldKey && Input.GetKey(KeyCode.X) || HoldKey == false) && _mouseDownCount > 1)
        {
            // check if rotation on the x-axis is allowed
            if (AllowX)
            {
                // check if the x-axis rotation needs to be inverted
                if (InvertX)
                    _rotation -= new Vector3(0, 1, 0) * mouseOffset.x * 0.3f; // if invertX is true, subtract mouse offset from the x-axis rotation
                else
                    _rotation += new Vector3(0, 1, 0) * mouseOffset.x * 0.3f; // if invertX is false, add mouse offset to the x-axis rotation
            }

            if (AllowY)
            {
                if (InvertY)
                    _rotation -= new Vector3(1, 0, 0) * mouseOffset.y * 0.3f; // if invertY is true, subtract mouse offset from the y-axis rotation
                else
                    _rotation += new Vector3(1, 0, 0) * mouseOffset.y * 0.3f; // if invertY is false, add mouse offset to the y-axis rotation
            }

            _rotation.x = Mathf.Clamp(_rotation.x, -80, 80); // keep the x-axis rotation within a certain range
            if (AllowHideUI) MainGui.Instance.SaveHideStateAndHideAndLock(this); // if AllowHideUI is true, call the SaveHideStateAndHideAndLock method in the MainGui class
        }
        else
        {
            MainGui.Instance.HideGuiLocker.Unlock(this); // if mouse button is not being held down, call the Unlock method in the HideGuiLocker class
        }


        _lerpRotation = _lerpRotation * 0.95f + _rotation * 0.05f; // calculate the lerp rotation using the current rotation and the target rotation
        transform.eulerAngles = _lerpRotation; // set the rotation of the object to the lerp rotation

        _lastMousePos = _mousePos; // set the last mouse position to the current mouse position
    }
}

﻿//TODO: Some names suck, I don't really understand what this is used for and what is the blit shader used for
/* TODO: Separating the AlignmentGui script into several single responsibility scripts will involve breaking up the functionality of the script into smaller, independent scripts that each have a specific responsibility. Here are some suggestions on how you could accomplish this:

You can extract the GUI-related functionality into a separate script. This script would be responsible for handling the user interface elements, such as buttons, sliders, and labels, that are used to control the alignment process.
You can extract the functionality related to the alignment maps (lensMap, perspectiveMap and alignMap) into a separate script. This script would be responsible for creating and updating the alignment maps, and applying the necessary filters and effects to them.
Extract the functionality related to the alignment points( PointTl, PointTr, PointBl, PointBr), into a separate script. This script would be responsible for handling the user's interactions with the alignment points, such as selecting and dragging them.
Extract the functionality related to the processing of the maps into a separate script. This script would be responsible for handling the processing of the maps, such as rescaling, and setting properties.
You can extract the functionality related to the material properties into a separate script. This script would be responsible for setting the properties of the material to be aligned
*/
/* Description:
 The Awake() function is used to set the _camera variable to the main camera in the scene.
Initialize() is used to set up the initial values and states of the object. It sets the gameObject active, initializes the _mainGui variable, sets the TestObject's shared material to ThisMaterial, and creates a new material _blitMaterial using the "Hidden/Blit_Alignment" shader. It also sets _textureToAlign to one of the maps from the mainGui.
CleanupTexture() method is used to clean up the textures and reset some variables when the object is closed
Close() method is used to deactivate the gameObject and call the CleanupTextures() method
SelectClosestPoint() method is used to select the point on the texture that's closest to the mouse cursor.
DragPoint() method is used to handle the logic for dragging points on the texture.
Update() Commented line by line. It calls the SelectClosestPoint() and DragPoint() methods, process the map, set the properties in the material and blitMaterial, and update the GUI elements.
DoMyWindow(int windowId) method is used to define the layout and functionality of the window that is displayed on the GUI. It contains sliders and buttons for selecting different maps and adjusting settings.
OnGUI() is a Unity built-in method that is called to display GUI elements on the screen.
ProcessMap(Texture2D map) method is used to process the maps, it creates and sets the render textures _alignMap, _lensMap and _perspectiveMap.
Texture2D SetMap(string map) method is used to set the map specified by the argument to _textureToAlign.
RenderTexture SetMap(string map) method is used to set the render texture specified by the argument.
SetMaps() method is used to set the maps in the material and blitMaterial.
 */
#region

using System.Collections;
using JetBrains.Annotations;
using UnityEngine;

#endregion

public class AlignmentGui : MonoBehaviour
{
    /// <summary>
    /// The Shader property ID for the target point.
    /// </summary>
    private static readonly int TargetPoint = Shader.PropertyToID("_TargetPoint");

    /// <summary>
    /// The Shader property ID for the main texture.
    /// </summary>
    private static readonly int MainTex = Shader.PropertyToID("_MainTex");

    /// <summary>
    /// The Shader property ID for the corrected texture.
    /// </summary>
    private static readonly int CorrectTex = Shader.PropertyToID("_CorrectTex");

    /// <summary>
    /// The Shader property ID for the point scale.
    /// </summary>
    private static readonly int PointScale = Shader.PropertyToID("_PointScale");
    /// <summary>
    /// The Property ID of the top-left point of the quadrilateral used for the alignment.
    /// </summary>
    private static readonly int PointTl = Shader.PropertyToID("_PointTL");
    /// <summary>
    /// The Property ID of the top-right point of the quadrilateral used for the alignment.
    /// </summary>
    private static readonly int PointTr = Shader.PropertyToID("_PointTR");
    /// <summary>
    /// The Property ID of the bottom-left point of the quadrilateral used for the alignment.
    /// </summary>
    private static readonly int PointBl = Shader.PropertyToID("_PointBL");
    /// <summary>
    /// The Property ID of the bottom-right point of the quadrilateral used for the alignment.
    /// </summary>
    private static readonly int PointBr = Shader.PropertyToID("_PointBR");
    /// <summary>
    /// The width of the texture
    /// </summary>
    private static readonly int Width = Shader.PropertyToID("_Width");
    /// <summary>
    /// The height of the texture
    /// </summary>
    private static readonly int Height = Shader.PropertyToID("_Height");
    /// <summary>
    /// The lens distort value
    /// </summary>
    private static readonly int Lens = Shader.PropertyToID("_Lens");
    /// <summary>
    /// The perspective x value
    /// </summary>
    private static readonly int PerspectiveX = Shader.PropertyToID("_PerspectiveX");
    /// <summary>
    /// The perspective y value
    /// </summary>
    private static readonly int PerspectiveY = Shader.PropertyToID("_PerspectiveY");
    /// <summary>
    /// The slider value
    /// </summary>
    private static readonly int Slider = Shader.PropertyToID("_Slider");

    private RenderTexture _alignMap;
    private Material _blitMaterial;
    private Camera _camera;
    private bool _doStuff;
    private int _grabbedPoint;
    private float _lensDistort;
    private string _lensDistortText = "0.0";
    private RenderTexture _lensMap;
    private MainGui _mainGui;
    private RenderTexture _perspectiveMap;

    private float _perspectiveX;
    private string _perspectiveXText = "0.0";

    private float _perspectiveY;
    private string _perspectiveYText = "0.0";
    private Vector2 _pointBl = new Vector2(0.0f, 0.0f);
    private Vector2 _pointBr = new Vector2(1.0f, 0.0f);

    private Vector2 _pointTl = new Vector2(0.0f, 1.0f);
    private Vector2 _pointTr = new Vector2(1.0f, 1.0f);

    private float _slider = 0.5f;
    private Vector2 _startOffset = Vector2.zero;

    private Texture2D _textureToAlign;

    private Rect _windowRect = new Rect(30, 300, 300, 530);
    [UsedImplicitly] public bool NewTexture;
    public GameObject TestObject;

    public Material ThisMaterial;

    private void Awake()
    {
        _camera = Camera.main;
    }

    public void Initialize()
    {
        gameObject.SetActive(true);
        _mainGui = MainGui.Instance;
        TestObject.GetComponent<Renderer>().sharedMaterial = ThisMaterial;
        _blitMaterial = new Material(Shader.Find("Hidden/Blit_Alignment")) {hideFlags = HideFlags.HideAndDontSave};

        if (_mainGui.DiffuseMapOriginal != null)
            _textureToAlign = _mainGui.DiffuseMapOriginal;
        else if (_mainGui.HeightMap != null)
            _textureToAlign = _mainGui.HeightMap;
        else if (_mainGui.MetallicMap != null)
            _textureToAlign = _mainGui.MetallicMap;
        else if (_mainGui.SmoothnessMap != null)
            _textureToAlign = _mainGui.SmoothnessMap;
        else if (_mainGui.EdgeMap != null)
            _textureToAlign = _mainGui.EdgeMap;
        else if (_mainGui.AoMap != null) _textureToAlign = _mainGui.AoMap;


        _doStuff = true;
    }


    private static void CleanupTexture(RenderTexture texture)
    {
        if (!texture) return;
        texture.Release();
        // ReSharper disable once RedundantAssignment
        texture = null;
    }

    public void Close()
    {
        CleanupTexture(_lensMap);
        CleanupTexture(_alignMap);
        CleanupTexture(_perspectiveMap);
        gameObject.SetActive(false);
    }

    private void SelectClosestPoint()
    {
        if (Input.GetMouseButton(0)) return;
        if (!_camera) return;

        if (!Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out var hit))
            return;

        var hitTc = hit.textureCoord;

        var dist1 = Vector2.Distance(hitTc, _pointTl);
        var dist2 = Vector2.Distance(hitTc, _pointTr);
        var dist3 = Vector2.Distance(hitTc, _pointBl);
        var dist4 = Vector2.Distance(hitTc, _pointBr);

        var closestDist = dist1;
        var closestPoint = _pointTl;
        _grabbedPoint = 0;
        if (dist2 < closestDist)
        {
            closestDist = dist2;
            closestPoint = _pointTr;
            _grabbedPoint = 1;
        }

        if (dist3 < closestDist)
        {
            closestDist = dist3;
            closestPoint = _pointBl;
            _grabbedPoint = 2;
        }

        if (dist4 < closestDist)
        {
            closestDist = dist4;
            closestPoint = _pointBr;
            _grabbedPoint = 3;
        }

        if (closestDist > 0.1f)
        {
            closestPoint = new Vector2(-1, -1);
            _grabbedPoint = -1;
        }

        ThisMaterial.SetVector(TargetPoint, closestPoint);
    }

    private void DragPoint()
    {
        if (!Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out var hit))
            return;

        var hitTc = hit.textureCoord;

        if (Input.GetMouseButtonDown(0))
        {
            _startOffset = hitTc;
        }
        else if (Input.GetMouseButton(0))
        {
            Vector2 point;
            switch (_grabbedPoint)
            {
                case 0:
                    _pointTl += hitTc - _startOffset;
                    point = _pointTl;
                    break;
                case 1:
                    _pointTr += hitTc - _startOffset;
                    point = _pointTr;
                    break;
                case 2:
                    _pointBl += hitTc - _startOffset;
                    point = _pointBl;
                    break;
                case 3:
                    _pointBr += hitTc - _startOffset;
                    point = _pointBr;

                    break;
                default: return;
            }

            if (point != null) ThisMaterial.SetVector(TargetPoint, point);

            _startOffset = hitTc;
        }

        _doStuff = true;
    }

    private void Update()
    {
        // calls the SelectClosestPoint method
        SelectClosestPoint();
        // calls the DragPoint method
        DragPoint();
        // calls the ProcessMap method, passing in the _textureToAlign variable
        ProcessMap(_textureToAlign);

        // calculate the aspect ratio of the texture, width divided by height
        var aspect = _textureToAlign.width / (float)_textureToAlign.height;
        // constant area of 1.0
        const float area = 1.0f;
        // initializes a Vector2 with a value of 1 in x and y
        var pointScale = Vector2.one;
        // sets the x value of pointScale to the aspect ratio
        pointScale.x = aspect;
        // calculates the new area using the pointScale x and y values
        var newArea = pointScale.x * pointScale.y;
        // calculates the areaScale using the square root of the ratio of area to newArea
        var areaScale = Mathf.Sqrt(area / newArea);

        // multiplies the x value of pointScale by areaScale
        pointScale.x *= areaScale;
        // multiplies the y value of pointScale by areaScale
        pointScale.y *= areaScale;

        // sets the MainTex texture property of the ThisMaterial material to the _lensMap texture
        ThisMaterial.SetTexture(MainTex, _lensMap);
        // Setting the _perspectiveMap RenderTexture to the CorrectTex property of ThisMaterial material 
        ThisMaterial.SetTexture(CorrectTex, _perspectiveMap);
        // Setting pointScale vector to the PointScale property of ThisMaterial material
        ThisMaterial.SetVector(PointScale, pointScale);

        // Setting _pointTl vector to the PointTl property of ThisMaterial material
        ThisMaterial.SetVector(PointTl, _pointTl);
        // Setting _pointTr vector to the PointTr property of ThisMaterial material
        ThisMaterial.SetVector(PointTr, _pointTr);
        // Setting _pointBl vector to the PointBl property of ThisMaterial material
        ThisMaterial.SetVector(PointBl, _pointBl);
        // Setting _pointBr vector to the PointBr property of ThisMaterial material
        ThisMaterial.SetVector(PointBr, _pointBr);
        // Setting _pointTl vector to the PointTl property of _blitMaterial material
        _blitMaterial.SetVector(PointTl, _pointTl);
        // Setting _pointTr vector to the PointTr property of _blitMaterial material
        _blitMaterial.SetVector(PointTr, _pointTr);
        // Setting _pointBl vector to the PointBl property of _blitMaterial material
        _blitMaterial.SetVector(PointBl, _pointBl);
        // Setting _pointBr vector to the PointBr property of _blitMaterial material
        _blitMaterial.SetVector(PointBr, _pointBr);
        // Setting the width of _textureToAlign to the Width property of _blitMaterial material
        _blitMaterial.SetFloat(Width, _textureToAlign.width);
        // Setting the height of _textureToAlign to the Height property of _blitMaterial material
        _blitMaterial.SetFloat(Height, _textureToAlign.height);
        // Setting the _lensDistort variable to the Lens property of _blitMaterial material
        _blitMaterial.SetFloat(Lens, _lensDistort);
        // Setting the _perspectiveX variable to the PerspectiveX property of _blitMaterial material
        _blitMaterial.SetFloat(PerspectiveX, _perspectiveX);
        // Setting the _perspectiveY variable to the PerspectiveY property of _blitMaterial material
        _blitMaterial.SetFloat(PerspectiveY, _perspectiveY);
        // if _doStuff is true, set _doStuff to false
        if (_doStuff)
        {
            _doStuff = false;
        }
        // Setting the _slider variable to the Slider property of ThisMaterial material
        ThisMaterial.SetFloat(Slider, _slider);
    }

    private void DoMyWindow(int windowId)
    {
        const int offsetX = 10;
        var offsetY = 30;

        GUI.Label(new Rect(offsetX, offsetY, 250, 30), "Alignment Reveal Slider");
        _slider = GUI.HorizontalSlider(new Rect(offsetX, offsetY + 20, 280, 10), _slider, 0.0f, 1.0f);
        offsetY += 40;

        GUI.Label(new Rect(offsetX, offsetY, 250, 30), "Preview Map");
        offsetY += 30;

        GUI.enabled = _mainGui.DiffuseMapOriginal != null;
        if (GUI.Button(new Rect(offsetX, offsetY, 130, 30), "Original Diffuse Map"))
        {
            _textureToAlign = _mainGui.DiffuseMapOriginal;
            _doStuff = true;
        }

        GUI.enabled = _mainGui.DiffuseMap != null;
        if (GUI.Button(new Rect(offsetX + 150, offsetY, 130, 30), "Diffuse Map"))
        {
            _textureToAlign = _mainGui.DiffuseMap;
            _doStuff = true;
        }

        offsetY += 40;


        GUI.enabled = _mainGui.HeightMap != null;
        if (GUI.Button(new Rect(offsetX, offsetY, 130, 30), "Height Map"))
        {
            _textureToAlign = _mainGui.HeightMap;
            _doStuff = true;
        }

        offsetY += 40;

        GUI.enabled = _mainGui.MetallicMap != null;
        if (GUI.Button(new Rect(offsetX, offsetY, 130, 30), "Metallic Map"))
        {
            _textureToAlign = _mainGui.MetallicMap;
            _doStuff = true;
        }

        GUI.enabled = _mainGui.SmoothnessMap != null;
        if (GUI.Button(new Rect(offsetX + 150, offsetY, 130, 30), "Smoothness Map"))
        {
            _textureToAlign = _mainGui.SmoothnessMap;
            _doStuff = true;
        }

        offsetY += 40;

        GUI.enabled = _mainGui.EdgeMap != null;
        if (GUI.Button(new Rect(offsetX, offsetY, 130, 30), "Edge Map"))
        {
            _textureToAlign = _mainGui.EdgeMap;
            _doStuff = true;
        }

        GUI.enabled = _mainGui.AoMap != null;
        if (GUI.Button(new Rect(offsetX + 150, offsetY, 130, 30), "AO Map"))
        {
            _textureToAlign = _mainGui.AoMap;
            _doStuff = true;
        }

        offsetY += 40;

        GUI.enabled = true;


        if (GuiHelper.Slider(new Rect(offsetX, offsetY, 280, 50), "Lens Distort Correction", _lensDistort,
            _lensDistortText, out _lensDistort, out _lensDistortText, -1.0f, 1.0f)) _doStuff = true;
        offsetY += 40;

        if (GuiHelper.Slider(new Rect(offsetX, offsetY, 280, 50), "Perspective Correction X", _perspectiveX,
            _perspectiveXText, out _perspectiveX, out _perspectiveXText, -5.0f, 5.0f)) _doStuff = true;
        offsetY += 40;

        if (GuiHelper.Slider(new Rect(offsetX, offsetY, 280, 50), "Perspective Correction Y", _perspectiveY,
            _perspectiveYText, out _perspectiveY, out _perspectiveYText, -5.0f, 5.0f)) _doStuff = true;
        offsetY += 50;

        if (GUI.Button(new Rect(offsetX, offsetY, 130, 30), "Reset Points"))
        {
            _pointTl = new Vector2(0.0f, 1.0f);
            _pointTr = new Vector2(1.0f, 1.0f);
            _pointBl = new Vector2(0.0f, 0.0f);
            _pointBr = new Vector2(1.0f, 0.0f);
        }


        if (GUI.Button(new Rect(offsetX + 150, offsetY, 130, 30), "Set All Maps")) StartCoroutine(SetMaps());


        GUI.DragWindow();
    }

    private void OnGUI()
    {
        _windowRect.width = 300;
        _windowRect.height = 430;

        _windowRect = GUI.Window(21, _windowRect, DoMyWindow, "Texture Alignment Adjuster");
    }

    private void ProcessMap(Texture2D textureTarget)
    {
        var width = textureTarget.width;
        var height = textureTarget.height;

        if (_lensMap == null)
            _lensMap = new RenderTexture(width, height, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
        if (_alignMap == null)
            _alignMap = new RenderTexture(width, height, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
        if (_perspectiveMap == null)
            _perspectiveMap =
                new RenderTexture(width, height, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);

        Graphics.Blit(textureTarget, _lensMap, _blitMaterial, 0);
        Graphics.Blit(_lensMap, _alignMap, _blitMaterial, 1);
        Graphics.Blit(_alignMap, _perspectiveMap, _blitMaterial, 2);
    }

    private Texture2D SetMap(Texture2D textureTarget)
    {
        var width = textureTarget.width;
        var height = textureTarget.height;

        CleanupTexture(_lensMap);
        CleanupTexture(_alignMap);
        CleanupTexture(_perspectiveMap);

        _lensMap = new RenderTexture(width, height, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
        _alignMap = new RenderTexture(width, height, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
        _perspectiveMap =
            new RenderTexture(width, height, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);

        Graphics.Blit(textureTarget, _lensMap, _blitMaterial, 0);
        Graphics.Blit(_lensMap, _alignMap, _blitMaterial, 1);
        Graphics.Blit(_alignMap, _perspectiveMap, _blitMaterial, 2);

        var replaceTexture = _textureToAlign == textureTarget;

        Destroy(textureTarget);
        // ReSharper disable once RedundantAssignment
        textureTarget = null;

        RenderTexture.active = _perspectiveMap;
        textureTarget = new Texture2D(width, height, TextureFormat.ARGB32, false, true);
        textureTarget.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        textureTarget.Apply();

        RenderTexture.active = null;

        CleanupTexture(_lensMap);
        CleanupTexture(_alignMap);
        CleanupTexture(_perspectiveMap);

        if (replaceTexture) _textureToAlign = textureTarget;

        _doStuff = true;

        return textureTarget;
    }

    private RenderTexture SetMap(RenderTexture textureTarget)
    {
        var width = textureTarget.width;
        var height = textureTarget.height;

        CleanupTexture(_lensMap);
        CleanupTexture(_alignMap);
        CleanupTexture(_perspectiveMap);

        _lensMap = new RenderTexture(width, height, 0, RenderTextureFormat.RHalf, RenderTextureReadWrite.Linear);
        _alignMap = new RenderTexture(width, height, 0, RenderTextureFormat.RHalf, RenderTextureReadWrite.Linear);
        _perspectiveMap = new RenderTexture(width, height, 0, RenderTextureFormat.RHalf, RenderTextureReadWrite.Linear);

        Graphics.Blit(textureTarget, _lensMap, _blitMaterial, 0);
        Graphics.Blit(_lensMap, _alignMap, _blitMaterial, 1);
        Graphics.Blit(_alignMap, _perspectiveMap, _blitMaterial, 2);

        if (textureTarget != null)
        {
            textureTarget.Release();
            textureTarget = null;
        }

        Graphics.Blit(_perspectiveMap, textureTarget);

        CleanupTexture(_lensMap);
        CleanupTexture(_alignMap);
        CleanupTexture(_perspectiveMap);

        _doStuff = true;

        return textureTarget;
    }

    private IEnumerator SetMaps()
    {
        if (_mainGui.HeightMap != null)
        {
            Debug.Log("Setting Height");
            _mainGui.HeightMap = SetMap(_mainGui.HeightMap);
        }

        if (_mainGui.HdHeightMap != null)
        {
            Debug.Log("Setting HD Height");
            _mainGui.HdHeightMap = SetMap(_mainGui.HdHeightMap);
        }

        yield return new WaitForSeconds(0.1f);

        if (_mainGui.DiffuseMap != null)
        {
            Debug.Log("Setting Diffuse");
            _mainGui.DiffuseMap = SetMap(_mainGui.DiffuseMap);
        }

        yield return new WaitForSeconds(0.1f);

        if (_mainGui.DiffuseMapOriginal != null)
        {
            Debug.Log("Setting Diffuse Original");
            _mainGui.DiffuseMapOriginal = SetMap(_mainGui.DiffuseMapOriginal);
        }

        yield return new WaitForSeconds(0.1f);

        if (_mainGui.NormalMap != null)
        {
            Debug.Log("Setting Normal");
            _mainGui.NormalMap = SetMap(_mainGui.NormalMap);
        }

        yield return new WaitForSeconds(0.1f);

        if (_mainGui.MetallicMap != null)
        {
            Debug.Log("Setting Metallic");
            _mainGui.MetallicMap = SetMap(_mainGui.MetallicMap);
        }

        yield return new WaitForSeconds(0.1f);

        if (_mainGui.SmoothnessMap != null)
        {
            Debug.Log("Setting Smoothness");
            _mainGui.SmoothnessMap = SetMap(_mainGui.SmoothnessMap);
        }

        yield return new WaitForSeconds(0.1f);

        if (_mainGui.EdgeMap != null)
        {
            Debug.Log("Setting Edge");
            _mainGui.EdgeMap = SetMap(_mainGui.EdgeMap);
        }

        yield return new WaitForSeconds(0.1f);

        if (_mainGui.AoMap != null)
        {
            Debug.Log("Setting AO");
            _mainGui.AoMap = SetMap(_mainGui.AoMap);
        }

        yield return new WaitForSeconds(0.1f);

        Resources.UnloadUnusedAssets();
    }
}
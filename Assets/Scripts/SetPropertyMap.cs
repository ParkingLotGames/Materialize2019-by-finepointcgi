using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// This script is responsible for setting the values for the different maps (Red, Blue, Green and Alpha) based on the selection of the user from the dropdown menus.
/// It also allows the user to set preset values for these maps by calling the SetPreset method
/// </summary>
public class SetPropertyMap : MonoBehaviour
{
    /// <summary>
    /// The type of the map that is being set by the script
    /// </summary>
    public string DropDownType;
    /// <summary>
    /// Reference to the dropdown menu for the Red map
    /// </summary>
    public Dropdown RedMap;
    /// <summary>
    /// Reference to the dropdown menu for the Blue map
    /// </summary>
    public Dropdown BlueMap;
    /// <summary>
    /// Reference to the dropdown menu for the Green map
    /// </summary>
    public Dropdown GreenMap;
    /// <summary>
    /// Reference to the dropdown menu for the Alpha map
    /// </summary>
    public Dropdown AlphaMap;

    /// <summary>
    /// Sets the selection of the given map type to the selection provided. 
    /// It calls the MapSelection method of the MainGui script
    /// </summary>
    public void SetPropertyMapSelection(int selection)
    {
        MainGui.Instance.MapSelection(selection, DropDownType);
    }

    /// <summary>
    /// Sets the preset values for the different maps based on the selection provided
    /// </summary>
    public void SetPreset(int selection)
    {
        switch (selection)
        {
            case 0:
                break;
            case 1:
                setChannels(1, 0, 4, 2);
                break;
            case 2:
                setChannels(2, 4, 1, 0);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Helper method to set the values for the different maps
    /// </summary>
    private void setChannels(int red, int blue, int green, int alpha)
    {
        RedMap.value = red;
        BlueMap.value = blue;
        GreenMap.value = green;
        AlphaMap.value = alpha;
        //RedMap.SetPropertyMapSelection(red);
        //BlueMap.SetPropertyMapSelection(blue);
        //GreenMap.SetPropertyMapSelection(green);
        //AlphaMap.SetPropertyMapSelection(alpha);
    }
}
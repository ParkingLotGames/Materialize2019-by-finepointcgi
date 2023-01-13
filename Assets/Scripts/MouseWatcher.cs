using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
///  MouseWatcher class is a script that implements the IPointerEnterHandler and IPointerExitHandler interfaces to detect when the mouse pointer enters or exits the object this script is attached to.
/// </summary>
public class MouseWatcher : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    /// <summary>
    /// This function is called when the mouse pointer enters the object this script is attached to.
    /// </summary>
    /// <param name="eventData">Data of the pointer event</param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        this.gameObject.SetActive(true); // Enable the gameObject
    }

    /// <summary>
    /// This function is called when the mouse pointer exits the object this script is attached to.
    /// </summary>
    /// <param name="eventData">Data of the pointer event</param>
    public void OnPointerExit(PointerEventData eventData)
    {
        this.gameObject.SetActive(false); // Disable the gameObject
    }
}

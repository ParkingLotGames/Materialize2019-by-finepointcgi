using UnityEngine;

public class FlipUIElement : MonoBehaviour
{
    public void FlipUiElement(GameObject Obj)
    {
        Obj.SetActive(!Obj.activeSelf);
    }
}

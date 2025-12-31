using UnityEngine.EventSystems;
using UnityEngine;

public class UIRaycastCatcher : MonoBehaviour, IPointerDownHandler
{
  public void OnPointerDown(PointerEventData eventData)
  {
    UIBlocker.ClickedUI = true;
  }
}

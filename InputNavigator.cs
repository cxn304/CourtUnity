using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 挂在userInput和passInput上
/// </summary>

public class InputNavigator : MonoBehaviour, ISelectHandler, IDeselectHandler

{
    EventSystem system;
    private bool _isSelect = false;
    void Start()
    {
        system = EventSystem.current;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && _isSelect)
        {
            Selectable next = null;
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnUp();
                if (next == null) next = system.lastSelectedGameObject.GetComponent<Selectable>();
            }
            else
            {
                next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();
                if (next == null) next = system.firstSelectedGameObject.GetComponent<Selectable>();
            }
            if (next != null)
            {
                InputField inputfield = next.GetComponent<InputField>();
                system.SetSelectedGameObject(next.gameObject, new BaseEventData(system));
            }
        }
    }

    public void OnSelect(BaseEventData eventData)

    {
        _isSelect = true;
    }

    public void OnDeselect(BaseEventData eventData)

    {
        _isSelect = false;
    }
}

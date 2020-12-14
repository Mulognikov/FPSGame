using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    [SerializeField] private Transform lookPoint;

    public delegate void LookToActionObjectHandler(string actionText);
    public static event LookToActionObjectHandler LookToActionObjectEvent;

    private RaycastHit hit;
    private IAction actionObject;
    private bool previousFrameAction = false;

    void Update()
    {
        CheckForActionObject();
    }

    private void CheckForActionObject()
    {
        if (Physics.Linecast(lookPoint.position, lookPoint.position + lookPoint.forward * 3, out hit))
        {
            if (hit.transform.CompareTag("Action"))
            {
                if (!previousFrameAction)
                {
                    actionObject = hit.transform.gameObject.GetComponent<IAction>();

                    if (actionObject.ActionText == "NO")
                    {
                        ClearActionText();
                        return;
                    }
                }

                if (actionObject.ActionText == "NO")
                {
                    ClearActionText();
                    return;
                }

                previousFrameAction = true;
                LookToActionObjectEvent?.Invoke(actionObject.ActionText);
                CheckForActionButton();
            }
            else
            {
                ClearActionText();
            }
        }
        else
        {
            ClearActionText();
        }
    }

    private void ClearActionText()
    {
        if (previousFrameAction)
        {
            LookToActionObjectEvent?.Invoke("");
        }

        previousFrameAction = false;
    }

    private void CheckForActionButton()
    {
        if (Input.GetKeyDown(PlayerInput.Action) || Input.GetKeyDown(KeyCode.Joystick1Button0))
        {
            actionObject.Action();
        }
    }
}

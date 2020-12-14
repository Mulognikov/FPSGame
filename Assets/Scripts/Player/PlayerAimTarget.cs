using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimTarget : MonoBehaviour
{
    [SerializeField] private Transform lookPoint;

    void Update()
    {
        SetPositionToLookTarget();
    }

    private void SetPositionToLookTarget()
    {
        RaycastHit hit;

        if (Physics.Raycast(lookPoint.transform.position,lookPoint.rotation.eulerAngles, out hit, 50f))
        {
            Debug.DrawRay(lookPoint.transform.position, lookPoint.forward, Color.green);
            transform.position = hit.point;
        }
    }
}

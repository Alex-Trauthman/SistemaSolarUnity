using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;

public class ClickObject : MonoBehaviour
{
    public GameObject objectPrefab;

    private Camera vuforiaCamera;

    void Start()
    {
        vuforiaCamera = VuforiaBehaviour.Instance.GetComponent<Camera>();
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = vuforiaCamera.ScreenPointToRay(touch.position);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform == transform)
                    {
                        CreateObjectClone(hit.transform.gameObject);
                    }
                }
            }
        }
    }

    void CreateObjectClone(GameObject originalObject)
    {
        GameObject clone = Instantiate(objectPrefab);

        Vector3 positionInFrontOfCamera = vuforiaCamera.transform.position + vuforiaCamera.transform.forward * 100f;

        clone.transform.position = positionInFrontOfCamera;

        clone.transform.LookAt(vuforiaCamera.transform);

        Destroy(clone.GetComponent<ClickObject>());
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailRendererConfig : MonoBehaviour
{
    public Transform trailRendererTarget;
    public Transform referenceObject;

    private Vector3 lastPosition;

    void Start()
    {
        if (trailRendererTarget == null || referenceObject == null)
        {
            Debug.LogError("Trail Renderer target or Reference Object is not assigned!");
            return;
        }

        lastPosition = referenceObject.InverseTransformPoint(trailRendererTarget.position);
    }

    void Update()
    {
        Vector3 currentPosition = referenceObject.InverseTransformPoint(trailRendererTarget.position);

        trailRendererTarget.position = referenceObject.TransformPoint(currentPosition);

        lastPosition = currentPosition;
    }
}

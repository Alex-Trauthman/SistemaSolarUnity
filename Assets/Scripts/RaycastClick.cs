using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastClick : MonoBehaviour
{
    public float distanciaMaxima = 100f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, distanciaMaxima))
            {
                if (hit.transform.CompareTag("Planet"))
                {
                    Renderer objetoRenderer = hit.transform.GetComponent<Renderer>();
                    if (objetoRenderer != null)
                    {
                        // objetoRenderer.material.color = Color.red;
                    }
                }
            }
        }
    }
}

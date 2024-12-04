using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlterarCor : MonoBehaviour
{
    private Renderer objetoRenderer;
    public Color novaCor = Color.red;

    void Start()
    {
        objetoRenderer = GetComponent<Renderer>();

        objetoRenderer.material.color = novaCor;
    }
}

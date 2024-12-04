using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoverObjeto : MonoBehaviour
{
    private Camera cameraPrincipal;       
    private GameObject objetoSelecionado; 
    private float distanciaDoObjeto;    
    private bool movendoObjeto = false;   

    void Start()
    {
        cameraPrincipal = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SelecionarObjeto();
        }

        if (Input.GetMouseButtonUp(0) && movendoObjeto)
        {
            SoltarObjeto();
        }

        if (movendoObjeto && objetoSelecionado != null)
        {
            MoverObjetoComMouse();
        }
    }

    void SelecionarObjeto()
    {
        Ray ray = cameraPrincipal.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider != null)
            {
                objetoSelecionado = hit.collider.gameObject;
                distanciaDoObjeto = Vector3.Distance(cameraPrincipal.transform.position, objetoSelecionado.transform.position);
                movendoObjeto = true;
            }
        }
    }
    void MoverObjetoComMouse()
    {
        Ray ray = cameraPrincipal.ScreenPointToRay(Input.mousePosition);

        Vector3 novaPosicao = ray.GetPoint(distanciaDoObjeto);

        objetoSelecionado.transform.position = novaPosicao;
    }

    void SoltarObjeto()
    {
        objetoSelecionado = null;
        movendoObjeto = false;
    }
}

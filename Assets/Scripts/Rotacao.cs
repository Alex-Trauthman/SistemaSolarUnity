using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotacao : MonoBehaviour
{
    public Transform centro;

    public float raio = 5f;

    public float periodoOrbital = 365f;

    private float velocidadeAngular;

    public float anguloAtual = 0f;

    void Start()
    {
        velocidadeAngular = (360f / periodoOrbital) * 30;
    }

    void Update()
    {
        if (PauseManager.isPaused)
            return; // Sai do Update se estiver pausado

        anguloAtual += (velocidadeAngular * Time.deltaTime)/2;

        float anguloRad = anguloAtual * Mathf.Deg2Rad;

        float x = centro.position.x + Mathf.Cos(anguloRad) * raio;
        float z = centro.position.z + Mathf.Sin(anguloRad) * raio;

        transform.position = new Vector3(x, centro.position.y, z); 
    }
}

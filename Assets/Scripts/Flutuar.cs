using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flutuar : MonoBehaviour
{
    // Variáveis públicas para controlar o movimento de flutuar
    public float floatAmplitude = 0.5f; // Amplitude do movimento de flutuar
    public float floatSpeed = 2f; // Velocidade do movimento de flutuar

    // Variáveis públicas para controlar a rotação
    public float rotationSpeedX = 30f; // Velocidade de rotação no eixo X
    public float rotationSpeedY = 30f; // Velocidade de rotação no eixo Y
    public float rotationSpeedZ = 30f; // Velocidade de rotação no eixo Z

    // Variável privada para controlar a posição inicial
    private Vector3 startPosition;

    void Start()
    {
        // Armazena a posição inicial para o movimento de flutuar
        startPosition = transform.position;
    }

    void Update()
    {
        // Movimento de flutuar: Faz o objeto subir e descer de acordo com o seno
        float newY = Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        transform.position = new Vector3(startPosition.x, startPosition.y + newY, startPosition.z);

        // Movimento de rotação: Rotaciona o objeto continuamente nos eixos X, Y, Z
        float rotationX = rotationSpeedX * Time.deltaTime;
        float rotationY = rotationSpeedY * Time.deltaTime;
        float rotationZ = rotationSpeedZ * Time.deltaTime;

        transform.Rotate(rotationX, rotationY, rotationZ);
    }
}

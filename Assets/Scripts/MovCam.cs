using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovCam : MonoBehaviour
{
    public float smoothSpeed = 0.125f; // Suavidade do movimento
    public Vector2 offset; // Pequeno deslocamento
    private float fixedZ; // Posição fixa no eixo Z

    public GameObject[] ilhas;

    void Start()
    {
        fixedZ = transform.position.z; // Mantém o Z da câmera
    }

    void LateUpdate()
    {
        if (ilhas.Length > 0 && ProgessManager.Instance != null)
        {
            int currentIndex = (ProgessManager.Instance.currentIsland / 2);

            if (currentIndex >= 0 && currentIndex < ilhas.Length)
            {
                Vector3 targetPosition = ilhas[currentIndex].transform.position;
                Vector3 desiredPosition = new Vector3(targetPosition.x + offset.x, targetPosition.y + offset.y, fixedZ);
                transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            }
        }
    }

    void Update()
    {
        float floatAmount = Mathf.Sin(Time.time * 2f) * 0.9f;
        transform.position += new Vector3(0, floatAmount * Time.deltaTime, 0);
    }
}

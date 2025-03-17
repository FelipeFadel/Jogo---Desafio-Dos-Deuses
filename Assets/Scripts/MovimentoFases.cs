using UnityEngine;

public class MovimentoFases : MonoBehaviour
{
    public GameObject[] pontos;
    private int indiceAtual = 0;
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
        if (pontos.Length > 0)
        {
            MoverCameraParaPonto(indiceAtual);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            indiceAtual = (indiceAtual + 1) % pontos.Length;
            MoverCameraParaPonto(indiceAtual);
        }
    }

    void MoverCameraParaPonto(int indice)
    {
        if (pontos[indice] != null)
        {
            cam.transform.position = pontos[indice].transform.position;
            cam.transform.rotation = pontos[indice].transform.rotation;
        }
    }
}

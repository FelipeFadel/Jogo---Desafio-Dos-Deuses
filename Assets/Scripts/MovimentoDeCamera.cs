using UnityEngine;

public class MovimentoDeCamera : MonoBehaviour
{
    public Transform cameraTransform; // Transform da câmera
    public float focusSpeed = 0.02f; // Velocidade da transição
    public float maxMoveDistance = 0.5f; // Máximo deslocamento permitido em X e Y

    private Vector3 defaultPosition; // Posição original da câmera
    private Quaternion defaultRotation; // Rotação original da câmera
    private Vector3 targetPosition;
    private Quaternion targetRotation;

    void Start()
    {
        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;

        defaultPosition = cameraTransform.position;
        defaultRotation = cameraTransform.rotation;
        targetPosition = defaultPosition;
        targetRotation = defaultRotation;
    }

    void Update()
    {
        // Movimenta a câmera suavemente para a posição alvo
        cameraTransform.position = Vector3.Lerp(cameraTransform.position, targetPosition, Time.deltaTime * focusSpeed);
        cameraTransform.rotation = Quaternion.Lerp(cameraTransform.rotation, targetRotation, Time.deltaTime * focusSpeed);
    }

    public void FocusOnHexagon(Transform hexTransform)
    {
        if (hexTransform != null)
        {
            // Calcula o deslocamento da câmera baseado na posição do hexágono
            Vector3 displacement = hexTransform.position - defaultPosition;

            // Limita o deslocamento para que X e Y não ultrapassem 0.5f
            displacement.x = Mathf.Clamp(displacement.x, -maxMoveDistance, maxMoveDistance);
            displacement.y = Mathf.Clamp(displacement.y, -maxMoveDistance, maxMoveDistance);

            // Define a nova posição da câmera, mantendo Z fixo
            targetPosition = new Vector3(defaultPosition.x + displacement.x, defaultPosition.y + displacement.y, defaultPosition.z);

            // Faz a câmera olhar para o hexágono
            Vector3 lookDirection = (hexTransform.position - cameraTransform.position).normalized;
            targetRotation = Quaternion.LookRotation(lookDirection);
        }
    }

    public void ResetCamera()
    {
        targetPosition = defaultPosition;
        targetRotation = defaultRotation;
    }
}

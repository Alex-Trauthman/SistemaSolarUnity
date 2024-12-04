using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Movimento : MonoBehaviour
{
    [Header("Configurações de Movimento")]
    public float velocidadeMovimento = 10f; // Velocidade do movimento
    public float sensibilidadeMouse = 100f; // Sensibilidade para rotação
    public float velocidadeVertical = 5f; // Velocidade do movimento vertical

    [Header("Limites de Rotação")]
    public float limiteVertical = 85f; // Limite para a rotação vertical

    private float rotacaoVertical = 0f; // Acumula a rotação no eixo X
    private Transform cameraTransform; // Referência à câmera
    private CharacterController characterController; // Controle de personagem

    void Start()
    {
        // Esconde o cursor e o bloqueia no centro da tela
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Busca os componentes necessários
        cameraTransform = GetComponentInChildren<Camera>().transform;
        characterController = GetComponent<CharacterController>();

        if (cameraTransform == null)
            Debug.LogError("Câmera não encontrada!");
        if (characterController == null)
            Debug.LogError("CharacterController não encontrado!");
    }

    void Update()
    {
        if (PauseManager.isPaused)
        {
            // Mostra o cursor e desbloqueia o movimento enquanto o jogo está pausado
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            return;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;

        // Movimento com base na orientação da câmera
        float movimentoX = Input.GetAxis("Horizontal") * velocidadeMovimento;
        float movimentoZ = Input.GetAxis("Vertical") * velocidadeMovimento;
        float movimentoY = 0f;

        // Controle de movimento vertical
        if (Input.GetKey(KeyCode.E))
        {
            movimentoY = velocidadeVertical;
        }
        else if (Input.GetKey(KeyCode.Q))
        {
            movimentoY = -velocidadeVertical;
        }

        // Calcula a direção de movimento com base na câmera
        Vector3 forward = cameraTransform.forward; // Direção para frente da câmera
        Vector3 right = cameraTransform.right;     // Direção para a direita da câmera

        // Remove o componente vertical do "forward" e "right" para evitar desbalanceamento
        forward.y = 0f;
        forward.Normalize();
        right.Normalize();

        // Combina os vetores de direção
        Vector3 movimento = (forward * movimentoZ + right * movimentoX + Vector3.up * movimentoY);

        // Aplica o movimento no personagem
        characterController.Move(movimento * Time.deltaTime);

        // Rotação do mouse
        float mouseX = Input.GetAxis("Mouse X") * sensibilidadeMouse * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensibilidadeMouse * Time.deltaTime;

        rotacaoVertical -= mouseY;
        rotacaoVertical = Mathf.Clamp(rotacaoVertical, -limiteVertical, limiteVertical);

        cameraTransform.localRotation = Quaternion.Euler(rotacaoVertical, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }
}


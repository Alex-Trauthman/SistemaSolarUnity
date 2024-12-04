using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlanetClickHandler : MonoBehaviour
{
    public TextMeshProUGUI infoText;    // Texto de informações no Canvas usando TextMeshPro
    public GameObject panel;            // Painel de informações
    public float cameraDistance = 20f;   // Distância da câmera ao planeta

    private PlanetInfo currentPlanetInfo; // Referência ao script do planeta clicado
    private Vector3 originalPlayerPosition; // Posição original do jogador
    private Quaternion originalPlayerRotation; // Rotação original do jogador
    private Transform playerTransform; // Transform do jogador
    private Camera mainCamera; // Câmera principal
    private bool isViewingPlanet = false; // Indica se está visualizando um planeta
    private List<Renderer> hiddenRenderers = new List<Renderer>(); // Renderizadores ocultos
    private Coroutine typingCoroutine; // Referência à coroutine de digitação
    private bool isTyping = false;

    void Start()
    {
        // Configurações iniciais
        panel.SetActive(false); // Esconde o painel no início
        mainCamera = Camera.main; // Atribui a câmera principal
        if (mainCamera == null)
        {
            Debug.LogError("Câmera principal não encontrada.");
            return;
        }

        playerTransform = mainCamera.transform.parent; // Assume que o jogador é o pai da câmera
        if (playerTransform == null)
        {
            Debug.LogError("Transform do jogador não encontrado! Certifique-se de que a câmera é filha do jogador.");
            return;
        }

        originalPlayerPosition = playerTransform.position;
        originalPlayerRotation = playerTransform.rotation;
    }

    void Update()
    {
        // Clique com o botão esquerdo do mouse
        if (Input.GetMouseButtonDown(0) && !isViewingPlanet)
        {
            HandlePlanetClick();
        }

        // Pressionar ESC para voltar à visão padrão
        if (Input.GetKeyDown(KeyCode.Escape) && isViewingPlanet)
        {
            ResetPlayerAndUI();
        }
    }

    private void HandlePlanetClick()
    {
       
        if (isTyping || isViewingPlanet) return; // Ignora se já está digitando ou visualizando um planeta
        RaycastHit hit;
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.CompareTag("Planet"))
            {
                currentPlanetInfo = hit.transform.GetComponent<PlanetInfo>();
                if (currentPlanetInfo == null)
                {
                    Debug.LogError("PlanetInfo não encontrado no planeta clicado.");
                    return;
                }

                playerTransform.position = new Vector3(playerTransform.position.x, 404f, playerTransform.position.z);
                panel.SetActive(true);

                // Interrompe corrotinas anteriores para evitar acúmulo
                if (typingCoroutine != null)
                {
                    StopCoroutine(typingCoroutine);
                }

                typingCoroutine = StartCoroutine(TypeText(currentPlanetInfo.planetDescription));

                PauseManager.isPaused = true;
                Time.timeScale = 0f;
                MoveCameraToPlanet(hit.transform);
                HideOtherObjects(hit.transform);

                isViewingPlanet = true; // Bloqueia cliques subsequentes
            }
        }
    }

    private IEnumerator TypeText(string text)
    {
        infoText.text = text;
        infoText.maxVisibleCharacters = 0;
        isTyping = true; // Inicia a digitação

        for (int i = 0; i <= text.Length; i++)
        {
            infoText.maxVisibleCharacters = i;
            yield return new WaitForSecondsRealtime(0.03f); // Ajuste o tempo de espera para controlar a velocidade da digitação
        }

        isTyping = false; // Finaliza a digitação
    }

    private void ResetPlayerAndUI()
    {
        // Reseta o jogador para a posição original
        playerTransform.position = originalPlayerPosition;
        playerTransform.rotation = originalPlayerRotation;

        // Reseta a UI
        panel.SetActive(false);
        infoText.text = "";

        // Mostra todos os objetos ocultos
        ShowHiddenObjects();

        // Retoma o jogo
        PauseManager.isPaused = false;
        Time.timeScale = 1f;

        isViewingPlanet = false;
    }

    private void MoveCameraToPlanet(Transform planet)
    {
        // Obtém o Collider do planeta para calcular o tamanho
        Collider planetCollider = planet.GetComponent<Collider>();
        if (planetCollider == null)
        {
            Debug.LogError("Collider não encontrado no planeta.");
            return;
        }

        // Calcula o raio do planeta com base no maior eixo do Collider
        float planetRadius = Mathf.Max(planetCollider.bounds.extents.x, planetCollider.bounds.extents.y, planetCollider.bounds.extents.z);

        // Calcula a direção do offset com base na posição atual da câmera em relação ao planeta
        Vector3 offsetDirection = (mainCamera.transform.position - planet.position).normalized;

        // Ajusta a distância da câmera com base no raio do planeta
        float adjustedCameraDistance = cameraDistance + planetRadius;

        // Calcula a nova posição da câmera com base no centro do planeta e na distância ajustada
        Vector3 newCameraPosition = planet.position + offsetDirection * adjustedCameraDistance;

        // Atualiza a posição da câmera (não mexe no jogador)
        playerTransform.position = newCameraPosition;

        // Faz com que a câmera olhe para o centro do planeta
        playerTransform.LookAt(planet.position);
    }

    private void HideOtherObjects(Transform clickedPlanet)
    {
        Renderer[] allRenderers = FindObjectsByType<Renderer>(FindObjectsSortMode.None);
        foreach (Renderer renderer in allRenderers)
        {
            if (renderer.transform != clickedPlanet && renderer.transform != panel.transform)
            {
                renderer.enabled = false;
                hiddenRenderers.Add(renderer);
            }
        }
    }

    private void ShowHiddenObjects()
    {
        foreach (Renderer renderer in hiddenRenderers)
        {
            renderer.enabled = true;
        }
        hiddenRenderers.Clear();
    }
}

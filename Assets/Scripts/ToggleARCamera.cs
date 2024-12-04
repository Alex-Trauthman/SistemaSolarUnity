using UnityEngine;

public class ToggleARCamera : MonoBehaviour {
    public GameObject arCamera;

    void Start() {
        SetARCameraState(false); // Desativa a AR Camera ao iniciar
    }

    public void SetARCameraState(bool isActive) {
        arCamera.SetActive(isActive);
    }
}

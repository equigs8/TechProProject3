using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Header("Cameras")]
    [Tooltip("Drag your second Camera here. The script will automatically disable it on start and use its position.")]
    public Camera waveCamera;

    [Header("Settings")]
    public float transitionSpeed = 3f;

    // We will automatically store the Main Camera's starting position here
    private Vector3 buildPosition;
    private Quaternion buildRotation;

    void Start()
    {
        // 1. Record this camera's initial position to use as the Building Phase view
        buildPosition = transform.position;
        buildRotation = transform.rotation;

        // 2. Automatically disable the second camera so it doesn't render over the main one
        if (waveCamera != null)
        {
            waveCamera.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (GameManager.instance == null) return;

        Vector3 targetPos = transform.position;
        Quaternion targetRot = transform.rotation;

        // Determine where the camera should be aiming
        if (GameManager.instance.gameState == GameManager.GameState.BuildingPhase)
        {
            targetPos = buildPosition;
            targetRot = buildRotation;
        }
        else if (GameManager.instance.gameState == GameManager.GameState.InWave && waveCamera != null)
        {
            targetPos = waveCamera.transform.position;
            targetRot = waveCamera.transform.rotation;
        }

        // Smoothly glide to the target position and rotation
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * transitionSpeed);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, Time.deltaTime * transitionSpeed);
    }
}
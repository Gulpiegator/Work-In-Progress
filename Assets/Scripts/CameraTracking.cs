using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTracking : MonoBehaviour
{
    public Transform player;

    // Offsets and rotations for each stage
    public Vector3 stageOneOffset;
    public Vector3 stageFiveOffset;
    public Vector3 stageSixOffset;
    public Vector3 stageOneRotation;
    public Vector3 stageFiveRotation;
    public Vector3 stageSixRotation;

    public float lerpSpeed = 5f;
    public float wiggleRoomX;
    public float wiggleRoomY;

    private Vector3 correctedPos;
    private Vector3 targetOffset;
    private Quaternion targetRotation;
    private int currentStage;

    private Camera mainCamera; // Reference to the Camera component

    void Start(){
        mainCamera = GetComponent<Camera>();
        if (mainCamera == null){
            Debug.LogError("CameraTracking script must be attached to a GameObject with a Camera component!");
            return;
        }

        // Initialize to stage one settings
        currentStage = player.GetComponent<PlayerMovement>().stage; // Example component
        StageChange(currentStage);

        // Set initial camera position and rotation
        correctedPos = transform.position;
        transform.position = player.position + targetOffset;
        transform.rotation = targetRotation;
    }

    void Update(){
        int newStage = player.GetComponent<PlayerMovement>().stage;
        if (newStage != currentStage){
            currentStage = newStage;
            StageChange(currentStage);
        }

        correctedPos = transform.position;

        if (currentStage == 5)
        {
            // Use wiggleRoomX as wiggleRoomZ for Stage 6
            if (transform.position.z - player.position.z < -wiggleRoomX)
                correctedPos.z = player.position.z - wiggleRoomX;
            else if (transform.position.z - player.position.z > wiggleRoomX)
                correctedPos.z = player.position.z + wiggleRoomX;
        }
        // Default behavior for X
        if (transform.position.x - player.position.x < -wiggleRoomX)
            correctedPos.x = player.position.x - wiggleRoomX;
        else if (transform.position.x - player.position.x > wiggleRoomX)
            correctedPos.x = player.position.x + wiggleRoomX;
        

        // Check for Y adjust
        if (transform.position.y - player.position.y < -wiggleRoomY)
            correctedPos.y = player.position.y - wiggleRoomY;
        else if (transform.position.y - player.position.y > wiggleRoomY)
            correctedPos.y = player.position.y + wiggleRoomY;

        // Keep Z
        if (currentStage != 5)
            correctedPos.z = 8;

        // Smoothly shift position/rotations
        transform.position = Vector3.Lerp(transform.position, correctedPos + targetOffset, Time.deltaTime * lerpSpeed);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * lerpSpeed);
    }

    void StageChange(int stage){
        switch (stage){
            case 1:
                targetOffset = stageOneOffset;
                targetRotation = Quaternion.Euler(stageOneRotation);
                if (!mainCamera.orthographic) // Switch to orthographic if not already
                {
                    mainCamera.orthographic = true;
                    Debug.Log("Switched to Orthographic for Stage 1");
                }
                break;
            case 4:
                targetOffset = stageFiveOffset;
                targetRotation = Quaternion.Euler(stageFiveRotation);
                if (mainCamera.orthographic) // Switch to perspective if currently orthographic
                {
                    mainCamera.orthographic = false;
                    Debug.Log("Switched to Perspective for Stage 5");
                }
                break;
            case 5:
                targetOffset = stageSixOffset;
                targetRotation = Quaternion.Euler(stageSixRotation);
                if (mainCamera.orthographic) // Switch to perspective if currently orthographic
                {
                    mainCamera.orthographic = false;
                    Debug.Log("Switched to Perspective for Stage 6");
                }
                break;
            default:
                targetOffset = stageOneOffset; // Default to stage one if the stage is unknown
                targetRotation = Quaternion.Euler(stageOneRotation);
                if (!mainCamera.orthographic) // Ensure default is orthographic
                {
                    mainCamera.orthographic = true;
                    Debug.Log("Switched to Orthographic for Default Stage");
                }
                break;
        }
    }
}

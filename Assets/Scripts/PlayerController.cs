using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public enum DriftDirection { LEFT, RIGHT };

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour {

    [Header("Components")]
    public CinemachineVirtualCamera virtualCam;
    GameManager gameManager;
    Rigidbody2D rigidbody2D;
    CinemachineBasicMultiChannelPerlin virtualCamNoise;

    [Header("Driving")]
    public float accelerationSpeed;
    public float maxDrivingSpeed;
    public float turningSpeed;
    public float movementSpeedForMaxTurnSpeed;

    [Header("Drifting")]
    bool isDrifting;
    DriftDirection driftDir;

    [Header("Camera")]
    public float explodeCooldownRate;

    void Awake() {
        gameManager = GameManager.instance;
        rigidbody2D = GetComponent<Rigidbody2D>();
        virtualCamNoise = virtualCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    void FixedUpdate() {
        // Capture inputs (x = left/right, y = up/down)
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        // Initiate drifting if turning into a direction and press Drift button
        if (Input.GetButtonDown("Drift") && input.x != 0 && !isDrifting) {
            driftDir = input.x < 0 ? DriftDirection.LEFT : DriftDirection.RIGHT;
        }

        if (!isDrifting) {
            // Accelerate and clamp speed normally if not drifting

            // Forward/backward
            rigidbody2D.AddForce(transform.up * input.y * accelerationSpeed);

            // Rotate with speed and movement magnitude (not moving = no rotate)
            transform.Rotate(0, 0, 
                -input.x * (input.y == 0 ? 1 : input.y) * turningSpeed * (
                    movementSpeedForMaxTurnSpeed == 0 ? 1 : Mathf.Clamp(
                    rigidbody2D.velocity.magnitude,
                    -movementSpeedForMaxTurnSpeed,
                    movementSpeedForMaxTurnSpeed)/movementSpeedForMaxTurnSpeed)
            );

            rigidbody2D.velocity = Vector3.ClampMagnitude(rigidbody2D.velocity, maxDrivingSpeed);
        } else {
            // Force velocity to be constant and direct car into the drift direction
        }

    }

    void Update() {
        // Take the shake down by a given rate every frame (decremental)
        if (virtualCamNoise.m_AmplitudeGain > 0) {
            virtualCamNoise.m_AmplitudeGain -= explodeCooldownRate * Time.deltaTime;
        }
    }

    void LateUpdate() {
        virtualCam.transform.rotation = transform.rotation;
    }
}

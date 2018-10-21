using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public enum DriftDirection { LEFT, RIGHT };

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{

    [Header("Components")]
    public CinemachineVirtualCamera virtualCam;
    public SpriteRenderer sprite;
    CinemachineBasicMultiChannelPerlin virtualCamNoise;
    GameManager gameManager;
    Rigidbody2D rigidbody2D;
    public TrailRenderer[] trails;
    public ParticleSystem sparks;

    [Header("Driving")]
    public float accelerationSpeed;
    public float maxDrivingSpeed;
    public float turningSpeed;
    public float movementSpeedForMaxTurnSpeed;

    [Header("Drifting")]
    public float driftSpeed;
    public float driftAutoTurnSpeed;
    public float driftTurningInfluence;
    public float driftRotationAngle;
    public float driftRotationSpriteRotFactor;
    bool isDrifting;
    DriftDirection driftDir;

    [Header("Camera")]
    public float explodeCooldownRate;
    public float camRotationLerp;

    void Awake()
    {
        gameManager = GameManager.instance;
        rigidbody2D = GetComponent<Rigidbody2D>();
        virtualCamNoise = virtualCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        // sparks = this.transform.Find("Sparks").gameObject;
    }

    void FixedUpdate()
    {
        // Capture inputs (x = left/right, y = up/down)
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        float amtForward = Vector3.Dot(rigidbody2D.velocity, transform.up);

        // Initiate drifting if turning into a direction and press Drift button and moving forward
        if (Input.GetButtonDown("Drift") && input.x != 0 && !isDrifting && amtForward > 0)
        {
            isDrifting = true;
            driftDir = input.x < 0 ? DriftDirection.LEFT : DriftDirection.RIGHT;

            sprite.gameObject.transform.localRotation = Quaternion.identity;

            transform.Rotate(0, 0, -input.x * driftRotationAngle);
            sprite.gameObject.transform.Rotate(0, 0, input.x * driftRotationAngle);
            // Adds trail when drifting
            foreach (TrailRenderer trail in trails) {
                trail.emitting = true;
            }
        }
        if (!isDrifting)
        {
            //Stop emitter
            sparks.Stop();

            // Accelerate and clamp speed normally if not drifting

            // Forward/backward
            rigidbody2D.AddForce(transform.up * input.y * accelerationSpeed);

            // Rotate with speed and movement magnitude (not moving = no rotate)
            transform.Rotate(0, 0,
                -input.x * (input.y == 0 ? 1 : input.y) * turningSpeed * (
                    movementSpeedForMaxTurnSpeed == 0 ? 1 : Mathf.Clamp(
                    rigidbody2D.velocity.magnitude,
                    -movementSpeedForMaxTurnSpeed,
                    movementSpeedForMaxTurnSpeed) / movementSpeedForMaxTurnSpeed)
            );

            rigidbody2D.velocity = Vector3.ClampMagnitude(rigidbody2D.velocity, maxDrivingSpeed);

            // Rotate sprite back to original if we were out of place
            sprite.gameObject.transform.localRotation = Quaternion.Lerp(
                sprite.gameObject.transform.localRotation, Quaternion.identity,
                driftRotationSpriteRotFactor);
            //Turns off trail when not drifting
            foreach (TrailRenderer trail in trails) {
                trail.emitting = false;
            }
        }
        else
        {
            //Start particles
            sparks.Play();

            // Kill drift if we let go of Drift key
            if (!Input.GetButton("Drift"))
            {
                isDrifting = false;
            }

            // Sprite during drift rotates a bit further than direction
            sprite.gameObject.transform.localRotation = Quaternion.Lerp(
                sprite.gameObject.transform.localRotation,
                Quaternion.Euler(
                    0, 0,
                    (driftDir == DriftDirection.LEFT ? 1 : -1) * driftRotationAngle * 0.5f),
                driftRotationSpriteRotFactor
            );
            // sprite.gameObject.transform.localEulerAngles = new Vector3(
            //     0, 0, Mathf.Lerp(
            //             sprite.gameObject.transform.localEulerAngles.z,
            //             (driftDir == DriftDirection.RIGHT ? 1 : -1) * driftRotationAngle * 0.5f,
            //             driftRotationSpriteRotFactor)
            // );

            // Force velocity to be constant and direct car into the drift direction
            float turnAmt = (driftDir == DriftDirection.LEFT ? 1 : -1) * driftAutoTurnSpeed;
            turnAmt += -input.x * driftTurningInfluence;

            Vector3 vel = rigidbody2D.velocity;

            vel = Vec2Rotate(vel.normalized,
                turnAmt);
            vel.Scale(Vector2.one * driftSpeed);

            rigidbody2D.velocity = vel;

            transform.Rotate(0, 0, turnAmt);
        }
    }

    void Update()
    {
        // Take the shake down by a given rate every frame (decremental)
        if (virtualCamNoise.m_AmplitudeGain > 0)
        {
            virtualCamNoise.m_AmplitudeGain -= explodeCooldownRate * Time.deltaTime;
        }
    }

    void LateUpdate()
    {
        virtualCam.transform.rotation = Quaternion.Lerp(
            virtualCam.transform.rotation, transform.rotation, camRotationLerp);
    }

    public static Vector2 Vec2Rotate(Vector2 v, float degrees)
    {
        float radians = degrees * Mathf.Deg2Rad;
        float sin = Mathf.Sin(radians);
        float cos = Mathf.Cos(radians);

        float tx = v.x;
        float ty = v.y;

        return new Vector2(cos * tx - sin * ty, sin * tx + cos * ty);
    }
}

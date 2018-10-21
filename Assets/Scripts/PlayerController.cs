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
    public BoxCollider2D hurtColliderTop, hurtColliderBottom;
    public BoxCollider2D sideColliderLeft, sideColliderRight;

    [Header("Driving")]
    public bool canControl;
    public float accelerationSpeed;
    public float maxDrivingSpeed;
    public Vector2 turningSpeedSpeedClamp;
    public float movementSpeedForMaxTurnSpeed;
    public Vector2 linearDragSpeedClamp;

    [Header("Drifting")]
    public float driftSpeed;
    public float driftAutoTurnSpeed;
    public float driftTurningInfluence;
    public float driftRotationAngle;
    public float driftRotationSpriteRotFactor;
    public bool disableHurtWhileDrifting;
    bool isDrifting;
    DriftDirection driftDir;
    public float boostSpeed = 30;
    float _normalSpeed;

    [Header("Camera")]
    public float explodeCooldownRate;
    public float camRotationLerp;
    public float explosionShakeAdd;
    public float screenShakeMax;

    void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        virtualCamNoise = virtualCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    void Start()
    {
        gameManager = GameManager.instance;

        _normalSpeed = driftSpeed;
    }

    void FixedUpdate()
    {
        // Capture inputs (x = left/right, y = up/down)
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (!canControl) {
            input = Vector2.zero;
        }

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

            if (disableHurtWhileDrifting) {
                hurtColliderTop.enabled = false;
                hurtColliderBottom.enabled = false;
            }
        }
        if (!isDrifting)
        {
            //Reset sprial meter
            gameManager.meterPercent -= gameManager.meterRate * Time.deltaTime;

            //Stop emitter
            sparks.Stop();

            // Accelerate and clamp speed normally if not drifting

            // Forward/backward
            rigidbody2D.AddForce(transform.up * input.y * accelerationSpeed);

            float turningSpeed = Mathf.Lerp(turningSpeedSpeedClamp.x, turningSpeedSpeedClamp.y,
                rigidbody2D.velocity.magnitude / (accelerationSpeed / 2f));

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
            //Increase spiral meter
            gameManager.meterPercent += gameManager.meterRate * Time.deltaTime;

            //Start particles
            sparks.Play();

            // Kill drift if we let go of Drift key
            if (!Input.GetButton("Drift"))
            {
                StopDrifting();
            }

            // Sprite during drift rotates a bit further than direction
            sprite.gameObject.transform.localRotation = Quaternion.Lerp(
                sprite.gameObject.transform.localRotation,
                Quaternion.Euler(
                    0, 0,
                    (driftDir == DriftDirection.LEFT ? 1 : -1) * driftRotationAngle * 0.5f),
                driftRotationSpriteRotFactor
            );

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

        rigidbody2D.drag = Mathf.Lerp(linearDragSpeedClamp.x, linearDragSpeedClamp.y,
            rigidbody2D.velocity.magnitude / accelerationSpeed);
    }

    void Update()
    {
        // Take the shake down by a given rate every frame (decremental)
        if (virtualCamNoise.m_AmplitudeGain > 0)
        {
            virtualCamNoise.m_AmplitudeGain -= explodeCooldownRate * Time.deltaTime;
        }

        //Increase maximum driving speed if boost meter is full
        if (gameManager.meterPercent >= 1)
        {
            driftSpeed = boostSpeed;
        } else
        {
            driftSpeed = _normalSpeed;
        }
    }

    void LateUpdate()
    {
        virtualCam.transform.rotation = Quaternion.Lerp(
            virtualCam.transform.rotation, transform.rotation, camRotationLerp);
    }

    public void AddScreenShake() {
        virtualCamNoise.m_AmplitudeGain = Mathf.Clamp(virtualCamNoise.m_AmplitudeGain + explosionShakeAdd, 0, screenShakeMax);
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

    void OnDestroy() {
        virtualCamNoise.m_AmplitudeGain = 0;
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.collider.tag == "Building") {
            StopDrifting();
        }
    }

    public void StopDrifting() {
        isDrifting = false;

        if (disableHurtWhileDrifting) {
            hurtColliderTop.enabled = true;
            hurtColliderBottom.enabled = true;
        }
    }
}

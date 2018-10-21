using UnityEngine;
using System.Collections;

public class Billboard : MonoBehaviour
{

    Camera gameCamera;
    private float initialRotation;

    void Start()
    {
        gameCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        initialRotation = transform.rotation.z;
    }

    void Update()
    {
        transform.rotation = gameCamera.transform.rotation;
        transform.Rotate(Vector3.up * initialRotation * 180); // Fix Rotation (Flipped sprites)
    }
}
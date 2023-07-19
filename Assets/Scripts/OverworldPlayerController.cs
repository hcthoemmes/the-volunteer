using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldPlayerController : MonoBehaviour
{

    private Rigidbody2D rb;
    [SerializeField] private int rootedLevel = 1; // ROOTEDNESS
    [SerializeField] private GameObject sprite;

    [Header("Camera")]
    public GameObject followCamera;
    [SerializeField] private float cameraLagSpeed = 5f;

    [Header("Rolling Movement")]
    [SerializeField] private KeyCode actionKey = KeyCode.E; // Used to interact with landmarks
    [SerializeField] private KeyCode brakeKey = KeyCode.Space;
    [SerializeField] private float maxMoveSpeed = 5f;
    [SerializeField] private float accelerateMultiplier = 2f;
    [SerializeField] private float decelerateMultiplier = 1.4f;
    [SerializeField] private float turnMultiplier = 0.4f; // How long it takes to change direction
    [SerializeField] private float brakeThreshold = 0.4f; // If speed is less than && brake is held, then stop movement

    private float currentMoveSpeed = 0f;
    Vector2 moveDirection = Vector2.zero;
    Vector2 currentMoveVector = Vector2.zero;

    [Header("Graphics")]
    [SerializeField] private float rotationFactor = 0.4f; // Sprite rotation speed
    public ParticleSystem smokeParticles;
    [SerializeField] private int smokeEmissionMax = 30; // Max smoke particles emitted
    [SerializeField] private float minSmokeEmittedSpeed = 3f; // Max smoke particles emitted

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        bool wantsBrake = Input.GetKey(brakeKey);

        // Only allow move direction to reach zero if not moving (braked)
        //if(currentMoveSpeed > brakeThreshold)
        //{
        //    if (moveX == 0) moveX = moveDirection.x;
        //    if (moveY == 0) moveY = moveDirection.y;
        //}

        moveDirection = new Vector2(moveX, moveY).normalized;

        // Handles smooth turning (changing directions)
        currentMoveVector.x = currentMoveVector.x - (currentMoveVector.x - moveDirection.x) * turnMultiplier * Time.deltaTime;
        currentMoveVector.y = currentMoveVector.y - (currentMoveVector.y - moveDirection.y) * turnMultiplier * Time.deltaTime;

        // Braking
        if(Input.GetKey(brakeKey))
        {
            if(currentMoveSpeed > 0) currentMoveSpeed = Mathf.Max(currentMoveSpeed - decelerateMultiplier * Time.deltaTime, 0);
        } else if (Vector3.Magnitude(moveDirection) > 0)
        {
            currentMoveSpeed = Mathf.Min(currentMoveSpeed + accelerateMultiplier * Time.deltaTime, maxMoveSpeed);
        }

        // Rotate sprite based on direction
        sprite.transform.eulerAngles += new Vector3(0, 0,  rotationFactor * (rb.velocity.magnitude/maxMoveSpeed) * (-currentMoveVector.x));

        // Emit smoke particles based on speed or if braking
        var psEmission = smokeParticles.emission;
        psEmission.rateOverTime = (wantsBrake || rb.velocity.magnitude > minSmokeEmittedSpeed) ? Mathf.Round((rb.velocity.magnitude / maxMoveSpeed) * smokeEmissionMax) : 0;

    }

    private void FixedUpdate()
    {
        rb.velocity = currentMoveVector * currentMoveSpeed;

        // Smooth follow camera
        Vector3 cameraTarget = new Vector3(transform.position.x, transform.position.y, followCamera.transform.position.z);
        followCamera.transform.position = Vector3.Lerp(followCamera.transform.position, cameraTarget, cameraLagSpeed * Time.fixedDeltaTime);
    }
    public int GetRootednessLevel()
    {
        return rootedLevel;
    }

    public KeyCode GetActionKey()
    {
        return actionKey;
    }
}

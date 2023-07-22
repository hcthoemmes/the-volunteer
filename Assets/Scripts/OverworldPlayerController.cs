using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class OverworldPlayerController : MonoBehaviour
{

    private Rigidbody2D rb;
    [SerializeField] private int rootedLevel = 1; // ROOTEDNESS
    [SerializeField] private GameObject sprite;

    [Header("Camera")]
    public Camera followCamera;
    [SerializeField] private float cameraLagSpeed = 5f;

    [Header("Rolling Movement")]
    [SerializeField] private KeyCode actionKey = KeyCode.E; // Used to interact with landmarks
    [SerializeField] private KeyCode brakeKey = KeyCode.Space;
    [SerializeField] private float maxMoveSpeed = 5f;
    [SerializeField] private float accelerateMultiplier = 2f;
    [SerializeField] private float brakeDecelerateMultiplier = 1.4f;
    [SerializeField] private float turnMultiplier = 0.4f; // How long it takes to change direction
    [Range(0f, 1f)]
    [SerializeField] private float speedLossOnBouncePercentage = 0.2f; // e.g. if 0.2, you lose 20% of speed upon bouncing

    [Header("Rootedness")]
    [SerializeField] private float gaugeIncreaseRate = 0.01f;
    private float rootGaugePercentage = 0f;

    [Min(1f)]
    [SerializeField] private float slingshotSpeedDecay = 0.2f; // Speed decrease rate while slingshotted
    [SerializeField] private float slingshotRegainControlSpeed = 1.75f;

    [SerializeField] private float baseSlingshotSpeed = 1f;
    [SerializeField] private float slingSpeedRootMult = 15f; // How fast to slingshot depending on rootedness percentage

    private bool isSlingshotting = false; // replace with enum for enemy accessibility and ease

    public enum playerState     { slingshotting, rolling, stopped, rooting }
    public enum stateCommands   { brakeHeld, moveHeld } // not sure if this one will be useful?

    public playerState currentState;
    public stateCommands playerCommands;

    [SerializeField] private Slider rootGaugeSlider;
    private float slingReleaseGaugeLevel, slingReleaseSpeed; // For rootedness percentage bar

    Vector2 lastVelocity;
    private float currentMoveSpeed = 0f;
    private Vector2 moveDirection = Vector2.zero;
    Vector2 currentMoveVector = Vector2.zero;
    private Vector2 slingshotDirection;

    [Header("Graphics")]
    [SerializeField] private float rotationFactor = 0.4f; // Sprite rotation speed
    public ParticleSystem smokeParticles;
    private ParticleSystem.EmissionModule psEmission;
    [SerializeField] private int smokeEmissionMax = 30; // Max smoke particles emitted
    [SerializeField] private float minSmokeEmittedSpeed = 3f; // Minimum speed need to be moving to display smoke particles
    [SerializeField] private GameObject slingshotArrow;

    [SerializeField] private RectTransform slingshotArrowBase;
    [SerializeField] private float arrowBaseStartWidth = 0.4f;
    [SerializeField] private float arrowBaseEndWidth = 2f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        psEmission = smokeParticles.emission;
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        bool wantsBrake = Input.GetKey(brakeKey);


        moveDirection = new Vector2(moveX, moveY).normalized;

        // Handles smooth turning (changing directions)

        if (!isSlingshotting) {
            currentMoveVector.x = currentMoveVector.x - (currentMoveVector.x - moveDirection.x) * turnMultiplier * Time.deltaTime;
            currentMoveVector.y = currentMoveVector.y - (currentMoveVector.y - moveDirection.y) * turnMultiplier * Time.deltaTime;

            // Braking
            if (Input.GetKey(brakeKey))
            {
                if (currentMoveSpeed > 0) currentMoveSpeed = Mathf.Max(currentMoveSpeed - brakeDecelerateMultiplier * Time.deltaTime, 0);

                if (rb.velocity.magnitude < 0.2f)
                {
                    rootGaugePercentage = Mathf.Min(rootGaugePercentage + gaugeIncreaseRate * Time.deltaTime, 1f);
                    rootGaugeSlider.value = rootGaugePercentage;

                    // Change arrow based on rootedness value
                    float arrowBaseWidth = arrowBaseStartWidth + rootGaugePercentage * (arrowBaseEndWidth - arrowBaseStartWidth);
                    slingshotArrowBase.sizeDelta = new Vector2(arrowBaseWidth, slingshotArrowBase.sizeDelta.y);

                    // Slingshot
                    slingshotArrow.SetActive(true);
                    Vector2 mousePos = Input.mousePosition;
                    float angle = Mathf.Atan2(mousePos.x - transform.position.x, mousePos.y - transform.position.y) * Mathf.Rad2Deg;
                    slingshotDirection = new Vector2(mousePos.x + (followCamera.transform.position.x - (Screen.width/2)) - transform.position.x, mousePos.y + (followCamera.transform.position.y - (Screen.height / 2)) - transform.position.y);
                    slingshotArrow.transform.up = slingshotDirection;
                }
            }
            else if (moveDirection.magnitude > 0) // Accelerating
            {
                slingshotArrow.SetActive(false);
                currentMoveSpeed = Mathf.Min(currentMoveSpeed + accelerateMultiplier * Time.deltaTime, maxMoveSpeed);
            }
            else if (lastVelocity.magnitude < 0.2f) // Reset speed if come to a full stop
            {
                slingshotArrow.SetActive(false);
                currentMoveSpeed = 0;
            }
        } else
        {
            slingshotArrow.SetActive(false);
            currentMoveSpeed = Mathf.Max(currentMoveSpeed - slingshotSpeedDecay * Time.deltaTime, 0);

            rootGaugePercentage = slingReleaseGaugeLevel * ((currentMoveSpeed - slingshotRegainControlSpeed) / (slingReleaseSpeed - slingshotRegainControlSpeed));

            if (currentMoveSpeed <= slingshotRegainControlSpeed)
            {
                isSlingshotting = false;
                rootGaugePercentage = 0;
            }

            rootGaugeSlider.value = rootGaugePercentage;
            
        }

        // Begin slingshot maneuver
        if(Input.GetKeyUp(brakeKey) && !isSlingshotting) // && moveDirection.magnitude > 0
        {
            slingReleaseSpeed = baseSlingshotSpeed + rootGaugePercentage * slingSpeedRootMult;
            slingReleaseGaugeLevel = rootGaugePercentage;

            currentMoveSpeed = slingReleaseSpeed;
            currentMoveVector = slingshotDirection.normalized;
            isSlingshotting = true;
            slingshotArrow.SetActive(false);
        }

        // Rotate sprite based on direction
        sprite.transform.eulerAngles += new Vector3(0, 0,  rotationFactor * (rb.velocity.magnitude/maxMoveSpeed) * (-currentMoveVector.x));

        // Emit smoke particles based on speed or if braking
        psEmission.rateOverTime = (wantsBrake || rb.velocity.magnitude > minSmokeEmittedSpeed) ? Mathf.Round((rb.velocity.magnitude / maxMoveSpeed) * smokeEmissionMax) : 0;

    }

    private void FixedUpdate()
    {
        rb.velocity = currentMoveVector * currentMoveSpeed;
        lastVelocity = rb.velocity;

        // Smooth follow camera
        Vector3 cameraTarget = new Vector3(transform.position.x, transform.position.y, followCamera.transform.position.z);
        followCamera.transform.position = Vector3.Lerp(followCamera.transform.position, cameraTarget, cameraLagSpeed * Time.fixedDeltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        currentMoveSpeed -= currentMoveSpeed * (isSlingshotting ? 0 : speedLossOnBouncePercentage);
        currentMoveVector = Vector2.Reflect(lastVelocity.normalized, collision.contacts[0].normal).normalized;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamMovement : PhysicsObject {

    public Camera followCam;

    private PlayerAnimator playerAnimator;

    private static readonly float playerDefaultSpeed = 4;
    private float playerSpeed;
    public static readonly float jumpMagnitude = 5f;

    private float currentZoom;
    private float currentZoomVelocity;
    private float requiredZoom;

    private Vector2 currentCamVelocity;
    //private static readonly float cameraLerpRate = 0.03f;
    private static readonly float cameraMaxSpeed = Mathf.Infinity;

    private static readonly float cameraSmoothTime = 0.7f;
    private static readonly float cameraZoomRate = 0.1f;
    private static readonly float cameraZoomTime = 0.3f;
    private static readonly float cameraMaxZoom = 2;
    private static readonly float cameraDefaultZoom = 3;
    private static readonly float cameraMinZoom = 5;

    bool isPlayerFalling;
    bool isPlayerFlipped;
    bool isPlayerMoving;
    bool isPlayerUsing;
    bool isPlayerJumping;

    protected override void Start() {
        base.Start();

        playerAnimator = GetComponent<PlayerAnimator>();

        playerSpeed = playerDefaultSpeed;
        isPlayerMoving = false;
        isPlayerFlipped = false;
        isPlayerFalling = false;
        isPlayerUsing = false;
        isPlayerJumping = false;

        currentCamVelocity = Vector2.zero;

        followCam.orthographicSize = cameraDefaultZoom;
        currentZoom = followCam.orthographicSize;
        currentZoomVelocity = 0;
        requiredZoom = currentZoom;
    }

    override protected void FixedUpdate() {
        base.FixedUpdate();
        UpdateTargetVelocity();
        isPlayerFalling = !grounded;
        UpdateCameraPosition();
        UpdateZoom();
    }

    private void Update() {
        HandleInput();
    }

    private void UpdateTargetVelocity() {
        float playerXMov = Input.GetAxis("Horizontal");

        isPlayerMoving = playerXMov != 0;
        if (playerXMov < 0) { isPlayerFlipped = true; }
        else if (playerXMov > 0) { isPlayerFlipped = false; };
        targetVelocity = IsPlayerImmobile() ? Vector2.zero : new Vector2(playerXMov * playerSpeed, 0);
    }

    private void UpdateCameraPosition() {

        //TODO: Fix player falling off of the screen problem

        Vector2 cameraPosition = new Vector2(followCam.transform.position.x,
           followCam.transform.position.y);

        cameraPosition = Vector2.SmoothDamp(cameraPosition, rb.position,
            ref currentCamVelocity, cameraSmoothTime, cameraMaxSpeed, Time.fixedDeltaTime);
        //cameraPosition = Vector2.Lerp(cameraPosition, rb.position, cameraLerpRate);

        followCam.transform.position = new Vector3(cameraPosition.x,
            cameraPosition.y, followCam.transform.position.z);
    }

    private void UpdateZoom() {
        if (currentZoomVelocity == 0 && Input.GetAxis("Vertical") != 0) {
            currentZoomVelocity = cameraZoomRate;
        }

        requiredZoom -= Input.GetAxis("Vertical") * cameraZoomRate;
        requiredZoom = Mathf.Min(cameraMinZoom, requiredZoom);
        requiredZoom = Mathf.Max(cameraMaxZoom, requiredZoom);

        currentZoom = Mathf.SmoothDamp(currentZoom, requiredZoom,
                ref currentZoomVelocity, cameraZoomTime);

        followCam.orthographicSize = currentZoom;
    }

    private void HandleInput() {
        if (Input.GetButtonDown("Use")) {
            isPlayerUsing = true;
        }
        else if (Input.GetButtonUp("Use")) {
            isPlayerUsing = false;
        }
        if (Input.GetButtonDown("Jump") && grounded) {
            Jump();
            isPlayerJumping = true;
        }
        else if (Input.GetButtonUp("Jump")) {
            isPlayerJumping = false;
        }
    }

    private void Jump() {
        velocity += Vector2.up * jumpMagnitude;
    }

    private bool IsPlayerImmobile() {
        return playerAnimator.IsPlayerImmobile();
    }

    public bool IsPlayerFlipped() { return isPlayerFlipped; }

    public bool IsPlayerMoving() { return isPlayerMoving; }

    public bool IsPlayerUsing() { return isPlayerUsing; }

    public bool IsPlayerFalling() { return isPlayerFalling; }

    public bool IsPlayerJumping() { return isPlayerJumping; }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEngine : PhysicsObject {

    public Camera followCam;
    private PlayerAnimator playerAnimator;
    private Transform playerTransform;
    Collider2D playerCollider;

    private static readonly float playerDefaultSpeed = 4;
    private float playerSpeed;
    public static readonly float jumpMagnitude = 5f;

    private float currentZoom;
    private float currentZoomVelocity;
    private float requiredZoom;

    private Vector2 currentCamVelocity;
    //private static readonly float cameraLerpRate = 0.03f;
    private static readonly float cameraMaxSpeed = Mathf.Infinity;

    private static readonly float cameraSmoothTime = 0.3f;
    private static readonly float cameraZoomRate = 0.1f;
    private static readonly float cameraZoomTime = 0.3f;
    private static readonly float cameraMaxZoom = 2;
    private static readonly float cameraDefaultZoom = 3;
    private static readonly float cameraMinZoom = 5;

    private float grabDistance = 0.2f;

    bool isPlayerFalling;
    bool isPlayerFlipped;
    bool isPlayerMoving;
    bool isPlayerUsing;
    bool isPlayerJumping;
    bool isTryingToGrab;

    protected override void Start() {
        base.Start();

        playerAnimator = GetComponent<PlayerAnimator>();
        playerTransform = GetComponent<Transform>();
        playerCollider = GetComponent<Collider2D>();

        playerSpeed = playerDefaultSpeed;
        isPlayerMoving = false;
        isPlayerFlipped = false;
        isPlayerFalling = false;
        isPlayerUsing = false;
        isPlayerJumping = false;
        isTryingToGrab = false;

        currentCamVelocity = Vector2.zero;

        followCam.orthographicSize = cameraDefaultZoom;
        currentZoom = followCam.orthographicSize;
        currentZoomVelocity = 0;
        requiredZoom = currentZoom;
    }

    protected override void FixedUpdate() {
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
        else if (Input.GetButtonDown("Grab")) {
            isTryingToGrab = true;
        }
        else if (Input.GetButtonUp("Grab")) {
            isTryingToGrab = false;
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
        velocity += (Vector2) transform.up * jumpMagnitude;
    }

    private bool IsPlayerImmobile() {
        return playerAnimator.IsPlayerImmobile();
    }

    public Vector2 GetTargetVelocity() {
        return targetVelocity;
    }

    RaycastHit2D[] resultsRight = new RaycastHit2D[1];
    RaycastHit2D[] resultsLeft = new RaycastHit2D[1];

    public bool IsGrabbing(GameObject movObj) {
        if (!isTryingToGrab) { return false; }

        resultsRight = new RaycastHit2D[1];
        resultsLeft = new RaycastHit2D[1];

        Vector2 transformR = playerTransform.right;
        transformR += new Vector2(0, ((BoxCollider2D) playerCollider).size.y / 2); // player collider must be box collider

        Vector2 transformL = -playerTransform.right;
        transformL += new Vector2(0, ((BoxCollider2D) playerCollider).size.y / 2);

        int countLeft = rb.Cast(transformR, contactFilter, resultsRight, grabDistance);
        int countRight = rb.Cast(transformL, contactFilter, resultsLeft, grabDistance);

        if (countLeft <= 0 && countRight <= 0) { return false; }
        bool isObjHit = false;
        for (int i = 0; i < 1; i++) {
            if (resultsRight[i].collider != null && resultsRight[i].collider.gameObject == movObj) {
                isObjHit = true;
            }
        }
        for (int i = 0; i < 1; i++) {
            if (resultsLeft[i].collider != null && resultsLeft[i].collider.gameObject == movObj) {
                isObjHit = true;
            }
        }
        return isTryingToGrab && isObjHit;
    }

    public Vector2 GetGroundNormal() {
        return groundNormal;
    }

    public bool IsPlayerTryingToGrab() { return isTryingToGrab; }

    public bool IsPlayerFlipped() { return isPlayerFlipped; }

    public bool IsPlayerMoving() { return isPlayerMoving; }

    public bool IsPlayerUsing() { return isPlayerUsing; }

    public bool IsPlayerFalling() { return isPlayerFalling; }

    public bool IsPlayerJumping() { return isPlayerJumping; }

    public bool IsPlayerGrounded() { return grounded; }
}

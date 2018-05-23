using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : MonoBehaviour {

    public float minGroundNormalY = 0.65f;
    public Rigidbody2D rb;

    protected bool grounded;
    protected Vector2 groundNormal;
    protected Vector2 targetVelocity;
    protected Vector2 velocity;

    protected ContactFilter2D contactFilter;
    protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
    protected List<RaycastHit2D> hitBufferList = new List<RaycastHit2D>(16);

    protected static readonly float shellRadius = 0.01f;
    protected static readonly float gravityModifier = 1f;
    protected static readonly float minMoveDistance = 0.001f;

    void OnEnable() {
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void Start() {
        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        contactFilter.useLayerMask = true;
    }

    protected virtual void FixedUpdate() {
        grounded = false;
        velocity += gravityModifier * Physics2D.gravity * Time.deltaTime;
        velocity.x = targetVelocity.x;
        Vector2 deltaPosition = velocity * Time.deltaTime;

        Vector2 moveAlongGround = new Vector2(groundNormal.y, -groundNormal.x);
        Vector2 movement = moveAlongGround * deltaPosition.x;
        Move(movement, false);

        movement = Vector2.up * deltaPosition.y;
        Move(movement, true);
    }

    public void Move(Vector2 movement, bool yMovement) {
        float distance = movement.magnitude;
        Vector2 previousGroundNormal = groundNormal;
        if (distance > minMoveDistance) {
            int count = rb.Cast
                (movement, contactFilter, hitBuffer, distance + shellRadius);
            hitBufferList.Clear();
            for (int i = 0; i < count; i++) { hitBufferList.Add(hitBuffer[i]); }

            foreach (RaycastHit2D raycast in hitBufferList) {
                Vector2 currentNormal = raycast.normal;
                if (currentNormal.y > minGroundNormalY) {
                    grounded = true;
                    if (yMovement) {
                        groundNormal = currentNormal;
                        currentNormal.x = 0;
                    }
                }
                float projection = Vector2.Dot(velocity, currentNormal);
                if (projection < 0) { velocity -= projection * currentNormal; }

                float modifiedDistance = raycast.distance - shellRadius;
                distance = modifiedDistance < distance ? modifiedDistance : distance;
            }
        }
        rb.position += movement.normalized * distance;
        
    }

}

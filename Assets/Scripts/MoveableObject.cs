using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableObject : PhysicsObject {

    private PlayerEngine player;

    protected override void Start() {
        player = GameManager.instance.player.GetComponent<PlayerEngine>();
        base.Start();
    }

    protected override void FixedUpdate() {
        if (player.IsGrabbing(this.gameObject)) {
            targetVelocity = player.GetTargetVelocity();
            
            
            //transform.up = player.GetGroundNormal();
        }
        else { targetVelocity = Vector2.zero; }
        base.FixedUpdate();
    }


}

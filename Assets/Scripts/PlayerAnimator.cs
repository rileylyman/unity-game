using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : Animatable {

    private PlayerCamMovement movementScript;

    public int IDLE = 0;
    public int MOVE = 1;
    public int JUMP = 2;
    public int USE_OBJECT = 3;
    public int FALL = 2;

    private int useDuration;
    private int currentUseStep;

    private int jumpDuration;
    private int currentJumpStep;

    private bool firstJumpIteration;
    private bool firstUseIteration;

    protected override void Start() {
        base.Start();
        movementScript = GetComponent<PlayerCamMovement>();
        
        useDuration = animations[USE_OBJECT].Count;
        currentUseStep = 0;

        jumpDuration = animations[JUMP].Count;
        currentJumpStep = 0;

    }

    protected override void Update() {
        base.Update();
        Flip();
        ResetFirstIterations();
    }

    private void ResetFirstIterations() {
        if (!firstUseIteration && currentUseStep == 0) {
            firstUseIteration = !movementScript.IsPlayerUsing();
        }
        if (!firstJumpIteration && currentJumpStep == 0) {
            firstJumpIteration = !movementScript.IsPlayerJumping();
        }
    }

    protected override int ChooseAnimation() {
        if (CheckOneTimeAnimation(movementScript.IsPlayerJumping(), ref currentJumpStep, jumpDuration, ref firstJumpIteration)
            && movementScript.IsPlayerFalling()) {
            return JUMP;
        }
        else if (movementScript.IsPlayerFalling()) {
            return FALL;
        }
        else if (CheckOneTimeAnimation(movementScript.IsPlayerUsing(), ref currentUseStep, useDuration, ref firstUseIteration)) {         
            return USE_OBJECT;
        }
        else if (movementScript.IsPlayerMoving()) {
            return MOVE;
        }
        else {
            return IDLE;
        }
    }

    public bool IsPlayerImmobile() {
        return currentAnimation == USE_OBJECT;
    }

    protected override void SetAllTimeCountersToZero() {
        currentWaitTime = 0;
    }

    private void Flip() {
        if (movementScript.IsPlayerFlipped()) { spr.flipX = true; } else { spr.flipX = false; }
    }

    public int GetJumpDuration() {
        return jumpDuration * animationWaitDuration;
    }
}

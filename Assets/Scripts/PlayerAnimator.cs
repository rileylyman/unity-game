using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : Animatable {

    private PlayerEngine player;

    public int IDLE = 0;
    public int MOVE = 1;
    public int JUMP = 2;
    public int USE_OBJECT = 3;
    public int FALL = 2;
    public int WINDY_MOVE = 1;
    public int WINDY_IDLE = 0;
    public int SNEAKING_IDLE = 0;
    public int SNEAKING_MOVE = 1;

    private int useDuration;
    private int currentUseStep;

    private int jumpDuration;
    private int currentJumpStep;

    private bool firstJumpIteration;
    private bool firstUseIteration;

    protected override void Start() {
        base.Start();
        player = GetComponent<PlayerEngine>();
        
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

    public void Toggle(string alternativeAnimation) {
        if (alternativeAnimation.Equals("WINDY")) {
            int temp = IDLE;
            IDLE = WINDY_IDLE;
            WINDY_IDLE = temp;

            temp = MOVE;
            MOVE = WINDY_MOVE;
            WINDY_MOVE = temp;
        }
        else if (alternativeAnimation.Equals("SNEAK")) {
            int temp = IDLE;
            IDLE = WINDY_IDLE;
            WINDY_IDLE = temp;

            temp = MOVE;
            MOVE = WINDY_MOVE;
            WINDY_MOVE = temp;
        }
    }

    private void ResetFirstIterations() {
        if (!firstUseIteration && currentUseStep == 0) {
            firstUseIteration = !player.IsPlayerUsing();
        }
        if (!firstJumpIteration && currentJumpStep == 0) {
            firstJumpIteration = !player.IsPlayerJumping();
        }
    }

    protected override int ChooseAnimation() {
        if (CheckOneTimeAnimation(player.IsPlayerUsing(), ref currentUseStep, useDuration, ref firstUseIteration)) {
            return USE_OBJECT;
        }
        else if (CheckOneTimeAnimation(player.IsPlayerJumping(), ref currentJumpStep, jumpDuration, ref firstJumpIteration)
            && player.IsPlayerFalling()) {
            return JUMP;
        }
        else if (player.IsPlayerFalling()) {
            return FALL;
        }
        else if (player.IsPlayerMoving()) {
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
        if (player.IsPlayerFlipped()) { spr.flipX = true; } else { spr.flipX = false; }
    }

    public int GetJumpDuration() {
        return jumpDuration * animationWaitDuration;
    }
}

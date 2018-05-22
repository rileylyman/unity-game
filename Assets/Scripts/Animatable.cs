using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Animatable : MonoBehaviour {

    protected const int animationWaitDuration = 5;
    protected int currentWaitTime;

    protected Dictionary<int, List<Sprite>> animations;
    public bool[] starts;
    public Sprite[] rawAnimationData;

    protected SpriteRenderer spr;

    protected int currentAnimation;
    protected int currentIndex;

    protected virtual void Start() {
        currentWaitTime = 0;

        spr = GetComponent<SpriteRenderer>();
        currentIndex = 0;

        animations = new Dictionary<int, List<Sprite>>();
        IndexAnimations();
    }

    protected virtual void Update() {
        int anim = ChooseAnimation();
        if (anim != currentAnimation) { 
            currentAnimation = anim;
            currentIndex = 0;
            SetAllTimeCountersToZero();
        }
        RunAnimation(currentAnimation);
    }

    protected abstract int ChooseAnimation();

    protected void RunAnimation(int animation) {
        List<Sprite> animationList = animations[animation];
        spr.sprite = animationList[currentIndex];
        currentWaitTime = (currentWaitTime + 1) % animationWaitDuration;
        if (currentWaitTime == 0) {
            currentIndex = (currentIndex + 1) % animationList.Count;
        }
    }

    private void IndexAnimations() {
        if (starts.Length != rawAnimationData.Length) {
            throw new System.Exception("Starts and RawAnimationData must be the same length");
        }
        int currentBin = -1;
        for (int i = 0; i < rawAnimationData.Length; i++) {
            if (starts[i]) {
                currentBin++;
                animations.Add(currentBin, new List<Sprite>());
            }
            animations[currentBin].Add(rawAnimationData[i]);
        }
    }

    protected bool CheckOneTimeAnimation(bool shouldAnimate, ref int currentStep, int maxSteps, ref bool firstTime) {
        if (shouldAnimate && currentStep == 0 && firstTime) {
            currentStep = maxSteps * animationWaitDuration;
            firstTime = false;
        }
        bool result = currentStep > 0;
        if (currentStep > 0) { currentStep--; }
        return result;
    }
    
    protected abstract void SetAllTimeCountersToZero();
}

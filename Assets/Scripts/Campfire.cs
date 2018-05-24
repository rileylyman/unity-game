using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Campfire : Interactable {

    private SpriteRenderer spr;
    public Item firewood;

    private void Start() {
        spr = GetComponent<SpriteRenderer>();
        spr.enabled = false;
        itemPrereqs = new Item[1];
        itemPrereqs[0] = firewood;
    }

    protected override void Go() {
        Debug.Log(4);
        spr.enabled = true;
        //AddComponent<CampFireAnimator>();
        Inventory.instance.Remove(firewood, false);
    }

    protected override bool PlayerCanInteract() {
        return PlayerHasItemPrereqs();
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour {

    protected Item[] itemPrereqs;

    //PlayerEngine calls this method when they "Use" near the interactable
    public virtual void Interact() {
        Debug.Log(3);
        if (PlayerCanInteract()) { Go(); }
    }

    //Does whatever the interactable does.
    protected abstract void Go();

    //This may also check bools in PlayerEngine, etc, to determine if the player can interact
    protected abstract bool PlayerCanInteract();
 
    protected virtual bool PlayerHasItemPrereqs() {
        var playerItems = Inventory.instance.GetItemsArray();
        foreach (Item req in itemPrereqs) {
            bool hasFoundPrereq = false;
            foreach (Item it in playerItems) {
                if (req.Equals(it)) { hasFoundPrereq = true; }
            }
            if (!hasFoundPrereq) { return false; }
        }
        return true;
    }
}

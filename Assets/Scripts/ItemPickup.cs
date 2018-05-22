using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour {

    public Item item;
    Inventory inv;

    private void Start() {
        inv = Inventory.instance;
    }

    public void PickUp() {
        if (inv == null) {
            return;
        }
        inv.Store(item);
        Destroy(gameObject);
    }

}

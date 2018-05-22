using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject {

    new public string name;
    public Sprite icon;
    public Sprite objectSprite;

    //item layer is default for now
    private string itemTag = "Collectable";

    public override int GetHashCode() {
        return (name + icon.ToString() + objectSprite.ToString()).GetHashCode();
    }

    public override bool Equals(object other) {
        if (other == null) {
            return false;
        }
        if (other.GetType() != this.GetType()) {
            return false;
        }
        Item otherItem = (Item) other;
        return otherItem.name == name && otherItem.icon == icon
            && otherItem.objectSprite == objectSprite;
    }

    public void Drop() {
        var go = new GameObject() {
            name = this.name,
            tag = itemTag
        };
        go.AddComponent<ItemPickup>();
        go.GetComponent<ItemPickup>().item = this;
        go.AddComponent<SpriteRenderer>();
        go.GetComponent<SpriteRenderer>().sprite = objectSprite;
        go.AddComponent<CircleCollider2D>();
        go.GetComponent<CircleCollider2D>().isTrigger = true;
        go.GetComponent<CircleCollider2D>().radius = 1.2f;
        go.GetComponent<Transform>().position = GameManager.instance.PlayerPosition();
    }
}

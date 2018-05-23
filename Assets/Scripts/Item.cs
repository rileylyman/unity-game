using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject {

    new public string name;
    public Sprite icon;
    public Sprite objectSprite;

    private float waitTime = 0;

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

        //dropPos.y += objectSprite.bounds.extents.y;

        go.AddComponent<ItemPickup>();
        go.GetComponent<ItemPickup>().item = this;
        go.AddComponent<SpriteRenderer>();
        go.GetComponent<SpriteRenderer>().sprite = objectSprite;
        go.AddComponent<CircleCollider2D>();
        go.GetComponent<CircleCollider2D>().isTrigger = true;
        go.GetComponent<CircleCollider2D>().radius = 1.2f;


        Bounds bounds = ((BoxCollider2D) GameManager.instance.player.GetComponent<Collider2D>()).bounds;
        Vector2 groundNormal = GameManager.instance.playerEngine.GetGroundNormal();
        float rotAngle = Vector2.Angle(groundNormal, Vector2.up);
        Vector2 dropLocation = bounds.min;
        if (groundNormal.x <= 0) {
            dropLocation.x = bounds.max.x;
        } else {
            Debug.Log("Shukran");
            dropLocation.x = bounds.min.x;
            rotAngle = Mathf.Rad2Deg * Mathf.PI / 2 - rotAngle;
        }
        go.GetComponent<Transform>().Rotate((new Vector3(0, 0, 1)), rotAngle);
        go.GetComponent<Transform>().position = dropLocation; 
    }
}

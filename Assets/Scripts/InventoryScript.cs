using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Slot {
    public GameObject gameObject;
    public Item item;
    public Slot(GameObject gameObject, Item item) {
        this.gameObject = gameObject;
        this.item = item;
    }
}

public class InventoryScript : MonoBehaviour {

    [Header("Slots")]
    public int slotAmount;
    public GameObject slotPrefab;
    public GameObject slotPanel;
    public GameObject itemPrefab;
    [Header("Items / Slots")]
    public List<Item> items = new List<Item>();
    public List<Slot> slots = new List<Slot>();
    public bool AddSteelGloves;

    private ItemDatabase itemDatabase;

    void Start() {

        itemDatabase = GetComponent<ItemDatabase>();

        // Loop through and add our slots to our item inventory
        for (int i = 0; i < slotAmount; i++) {
            //Clone the slot
            GameObject clone = Instantiate(slotPrefab);
            // set slot's parent to be slow panel
            clone.transform.SetParent(slotPanel.transform);
            //Create a new slot
            Slot slot = new Slot(clone, null);

            // Get Slot Data
            SlotData slotData = clone.GetComponent<SlotData>();
            slotData.inventory = this;
            slotData.slot = slot;

            // Add that new slot to the list
            slots.Add(slot);
        }
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Q)) {
            AddItem("Manny Gloves", 1);
        }
        if (Input.GetKeyDown(KeyCode.W)) {
            AddItem("Steel Gloves", 1);
        }
        if (Input.GetKeyDown(KeyCode.E)) {
            AddItem("Bronze Gloves", 1);
        }
    }

    public void AddItem(string itemName, int itemAmount = 1) {
        Item newItem = ItemDatabase.GetItem(itemName); // Find the item name
        Slot newSlot = GetEmptySlot(); // Find an empty slot
        if (newItem != null && newSlot != null) {
            if (HasStacked(newItem, itemAmount)) {
                return;
            }
            // Set the empty slot
            newSlot.item = newItem;
            // Create a new item instance
            GameObject item = Instantiate(itemPrefab);
            item.transform.position = newSlot.gameObject.transform.position;
            item.transform.SetParent(newSlot.gameObject.transform);
            item.name = newItem.Title;
            // Set the item's gameObject
            newItem.gameObject = item;
            // Get the image component from the item
            Image image = item.GetComponent<Image>();
            image.sprite = newItem.Sprite;
            // Set the itemdata
            ItemData itemData = item.GetComponent<ItemData>();
            itemData.item = newItem;
            itemData.slot = newSlot;
        }
    }

    // Finds an empty slot and returns it.
    public Slot GetEmptySlot() {
        for (int i = 0; i < slots.Count; i++) {
            if (slots[i].item == null) {
                return slots[i];
            }
        }
        print("No empty slots found");
        return null;
    }

    bool HasStacked(Item itemToAdd, int itemAmount = 1) {
        // Check if item is stackable
        if (itemToAdd.Stackable) {
            //Obtain the occupied slot with the same item
            Slot occupiedSlot = GetSlotWithItem(itemToAdd);
            if (occupiedSlot != null) {
                //Get reference to item in occupied slot
                Item item = occupiedSlot.item;
                //Obtain the script attached to the item
                ItemData itemData = item.gameObject.GetComponent<ItemData>();
                //Increase the item amount
                itemData.amount += itemAmount;
                //Set its text element
                Text textElement = item.gameObject.GetComponentInChildren<Text>();
                textElement.text = itemData.amount.ToString();
                return true;
            }
        }
        return false;
    }

    Slot GetSlotWithItem(Item item) {
        for (int i = 0; i < slots.Count; i++) {
            Item currentItem = slots[i].item;
            //Check if slot is not empty AND check if item si the same.
            if (currentItem != null && currentItem.Title == item.Title) {
                return slots[i];
            }
        }
        return null;
    }
}

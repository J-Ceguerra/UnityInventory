using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using LitJson;

public class ItemDatabase : MonoBehaviour {

    // Stores all the different types of items in a database
    // Use this list to spawn multiple 
    private Dictionary<string, Item> database = new Dictionary<string, Item>();

    // Holds the JSON data we pull in from the scene
    private JsonData itemData;

    private static ItemDatabase instance = null;

    void Awake() {
        if (instance == null) {
            // Set instance to the current instance
            instance = this;

            // Obtain the file path for Items.json
            string jsonFilePath = Application.dataPath + "/StreamingAssets/Items.json";
            //Read the entire file into a string
            string jsonText = File.ReadAllText(jsonFilePath);
            //Load int he data through jsonMapper
            itemData = JsonMapper.ToObject(jsonText);
            //Construct the item database
            ConstructDatabase();

        } else {
            Destroy(this.gameObject);
        }
    }

    void ConstructDatabase() {
        //Loop through all the items inside of itemData
        for (int i = 0; i < itemData.Count; i++) {
            //Obtain the data
            JsonData data = itemData[i];
            //Create a new item and add to database
            Item newItem = new Item(data);
            //Add item to database
            database.Add(newItem.Title, newItem);
        }
    }

    public static Item GetItem(string itemName) {
        // Store database into shorter name
        Dictionary<string, Item> database = instance.database;
        if (database.ContainsKey(itemName)) {
            return database[itemName];
        }
        // Otherwise return null
        return null;
    }

    public static Dictionary<string,Item> GetDatabase() {
        // Return the database from singleton
        return instance.database;
    }
}

public class Item {
    public int ID { get; set; }
    public string Title { get; set; }
    public int Value { get; set; }
    public Stat Stats { get; set; }
    public string Description { get; set; }
    public bool Stackable { get; set; }
    public int Rarity { get; set; }
    public Sprite Sprite { get; set; }
    public GameObject gameObject { get; set; }

    public Item() {
        // Set the ID to -1 indicating that the item has not been set
        ID = -1;
    }

    public Item(JsonData data) {
        ID = (int)data["id"];
        Title = data["title"].ToString();
        Value = (int)data["value"];
        Stats = new Stat(data["stats"]);
        Description = data["description"].ToString();
        Stackable = (bool)data["stackable"];
        Rarity = (int)data["rarity"];
        string filename = data["sprite"].ToString();

        Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/Items/itemmap");
        foreach (Sprite spr in sprites) {
            if (spr.name == filename) {
                Sprite = spr;
            }
        }
        //Sprite = Resources.Load<Sprite>("Sprites/Items/" + filename);
    }
}

public class Stat {
    public int Power { get; set; }
    public int Defence { get; set; }
    public int Vitality { get; set; }
    public Stat(Stat stat) {
        this.Power = stat.Power;
        this.Defence = stat.Defence;
        this.Vitality = stat.Vitality;
    }
    public Stat(JsonData data) {
        Power = (int)data["power"];
        Defence = (int)data["defence"];
        Vitality = (int)data["vitality"];
    }
}
using UnityEngine;
using System.Collections;

public class ItemScript : MonoBehaviour {
    public int id;
    public string Name;
    public string Description;
    public int ItemCount = 1;
    public GameObject GO;
    public Sprite icon;
    public Inventory.Item item;
    public bool Type;
    public bool ScriptIsLoad;
	// Use this for initialization
	void Start () {
        if (Type == true)
        {
            switch (Name)
            {
                case "Dirt":
                    GO = (GameObject)Resources.Load("/prefab/Block/Dirt");
                    icon = Sprite.Create((Texture2D)Resources.Load("/Sprites/GUI/ItemSheet"), new Rect(64f, 0, 16f, 16f), new Vector2(0.5f, 0.5f));
                    break;
                case "Stone":
                    GO = (GameObject)Resources.Load("/prefab/Block/Dirt");
                    icon = Sprite.Create((Texture2D)Resources.Load("/Sprites/GUI/ItemSheet"), new Rect(80f, 0, 16f, 16f), new Vector2(0.5f, 0.5f));
                    break;
                case "CopperOre":
                    GO = (GameObject)Resources.Load("/prefab/Block/Dirt");
                    icon = Sprite.Create((Texture2D)Resources.Load("/Sprites/GUI/ItemSheet"), new Rect(16f, 0, 16f, 16f), new Vector2(0.5f, 0.5f));
                    break;
                case "Coal":
                    GO = (GameObject)Resources.Load("/prefab/Block/Dirt");
                    icon = Sprite.Create((Texture2D)Resources.Load("/Sprites/GUI/ItemSheet"), new Rect(0, 0, 16f, 16f), new Vector2(0.5f, 0.5f));
                    break;
                case "IronOre":
                    GO = (GameObject)Resources.Load("/prefab/Block/Dirt");
                    icon = Sprite.Create((Texture2D)Resources.Load("/Sprites/GUI/ItemSheet"), new Rect(48f, 0, 16f, 16f), new Vector2(0.5f, 0.5f));
                    break;
                case "TinOre":
                    GO = (GameObject)Resources.Load("/prefab/Block/Dirt");
                    icon = Sprite.Create((Texture2D)Resources.Load("/Sprites/GUI/ItemSheet"), new Rect(32f, 0, 16f, 16f), new Vector2(0.5f, 0.5f));
                    break;
            }
            item = new Inventory.Item(Name, Description, icon, GO, id, ItemCount);
        }
        else
        item = new Inventory.Item(id, ItemCount);
        ScriptIsLoad = true;
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    private void FixedUpdate()
    {
        var pers = GameObject.Find("Man_Body");
        var pos = pers.GetComponent<Transform>();
        var dist = Vector3.Distance(pos.position, transform.position);
        if (dist < 0.5f)
        {
            pers.GetComponent<Inventory>().AddItem(this.item);
            Destroy(this.gameObject);
        }
        else
            this.GetComponent<Transform>().Translate((pers.transform.position * Time.deltaTime) / 10);
    }
}

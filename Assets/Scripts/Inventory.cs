using UnityEngine;
using System;
using System.Collections;
using System.Linq;

public class Inventory : MonoBehaviour {
    public Item[] ItemArray = new Item[40];
    public Texture2D HotItemTex;
    public Texture2D ItemTex;
    public Texture2D InventoryTex;
    public Sprite[] ItemIcon;
    private const int MaxStack = 128;
    private const int AllExceptWater = ~1 << 4;
    private const int WaterLayer = 1 << 4;
    private const int DestroyMask = 1 << 9;
    private const int GroundLayer = 1 << 9;
    private bool InventoryState=false;
    private GameObject CoalOreGameO,IronOreGamO,CopperOreGameO,TinOreGameO,DirtGameO,StonGameO;
    public int currentItem;
	// Use this for initialization
	void Start () {

	ItemIcon = new Sprite[(int)(ItemTex.width/16f)];
    for (int i = 0; i < ItemIcon.Length; i++)
        ItemIcon[i] = Sprite.Create(ItemTex, new Rect(i * 16f, 0, 16f, 16f), new Vector2(0.5f, 0.5f));
	}
	
	// Update is called once per frame
	void Update () {
	    if(Input.GetKeyDown(KeyCode.I))
            InventoryState=!InventoryState;
        if (InventoryState == true && Input.GetKeyDown(KeyCode.Escape))
            InventoryState = false;
        if (Input.GetKeyDown(KeyCode.Alpha1))
            currentItem = 0;
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            currentItem = 1;
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            currentItem = 2;
        else if (Input.GetKeyDown(KeyCode.Alpha4))
            currentItem = 3;
        else if (Input.GetKeyDown(KeyCode.Alpha5))
            currentItem = 4;
        else if (Input.GetKeyDown(KeyCode.Alpha6))
            currentItem = 5;
        else if (Input.GetKeyDown(KeyCode.Alpha7))
            currentItem = 6;
        else if (Input.GetKeyDown(KeyCode.Alpha8))
            currentItem = 7;
        else if (Input.GetKeyDown(KeyCode.Alpha9))
            currentItem = 8;
        else if (Input.GetKeyDown(KeyCode.Alpha0))
            currentItem = 9;
        else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0)
            if (currentItem < 9)
                currentItem++;
            else
                currentItem = 0;
        else if (Input.GetAxisRaw("Mouse ScrollWheel") > 0)
            if (currentItem > 0)
                currentItem--;
            else
                currentItem = 9;

                if (Input.GetMouseButton(1))
                {
                    Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    pos.z = 0;
                    pos.x = pos.x - ((float)Math.IEEERemainder((double)(pos.x), (double)(0.16)));
                    pos.y = pos.y - ((float)Math.IEEERemainder((double)(pos.y), (double)(0.16)));
                    if (Vector3.Distance(pos, transform.position) < 2.5f)
                        if (!Physics2D.OverlapPoint(new Vector2(pos.x, pos.y), AllExceptWater)) {//Если не столкнулись ни с чем кроме воды
                            if (Physics2D.OverlapPoint(new Vector2(pos.x, pos.y), WaterLayer))//Если столкнулись с водой
                            {
                                Destroy(Physics2D.OverlapPoint(new Vector2(pos.x, pos.y), WaterLayer).gameObject);//То уничтожаем воду
                                Instantiate(ItemArray[currentItem].ItemGO, pos, Quaternion.identity);//И ставим блок
                                if (Physics2D.OverlapPoint(new Vector2(pos.x + 0.16f, pos.y), GroundLayer) == true)
                                    Physics2D.OverlapPoint(new Vector2(pos.x + 0.16f, pos.y), GroundLayer).gameObject.GetComponent<BlockScript>().Refresh();
                                if (Physics2D.OverlapPoint(new Vector2(pos.x - 0.16f, pos.y), GroundLayer) == true)
                                    Physics2D.OverlapPoint(new Vector2(pos.x - 0.16f, pos.y), GroundLayer).gameObject.GetComponent<BlockScript>().Refresh();
                                if (Physics2D.OverlapPoint(new Vector2(pos.x, pos.y + 0.16f), GroundLayer) == true)
                                    Physics2D.OverlapPoint(new Vector2(pos.x, pos.y + 0.16f), GroundLayer).gameObject.GetComponent<BlockScript>().Refresh();
                                if (Physics2D.OverlapPoint(new Vector2(pos.x, pos.y - 0.16f), GroundLayer) == true)
                                    Physics2D.OverlapPoint(new Vector2(pos.x, pos.y - 0.16f), GroundLayer).gameObject.GetComponent<BlockScript>().Refresh();
                            }
                            else if (ItemArray[currentItem].ItemGO != null)
                            {
                                Instantiate(ItemArray[currentItem].ItemGO, pos, Quaternion.identity);
                                if (Physics2D.OverlapPoint(new Vector2(pos.x + 0.16f, pos.y), GroundLayer) == true)
                                    Physics2D.OverlapPoint(new Vector2(pos.x + 0.16f, pos.y), GroundLayer).gameObject.GetComponent<BlockScript>().Refresh();
                                if (Physics2D.OverlapPoint(new Vector2(pos.x - 0.16f, pos.y), GroundLayer) == true)
                                    Physics2D.OverlapPoint(new Vector2(pos.x - 0.16f, pos.y), GroundLayer).gameObject.GetComponent<BlockScript>().Refresh();
                                if (Physics2D.OverlapPoint(new Vector2(pos.x, pos.y + 0.16f), GroundLayer) == true)
                                    Physics2D.OverlapPoint(new Vector2(pos.x, pos.y + 0.16f), GroundLayer).gameObject.GetComponent<BlockScript>().Refresh();
                                if (Physics2D.OverlapPoint(new Vector2(pos.x, pos.y - 0.16f), GroundLayer) == true)
                                    Physics2D.OverlapPoint(new Vector2(pos.x, pos.y - 0.16f), GroundLayer).gameObject.GetComponent<BlockScript>().Refresh();
                            }
                            ItemArray[currentItem].Count--;
                            if (ItemArray[currentItem].Count <= 0)
                            {
                                ItemArray[currentItem] = new Item();
                            }
                        }
                }
        if (Input.GetMouseButton(0))
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0;
            if (Vector3.Distance(pos, transform.position) < 1.5f)
                if (Physics2D.OverlapPoint(new Vector2(pos.x, pos.y), DestroyMask))
                {
                    if (Physics2D.OverlapPoint(new Vector2(pos.x, pos.y), DestroyMask).GetComponent<Transform>().childCount > 0) 
                        Destroy(Physics2D.OverlapPoint(new Vector2(pos.x, pos.y), DestroyMask).GetComponent<Transform>().GetChild(0).gameObject);
                    else
                    Physics2D.OverlapPoint(new Vector2(pos.x, pos.y), DestroyMask).GetComponent<BlockScript>().health -= Math.Max(1, (int)(1000 * Time.deltaTime));
                    if (Physics2D.OverlapPoint(new Vector2(pos.x, pos.y), DestroyMask).GetComponent<BlockScript>().health < 0)
                    {
                        Physics2D.OverlapPoint(new Vector2(pos.x, pos.y), DestroyMask).GetComponent<BlockScript>().health = 0;
                        Physics2D.OverlapPoint(new Vector2(pos.x, pos.y), DestroyMask).GetComponent<BlockScript>().RemoveBlock();
                    }
                }
        }

	}
    void FixedUpdate()
    {

    }
    public void AddItem(Item item)
    {
        for (int i = 0; i < 40; i++)
        {
            if (ItemArray[i].ItemGO == null)
            {
                ItemArray[i] = new Item(item.Id, item.Count);
                break;
            }
            else if (ItemArray[i].Id == item.Id)
                if (ItemArray[i].Count < MaxStack)
                {
                    ItemArray[i].Count += item.Count;
                    break;
                }
        }
    }
    void OnGUI()
    {
        var hotbarX = Screen.width / 2 - HotItemTex.width / 2;
        GUI.DrawTexture(new Rect(hotbarX, 0, HotItemTex.width, HotItemTex.height), HotItemTex);
        for (int i = 0; i < 10; ++i)
        {
            if (ItemArray[i].Id > 0 && ItemArray[i].Count > 0)
            {
                var texture = textureFromSprite(ItemArray[i].Icon);
                var color = GUI.color;
                var fadecolor = color;
                if (i != currentItem)
                {
                    fadecolor.a = 0.7f;
                }
                GUI.color = fadecolor;
                GUI.DrawTexture(
                    new Rect(
                        hotbarX + 43 + (HotItemTex.width / 12.0f) * (i), 
                        1,
                        40,
                        40
                    ),
                    texture
                );
                GUI.Label(
                    new Rect(
                        hotbarX + 33 + (HotItemTex.width / 12.0f) * (i + 1),
                        1,
                        40,
                        40
                    ), 
                    ItemArray[i].Count.ToString()
                );
                GUI.color = color;
                Destroy(texture); // should be cached instead
            }
        }
        if(InventoryState==true)
            GUI.DrawTexture(new Rect(Screen.width/2-InventoryTex.width/2,Screen.height/2-InventoryTex.height/2,InventoryTex.width,InventoryTex.height),InventoryTex);
    }
    public static Texture2D textureFromSprite(Sprite sprite)
    {
        if (sprite.rect.width != sprite.texture.width)
        {
            Texture2D newText = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
            Color[] newColors = sprite.texture.GetPixels((int)sprite.textureRect.x,
                                                         (int)sprite.textureRect.y,
                                                         (int)sprite.textureRect.width,
                                                         (int)sprite.textureRect.height);
            newText.SetPixels(newColors);
            newText.Apply();
            return newText;
        }
        else
            return sprite.texture;
    }

    public struct Item
    {
        readonly public int Id;
        readonly public string Name;
        readonly public string Description;
        readonly public Sprite Icon;
        readonly public GameObject ItemGO;
        public int Count;

        public Item (string name, string description, Sprite icon, GameObject gameobject,int id,int count)
        {
            this.Id = id;
            this.Name = name;
            this.Description = description;
            this.Icon = icon;
            this.ItemGO = gameobject;
            this.Count = count;
        }
        public Item(int id, int count)
        {
            this.Id = id;
            this.Count = count;
            this.Name = "Default";
            this.Description = "Defau;t Description";
            this.Icon = null;       
            this.ItemGO = new GameObject();
            for (int i = 0; i < ItemLibrary.Library.Length; i++)
                if (ItemLibrary.Library[i].Id == id)
                {
                    this.Name = ItemLibrary.Library[i].Name;
                    this.Icon = ItemLibrary.Library[i].Icon;
                    this.Description = ItemLibrary.Library[i].Description;
                    this.ItemGO = ItemLibrary.Library[i].ItemGO;
                    }

        }
    }
    public struct ItemLibrary
    {
        public static Item[] Library = new Item[] { new Item(),
                                                    new Item("Земля", "Обычная земля.", Sprite.Create((Texture2D)Resources.Load("Sprites/GUI/ItemSheet"), new Rect(80 * 4, 0, 64, 64), new Vector2(0.5f, 0.5f)), (GameObject)Resources.Load("prefab/Block/Dirt", typeof(GameObject)), 1, 1),
                                                    new Item("Камень", "Крепкий камень.", Sprite.Create((Texture2D)Resources.Load("Sprites/GUI/ItemSheet"), new Rect(64 * 4, 0, 64, 64), new Vector2(0.5f, 0.5f)), (GameObject)Resources.Load("prefab/Block/Stone", typeof(GameObject)), 2, 1),
                                                    new Item("Уголь", "Хорошо горит.", Sprite.Create((Texture2D)Resources.Load("Sprites/GUI/ItemSheet"), new Rect(0, 0, 64, 64), new Vector2(0.5f, 0.5f)), (GameObject)Resources.Load("prefab/Block/CoalOre", typeof(GameObject)), 3, 1),
                                                    new Item("Медная руда", "Можно переплавить в медь.", Sprite.Create((Texture2D)Resources.Load("Sprites/GUI/ItemSheet"), new Rect(32 * 4, 0, 64, 64), new Vector2(0.5f, 0.5f)), (GameObject)Resources.Load("prefab/Block/CopperOre", typeof(GameObject)), 4, 1),
                                                    new Item("Оловянная руда", "Можно переплавить в олово.", Sprite.Create((Texture2D)Resources.Load("Sprites/GUI/ItemSheet"), new Rect(48 * 4, 0, 64, 64), new Vector2(0.5f, 0.5f)), (GameObject)Resources.Load("prefab/Block/TinOre", typeof(GameObject)), 5, 1),
                                                    new Item("Железная руда", "Можно переплавить в железо.", Sprite.Create((Texture2D)Resources.Load("Sprites/GUI/ItemSheet"), new Rect(16 * 4, 0, 64, 64), new Vector2(0.5f, 0.5f)), (GameObject)Resources.Load("prefab/Block/IronOre", typeof(GameObject)), 6, 1)
        };
    }
}
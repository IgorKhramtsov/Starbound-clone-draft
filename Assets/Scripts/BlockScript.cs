using UnityEngine;
using System.Collections;

public class BlockScript : MonoBehaviour {


    public bool MultiTexture = false;
    public string Type;
    private SpriteRenderer SprR;
    private LayerMask GroundMask;
    private LayerMask VegetationMask;
    private const int WaterMask = 1 << 4;
    private const int GroundLayer = 1 << 9;
    public Texture2D Tex;
    public GameObject Grass;
    public bool State=false;
    public GameObject DroppedObj;
    public int health;
    // Use this for initialization
	void Start () {
        GroundMask = 1 << 9;
        GroundMask += 1 << 10;
        VegetationMask = 1 << 9;
        VegetationMask += 1 << 11;
        switch (Type)
        {
            case "Dirt":
                health = 100;
                break;
            case "Stone":
                health = 250;
                break;
            case "CoalOre":
                health = 300;
                break;
            case "IronOre":
                health = 300;
                break;
            case "CopperOre":
                health = 300;
                break;
            case "TinOre":
                health = 300;
                break;
        }
        Refresh();
        if(Type == "Dirt")
        SetVegetation();
	}
	// Update is called once per frame
	void Update () {

	}
    public void Refresh()
    {
        if (MultiTexture==true)
            {
                SprR = GetComponent<SpriteRenderer>();
                if (CheckGround(0, 0.16f) == false && CheckGround(0, -0.16f) == false)
                {
                    SprR.sprite = spr("BT");
                    if (CheckGround(0.16f, 0) == false && CheckGround(-0.16f, 0) == false)
                        SprR.sprite = spr("RBLT");
                    else if (CheckGround(0.16f, 0) == false)
                        SprR.sprite = spr("RBT");
                    else if (CheckGround(-0.16f, 0) == false)
                        SprR.sprite = spr("BLT");

                }
                else if (CheckGround(0, 0.16f) == false)
                {
                    SprR.sprite = spr("T");
                    if (CheckGround(0.16f, 0) == false && CheckGround(-0.16f, 0) == false)
                        SprR.sprite = spr("RLT");
                    else if (CheckGround(0.16f, 0) == false)
                    {
                        SprR.sprite = spr("RT");
                        if (CheckGround(0, -0.16f) == false)
                            SprR.sprite = spr("RBT");
                    }
                    else if (CheckGround(-0.16f, 0) == false)
                    {
                        SprR.sprite = spr("LT");
                        if (CheckGround(0, -0.16f) == false)
                            SprR.sprite = spr("BLT");
                    }
                }
                else if (CheckGround(0, -0.16f) == false)
                {
                    SprR.sprite = spr("B");
                    if (CheckGround(-0.16f, 0) == false && CheckGround(0.16f, 0) == false)
                        SprR.sprite = spr("RBL");
                    else if (CheckGround(-0.16f, 0) == false)
                        SprR.sprite = spr("BL");
                    else if (CheckGround(0.16f, 0) == false)
                        SprR.sprite = spr("RB");
                }
                else if (CheckGround(0.16f, 0) == false && CheckGround(-0.16f, 0) == false)
                    SprR.sprite = spr("RL");
                else if (CheckGround(0.16f, 0) == false)
                    SprR.sprite = spr("R");
                else if (CheckGround(-0.16f, 0) == false)
                    SprR.sprite = spr("L");
                else
                    SprR.sprite = spr("Main");
            }
    }
    public void SetVegetation()
    {
        if (Physics2D.OverlapPoint(new Vector2(transform.position.x, transform.position.y + 0.16f), VegetationMask)==false && Physics2D.OverlapPoint(new Vector2(transform.position.x, transform.position.y + 0.16f), WaterMask)==false)
            SetGrass();
    }
    public void RemoveBlock()
    {
        Instantiate(DroppedObj, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    void OnDestroy()
    {
        RefreshAround();
    }
    void RefreshAround()
    {
        if (Physics2D.OverlapPoint(new Vector2(transform.position.x + 0.16f, transform.position.y), WaterMask))
            Physics2D.OverlapPoint(new Vector2(transform.position.x + 0.16f, transform.position.y), WaterMask).gameObject.GetComponent<WaterScript>().active();

        if (Physics2D.OverlapPoint(new Vector2(transform.position.x - 0.16f, transform.position.y), WaterMask))
            Physics2D.OverlapPoint(new Vector2(transform.position.x - 0.16f, transform.position.y), WaterMask).gameObject.GetComponent<WaterScript>().active();

        if (Physics2D.OverlapPoint(new Vector2(transform.position.x, transform.position.y + 0.16f), WaterMask))
            Physics2D.OverlapPoint(new Vector2(transform.position.x, transform.position.y + 0.16f), WaterMask).gameObject.GetComponent<WaterScript>().active();

        if (Physics2D.OverlapPoint(new Vector2(transform.position.x, transform.position.y - 0.16f), WaterMask))
            Physics2D.OverlapPoint(new Vector2(transform.position.x, transform.position.y - 0.16f), WaterMask).gameObject.GetComponent<WaterScript>().active();

        if (Physics2D.OverlapPoint(new Vector2(transform.position.x + 0.16f, transform.position.y), GroundLayer) == true)
            if (Physics2D.OverlapPoint(new Vector2(transform.position.x + 0.16f, transform.position.y), GroundLayer).GetComponent<BlockScript>() == true)
                Physics2D.OverlapPoint(new Vector2(transform.position.x + 0.16f, transform.position.y), GroundLayer).gameObject.GetComponent<BlockScript>().Refresh();
        if (Physics2D.OverlapPoint(new Vector2(transform.position.x - 0.16f, transform.position.y), GroundLayer) == true)
            if (Physics2D.OverlapPoint(new Vector2(transform.position.x - 0.16f, transform.position.y), GroundLayer).gameObject.GetComponent<BlockScript>() == true)
                Physics2D.OverlapPoint(new Vector2(transform.position.x - 0.16f, transform.position.y), GroundLayer).gameObject.GetComponent<BlockScript>().Refresh();
        if (Physics2D.OverlapPoint(new Vector2(transform.position.x, transform.position.y + 0.16f), GroundLayer) == true)
            if (Physics2D.OverlapPoint(new Vector2(transform.position.x, transform.position.y + 0.16f), GroundLayer).gameObject.GetComponent<BlockScript>() == true)
                Physics2D.OverlapPoint(new Vector2(transform.position.x, transform.position.y + 0.16f), GroundLayer).gameObject.GetComponent<BlockScript>().Refresh();
        if (Physics2D.OverlapPoint(new Vector2(transform.position.x, transform.position.y - 0.16f), GroundLayer) == true)
            if (Physics2D.OverlapPoint(new Vector2(transform.position.x, transform.position.y - 0.16f), GroundLayer).gameObject.GetComponent<BlockScript>() == true)
                Physics2D.OverlapPoint(new Vector2(transform.position.x, transform.position.y - 0.16f), GroundLayer).gameObject.GetComponent<BlockScript>().Refresh();
          
    }
    private bool CheckGround (float xOffset,float yOffset)
{
        Vector2 Pos = new Vector2(transform.position.x+xOffset,transform.position.y+yOffset);
        if(Physics2D.OverlapPoint(Pos,GroundMask)==true)
            return true;
        else
            return false;
}
    private Sprite spr(string place)
    {
        Sprite sprite = null; // new Sprite();
        if (place == "LT")
            sprite = Sprite.Create(Tex, new Rect(32, 64, 16, 16), new Vector2(0.5f, 0.5f));
        else if (place == "RT")
            sprite = Sprite.Create(Tex, new Rect(48, 64, 16, 16), new Vector2(0.5f, 0.5f));
        else if (place == "RBLT")
            sprite = Sprite.Create(Tex, new Rect(64, 64, 16, 16), new Vector2(0.5f, 0.5f));
        else if (place == "BL")
            sprite = Sprite.Create(Tex, new Rect(32, 48, 16, 16), new Vector2(0.5f, 0.5f));
        else if (place == "RB")
            sprite = Sprite.Create(Tex, new Rect(48, 48, 16, 16), new Vector2(0.5f, 0.5f));
        else if (place == "BT")
            sprite = Sprite.Create(Tex, new Rect(0, 32, 16, 16), new Vector2(0.5f, 0.5f));
        else if (place == "RL")
            sprite = Sprite.Create(Tex, new Rect(16, 32, 16, 16), new Vector2(0.5f, 0.5f));
        else if (place == "RLT")
            sprite = Sprite.Create(Tex, new Rect(48, 32, 16, 16), new Vector2(0.5f, 0.5f));
        else if (place == "T")
            sprite = Sprite.Create(Tex, new Rect(0, 16, 16, 16), new Vector2(0.5f, 0.5f));
        else if (place == "L")
            sprite = Sprite.Create(Tex, new Rect(16, 16, 16, 16), new Vector2(0.5f, 0.5f));
        else if (place == "BLT")
            sprite = Sprite.Create(Tex, new Rect(32, 16, 16, 16), new Vector2(0.5f, 0.5f));
        else if (place == "Main")
            sprite = Sprite.Create(Tex, new Rect(48, 16, 16, 16), new Vector2(0.5f, 0.5f));
       else if(place == "RBT")
            sprite = Sprite.Create(Tex, new Rect(64, 16, 16, 16), new Vector2(0.5f, 0.5f));
        else if(place == "B")
            sprite = Sprite.Create(Tex, new Rect(0, 0, 16, 16), new Vector2(0.5f, 0.5f));
       else if(place == "R")
            sprite = Sprite.Create(Tex, new Rect(16, 0, 16, 16), new Vector2(0.5f, 0.5f));
       else if(place == "RBL")
            sprite = Sprite.Create(Tex, new Rect(48, 0, 16, 16), new Vector2(0.5f, 0.5f));
        sprite.name = "sprite_"+place;
        return sprite;
    }
    private void SetGrass()
    {
        GameObject grass = (GameObject)Instantiate(Grass, new Vector3(transform.position.x, transform.position.y + 0.11f), Quaternion.identity);
        grass.transform.parent = transform;
    }
}

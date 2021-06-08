using UnityEngine;
using System.Collections;

public class WaterScript : MonoBehaviour {
	public int Mass;
	public bool Active;
    public Texture2D WaterTex;
    public int SleepTime=60;
    private int SleepCount;
    public int OldMass;
    private SpriteRenderer SprR;
	private const int WaterMask = 1 << 4;
    private const int GroundMask = 1 << 9;
    public float[] LeftRight = new float[4];
    public float[] Left = new float[4];
    public float[] Right = new float[4];
    public float[] ELSE = new float[4];
	// Use this for initialization
	void Start () {
        SprR = GetComponent<SpriteRenderer>();
		Active = true;
	}
    void Update() {
        if (Active == true)
        {
            if (OldMass == Mass && (int)Time.time % 10 == 0)
            {
                SleepCount++;
                if (SleepCount == 1)
                {
                    DeActive();
                }
            }
            else
            {
                OldMass = Mass;
                if (!((int)Time.time % 10 == 0))
                SleepCount = 0;
            }
        }
        else
        {
            if (OldMass != Mass)
            {
                active();
                OldMass = Mass;
            }
            if (!((int)Time.time % 10 == 0))
            SleepCount = 0;
        }

        DrawWater();
        SimulateWater();
    }
    void DrawWater()
    {
        if (SprR.sprite != null)
        {
            DestroyImmediate(SprR.sprite, true);
        }
        Sprite sprite;
        if (Physics2D.OverlapPoint(new Vector2(transform.position.x - 0.16f, transform.position.y), GroundMask) == true && Physics2D.OverlapPoint(new Vector2(transform.position.x + 0.16f, transform.position.y), GroundMask) == true)
        {
            if (Physics2D.OverlapPoint(new Vector2(transform.position.x, transform.position.y + 0.16f), WaterMask))
            {
                sprite = Sprite.Create(WaterTex, new Rect(130, 0, 26f, 16f), new Vector2(0.5f, 0.5f));
                if(Physics2D.OverlapPoint(new Vector2(transform.position.x, transform.position.y - 0.16f), WaterMask))
                    sprite = Sprite.Create(WaterTex, new Rect(130, 0, 26f, 16f), new Vector2(0.5f, 0.5f));
            }
            else if (Physics2D.OverlapPoint(new Vector2(transform.position.x, transform.position.y - 0.16f), GroundMask))
            {
            if (Mass >= 99)
                sprite = Sprite.Create(WaterTex, new Rect(0, 0, 26f, 18f), new Vector2(0.515f, 0.55f));
            else if (Mass >= 75)
                sprite = Sprite.Create(WaterTex, new Rect(26, 0, 26f, 18f), new Vector2(0.515f, 0.55f));
            else if (Mass >= 50)
                sprite = Sprite.Create(WaterTex, new Rect(52, 0, 26f, 18f), new Vector2(0.515f, 0.55f));
            else if (Mass >= 25)
                sprite = Sprite.Create(WaterTex, new Rect(78, 0, 26f, 18f), new Vector2(0.515f, 0.55f));
            else
                sprite = Sprite.Create(WaterTex, new Rect(104, 0, 26f, 18f), new Vector2(0.515f, 0.55f));
            }
            else if (Mass >= 99)
                sprite = Sprite.Create(WaterTex, new Rect(0, 0, 26f, 16f), new Vector2(0.515f, 0.5f));
            else if (Mass >= 75)
                sprite = Sprite.Create(WaterTex, new Rect(26, 0, 26f, 16f), new Vector2(0.515f, 0.5f));
            else if (Mass >= 50)
                sprite = Sprite.Create(WaterTex, new Rect(52, 0, 26f, 16f), new Vector2(0.515f, 0.5f));
            else if (Mass >= 25)
                sprite = Sprite.Create(WaterTex, new Rect(78, 0, 26f, 16f), new Vector2(0.515f, 0.5f));
            else
                sprite = Sprite.Create(WaterTex, new Rect(104, 0, 26f, 16f), new Vector2(0.515f, 0.5f));
        }
        else if (Physics2D.OverlapPoint(new Vector2(transform.position.x - 0.16f, transform.position.y), GroundMask) == true)//Left
        {
            if (Physics2D.OverlapPoint(new Vector2(transform.position.x, transform.position.y + 0.16f), WaterMask))
            {
                if (Physics2D.OverlapPoint(new Vector2(transform.position.x, transform.position.y - 0.16f), WaterMask))
                    sprite = Sprite.Create(WaterTex, new Rect(130, 0, 20f, 16f), new Vector2(0.6f, 0.5f));
                else
                    sprite = Sprite.Create(WaterTex, new Rect(130, 0, 20f, 18f), new Vector2(0.6f, 0.55f));
            }
                else if(Physics2D.OverlapPoint(new Vector2(transform.position.x, transform.position.y - 0.16f), GroundMask))
                {
            if (Mass >= 99)
                sprite = Sprite.Create(WaterTex, new Rect(0, 0, 20f, 18f), new Vector2(0.6f, 0.55f));
            else if (Mass >= 75)
                sprite = Sprite.Create(WaterTex, new Rect(26, 0, 20f, 18f), new Vector2(0.6f, 0.55f));
            else if (Mass >= 50)
                sprite = Sprite.Create(WaterTex, new Rect(52, 0, 20f, 18f), new Vector2(0.6f, 0.55f));
            else if (Mass >= 25)
                sprite = Sprite.Create(WaterTex, new Rect(78, 0, 20f, 18f), new Vector2(0.6f, 0.55f));
            else
                sprite = Sprite.Create(WaterTex, new Rect(104, 0, 20f, 18f), new Vector2(0.6f, 0.55f));
                }
            else if (Mass >= 99)
                sprite = Sprite.Create(WaterTex, new Rect(0, 2, 20f, 16f), new Vector2(0.6f, 0.5f));
            else if (Mass >= 75)
                sprite = Sprite.Create(WaterTex, new Rect(26, 2, 20f, 16f), new Vector2(0.6f, 0.5f));
            else if (Mass >= 50)
                sprite = Sprite.Create(WaterTex, new Rect(52, 2, 20f, 16f), new Vector2(0.6f, 0.5f));
            else if (Mass >= 25)
                sprite = Sprite.Create(WaterTex, new Rect(78, 2, 20f, 16), new Vector2(0.6f, 0.5f));
            else
                sprite = Sprite.Create(WaterTex, new Rect(104, 2, 20f, 16f), new Vector2(0.6f, 0.5f));
        }
        else if (Physics2D.OverlapPoint(new Vector2(transform.position.x + 0.16f, transform.position.y), GroundMask) == true)//Right
        {
            if (Physics2D.OverlapPoint(new Vector2(transform.position.x, transform.position.y + 0.16f), WaterMask))
            {
                if (Physics2D.OverlapPoint(new Vector2(transform.position.x, transform.position.y - 0.16f), WaterMask))
                    sprite = Sprite.Create(WaterTex, new Rect(130, 0, 20f, 16f), new Vector2(0.4f, 0.5f));
                else
                    sprite = Sprite.Create(WaterTex, new Rect(130F, 0, 20f, 18f), new Vector2(0.4f, 0.55f));
            }
            else if(Physics2D.OverlapPoint(new Vector2(transform.position.x, transform.position.y - 0.16f), GroundMask))
            {
            if (Mass >= 99)
                sprite = Sprite.Create(WaterTex, new Rect(0, 0, 20f, 18f), new Vector2(0.4f, 0.55f));
            else if (Mass >= 75)
                sprite = Sprite.Create(WaterTex, new Rect(26F, 0, 20f, 18f), new Vector2(0.4f, 0.55f));
            else if (Mass >= 50)
                sprite = Sprite.Create(WaterTex, new Rect(52F, 0, 20f, 18f), new Vector2(0.4f, 0.55f));
            else if (Mass >= 25)
                sprite = Sprite.Create(WaterTex, new Rect(78F, 0, 20f, 18f), new Vector2(0.4f, 0.55f));
            else
                sprite = Sprite.Create(WaterTex, new Rect(104F, 0, 20f, 18f), new Vector2(0.4f, 0.55f));
            }
            else if (Mass >= 99)
                sprite = Sprite.Create(WaterTex, new Rect(0, 2, 20f, 16f), new Vector2(0.4f, 0.5f));
            else if (Mass >= 75)
                sprite = Sprite.Create(WaterTex, new Rect(26F, 2, 20f, 16f), new Vector2(0.4f, 0.5f));
            else if (Mass >= 50)
                sprite = Sprite.Create(WaterTex, new Rect(52F, 2, 20f, 16f), new Vector2(0.4f, 0.5f));
            else if (Mass >= 25)
                sprite = Sprite.Create(WaterTex, new Rect(78F, 2, 20f, 16f), new Vector2(0.4f, 0.5f));
            else
                sprite = Sprite.Create(WaterTex, new Rect(104F, 2, 20f, 16f), new Vector2(0.4f, 0.5f));
        }
        else
        {
            if (Physics2D.OverlapPoint(new Vector2(transform.position.x, transform.position.y + 0.16f), WaterMask))
            {
                if (Physics2D.OverlapPoint(new Vector2(transform.position.x, transform.position.y - 0.16f), WaterMask))
                    sprite = Sprite.Create(WaterTex, new Rect(130f, 0, 16f, 16f), new Vector2(0.5f, 0.5f));
                else
                    sprite = Sprite.Create(WaterTex, new Rect(130f, 0, 16f, 18f), new Vector2(0.5f, 0.55f));
            }
            else if (Physics2D.OverlapPoint(new Vector2(transform.position.x, transform.position.y - 0.16f), GroundMask))
            {
            if (Mass >= 99)
                sprite = Sprite.Create(WaterTex, new Rect(0, 0, 16f, 18f), new Vector2(0.5f, 0.55f));
            else if (Mass >= 75)
                sprite = Sprite.Create(WaterTex, new Rect(26, 0, 16f, 18f), new Vector2(0.5f, 0.5f));
            else if (Mass >= 50)
                sprite = Sprite.Create(WaterTex, new Rect(52, 0, 16f, 18f), new Vector2(0.5f, 0.5f));
            else if (Mass >= 25)
                sprite = Sprite.Create(WaterTex, new Rect(78, 0, 16f, 18f), new Vector2(0.5f, 0.5f));
            else
                sprite = Sprite.Create(WaterTex, new Rect(104, 0, 16f, 18f), new Vector2(0.5f, 0.5f));
            }
            else if (Mass >= 99)
                sprite = Sprite.Create(WaterTex, new Rect(0, 2, 16f, 16f), new Vector2(0.5f, 0.5f));
            else if (Mass >= 75)
                sprite = Sprite.Create(WaterTex, new Rect(26, 2, 16f, 16f), new Vector2(0.5f, 0.5f));
            else if (Mass >= 50)
                sprite = Sprite.Create(WaterTex, new Rect(52, 2, 16f, 16f), new Vector2(0.5f, 0.5f));
            else if (Mass >= 25)
                sprite = Sprite.Create(WaterTex, new Rect(78, 2, 16f, 16f), new Vector2(0.5f, 0.5f));
            else
                sprite = Sprite.Create(WaterTex, new Rect(104, 2, 16f, 16f), new Vector2(0.5f, 0.5f));
        }
        

        SprR.sprite = sprite;
    }
	void SimulateWater()
	{
		if(Mass<=0)
			Destroy(gameObject);
		else if(Active==true)
		{
            if (CheckBlock(transform.position.x, transform.position.y - 0.16f, WaterMask) == true && GetBlockMass(transform.position.x, transform.position.y - 0.16f) < 100)//Если снизу есть блок
            {
                    Transfuse(GetBlock(transform.position.x, transform.position.y - 0.16f, WaterMask),true);//Переливаемся
            }
            else if (CheckBlock(transform.position.x, transform.position.y - 0.16f, GroundMask) == false && CheckBlock(transform.position.x, transform.position.y - 0.16f, WaterMask) == false)//Иначе если снизу нет блока
            {
                CreateWater(transform.position.x, transform.position.y - 0.16f);//создаем воду
                Transfuse(GetBlock(transform.position.x, transform.position.y - 0.16f, WaterMask),true);//Переливаемся
            }
            else
            {
                GoRight();
                GoLeft();
            }               
		}
	}
    public void DeActive()
    {
        if ((CheckBlock(transform.position.x + 0.16f, transform.position.y, WaterMask) == true || CheckBlock(transform.position.x + 0.16f, transform.position.y, GroundMask) == true) && (CheckBlock(transform.position.x - 0.16f, transform.position.y, WaterMask) == true||CheckBlock(transform.position.x - 0.16f, transform.position.y, GroundMask) == true) &&( CheckBlock(transform.position.x, transform.position.y - 0.16f, WaterMask) == true || CheckBlock(transform.position.x, transform.position.y - 0.16f, GroundMask) == true))
        {
            Active = false;
            if (CheckBlock(transform.position.x + 0.16f, transform.position.y, WaterMask) == true)
                if (GetBlock(transform.position.x + 0.16f, transform.position.y, WaterMask).GetComponent<WaterScript>().Active == true)
                    GetBlock(transform.position.x + 0.16f, transform.position.y, WaterMask).GetComponent<WaterScript>().DeActive();

            if (CheckBlock(transform.position.x - 0.16f, transform.position.y, WaterMask) == true)
                if (GetBlock(transform.position.x - 0.16f, transform.position.y, WaterMask).GetComponent<WaterScript>().Active == true)
                    GetBlock(transform.position.x - 0.16f, transform.position.y, WaterMask).GetComponent<WaterScript>().DeActive();
        }
    }
    public void active()
    {
        SleepCount = 0;
        Active = true;
        if (CheckBlock(transform.position.x + 0.16f, transform.position.y, WaterMask) == true)
            if (GetBlock(transform.position.x + 0.16f, transform.position.y, WaterMask).GetComponent<WaterScript>().Active == false)
                GetBlock(transform.position.x + 0.16f, transform.position.y, WaterMask).GetComponent<WaterScript>().active();

        if (CheckBlock(transform.position.x - 0.16f, transform.position.y, WaterMask) == true)
            if (GetBlock(transform.position.x - 0.16f, transform.position.y, WaterMask).GetComponent<WaterScript>().Active == false)
                GetBlock(transform.position.x - 0.16f, transform.position.y, WaterMask).GetComponent<WaterScript>().active();

        if (CheckBlock(transform.position.x, transform.position.y - 0.16f, WaterMask) == true)
            if (GetBlock(transform.position.x, transform.position.y - 0.16f, WaterMask).GetComponent<WaterScript>().Active == false)
                GetBlock(transform.position.x, transform.position.y - 0.16f, WaterMask).GetComponent<WaterScript>().active();

        if (CheckBlock(transform.position.x, transform.position.y + 0.16f, WaterMask) == true)
            if (GetBlock(transform.position.x, transform.position.y + 0.16f, WaterMask).GetComponent<WaterScript>().Active == false)
                GetBlock(transform.position.x, transform.position.y + 0.16f, WaterMask).GetComponent<WaterScript>().active();
    }
    void GoRight()
    {
        if (CheckBlock(transform.position.x, transform.position.y - 0.16f, WaterMask) == true && GetBlockMass(transform.position.x, transform.position.y - 0.16f) < 100)//Если снизу есть блок
        {
            Transfuse(GetBlock(transform.position.x, transform.position.y - 0.16f, WaterMask), false);//Переливаемся
        }
        else if (CheckBlock(transform.position.x, transform.position.y - 0.16f, GroundMask) == false && CheckBlock(transform.position.x, transform.position.y - 0.16f, WaterMask) == false)//Иначе если снизу нет блока
        {
            CreateWater(transform.position.x, transform.position.y - 0.16f);//создаем воду
            Transfuse(GetBlock(transform.position.x, transform.position.y - 0.16f, WaterMask), false);//Переливаемся
        }
        else
        {
            if (CheckBlock(transform.position.x + 0.16f, transform.position.y, WaterMask) == true && GetBlockMass(transform.position.x + 0.16f, transform.position.y) < 100)//Если справа есть блок
            {
                Transfuse(GetBlock(transform.position.x + 0.16f, transform.position.y, WaterMask), false);//Переливаемся
            }
            else if (CheckBlock(transform.position.x + 0.16f, transform.position.y, GroundMask) == false && CheckBlock(transform.position.x + 0.16f, transform.position.y, WaterMask) == false  && Mass > 10)//Иначе еесли справа нет блока
            {
                CreateWater(transform.position.x + 0.16f, transform.position.y);//Создаем воду
                Transfuse(GetBlock(transform.position.x + 0.16f, transform.position.y, WaterMask), false);//Переливаемся
            }
        }


    }
    void GoLeft()
    {
        if (CheckBlock(transform.position.x, transform.position.y - 0.16f, WaterMask) == true && GetBlockMass(transform.position.x, transform.position.y - 0.16f) < 100)//Если снизу есть блок
        {
            Transfuse(GetBlock(transform.position.x, transform.position.y - 0.16f, WaterMask), false);//Переливаемся
        }
        else if (CheckBlock(transform.position.x, transform.position.y - 0.16f, GroundMask) == false && CheckBlock(transform.position.x, transform.position.y - 0.16f, WaterMask) == false)//Иначе если снизу нет блока
        {
            CreateWater(transform.position.x, transform.position.y - 0.16f);//создаем воду
            Transfuse(GetBlock(transform.position.x, transform.position.y - 0.16f, WaterMask), false);//Переливаемся
        }
        else
        {
            if (CheckBlock(transform.position.x - 0.16f, transform.position.y, WaterMask) == true && GetBlockMass(transform.position.x - 0.16f, transform.position.y) < 100)//Если слева есть блок
            {
                Transfuse(GetBlock(transform.position.x - 0.16f, transform.position.y, WaterMask), false);//Переливаемся
            }
            else if (CheckBlock(transform.position.x - 0.16f, transform.position.y, GroundMask) == false && CheckBlock(transform.position.x - 0.16f, transform.position.y, WaterMask) == false && Mass > 10)//Иначе создаем блок
            {
                CreateWater(transform.position.x - 0.16f, transform.position.y);
                Transfuse(GetBlock(transform.position.x - 0.16f, transform.position.y, WaterMask), false);
            }
        }
    }
	void Transfuse(GameObject Water,bool Down)
	{

		int transfuseMass = TransfuseMass (Water,Down);
		if(transfuseMass!=0)
		{
			Mass -= transfuseMass;
			Water.GetComponent<WaterScript>().Mass += transfuseMass;
		}
	}
	int TransfuseMass(GameObject nearWater,bool Down)
	{
		int nearMass = nearWater.GetComponent<WaterScript> ().Mass;
		int transfuseMass;
        if (nearMass < 100)
        {
            if (Down == true)
            {
                if (Mass <= 1)
                    transfuseMass = Mass;
                else
                    transfuseMass = Mass / 2;
                if (nearMass + transfuseMass > 100)
                    transfuseMass = 100 - nearMass;
            }
            else
            {
                if (Mass <= 1)
                    transfuseMass = Mass;
                else if (nearMass == Mass)
                    transfuseMass = 0;
                else if (nearMass < Mass)
                    transfuseMass = ((nearMass + Mass) / 2) - nearMass;
                else
                    transfuseMass = 0;
                if (nearMass + transfuseMass > 100)
                    transfuseMass = 100 - nearMass;
            }
        }
        else
            transfuseMass = 0;
        return transfuseMass;
	}
    int GetBlockMass(float x, float y)
    {
        Vector2 Position = new Vector2(x,y);
        if(Physics2D.OverlapPoint(Position,WaterMask))
        return Physics2D.OverlapPoint(Position, WaterMask).gameObject.GetComponent<WaterScript>().Mass;
        else 
        return -1;
    }
	bool CheckBlock(float x,float y)
	{
		Vector2 Position = new Vector2 (x, y);
		if(Physics2D.OverlapPoint(Position))
			return true;
		else 
			return false;
	}
	bool CheckBlock(float x,float y,int Mask)
	{
		Vector2 Position = new Vector2 (x, y);
		if(Physics2D.OverlapPoint(Position,Mask))
			return true;
		else 
			return false;
	}
	GameObject GetBlock(float x,float y,int Mask)
	{
		return Physics2D.OverlapPoint (new Vector2 (x, y),Mask).gameObject;
	}
	void CreateWater(float x, float y)
	{
		GameObject NewWater = (GameObject)Instantiate (gameObject, new Vector3 (x, y), Quaternion.identity);
		NewWater.name = "BlockWater";
		NewWater.GetComponent<WaterScript> ().Mass = 0;
	}

    void OnDestroy()
    {
        active();
    }
}

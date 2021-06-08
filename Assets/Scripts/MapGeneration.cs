using UnityEngine;
using System.Collections;

public class MapGeneration : MonoBehaviour {
    public int _Width;
    public int _Height;
	public GameObject[] Block;
	public GameObject[] Ore;
	public GameObject WaterBlock;
    public GameObject EdgeColl;
	public int Cave = 30;

	Texture2D noiseTex;
    Texture2D OreMap;
	private Color[] pix;
    private Color[] Orepix;
	float LeftEdgeWidth;
	float RightEdgeWidth;

	Vector3 Temp,lastTemp;
	
	public GameObject _Plane;

	public float DownEdge;
	public int CoalOre = 60;
	public int IronOre = 60;
	public int CopperOre = 60;
	public int TinOre = 60;
	[Range(0,1f)]
	public float ClearNoise=0;
	[Range(0,1f)]
	public float ClearNoiseCoalOre = 0;
	[Range(0,1f)]
	public float ClearNoiseIroneOre = 0;
	[Range(0,1f)]
	public float ClearNoiseCopperOre = 0;
	[Range(0,1f)]
	public float ClearNoiseTinOre = 0;

	private float LeftEdge=-1.0f;
	private float RightEdge;
	[HideInInspector]
	public float RightEdgeCollPosX;
    [HideInInspector]
    public float MaxHeightSurface;
    [HideInInspector]
    public float MaxLowestSurface;
    [HideInInspector]
    public int CoalOreCount,IronOreCount,CopperOreCount,TinOreCount;
	void Start () {
		GenerateAllMap (_Width);
        CreateCave((int)(RightEdge/0.16f), _Height, Random.Range(0, 10000), Random.Range(0, 1000), Cave);
		GenOre (GenOreMap (0, 0, CoalOre,ClearNoiseCoalOre ), 0);
		GenOre (GenOreMap (0, 0, IronOre,ClearNoiseIroneOre), 1);
		GenOre (GenOreMap (0, 0, CopperOre,ClearNoiseCopperOre), 2);
		GenOre (GenOreMap (0, 0, TinOre,ClearNoiseTinOre), 3);
		CreateWaterInCave ();
	}

	public void GenerateAllMap(int Size)
	{
		//Генерируем левый край(берег) карты
		Temp = new Vector3 (-1, _Height);
		lastTemp = Temp;
//		while (Temp.y<_Height)
//		{
//			Temp=new Vector3(lastTemp.x+1,Random.Range((int)lastTemp.y,(int)lastTemp.y+2));
//			if(Temp.y<=_Height*0.32f)
//				Instantiate(Block[2],Temp*0.32f,Quaternion.identity);
//			else
//				Instantiate(Block[0],Temp*0.32f,Quaternion.identity);
//
//			lastTemp=Temp;
//			Vector3 TMP = Temp;
//			LeftEdgeWidth=Temp.x*0.32f;
//			while(TMP.y>_Height*0.32f)
//			{
//				TMP.y--;
//				if((TMP.y+3f)>Temp.y){
//					if(TMP.y>_Height*0.32f)
//						Instantiate(Block[0],TMP*0.32f,Quaternion.identity);
//					else 
//					Instantiate(Block[2],TMP*0.32f,Quaternion.identity);
//				}
//				else if(!((TMP.y)>_Height*0.32f))
//					Instantiate(Block[2],TMP*0.32f,Quaternion.identity);
//				else
//					Instantiate(Block[1],TMP*0.32f,Quaternion.identity);
//			}
//			//Заполнение водой
//			/*
//			TMP = Temp;
//			while(TMP.y<_Height){
//				TMP.y++;
//				Instantiate(Block[3],TMP*0.32f,Quaternion.identity);
//			}
//			*/
//		}
		for(int _i=0;_i<_Width;_i++)
		{
			Temp = new Vector3(lastTemp.x+1,Random.Range((int)lastTemp.y-1,(int)lastTemp.y+2));
			Instantiate (Block [0], Temp * 0.16f, Quaternion.identity);
			if(Temp.y>MaxHeightSurface)
				MaxHeightSurface=Temp.y*0.16f;//Записываем самый высокий блок
			if(LeftEdge<0)
				LeftEdge = Temp.x*0.16f; //Записываем левый край карты
			RightEdge = Temp.x*0.16f;//Записываем правый край карты
			lastTemp=Temp;
			if(Temp.y*0.16f<MaxLowestSurface)
				MaxLowestSurface = Temp.y*0.16f;
				for(int i=0;i<(int)Random.Range((int)4,(int)7);i++)
				{
					Temp.y--;
					Instantiate(Block[0],Temp*0.16f,Quaternion.identity);
				}
				while  (Temp.y>0)
				{
					Temp.y--; 
					if(!((Temp.y)>0))
					Instantiate (Block [2], Temp*0.16f, Quaternion.identity);
					else
					Instantiate (Block [1], Temp*0.16f, Quaternion.identity);
					if(!((Temp.y-1)>0))
						DownEdge = Temp.y*0.16f;//Записываем нижний край карты
				}
		}
		//Генерируем правый край(берег) карты
//		Temp = lastTemp;
//		while (Temp.y>_Height*0.32f)
//		{
//			Temp=new Vector3(lastTemp.x+1,Random.Range((int)lastTemp.y-1,(int)lastTemp.y+1));
//			if(Temp.y<=_Height*0.32f)
//				Instantiate(Block[3],Temp*0.32f,Quaternion.identity);
//			else
//			Instantiate(Block[1],Temp*0.32f,Quaternion.identity);
//			lastTemp=Temp;
//			Vector3 TMP = Temp;
//			RightEdgeWidth=Temp.x;
//			while(TMP.y>_Height*0.32f)
//			{
//				TMP.y--;
//				if((TMP.y+3f)>Temp.y){
//					if(TMP.y>_Height*0.32f)
//					Instantiate(Block[1],TMP*0.32f,Quaternion.identity);
//					else 
//					Instantiate(Block[3],TMP*0.32f,Quaternion.identity);
//				}
//				else if(!((TMP.y)>_Height*0.32f))
//					Instantiate(Block[3],TMP*0.32f,Quaternion.identity);
//				else
//					Instantiate(Block[2],TMP*0.32f,Quaternion.identity);
//			}
//			//Заполнение водой
//			/*
//			while(TMP.y<_Height){
//				TMP.y++;
//				if(!Physics2D.OverlapPoint(new Vector2(TMP.x,TMP.y)*0.32f))
//				Instantiate(Block[4],TMP*0.32f,Quaternion.identity);
//				else
//					continue;
//			}
//			*/
//
//		}
//		RightEdgeCollPosX = Temp.x * 0.32f - 3f;
	}
    void CreateCave(int pixWidtnh, int pixHeight, float xOrg, float yOrg, float scale)
	{
        noiseTex = new Texture2D(pixWidtnh, pixHeight);
        pix = new Color[noiseTex.width * noiseTex.height];
        //_Plane.renderer.material.mainTexture = noiseTex;

        float y = 0.0F;
        while (y < noiseTex.height)
        {
            float x = 0.0F;
            while (x < noiseTex.width)
            {
                float xCoord = xOrg + x / noiseTex.width * scale;
                float yCoord = yOrg + y / noiseTex.height * scale;
                float sample = Mathf.PerlinNoise(yCoord, xCoord);
                pix[(int)(y * noiseTex.width + x)] = new Color(sample, sample, sample);
                x++;
            }
            y++;
        }
        noiseTex.SetPixels(pix);
        noiseTex.Apply();
        //Упрощение шума
        y = 0.0F;
        while (y < noiseTex.height)
        {
            float x = 0.0F;
            while (x < noiseTex.width)
            {
                if (pix[(int)(y * noiseTex.width + x)].r < ClearNoise)
                    pix[(int)(y * noiseTex.width + x)] = new Color(0, 0, 0);
                x++;
            }
            y++;
        }
        noiseTex.SetPixels(pix);
        noiseTex.Apply(); 

		for(int i=0;i<noiseTex.width;i++)
			for(int a=0;a<noiseTex.height;a++)
			{
				if(noiseTex.GetPixel(i,a).r>0f)
					if((Physics2D.OverlapPoint(new Vector2((float)i*0.16f,(float)a*0.16f),1<<9))==true)
						GameObject.DestroyObject((GameObject)Physics2D.OverlapPoint(new Vector2((float)i*0.16f,(float)a*0.16f),1<<9).gameObject);
			}
	}
	void CreateWaterInCave()
	{
		Instantiate (EdgeColl, new Vector3 (0f, 0f, 0f), Quaternion.identity);
		Instantiate (EdgeColl, new Vector3 (RightEdgeCollPosX, 0f, 0f), Quaternion.identity);
		for(int x=0;x<noiseTex.width;x++)
			for(int y=0;y<MaxHeightSurface/0.16f;y++)
				if(noiseTex.GetPixel(x,y).r>0.84f&&y*0.16f>DownEdge)
				{
				if(Physics2D.OverlapPoint(new Vector2(x,y)*0.16f))
					continue;
				else
					Instantiate(WaterBlock,new Vector3(x,y)*0.16f,Quaternion.identity);
				}

	}
	Texture2D GenOreMap(float xOrg,float yOrg,float scale,float ClearNoise)
	{
		OreMap = new Texture2D (noiseTex.width, noiseTex.height);
		Orepix = new Color[OreMap.width * OreMap.height];
		//_Plane.renderer.material.mainTexture = OreMap;

		float y = 0f;
		while (y<OreMap.height) {
			float x=0f;
			while (x<OreMap.width){
				float xCoord = xOrg+x/OreMap.width*scale;
				float yCoord = yOrg+y/OreMap.height*scale;
				float Sample = Mathf.PerlinNoise(yCoord,xCoord);
				Orepix[(int)(y*OreMap.width+x)] = new Color(Sample,Sample,Sample);
				x++;
			}
			y++;
		}
		OreMap.SetPixels (Orepix);
		OreMap.Apply ();
		//Упрощение шума
		y = 0.0F;
		while (y < OreMap.height) {
			float x = 0.0F;
			while (x < OreMap.width) {
				if(Orepix[(int)(y * OreMap.width + x)].r < ClearNoise)
					Orepix[(int)(y * OreMap.width + x)] = new Color(0,0,0);
				x++;
			}
			y++;
		}
		OreMap.SetPixels(Orepix);
		OreMap.Apply(); 
		return OreMap;
	}
	void GenOre (Texture2D Tex,int OreIndex)
	{
		for(int i=0;i<Tex.width;i++)
			for(int a=0;a<Tex.height;a++)
		{
			if(Tex.GetPixel(i,a).r>0f)
				if((Physics2D.OverlapPoint(new Vector2((float)i*0.16f,(float)a*0.16f),1<<9))){
					GameObject.DestroyObject((GameObject)Physics2D.OverlapPoint(new Vector2((float)i*0.16f,(float)a*0.16f),1<<9).gameObject);
					Instantiate(Ore[OreIndex],new Vector3(i,a,0)*.16f,Quaternion.identity);
                    switch(OreIndex)
                    {
                        case 0:
                            CoalOreCount++;
                            break;
                        case 1:
                            IronOreCount++;
                            break;
                        case 2:
                            CopperOreCount++;
                            break;
                        case 3:
                            TinOreCount++;
                            break;
                    }
			}
		}
	}



}
/*Виды руд
 * 1.Уголь
 * 2.Железо
 * 3.Медь
 * 4.Олово
 */
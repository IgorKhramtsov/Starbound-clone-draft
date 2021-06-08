using UnityEngine;
using System.Collections;

public class GrassScript : MonoBehaviour {
    public Texture2D Tex;
	// Use this for initialization
	void Start () {
        gameObject.GetComponent<SpriteRenderer>().sprite = Sprite.Create(Tex, new Rect(Random.Range(5,0) * 0.16f, 0, 16, 16), new Vector2(0.5f, 0.5f));
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

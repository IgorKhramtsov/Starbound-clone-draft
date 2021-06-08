using UnityEngine;
using System.Collections;

public class TestScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnTriggerEnter2D(Collider2D coll)
	{
		coll.GetComponent<Renderer>().enabled = true;
	}
	void OnTriggerExit2D(Collider2D coll)
	{
		coll.GetComponent<Renderer>().enabled = false;
	}
}

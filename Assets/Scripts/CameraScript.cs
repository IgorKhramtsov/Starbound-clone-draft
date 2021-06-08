using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour 
{
	public Transform targetTr;
    float fps = 0;
    float LastFps;

	public float damping = 1;
	public float lookAheadFactor = 3;
	public float lookAheadReturnSpeed = 0.5f;
	public float lookAheadMoveThreshold = 0.1f;
	float offsetRight;
    public float SizeFactor = 0.1f;
    [HideInInspector]
    public float RightOffset = 0;

	float offsetZ;
	Vector3 lastTargetPosition;
	Vector3 currentVelocity;
	Vector3 lookAheadPos;

	void Start () {
		lastTargetPosition = targetTr.position;
		offsetZ = (transform.position - targetTr.position).z;
		transform.parent = null;
	}
	void Update () 
	{

		//Следование камеры
		float xMoveDelta = (targetTr.position - lastTargetPosition).x;
		
		bool updateLookAheadTarget = Mathf.Abs(xMoveDelta) > lookAheadMoveThreshold;
		
		if (updateLookAheadTarget) {
			lookAheadPos = lookAheadFactor * Vector3.right * Mathf.Sign(xMoveDelta);
		} else {
			lookAheadPos = Vector3.MoveTowards(lookAheadPos, Vector3.zero, Time.deltaTime * lookAheadReturnSpeed);	
		}
		
		Vector3 aheadTargetPos = targetTr.position + lookAheadPos + Vector3.forward * offsetZ;
		Vector3 newPos = Vector3.SmoothDamp(transform.position, aheadTargetPos, ref currentVelocity, damping);

            transform.position = new Vector3(newPos.x, newPos.y,transform.position.z);
		lastTargetPosition = targetTr.position;
	}
	void OnGUI()
	{
        if (Time.time > LastFps + 0.35)
        {
            LastFps = Time.time;
            fps = 1.0f / Time.deltaTime;
        }
		GUI.Label (new Rect (0, 0, 100f, 20f), "FPS = "+fps.ToString("f1"));
        GUI.Label(new Rect(0, 20f, 100f, 20f), "Time: " + (int)Time.time);
        GUI.Label(new Rect(0, 40f, 200f, 20f), "TimeSinceStartup: " + (int)Time.realtimeSinceStartup);
	}
}
//AspectRatio
//if 5:4 to SizeFactor = 1.3
//if 4:3 to SizeFactor = 1.35
//if 3:2 to SizeFactor = 1.5
//if 16:10 to SizeFactor = 1.6
//if 16:9 to SizeFactor = 1.8

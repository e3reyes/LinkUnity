using UnityEngine;
using System.Collections;

public class CamFollow : MonoBehaviour {
	public GameObject targetToFollow;
	Transform t;
	// Use this for initialization
	void Start () {
		t = targetToFollow.transform;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3 (t.position.x, t.position.y, transform.position.z);
	}
}

using UnityEngine;
using System.Collections;

public class ChickenScript : MonoBehaviour {
	public GameObject target;
	//public LinkScript playerTarget;
	public float speed = .3f;
	public float maxDistance = 3;
	//public float offset = 16;
	public int lives = 100;
	public float dist;
	Animator anim;
	Transform t;
	public float distanceToAttack = .3f;
	// Use this for initialization
	void Start () {
		t = target.transform;
		anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if(lives < 1){
			Destroy(gameObject);
			t.GetComponent<LinkScript>().targets.Remove (gameObject.transform);
			t.GetComponent<LinkScript>().points++;
			
			
		}
		Vector3 pos = transform.position;
		dist = Vector3.Distance(t.transform.position, transform.position);
		//Debug.Log (dist);
		if(dist <= maxDistance && dist >distanceToAttack){
			if(pos.x < t.position.x){
				pos.x += 1 * speed* Time.deltaTime;
				anim.SetBool ("right", true);
			}
			else if(pos.x > t.position.x){
				pos.x += -1 * speed* Time.deltaTime;
				anim.SetBool ("right", false);
			}
			
			if(pos.y <= t.position.y){
				pos.y += 1 * speed* Time.deltaTime;
			}
			else if (pos.y > t.position.y){
				pos.y += -1 * speed* Time.deltaTime;
			}
			
			transform.position = pos;
		}
		
		if(dist <= distanceToAttack+.2f)
			t.GetComponent<LinkScript>().lives--;
	}
	
	
}

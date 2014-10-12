using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class LinkScript : MonoBehaviour
{
		public float speed = 1;
		Animator anim;
		public int lives = 3;
		public int points = 0;
		//public GameObject target;
		public bool attack = false;
		public bool prevAttack = false;
		
		public List<Transform> targets;
		//public List<GameObject> enemies;
		public Transform linksTarget;
		public float distanceToAttack = .5f;
		public int startingEnemies = 1;
		public GameObject baseChicken;
		//public GameObject[] newEnemies;
		public enum Dir{LEFT, UP, RIGHT, DOWN};
		public Dir linkDir;
		// Use this for initialization
		void Start ()
		{
				anim = GetComponent<Animator> ();
				targets = new List<Transform>();
				//enemies = new List<GameObject>();
				
				for(int i = 0; i < startingEnemies; i++){
					GameObject newChicken =  (GameObject)Instantiate(baseChicken);
					newChicken.GetComponent<ChickenScript>().target = gameObject;
					newChicken.transform.position = new Vector3(Random.Range(0f, 6f), Random.Range(-6f, 6f), baseChicken.transform.position.z);
					//enemies.Add(newChicken/*((GameObject)Instantiate(baseChicken)).transform.GetComponent<ChickenScript>().target = gameObject*/);
				}
				addEnemies();
				linkDir = Dir.DOWN;
		}
	
		// Update is called once per frame
		void Update ()
		{
				if(lives < 1){
					Destroy(gameObject);
					Debug.Log (points);
				}
				if(targets.Count > 0)
					targetEnemy();
				//if(targets.Count < 2)
					//createEnimies();
					
					
				Vector3 pos = transform.position;
				float hSpeed = Input.GetAxis ("Horizontal") * speed * Time.deltaTime;
				float vSpeed = Input.GetAxis ("Vertical") * speed * Time.deltaTime;
				if(Input.GetKeyDown ("space"))
					attack = true;
				else
					attack = false;

				// do this to avoid jitter when switching animation
				// set speeds
				if (vSpeed != 0) {
						anim.SetFloat ("hSpeed", 0);
						anim.SetBool ("attacking", false);
						anim.SetBool ("idle", false);
						anim.SetFloat ("vSpeed", vSpeed);
						if(vSpeed > 0)
							linkDir = Dir.UP;
						else if(vSpeed < 0)
							linkDir = Dir.DOWN;
				} else {
						anim.SetFloat ("vSpeed", 0);
						anim.SetBool ("attacking", false);
						anim.SetBool ("idle", false);
						anim.SetFloat ("hSpeed", hSpeed);
						
						if(hSpeed > 0)
							linkDir = Dir.RIGHT;
						else if(hSpeed < 0)
							linkDir = Dir.LEFT;
				}
				//Debug.Log(linkDir);
				// check if idle
				if (hSpeed == 0 && vSpeed == 0) {

						anim.SetBool ("idle", true);
						anim.SetBool ("attacking", false);
				}

				
				// check for attack
				if (attack != prevAttack) {
						anim.SetFloat ("hSpeed", 0);
						anim.SetFloat ("vSpeed", 0);
						anim.SetBool ("idle", true);
						anim.SetBool ("attacking", true);
						linkAttack();
				}
				
				prevAttack = attack;
				
				if ((hSpeed != 0 || vSpeed != 0) && !anim.GetBool ("attacking")) {
						pos.x += hSpeed;
						pos.y += vSpeed;
						transform.position = pos;
				}
		}
		
		private void linkAttack()
		{
			if(targets.Count > 0){
				float dist = linksTarget.GetComponent<ChickenScript>().dist;
				bool canAttack = false;
				if(linkDir == Dir.RIGHT){
					if(linksTarget.position.x >= transform.position.x)
						canAttack = true;
				}
				else if (linkDir == Dir.UP){
					if(linksTarget.position.y >= transform.position.y)
						canAttack = true;
				}
				else if (linkDir == Dir.LEFT){
					if(linksTarget.position.x < transform.position.x)
						canAttack = true;
				}
				else if(linkDir == Dir.DOWN){
					if(linksTarget.position.y < transform.position.y)
						canAttack = true;
				}
				
				if(canAttack){
					if(dist <= distanceToAttack){
						linksTarget.GetComponent<ChickenScript>().lives--;
						//Debug.Log("attacking");
						linksTarget = null;
					}
				}
			}
		}
		
		private void addEnemies(){
			targets.Clear();
			GameObject[] chickens = GameObject.FindGameObjectsWithTag("Enemy");
			foreach(GameObject chicken in chickens){
				targets.Add(chicken.transform);
			}
			
			if(chickens.Length > 0)
				sortByDistance();
		}
		
		private void sortByDistance(){
			if(targets.Count  > 0){
				targets.Sort(delegate(Transform t1, Transform t2)
				{ return (Vector3.Distance(t1.position, this.transform.position).
				CompareTo(Vector3.Distance(t2.position, this.transform.position)));});
			}
		}
		
		private void targetEnemy(){
				if(targets.Count > 0){
					sortByDistance();
					linksTarget = targets[0];
				}
				else
					linksTarget = null;
		}
		
		// NOT WORKING
		private void createEnimies(){
		//enemies.Clear();
		startingEnemies = startingEnemies * 2;
		if (startingEnemies > 100)
			startingEnemies = 100;
			
		for(int i = 0; i < startingEnemies; i++){
			GameObject newChicken =  (GameObject)Instantiate(baseChicken);
			newChicken.GetComponent<ChickenScript>().target = gameObject;
			newChicken.transform.position = new Vector3(Random.Range(0f, 3f), Random.Range(0f, 3f), baseChicken.transform.position.z);
			//enemies.Add(newChicken/*((GameObject)Instantiate(baseChicken)).transform.GetComponent<ChickenScript>().target = gameObject*/);
		}
		addEnemies();
		
		}
		
		
		
		
}

using UnityEngine;
using System.Collections;

public class MasterController : MonoBehaviour {
	
	public bool canRotate = true;
	public Vector3 curRot = new Vector3();
	public GameObject rotator;
	
	public GameObject basicBrick;
	public GameObject mpText;
	
	public int score = 0;
	public int mp = 0; // Multiplier
	
	bool calculate = false;
	
	int maxShakeTime = 100;
	float shakeTime = 0;
	
	Vector3 camBasePos = new Vector3(-4.3f, 3.6f, -4f);
	
	private ArrayList removeableBricks = new ArrayList();
	
	// Use this for initialization
	void Start () {
		for(int x = 0; x < 3; x++) {
			for(int y = 0; y < 3; y++) {
				for(int z = 0; z < 3; z++) {
					if(x == 1 && y == 1 && z == 1) {
					}else {
						GameObject go = Instantiate(basicBrick, new Vector3(x-1, y-1, z-1), new Quaternion()) as GameObject;
						go.GetComponent<Brick>().loc = new Vector3(x-1, y-1, z-1);
						go.GetComponent<Brick>().color = Random.Range(0, 6);
						go.transform.parent = GameObject.Find("BricksController").transform;
					}
				}
			}
		}
		rotator = new GameObject();
		rotator.transform.parent = GameObject.Find("BricksController").transform;
		
		checkColors();
	}
	
	// Update is called once per frame
	void Update () {
		if(!canRotate) {
			
		}
		
		if (Input.GetKeyDown(KeyCode.Space) && !calculate) {
			calculate = true;
			checkColors();
		}
		
		if (Input.GetKeyUp(KeyCode.Space))
			calculate = false;
		
		GameObject scoreGo = GameObject.Find("score");
		scoreGo.GetComponent<TextMesh>().text = "Score: " + score;
		GameObject mpGo = GameObject.Find("multiplier");
		mpGo.GetComponent<TextMesh>().text = "MPx " + mp;
		if(shakeTime > 0) {
			shakeTime -= Time.deltaTime * 1000;
			GameObject cam = GameObject.Find("Main Camera");
			Vector3 pos = cam.transform.position;
			
			pos.x += Random.Range(-1, 1) * Time.deltaTime;
			pos.y += Random.Range(-1, 1) * Time.deltaTime;
			pos.z += Random.Range(-1, 1) * Time.deltaTime;
			cam.transform.position = pos;
		}else {
			GameObject cam = GameObject.Find("Main Camera");
			cam.transform.position = camBasePos;
		}
		
	}
	
	void OnGUI() {
		Event e = Event.current;
		if(e.type == EventType.MouseDrag && canRotate) {
			//Vector3 newRot = new Vector3(e.delta.y, e.delta.x, 0);
			//GameObject.Find("BricksController").transform.Rotate(newRot, Space.World);
		}
	}
	
	public void rotateGroupChildren(GameObject brick, Vector3 mStart, Vector3 hitDir) {
		
		//brick.transform.Rotate(dir);
		Vector3 rotatorAngles;
		bool dir;
		GameObject[] bricks = GameObject.FindGameObjectsWithTag("brick");
		
		GameObject go = rotator;
		go.transform.eulerAngles = GameObject.Find("BricksController").transform.localEulerAngles;
		
		float x = mStart.x - Input.mousePosition.x;
		float y = mStart.y - Input.mousePosition.y;
		
		float xDif = 0;
		float yDif = 0;
		
		if(x > 0) {
			xDif = x;
		}else{
			xDif -= x;
		}
		if(y > 0){
			yDif = y;
		}else{
			yDif -= y;
		}
		
		if(xDif >= yDif){
			rotatorAngles = new Vector3(0,x,0);
			dir = true;
		}else{
			rotatorAngles = hitDir * y;
			dir = false;
		}
		
		go.transform.DetachChildren();
		foreach (GameObject newBrick in bricks) {
			newBrick.transform.parent = GameObject.Find("BricksController").transform;
		}
		
		foreach (GameObject newBrick in bricks) {
			if(dir) {
				if(brick.GetComponent<Brick>().loc.y == newBrick.GetComponent<Brick>().loc.y) {
					newBrick.transform.parent = go.transform;
				}
			}else{
				if(hitDir == Vector3.left) {
					if(brick.GetComponent<Brick>().loc.x == newBrick.GetComponent<Brick>().loc.x) {
						newBrick.transform.parent = go.transform;
					}
				} else {
					if(brick.GetComponent<Brick>().loc.z == newBrick.GetComponent<Brick>().loc.z) {
						newBrick.transform.parent = go.transform;
					}
				}
			}
		}
		
		go.transform.localEulerAngles = rotatorAngles;
		rotatorAngles = go.transform.localEulerAngles;
		rotatorAngles.x = roundToNearestHook(rotatorAngles.x);
		rotatorAngles.y = roundToNearestHook(rotatorAngles.y);
		rotatorAngles.z = roundToNearestHook(rotatorAngles.z);
		go.transform.localEulerAngles = rotatorAngles;
	}
	
	Vector3 checkRotationLock(Vector3 mStart) {
		
		float x = mStart.x - Input.mousePosition.x;
		float y = mStart.y - Input.mousePosition.y;
		float xDif = 0;
		float yDif = 0;
		
		if(x > 0){
			xDif = x;
		}else{
			xDif -= x;
		}
		if(y > 0){
			yDif = y;
		}else{
			yDif -= y;
		}
		
		if(xDif >= yDif){
			return new Vector3(0,x,0);
		}else{
			return new Vector3(y,0,0);
		}
	}
	
	public float roundToNearestHook(float num) {
		if(num > 360)
			num = num - (Mathf.Floor(num / 360) * 360);
		
		if(num > 75 && num < 105)
			num = 90;
		if(num > 165 && num < 195)
			num = 180;
		if(num > 255 && num < 285)
			num = 270;
		if(num > 345 && num < 375)
			num = 0;
		return num;
	}
	
	public float completeToHook(float num) {
		if(num > 0 && num < 90)
			if(num >= 45)
				num = 90;
			else
				num = 0;
		
		if(num > 90 && num < 180)
			if(num >= 135)
				num = 180;
			else
				num = 90;
		if(num > 180 && num < 270)
			if(num >= 225)
				num = 270;
			else
				num = 180;
		if(num > 270 && num < 360)
			if(num >= 315)
				num = 0;
			else
				num = 270;
		
		return num;
	}
	
	public void fixPosAndLoc() {
		GameObject[] bricks = GameObject.FindGameObjectsWithTag("brick");
		foreach (GameObject newBrick in bricks) {
			Vector3 pos;
			pos.x = Mathf.RoundToInt(newBrick.transform.localPosition.x);
			pos.y = Mathf.RoundToInt(newBrick.transform.localPosition.y);
			pos.z = Mathf.RoundToInt(newBrick.transform.localPosition.z);
			newBrick.transform.localPosition = pos;
			newBrick.GetComponent<Brick>().loc = new Vector3(newBrick.transform.localPosition.x, newBrick.transform.localPosition.y + 1, newBrick.transform.localPosition.z);
		}
	}
	
	public bool checkColors() {
		GameObject[] bricks = GameObject.FindGameObjectsWithTag("brick");
		removeableBricks = new ArrayList();
		bool val = false;
		
		foreach (GameObject brick in bricks) {
			ArrayList t = checkBrick(brick, Vector3.zero, new ArrayList());
//			foreach(GameObject b in t) {
//				Brick s = b.GetComponent<Brick>();
//				Debug.Log(s.color);
//			}
			
			if(t.Count >= 3) {
				//Debug.Log("Found Match :" + t.Count);
				foreach(GameObject a in t) {
					removeableBricks.Remove(a);
				}
				removeableBricks.AddRange(t);
				mp++;
				GameObject b = t[1] as GameObject;
				GameObject go = Instantiate(mpText, new Vector3(transform.position.x, transform.position.y, transform.position.z), GameObject.Find("multiplier").transform.rotation) as GameObject;
				go.GetComponent<TextMesh>().text = "x " + mp;
				
				score += 10 * mp;
				val = true;
				shakeTime = maxShakeTime;
			}
		}
		
		if(removeableBricks.Count >= 3) {
			//Debug.Log("Remove these Matches " + removeableBricks.Count);
			foreach(GameObject a in removeableBricks) {
				Brick b = a.GetComponent<Brick>();
				//Debug.Log("color: " + b.color + " | loc: " + b.loc.x + ", " + b.loc.y + ", " + b.loc.z);
				GameObject go = Instantiate(basicBrick, a.transform.position, new Quaternion()) as GameObject;
				go.GetComponent<Brick>().loc = b.loc;
				go.GetComponent<Brick>().color = Random.Range(0, 6);
				go.transform.parent = GameObject.Find("BricksController").transform;
				b.die();
				gameObject.GetComponent<AudioSource>().Play();
			}
			checkColors();
		}
		
//		foreach(GameObject b in removeableBricks) {
//			Brick brick = b.GetComponent<Brick>();
//			Debug.Log(brick.loc);
//		}
		return val;
	}
	
	public ArrayList checkBrick(GameObject incomingBrick, Vector3 dir, ArrayList colors) {
		GameObject[] bricks = GameObject.FindGameObjectsWithTag("brick");
		Brick incBrick = incomingBrick.GetComponent<Brick>();
		
		Vector3 checkDir = new Vector3(dir.x + incBrick.loc.x, dir.y + incBrick.loc.y, dir.z + incBrick.loc.z);
		foreach (GameObject go in bricks) {
			Brick brick = go.GetComponent<Brick>();
			if(incBrick.color == brick.color) {
				if(dir == Vector3.zero && !colors.Contains(go)) {
					if(incBrick.loc.x - 1 == brick.loc.x && incBrick.loc.y == brick.loc.y && incBrick.loc.z == brick.loc.z) {
						ArrayList temp = new ArrayList();
						temp.Add(go);
						checkBrick(go, Vector3.right, temp);
						if(temp.Count >= 3) {
							colors.AddRange(temp);
						}
					}
					else if(incBrick.loc.x + 1 == brick.loc.x && incBrick.loc.y == brick.loc.y && incBrick.loc.z == brick.loc.z) {
						ArrayList temp = new ArrayList();
						temp.Add(go);
						checkBrick(go, Vector3.left, temp);
						if(temp.Count >= 3) {
							colors.AddRange(temp);
						}
					}
					
					if(incBrick.loc.y - 1 == brick.loc.y && incBrick.loc.x == brick.loc.x && incBrick.loc.z == brick.loc.z) {
						ArrayList temp = new ArrayList();
						temp.Add(go);
						checkBrick(go, Vector3.up, temp);
						if(temp.Count >= 3) {
							colors.AddRange(temp);
						}
					}else if(incBrick.loc.y + 1 == brick.loc.y && incBrick.loc.x == brick.loc.x && incBrick.loc.z == brick.loc.z) {
						ArrayList temp = new ArrayList();
						temp.Add(go);
						checkBrick(go, Vector3.down, temp);
						if(temp.Count >= 3) {
							colors.AddRange(temp);
						}
					}
					
					if(incBrick.loc.z - 1 == brick.loc.z && incBrick.loc.y == brick.loc.y && incBrick.loc.x == brick.loc.x) {
						ArrayList temp = new ArrayList();
						temp.Add(go);
						checkBrick(go, Vector3.forward, temp);
						if(temp.Count >= 3) {
							colors.AddRange(temp);
						}
					}else if(incBrick.loc.z + 1 == brick.loc.z && incBrick.loc.y == brick.loc.y && incBrick.loc.x == brick.loc.x) {
						ArrayList temp = new ArrayList();
						temp.Add(go);
						checkBrick(go, Vector3.back, temp);
						if(temp.Count >= 3) {
							colors.AddRange(temp);
						}
					}
				} 
				else if(checkDir == brick.loc && !colors.Contains(go)) {
					colors.Add(go);
					ArrayList t = checkBrick(go, dir, colors);
					return colors;
				}
				
			}
		}
		return colors;
	}
	
}
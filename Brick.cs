using UnityEngine;
using System.Collections;

public class Brick : MonoBehaviour {
	
	Vector3 mStart;
	Vector3 hitDir;
	public Vector3 loc;
	public int color;
	
	public int checkPosition;
	
	public float dieTime;
	
	private Quaternion dieRot;
	
	// Use this for initialization
	void Start () {
		Color newColor = Color.red;
		
		if(color == 0) {
			newColor = Color.red;
		}
		if(color == 1) {
			newColor = Color.blue;
		}
		if(color == 2) {
			newColor = Color.green;
		}
		if(color == 3) {
			newColor = Color.magenta;
		}
		if(color == 4) {
			newColor = Color.white;
		}
		if(color == 5) {
			newColor = Color.yellow;
		}
		
		renderer.material.color = newColor;
	}
	
	// Update is called once per frame
	void Update () {
		if(gameObject.tag == "brickdestroy") {
			
			Vector3 pos = transform.position;
			pos.x += pos.x * Time.fixedDeltaTime * 15;
			pos.y += pos.y * Time.fixedDeltaTime * 15;
			pos.z += pos.z * Time.fixedDeltaTime * 15;
			transform.position = pos;
			
			dieTime -= Time.deltaTime * 1000;
			
			if(dieTime <= 0) {
				Destroy(gameObject);
			}
		}
		
		if(transform.localScale.x < 1) {
			if(transform.localScale.x + transform.localScale.x * Time.fixedDeltaTime * 15 >= 1) {
				transform.localScale = new Vector3(1, 1, 1);
			}else {
				transform.localScale += transform.localScale * Time.fixedDeltaTime * 15;
			}
		}
	}
	
	void OnMouseDown() {
		GameObject.Find("Master").GetComponent<MasterController>().canRotate = false;
		mStart.x = Input.mousePosition.x;
		mStart.y = Input.mousePosition.y;
		
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		
		if (Physics.Raycast(ray, out hit)) {
			if(hit.point.x < hit.point.z) {
				hitDir = Vector3.forward;
			} else {
				hitDir = Vector3.left;
			}
		}
	}
	
	void OnMouseDrag() {
		GameObject.Find("Master").GetComponent<MasterController>().rotateGroupChildren(this.gameObject, mStart, hitDir);
	}
	
	void OnMouseUp() {
		GameObject.Find("Master").GetComponent<MasterController>().canRotate = true;
		Vector3 oldAngles;
		Vector3 oldLoc = loc;
		oldAngles = GameObject.Find("Master").GetComponent<MasterController>().rotator.transform.localEulerAngles;
		oldAngles.x = GameObject.Find("Master").GetComponent<MasterController>().completeToHook(oldAngles.x);
		oldAngles.y = GameObject.Find("Master").GetComponent<MasterController>().completeToHook(oldAngles.y);
		oldAngles.z = GameObject.Find("Master").GetComponent<MasterController>().completeToHook(oldAngles.z);
		GameObject.Find("Master").GetComponent<MasterController>().rotator.transform.localEulerAngles = oldAngles;
		
		GameObject.Find("Master").GetComponent<MasterController>().rotator.transform.DetachChildren();
		GameObject[] bricks = GameObject.FindGameObjectsWithTag("brick");
		foreach (GameObject newBrick in bricks) {
			newBrick.transform.parent = GameObject.Find("BricksController").transform;
		}
		GameObject.Find("Master").GetComponent<MasterController>().fixPosAndLoc();
		
		if(!GameObject.Find("Master").GetComponent<MasterController>().checkColors() && loc != oldLoc) {
			if(GameObject.Find("Master").GetComponent<MasterController>().mp > 0) {
				GameObject.Find("Master").GetComponent<MasterController>().mp--;
			}
		}
	}
	
	public void changeColor() {
		Color newColor = Color.red;
		if(color == 0) {
			newColor = Color.red;
		}
		if(color == 1) {
			newColor = Color.blue;
		}
		if(color == 2) {
			newColor = Color.green;
		}
		if(color == 3) {
			newColor = Color.magenta;
		}
		if(color == 4) {
			newColor = Color.white;
		}
		if(color == 5) {
			newColor = Color.yellow;
		}
		
		renderer.material.color = newColor;
	}
	
	string furthestAway() {
		float x = mStart.x - Input.mousePosition.x;
		float y = mStart.y - Input.mousePosition.y;
		float xDif = 0;
		float yDif = 0;
		
		if(x > 0) {
			xDif = x;
		} else {
			xDif -= x;
		}
		if(y > 0) {
			yDif = y;
		} else {
			yDif -= y;
		}
		
		if(xDif >= yDif) {
			if(x > 0)
				return "left";
			else
				return "right";
		} else {
			if(y > 0)
				return "down";
			else
				return "up";
		}
	}
	
	public void die() {
		gameObject.tag = "brickdestroy";
		dieRot = new Quaternion();
	}
}

using UnityEngine;
using System.Collections;

public class multiplayerText : MonoBehaviour {
	
	float lifeTime = 50;
	float speed = 20;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		speed *= 0.85f;
		transform.position += Vector3.up * Time.deltaTime * speed;
		if(transform.localScale.x < 1) {
			Vector3 sc = transform.localScale;
			sc.x += Time.deltaTime * 10;
			sc.y += Time.deltaTime * 10;
			sc.z += Time.deltaTime * 10;
			transform.localScale = sc;
		}
		if(lifeTime >= 0) {
			lifeTime -= Time.deltaTime * 40;
			Debug.Log(speed);
		} else {
			Destroy(gameObject);
		}
	}
}

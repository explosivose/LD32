using UnityEngine;
using System.Collections;

public class Crowd : MonoBehaviour {
	
	public float speed;
	public float angle;
	private float[] angles;

	// Use this for initialization
	void Start () {
		angles = new float[transform.childCount];
		StartCoroutine("meh");
	}
	
	// Update is called once per frame
	void Update () {
		int i = 0;
		foreach (Transform child in transform){
			Quaternion r = child.FindChild("head").transform.rotation;
			child.FindChild("head").transform.rotation =  Quaternion.Slerp(r, Quaternion.Euler(0, angles[i], 0), Time.deltaTime * speed);
			i += 1;
		}
	}

	IEnumerator meh(){
		while(true){
			int i = 0;
			foreach (Transform child in transform){
				angles[i] = Random.Range(-90f+angle, 90f+angle);
				i += 1;
			}
			yield return new WaitForSeconds(1f);
		}
	}


}

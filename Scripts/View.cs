using UnityEngine;
using System.Collections;

public class View : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 pos = GameObject.Find("Player").transform.position;
        transform.position = new Vector3(pos.x, pos.y, pos.z);
    }
}

using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {
    public float speed = 0.1f;
    Rigidbody rb;
    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 pos = transform.position;
        Vector3 camera_pos = Camera.main.transform.position;
        if (Input.GetKey("up") || Input.GetKey("w"))
        {
            rb.AddForce(Camera.main.transform.forward);
        }
        if (Input.GetKey("down") || Input.GetKey("s"))
        {
            rb.AddForce(Camera.main.transform.forward * -1);
        }
        if (Input.GetKey("right") || Input.GetKey("d"))
        {
            Camera.main.transform.Rotate(0, 10, 0);
        }
        if (Input.GetKey("left") || Input.GetKey("a"))
        {
            Camera.main.transform.Rotate(0, -10, 0);
        }
        // ----------------------------------------
        if (transform.position.y < 0)
        {
            pos = new Vector3(0, 1, 0);
        }

        transform.position = pos;
    }
}

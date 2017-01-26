using UnityEngine;
using System.Collections;

public class Cube : MonoBehaviour {

    public float health;
    public string type { get; set; }
    float max_health;

    public Cube (string set_type)
    {
        type = set_type;
        health = set_health(type);
        max_health = health;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private float set_health(string type)
    {
        switch (type)
        {
            case "Dirt":
                return 3;
            default:
                return 1;
        }
    }

    public void damage_cube(float damage)
    {
        health -= damage;
    }
}

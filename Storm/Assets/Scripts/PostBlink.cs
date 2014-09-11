using UnityEngine;
using System.Collections;

public class PostBlink : MonoBehaviour {
	private bool on = true;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(on){
			if(this.light.range > 4f)
				this.light.range -= .01f;
			else
				on = false;
		}
		else{
			if(this.light.range < 5f)
				this.light.range += .01f;
			else
				on = true;
		}
	}
}

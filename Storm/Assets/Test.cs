using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {

	// Use this for initialization
	void Start() {
		TrooperAI[] objs = this.gameObject.GetComponentsInChildren <TrooperAI>();
		Transform[] targs = new Transform[objs.Length];

		for(int i = 0; i < objs.Length; i++){
			TrooperAI t = objs[i];
			targs[i] = t.transform;
			t.SetSeparationTargets(targs);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

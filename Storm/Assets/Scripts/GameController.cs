using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	public int TroopSize = 5;
	public AIFlock flock;
	public GameObject Troop;

	// Use this for initialization
	void Start () {
		for(int i = 0; i < TroopSize; i++){
			float rand_x = Random.Range(0f, 20f) - 10;
			float rand_z = Random.Range(0f, 20f) - 10;

			GameObject newTrooper = (GameObject)GameObject.Instantiate(Troop, new Vector3(rand_x, 0, rand_z), Quaternion.Euler (Vector3.zero));

			flock.AddTrooper(newTrooper);
		}

		flock.SetLeader(0);
		flock.BeginFlock ();
	}
}

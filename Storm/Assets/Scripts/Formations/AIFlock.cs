using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIFlock : MonoBehaviour {
	private List<GameObject> Troopers;
	private GameObject center;

	public void AddTrooper(GameObject trooper){
		Troopers.Add (trooper);
		TrooperAI ai = GetAIScript (trooper);
		ai.target = center.transform;
		ai.BehaviourSetting = AIBehaviourType.ReachGoal;
	}

	public void SetLeader(int index){
		GetAIScript(Troopers[index]).BehaviourSetting = AIBehaviourType.Wander;
	}

	public void BeginFlock(){
		Transform[] targets = new Transform[Troopers.Count];

		for(int i = 0; i < Troopers.Count; i++){
			GameObject t = Troopers[i];
			targets[i] = t.transform;

			GetAIScript(t).SetSeparationTargets(targets);
			GetAIScript(t).target = center.transform;
		}
	}

	private Transform GetTransform(GameObject trooper){
		return trooper.transform;
	}

	private TrooperAI GetAIScript(GameObject trooper){
		return trooper.GetComponent<TrooperAI>();
	}

	void Awake(){
		center = new GameObject();
		center.name = "Flock center";
		Troopers = new List<GameObject>();
	}

	void Update(){
		center.transform.position = Vector3.zero;
		
		for(int i = 0; i < Troopers.Count; i++){
			Transform t = Troopers[i].transform;
			center.transform.position += t.position;
		}
		
		center.transform.position /= (float)Troopers.Count;
	}
}

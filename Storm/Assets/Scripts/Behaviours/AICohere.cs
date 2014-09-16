using UnityEngine;
using System.Collections;

public class AICohere : AIBehaviour {
	private Transform trans;
	public Transform[] Targets;
	public Transform average;
	private GameObject go;

	public AICohere(AIState state, Transform obj, float weight){
		trans = obj;
		go = new GameObject();
		average = go.transform;
		this.State = state;
		this.Weight = weight;
		Targets = new Transform[]{};
	}
	
	public override AIBehaviourType BehaviourType {
		get { return AIBehaviourType.Cohere; }
	}
	
	public override AIDynamic GetDynamics(){
		
		AIDynamic ai = new AIDynamic();
		ai.Linear = Vector3.zero;
		ai.Angular = 0;

		//UPDATE TARGET
		Vector3 center = Vector3.zero;

		for(int i = 0; i < Targets.Length; i++){
			Transform t = Targets[i];
			center += t.position;
		}

		center /= Targets.Length;
		average.position = center;
		
		return ai;
	}
}

using UnityEngine;
using System.Collections;

public class AICohesion : AIBehaviour {
	private Transform trans;
	public Transform[] Targets;
	public Transform average;

	public AICohesion(AIState state, Transform obj, float weight){
		GameObject centerGoal = new GameObject();
		centerGoal.name = "Cohesion...";
		average = centerGoal.transform;
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
		
		center /= (float)Targets.Length;

		//
		Debug.Log (center);

		average.position = center;
		
		return ai;
	}
}

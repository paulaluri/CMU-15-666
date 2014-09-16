using UnityEngine;
using System.Collections;

public class AISeek : AIBehaviour {
	private Transform trans;

	public override AIBehaviourType BehaviourType {
		get { return AIBehaviourType.Seek; }
	}
	
	public AISeek(AIState state, Transform obj, float weight){
		trans = obj;
		this.State = state;
		this.Weight = weight;
	}
	
	public override AIDynamic GetDynamics(){
		AIDynamic ai = new AIDynamic();
		ai.Linear = (State.Target - trans.position).normalized * State.MaxLinearAcceleration;
		ai.Angular = 0;

		return ai;
	}
}

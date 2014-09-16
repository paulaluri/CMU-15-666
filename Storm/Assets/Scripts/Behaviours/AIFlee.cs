using UnityEngine;
using System.Collections;

public class AIFlee : AIBehaviour {
	private Transform trans;
	
	public override AIBehaviourType BehaviourType {
		get { return AIBehaviourType.Flee; }
	}
	
	public AIFlee(AIState state, Transform obj, float weight){
		trans = obj;
		this.State = state;
		this.Weight = weight;
	}
	
	public override AIDynamic GetDynamics(){
		AIDynamic ai = new AIDynamic();
		ai.Linear = (trans.position - State.Target).normalized * State.MaxLinearAcceleration;
		ai.Angular = 0;
		
		return ai;
	}
}

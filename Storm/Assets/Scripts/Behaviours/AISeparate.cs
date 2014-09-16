using UnityEngine;
using System.Collections;

public class AISeparate : AIBehaviour {
	private Transform trans;

	private const float THRESHOLD = 1f;
	private const float DECAY = .1f;
	public Transform[] Targets;

	public AISeparate(AIState state, Transform obj, float weight){
		trans = obj;
		this.State = state;
		this.Weight = weight;
		Targets = new Transform[]{};
	}

	public override AIBehaviourType BehaviourType {
		get { return AIBehaviourType.Separate; }
	}

	public override AIDynamic GetDynamics(){

		AIDynamic ai = new AIDynamic();
		ai.Linear = Vector3.zero;
		ai.Angular = 0;

		foreach(Transform t in Targets){
			Vector3 direction = (trans.position - t.position).normalized;
			float distance = direction.magnitude;

			if(distance < THRESHOLD){

				float strength = DECAY * distance * distance;
				if(State.MaxLinearAcceleration < strength)
					strength = State.MaxLinearAcceleration;
				ai.Linear += strength * direction;
			}
		}

		//Debug.DrawLine(trans.position, trans.position + 5*ai.Linear, Color.yellow);

		return ai;
	}
}

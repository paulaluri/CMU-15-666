using UnityEngine;
using System.Collections;

public class AIArrive : AIBehaviour {
	private Transform trans;
	private float TARGET_RADIUS = 4f;
	private float SLOW_RADIUS = 5f;
	private float TIME_TO_TARGET = 0.1f;

	public AIArrive(AIState state, Transform obj){
		trans = obj;
		this.State = state;
	}
	
	public override AIDynamic GetDynamics(){
		AIDynamic ai = new AIDynamic();
		ai.Angular = 0;

		Vector3 desired_vector = State.Target - trans.position;
		float distance = desired_vector.magnitude;
		float targetSpeed;
		Vector3 targetVelocity;

		if(distance < TARGET_RADIUS){
			this.State.LinearVelocity = Vector3.zero;
			ai.Linear = Vector3.zero;
			return ai;
		}

		if(distance > SLOW_RADIUS)
			targetSpeed = State.MaxLinearVelocity.magnitude;
		else
			targetSpeed = State.MaxLinearVelocity.magnitude * (distance / SLOW_RADIUS);

		targetVelocity = desired_vector.normalized * targetSpeed;
        
		ai.Linear = (targetVelocity - State.LinearVelocity) / TIME_TO_TARGET;

		if(ai.Linear.magnitude > this.State.MaxLinearAcceleration)
			ai.Linear = ai.Linear.normalized * this.State.MaxLinearAcceleration;

		return ai;
	}
}

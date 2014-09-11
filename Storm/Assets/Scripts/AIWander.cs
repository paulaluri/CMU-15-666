using UnityEngine;
using System.Collections;

public class AIWander : AIBehaviour {
	private const int UPDATE_DELAY = 90;
	private const int WANDER_CIRCLE_RADIUS = 4;
	private const int WANDER_CIRCLE_DISTANCE = 15;
	private const float WANDER_FREEDOM = 120f;

	public float WanderFreedom{get; set;}

	private Transform trans;

	public AIWander(AIState state, Transform obj){
		trans = obj;
		this.State = state;
	}

	public override AIDynamic GetDynamics(){
		if(Time.frameCount%UPDATE_DELAY == 0){
			float r = Random.Range (0f, 1f) - Random.Range (0f, 1f);
			Vector3 wanderCircle = trans.position + trans.forward * WANDER_CIRCLE_DISTANCE;
			State.Target = Quaternion.AngleAxis(WANDER_FREEDOM*r, Vector3.up)*(State.LinearVelocity.normalized)*WANDER_CIRCLE_RADIUS + wanderCircle;
		}

		AIDynamic ai = new AIDynamic();
		ai.Angular = 0;
	    ai.Linear = (State.Target - trans.position).normalized * State.MaxLinearAcceleration;

		return ai;
	}
}

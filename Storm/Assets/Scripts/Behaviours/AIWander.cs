﻿using UnityEngine;
using System.Collections;

public class AIWander : AIBehaviour {
	private const int UPDATE_DELAY = 180;
	private const int WANDER_CIRCLE_RADIUS = 16;
	private const int WANDER_CIRCLE_DISTANCE = 32;
	private const float WANDER_FREEDOM = 180f;

	public float WanderFreedom{get; set;}

	private Transform trans;

	public AIWander(AIState state, Transform obj, float weight){
		trans = obj;
		this.State = state;
		this.Weight = weight;
	}

	public override AIBehaviourType BehaviourType {
		get { return AIBehaviourType.Wander; }
	}

	public override AIDynamic GetDynamics(){
		if(Time.frameCount%UPDATE_DELAY == 0){
			float r = Random.Range (0f, 1f) - Random.Range (0f, 1f);
			Vector3 wanderCircle = trans.position + trans.forward * WANDER_CIRCLE_DISTANCE;

			State.Target = Quaternion.AngleAxis(WANDER_FREEDOM*r, Vector3.up)*(State.LinearVelocity.normalized)*WANDER_CIRCLE_RADIUS + wanderCircle;

			/* Draw vectors
			Debug.DrawLine(trans.position, wanderCircle, Color.yellow, 4);
			Debug.DrawLine (trans.position, State.Target, Color.blue, 4);
			Debug.DrawLine(State.Target, wanderCircle, Color.red, 4);*/
		}

		AIDynamic ai = new AIDynamic();
		ai.Angular = 0;
	    ai.Linear = (State.Target - trans.position).normalized * State.MaxLinearAcceleration;

		return ai;
	}
}
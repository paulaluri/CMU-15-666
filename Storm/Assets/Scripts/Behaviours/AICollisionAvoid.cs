using UnityEngine;
using System.Collections;

public class AICollisionAvoid : AIBehaviour  {
	private Transform trans;
	private const float RADIUS = 2f;
	private const float THRESHOLD = 10f;

	public Transform[] Targets;

	public override AIBehaviourType BehaviourType {
		get { return AIBehaviourType.CollisionAvoid; }
	}

	public AICollisionAvoid(AIState state, Transform obj, float weight){
		trans = obj;
		this.State = state;
		this.Weight = weight;
		Targets = new Transform[]{};
	}

	public override AIDynamic GetDynamics(){
		
		AIDynamic ai = new AIDynamic();
		ai.Linear = Vector3.zero;
		ai.Angular = 0;

		float shortestTime = float.PositiveInfinity;
		Transform firstTarget = null;
		float firstMinSeparation = 0;
		float firstDistance = 0;
		Vector3 firstRelativePos = Vector3.zero;
		Vector3 firstRelativeVel = Vector3.zero;

		foreach(Transform t in Targets){
			Vector3 relativePos = trans.position - t.position;
			Vector3 relativeVel = t.gameObject.GetComponent<TrooperAI>().GetVelocity() - State.LinearVelocity;
			float relativeSpeed = relativeVel.magnitude;

			float timeToCollision = Vector3.Dot (relativePos, relativeVel) / (relativeSpeed * relativeSpeed);

			float distance = relativePos.magnitude;

			if(distance > THRESHOLD || t.gameObject.GetComponent<TrooperAI>().GetVelocity().magnitude < .1f)
				continue;

			float minSeparation = distance - relativeSpeed*shortestTime;

			if(minSeparation > 2*RADIUS)
				continue;

			if((timeToCollision > 0) && timeToCollision < shortestTime){
				shortestTime = timeToCollision;
				firstTarget = t;
				firstMinSeparation = minSeparation;
				firstDistance = distance;
				firstRelativePos = relativePos;
				firstRelativeVel = relativeVel;
			}
		}

		if(firstTarget != null)
		    Debug.DrawLine (trans.position, firstTarget.position, Color.red);

		if(!firstTarget)
			return ai;

		Vector3 finalRelativePos;

		if(firstMinSeparation <= 0 || firstDistance < 2*RADIUS)
			finalRelativePos = trans.position - firstTarget.position;
		else
			finalRelativePos = firstRelativePos + firstRelativeVel * shortestTime;

		ai.Linear = finalRelativePos.normalized * State.MaxLinearAcceleration;

		Debug.DrawLine (this.trans.position, this.trans.position + ai.Linear*4, Color.cyan);
		return ai;
	}
}

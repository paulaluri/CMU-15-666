using UnityEngine;
using System.Collections;

public enum AIBehaviourType{
	Wander,
	ReachGoal,
	Align,
	Separate,
	CollisionAvoid,
	Cohere,
	ObjectAvoid,
	Seek,
	Flee
}

public struct AIDynamic{
	public Vector3 Linear;
	public float Angular;
}

public class  AIState{
	public Vector3 LinearVelocity{get; set;}
	public float AngularVelocity{get; set;}
	public Vector3 MaxLinearVelocity{get; set;}
	public float MaxAngularVelocity{get; set;}
	public float MaxLinearAcceleration{get; set;}
	public float MaxAngularAcceleration{get; set;}
	public Vector3 Target{get; set;}
}

public abstract class AIBehaviour {
	public AIState State;

	public abstract AIBehaviourType BehaviourType{ get;}
	public float Weight{ get; set; }

	public static Vector3 GetVectorFromDirection2D(float y_angle){
		float x = Mathf.Cos (y_angle);
		float z = Mathf.Sin (y_angle);

		return (new Vector3(x, 0, z)).normalized;
	}

	public static float GetDirectionFromVector2D(Vector3 vector){
		return Mathf.Rad2Deg*Mathf.Atan2(vector.x, vector.z);
	}

	abstract public AIDynamic GetDynamics();
}
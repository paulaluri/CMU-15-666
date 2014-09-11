using UnityEngine;
using System.Collections;

public class TrooperAI : MonoBehaviour {
	public Transform target;

	private AIArrive arrive;
	private AIWander wander;
	private AIAlign align;
	private AIState state;
	private Animator animator;
	private AIDynamic align_info;
	private AIDynamic arrive_info;

	// Use this for initialization
	void Start () {
		state = new AIState();

		//Initialize state
     	state.MaxLinearAcceleration = 5f;
		state.AngularVelocity = 0f;
		state.MaxAngularAcceleration = 10f;
		state.MaxLinearVelocity = new Vector3(5f, 0, 5f);
		state.MaxAngularVelocity = 10f;

		state.LinearVelocity = state.MaxLinearVelocity.magnitude * this.transform.forward;

		Vector3 temp = target.position;
		temp.y = 0;
		state.Target = temp;

		animator = this.GetComponent<Animator>();
		animator.SetBool ("run", true);
	}
	
	// Update is called once per frame
	void Update () {

		Debug.DrawLine (this.transform.position, this.transform.position + state.LinearVelocity * 5, Color.cyan, 30);
		//Debug.DrawLine (this.transform.position, this.transform.position + this.transform.forward * 5, Color.yellow, 30);

		/* Behaviours */
		AIDynamic movement = new AIDynamic();

		wander = new AIWander(state, this.transform);
		align = new AIAlign(state, this.transform);
		arrive = new AIArrive(state, this.transform);
		//AIDynamic wander_info = wander.GetDynamics();
		align_info = align.GetDynamics();
		arrive_info = arrive.GetDynamics();

		if(Mathf.Abs((this.transform.position - this.state.Target).magnitude) < 4f)
			animator.SetBool("run", false);

		movement.Angular = align_info.Angular;
		movement.Linear = arrive_info.Linear;
		/* Update Position */

		//Set linear velocity to max
		if(state.LinearVelocity.magnitude > state.MaxLinearVelocity.magnitude)
			state.LinearVelocity = state.LinearVelocity.normalized * state.MaxLinearVelocity.magnitude;

		//Set angular velocity to max
		if(Mathf.Abs (state.AngularVelocity) > Mathf.Abs (state.MaxAngularVelocity))
			state.AngularVelocity = (state.AngularVelocity / Mathf.Abs (state.AngularVelocity)) * state.MaxAngularVelocity;

		//Update position and linear velocity
		this.transform.position += state.LinearVelocity * Time.deltaTime;
		state.LinearVelocity += movement.Linear * Time.deltaTime;

		//Update orientation and angular velocity
		Vector3 rotation = this.transform.rotation.eulerAngles;
		rotation.y += state.AngularVelocity * Time.deltaTime;
		this.transform.rotation = Quaternion.Euler (rotation);
		state.AngularVelocity += movement.Angular * Time.deltaTime;
	}
}

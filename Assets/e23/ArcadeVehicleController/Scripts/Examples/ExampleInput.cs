using UnityEngine;

namespace e23.VehicleController.Examples
{
	public class ExampleInput : MonoBehaviour
	{
		[SerializeField] VehicleBehaviour vehicleBehaviour;

		[Header("Controls")]
		[SerializeField] KeyCode accelerate = KeyCode.W;
		[SerializeField] KeyCode brake = KeyCode.S;
		[SerializeField] KeyCode steerLeft = KeyCode.A;
		[SerializeField] KeyCode steerRight = KeyCode.D;
		[SerializeField] KeyCode strafeLeft;
		[SerializeField] KeyCode strafeRight;
		[SerializeField] KeyCode boost = KeyCode.Space;
		[SerializeField] KeyCode oneShotBoost = KeyCode.B;

		[Header("Settings")]
		[SerializeField] float boostLength = 1f;

		public VehicleBehaviour VehicleBehaviour {
			get { return vehicleBehaviour; }
			set { vehicleBehaviour = value; }
		}

		private void Update()
		{
			// Acceleration
			if (Input.GetKey(accelerate)) { VehicleBehaviour.ControlAcceleration(); }
			if (Input.GetKey(brake)) { VehicleBehaviour.ControlBrake(); }

			// Steering
			if (Input.GetKey(steerLeft)) { VehicleBehaviour.ControlTurning(-1); }
			if (Input.GetKey(steerRight)) { VehicleBehaviour.ControlTurning(1); }

			// Strafing
			if (Input.GetKey(strafeLeft)) { VehicleBehaviour.ControlStrafing(-1); }
			if (Input.GetKey(strafeRight)) { VehicleBehaviour.ControlStrafing(1); }

			// Boost
			if (Input.GetKeyDown(boost)) { VehicleBehaviour.Boost(); }
			if (Input.GetKeyUp(boost)) { VehicleBehaviour.StopBoost(); }
			if (Input.GetKey(oneShotBoost)) { VehicleBehaviour.OneShotBoost(boostLength); }
		}
	}
}
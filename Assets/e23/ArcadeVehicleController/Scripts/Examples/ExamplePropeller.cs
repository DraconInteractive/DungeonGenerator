using UnityEngine;

namespace e23.VehicleController.Examples
{
    public class ExamplePropeller : MonoBehaviour
    {
        [SerializeField] Transform propeller;
        [SerializeField] Vector3 rotationSpeed;

        private void Update()
        {
            RotatePropeller();
        }

        private void RotatePropeller()
        {

            propeller.Rotate(rotationSpeed.x * Time.deltaTime, rotationSpeed.y * Time.deltaTime, rotationSpeed.z * Time.deltaTime);
        }
    }
}
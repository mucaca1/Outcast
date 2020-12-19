using UnityEngine;
using UnityEngine.AI;

public class Mover : MonoBehaviour {
    void Update() {
        UpdateAnimator();
    }

    private void UpdateAnimator() {
        Vector3 velocity = GetComponent<NavMeshAgent>().velocity;
        Vector3 localVelocity = transform.InverseTransformDirection(velocity);
        float speed = localVelocity.z;
        GetComponent<Animator>().SetFloat("fowardSpeed", speed);
    }

    public void MoveTo(Vector3 destination) {
        GetComponent<NavMeshAgent>().destination = destination;
    }
}
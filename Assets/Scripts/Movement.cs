using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class Movement : MonoBehaviour {

    [SerializeField] private Transform target;
    private Ray lastRay;
    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            lastRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        }
        Debug.DrawRay(lastRay.origin, lastRay.direction * 100);
        GetComponent<NavMeshAgent>().destination = target.position;
    }
}
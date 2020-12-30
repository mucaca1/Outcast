using System;
using UnityEngine;
using UnityEngine.UI;

namespace Outcast.Core {
    public class CameraFacing : MonoBehaviour {
        private void Update() {
            transform.forward = Camera.main.transform.forward;
        }
    }
}
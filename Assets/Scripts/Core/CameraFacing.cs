﻿using System;
using UnityEngine;
using UnityEngine.UI;

namespace Outcast.Core {
    public class CameraFacing : MonoBehaviour {
        private void LateUpdate() {
            transform.forward = Camera.main.transform.forward;
        }
    }
}
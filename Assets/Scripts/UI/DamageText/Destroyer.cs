using UnityEngine;

namespace Outcast.UI.DamageText {
    public class Destroyer : MonoBehaviour {
        [SerializeField] private GameObject _parent = null;

        public void Destroy() {
            if (_parent == null) return;
            Destroy(_parent);
        }
    }
}
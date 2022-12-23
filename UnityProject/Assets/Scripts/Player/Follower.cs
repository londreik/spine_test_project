using UnityEngine;

namespace Player {
    public class Follower : MonoBehaviour {
        
        [SerializeField] private Transform _target;
        [SerializeField] private Vector3 _offset;
        [SerializeField] private float _speed;

        private Vector3 _startPosition;

        protected void OnEnable()
        {
            _startPosition = transform.position;
        }

        private void Update()
        {
            transform.position = Vector3.Lerp(transform.position, _target.position + _startPosition + _offset,
                _speed * Time.deltaTime);
        }
    }
}

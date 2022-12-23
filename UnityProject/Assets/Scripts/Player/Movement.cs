using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Movement : MonoBehaviour
    {
        [SerializeField, HideInInspector] private Rigidbody2D _rigidbody2D;
        [SerializeField] private SpineAnimationController _animationController;
        [SerializeField] private float _speed;
     
        private const string HORIZONTAL_AXIS_NAME = "Horizontal";
        private float _horizontalAxis;

        private bool CanMove { get; set; } = true;

        private void Update()
        {
            CanMove = _rigidbody2D.velocity.y == 0;

            switch (_animationController.CurrentDirection)
            {
                case SpineSkeletonDirection.Left:
                    if (_rigidbody2D.velocity.x > 0) _animationController.Turn();
                    break;
                case SpineSkeletonDirection.Right:
                    if (_rigidbody2D.velocity.x < 0) _animationController.Turn();
                    break;
            }

            if (CanMove) _horizontalAxis = Input.GetAxis(HORIZONTAL_AXIS_NAME);
            else
            {
                _horizontalAxis = 0f;
                if (_animationController != null && _animationController.CurrentAnimation != SpineAnimationType.Slide)
                    _animationController.PlayAnimation(SpineAnimationType.Slide);
                return;
            }

            switch (_animationController.CurrentAnimation)
            {
                case SpineAnimationType.Idle:
                    if (_horizontalAxis != 0) _animationController.PlayAnimation(SpineAnimationType.Run);
                    break;
                case SpineAnimationType.Run:
                    if (_horizontalAxis == 0) _animationController.PlayAnimation(SpineAnimationType.Idle);
                    break;
                case SpineAnimationType.Slide:
                    _animationController.PlayAnimation(SpineAnimationType.Idle);
                    break;
            }
        }

        private void FixedUpdate()
        {
            if (CanMove) _rigidbody2D.velocity = new Vector2(_horizontalAxis * _speed * Time.fixedDeltaTime, 0);
        }

        private void OnValidate()
        {
            if (_rigidbody2D == null) _rigidbody2D = GetComponent<Rigidbody2D>();
        }
    }
}

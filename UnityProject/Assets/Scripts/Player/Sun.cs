using DG.Tweening;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(MeshRenderer))]
    public class Sun : MonoBehaviour
    {
        [SerializeField, HideInInspector] private MeshRenderer _meshRenderer;
        [SerializeField] private SpineAnimationController _target;
        [SerializeField] private Color _baseColor;
        [SerializeField] private Color _slideColor;
        [SerializeField] private float _transitionTime = 0.5f;

        private void OnEnable()
        {
            _meshRenderer.material.DOColor(_baseColor, 0.0f);
            _target.OnAnimationPlay += TargetOnOnAnimationPlay;
        }

        private void OnDisable()
        {
            _target.OnAnimationPlay -= TargetOnOnAnimationPlay;
        }

        private void TargetOnOnAnimationPlay(SpineAnimationType state)
        {
            _meshRenderer.material.DOKill();
            _meshRenderer.material.DOColor(state == SpineAnimationType.Slide ? _slideColor : _baseColor, _transitionTime);
        }

        private void OnValidate()
        {
            if (_meshRenderer == null) _meshRenderer = GetComponent<MeshRenderer>();
        }
    }
}

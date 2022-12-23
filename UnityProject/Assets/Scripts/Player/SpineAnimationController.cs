using System;
using System.Collections.Generic;
using Spine;
using Spine.Unity;
using UnityEngine;

namespace Player
{
    public enum SpineAnimationType
    {
        Idle,
        Run,
        Slide
    }

    public enum SpineSkeletonDirection
    {
        Left = -1,
        Right = 1
    }

    [Serializable]
    public class AnimationState
    {
        public SpineAnimationType SpineAnimationType;
        [SpineAnimation] public string SpineAnimationName;
    }
    
    [RequireComponent(typeof(SkeletonAnimation))]
    public class SpineAnimationController : MonoBehaviour
    {
        [SerializeField, HideInInspector] private SkeletonAnimation _skeletonAnimation;
        [SerializeField] private AnimationState[] _animations;

        private readonly Dictionary<SpineAnimationType, string> _animationsMap = new();

        private SpineAnimationType _currentAnimation;
        private SpineSkeletonDirection _currentDirection = SpineSkeletonDirection.Right;
        private Spine.AnimationState _animationState;
        private Skeleton _skeleton;

        public SpineAnimationType CurrentAnimation => _currentAnimation;
        public SpineSkeletonDirection CurrentDirection => _currentDirection;

        public event Action<SpineAnimationType> OnAnimationPlay; 

        private void Awake()
        {
            CreateMap();
            _animationState = _skeletonAnimation.AnimationState;
            _skeleton = _skeletonAnimation.skeleton;
        }

        private void CreateMap()
        {
            _animationsMap.Clear();
            foreach (var anim in _animations)
            {
                _animationsMap.Add(anim.SpineAnimationType, anim.SpineAnimationName);
            }
        }

        public void PlayAnimation(SpineAnimationType type, bool loop = true)
        {
            if (!_animationsMap.ContainsKey(type)) return;

            _animationState.SetAnimation(0, _animationsMap[type], loop);
            _currentAnimation = type;
            OnAnimationPlay?.Invoke(type);
        }

        public void Turn()
        {
            _currentDirection = _currentDirection == SpineSkeletonDirection.Left
                ? SpineSkeletonDirection.Right
                : SpineSkeletonDirection.Left;
            _skeleton.ScaleX = (int)_currentDirection;
        }

        private void OnValidate()
        {
            if (_skeletonAnimation == null) _skeletonAnimation = GetComponent<SkeletonAnimation>();
        }
    }
}

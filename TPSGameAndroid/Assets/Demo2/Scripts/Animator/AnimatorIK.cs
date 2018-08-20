using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorIK : MonoBehaviour {
    private Animator animator;

    [SerializeField]
    private Transform lefthand;
    [SerializeField]
    private Transform righthand;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnAnimatorIK(int layerIndex)
    {
        //Debug.Log("OnAnimatorIK");
        if (layerIndex == 1)
        {
            animator.SetIKPosition(AvatarIKGoal.LeftHand, lefthand.position);
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
            animator.SetIKRotation(AvatarIKGoal.LeftHand, lefthand.rotation);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);

            animator.SetIKPosition(AvatarIKGoal.RightHand, righthand.position);
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
            animator.SetIKRotation(AvatarIKGoal.RightHand, righthand.rotation);
            animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
        }

    }
}

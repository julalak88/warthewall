using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
public class AnimationControl : MonoBehaviour
{


	[SpineAnimation] public string IdleUnFriendly1;
	[SpineAnimation] public string HappyFriendly;
	[SpineAnimation] public string MoodyUnFriendly;
	[SpineAnimation] public string CheerFriendly;

	public Spine.AnimationState spineAnimationState;
	public SkeletonAnimation skeletonAnimation;
	public float duration;
	bool isLoop;
	void Start()
	{
		spineAnimationState = skeletonAnimation.AnimationState;

	}


	public void CallAnimation(string _nameAnimation, bool _isLoop)
	{
		isLoop = _isLoop;
		StartCoroutine(PlayAnimation(_nameAnimation));
	}

	IEnumerator PlayAnimation(string _nameAnimation)
	{

		if (spineAnimationState == null) spineAnimationState = skeletonAnimation.AnimationState;
		spineAnimationState.SetAnimation(0, _nameAnimation, isLoop);
		if (!isLoop)
		{
			yield return new WaitForSeconds(duration);
			spineAnimationState.SetAnimation(0, IdleUnFriendly1, true);
		}

	}


}

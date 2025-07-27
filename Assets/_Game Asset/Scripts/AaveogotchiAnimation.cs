using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AaveogotchiAnimation: MonoBehaviour
{
    [SerializeField] Animator _animator;

    DrssingRoomPlayer _aavegotchiVisuals;

    CustomAnimationsDressingRoom _animationPlay; bool isLefthand;
    CustomAnimationsDressingRoom _lastAnimation;

    private void OnEnable()
    {
        if (_aavegotchiVisuals == null)
            _aavegotchiVisuals = GetComponent<DrssingRoomPlayer>();
    }

    public void PlayWeaponAnimaiton(CustomAnimationsDressingRoom weaponType, bool isInLeftHand)
    {
        _animationPlay = weaponType;
        isLefthand = isInLeftHand;

        AnimateWithDelay();
    }

    private void AnimateWithDelay()
    {
        //if (_animationPlay == CustomAnimationsDressingRoom.Parasol_Attack)
        //{
        //    _aavegotchiVisuals._parasolAnimationObject.SetActive(true);

        //    if (isLefthand)
        //        _aavegotchiVisuals._leftHandPlaceHolder.SetActive(false);
        //    else
        //        _aavegotchiVisuals._rightHandPlaceHolder.SetActive(false);

        //    Invoke("ResetParsol", 1f);
        //}

        if (_lastAnimation == CustomAnimationsDressingRoom.Death)
        {
            _aavegotchiVisuals._leftHandPh.gameObject.SetActive(true);
            _aavegotchiVisuals._rightHandPH.gameObject.SetActive(true);
        }


        if (_animationPlay == CustomAnimationsDressingRoom.Death)
        {
            _aavegotchiVisuals._leftHandPh.gameObject.SetActive(false);
            _aavegotchiVisuals._rightHandPH.gameObject.SetActive(false);
        }


        _animator.SetBool(_lastAnimation.ToString(), false);
        _animator.SetBool(_animationPlay.ToString(), true);
        _lastAnimation = _animationPlay;
    }


    public void PlayerGenericAnimation(CustomAnimationsDressingRoom animaitonState, bool state)
    {
        _animator.SetBool(animaitonState.ToString(), state);
    }

    internal void SetAnimation(CustomAnimationsDressingRoom animation)
    {
        PlayWeaponAnimaiton(animation, true);
    }
}

public enum CustomAnimationsDressingRoom
{
    Idle, Death, 
    IdleDrunk,
    MeleeAttack, RangeAttack, LobAttack, flurryAttack
}


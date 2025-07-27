using DG.Tweening;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DressingRoomInteraction : MonoBehaviour
{
    public TMP_Dropdown _animationDropDown;
    public TMP_Dropdown _collateralDropDown;
    public TMP_Dropdown _eyeDropDown;
    public TMP_Dropdown _eyeShapeDropDown;
    public TMP_Dropdown _groundColors;

    public AaveogotchiAnimation _animation;
    public DrssingRoomPlayer _player;

    private void Start()
    {
        foreach (var slot in Enum.GetValues(typeof(CustomAnimationsDressingRoom)))
        {
            _animationDropDown.AddOptions(new List<string> { slot.ToString() });
        } 
        
        foreach (var slot in Enum.GetValues(typeof(CollateralTypes)))
        {
            _collateralDropDown.AddOptions(new List<string> { slot.ToString() });
        }   
        
        foreach (var slot in Enum.GetValues(typeof(EyeColors)))
        {
            _eyeDropDown.AddOptions(new List<string> { slot.ToString() });
        }
        
        foreach (var slot in Enum.GetValues(typeof(EyeShape)))
        {
            _eyeShapeDropDown.AddOptions(new List<string> { slot.ToString() });
        }
          
        foreach (var slot in Enum.GetValues(typeof(GrassColours)))
        {
            _groundColors.AddOptions(new List<string> { slot.ToString() });
        }

        _player.SetGrassColor(0);
        _player.SetEyeColor(0);
        _player.SetEyeShape(0);
        _player.SetCollateral(0);
        _player.SetEyeColor(0);
        OnDropDownValueChange();
    }

    public void OnDropDownValueChange()
    {
        int value = _animationDropDown.value;
        CustomAnimationsDressingRoom animation = (CustomAnimationsDressingRoom)value;

        _animation.SetAnimation(animation);
    }   
    
    public void OCollateralValueChange()
    {
        int value = _collateralDropDown.value;
        _player.SetCollateral(value);
    }  
    public void OnEyeShapelValueChange()
    {
        int value = _eyeShapeDropDown.value;
        _player.SetEyeShape(value);
    } 
    
    public void OnEyeColoValueChange()
    {
        int value = _eyeDropDown.value;
        _player.SetEyeColor(value);
    }
    
    public void OnGroundValueChange()
    {
        int value = _groundColors.value;
        _player.SetGrassColor(value);
    }

    public Transform _rendertextureHalfPosition, _renderTextureFull, _optionsHalf, _optionsFull;
    public Transform _rendertexture, _options;
    bool fullScreen = false;
    public void ToggelFullScreen()
    {
        if (!fullScreen)
        {
            fullScreen = true;

            _rendertexture.DOMove(_renderTextureFull.position, .5f);
            _options.DOMove(_optionsFull.position, .5f);

        }
        else
        {
            fullScreen = false;

            _rendertexture.DOMove(_rendertextureHalfPosition.position, .5f);
            _options.DOMove(_optionsHalf.position, .5f);

        }
    }
}

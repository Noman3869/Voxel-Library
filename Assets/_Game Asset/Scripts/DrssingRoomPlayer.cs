using PortalDefender.AavegotchiKit;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

public class DrssingRoomPlayer : MonoBehaviour
{
    [SerializeField] internal Transform _headPH, _facePH, _eyePH, _leftHandPh, _rightHandPH, _petPH, _bodyPH, _moveablePetPH;
    [SerializeField] Transform _leftHandSleevesPh, _rightHandSleevesPH;

    [SerializeField] List<GameObject> EyeShapes = new List<GameObject>();
    [SerializeField] List<GameObject> EyeColorPart = new List<GameObject>();
    [SerializeField] List<GameObject> collateralParts = new List<GameObject>();
    [SerializeField] List<Material> collateralBasic = new List<Material>();
    [SerializeField] List<Material> collateralGlow = new List<Material>();
    [SerializeField] List<Material> eyeMat = new List<Material>();
    [SerializeField] List<Color> groundColours = new List<Color>();
    
    [SerializeField] GameObject eyeParent;
    [SerializeField] Material groundMat;

    string SleevesPrefix = "_sleeve_";
    string RightHandWeapon = "_RWeapon";

    private int[] _moveablePets = new int[] { 40};


    public void SetCollateral(int v)
    {
        currentCollateral = v;

        // Mouth
        collateralParts[0].GetComponent<MeshRenderer>().material = collateralBasic[v];

        //Collateral
        collateralParts[1].GetComponent<MeshRenderer>().material = collateralGlow[v];
        collateralParts[2].GetComponent<MeshRenderer>().material = collateralGlow[v];

        //Eyes
        for (int i = 3; i < collateralParts.Count; i++)
        {
            collateralParts[i].GetComponent<MeshRenderer>().material = collateralBasic[v];
        }

        if (eyeMatIsEyeCollateral) SetEyeColor(eyeMat.Count);
    }

    int currentCollateral;
    bool eyeMatIsEyeCollateral;

    internal void SetEyeColor(int v)
    {
        var mat = eyeMat[0];

        if (eyeMat.Count <= v)
        {
            mat = collateralBasic[currentCollateral];
            eyeMatIsEyeCollateral = true;
        }
        else
        {
            mat = eyeMat[v];
            eyeMatIsEyeCollateral = false;

        }
        for (int i = 0; i < EyeColorPart.Count; i++)
        {
            EyeColorPart[i].GetComponent<MeshRenderer>().material = mat;
        }
    }

    internal void SetEyeShape(int v)
    {
        for (int i = 0; i < EyeShapes.Count; i++)
        {
            EyeShapes[i].gameObject.SetActive(false);   
        }

        EyeShapes[v].gameObject.SetActive(true);

    }

    internal void SetGrassColor(int v)
    {
        groundMat.color = groundColours[v];
    }

    bool LoadingModle;
    public void LoadModel(int index, GotchiEquipmentSlot slot)
    {
        if(!LoadingModle)
        {
            LoadingModle = true;
            StartCoroutine(LoadModelProcess(index, slot));
        }
    }

    IEnumerator LoadModelProcess(int index, GotchiEquipmentSlot slot)
    {
        var ph = transform;

        Debug.Log("Load Model " + index + " at " + slot.ToString());

        switch (slot)
        {
            case GotchiEquipmentSlot.BODY: ph = _bodyPH; break;
            case GotchiEquipmentSlot.HEAD: ph = _headPH; break;
            case GotchiEquipmentSlot.FACE: ph = _facePH; break;
            case GotchiEquipmentSlot.EYES: ph = _eyePH; break;
            case GotchiEquipmentSlot.HAND_LEFT: ph = _leftHandPh; break;
            case GotchiEquipmentSlot.HAND_RIGHT: ph = _rightHandPH; break;
            case GotchiEquipmentSlot.PET: ph = _petPH; break;
        }



        if (index < 0)
        {
            RemoveItem(ph);
        }
        else
        {
            var path = Spawn3dData.path_ModelPrefix + index + Spawn3dData.path_ModelPostFix;

            if (slot == GotchiEquipmentSlot.PET && _moveablePets.Contains(index))
            {
                ProceesLoading(index, _moveablePetPH, path);
            }
            else
            {
                ProceesLoading(index, ph, path);
            }


            yield return new WaitForSeconds(.1f);

            if (slot == GotchiEquipmentSlot.BODY)
            {
                path = Spawn3dData.path_ModelPrefix + index + SleevesPrefix + "L" + Spawn3dData.path_ModelPostFix;
                ProceesLoading(index, _leftHandSleevesPh, path);

                path = Spawn3dData.path_ModelPrefix + index + SleevesPrefix + "R" + Spawn3dData.path_ModelPostFix;
                ProceesLoading(index, _rightHandSleevesPH, path);

            }

            if (slot == GotchiEquipmentSlot.HAND_RIGHT)
            {
                var pathRighthand = Spawn3dData.path_ModelPrefix + index + RightHandWeapon + Spawn3dData.path_ModelPostFix;
                ProceesLoading(index, ph, pathRighthand);
            }

        }

        LoadingModle = false;
    }

 
    private void ProceesLoading(int index, Transform ph, string path)
    {
        Addressables.LoadResourceLocationsAsync(path).Completed += (AsyncOperationHandle<IList<IResourceLocation>> handle) =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                if (handle.Result != null && handle.Result.Count > 0)
                {
                    //LoadAsset(key);

                    AsyncOperationHandle<GameObject> newhandle = Addressables.LoadAssetAsync<GameObject>(path);

                    newhandle.Completed += (asyncOperationHandle) =>
                    {
                        // Check if loading succeeded
                        if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
                        {
                            // Asset successfully loaded, proceed to instantiate
                            SpawnItem(asyncOperationHandle.Result, ph);


                            if (ph == _eyePH)
                            {
                                if (index == 301 || index == 381)
                                {
                                    DisableEye(true);
                                }
                                else DisableEye(false);
                            }
                        }
                        else
                        {
                            // Asset loading failed, log or handle error
                            Debug.Log("Failed to load asset:" + index + " Error: " + asyncOperationHandle.OperationException);

                        }
                    };

                }
                else
                {
                    //if (ph == _rightHandSleevesPH || ph == _leftHandSleevesPh)
                    //    RemoveItem(ph);

                    Debug.Log(index + " Fail to load" + AsyncOperationStatus.Failed);
                }

            }
            else
            {
                //if(ph == _rightHandSleevesPH || ph == _leftHandSleevesPh) 
                //    RemoveItem(ph);

                Debug.Log(path + " Do not exsist");
            }
        };
    }

    private void DisableEye(bool State)
    {
        if(eyeParent != null) eyeParent.SetActive(!State);
    }

    private void SpawnItem(GameObject prefab, Transform ph )
    {

        if (prefab == null)
            return;

        RemoveItem(ph);



        var item = Instantiate(prefab, ph);
        item.SetActive(true);
    }

    private void RemoveItem(Transform ph)
    {
        if(ph == _petPH)
        {
            RemoveItem(_moveablePetPH);
        } 
        
        if(ph == _bodyPH)
        {
            RemoveItem(_rightHandSleevesPH);
            RemoveItem(_leftHandSleevesPh);
        }

        

        if (ph.childCount > 0)
        {
            for (int i = ph.childCount - 1; i >= 0; i--)
            {
               // Lean.Pool.LeanPool.Despawn(ph.GetChild(i));
               Destroy(ph.GetChild(i).gameObject);
            }
        }
    }

    
}




public enum CollateralTypes
{
    aave, dai, eth, link, matic, tusd, uni, usdc, usdt, weth, wvtc, yfi
}

public enum EyeColors
{
    mythical_H,
    mythical_L,
    rare_H,
    rare_L,
    uncomon_H,
    uncomon_L, 
    Collateral

}

public enum GrassColours
{
    White,
    Purple,
    Blue,
    Green, 
    Pink, 
    Wheat
}

public enum EyeShape
{
    USDT_Col,
    BTC_Col,
    YFI_Col,
    WETH_Col,
    USDC_Col,
    UNI_Col,
    Uncommon_Low_3,
    Uncommon_Low_2,
    Uncommon_Low_1,
    Uncommon_High_3,
    Uncommon_High_2,
    Uncommon_High_1,
    TUSD_Col,
    Rare_Low_3,
    Rare_Low_2,
    Rare_Low_1,
    Rare_High_3,
    Rare_High_2,
    Rare_High_1,
    Mythical_low_2,
    Mythical_low_1,
    Mythical_low_2_H2,
    Mythical_low_1_H2,
    POLY_Col,
    LINK_Col,
    ETH_Col,
    DAI_Col,
    Common_Low_3,
    Common_Low_2,
    Common_Low_1,
    AAVE_Col,
}
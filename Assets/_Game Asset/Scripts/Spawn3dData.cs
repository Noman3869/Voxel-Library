using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

public class Spawn3dData : MonoBehaviour
{
    [SerializeField] Transform _capPH, _fasePH, _eyePH, _leftHandPh, _rightHandPH, _petPH, _bodyPH, _moveablePetPH;
    [SerializeField] List<GameObject> allSpawnedItems = new List<GameObject>();

    CustomWearables[] itemList;

    string SleevesPrefix = "_sleeve_";
    string RightHandWeapon = "_RWeapon";

    public static string wearablesSleevesLPrefix = "_sleeve_L";
    public static string wearablesSleevesRPrefix = "_sleeve_R";
    public static string path_ModelPrefix = "Assets/_Game Asset/Prefabs/_Phase2-Prefs/_3d Models/Wearables/";
    public static string path_ModelPostFix = ".prefab";

    public static string path_ScriptablePrefix = "Assets/_Game Asset/Scripts/V2Scripts/Wearables/";
    public static string path_ScriptablePostfix = ".asset";

    internal void DespawnOnldModel()
    {
        foreach (var item in allSpawnedItems)
        {
           Destroy(item);
        }

        allSpawnedItems.Clear();


    }

    ////List of all seprate parts like head body and then find in it with the id
    //internal void SpawnNewGotchi(ushort bodyIndex, ushort faceIndex, ushort eyeIndex, ushort headIndex, ushort handLIndex, ushort handRndex, ushort petIndex)
    //{
    //    Get3DItems(bodyIndex, faceIndex, eyeIndex, headIndex, handLIndex, handRndex, petIndex);
    //}

    public CustomWearables[] Get3DItems(ushort bodyIndex, ushort faceIndex, ushort eyeIndex, ushort headIndex, ushort handLIndex, ushort handRndex, ushort petIndex, Action delayStrtCalculation)
    {
        itemList = new CustomWearables[7];
        oprerationComplete = 0;
        if (faceIndex != 0)
        {
            LoadScriptable(faceIndex, 1);
            LoadModel(faceIndex, _fasePH);
        }
        else CheckOperationComplete();

        if (eyeIndex != 0)
        {
            LoadScriptable(eyeIndex, 0);
            LoadModel(eyeIndex, _eyePH);
        }
        else CheckOperationComplete();

        if (bodyIndex != 0)
        {
            LoadScriptable(bodyIndex, 2);
            LoadModel(bodyIndex, _bodyPH);
        }
        else CheckOperationComplete();

        if (headIndex != 0)
        {
            LoadScriptable(headIndex, 3);
            LoadModel(headIndex, _capPH);
        }  
        else CheckOperationComplete();

        if (handRndex != 0)
        {
            LoadScriptable(handRndex, 4);
            LoadModel(handRndex, _leftHandPh);
        }
        else CheckOperationComplete();

        if (handLIndex != 0)
        {
            LoadScriptable(handLIndex, 5);
            LoadModel(handLIndex, _rightHandPH);
        }
        else CheckOperationComplete();

        if (petIndex != 0)
        {
            LoadScriptable(petIndex, 6);
            LoadModel(petIndex, _petPH);
        }
        else CheckOperationComplete();

        return itemList;

    }

    int oprerationComplete;


    void LoadScriptable(int index, int arrIndex)
    {
        try
        {
            Addressables.LoadAssetAsync<CustomWearables>((path_ScriptablePrefix + index + path_ScriptablePostfix)).Completed +=
           (asyncOperationHandle) =>
           {
               if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
               {
                   itemList[arrIndex] = asyncOperationHandle.Result;
                   CheckOperationComplete();

               }
               else
                   CheckOperationComplete();


           };

        }
        catch (Exception e)
        {
            CheckOperationComplete();
        }

    }

    void LoadModel(int index, Transform ph)
    {
        var path = path_ModelPrefix + index + path_ModelPostFix;
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
                        }
                        else
                        {
                            // Asset loading failed, log or handle error
                            Debug.Log("Failed to load asset:" + index + " Error: " + asyncOperationHandle.OperationException);
                            CheckOperationComplete();

                        }
                    };

                }
                else
                {
                    CheckOperationComplete();
                }
            }
            else
            {
                CheckOperationComplete();
            }
        };
    }

   
    

    private void SpawnItem(GameObject prefab, Transform ph )
    {

        if (prefab == null)
            return;

        if(ph.childCount > 0)
        {
            for (int i = ph.childCount; i >= 0 ; i--)
            {
               // Lean.Pool.LeanPool.Despawn(ph.GetChild(i));
            }
        }

        var item = Instantiate(prefab, ph);
        allSpawnedItems.Add(item);
        item.SetActive(true);
    }  
    
 

    private void CheckOperationComplete()
    {
        oprerationComplete++;
        if(oprerationComplete >= 7)
        {
            //GetComponent<Aavegotchi_3D>().DelayStrtCalculation();
        }

    }
}



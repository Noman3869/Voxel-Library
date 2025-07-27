using PortalDefender.AavegotchiKit;
using UnityEngine;
using UnityEngine.AddressableAssets;

[System.Serializable]
public class CustomWearables : ScriptableObject
{
    public int id;
    public bool haveSleeves;
    public bool disableEyes;
    public bool isMoveableTail;
    public bool haveCustomRHandWeaponRotation;

    //public float yRotationForWeaponRHand;

    //public AssetReference prefabRefrence;
    //public GameObject prefabLSleevesRefrence;
    //public GameObject prefabRSleevesRefrence;

    public GotchiWearableRarity rarity;


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Sirenix.OdinInspector;


public class PlayerWeaponInteraction : MonoBehaviour
{
    public GameObject playerHand;
    public GameObject equippedItemInstance;
    public KeyCode attackKey = KeyCode.X;

    [SerializeField]
    private string itemToEquipAddress;

    private GameObject itemToEquipPrefab;

    void Awake()
    {
        equippedItemInstance = null;

        // Start loading the prefab asynchronously
        LoadItemPrefab();
    }

    private void LoadItemPrefab()
    {
        if (!string.IsNullOrEmpty(itemToEquipAddress))
        {
            // Load the prefab dynamically from Addressables using the assigned address
            Addressables.LoadAssetAsync<GameObject>(itemToEquipAddress)
                .Completed += HandleLoadCompleted;
        }
    }

    private void HandleLoadCompleted(AsyncOperationHandle<GameObject> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            // Assign the loaded prefab
            itemToEquipPrefab = handle.Result;

            // Call the EquipWeapon method after the prefab is loaded
            EquipWeapon();
        }
        else
        {
            Debug.LogError($"Failed to load item prefab: {itemToEquipAddress}");
        }
    }

    public void EquipWeapon()
    {
        // Check if the loaded prefab is valid
        if (itemToEquipPrefab == null)
        {
            Debug.LogError("Item prefab not found");
            return;
        }

        // Instantiate the prefab
        GameObject itemInstance = Instantiate(itemToEquipPrefab, playerHand.transform.position, playerHand.transform.rotation);

        // Check if the instantiation was successful
        if (itemInstance == null)
        {
            Debug.LogError("Failed to instantiate item");
            return;
        }

        // Set the parent and tag
        itemInstance.transform.SetParent(playerHand.transform);
        itemInstance.tag = gameObject.tag;

        // Assign the equipped weapon
        equippedItemInstance = itemInstance;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(attackKey))
        {
            PerformWeaponAttack();
        }
    }

    public void PerformWeaponAttack()
    {
        if (equippedItemInstance != null)
        {
            equippedItemInstance.GetComponent<IWeapon>().PerformAttack();
        }
        else
        {
            Debug.LogWarning("No equipped weapon to perform attack");
        }
    }
}

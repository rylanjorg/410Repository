using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class WeaponsManager : MonoBehaviour
{
    public string weaponAddress = "Weapons/Prefab/TurretWeapon"; // Replace with your actual address

    private void Start()
    {
        LoadWeapon();
    }

    private void LoadWeapon()
    {
        // Load the weapon asynchronously
        AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(weaponAddress);

        // Callback when the asset is loaded
        handle.Completed += OnWeaponLoaded;
    }

    private void OnWeaponLoaded(AsyncOperationHandle<GameObject> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            // Instantiate the loaded weapon prefab
            GameObject weaponPrefab = handle.Result;
            Instantiate(weaponPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Debug.LogError("Failed to load weapon: " + handle.DebugName);
        }
    }
}


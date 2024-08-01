using UnityEngine;


public class WeaponModel : MonoBehaviour
{
    // Serialized field for assigning the WeaponSettings in the Unity Editor
    [SerializeReference]
    public Weapon weaponSettingsInstance;

    private void Start()
    {
        if (weaponSettingsInstance != null)
        {
            // Use the settings to position the weapon model or perform other actions
            // Example: transform.position = weaponSettings.projectileSpawnPoint.position;
        }
        else
        {
            Debug.LogError("weaponSettingsInstance not assigned to the WeaponModel.");
        }
    }
}


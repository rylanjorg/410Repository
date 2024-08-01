using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerData;
using PlayerStates;

public class KillBox : MonoBehaviour
{
    // This tag should match your player's tag
    private const string playerTag = "Player";
    bool setPosition = false;
    public float offsetHeight = 1.0f;
    [SerializeField] private OnEnterKillBoxData onEnterKillBoxData;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.transform.name + " : other enter!");
        if (other.CompareTag(playerTag))
        {
            KillPlayer(other.gameObject);
        }
    }

    private void KillPlayer(GameObject player)
    {
         StartCoroutine(Anicipation(player));

        
    }

    private void SpawnKillBoxVFX(Transform playerTransform)
    {
        foreach(VisualEffectStruct vfx in onEnterKillBoxData.onTeleportVFX)
        {
            VFXEventController.Instance.SpawnSimpleVFXGeneral(vfx, playerTransform, playerTransform);
        }
    }

    private void SpawnKillBoxAnticipationVFX(Transform playerTransform)
    {
        foreach(VisualEffectStruct vfx in onEnterKillBoxData.onTeleportAnticipationVFX)
        {
            VFXEventController.Instance.SpawnSimpleVFXGeneral(vfx, playerTransform, playerTransform);
        }
    }

    private IEnumerator Anicipation(GameObject player)
    {
        // Get the parent of the player GameObject
        Transform parent = player.transform.parent;

        // Try to get the PlayerDataManagement component from the parent
        PlayerDataManagement dataManagement = parent?.GetComponent<PlayerDataManagement>();
        FSMPlayer fsmPlayer = parent?.GetComponent<FSMPlayer>();

        

        if (dataManagement != null && fsmPlayer != null)
        {
           
            // TODO: Use the PlayerDataManagement component
            //SpawnKillBoxAnticipationVFX(dataManagement.playerRuntimeData.generalData.playerRoot.transform);
            SpawnKillBoxAnticipationVFX(dataManagement.playerRuntimeData.generalData.playerRoot.transform);
            dataManagement.playerRuntimeData.generalData.playerCameraData.OnPlayerEnterKillBox();
            dataManagement.celShadePlayerController.EnablePlayerGlow();
            yield return new WaitForSeconds(0.5f);
            dataManagement.celShadePlayerController.DisablePlayerRenderer();
            yield return new WaitForSeconds(1.5f);
            
            Debug.Log(player.transform.name + " : Player killed! : safe position: " + dataManagement.playerRuntimeData.lastSafePositionRuntimeData.safePosition + " : player root position: " + dataManagement.playerRuntimeData.generalData.playerRoot.transform.position);
            fsmPlayer.enabled = false;
            dataManagement.playerRuntimeData.generalData.playerRoot.transform.position = dataManagement.playerRuntimeData.lastSafePositionRuntimeData.safePosition + new Vector3(0,offsetHeight, 0);
            dataManagement.playerRuntimeData.generalData.playerCameraData.OnPlayerTeleportFromKillBox();

            SpawnKillBoxVFX(dataManagement.playerRuntimeData.generalData.playerRoot.transform);
            
            yield return new WaitForSeconds(1.0f);
            dataManagement.celShadePlayerController.EnablePlayerRenderer();
            yield return new WaitForSeconds(0.5f);
            dataManagement.celShadePlayerController.DisablePlayerGlow();
            
            

            // Start a coroutine to re-enable the FSMPlayer component after a delay
            StartCoroutine(EnableFSMPlayer(fsmPlayer));
           
        }
        else
        {
            Debug.LogWarning("PlayerDataManagement component not found on parent of " + player.transform.name);
        }
        //yield return new WaitForSeconds(0.5f);
    }

    private IEnumerator EnableFSMPlayer(FSMPlayer fsmPlayer)
    {
        // Wait for the end of the frame
        yield return new WaitForSeconds(0.5f);

        // Re-enable the FSMPlayer component
        fsmPlayer.enabled = true;
    }
}

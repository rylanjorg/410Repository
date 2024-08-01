using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using UnityEngine.VFX;
using UnityEngine.Animations.Rigging;
using UnityEngine.Playables;

using PlayerStates;
using PlayerData;

public class PlayerWeaponContainer : MonoBehaviour
{
    public enum WeaponState
    {
        Idle,
        HipFire,
        AimDownSights,
        Unequipped
    }




    public KeyCode attackKey = KeyCode.Mouse0;
    public KeyCode attackKeySecondary = KeyCode.Mouse1;
    public KeyCode dequipWeaponKey = KeyCode.E;



    // IEnemyHasWeapon Implementation:
    // ---------------------------------------------------------------------------

    [Title("IEnemyHasWeapon (Weapon Instance):")]
    [TabGroup("tab3", "Inscribed")] [SerializeField] private GameObject animatorObject;
    [TabGroup("tab3", "Inscribed")] [SerializeField] private Transform weaponSpawnPoint;
    [TabGroup("tab3", "Inscribed")] [SerializeField] private Transform weaponParentTransform;
    [TabGroup("tab3", "Inscribed")] [SerializeReference] private Rig rig;
    [TabGroup("tab3", "Inscribed")] [SerializeReference] private FSMPlayer fsmPlayer;
    [TabGroup("tab3", "Inscribed")] [SerializeReference] private WeaponIK weaponIK;
    [TabGroup("tab3", "Inscribed")] [SerializeReference] private string aimTargetPath = "RayCast";
    [TabGroup("tab3", "Inscribed")] [SerializeField] private bool canShoot = false;
    [TabGroup("tab3", "Inscribed")] [SerializeField] private bool overrideTransition = false;
    [TabGroup("tab3", "Inscribed")] [SerializeField] private float stateTransitionDelay = 0.5f;
    [TabGroup("tab3", "Inscribed")] [SerializeField] private Transform playerRoot;

    [TabGroup("tab3", "SecondOrderGunPosition")] [SerializeField] float updateInterval = 0.001f;
    [TabGroup("tab3", "SecondOrderGunPosition")] public UpdateMode updateMode;
    [TabGroup("tab3", "SecondOrderGunPosition")] public float f = 5;
    [TabGroup("tab3", "SecondOrderGunPosition")] public float z = 1.0f;
    [TabGroup("tab3", "SecondOrderGunPosition")] public float r = 0.0f;



    [TabGroup("tab3", "Inscribed")] [SerializeField] private float weaponEquipDelay = 0.3f;

    [TabGroup("tab3", "Inscribed")] [SerializeField] private int spawnWeaponStateIndex = 0;
    [TabGroup("tab3", "Inscribed")] [SerializeField] private int handholderWeaponStateIndex = 0;
    [TabGroup("tab3", "Inscribed")] [SerializeField] private int handholderIdleWeaponStateIndex = 0;
    [TabGroup("tab3", "Inscribed")] [SerializeField] private int AimDownSightsWeaponStateIndex = 0;

    [TabGroup("tab3", "Inscribed")] public List<PlayerWeaponState> playerWeaponStates = new List<PlayerWeaponState>();
    [TabGroup("tab3", "Inscribed")] [SerializeField] private float aimSpeedModifier = 0.5f;


    [TabGroup("tab3", "Dynamic", TextColor = "blue")] [SerializeField] [ReadOnly] private GameObject weaponInstance;
    [TabGroup("tab3", "Dynamic")] [SerializeReference] [ReadOnly] public WeaponState currentWeaponState; 
    [TabGroup("tab3", "Dynamic")] [SerializeReference] [ReadOnly] private Weapon weaponSettingsInstance;
    [TabGroup("tab3", "Dynamic")] [SerializeReference] [ReadOnly] private PlayerIK playerIK;
    [TabGroup("tab3", "Dynamic")] [SerializeReference] [ReadOnly] public bool isWeaponEquipped = false;
    [TabGroup("tab3", "Dynamic")] [SerializeReference] [ReadOnly] public bool listenForInput = false;
    [TabGroup("tab3", "Dynamic")] [SerializeReference] [ReadOnly] public int activeCoroutineCount = 0;
    [TabGroup("tab3", "Dynamic")] [SerializeField] [ReadOnly] private SecondOrderDemoPositionGun secondOrderDemoPosition;
    [TabGroup("tab3", "Dynamic")] [SerializeField] [ReadOnly]  public bool doSetState = true;
    [TabGroup("tab3", "Dynamic")] [SerializeField] [ReadOnly]  public SimpleWeaponObjectPlayer simpleWeaponObject;
     [TabGroup("tab3", "Dynamic")] [SerializeField] [ReadOnly]  public bool doFire = false;


    [TabGroup("tab3/Dynamic/SubTabGroup", "Animator", TextColor = "orange")] [SerializeField] [ReadOnly] Animator animator;
    [TabGroup("tab3/Dynamic/SubTabGroup", "Animator")] [SerializeField] [ReadOnly] bool _hasAnimator;
    [TabGroup("tab3/Dynamic/SubTabGroup", "Animator")] [SerializeField] [ReadOnly] int  _animIDWeaponAttack1;
    [TabGroup("tab3/Dynamic/SubTabGroup", "Animator")] [SerializeField] [ReadOnly] int _animIDWeaponIsEquipped;
    [TabGroup("tab3/Dynamic/SubTabGroup", "Animator")] [SerializeField] [ReadOnly] int _animIDIsAiming;
    

    public GameObject WeaponInstance { get => weaponInstance; set => weaponInstance = value; }
    public Weapon WeaponSettingsInstance { get => weaponSettingsInstance; set => weaponSettingsInstance = value; }
    public Transform WeaponSpawnPoint { get => weaponSpawnPoint; set => weaponSpawnPoint = value; }
    public Transform WeaponParentTransform { get => weaponParentTransform; set => weaponParentTransform = value; }

    void Awake()
    {
        playerIK = animatorObject.GetComponent<PlayerIK>();
        listenForInput = true;
        canShoot = true;
        doSetState = true;
        simpleWeaponObject = GetComponent<SimpleWeaponObjectPlayer>();

        // Subscribe to the OnWeaponCreated event
        simpleWeaponObject.simpleWeapon.weaponCreationHandler.OnWeaponCreated += OnWeaponCreated;
    }


    void OnWeaponCreated(GameObject createdWeapon)
    {
        weaponInstance = simpleWeaponObject.simpleWeapon.weaponModelInstance;
        weaponSettingsInstance = simpleWeaponObject.simpleWeapon;
        
        //secondOrderDemoPosition = weaponInstance.GetTransformInHeiarchy().AddComponent<SecondOrderDemoPositionGun>();
        secondOrderDemoPosition = WeaponExtensions.FindTransformInHierarchy(gameObject.transform, "SecondOrderGunTarget").gameObject.AddComponent<SecondOrderDemoPositionGun>();
        secondOrderDemoPosition.baseTransform = playerRoot;
        secondOrderDemoPosition.updateMode = updateMode;
        secondOrderDemoPosition.updateInterval = updateInterval;
        secondOrderDemoPosition.target = playerWeaponStates[spawnWeaponStateIndex].parentTransform;
        
        secondOrderDemoPosition.f = f;
        secondOrderDemoPosition.z = z;
        secondOrderDemoPosition.r = r;
        secondOrderDemoPosition.RecalculateConstants();
        
        weaponInstance.transform.localRotation = Quaternion.identity; 
        
        playerIK.HandIKAmount = playerWeaponStates[spawnWeaponStateIndex].handIKAmount;
        playerIK.ElbowIKAmount = playerWeaponStates[spawnWeaponStateIndex].elbowIKAmount;
       
        weaponIK.aimTransform = FindChildByName(weaponInstance.transform, aimTargetPath);

        SetWeaponState(spawnWeaponStateIndex);
        secondOrderDemoPosition.CustomStart();
    }

    // Start is called before the first frame update
    void Start()
    {
        _hasAnimator = animatorObject.TryGetComponent(out animator);
        AssignAnimationIDs();
    }

    // Update is called once per frame
    void Update()
    {   
        doFire = false;

        switch (currentWeaponState)
        {
            case WeaponState.Idle:
                HandleIdleState();
                break;
            case WeaponState.HipFire:
                HandleHipFireState();
                break;
            case WeaponState.AimDownSights:
                HandleAimDownSights();
                break;
            case WeaponState.Unequipped:
                HandleUnequippedState();
                break;
        }
        
        // Assuming weaponTransform is the Transform you want to check
        isWeaponEquipped = playerWeaponStates[handholderWeaponStateIndex].parentTransform.childCount > 0 || playerWeaponStates[AimDownSightsWeaponStateIndex].parentTransform.childCount > 0;

        if(WeaponSettingsInstance != null) weaponSettingsInstance.Update();

        SetAnimatorVars();
    }  

    void SetAnimatorVars()
    {
        if (_hasAnimator) 
        {
            animator.SetBool(_animIDIsAiming, currentWeaponState == WeaponState.HipFire || currentWeaponState == WeaponState.AimDownSights);
            animator.SetBool(_animIDWeaponIsEquipped, isWeaponEquipped);
        }
    }

    void LateUpdate()
    {
        simpleWeaponObject.weaponStateMachineTransitionHandler.tryPerformAttack = doFire;
    }

    bool ShouldDequipWeapon()
    {
        if((fsmPlayer.playerData.playerRuntimeData.currentState == MovementState.Sliding) || (fsmPlayer.playerData.playerRuntimeData.currentState == MovementState.EdgeHold))
        {
            return true;
        }
        else 
        {
            return false;
        }
    }

    bool CanEquipWeapon()
    {
        if((fsmPlayer.playerData.playerRuntimeData.currentState == MovementState.EdgeHold) || listenForInput == false)
        {
            return false;
        }
        else 
        {
            return true;
        }
    }


    void HandleIdleState()
    {
        if(Input.GetKeyDown(dequipWeaponKey) || ShouldDequipWeapon())
        { 
            fsmPlayer.playerData.playerRuntimeData.walkSpeedModifier = 1.0f;
            if(activeCoroutineCount < 1) StartCoroutine(WeaponTransitionDelay(WeaponState.Unequipped, weaponEquipDelay));
        }
        else if(Input.GetKeyDown(attackKey) && CanEquipWeapon())
        {
            if(activeCoroutineCount < 1) StartCoroutine(WeaponTransitionDelay(WeaponState.HipFire, stateTransitionDelay, false));
        }
        else if(Input.GetKeyDown(attackKeySecondary) && CanEquipWeapon())
        {
            
            if(activeCoroutineCount < 1) StartCoroutine(WeaponTransitionDelay(WeaponState.AimDownSights, stateTransitionDelay));
        }

        // PerformIdleState
        fsmPlayer.playerData.playerRuntimeData.walkSpeedModifier = 1.0f;
        weaponIK.weaponAnimationHelper.targetWeight = 0;
        //secondOrderDemoPosition.enabled = true;
        SetWeaponState(handholderIdleWeaponStateIndex);
    }
    





    void HandleHipFireState()
    {
        if(Input.GetKeyDown(dequipWeaponKey) && isWeaponEquipped )
        { 
         
            canShoot = false;
            fsmPlayer.playerData.playerRuntimeData.walkSpeedModifier = 1.0f;
            if(activeCoroutineCount < 1) StartCoroutine(WeaponTransitionDelay(WeaponState.Unequipped, weaponEquipDelay));
        }
        else if(!Input.GetKey(attackKey) && !Input.GetKey(attackKeySecondary) || ShouldDequipWeapon())
        {
            //canShoot = false;
            fsmPlayer.playerData.playerRuntimeData.walkSpeedModifier = 1.0f;
            if(activeCoroutineCount < 1) StartCoroutine(WeaponTransitionDelay(WeaponState.Idle, stateTransitionDelay, true));
        }
        else if(Input.GetKey(attackKeySecondary) || Input.GetKeyDown(attackKeySecondary) && canShoot)
        {
            canShoot = false;
            fsmPlayer.playerData.playerRuntimeData.walkSpeedModifier = 1.0f;
            if(activeCoroutineCount < 1) StartCoroutine(WeaponTransitionDelay(WeaponState.AimDownSights, stateTransitionDelay)); 
        }


        // PerformActiveState
        //------------------------------------------------------------------------//

        fsmPlayer.playerData.playerRuntimeData.walkSpeedModifier = aimSpeedModifier;
        weaponIK.weaponAnimationHelper.targetWeight = 1;
        //secondOrderDemoPosition.enabled = true;

        if(Input.GetKeyDown(attackKey) || Input.GetKey(attackKey) && canShoot)
        {
            SetWeaponState(handholderWeaponStateIndex);
            PerformWeaponAttackAtIndex(0);
            overrideTransition = true;
        }

        //------------------------------------------------------------------------//
    }





    void HandleAimDownSights()
    {
        if(Input.GetKeyDown(dequipWeaponKey) && isWeaponEquipped )
        { 
            canShoot = false;
            fsmPlayer.playerData.playerRuntimeData.walkSpeedModifier = 1.0f;
            if(activeCoroutineCount < 1) StartCoroutine(WeaponTransitionDelay(WeaponState.Unequipped, weaponEquipDelay));
        }
        else if(!Input.GetKey(attackKeySecondary) || ShouldDequipWeapon())
        {
            fsmPlayer.playerData.playerRuntimeData.walkSpeedModifier = 1.0f;
            if(activeCoroutineCount < 1) StartCoroutine(WeaponTransitionDelay(WeaponState.Idle, stateTransitionDelay, true));
        }

        // PerformActiveState
        //------------------------------------------------------------------------//
        fsmPlayer.playerData.playerRuntimeData.walkSpeedModifier = aimSpeedModifier;
        weaponIK.weaponAnimationHelper.targetWeight = 1;
        //secondOrderDemoPosition.enabled = true;
        isWeaponEquipped = true;
        SetWeaponState(AimDownSightsWeaponStateIndex);

        if(Input.GetKeyDown(attackKey) || Input.GetKey(attackKey) && canShoot)
        {
            //SetWeaponState(AimDownSightsWeaponStateIndex);
            PerformWeaponAttackAtIndex(1);
            overrideTransition = true;
        }

        //------------------------------------------------------------------------//

    }

    void HandleUnequippedState()
    {   
        if(Input.GetKeyDown(dequipWeaponKey) && !isWeaponEquipped && CanEquipWeapon())
        {
            //secondOrderDemoPosition.enabled = true;
            if(activeCoroutineCount < 1) StartCoroutine(WeaponTransitionDelay(WeaponState.Idle, weaponEquipDelay));
        }
        else if(Input.GetKeyDown(attackKey) && CanEquipWeapon())
        {
            //secondOrderDemoPosition.enabled = true;
            if(activeCoroutineCount < 1) StartCoroutine(WeaponTransitionDelay(WeaponState.HipFire, stateTransitionDelay));
        }
        else if(Input.GetKey(attackKeySecondary) || Input.GetKeyDown(attackKeySecondary) && CanEquipWeapon())
        {
            //secondOrderDemoPosition.enabled = true;
            if(activeCoroutineCount < 1) StartCoroutine(WeaponTransitionDelay(WeaponState.AimDownSights, stateTransitionDelay));
        }
        

        // PerformUnequippedState
        fsmPlayer.playerData.playerRuntimeData.walkSpeedModifier = 1.0f;
        weaponIK.weaponAnimationHelper.targetWeight = 0;
        //secondOrderDemoPosition.enabled = false;
        SetWeaponState(spawnWeaponStateIndex);
    }


    private void SetWeaponState(int index)
    {
        weaponInstance.transform.localRotation = Quaternion.identity; 

        if(doSetState)
        {
            secondOrderDemoPosition.target = playerWeaponStates[index].parentTransform;

            weaponInstance.transform.parent = playerWeaponStates[index].parentTransform;
            weaponInstance.transform.localPosition = new Vector3(0,0,0); 
        
            playerIK.HandIKAmount = playerWeaponStates[index].handIKAmount;
            playerIK.ElbowIKAmount = playerWeaponStates[index].elbowIKAmount;
            rig.weight =  playerWeaponStates[index].aimIKAmount;
            doSetState = false;
        }
    }











    public void PerformWeaponAttackAtIndex(int index)
    {
        if (WeaponSettingsInstance != null)
        {
            if (_hasAnimator) animator.SetTrigger(_animIDWeaponAttack1);

            simpleWeaponObject.weaponStateMachineTransitionHandler.chosenAttackIndex = index;
            simpleWeaponObject.weaponStateMachineActionHandler.chosenAttackIndex = index;
            simpleWeaponObject.weaponStateMachineTransitionHandler.chosenAttackInstance = WeaponSettingsInstance.AttackData[index].attack;
            doFire = true;
        }
        else
        {
            Debug.LogWarning("No equipped weapon to perform attack");
        }
    }



    IEnumerator WeaponTransitionDelay(WeaponState targetState, float delay = 0.5f, bool overrideByInput = false)
    {
        activeCoroutineCount++;
        float timer = 0;
        
        while(timer < delay)
        {
            timer += Time.deltaTime;
            yield return null;
        } 

        if(overrideTransition == false || overrideByInput == false)
        {
            currentWeaponState = targetState;
        }
        else if(overrideTransition == true && overrideByInput == true)
        {
            canShoot = true;
            overrideTransition = false;
        }

        doSetState = true;
        activeCoroutineCount--;
    }
 




    private void AssignAnimationIDs()
    {
        _animIDWeaponAttack1 = Animator.StringToHash("WeaponAttack1");
        _animIDWeaponIsEquipped = Animator.StringToHash("WeaponIsEquipped");
        _animIDIsAiming = Animator.StringToHash("IsAiming");
    }

    // Helper method to find a child transform by name recursively
    Transform FindChildByName(Transform parent, string name)
    {
        foreach (Transform child in parent)
        {
            if (child.name == name)
                return child;

            Transform foundChild = FindChildByName(child, name);
            if (foundChild != null)
                return foundChild;
        }
        return null;
    }

    void OnDestroy()
    {
        simpleWeaponObject.simpleWeapon.weaponCreationHandler.OnWeaponCreated -= OnWeaponCreated;
    }
}

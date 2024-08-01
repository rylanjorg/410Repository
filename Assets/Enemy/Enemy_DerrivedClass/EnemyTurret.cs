using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using UnityEngine.VFX;
using System;

public class EnemyTurret : Enemy
{
    // Derived Enemy:
    // ---------------------------------------------------------------------------
    [Title("Derived Enemy Class:")]
    [TabGroup("tab2", "Inscribed", TextColor = "green")]
    [TabGroup("tab2/Inscribed/SubTabGroup", "General", TextColor = "lightgreen")]
    [TabGroup("tab2/Inscribed/SubTabGroup", "General")] [SerializeField] public float agroRange = 8f;
    [TabGroup("tab2/Inscribed/SubTabGroup", "General")] [SerializeField] public float extendedAgroRange = 12f;
    [TabGroup("tab2/Inscribed/SubTabGroup", "General")] [SerializeField] public float swivelSpeed = 45f;
    [TabGroup("tab2/Inscribed/SubTabGroup", "General")] [SerializeField] public float rotationSpeed = 2f;

 
    // OnDieEvent:
    // ---------------------------------------------------------------------------

    [Title("Enemy Death Event:")]
    [TabGroup("tab2", "Events", TextColor = "purple")]
    [TabGroup("tab2/Events/SubTabGroup", "OnDieEvent", TextColor = "purple")] [SerializeField] private GameObject OnDieEffectPrefab = null;
    [TabGroup("tab2/Events/SubTabGroup", "OnDieEvent", TextColor = "purple")] [SerializeField] private Vector3 OnDieEffectScale = new Vector3(1, 1, 1);


    // Custom Animation:
    // ---------------------------------------------------------------------------
    
    [Title("Custom Animation Properties:")]
    [TabGroup("tab2", "Inscribed", TextColor = "green")]
    [TabGroup("tab2/Inscribed/SubTabGroup", "CustomAnimation", TextColor = "lightyellow")]
    [SerializeField] private AnimationUtility.AnimationCycleDuration cycleDuration = AnimationUtility.AnimationCycleDuration.OneFrame;

    [SerializeField]
    [TabGroup("tab2/Inscribed/SubTabGroup", "CustomAnimation")]
    private int frameCount = 0;
    private Quaternion animCycle_targetRotation = Quaternion.identity;


    // IEnemyHasWeapon Implementation:
    // ---------------------------------------------------------------------------

    [Title("IEnemyHasWeapon (Weapon Instance):")]
    [TabGroup("tab3", "Inscribed", TextColor = "green")] [SerializeField] SimpleWeaponObjectEnemy simpleWeaponObjectEnemy;
    //  private WeaponObject weaponObject;

    //[TabGroup("tab3", "Dynamic", TextColor = "blue")] [SerializeField] [ReadOnly] private GameObject weaponInstance;
    //[TabGroup("tab3", "Dynamic")] [SerializeReference] [ReadOnly] private Weapon weaponSettingsInstance;

    //public WeaponObject WeaponObject { get => weaponObject; set => weaponObject = value; }
    //public GameObject WeaponInstance { get => weaponInstance; set => weaponInstance = value; }
    //public Weapon WeaponSettingsInstance { get => weaponSettingsInstance; set => weaponSettingsInstance = value; }
    //public Transform WeaponSpawnPoint { get => weaponSpawnPoint; set => weaponSpawnPoint = value; }
    //public Transform WeaponParentTransform { get => weaponParentTransform; set => weaponParentTransform = value; }

    // Procedural Animation:
    // ---------------------------------------------------------------------------
    [TabGroup("tab2/Inscribed/SubTabGroup", "ProceduralAnim", TextColor = "purple")] [SerializeField]  private ProceduralLegController legController;
    [TabGroup("tab2/Inscribed/SubTabGroup", "ProceduralAnim")] [SerializeField] GameObject PABodyTarget;
    [TabGroup("tab2/Inscribed/SubTabGroup", "ProceduralAnim")] [SerializeField] PATargetController paTargetController;
    [TabGroup("tab2/Inscribed/SubTabGroup", "ProceduralAnim")] [SerializeField] SecondOrderDemoPosition secondOrderDemoPosition;
    [TabGroup("tab2/Inscribed/SubTabGroup", "ProceduralAnim")] [SerializeField] float idlebodyHeight;
    [TabGroup("tab2/Inscribed/SubTabGroup", "ProceduralAnim")] [SerializeField] float movingbodyHeight;


    protected override void Awake()
    {
        base.Awake();
        // Call the Awake method of WeaponObject
        //weaponObject.weapon.Awake(weaponObject.weapon);
        
        // Store the initial rotation
        //initialRotation = transform.rotation;
        // Spawn the weapon model
        //weaponInstance = weaponObject.weapon.SpawnWeaponModel(weaponSpawnPoint, weaponParentTransform);
        //weaponSettingsInstance = weaponInstance.GetComponent<WeaponModel>().weaponSettingsInstance;

    }

    protected override void Start()
    {
        base.Start();
        //EnemyEvents.current.onEnemyDie += OnDie;
        // Set the initial rotation
        //initialRotation = transform.rotation;
        // Spawn the weapon model
        //weaponInstance = weaponObject.weapon.SpawnWeaponModel(weaponSpawnPoint, weaponParentTransform);
        //weaponSettingsInstance = weaponInstance.GetComponent<WeaponModel>().weaponSettingsInstance;
        hitBox = SerializationUtility.FindHitBoxInTree(this.gameObject);
        if(hitBox != null)
        {
            hitBox.enemyInstance = this;
        }
    }



    // Overrides the virtual method in the base class
    protected override void Update()
    {
        // Call the base class implementation first
        base.Update();
        //weaponObject.weapon.Update();

        frameCount++;


        if(legController.currentState == ProceduralLegController.State.Idle)
        {
            Vector3 position = PABodyTarget.transform.localPosition;
            position.y = idlebodyHeight;
            PABodyTarget.transform.localPosition = position;
        }
        else
        {
            Vector3 position = PABodyTarget.transform.localPosition;
            position.y = movingbodyHeight;
            PABodyTarget.transform.localPosition = position;
        }

        if(stateMachine.currentStateName == "IdleState")
        {

            // Find state in stateMachine stateInstance list
            State state = stateMachine.stateInstances.Find(x => x.StateName == "FollowState");
            if(state != null && state is FollowState followState)
            {
                Swivel(followState.exitRotationY);
            }
            
        }


        else if(stateMachine.currentStateName == "FollowState" || stateMachine.currentStateName == "FollowState_PostStat")
        {
            paTargetController.EnableAIPathFind();
            Vector3 position = PABodyTarget.transform.localPosition;
            position.y = movingbodyHeight;
            PABodyTarget.transform.localPosition = position;
        }

        else if(stateMachine.currentStateName == "TurretStationaryState")
        {
            Vector3 position = PABodyTarget.transform.localPosition;
            position.y = idlebodyHeight;
            PABodyTarget.transform.localPosition = position;
        }

        else if(stateMachine.currentStateName == "TurretStationaryAttackState")
        {
            //if(weaponSettingsInstance != null) weaponSettingsInstance.Update();

            if (playerRef != null)
            {
                Debug.Log("Try attack from enemy turret");
                simpleWeaponObjectEnemy.TryAttack();
               //weaponSettingsInstance.TryPickAttackAtRandom();
            }
        }

        else if(stateMachine.currentStateName == "StationaryLeaveState")
        {
            /*if(weaponSettingsInstance != null) weaponSettingsInstance.Update();

            if (playerRef != null)
            {
               weaponSettingsInstance.PickAttackAtRandom();
            }*/
            paTargetController.DisableAIPathFind();
        }
        else
        {
            simpleWeaponObjectEnemy.weaponStateMachineTransitionHandler.tryPerformAttack = false;
        }
        

    }

        // Sets direction and normalizes 
        // Vector3 direction = playerRef.transform.position - weaponSettingsInstance.RootTransform.position;
        // direction.Normalize();
        // Rotate the enemy to face the player smoothly
        //Quaternion targetRotation = Quaternion.LookRotation(direction);
        //bool updateRotation = AnimationUtility.AnimCycle_ShouldUpdateVariable(frameCount, cycleDuration);

        /* if(updateRotation)
        {
        //weaponSettingsInstance.RootTransform.localRotation = animCycle_targetRotation;
        }
        else
        {
        animCycle_targetRotation = Quaternion.Slerp(animCycle_targetRotation, targetRotation, Time.deltaTime * rotationSpeed);
        }*/

        //weaponSettingsInstance.RootTransform.rotation = Quaternion.Slerp(weaponSettingsInstance.RootTransform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

    void Swivel(Quaternion targetRotation)
    {
        // Calculate the rotation offset from the target rotation
        float offset = targetRotation.eulerAngles.y;

        // Swivel the turret smoothly between targetRotation + 30 and targetRotation - 30
        float angle = Mathf.PingPong(Time.time * swivelSpeed, 80f) - 40f;

        bool updateRotation = AnimationUtility.AnimCycle_ShouldUpdateVariable(frameCount, cycleDuration);
        
        if(updateRotation)
        {
            //weaponSettingsInstance.RootTransform.localRotation = animCycle_targetRotation;
        }
        else
        {
            // Interpolate between the current rotation and the swiveled rotation
            animCycle_targetRotation = Quaternion.Slerp(
            animCycle_targetRotation, 
            targetRotation * Quaternion.Euler(0, angle, 0), 
            Time.deltaTime * rotationSpeed
            );
        }
    }

    public override void OnDie()
    {
        base.OnDie();

        // Instantiate the visual effect p=refab
        if (OnDieEffectPrefab != null)
        {
            GameObject onDieEffectInstance = Instantiate(OnDieEffectPrefab, transform.position, transform.rotation);

            // Detach the visual effect from the root object
            onDieEffectInstance.transform.parent = null;

            // Play the visual effect
            VisualEffect visualEffect = onDieEffectInstance.GetComponent<VisualEffect>();
            if(visualEffect != null)
            {
                onDieEffectInstance.transform.localScale = OnDieEffectScale;
                visualEffect.Play();
            }
        }
    
        Destroy(transform.root.gameObject);
    }


    public override void OnHitBoxTriggerEnter(DamageEffect damageInstance)
    {
        if(damageInstance != null) {
             Debug.Log($"DamageEffect Damage: {damageInstance.damage}, DamageEffect Knockback: {damageInstance.knockback}");
        }
        else {
            Debug.Log("DamageEffect is null");
        }
        base.OnHitBoxTriggerEnter(damageInstance);
        healthBar.UpdateHealthBar(MaxHealth, CurrentHealth);    
    }



}

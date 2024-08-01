using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using UnityEngine;
using TMPro;

[System.Serializable]
public class AttackRuntimeData
{
    [BoxGroup("AttackRuntimeData")] [SerializeField] public int attackID;
    [BoxGroup("AttackRuntimeData")] [SerializeReference] [ReadOnly] public Attack attackInstance;
    [BoxGroup("AttackRuntimeData")] [SerializeReference] [ReadOnly] public Weapon weaponInstance;
    [BoxGroup("AttackRuntimeData")] public SimpleCooldownData cooldownData;
    [BoxGroup("AttackRuntimeData")] [ReadOnly] public List<AttackModifierData> modifierData = new List<AttackModifierData>();
    [BoxGroup("AttackRuntimeData")] [ReadOnly] public VisualEffectData visualEffectData;
    [BoxGroup("AttackRuntimeData")] [SerializeReference] [ReadOnly] public AbstractAttackData attackData;
    public List<int> allVFXIDs = new List<int>();

    
    public TextMeshProUGUI uiText; 



    public AttackRuntimeData(Attack attackInstance, Weapon weaponInstance)
    {
        this.cooldownData = new SimpleCooldownData(attackInstance.simpleCooldown, this);
        this.visualEffectData = new VisualEffectData(attackInstance, weaponInstance);
        this.attackInstance = attackInstance;
        this.weaponInstance = weaponInstance;
     
        

        foreach (var attackModifier in attackInstance.Modifiers)
        {
            modifierData.Add(new AttackModifierData(attackModifier, attackInstance, weaponInstance));
        }


        // Check if the weaponInstance implements the IProjectileWeapon interface
        if (attackInstance is IProjectileAttack projectileAttack)
        {
            attackData = new ProjectileAttackData(this, attackInstance);
        }
        else if (attackInstance is IHitscanAttack hitscanAttack)
        {
            //attackData = new HitscanAttackData(hitscanAttack, this);
        }

        int i = 0;
        foreach(CooldownEventTrigger cet in attackInstance.simpleCooldown.cooldownEventTriggers)
        {
            allVFXIDs.Add(i);
            i++;
        }

       
    }

    public IProjectileAttackData TryCastProjectileAttackData()
    {
        if (attackData is IProjectileAttackData projectileAttackData)
        {
            return projectileAttackData;
        }
        return null;
    }

    public IProjectileAttack TryCastProjectileAttack()
    {
        if (attackInstance is IProjectileAttack projectileAttack)
        {
            return projectileAttack;
        }
        return null;
    }

    public void TryUpdateUIPopUp()
    {
        if(attackInstance.useUIPopUp == true && uiText != null)
        {
           uiText.text = TryCastProjectileAttackData().CurrentAmmoCount + "/" + TryCastProjectileAttack().AmmoCount;
        }
    }
    public void SetUI()
    {
        if(attackInstance.useUIPopUp == true) 
        {
            uiText = PopUpGenerator.Instance.CreateAttackUIPopUp(attackID).GetComponent<TextMeshProUGUI>();
            TryUpdateUIPopUp();
        } 
    }

    [System.Serializable]
    public class SimpleCooldownData
    {
        private AttackRuntimeData attackRuntimeData;
        [HideInInspector] public SimpleCooldown data;
        [HideInInspector] public CooldownEvent OnCooldownTick = new CooldownEvent();
        [SerializeField] [ReadOnly] private float timer;
        [SerializeField] [ReadOnly] public bool canUseCooldown;
        [SerializeField] [ReadOnly] private bool resetCooldown = false;
        

        public SimpleCooldownData(SimpleCooldown data, AttackRuntimeData attackRuntimeData)
        {
            this.attackRuntimeData = attackRuntimeData;
            this.data = data;
            this.timer = 0;
            this.canUseCooldown = true;
            this.resetCooldown = false;
        }

        public void StartCooldown(float cooldownDuration)
        {  
            CooldownManagement.UseCooldown(ref timer, ref canUseCooldown, cooldownDuration);
        }

        public void UpdateCooldown(List<CooldownEventTrigger> cooldownEventTriggers, float cooldownDuration)
        {
            if(canUseCooldown == true)
            return;

            bool prev = canUseCooldown;
            
            CooldownManagement.TickCooldownTimer(ref timer, ref resetCooldown);

            // Trigger the events at the specified times
            foreach (var eventTrigger in cooldownEventTriggers)
            {
                //Debug.Log($"Checking and triggering event with cooldownProgress: {timer}");
                eventTrigger.CheckAndTriggerEvent(cooldownDuration - timer, attackRuntimeData);
            }

            if(resetCooldown == true && canUseCooldown == false)
            {
                ResetCooldown(data.cooldownEventTriggers, data.cooldownDuration);
            }
        }

        public bool IsCooldownActive()
        {
            return !canUseCooldown;
        }

        public void ResetCooldown(List<CooldownEventTrigger> cooldownEventTriggers, float cooldownDuration)
        {
            resetCooldown = false;
            CooldownManagement.ResetCooldownTimer(ref timer, cooldownDuration);
            canUseCooldown = true;
            
            foreach (var eventTrigger in cooldownEventTriggers)
            {
                eventTrigger.HasTriggered = false;
            }
        }
    }


    [System.Serializable]
    public class AttackModifierData
    {
        [Title("Attack Modifier", "Dynamic variables for base class for all attack modifiers")]
        [HideInInspector] [SerializeReference] public AttackModifier attackModifier;
        public AttackModifierData(AttackModifier attackModifier, Attack attackInstance, Weapon weaponInstance)
        {
            this.attackModifier = attackModifier;
        }
    }


    [System.Serializable]
    public class VisualEffectData
    {
        [SerializeField] [ReadOnly] public List<Transform> parentTransforms;

        public VisualEffectData(Attack attackInstance, Weapon weaponInstance)
        {
            parentTransforms = new List<Transform>();
            SetVisualEffectRuntimeData(weaponInstance, attackInstance);
        }

        private void SetVisualEffectRuntimeData(Weapon weaponInstance, Attack attackInstance)
        {
            foreach(CooldownEventTrigger trigger in attackInstance.simpleCooldown.CooldownEventTriggers)
            {
                parentTransforms.Add(WeaponExtensions.FindTransformInHierarchy(weaponInstance.weaponModelInstance.transform, trigger.visualEffectStruct.parentTransformName));
            }
        }
    }



    public void CallFunctionForMatchingValues(List<int> list1, List<int> list2)
    {
        foreach (int value in list2)
        {
            if (list1.Contains(value))
            {
                attackInstance.SimpleCooldown.CooldownEventTriggers[value].OverrideSpawnVFX(this);
            }
        }
    }
}

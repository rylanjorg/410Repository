using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using System;

public enum AttackType
{
    Projectile,
    Hitscan,
    Melee
}


[System.Serializable]
[CreateAssetMenu(fileName = "New Attack", menuName = "Attack")]
public class Attack : ScriptableObject
{   
    [SerializeField] [TabGroup("tab4", "Audio", TextColor = "orange")] protected AudioClip attackFireSound;
    [SerializeField] [TabGroup("tab4","Audio")] protected float audioClipPitch = 1.0f;
    [SerializeField] [TabGroup("tab4","Audio")] protected float audioClipVolume = 1.0f;
    [SerializeField]  public bool useUIPopUp = false;
    

    public virtual AttackType Type { get; }
    
    [TabGroup("tab5", "Cooldown", TextColor = "orange")]  [SerializeReference] [HideLabel] [HideReferenceObjectPicker] public SimpleCooldown simpleCooldown = new SimpleCooldown();
    [SerializeReference] [HideLabel] [TabGroup("tab6","AttackModifiers", TextColor = "orange")] [HideReferenceObjectPicker] [ListDrawerSettings(ShowFoldout = true, DraggableItems = false, ShowItemCount = false)] public List<AttackModifier> Modifiers = new List<AttackModifier>();
    
    protected Action<AttackRuntimeData> baseAttackDelegate;

    public AudioClip AttackFireSound
    {
        get { return attackFireSound; }
        set { attackFireSound = value; }
    }
    public float AudioClipPitch 
    {
        get { return audioClipPitch; }
        set { audioClipPitch = value; }
    }

    public float AudioClipVolume
    {
        get { return audioClipVolume; }
        set { audioClipVolume = value; }
    }

    public SimpleCooldown SimpleCooldown
    {
        get { return simpleCooldown; }
        set { simpleCooldown = value; }
    }






    public virtual void OnAwake(AttackRuntimeData attackRuntimeData)
    {
        if(attackRuntimeData == null)
        {
            return;
        } 

        // Initialize all modifier instances
        foreach (var modifier in  attackRuntimeData.attackInstance.Modifiers)
        {
            modifier.InitializeAttackModifier(attackRuntimeData.attackInstance, attackRuntimeData.weaponInstance);
        }

    }

    


    public virtual void UpdateVar()
    {
    }

    protected virtual void BaseAttack(AttackRuntimeData attackRuntimeData)
    {
        //attackRuntimeData.weaponInstance.ResetAttackTime();
        PlayAttackSound(attackRuntimeData.weaponInstance, attackRuntimeData.attackInstance);
        attackRuntimeData.cooldownData.StartCooldown(attackRuntimeData.attackInstance.SimpleCooldown.cooldownDuration);
    }

    public virtual void PerformTypeAttack(AttackRuntimeData attackRuntimeData, bool ignoreModifiers, bool overrideCooldown = false)
    {
        Debug.Log("Performing type attack!" + attackRuntimeData.attackInstance + " ignore modifiers: " + ignoreModifiers + " override cooldown: " + overrideCooldown);
        
        if(!attackRuntimeData.cooldownData.canUseCooldown && !overrideCooldown)
        {
            Debug.Log("Attack is on cooldown");
            return;
        }

       
        bool overrideBaseAttack = false;


        if(!ignoreModifiers)
        {
            // Check if Modifiers is not null
            if (attackRuntimeData.attackInstance.Modifiers != null)
            {
                // Apply all modifiers
                foreach (var modifier in attackRuntimeData.attackInstance.Modifiers)
                {
                    overrideBaseAttack = modifier.ApplyModifier(ignoreModifiers, baseAttackDelegate, attackRuntimeData);
                    if (overrideBaseAttack)
                    {
                        break;
                    }
                }
            }
        }

        //If no modifiers were applied, or none of them overrode the base function call, perform the base attack
        if (!overrideBaseAttack)
        {
            Debug.Log("Performing base attack" + overrideBaseAttack);
            BaseAttack(attackRuntimeData);  
        }
        
    }

    private void PlayAttackSound(Weapon weaponInstance, Attack attackInstance)
    {
        //Play the audio clip
        if(attackInstance.AttackFireSound != null)
        {
            if( weaponInstance.MyAudioSource != null)
            {
                weaponInstance.MyAudioSource.pitch = attackInstance.AudioClipPitch;
                weaponInstance.MyAudioSource.clip = attackInstance.AttackFireSound;
                weaponInstance.MyAudioSource.volume = attackInstance.AudioClipVolume;

                // Set the position of the audio source
                weaponInstance.MyAudioSource.transform.position = weaponInstance.RootTransform.position;

                // Play the audio
                weaponInstance.MyAudioSource.Play();
            }
            else
            {
                Debug.LogError("Audio source is null");
            
            }
        }
    }



    
    public virtual Attack Clone()
    {
        // Create a shallow copy of the current instance
        Attack copy = (Attack)this.MemberwiseClone();

        // Create a deep copy of the Modifiers list
        copy.Modifiers = new List<AttackModifier>();
        foreach (AttackModifier modifier in this.Modifiers)
        {
            copy.Modifiers.Add(modifier.Clone()); // Assume that AttackModifier has a Clone method
        }


        // If SimpleCooldown is not null, create a new instance with the same properties
        /*if (this.SimpleCooldown != null)
        {
            copy.SimpleCooldown = this.SimpleCooldown.Clone();
        }*/

        Debug.Log("Attack Clone" + copy.Modifiers.Count);
        return copy;
    }
}

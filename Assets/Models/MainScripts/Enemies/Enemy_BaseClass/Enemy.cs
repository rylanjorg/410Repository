using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[RequireComponent(typeof(Animator))]
public abstract class Enemy : MonoBehaviour, ISwappable, IStateMachine
{
    [Title("Enemy Base Class:")]
    [TabGroup("Inscribed", TextColor = "green")]
    [SerializeField] protected new string name = "DefaultName";
    [TabGroup("Inscribed")] [SerializeField] protected float maxHealth = 1;
    [TabGroup("Inscribed")] [SerializeField] protected float knockbackSpeed = 10;
    [TabGroup("Inscribed")] [SerializeField] protected float currencyDropped = 10;
    [TabGroup("Inscribed")] [SerializeField] protected float knockbackDuration = 0.25f;
    [TabGroup("Inscribed")] [SerializeField] protected float invincibleDuration = 0.5f;
    [TabGroup("Inscribed")] [SerializeField] protected GameObject _guaranteedDrop = null;
    [TabGroup("Inscribed")] [SerializeField] [ListDrawerSettings(NumberOfItemsPerPage = 5)] protected List<GameObject> randomItems;
  

    [Title("Enemy Base Class:")]
    [TabGroup("Dynamic", TextColor = "blue")] [SerializeField] [ReadOnly] protected float currentHealth;
    [TabGroup("Dynamic")] [SerializeField] [ReadOnly]protected bool invincible = false;
    [TabGroup("Dynamic")] [SerializeField] [ReadOnly] protected float invincibleDone = 0;
    [TabGroup("Dynamic")] [SerializeField] [ReadOnly]protected bool knockback = false;
    [TabGroup("Dynamic")] [SerializeField] [ReadOnly]  protected float knockbackDone = 0;
    [TabGroup("Dynamic")] [SerializeField] [ReadOnly] protected Vector2 knockbackVel;
    [TabGroup("Dynamic")] [SerializeField] [ReadOnly] protected StateMachine _stateMachine = null;
    [TabGroup("Dynamic")] [SerializeField] [ReadOnly] protected HitBox hitBox = null;
    [TabGroup("Dynamic")] [SerializeField] [ReadOnly] protected GameObject playerRef = null;
    [TabGroup("Dynamic")] [SerializeField] [ReadOnly] protected Animator anim;
    [TabGroup("Dynamic")] [SerializeField] [ReadOnly] protected EnemyHealthBar healthBar;
    

    protected string Name
    {
        get { return name; }
        set { name = value; }
    }
    protected float MaxHealth
    {
        get { return maxHealth; }
        set { maxHealth = value; }
    }
    protected float KnockbackSpeed
    {
        get { return knockbackSpeed; }
        set { knockbackSpeed = value; }
    }
    protected float CurrencyDropped
    {
        get { return currencyDropped; }
        set { currencyDropped = value; }
    }
    protected float KnockbackDuration
    {
        get { return knockbackDuration; }
        set { knockbackDuration = value; }
    }
    protected float InvincibleDuration
    {
        get { return invincibleDuration; }
        set { invincibleDuration = value; }
    }
    protected GameObject GuaranteedDrop
    {
        get { return _guaranteedDrop; }
        set { _guaranteedDrop = value; }
    }
    protected List<GameObject> RandomItems
    {
        get { return randomItems; }
        set { randomItems = value; }
    }
    protected HitBox HitBox
    {
        get { return hitBox; }
        set { hitBox = value; }
    }
    protected GameObject PlayerRef
    {
        get { return playerRef; }
        set { playerRef = value; }
    }
    protected Animator Anim
    {
        get { return anim; }
        set { anim = value; }
    }
    protected float CurrentHealth
    {
        get { return currentHealth; }
        set { currentHealth = value; }
    }
    protected bool Invincible
    {
        get { return invincible; }
        set { invincible = value; }
    }
    protected float InvincibleDone
    {
        get { return invincibleDone; }
        set { invincibleDone = value; }
    }
    protected bool Knockback
    {
        get { return knockback; }
        set { knockback = value; }
    }
    protected float KnockbackDone
    {
        get { return knockbackDone; }
        set { knockbackDone = value; }
    }
    protected Vector2 KnockbackVel
    {
        get { return knockbackVel; }
        set { knockbackVel = value; }
    }
    public StateMachine stateMachine
    {
        get { return _stateMachine; }
        set { _stateMachine = value; }
    }
    public GameObject guaranteedDrop
    {
        get { return _guaranteedDrop; }
        set { _guaranteedDrop = value; }
    }

    


    protected virtual void Awake()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();
        stateMachine = GetComponent<StateMachine>();
        playerRef = GameObject.FindGameObjectWithTag("PlayerTargetRoot");
        healthBar = SerializationUtility.FindEnemyHealthBarInTree(this.gameObject);
    }
    protected virtual void Start()
    {
        healthBar.UpdateHealthBar(maxHealth, currentHealth);
    }
    protected virtual void Update()
    {
        // Check knockback and invincibility
        if (invincible && Time.time > invincibleDone)
            invincible = false;

        //sRend.color = invincible ? Color.red : Color.white;

        if (knockback)
        {
            //rigid.velocity = knockbackVel;
            if (Time.time < knockbackDone)
                return;
        }

        anim.speed = 1;
        knockback = false;
    }

    public virtual void OnDie() 
    {
        GameObject go;

        if (guaranteedDrop != null)
        {
            go = Instantiate<GameObject>(guaranteedDrop);
            go.transform.position = transform.position;
        }
        else if (randomItems.Count > 0)
        {
            int n = UnityEngine.Random.Range(0, randomItems.Count);
            GameObject prefab = randomItems[n];
            if (prefab != null)
            {
                go = Instantiate<GameObject>(prefab);
                go.transform.position = transform.position;
            }
        }

        if(PlayerInfo.Instance != null)
        {
            PlayerInfo.Instance.playerModifiers.currency += Mathf.FloorToInt(currencyDropped * PlayerInfo.Instance.playerModifiers.lootingMultiplier);
        }
        else
        {
            Debug.Log("PlayerInfo.Instance is null so no currency was added");
        }
    }

    public virtual void OnHitBoxTriggerEnter(DamageEffect damageInstance)
    {
       
        if (invincible)
            return; // Return if this cannot be damaged

        if (damageInstance == null)
            return; // If no DamageEffect, exit this method
        
        currentHealth -= damageInstance.damage; // Subtract the damage amount from health

        if (currentHealth <= 0)
        {
            if (EventManager.Instance != null)
            {
                EventManager.Instance.SubscribleToEnemyOnDie(this);
            }
            else
            {
                Debug.Log("onDieEvent is null");
            }
        }

           

        invincible = true; // Make this invincible
        invincibleDone = Time.time + invincibleDuration;

        /*if (dEf.knockback)
        {
            // Knockback this Enemy

            Vector2 delta;

            // Is an IFacingMover attached to the Collider that triggered this?
            IFacingMover iFM = colld.GetComponentInParent<IFacingMover>(); // f

            if (iFM != null)
            {
                // Determine the direction of knockback from the iFM�s facing
                delta = directions[iFM.GetFacing()];
            }
            else
            {
                // Determine the direction of knockback from relative position
                delta = transform.position - colld.transform.position;

                if (Mathf.Abs(delta.x) >= Mathf.Abs(delta.y))
                {
                    // Knockback should be horizontal
                    delta.x = (delta.x > 0) ? 1 : -1;
                    delta.y = 0;
                }
                else
                {
                    // Knockback should be vertical
                    delta.x = 0;
                    delta.y = (delta.y > 0) ? 1 : -1;
                }
            }

            // Apply knockback speed to the Rigidbody
            knockbackVel = delta * knockbackSpeed;
            rigid.velocity = knockbackVel;

            // Set mode to knockback and set time to stop knockback
            knockback = true;
            knockbackDone = Time.time + knockbackDuration;
            anim.speed = 0;
        }*/
    }


  
    public int tileNum { get; private set; }

    public virtual void Init(int fromTileNum, int tileX, int tileY)
    {
        tileNum = fromTileNum;

        // Position this GameObject correctly
        //transform.position = new Vector3(tileX, tileY, 0) + MapInfo.OFFSET;
    }

}

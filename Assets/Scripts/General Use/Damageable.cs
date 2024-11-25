using Assets.Scripts.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.Rendering.DebugUI;

public class Damageable : MonoBehaviour
{
    public bool debug = false;
    public bool animate = true;

    [Header ("Health AirVariables")]
    [SerializeField]
    private int maxHealth = 10;
    public int MaxHealth
    {
        get
        {
            return maxHealth;
        }
        set
        {
            maxHealth = value;
        }
    }
    [SerializeField]
    private int health = 10;
    public int Health
    {
        get
        {
            return health;
        }
        set
        {
            health = value;

            if (health <= 0)
            {
                IsAlive = false;
            }
        }
    }
    [SerializeField]
    private bool isAlive = true;
    public bool IsAlive
    {
        get
        {
            return isAlive;
        }
        set
        {
            isAlive = value;
            animator.SetBool(AnimationStrings.isAlive, value);
            Debug.Log(transform.name + "'s alive state was set to " + value);
        }
    }

    public bool isInvincible;
    bool isHit;
    public bool IsHit
    {
        get
        {
            return isHit;
        }
        private set
        {
            isHit = value;
            animator.SetBool(AnimationStrings.isHit, value);

            // Play hit audio clip
            if (audioSource != null && dmgClip != null && IsHit && Health > 0)
            {
                audioSource.PlayOneShot(dmgClip);
            }
            else if (audioSource != null && dmgLib != null && IsHit && Health > 0)
            {
                audioSource.PlayOneShot(dmgLib.Clip);
            }
        }
    }

    public AudioSource audioSource;
    private AudioSource deathAudioSource;

    [Header("Damageable Sound Variables")]

    public AudioClip dmgClip;
    public RandomAudioLibrary dmgLib;
    public AudioClip deathClip;
    public RandomAudioLibrary deathLib;

    [Header ("Hit/Death Events")]
    public bool takeKnockback;
    public float knockback;
    [SerializeField]
    private float timeSinceHit = 0;
    public float iFrames = 0.5f;
    [HideInInspector]
    public bool isDead;
    public UnityEvent onHit, onDeath;

    public Rigidbody2D rb2d;
    public Animator animator;

    private void Awake()
    {
    }

    private void Start()
    {
        Health = maxHealth;
    }

    private void Update()
    {
        if (isInvincible)
        {
            if (timeSinceHit > iFrames)
            {
                isInvincible = false;
                timeSinceHit = 0;
                IsHit = false;
            }
            timeSinceHit += Time.deltaTime;
        }

        if (!IsAlive && !isDead) { onDeath.Invoke(); isDead = true; DeathAudio(); }
    }

    public bool Hit(int damage)
    {
        if (animate)
        {
            if (IsAlive && !isInvincible)
            {
                Health -= damage;
                isInvincible = true;

                onHit.Invoke();

                if(debug) CharacterEvents.characterDamaged.Invoke(gameObject, damage);

                IsHit = true;

                return true;
            }
            else
            {
                return false;
            }
        }
        
        else
        {
            if (IsAlive && !isInvincible)
            {
                Health -= damage;
                isInvincible = true;

                onHit.Invoke();

                CharacterEvents.characterDamaged.Invoke(gameObject, damage);

                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public void Knockback(Vector3 damager)
    {
        if(rb2d != null)
        {
            Vector2 direction = (transform.position - damager).normalized;
            rb2d.velocity = direction * knockback;
        }        
    }

    public void Heal(int healAmount)
    {
        if (Health + healAmount < maxHealth) Health += healAmount;
        else if(Health + healAmount >= maxHealth) Health = maxHealth;

        if(animate) CharacterEvents.characterHealed.Invoke(gameObject, healAmount);
    }

    public void DeathAudio()
    {
        // Play death audio clip
        if (deathAudioSource == null)
        {
            GameObject audioObject = new(transform.name + "'s Death Clip");

            deathAudioSource = audioObject.AddComponent<AudioSource>();
            deathAudioSource.outputAudioMixerGroup = audioSource.outputAudioMixerGroup;

            if(deathClip != null)
            {
                deathAudioSource.clip = deathClip;
                deathAudioSource.Play();
                Destroy(audioObject, deathClip.length);
            }
            else if (deathLib != null)
            {
                AudioClip clip = deathLib.Clip;
                deathAudioSource.clip = clip;
                deathAudioSource.Play();
                Destroy(audioObject, clip.length);
            }
            deathAudioSource = null;
        }
    }

    public void DestroyThisEntity()
    {
        Destroy(gameObject);
    }
}

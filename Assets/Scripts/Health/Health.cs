using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Health")] 
    [SerializeField] private float startingHealth;
    public float currentHealth { get; private set; } 
    private Animator anim;
    private bool dead;

    [Header("iFrames")]
    [SerializeField] private float iFramesDuration;
    [SerializeField] private int numberOfFlashes;
    private SpriteRenderer spriteRend;
    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
    }
    public void TakeDamage(float _damage)   
    {
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth); 
        if (currentHealth > 0)
        {
            anim.SetTrigger("hurt");
            StartCoroutine(Invunerability());
        }
        else
        {
            if (!dead)
            {
                anim.SetTrigger("die");
                //Oyuncu
                if(GetComponent<PlayerMovement>() != null)
                    GetComponent<PlayerMovement>().enabled = false; 

                //D��man
                if(GetComponentInParent<EnemyPatrol>() !=null)
                     GetComponentInParent<EnemyPatrol>().enabled=false;

                if(GetComponent<MeleeEnemy>() !=null)
                    GetComponent<MeleeEnemy>().enabled = false;

                dead = true;
            }

        }
    }
    public void AddHealth(float _value)
    {
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, startingHealth);
    }
    private IEnumerator Invunerability() 
    {
        Physics2D.IgnoreLayerCollision(10,11, true); 
        for (int i = 0; i < numberOfFlashes; i++)
        {
            spriteRend.color = new Color(1, 0, 0, 0.5f);  
            yield return new WaitForSeconds(iFramesDuration/numberOfFlashes); 
            spriteRend.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration/numberOfFlashes);
            if (i == numberOfFlashes - 1)
                ActivatedLayer();
        }
    }
    void ActivatedLayer() 
    {
        Physics2D.IgnoreLayerCollision(10, 11, false);
    }
}

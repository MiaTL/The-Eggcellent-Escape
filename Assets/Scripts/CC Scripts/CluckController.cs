using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CluckController : MonoBehaviour
{
    private bool isInvincible;
    private Animator animator;

    public int currentHealth;
    public int maxHealth = 10;
    public int contactDamage = 1;

    private GameObject playerObject;
    private Transform player;

    public GameObject healthBar;
    public GameObject healthOutline;
    public GameObject ccText;

    public GameObject eggscapePod;

    [SerializeField] private AudioSource deathSoundEffect;
    [SerializeField] private AudioSource damageSoundEffect;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();

        playerObject = GameObject.FindGameObjectWithTag("Player");
        player = playerObject.transform;
    }

    void Update()
    {
        if (player != null)
        {
            Vector3 playerPosition = player.position;
            Vector3 objectPosition = transform.position;

            if (playerPosition.x > objectPosition.x)
            {
                // Player is in front, flip the object to face the player
                FlipOnXAxis(true);
            }
            else
            {
                // Player is behind, flip the object to face away from the player
                FlipOnXAxis(false);
            }
        }
    }

    void FlipOnXAxis(bool flip)
    {
        Vector3 scale = transform.localScale;
        if (flip)
        {
            scale.x = Mathf.Abs(scale.x) * -1;
        }
        else
        {
            scale.x = Mathf.Abs(scale.x);
        }
        transform.localScale = scale;
    }

    public void Invincible(bool invincibility)
    {
        isInvincible = invincibility;
    }

    public void TakeDamage(int damage)
    {
        if (!isInvincible)
        {
            currentHealth -= damage;
            healthBarUpdate();
            damageSoundEffect.Play();
            Mathf.Clamp(currentHealth, 0, maxHealth);
            if (currentHealth <= 0)
            {
                // deathSoundEffect.Play(); 
                healthOutline.SetActive(false);
                ccText.SetActive(false);
                Defeat();
            }
        }
    }

    public void Defeat()
    {
        deathSoundEffect.Play(); // not playing on death I do not know why...
        animator.Play("CC_Death");
        StartCoroutine(DestroyAfterAnimation());

        if (eggscapePod != null)
        {
            eggscapePod.SetActive(true);
        }
    }

    public void healthBarUpdate()
    {
        LeanTween.scaleX(healthBar, currentHealth * 0.1f, 0);
    }

    private IEnumerator DestroyAfterAnimation()
    {
        // Wait until the "Death" animation has finished
        yield return new WaitForSeconds(animator.GetCurrentAnimatorClipInfo(0)[0].clip.length);

        // Now, destroy the game object
        Destroy(gameObject);
    }
}

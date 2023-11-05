using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerLife : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;

    private bool IsDeath;
    public int Hp;
    public int numOfHp;

    public Image[] hearts;
    public Sprite fullHp;
    public Sprite emptyHp;

    [Header("iFrames")]
    [SerializeField] private float iFramesDuration;
    [SerializeField] private int numberOfFlashes;
    [SerializeField] private bool invisble;
    private SpriteRenderer spriteRend;

    [Header("Heal")]
    private float timeSinceLastDamage = 0.0f;
    private bool isCountingDamageTime = false;
    private float timeToRestoreHealth = 7.0f;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRend = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Hp > numOfHp)
        {
            Hp = numOfHp;
        }


        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < Hp)
            {
                hearts[i].sprite = fullHp;
            }
            else
            {
                hearts[i].sprite = emptyHp;
            }

            if (i < numOfHp)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }

        if (isCountingDamageTime)
        {
            timeSinceLastDamage += Time.deltaTime;
            if (timeSinceLastDamage >= timeToRestoreHealth)
            {
                // Restore health when 7 seconds have passed without taking damage
                Hp = numOfHp; 
                isCountingDamageTime = false;
            }
        }

        if (IsDeath && (invisble ==false))

            RestartLevel();

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Attack"))
        {
            //anim.SetTrigger("Hurt");
            Hurt();
        }
    }

    public void Hurt()
    {
        Hp--;
        StartCoroutine(Invunerability());
        if (Hp == 0)
            Die();
        timeSinceLastDamage = 0.0f;
        isCountingDamageTime = true;

    }
    private void Die()
    {
        
        rb.bodyType = RigidbodyType2D.Static;
        //anim.SetTrigger("death");
        IsDeath = true;
    }

    private void RestartLevel()
    {
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

    private IEnumerator Invunerability()
    {
        invisble = true;
        Physics2D.IgnoreLayerCollision(7, 8, true);
        for (int i = 0; i < numberOfFlashes; i++)
        {
            spriteRend.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
            spriteRend.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
        }

        Physics2D.IgnoreLayerCollision(7, 8, false);
        invisble = false;
    }
}

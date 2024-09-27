using System.Collections;
using UnityEngine;
public class Character : MonoBehaviour
{
    [SerializeField] protected int maxHP ;
    [SerializeField] protected int currentHP;
    [SerializeField] protected float missedChance ;
    [SerializeField] protected float timeToThink ;
    [SerializeField] protected float timeToWarning ;

    [SerializeField] protected int normalAttackDamage ;
    [SerializeField] protected int smallAttackDamage;
    [SerializeField] protected int powerThrowDamage ;
    [SerializeField] protected int doubleAttackDamage ;
    [SerializeField] protected int attackAmount ;
    [SerializeField] protected int healAmount ;
    [SerializeField] protected int throwDirection;

    public bool isTurn;
    protected float windForce;
  public int MaxHP
    {
        get { return maxHP; }
    }

      public int AttackAmount
    {
        get { return attackAmount; }
    }



    public int NormalAttackDamage
    {
        get { return normalAttackDamage; }
    }

 
 public int SmallAttackDamage
    {
        get { return smallAttackDamage; }
    }
 
   public int PowerThrowDamage
    {
        get { return powerThrowDamage; }
    }
 
 public int DoubleAttackDamage
    {
        get { return doubleAttackDamage; }
    }



    public int CurrentHP
    {
        get { return currentHP; }
    }
 public int ThrowDirection
    {
        get { return throwDirection; }
    }

    protected virtual void Start()
    {
        
    }

    public virtual void TakeDamage(int damage)
    {
        currentHP -= damage;
        Debug.Log(gameObject.name + " takes " + damage + " damage. Remaining HP: " + currentHP);

        if (currentHP <= 0)
        {
            currentHP = 0;
            Die();
        }
    }

    public virtual void Heal()
    {
        currentHP += healAmount;
        if (currentHP > maxHP)
            currentHP = maxHP;
    }

    protected virtual void Die()
    {
        Debug.Log(gameObject.name + " has been defeated.");
        // Add any additional defeat logic (e.g., disabling the player or ending the game).
    }

    public bool IsAlive()
    {
        return currentHP > 0;
    }
}

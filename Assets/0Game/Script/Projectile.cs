using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 windForce;

    public Player currentPlayer;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // ฟังก์ชันสำหรับการโยนไอเทม
    public void Launch(Vector2 direction, float power, Vector2 _windForce)
    {

        windForce = _windForce;

        Vector2 force = direction * power + windForce;
        rb.AddForce(force, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Player>() != null)
        {
            if (collision.gameObject.GetComponent<Player>().playername != currentPlayer.playername)
            {
                Player target = collision.gameObject.GetComponent<Player>();

                if (target != null)
                {
                    Vector2 hitPoint = collision.contacts[0].point;
                    Vector2 playerCenter = target.transform.position;

                      currentPlayer.animationControl.CallAnimation(currentPlayer.animationControl.HappyFriendly, false);
                     

                    if (currentPlayer.isPowerThrow)
                    {

                        target.TakeDamage(currentPlayer.PowerThrowDamage);
                        currentPlayer.isPowerThrow = false;
                    }
                    else if (currentPlayer.isDoubleAttack)
                    {

                        target.TakeDamage(currentPlayer.DoubleAttackDamage*currentPlayer.AttackAmount);
                          currentPlayer.isDoubleAttack = false;
                    }
                    else
                    {

                        if (hitPoint.y > playerCenter.y + 0.5f)
                        {

                            target.TakeDamage(currentPlayer.NormalAttackDamage);
                        }
                        else
                        {

                            target.TakeDamage(currentPlayer.SmallAttackDamage);
                        }
                    }


                    Destroy(gameObject);
                }
            }
        }
    }
}

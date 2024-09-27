using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class Player : Character
{
    public string playername;
    public bool isBot = false;
    private float powerMeter;
    private bool increasingPower = true;
    [SerializeField] private float maxPower = 10f;
    [SerializeField] private float powerMeterSpeed = 2f;
    [SerializeField] private Transform throwOrigin;
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private GameObject bombPrefab;
    public HPControl hpControl;
    public ItemsControl itemsControl;
    [HideInInspector]
    public bool isPowerThrow = false;
    [HideInInspector]
    public bool isDoubleAttack = false;
    GoogleSheetsData data;
    private bool hasThrown = false;
    public AnimationControl animationControl;
    protected override void Start()
    {
        base.Start();
        animationControl.CallAnimation(animationControl.IdleUnFriendly1, true);


    }

    public void SetupGameData()
    {
        data = GoogleSheetsData.googleSheets;
        if (isBot)
        {
            int level = PlayerPrefs.GetInt("level");
            maxHP = data.gameDataList.enemyLevel[level].hp;
            missedChance = data.gameDataList.enemyLevel[level].missedChance;
        }
        else
        {
            maxHP = data.gameDataList.player.hp;
        }

        normalAttackDamage = data.gameDataList.normalAttack.damage;
        smallAttackDamage = data.gameDataList.smallAttack.damage;
        powerThrowDamage = data.gameDataList.powerThorw.damage;
        doubleAttackDamage = data.gameDataList.doubleAttack.damage;
        attackAmount = data.gameDataList.doubleAttack.amount;
        healAmount = data.gameDataList.heal.hp;
        timeToThink = data.gameDataList.timetoThink.sec;
        timeToWarning = data.gameDataList.timetowarnning.sec;
        currentHP = maxHP;
        hpControl.UpdateHPBar(currentHP, maxHP);

    }
    private void Update()
    {
        if (isTurn && !isBot)
        {
            HandleInput();
        }
    }

    public void SetTurn(bool turn)
    {
        isTurn = turn;

        if (isTurn)
        {

            if (isBot)
            {
                StartCoroutine(BotTakeTurn());
            }
        }
    }

    public void HandleInput()
    {


        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }


        if (Input.GetMouseButtonDown(0))
        {
            itemsControl.powerGroup.SetActive(true);
            powerMeter = 0f;
            hasThrown = false;
        }


        if (Input.GetMouseButton(0))
        {
            if (increasingPower)
            {
                powerMeter += Time.deltaTime * powerMeterSpeed * maxPower;
                if (powerMeter >= maxPower)
                {
                    powerMeter = maxPower;
                    increasingPower = false;
                }
            }
            else
            {
                powerMeter -= Time.deltaTime * powerMeterSpeed * maxPower;
                if (powerMeter <= 0)
                {
                    powerMeter = 0;
                    increasingPower = true;
                }
            }

            itemsControl.UpdatePowerUI(powerMeter, maxPower);
        }


        if (Input.GetMouseButtonUp(0) && !hasThrown)
        {
            hasThrown = true;
            itemsControl.ResetPowerUI();
            ThrowItem(powerMeter);
            Invoke(nameof(DelayTurn), 0.5f);
        }

    }

    void DelayTurn()
    {

        GameManager.instance.turnManager.SwitchTurn();
    }

    public IEnumerator BotTakeTurn()
    {
        yield return new WaitForSeconds(1.5f);

        int level = PlayerPrefs.GetInt("level");
    
        bool throwMissed = Random.value < missedChance;

        if (level == 0)
        {
            
            if (throwMissed)
            {

                ThrowItem(2);
            }
            else
            {
                ThrowItem(Random.Range(5, 10));
            }
        }
        else if (level == 1)
        {

            if (windForce < 0.5f)
            {
                isDoubleAttack = true;
                ThrowItem(5);
            }
            else if (!throwMissed)
            {
                ThrowItem(Random.Range(5, 10));
            }
            else
            {

                ThrowItem(2);
            }
        }
        else if (level == 2)
        {

            if (windForce > 0.5f)
            {
                isPowerThrow = true;
                ThrowItem(10);
            }
            else
            {
                ThrowItem(5);
            }
        }

        GameManager.instance.turnManager.SwitchTurn();
    }



    private void ThrowItem(float power)
    {

        GameObject item;
        if (isPowerThrow)
        {
            item = Instantiate(bombPrefab, throwOrigin.position, Quaternion.identity);
        }
        else
        {
            item = Instantiate(itemPrefab, throwOrigin.position, Quaternion.identity);
        }


        Projectile projectile = item.GetComponent<Projectile>();
        projectile.currentPlayer = this;
        Vector2 throwDirection = new Vector2(ThrowDirection, 1).normalized;

        projectile.Launch(throwDirection, power, new Vector2(windForce, 0));

    }

    public void SetWindForce(float wind)
    {
        windForce = wind;
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        hpControl.UpdateHPBar(currentHP, maxHP);
        if (!IsAlive())
        {
            EndGame();
        }
    }

    public void SetHeal()
    {
        Heal();
    }


    private void EndGame()
    {
        animationControl.CallAnimation(animationControl.MoodyUnFriendly, true);
        GameManager.instance.turnManager.GetOpponent(this).animationControl.CallAnimation(animationControl.CheerFriendly, true);
        if (isBot)
        {


            GameManager.instance.turnManager.EndGame("YOU Wins!");
        }
        else
        {

            GameManager.instance.turnManager.EndGame(GameManager.instance.turnManager.GetOpponent(this).playername + " Wins!");
        }
    }



}

using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public bool isPlayer1Turn = true;

    GameManager gm;
    private void Start()
    {
        gm = GameManager.instance;

        gm.player1.SetTurn(true);
        gm.player2.SetTurn(false);



        float wind = Random.Range(-1f, 1f);  
        gm.player1.SetWindForce(wind);
        gm.player2.SetWindForce(wind);
    }
    private void Update()
    {

        if (!gm.player1.IsAlive() || !gm.player2.IsAlive())
        {

            return;
        }
    }


    public void SwitchTurn()
    {
        if (gm.player1.CurrentHP <= 0 || gm.player2.CurrentHP <= 0)
        {
            return;
        }

        isPlayer1Turn = !isPlayer1Turn;


        gm.player1.SetTurn(isPlayer1Turn);
        gm.player2.SetTurn(!isPlayer1Turn);


        float wind = Random.Range(-1f, 1f);
        gm.player1.SetWindForce(wind);
        gm.player2.SetWindForce(wind);


    }

    public void EndGame(string result)
    {
      
       gm.resultControl.ShowResult(result);
    }

    // ฟังก์ชันดึงผู้เล่นฝ่ายตรงข้าม
    public Player GetOpponent(Player currentPlayer)
    {
        return currentPlayer == gm.player1 ? gm.player2 : gm.player1;
    }
}

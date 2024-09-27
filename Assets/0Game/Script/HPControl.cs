using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPControl : MonoBehaviour
{
     
[SerializeField] private Image hpBar; // Image สำหรับ HP Bar
 

     

    public void UpdateHPBar(int hp,int maxHP)
    {
        
        hpBar.fillAmount = (float)hp / maxHP;
    }

    
   
 
}

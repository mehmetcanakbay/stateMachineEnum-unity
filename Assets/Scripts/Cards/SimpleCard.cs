using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//simple card, simply check if we have enough mana
public class SimpleCard : BaseCard
{
    //consuming the amount to spend logic is in the basecard 
    public override bool CheckCondition() {
        if (GameManager.Instance == null) return false;
        if (GameManager.Instance.ReturnCurrentPlayerMana() >= amountToSpend) {
            return true;
        } else {
            return false;
        }
    }
}

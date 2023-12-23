using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseCard : MonoBehaviour, IConditional
{
    [SerializeField]
    protected int amountToSpend = 0;
    //In this game, you could actually make this non abstract. 
    //e.g. card needs -3 mana, check here, then do what you need to etc.
    //but, just to be sure, if a card needs a special condition, lets just do it this way.
    public abstract bool CheckCondition();

    public void OnClickedInteractable() {
        if (GameManager.Instance != null) {
            GameManager.Instance.AddToPlayerMana(-amountToSpend);
        }
    }

}

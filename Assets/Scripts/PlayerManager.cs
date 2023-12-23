using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField]
    private int Health;
    [SerializeField]
    private int MaxHealth;
    
    public delegate void SingleIntArgumentSignature(int arg1);
    public event SingleIntArgumentSignature healthChange;

    public void AddToPlayerHealth(int amount) {
        Health += amount;
        healthChange.Invoke(amount);
    }    

    public int ReturnHealth() {
        return Health;
    }

    public int ReturnMaxHealth() {
        return MaxHealth;
    }
    
}

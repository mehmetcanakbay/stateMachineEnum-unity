using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public static GameManager Instance;

    public GameObject Player;
    public GameObject Enemy;

    public UIManager uiManager;
    public GameStateManager gameStateManager;
    public NotificationInstance enemyNotif;

    //this should be in gamemanager and not in their respective scripts.
    //accesing them would be way harder that way, this is not a big game, not needed
    //to make this more "generalized", a scriptable object approach might be better
    //to explain this a little bit more, for example, player's one can stay like this,
    //but if an enemy has an abnormal max mana for example,
    //during reloading the enemy (current enemy will eventually die and you have to respawn it again)
    //there can be a specialized function in here that would call "SwitchScriptableObject" and you would then call
    //an initialize function written for example. 
    //think about how its done in states, but it will instead inherit from scriptable object.
    [SerializeField]
    private int playerMaxMana;
    private int currentPlayerMana;

    private int enemyMaxMana;
    private int currentEnemyMana;

    /// <summary>
    /// Specify the random amount that will be added to the current player. This uses square parantheses [] meaning both inclusive.
    /// </summary>
    public int[] randomAmountClosedPlayer = new int[2]; 

    /// <summary>
    /// Specify the random amount that will be added to the current enemy. This uses square parantheses [] meaning both inclusive.
    /// </summary>
    public int[] randomAmountClosedEnemy = new int[2]; 


    private void Awake() {
        //set singleton
        Instance = this;
    }

    private void Start() {
        currentPlayerMana = 2;
        currentEnemyMana = 1;
    }

    /// <summary>
    /// Add to player mana. This can accept negative values to "consume" mana.
    /// </summary>
    /// <param name="amount">Specify the amount of mana to add</param>
    public void AddToPlayerMana(int amount) {
        currentPlayerMana += amount;
        currentEnemyMana = Mathf.Clamp(currentPlayerMana, 0, playerMaxMana);
        uiManager.UpdateManaUI(true);
    }

    /// <summary>
    /// Return Max Player Mana
    /// </summary>
    /// <returns>Return Max Player Mana</returns>
    public int ReturnMaxPlayerMana() {
        return playerMaxMana;
    }

    /// <summary>
    /// Return Current Player Mana
    /// </summary>
    /// <returns>Return Current Player Mana</returns>
    public int ReturnCurrentPlayerMana() {
        return currentPlayerMana;
    }

    /// <summary>
    /// Add to enemy mana. This can accept negative values to "consume" mana.
    /// </summary>
    /// <param name="amount">Specify the amount of mana to add</param>
    public void AddToEnemyMana(int amount) {
        currentEnemyMana += amount;
        uiManager.UpdateManaUI();
    }

    /// <summary>
    /// Return Max Enemy Mana
    /// </summary>
    /// <returns>Return Max Enemy Mana</returns>
    public int ReturnMaxEnemyMana() {
        return enemyMaxMana;
    }

    /// <summary>
    /// Return Current Enemy Mana
    /// </summary>
    /// <returns>Return Current Enemy Mana</returns>
    public int ReturnCurrentEnemyMana() {
        return currentEnemyMana;
    }

    //wrapper, no need to make UIManager singleton as well have a ref here then just wrap it using this singleton
    public void EnablePlayerChoice() {
        uiManager.EnablePlayerChoices();
    }

    public void DisablePlayerChoice() {
        uiManager.DisablePlayerChoices();
    }
    /*wrap end*/

    public void ChangeEnemyState(E_EnemyStateMachineStates state) {
        //I'm not going to cache this. Since the enemy can change, its better to get the base class like this.
        //But to be honest, if the enemy could change, then on the change function you would probably renew the
        //variable as well. overall I'm not going to bother with it in this project, not worth
        Enemy.GetComponent<EnemyBase>().ChangeState(state);
        uiManager.PushEnemyStateDecisionNotif(state);
    }

    public void PushNotif(string txt) {
        enemyNotif.PushNotif(txt);
    }

}
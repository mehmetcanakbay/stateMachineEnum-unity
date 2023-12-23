using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class UIManager : MonoBehaviour
{
    public GameStateManager stateManager;
    public HorizontalLayoutGroup playerChoiceLayoutGroup;
    public TMP_Text stateNameText;

    //probably better to hold a ref to layout group than a gameobject?
    public GameObject manaInfoHolder;

    private Button[] playerChoiceButtons;
    private PlayerManager playerManager;
    public Slider playerHealthBar;

    

    bool didInitializeProperly = false;

    // Start is called before the first frame update
    void Start()
    {
        //if I dont do this, there can be a race condition between gamemanager and this one
        //check order of operations in unity
        if (!didInitializeProperly) {
            if (GameManager.Instance != null) {
                GameManager.Instance.gameStateManager.onStateChange += UpdateStateNameNotif;

                playerManager = GameManager.Instance.Player.GetComponent<PlayerManager>();
                playerManager.healthChange += UpdateHealthBar;

                didInitializeProperly = true;
            } else {
                Debug.LogWarning("UIManager could not bind functions to GameManager!!!");
            }
        }

        Transform layoutTransform = playerChoiceLayoutGroup.transform;
        playerChoiceButtons = new Button[layoutTransform.childCount];
        //cache the buttons
        for (int i = 0; i<layoutTransform.childCount; i++) {
            Button childButton = layoutTransform.GetChild(i).GetComponent<Button>();
            playerChoiceButtons[i] = childButton;
        }

        UpdateManaUI();
        UpdateManaUI(true);
        EnablePlayerChoices();
    }

    private void OnEnable() {
        if (!didInitializeProperly) return;
        if (GameManager.Instance != null)
            GameManager.Instance.gameStateManager.onStateChange += UpdateStateNameNotif;
    }

    private void OnDisable() {
        if (GameManager.Instance != null)
            GameManager.Instance.gameStateManager.onStateChange -= UpdateStateNameNotif;
    }

    public void DebugOne() {
        stateManager.ChangeState(E_GameStates.EnemyTurn);
    }

    public void DebugTwo() {
    }

    public void DebugThird() {
        // stateManager.ConfirmEndAllocatingTurn();
    }

    private void UpdateHealthBar(int amount) {
        //cast to float so the outcome becomes a float
        playerHealthBar.value = (float)playerManager.ReturnHealth() / playerManager.ReturnMaxHealth();
    }

    //these two are called in StatePlayerTurn, onstateenter and exit respectively
    public void EnablePlayerChoices() {
        for (int i = 0; i<playerChoiceButtons.Length; i++) {
            IConditional doesImplement = playerChoiceButtons[i].GetComponent<IConditional>();
            //if there is an interface
            //wow this looks ugly as hell
            if (doesImplement != null) {
                //if the condition is true
                if (doesImplement.CheckCondition()) {
                    playerChoiceButtons[i].interactable = true;
                } else {
                    playerChoiceButtons[i].interactable = false;
                }
            } else { //if not implemented activate it
                playerChoiceButtons[i].interactable = true;
            }
        }
    }

    public void DisablePlayerChoices() {
        for (int i = 0; i<playerChoiceButtons.Length; i++) {
            playerChoiceButtons[i].interactable = false;
        }
    }

    //okay... if we had more options, an enum would be better, but I'll just use a bool. It could be 
    //better to just... i dunno, split the functions, but that results in copy paste code; it looks ugly 
    public void UpdateManaUI(bool player = false) {
        Transform child = manaInfoHolder.transform.GetChild(player ? 1 : 0);
        if (child != null) {
            TMP_Text textAsset = child.GetComponent<TMP_Text>();
            if (!player) {
                textAsset.text = GameManager.Instance ? GameManager.Instance.ReturnCurrentEnemyMana().ToString() : "999";
            } else {
                textAsset.text = GameManager.Instance ? GameManager.Instance.ReturnCurrentPlayerMana().ToString() : "999";
            }
        }
    }

    //on a state change, this function fires, its bound to an event that is fired 
    //after a state change in gamestatemanager
    private void UpdateStateNameNotif(E_GameStates newState) {
        string toChangeInto = "NULL";
        switch(newState) {
            case E_GameStates.PlayerTurn:
                toChangeInto = "Player Turn";
                break;
            case E_GameStates.EnemyTurn:
                toChangeInto = "Enemy Turn";
                break;
            case E_GameStates.AllocatingPhase:
                toChangeInto = "Allocating Mana...";
                break;
        }
        stateNameText.text = toChangeInto;
    }

    public void PushEnemyStateDecisionNotif(E_EnemyStateMachineStates state) {
        //TODO
    }
}

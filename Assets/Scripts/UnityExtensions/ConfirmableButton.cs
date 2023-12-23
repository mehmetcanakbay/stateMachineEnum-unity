using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;

public class ConfirmableButton : Button
{
    //if I touch this, go into an confirmable button mode,
    //loop through all these buttons, and delete their confirmable status.
    //activate this one's confirmable status afterwards. 
    [System.Serializable]
    public class OnConfirmEvent : UnityEvent {}

    [SerializeField]
    public OnConfirmEvent onConfirm;
    ConfirmableButton[] sharedButtonPrompt;
    //could just use a bool here, but maybe I need more clicked queries later down in the line
    public int clickedAmount = 0;

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        if (clickedAmount == 0) {
            onClick.Invoke();
            clickedAmount += 1;
            return;
        } 

        if (clickedAmount == 1) {
            onConfirm.Invoke();
        }
    
        

        
    }
}

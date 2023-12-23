using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NotificatorObject : MonoBehaviour, INotification
{

    private TMP_Text textObj;
    Transform mainTextHolder;
    RectTransform animatorObj;
    CanvasGroup canvasGroup;

    //I can use get component on these easily, because I know for sure that these cant be null.
    //If they are null, then theres something horribly wrong.
    void Initialize() {
        mainTextHolder = transform.GetChild(0);
        textObj = mainTextHolder.GetChild(0).GetComponent<TMP_Text>();
        canvasGroup = GetComponent<CanvasGroup>();
        
        animatorObj = transform.GetChild(1).GetComponent<RectTransform>();
    }

    //Animations should sit here because its a pooled object.
    private void OnEnable() {
        if (mainTextHolder == null) {
            Initialize();
        }
        //first, hide the text and its bg.
        mainTextHolder.gameObject.SetActive(false);
        canvasGroup.alpha = 1.0f;

        //then scale x pivot x 0
        animatorObj.pivot = new Vector2(0.0f, 1.0f);
        animatorObj.sizeDelta = Vector2.zero;
        LeanTween.value(0.0f, 1.0f, 0.35f).setOnUpdate((float val)=>{
            //vector is a struct. this "new" doesnt impact performance
            animatorObj.localScale = new Vector3(val, 1.0f, 1.0f);
        }).setEase(LeanTweenType.easeOutSine);

        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut() {
        yield return new WaitForSeconds(0.35f);
        //enable text and its bg
        mainTextHolder.gameObject.SetActive(true);

        //after this, pivot x1, scale to 0
        animatorObj.pivot = new Vector2(1.0f, 1.0f);
        animatorObj.sizeDelta = Vector2.zero;
        LeanTween.value(1.0f, 0.0f, 0.6f).setOnUpdate((float val)=>{
            //vector is a struct. this "new" doesnt impact performance
            animatorObj.localScale = new Vector3(val, 1.0f, 1.0f);
        }).setEase(LeanTweenType.easeOutExpo);

        //FADE OUT START
        yield return new WaitForSeconds(1.0f);
        LeanTween.value(1.0f, 0.0f, 0.6f).setOnUpdate((float val)=>{
            //vector is a struct. this "new" doesnt impact performance
            canvasGroup.alpha = val;
        });
    }

    private void SetText(string txt) {
        textObj.text = txt;
    }

    public void PushNotif(string txt) {
        SetText(txt);
    }

}

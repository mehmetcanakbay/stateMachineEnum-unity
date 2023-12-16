using UnityEngine;

public class MyGameStateData : IBaseStateData {
    public GameObject gameObject1;
    public GameObject gameObject2;
    public string text;

    public MyGameStateData(GameObject go1, GameObject go2, string tx) {
        gameObject1 = go1;
        gameObject2 = go2;
        text = tx;
    }
}
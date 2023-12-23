using UnityEngine;

public class ReflectionUnity : IInitializable, ITickable {
    public void Initialize() {
        Debug.Log("Init"); 
    }

    public void Tick() {
        Debug.Log("Tick");
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField]
    private GameObject objectToPool;
    [SerializeField]
    private int amountToPool;

    private GameObject[] pooledObjects;  

    //going to make this in awake just in case I want to push notifs in start
    //though thats unlikely 
    void Awake()
    {
        pooledObjects = new GameObject[amountToPool];
        
        for (int i = 0; i<amountToPool; i++) {
            GameObject pooledObj = Instantiate(objectToPool, transform.position, Quaternion.identity); 
            pooledObj.SetActive(false);   
            RectTransform rt = pooledObj.GetComponent<RectTransform>(); 
            rt.SetParent(this.transform, false);    
            rt.localPosition = Vector3.zero;
            pooledObjects[i] = pooledObj;
        }
    }

    public int GiveAvailableObjectIndex(bool setActiveImmediately = true) {
        for (int i = 0; i<amountToPool; i++) {  
            //find the first non-active gameobject, and return the index     
            if (!pooledObjects[i].activeSelf) {
                pooledObjects[i].SetActive(setActiveImmediately);
                return i;
            }
        }
        Debug.LogWarning(this.gameObject.name + " Object Pool doesnt have enough pooled objects");
        return 0;
    }

    public GameObject ReturnObjectRef(int i) {
        return pooledObjects[i];
    }

    public void ReturnToPool(int i) {
        pooledObjects[i].SetActive(false);
    }

}

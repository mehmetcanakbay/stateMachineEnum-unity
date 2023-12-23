using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationInstance : MonoBehaviour
{
    [SerializeField]
    private ObjectPool pool;

    public void PushNotif(string txt) {
        int availableObject = pool.GiveAvailableObjectIndex();
        GameObject availableObj = pool.ReturnObjectRef(availableObject);

        INotification doesImplement = availableObj.GetComponent<INotification>();

        if (doesImplement != null) {
            doesImplement.PushNotif(txt);
        }

    }
}

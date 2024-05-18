using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenobjectHolder : MonoBehaviour
{
    [SerializeField] private Transform holdPoint;
    private KitchenObject kitchenObject;

    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }
    public KitchenObjectSO GetKitchenObjectSO() 
    { 
        return kitchenObject.GetKitchenObjectSO();
    }
    public bool IsHaveKitchenObject()
    {
        return kitchenObject != null;
    }
    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
        kitchenObject.transform.localPosition = Vector3.zero;
    }
    public Transform GetHoldPoint()
    {
        return holdPoint;
    }
    public void TransferKitchenObject(KitchenobjectHolder sourceHolder, KitchenobjectHolder targetHolder)
    {
        if (sourceHolder.GetKitchenObject() == null)
        {
            Debug.LogWarning("Nao ha ingredientes no suporte original e a transferencia falhou.");
            return;
        }
        if (targetHolder.GetKitchenObject() != null)
        {
            Debug.LogWarning(" ha ingredientes no suporte original e a transferencia falhou.");
            return;
        }
        targetHolder.AddKitchenObject(sourceHolder.GetKitchenObject());
        sourceHolder.ClearKitchenObject();
    }
    public void AddKitchenObject(KitchenObject kitchenObject)
    {
        kitchenObject.transform.SetParent(holdPoint);
        kitchenObject.transform.localPosition = Vector3.zero;
        this.kitchenObject = kitchenObject;
    }

    public void ClearKitchenObject()
    {
        this.kitchenObject = null;
    }
    public void DestroyKitchenObject()
    {
        Destroy(kitchenObject.gameObject);
        ClearKitchenObject ();
    }
    public void CreateKitchenObject(GameObject kitchenObjectPrefab)
    {
        KitchenObject kitchenObject = GameObject.Instantiate(kitchenObjectPrefab, GetHoldPoint()).GetComponent<KitchenObject>();
        SetKitchenObject(kitchenObject);
    }
}

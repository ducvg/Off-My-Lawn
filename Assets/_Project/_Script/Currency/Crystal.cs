using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;
using UnityEngine.EventSystems;

public class Crystal : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] FloatValueSO crystalValue;
    [SerializeField] Rigidbody rb;
    private float amount = 25f;
    private Tween moveTween;
    private bool isCollected;

    public void Init(CrystalConfigSO config)
    {
        amount = config.Amount;
        transform.localScale = Vector3.one * config.Size;
        isCollected = false;

        moveTween.Complete();
    }
    
    public void Fling(float force)
    {
        rb.velocity = Vector3.zero;
        const float offsetRange = 1f; 
        Vector3 randXZ = new Vector3(
            Random.Range(-offsetRange, offsetRange),
            0,
            Random.Range(-offsetRange, offsetRange)
        );
        rb.AddForce(Vector3.up * force + randXZ, ForceMode.Impulse);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isCollected) return;
        isCollected = true;
        crystalValue.Value += amount;
        moveTween = Tween.Position(transform, new Vector3(0.5f,2.5f,6f), duration: 2f, Ease.OutQuart)
                        .OnComplete(this, target => target.Release());
    }

    void Release()
    {
        CrystalFactory.Instance.Release(this);
    }
}

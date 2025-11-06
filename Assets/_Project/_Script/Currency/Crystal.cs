using PrimeTween;
using UnityEngine;
using UnityEngine.EventSystems;

public class Crystal : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] FloatValueSO crystalValue;
    [SerializeField] Transform graphicTransform;
    [SerializeField] Rigidbody rb;
    private float amount = 25f;
    private Tween collectTween;
    private Tween idleTween;
    private bool isCollected;

    public void Init(CrystalConfigSO config)
    {
        amount = config.Amount;
        transform.localScale = Vector3.one * config.Size;
        isCollected = false;
        Idle();
    }

    void Idle()
    {
        idleTween = Tween.EulerAngles(graphicTransform, startValue: Vector3.zero, endValue: new Vector3(0, 360, 0), duration: 1f,
                                        Ease.Linear, cycles: -1, cycleMode: CycleMode.Restart);
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

        idleTween.Complete();
        crystalValue.Value += amount;
        collectTween = Tween.Position(transform, new Vector3(-0.33f, 1.82f, 6.71f), duration: 2f, Ease.OutQuart)
                        .OnComplete(this, target => target.Release());
    }

    void Release()
    {
        collectTween.Complete();
        idleTween.Complete();
        CrystalFactory.Instance.Release(this);
    }
}

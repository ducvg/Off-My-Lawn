using PrimeTween;
using UnityEngine;

public class FakeBodyPart : MonoBehaviour
{
    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private MeshCollider meshCollider;
    [SerializeField] private Rigidbody rb;
    private Material material;

    public void Init(SkinnedMeshRenderer skinnedMesh)
    {
        meshCollider.sharedMesh = meshFilter.mesh = skinnedMesh.sharedMesh;
        meshRenderer.material = skinnedMesh.sharedMaterial;
        material = meshRenderer.material;

        Tween.PositionY(transform, endValue: -2f, duration: 2f, Ease.InCubic, startDelay: GameConstant.OBJECT_DESPAWN_TIME)
            .OnComplete(this, target => target.Release());
    }

    public void Fling(float force)
    {
        rb.velocity = Vector3.zero;
        const float offsetRange = 0.3f; 
        Vector3 randXZ = new Vector3(
            Random.Range(-offsetRange, offsetRange),
            0,
            Random.Range(-offsetRange, offsetRange)
        );
        rb.AddForce(Vector3.up * force + randXZ, ForceMode.Impulse);
    }

    void Release()
    {
        Destroy(material);
        BodyPartFactory.Instance.Release(this);
    }

    void OnDestroy()
    {
        if(material) Destroy(material);
    }
}
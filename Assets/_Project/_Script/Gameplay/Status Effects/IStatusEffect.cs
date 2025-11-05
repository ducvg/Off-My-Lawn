using Unity.VisualScripting;

public interface IStatusEffect
{
    void OnDuplicate(Entity target);
    void OnApply(Entity target);
    void OnUpdate(Entity target);
    void OnRemove(Entity target);
    IStatusEffect Clone();
}

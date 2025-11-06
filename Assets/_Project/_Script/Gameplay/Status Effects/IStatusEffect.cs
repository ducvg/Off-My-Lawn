public interface IStatusEffect
{
    void OnDuplicate(Entity target);
    bool OnApply(Entity target);
    void OnUpdate(Entity target);
    void OnRemove(Entity target);
    IStatusEffect Clone();
}


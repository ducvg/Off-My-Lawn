public class GenericHeadArmor : Armor
{
    public override void Block(Entity entity, ref float damage)
    {
        float leftoverDamage = damage - health;
        health -= damage;
        damage = leftoverDamage > 0 ? leftoverDamage : 0;

        if (health <= 0)
        {
            Detach();
            Unequip(entity);
        }
    }

    void Detach()
    {
        DetachObjectFactory.Instance.Spawn(meshFilter.mesh, Material, transform.position, transform.rotation)
            .Fling(force: 5f);
    }
}
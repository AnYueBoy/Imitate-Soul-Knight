public interface IConfigManager {
    void Init ();

    WeaponConfig WeaponConfig { get; }

    EnemyConfig EnemyConfig { get; }

    RoleConfig RoleConfig { get; }

}
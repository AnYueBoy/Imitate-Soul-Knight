public interface IPlayerDataManager {

    void Init ();

    void SaveDataByFixedTime (float deltaTime);

    int CurRoleId { get; }

    int CurWeaponId { get; }
}
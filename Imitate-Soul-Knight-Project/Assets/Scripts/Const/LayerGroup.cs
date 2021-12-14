/*
 * @Author: l hy 
 * @Date: 2021-11-12 12:02:22 
 * @Description: Layer组
 */
public class LayerGroup {

    #region  设定在unity中的层
    public const string enemy = "Enemy";

    public const string player = "Player";

    public const string block = "Block";

    public const string destructibleBlock = "DestructibleBlock";

    #endregion

    #region  未设定在unity中的层，但在项目中使用

    public const string enemyBullet = "EnemyBullet";
    public const string playerBullet = "PlayerBullet";

    #endregion
}
/*
 * @Author: l hy 
 * @Date: 2021-11-16 08:18:12 
 * @Description: 敌人数据
 */

public class EnemyData {
    public float curHp;

    public EnemyData (EnemyConfigData enemyConfigData) {
        this.curHp = enemyConfigData.hp;
    }
}
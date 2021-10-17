/*
 * @Author: l hy 
 * @Date: 2021-05-26 19:45:06 
 * @Description: 基础数据类
 */

using LitJson;

public class BaseData {

    public T clone<T> () where T : BaseData {
        string content = JsonMapper.ToJson (this);
        T targetData = JsonMapper.ToObject<T> (content);
        return targetData;
    }
}
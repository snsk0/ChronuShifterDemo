using System.Collections.Generic;

namespace FSMUtils
{
    public class ObjectContainer : IReadOnlyObjectContainer
    {
        //ロジックリスト
        private readonly List<object> objectList;


        //コンストラクタ
        public ObjectContainer()
        {
            objectList = new List<object>();
        }



        //ロジックの登録関数
        public bool AddObject<T>(T obj) where T : class
        {
            //指定したlogicが登録済みかどうか
            if (objectList.Contains(obj)) return false;

            //未登録なら登録する
            objectList.Add(obj);
            return true;
        }


        //ロジックの取得関数
        public T GetObject<T>() where T : class
        {
            foreach (object obj in objectList)
            {
                if (obj is T) return (T)obj;
            }
            return null;
        }
    }



    //読み取り専用
    public interface IReadOnlyObjectContainer
    {
        public T GetObject<T>() where T : class;
    }
}

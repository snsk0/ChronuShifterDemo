using System.Collections.Generic;

namespace FSMUtils
{
    public class ObjectContainer : IReadOnlyObjectContainer
    {
        //���W�b�N���X�g
        private readonly List<object> objectList;


        //�R���X�g���N�^
        public ObjectContainer()
        {
            objectList = new List<object>();
        }



        //���W�b�N�̓o�^�֐�
        public bool AddObject<T>(T obj) where T : class
        {
            //�w�肵��logic���o�^�ς݂��ǂ���
            if (objectList.Contains(obj)) return false;

            //���o�^�Ȃ�o�^����
            objectList.Add(obj);
            return true;
        }


        //���W�b�N�̎擾�֐�
        public T GetObject<T>() where T : class
        {
            foreach (object obj in objectList)
            {
                if (obj is T) return (T)obj;
            }
            return null;
        }
    }



    //�ǂݎ���p
    public interface IReadOnlyObjectContainer
    {
        public T GetObject<T>() where T : class;
    }
}

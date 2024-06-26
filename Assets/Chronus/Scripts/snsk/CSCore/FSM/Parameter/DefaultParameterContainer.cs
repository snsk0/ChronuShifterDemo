using System.Collections.Generic;

namespace FSM.Parameter
{
    public class DefaultParameterContainer : IParameterContainer
    {
        private readonly Dictionary<string, object> valueList;

        //初期化
        public DefaultParameterContainer()
        {
            valueList = new Dictionary<string, object>();
        }

        //なかった場合エラーを返す
        public T GetParameter<T>(string name)
        {
            if (valueList.ContainsKey(name))
            {
                object value = valueList[name];
                if (value is T) return (T)value;
            }
            throw new System.Exception("ParameteContainerError: " + name);
        }

        public bool SetParameter<T>(string name, T parameter)
        {
            if (!valueList.ContainsKey(name))
            {
                valueList.Add(name, parameter);
            }
            else
            {
                valueList[name] = parameter;
            }
            return true;
        }
    }
}

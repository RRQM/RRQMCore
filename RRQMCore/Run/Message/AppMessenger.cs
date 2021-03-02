using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using RRQMCore.Exceptions;

namespace RRQMCore.Run
{
    /*
    若汝棋茗
    */
    /// <summary>
    /// 消息通知类
    /// </summary>
    public class AppMessenger
    {
        /// <summary>
        /// 注册已加载程序集中直接或间接继承自IMassage接口的所有类，并创建新实例
        /// </summary>
        public void RegistAll()
        {
            var types = AppDomain.CurrentDomain.GetAssemblies()
                            .SelectMany(a => a.GetTypes().Where(t => t.GetInterfaces().Contains(typeof(IMessage))))
                            .ToArray();
            foreach (var v in types)
            {
                IMessage  message = (IMessage)Activator.CreateInstance(v);
                MethodInfo[] methods = message.GetType().GetMethods();
                foreach (var item in methods)
                {
                    RegistMethodAttribute attribute = item.GetCustomAttribute<RegistMethodAttribute>();
                    if (attribute != null)
                    {
                        if (attribute.Token == null)
                        {
                            Default.Register(message, item.Name, item);
                        }
                        else
                        {
                            Default.Register(message, attribute.Token, item);
                        }
                    }
                }
            }
        }

       
        private static AppMessenger instance;

        /// <summary>
        /// 默认单例实例
        /// </summary>
        public static AppMessenger Default
        {
            get
            {
                if (instance == null)
                {
                    instance = new AppMessenger();
                }

                return instance;
            }
        }

        Dictionary<string, TokenInstance> tokenAndInstance = new Dictionary<string, TokenInstance>();

        /// <summary>
        /// 注册消息
        /// </summary>
        /// <param name="messageObject"></param>
        /// <param name="token"></param>
        /// <param name="action"></param>
        /// <exception cref="MessageRegisteredException"></exception>
        public void Register(IMessage messageObject, string token, Action action)
        {
            TokenInstance tokenInstance = new TokenInstance();
            tokenInstance.MessageObject = messageObject;
            tokenInstance.MethodInfo = action.Method;
            try
            {
                tokenAndInstance.Add(token, tokenInstance);
            }
            catch (Exception)
            {

                throw new MessageRegisteredException("该Token消息已注册");
            }

        }

        /// <summary>
        /// 注册消息
        /// </summary>
        /// <param name="messageObject"></param>
        public void Register(IMessage messageObject)
        {
            MethodInfo[] methods = messageObject.GetType().GetMethods();
            foreach (var item in methods)
            {
                RegistMethodAttribute attribute = item.GetCustomAttribute<RegistMethodAttribute>();
                if (attribute != null)
                {
                    if (attribute.Token == null)
                    {
                        Default.Register(messageObject, item.Name, item);
                    }
                    else
                    {
                        Default.Register(messageObject, attribute.Token, item);
                    }
                }
            }
        }

        /// <summary>
        /// 注册消息
        /// </summary>
        /// <param name="messageObject"></param>
        /// <param name="token"></param>
        /// <param name="methodInfo"></param>
        /// <exception cref="MessageRegisteredException"></exception>
        public void Register(IMessage messageObject, string token, MethodInfo methodInfo)
        {
            TokenInstance tokenInstance = new TokenInstance();
            tokenInstance.MessageObject = messageObject;
            tokenInstance.MethodInfo = methodInfo;
            try
            {
                tokenAndInstance.Add(token, tokenInstance);
            }
            catch (Exception)
            {
                throw new MessageRegisteredException("该Token消息已注册");
            }

        }

        /// <summary>
        /// 注册消息
        /// </summary>
        /// <typeparam name="T">参数类型</typeparam>
        /// <param name="messageObject"></param>
        /// <param name="token"></param>
        /// <param name="action"></param>
        /// <exception cref="MessageRegisteredException"></exception>
        public void Register<T>(IMessage messageObject, string token, Action<T> action)
        {
            TokenInstance tokenInstance = new TokenInstance();
            tokenInstance.MessageObject = messageObject;
            tokenInstance.MethodInfo = action.Method;
            try
            {
                tokenAndInstance.Add(token, tokenInstance);
            }
            catch (Exception)
            {
                throw new MessageRegisteredException("该Token消息已注册");
            }
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <typeparam name="T">参数类型</typeparam>
        /// <typeparam name="TReturn">返回值类型</typeparam>
        /// <param name="messageObject"></param>
        /// <param name="token"></param>
        /// <param name="action"></param>
        public void Register<T, TReturn>(IMessage messageObject, string token, Func<T, TReturn> action)
        {
            TokenInstance tokenInstance = new TokenInstance();
            tokenInstance.MessageObject = messageObject;
            tokenInstance.MethodInfo = action.Method;
            try
            {
                tokenAndInstance.Add(token, tokenInstance);
            }
            catch (Exception)
            {
                throw new MessageRegisteredException("该Token消息已注册");
            }
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <typeparam name="TReturn">返回值类型</typeparam>
        /// <param name="messageObject"></param>
        /// <param name="token"></param>
        /// <param name="action"></param>
        public void Register<TReturn>(IMessage messageObject, string token, Func<TReturn> action)
        {
            TokenInstance tokenInstance = new TokenInstance();
            tokenInstance.MessageObject = messageObject;
            tokenInstance.MethodInfo = action.Method;
            try
            {
                tokenAndInstance.Add(token, tokenInstance);
            }
            catch (Exception)
            {
                throw new MessageRegisteredException("该Token消息已注册");
            }
        }

        /// <summary>
        /// 卸载消息
        /// </summary>
        /// <param name="messageObject"></param>
        public void Unregister(IMessage messageObject )
        {
            List<string> key = new List<string>();

            foreach (var item in tokenAndInstance.Keys)
            {
                if (messageObject == tokenAndInstance[item].MessageObject)
                {
                    key.Add(item);
                }
            }

            foreach (var item in key)
            {
                tokenAndInstance.Remove(item);
            }
        }

        /// <summary>
        /// 清除所有消息
        /// </summary>
        public void Clear()
        {
            tokenAndInstance.Clear();
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="token"></param>
        /// <param name="parameters"></param>
        /// <exception cref="MessageNotFoundException"></exception>
        public void Send(string token, params object[] parameters)
        {
            try
            {
                tokenAndInstance[token].MethodInfo.Invoke(tokenAndInstance[token].MessageObject, parameters);
            }
            catch (KeyNotFoundException)
            {
                throw new MessageNotFoundException("未找到该消息");
            }

        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <typeparam name="T">返回值类型</typeparam>
        /// <param name="token"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        /// <exception cref="MessageNotFoundException"></exception>
        public T Send<T>(string token, params object[] parameters)
        {
            try
            {
                return (T)tokenAndInstance[token].MethodInfo.Invoke(tokenAndInstance[token].MessageObject, parameters);
            }
            catch (KeyNotFoundException)
            {
                throw new MessageNotFoundException("未找到该消息");
            }
        }
    }
}

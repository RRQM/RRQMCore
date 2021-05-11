using RRQMCore.ByteManager;
using RRQMCore.Data.Security;
using RRQMCore.Data.XML;
using RRQMCore.Helper;
using RRQMCore.Pool;
using RRQMCore.Run;
using System;
using System.Collections.Generic;

namespace RRQMCore.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ReadKey();
            TestBytePool();
            Console.ReadKey();
        }

        private void TestXml()
        {
            XmlTool xmlTool = new XmlTool("Test.xml");

            //储存单节点、单属性值
            xmlTool.AttributeStorage("Node1", "AttributeName", "AttributeValue");

            //储存单节点、多属性值
            string[] attributeNames = new string[] { "A1", "A2" };
            string[] attributeValues = new string[] { "V1", "V2" };
            xmlTool.AttributeStorage("Node2", attributeNames, attributeValues);

            //储存多节点、多属性
            string[] nades = new string[] { "N1", "N2" };
            xmlTool.AttributeStorage(nades, attributeNames, attributeValues);

            //判断Node1节点是否存在
            xmlTool.NodeExist("Node1");

            //获取Node1节点下，属性名为AttributeName的属性值。
            string attributeValue = xmlTool.SearchWords("Node1", "AttributeName");

            //获取Node2下所有属性集合，并包装为字典
            Dictionary<string, string> attributes = xmlTool.SearchAllAttributes("Node2");
        }
        private void Test3DES()
        {
            DataLock.EncryptDES(new byte[10], "RRQM1234");
            byte[] data = DataLock.DecryptDES(new byte[10], "RRQM1234");
        }
        private void TestAppMessenger()
        {
            //注册静态方法
            AppMessenger.Default.Register(null, "SayHelloOne", MyMessage.SayHelloOne);

            //注册实例单个方法
            MyMessage myMessage = new MyMessage();
            AppMessenger.Default.Register(myMessage, "SayHelloTwo", myMessage.SayHelloTwo);

            //注册实例中被RegistMethod标记的公共方法。
            AppMessenger.Default.Register(new MyMessage());

            //触发已注册的SayHelloOne方法
            AppMessenger.Default.Send("SayHelloOne", null);

            //触发已注册的SayHelloThree方法，传入string参数，返回string参数。
            string mes = AppMessenger.Default.Send<string>("SayHelloThree", "若汝棋茗");
        }
        private static void CreatWaitHandle()
        {
            RRQMWaitHandle<MyWaitResult> waitHandle = new RRQMWaitHandle<MyWaitResult>();
            WaitData<MyWaitResult> waitData = waitHandle.GetWaitData();
            waitData.Wait(10 * 1000);

            waitData.Set(new MyWaitResult());

            MyWaitResult myWaitResult = waitData.WaitResult;
        }
        private static void CreatObjectPool()
        {
            ObjectPool<MyObject> objectPool = new ObjectPool<MyObject>();
            MyObject myObject = objectPool.GetObject();
            objectPool.DestroyObject(myObject);
        }
        private static void TestBytePool()
        {
            BytePool bytePool = new BytePool(1024 * 1024 * 100, 1024 * 1024);

            TimeSpan timeSpan1 = RRQMCore.Diagnostics.TimeMeasurer.Run(() =>
              {
                  for (int i = 0; i < 100000; i++)
                  {
                      byte[] buffer = new byte[1024 * 1024];
                  }
              });

            TimeSpan timeSpan2 = RRQMCore.Diagnostics.TimeMeasurer.Run(() =>
              {
                  for (int i = 0; i < 100000; i++)
                  {
                      ByteBlock byteBlock = bytePool.GetByteBlock(1024 * 1024);
                      byteBlock.Dispose();
                  }
              });
            Console.WriteLine($"System:{timeSpan1}");
            Console.WriteLine($"RRQMCore:{timeSpan2}");

        }
        private static void CreatByteBlock()
        {
            BytePool bytePool = new BytePool(1024 * 1024 * 100, 1024 * 1024);

            //获取任意长度的空闲ByteBlock，如果没有空闲，则创建一个最大单元
            ByteBlock byteBlock1 = bytePool.GetByteBlock();

            //获取不小于64kb长度ByteBlock
            ByteBlock byteBlock2 = bytePool.GetByteBlock(1024 * 64);

            //获取64kb长度ByteBlock，且必须为64kb
            ByteBlock byteBlock3 = bytePool.GetByteBlock(1024 * 64, true);

            byteBlock1.Write(10);//写入byte
            byte[] buffer = new byte[1024];
            new Random().NextBytes(buffer);
            byteBlock1.Write(new byte[1024]);//写入byte[]

            byteBlock1.Dispose();
            byteBlock2.Dispose();
            byteBlock3.Dispose();//回收至内存池

            byteBlock1.SetHolding(true);
        }
    }
    public class MyMessage : IMessage
    {
        public static void SayHelloOne()
        {

        }

        public void SayHelloTwo()
        {

        }

        [RegistMethod]
        public string SayHelloThree(string name)
        {
            return "SayHelloThree";
        }
    }

    class MyWaitResult : WaitResult
    {

    }

    class MyObject : IPoolObject
    {
        public bool NewCreat { get; set; }

        public void Create()
        {

        }

        public void Destroy()
        {

        }

        public void Recreate()
        {

        }
    }
}

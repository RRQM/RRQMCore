//------------------------------------------------------------------------------
//  此代码版权归作者本人若汝棋茗所有
//  源代码使用协议遵循本仓库的开源协议及附加协议，若本仓库没有设置，则按MIT开源协议授权
//  CSDN博客：https://blog.csdn.net/qq_40374647
//  哔哩哔哩视频：https://space.bilibili.com/94253567
//  Gitee源代码仓库：https://gitee.com/RRQM_Home
//  Github源代码仓库：https://github.com/RRQM
//  交流QQ群：234762506
//  感谢您的下载和使用
//------------------------------------------------------------------------------
//------------------------------------------------------------------------------
using RRQMCore.ByteManager;
using RRQMCore.Data.Security;
using RRQMCore.Data.XML;
using RRQMCore.Diagnostics;
using RRQMCore.Helper;
using RRQMCore.IO;
using RRQMCore.Pool;
using RRQMCore.Run;
using RRQMCore.Serialization;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RRQMCore.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ReadKey();
            TestBytePoolPerformance_one();
           
            Console.ReadKey();
        }



        /// <summary>
        /// 测试内存池并发性能
        /// </summary>
        private static void TestBytePoolPerformance_one()
        {
            ThreadPool.SetMinThreads(100, 100);
            BytePool bytePool = new BytePool(1024 * 1024 * 1024, 1024 * 1024);

            for (int j = 0; j < 100; j++)//100并发
            {
                Task.Run(() =>
                {
                    TimeSpan timeSpan = TimeMeasurer.Run(() =>
                    {
                        for (int i = 0; i < 1000000; i++)//每次申请，销毁10w次
                        {
                            ByteBlock byteBlock = bytePool.GetByteBlock(1024);
                            byteBlock.Dispose();
                        }
                    });
                    Console.WriteLine(timeSpan);
                });
            }

            Console.WriteLine("回车测试重复元素");
            Console.ReadKey();
            Dictionary<ByteBlock, string> pairs = new Dictionary<ByteBlock, string>();
            foreach (var item in bytePool)
            {
                pairs.Add(item, null);
            }
            Console.WriteLine("无重复元素");
        }

        /// <summary>
        /// 测试延迟销毁
        /// </summary>
        private static void TestBytePoolPerformance_two()
        {
            ThreadPool.SetMinThreads(100, 100);
            BytePool bytePool = new BytePool(1024 * 1024 * 1000, 1024 * 1024);
            List<ByteBlock> byteBlocks = new List<ByteBlock>();


            Task.Run(() =>
            {
                while (true)
                {
                    int len = byteBlocks.Count;
                    for (int i = 0; i < len; i++)
                    {
                        if (!byteBlocks[i].Using)
                        {
                            Console.WriteLine("发生异常");
                        }
                        else
                        {
                            byteBlocks[i].Dispose();
                        }
                    }
                    lock (typeof(Program))
                    {
                        byteBlocks.RemoveRange(0, len);
                        Console.WriteLine(len);
                    }
                    Thread.Sleep(10);
                }
            });

            for (int n = 0; n < 100; n++)
            {
                Task.Run(() =>
                {
                    for (int j = 0; j < 1000; j++)
                    {
                        for (int i = 0; i < 10; i++)
                        {
                            ByteBlock byteBlock = bytePool.GetByteBlock(1024 * 64);
                            lock (typeof(Program))
                            {
                                byteBlocks.Add(byteBlock);
                            }

                        }

                        Thread.Sleep(10);
                    }

                });
            }


        }

        private static void TestSerializePerformance()
        {
            Student student = new Student();
            student.P1 = 10;
            student.P2 = "若汝棋茗";
            student.P3 = 100;
            student.P4 = 0;
            student.P5 = DateTime.Now;
            student.P6 = 10;
            student.P7 = new byte[1024 * 64];

            Random random = new Random();
            random.NextBytes(student.P7);

            student.List1 = new List<int>();
            student.List1.Add(1);
            student.List1.Add(2);
            student.List1.Add(3);

            student.List2 = new List<string>();
            student.List2.Add("1");
            student.List2.Add("2");
            student.List2.Add("3");

            student.List3 = new List<byte[]>();
            student.List3.Add(new byte[1024]);
            student.List3.Add(new byte[1024]);
            student.List3.Add(new byte[1024]);

            student.Dic1 = new Dictionary<int, int>();
            student.Dic1.Add(1, 1);
            student.Dic1.Add(2, 2);
            student.Dic1.Add(3, 3);

            student.Dic2 = new Dictionary<int, string>();
            student.Dic2.Add(1, "1");
            student.Dic2.Add(2, "2");
            student.Dic2.Add(3, "3");

            student.Dic3 = new Dictionary<string, string>();
            student.Dic3.Add("1", "1");
            student.Dic3.Add("2", "2");
            student.Dic3.Add("3", "3");

            student.Dic4 = new Dictionary<int, Arg>();
            student.Dic4.Add(1, new Arg(1));
            student.Dic4.Add(2, new Arg(2));
            student.Dic4.Add(3, new Arg(3));

            BytePool bytePool = new BytePool(1024 * 1024 * 10, 102400);

            TimeSpan timeSpan1 = TimeMeasurer.Run(() =>
            {
                for (int i = 0; i < 100000; i++)
                {
                    ByteBlock byteBlock = bytePool.GetByteBlock(1024 * 100);
                    SerializeConvert.RRQMBinarySerialize(byteBlock, student, true);
                    Student student1 = SerializeConvert.RRQMBinaryDeserialize<Student>(byteBlock.Buffer, 0);
                    byteBlock.Dispose();
                    if (i % 1000 == 0)
                    {
                        Console.WriteLine(i);
                    }
                }
            });

            TimeSpan timeSpan2 = TimeMeasurer.Run(() =>
            {
                for (int i = 0; i < 100000; i++)
                {
                    ByteBlock byteBlock = bytePool.GetByteBlock(1024 * 100);
                    SerializeConvert.BinarySerialize(byteBlock, student);
                    byteBlock.Position = 0;
                    Student student1 = SerializeConvert.BinaryDeserialize<Student>(byteBlock);
                    byteBlock.Dispose();
                    if (i % 1000 == 0)
                    {
                        Console.WriteLine(i);
                    }
                }
            });

            Console.WriteLine($"RRQM:{timeSpan1}");
            Console.WriteLine($"System:{timeSpan2}");
        }
        private void TestSerialize()
        {
            string obj = "RRQM";

            //用系统二进制序列化、反序列化
            byte[] data1 = SerializeConvert.BinarySerialize(obj);
            string newObj = SerializeConvert.BinaryDeserialize<string>(data1);

            //用系统二进制序列化至文件、反序列化
            SerializeConvert.BinarySerializeToFile(obj, "C:/1.txt");
            string newFileObj = SerializeConvert.BinaryDeserializeFromFile<string>("C:/1.txt");

            //用RRQM二进制序列化、反序列化
            byte[] data2 = SerializeConvert.RRQMBinarySerialize(obj, true);
            string newRRQMObj = SerializeConvert.RRQMBinaryDeserialize<string>(data2, 0);

            //用Xml序列化、反序列化
            byte[] data3 = SerializeConvert.XmlSerializeToBytes(obj);
            string newXmlObj = SerializeConvert.XmlDeserializeFromBytes<string>(data3);
        }
        private void TestFileControler()
        {
            //判断该文件时候已在打开状态
            FileControler.FileIsOpen("C:/1.txt");

            //获取文件SHA256值，转为大写16进制
            FileControler.GetFileHash("C:/1.txt");
        }
        private void TestTimeRun()
        {
            Action action = new Action(() => { Console.WriteLine("Hello"); });
            TimeRun.Run(action, 2);//延迟两秒执行
            TimeRun.Run(action, TimeSpan.FromSeconds(2));//延迟两秒执行
        }
        private void TestTimeMeasurer()
        {
            TimeSpan timeSpan = TimeMeasurer.Run(() =>
              {
                  for (int i = 0; i < 1000; i++)
                  {
                      Console.WriteLine(i);
                  }
              });

            Console.WriteLine(timeSpan);
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

            //获取不小于64kb长度ByteBlock
            ByteBlock byteBlock1 = bytePool.GetByteBlock(1024 * 64);

            //获取64kb长度ByteBlock，且必须为64kb
            ByteBlock byteBlock2 = bytePool.GetByteBlock(1024 * 64, true);

            byteBlock1.Write(10);//写入byte
            byte[] buffer = new byte[1024];
            new Random().NextBytes(buffer);
            byteBlock1.Write(new byte[1024]);//写入byte[]

            byteBlock1.Dispose();
            byteBlock2.Dispose();//回收至内存池

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
        public bool NewCreate { get; set; }

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

    [Serializable]
    public class Student
    {
        public int P1 { get; set; }
        public string P2 { get; set; }
        public long P3 { get; set; }
        public byte P4 { get; set; }
        public DateTime P5 { get; set; }
        public double P6 { get; set; }
        public byte[] P7 { get; set; }

        public List<int> List1 { get; set; }
        public List<string> List2 { get; set; }
        public List<byte[]> List3 { get; set; }

        public Dictionary<int, int> Dic1 { get; set; }
        public Dictionary<int, string> Dic2 { get; set; }
        public Dictionary<string, string> Dic3 { get; set; }
        public Dictionary<int, Arg> Dic4 { get; set; }
    }
    [Serializable]
    public class Arg
    {
        public Arg(int myProperty)
        {
            this.MyProperty = myProperty;
        }

        public Arg()
        {
            Person person = new Person();
            person.Name = "张三";
            person.Age = 18;
        }

        public int MyProperty { get; set; }
    }
    [Serializable]
    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
}

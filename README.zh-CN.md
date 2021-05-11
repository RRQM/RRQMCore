<link rel="stylesheet" href="https://use.fontawesome.com/releases/v5.0.13/css/all.css">
<p></p>
<p></p>
<p align="center">
<img src="https://img-blog.csdnimg.cn/20210406140816743.png" width = "100" height = "100" alt="图片名称" align=center />
</p>

 <div align="center"> 
  
[![NuGet version (RRQMCore)](https://img.shields.io/nuget/v/RRQMCore.svg?style=flat-square)](https://www.nuget.org/packages/RRQMCore/)
[![License](https://img.shields.io/badge/license-Apache%202-4EB1BA.svg)](https://www.apache.org/licenses/LICENSE-2.0.html)
[![Download](https://img.shields.io/nuget/dt/RRQMCore)](https://www.nuget.org/packages/RRQMCore/)

</div> 

<div align="center">

日月之行，若出其中；星汉灿烂，若出其里。

</div>
<div align="center">

**简体中文 | [English](./README.md)**

</div>

## 💿描述
&emsp;&emsp;RRQMCore是为RRQM系提供基础服务功能的库，其中包含：**内存池**、**对象池**、**等待逻辑池**、**AppMessenger**、**3DES加密**、**Xml快速存储**、**运行时间测量器**、**文件快捷操作**、**高性能序列化器**、**规范日志接口**等。

## 🖥支持环境
- .NET Framework4.5及以上。
- .NET Core3.1及以上。
- .NET Standard2.0及以上。

## 🥪支持框架
- WPF
- Winform
- Blazor
- Xamarin
- Mono
- Unity
- 其他（即所有C#系）

## 🔗联系作者

 - [CSDN博客主页](https://blog.csdn.net/qq_40374647)
 - [哔哩哔哩视频](https://space.bilibili.com/94253567)
 - [源代码仓库主页](https://gitee.com/RRQM_Home) 
 - 交流QQ群：234762506


 
## 📦 安装

- [Nuget RRQMCore](https://www.nuget.org/packages/RRQMCore/)
- [微软Nuget安装教程](https://docs.microsoft.com/zh-cn/nuget/quickstart/install-and-use-a-package-in-visual-studio)

## 内存池（BytePool）

内存池的基本单元是内存块（ByteBlock），内存块是继承自Stream的实际内存，它具有和MemoryStream一样的功能和效率，同时也具备Byte数组的灵活，最重要的是可回收，可扩展。

#### 创建、使用、回收

``` CSharp
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

```
#### 高级功能

**1.长期持有**

使Dispose失效，在使用完成后，再次调用传false即可回收（结束后不可再调Dispose，避免重复释放）。
``` CSharp
byteBlock.SetHolding(true);
```
**2.操作Byte数组**

Buffer属性为Byte[]类型，可以直接参与Byte[]运算。
``` CSharp
byteBlock.Buffer
```
#### 性能对比
**获取10w次64kb内存**
![](https://i.loli.net/2021/05/11/ZnP17IhUpikYTjA.jpg)

**获取10w次1Mb内存**
![](https://i.loli.net/2021/05/11/9I2QydTxWi4MKpo.jpg)

## 对象池
对象池是为避免重复创建、销毁对象而设立的。

#### 创建、使用对象池
``` CSharp
ObjectPool<MyObject> objectPool = new ObjectPool<MyObject>();
MyObject myObject= objectPool.GetObject();
objectPool.DestroyObject(myObject);
```

``` CSharp
class MyObject : IPoolObject
{
    public bool NewCreat { get; set ; }

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
```
##  等待逻辑池

等待逻辑池是封装EventWaitHandle后的池产物，以达到多线程等待返回结果的目的。

``` CSharp
RRQMWaitHandle<MyWaitResult> waitHandle = new RRQMWaitHandle<MyWaitResult>();
WaitData<MyWaitResult> waitData = waitHandle.GetWaitData();
waitData.Wait(10*1000);

waitData.Set(new MyWaitResult());

MyWaitResult myWaitResult = waitData.WaitResult;
```

``` CSharp
class MyWaitResult : WaitResult
{

}
```

## AppMessenger
AppMessenger是在App域内能够传递消息、调用方法的信使。
``` CSharp
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

```
``` CSharp
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

```

## 3DES加密

``` CSharp
DataLock.EncryptDES(new byte[10], "RRQM1234");
byte[] data = DataLock.DecryptDES(new byte[10], "RRQM1234");
```

## Xml快速存取
``` CSharp
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
```

## 时间控制

#### 运行时间测量器

TimeMeasurer是对Stopwatch的封装，方便调用测试代码片运行时间。

``` CSharp
TimeSpan timeSpan = TimeMeasurer.Run(() =>
  {
      for (int i = 0; i < 1000; i++)
      {
          Console.WriteLine(i);
      }
  });

Console.WriteLine(timeSpan);
```
#### 延迟时间运行使

TimeRun类可以延迟执行委托。

``` CSharp
Action action = new Action(()=> { Console.WriteLine("Hello"); });
TimeRun.Run(action,2);//延迟两秒执行
TimeRun.Run(action,TimeSpan.FromSeconds(2));//延迟两秒执行
```

## 文件快捷操作

``` CSharp
 //判断该文件时候已在打开状态
 FileControler.FileIsOpen("C:/1.txt");

 //获取文件SHA256值，转为大写16进制
 FileControler.GetFileHash("C:/1.txt");

```

## 序列化器

#### 高性能序列化器（RRQMBinaryFormatter）

RRQMBinaryFormatter类用法基本和BinaryFormatter一致。

``` CSharp
RRQMBinaryFormatter bf = new RRQMBinaryFormatter();
bf.Serialize(stream, new object(), true);//布尔值标识是否保存属性名。

```

#### 简易序列化操作

在RRQMCore中已经封装了简单操作的序列化、反序列化类，具体操作如下：

``` CSharp
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

```

#### 性能对比

**待测试类**

``` CSharp
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

```

**测试代码**

``` CSharp
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
        SerializeConvert.RRQMBinarySerialize(byteBlock, student,true);
        Student student1 = SerializeConvert.RRQMBinaryDeserialize<Student>(byteBlock.Buffer,
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

```

**测试结果**

![](https://i.loli.net/2021/05/12/1rTf6QOo2sC8KmM.jpg)


## 致谢

谢谢大家对我的支持，如果还有其他问题，请加群QQ：234762506讨论。


## 💕 支持本项目
您的支持就是我不懈努力的动力。打赏时请一定留下您的称呼。

 **赞助总金额:366.6￥** 

**赞助名单：** 

（以下排名只按照打赏时间顺序）

> 1.Bobo Joker

> 2.UnitySir

> 3.Coffee

<img src="https://images.gitee.com/uploads/images/2021/0330/234046_7662fb8c_8553710.png" width = "600" height = "400" alt="图片名称" align=center />




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

**English | [简体中文](./README.zh-CN.md)**

</div>

## 💿Description
&emsp;&emsp;Rrqmcore is a library that provides the basic service function for the RRQM, including: **memory pool**, **object pool**, **waiting logic pool**, **AppMessenger**, **3DES encryption**, **XML Quick Storage**, **Run Time Meter**, **File Shortcut Operation**, **High Performance Series**, **Specifies Log Interface**, etc.。

## 🖥Support environment
- .NET Framework4.5 and above.
- .NET Core3.1 and above.
- .net Standard 2.0 and above.

## 🥪Support framework
- WPF
- Winform
- Blazor
- Xamarin
- Mono
- Unity
- Others (ie all C # pedics)

## 🔗Contact the author

 - [CSDN Blog Home] (https://blog.csdn.net/qq_40374647)
 - [哩 哩 视频 视频] (https://space.bilibili.com/94253567)
 - [Source Code Warehouse Home] (https://gitee.com/rrqm_home)
 - Communication QQ group: 234762506

 
## 📦 Installation

- [NUGET RRQMCORE] (https://www.nuget.org/packages/rzmcore/)
- [Microsoft NuGet Installation Tutorial] (https://docs.microsoft.com/enstall-/uickstart/install-and-use-a-package-in-visual-studio)

## Memory pool（BytePool）

The basic unit of the memory pool is a ByteBlock. The memory block is inherited from the actual memory of Stream, which has the same function and efficiency as MemoryStream, and also has the flexibility of the Byte array. It is recyclable and scalable.

#### Create, use, recycle

``` CSharp
BytePool bytePool = new BytePool(1024 * 1024 * 100, 1024 * 1024);

// Get an idle Byteblock for any length, if not idle, create a maximum unit
ByteBlock byteBlock1 = bytePool.GetByteBlock();

/ / Get not less than 64kb length byteblock
ByteBlock byteBlock2 = bytePool.GetByteBlock(1024 * 64);

/ / Get 64KB length byteblock, and must be 64KB
ByteBlock byteBlock3 = bytePool.GetByteBlock(1024 * 64, true);

byteBlock1.Write(10);// Write to BYTE
byte[] buffer = new byte[1024];
new Random().NextBytes(buffer);
byteBlock1.Write(new byte[1024]);// Write Byte []

byteBlock1.Dispose();
byteBlock2.Dispose();
byteBlock3.Dispose();// Recycle to the memory pool

```
#### Advanced Features

**Long-term hold**

Make the Dispose, after use, call the pass FALSE to reclaim again (the end will not be again adjusted to avoid repeated release).
``` CSharp
byteBlock.SetHolding(true);
```
**Operation Byte array**

Buffer attribute is a BYTE [] type, which can be directly involved in the Byte [] operation.
``` CSharp
byteBlock.Buffer
```
#### Performance comparison
**Get 10W 64KB memory**
![](https://i.loli.net/2021/05/11/ZnP17IhUpikYTjA.jpg)

**Get 10W 1MB of memory**
![](https://i.loli.net/2021/05/11/9I2QydTxWi4MKpo.jpg)

## Object pool
The object pool is set to avoid repeated creation, destroy objects.

#### Create, use the object pool
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
##  Waiting logic pool

Waiting logic pool is a pool product after encapsulating Eventwaithandle to achieve a multi-thread wait for the purpose of returning results.

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
AppMessenger is a messenger that is capable of passing messages and calling methods within the App field.
``` CSharp
//Register static method
AppMessenger.Default.Register(null, "SayHelloOne", MyMessage.SayHelloOne);

//Registered instance single method
MyMessage myMessage = new MyMessage();
AppMessenger.Default.Register(myMessage, "SayHelloTwo", myMessage.SayHelloTwo);

//A public method that is marked by the RegistMethod in the registration instance.
AppMessenger.Default.Register(new MyMessage());

//Trigger the registered SayHelloone method
AppMessenger.Default.Send("SayHelloOne", null);

//Trigger the registered SayHellothree method, incurring the String parameter, returns the String parameter.
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

## 3DES encryption

``` CSharp
DataLock.EncryptDES(new byte[10], "RRQM1234");
byte[] data = DataLock.DecryptDES(new byte[10], "RRQM1234");
```

## XML Quick Access

``` CSharp
XmlTool xmlTool = new XmlTool("Test.xml");

//Storage single node, single attribute value
xmlTool.AttributeStorage("Node1", "AttributeName", "AttributeValue");

//Storage single node, multi-attribute value
string[] attributeNames = new string[] { "A1", "A2" };
string[] attributeValues = new string[] { "V1", "V2" };
xmlTool.AttributeStorage("Node2", attributeNames, attributeValues);

//Store multiple nodes, multi-attributes
string[] nades = new string[] { "N1", "N2" };
xmlTool.AttributeStorage(nades, attributeNames, attributeValues);

//Determine if the Node1 node exists
xmlTool.NodeExist("Node1");

//Get the property value of AttributeName under the Node1 node.
string attributeValue = xmlTool.SearchWords("Node1", "AttributeName");

//Get all properties collection under Node2 and packaged as a dictionary
Dictionary<string, string> attributes = xmlTool.SearchAllAttributes("Node2");
```

## Time control

#### Running time measuring device

TimeMeasurer is a package for StopWatch for easy calling test code.

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
#### Delay time operation

The TimerUn class can be delayed executing delegation.

``` CSharp
Action action = new Action(()=> { Console.WriteLine("Hello"); });
TimeRun.Run(action,2);//Delayed two seconds
TimeRun.Run(action,TimeSpan.FromSeconds(2));//Delayed two seconds
```

## File shortcut

``` CSharp
 //Judging the file is already open
 FileControler.FileIsOpen("C:/1.txt");

 //Get file SHA256 values, turn to uppercase 16
 FileControler.GetFileHash("C:/1.txt");

```

## Serializer

#### High performance sequencer（RRQMBinaryFormatter）

RrqmbinaryFormatter class is basically uniform and binaryformatter.

``` CSharp
RRQMBinaryFormatter bf = new RRQMBinaryFormatter();
bf.Serialize(stream, new object(), true);//The Boolean value identifies if the property name is saved.

```

#### Simple sequential operation

A simple operation of the simple operation has been encapsulated in RrqMcore, and the specific operation is as follows:

``` CSharp
string obj = "RRQM";

//Use system binary serialization, reverse sequence
byte[] data1 = SerializeConvert.BinarySerialize(obj);
string newObj = SerializeConvert.BinaryDeserialize<string>(data1);

//Use system binary sequence to file, anti-sequence
SerializeConvert.BinarySerializeToFile(obj, "C:/1.txt");
string newFileObj = SerializeConvert.BinaryDeserializeFromFile<string>("C:/1.txt");

//Using RRQM binary serialization, reverse sequence
byte[] data2 = SerializeConvert.RRQMBinarySerialize(obj, true);
string newRRQMObj = SerializeConvert.RRQMBinaryDeserialize<string>(data2, 0);

//Serialized, reverse sequencement with XML
byte[] data3 = SerializeConvert.XmlSerializeToBytes(obj);
string newXmlObj = SerializeConvert.XmlDeserializeFromBytes<string>(data3);

```

#### Performance comparison

**Test class**

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

**Test code**

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

**Test Results**

![](https://i.loli.net/2021/05/12/1rTf6QOo2sC8KmM.jpg)


## Thank you

Thank you for your support, if there are other problems, please add group QQ: 234762506.


## 💕 Support this project
Your support is the driving force for my unremitting efforts. Please leave your name when you reward.

 **Sponsorship total amount: 366.6 ¥** 

**Sponsored list:** 

(The following ranking is only in the order of rewards)

> 1.Bobo Joker

> 2.UnitySir

> 3.Coffee

<img src="https://images.gitee.com/uploads/images/2021/0330/234046_7662fb8c_8553710.png" width = "600" height = "400" alt="图片名称" align=center />




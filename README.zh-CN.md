<head> 
    <script defer src="https://use.fontawesome.com/releases/v5.0.13/js/all.js"></script> 
    <script defer src="https://use.fontawesome.com/releases/v5.0.13/js/v4-shims.js"></script> 
</head> 
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




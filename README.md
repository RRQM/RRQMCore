## 前言
[RRQMCore](https://gitee.com/RRQM_Home/RRQMCore)是一个综合性程序集库，也是RRQM系列的基库，其内容完全开源，并且已发布至Gitee。因为其包含内容比较多，所以此处只做功能展示。

## Nuget引用
RRQMCore已经发布到Nuget网站，欢迎各位童鞋下载使用。
![在这里插入图片描述](https://img-blog.csdnimg.cn/20210205162104697.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L3FxXzQwMzc0NjQ3,size_16,color_FFFFFF,t_70)
![在这里插入图片描述](https://img-blog.csdnimg.cn/20210205162203864.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L3FxXzQwMzc0NjQ3,size_16,color_FFFFFF,t_70)

## 内存池（BytePool）
内存池是能够重复利用内存的管理中心，其中包含了内存的申请，回收，销毁等。
**主要属性：**
MaxSize：内存池最大值，当内存池到达最大时，继续申请的内存将不再进入池中，使用完成后由系统自行销毁。
MaxBlockSize：内存块最大值，当申请的内存大于该值时，申请的内存将不再进入池中，使用完成后由系统自行销毁。
**主要方法：**
GetByteBlock(long byteSize, bool equalSize):获取长度为byteSize的内存块，当equalSize为false时，获取到的内存块为最小值，即有可能获取到比byteSize大的内存块。

#### 内存块（ByteBlock）
内存块是内存池的基本组成单元，只能由内存池创建其实例，其继承自Stream，所以可以直接参与流操作，在使用完后调用Dispose会自动回收到内存池或自行销毁。
**主要属性：**
Buffer：内存块实际Byte[]，在调用Write方法时有可能会因为内存不足而扩建，从而导致原地址失效。
Using：标志该实例是否在使用当中。

**主要方法：**
ToArray():将内存流从0开始到Position位置的元数据复制导出。
Dispose()：将内存块回收或自行销毁。

## 线程安全的List（ConcurrentList）
该List在Add，Remove，Index模式下是线程安全的，和System.Collections.Concurrent命名空间下的安全集合不同的是，ConcurrentList具有和List一样的功能，但是线程安全所带来的问题就是性能问题，具体的细节在这里就不叙述了，大家可以下载来看看。

## 3DES加密（DataLock）
DataLock是一个静态类，类中提供了EncryptDES和DecryptDES方法，能够快速的对Byte[]加密或解密。

## Xml快速存储（XmlTool）
我们有时候想用xml存储或读取数据，尤其是在多节点，多属性中，如果每次只读一个节点属性，那磁盘必定会频繁的读取，所以XmlTool提供整合性属性，能够将多个节点，属性，属性值一次性写入或读取。

## 时间测量器（TimeMeasurer）
有时候我们在测试性能时需要准确得知某个代码块所耗时间，所以TimeMeasurer可以完成这项工作，其工作原理是用Stopwatch测量，然后返回TimeSpan，但经过封装后使用起来比较方便。

## 文件操作（FileControler）
FileControler可以帮助快速的获取文件Hash值，或者任何Stream的Hash。不仅如此，FileControler还能判断某个文件是否正在被使用，从而避免删除、剪切、读取等异常。
**主要方法：**
FileIsOpen(string fileFullName):判断该路径下的文件目前是否处于被打开状态。

## 消息管理类（AppMessenger）

AppMessenger是致力于同一程序域中的不同模块由于依赖反转而不能调用的问题。

## 时间使者（TimeRun）
TimeRun封装了异步等待某个时间再执行的方法，又名定时执行。

## 等待逻辑池（RRQMWaitHandle）
RRQMWaitHandle是封装了EventWaitHandle的池话产物，可以实现获得一个闲置句柄，然后等待，当收到set信号后继续执行并返回等待结果。

#### 等待数据对象（WaitData）
WaitData是RRQMWaitHandle的基本单元，可以单独设置Wait或Set以及WaitResult。

## 高性能序列化器（SerializeConvert）
SerializeConvert封装了二进制，xml的基本序列化方法，其中使用静态方法时，均为常规序列化方法，只有创建实例后才可以使用高性能序列化方法。

当然如果还有什么不明白的，可以私信本猿哦，QQ：505554090，QQ群：234762506








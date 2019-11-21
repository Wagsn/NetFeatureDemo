# 序列化器

核心思想：序列化与反序列化

采用各种成熟的第三方解析库组装成通用API以供简便使用，部分简单的可以自己实现解析器。

## 用途：

- 持久化存储配置文件 Config
- 网络之间传输数据 DTO
- 不同编程语言之间交换数据

## 已有的相关语言

- CSV(Comma-Separated Values): 以纯文本的形式存储表格数据
- INI(Initialization File, 初始化文件): 是windows的系统配置文件所采用的存储格式, 后缀名：[.ini]
- [x] JSON(JavaScript Object Notation): (用于WebAPI传输和配置) 后缀名：[.json] 依赖于 [Newtonsoft.Json](https://github.com/JamesNK/Newtonsoft.Json)
- Properties: (用户配置)
- Protobuf: (用户WebAPI传输) 后缀名：[.proto]
- TOML:
- XML:
- YAML: (Python, Perl, Ruby; 用于持久化和配置) 后缀名：[.yml, .yaml]

## 序列化器与工厂

ISerializer(序列化器)主要方法为：
```
Serialize(entity: Object): String # 序列化 内存中
Deserialize<TypeEntity>(content: String): TypeEntity # 反序列化 内存中
```
工厂通过语言名称获取序列化器
```txt
factory.GetByName("YAML"): ISerializer
```
可配置，如：
- 序列化模式：压缩与格式化（某些强格式类型不支持）

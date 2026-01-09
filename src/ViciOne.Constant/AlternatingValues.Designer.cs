using System;
using ViciOne.Core.Contracts.DataModel;
using ViciOne.Core.Contracts.Runtime;
using ViciOne.Core.Runtime;
using ViciOne.Core.Runtime.Attributes;

namespace ViciOne.Constant;

[FunctionBlockDesign("152ec7e7-c6a6-4eca-a767-9ab0393d1c54", "f885a8ea-d26d-4c69-8045-3ac031ae9e19", "2c0779ba-f3e0-43af-bdfb-3e13dd917b57")]
[DefaultRunMode(RunMode.Cyclic)]
[ValidRunModes(RunMode.Cyclic)]
public partial class AlternatingValues : RuntimeFunctionBlock<AlternatingValues>
{
    public new Outputs_ ProcessDataOutputs { get; } = new Outputs_();
    [OutputConnectorDesigns]
    public class Outputs_
    {
        [OutputConnectorDesign("a44ab3a7-b3db-4ee2-8d7e-711c6dd1a9b3")]
        public IRuntimeConnector<bool> BoolValue { get; internal set; }
        [OutputConnectorDesign("ee09df09-8049-46d4-b0ec-176e7a34cc42")]
        public IRuntimeConnector<double> DoubleValue { get; internal set; }
        [OutputConnectorDesign("7ac29425-ef58-49d3-95b1-2019e5be8cf3")]
        public IRuntimeConnector<byte> ByteValue { get; internal set; }
        [OutputConnectorDesign("4ab01433-3a8d-4135-85b4-5a7dcfce6f65")]
        public IRuntimeConnector<sbyte> SByteValue { get; internal set; }
        [OutputConnectorDesign("46850746-1670-488b-aadb-e370d297730c")]
        public IRuntimeConnector<char> CharValue { get; internal set; }
        [OutputConnectorDesign("5b6bd25a-2380-4702-966f-b74c1f57daca")]
        public IRuntimeConnector<short> ShortValue { get; internal set; }
        [OutputConnectorDesign("daad6464-c522-4274-8844-12ede158e356")]
        public IRuntimeConnector<ushort> UShortValue { get; internal set; }
        [OutputConnectorDesign("dc59f671-b641-44c7-8aaa-70fe1311eb59")]
        public IRuntimeConnector<int> IntValue { get; internal set; }
        [OutputConnectorDesign("399d013b-56c4-4170-97fb-ca213234d0fa")]
        public IRuntimeConnector<uint> UIntValue { get; internal set; }
        [OutputConnectorDesign("10b540d8-e6be-4e5b-9512-22a38769ea5c")]
        public IRuntimeConnector<long> LongValue { get; internal set; }
        [OutputConnectorDesign("39f1d69a-11ff-4ead-94de-78068614c29d")]
        public IRuntimeConnector<ulong> ULongValue { get; internal set; }
        [OutputConnectorDesign("e86c530a-471c-4e95-a8e6-0af3b95041d6")]
        public IRuntimeConnector<float> FloatValue { get; internal set; }
        [OutputConnectorDesign("370b51b9-796c-4b6d-bc49-35607998a523")]
        public IRuntimeConnector<decimal> DecimalValue { get; internal set; }
        [OutputConnectorDesign("9f1a206a-f473-4424-980d-ccfb805beae6")]
        public IRuntimeConnector<string> StringValue { get; internal set; }
        [OutputConnectorDesign("909b7f4a-9b8c-4bbc-b346-cdf81aaad006")]
        public IRuntimeConnector<DateTime> DateTimeValue { get; internal set; }
        [OutputConnectorDesign("1fecf84c-f13a-42b6-8584-3655067a588c")]
        public IRuntimeConnector<TimeSpan> TimeSpanValue { get; internal set; }
    }
    
    public new Settings_ Settings { get; } = new Settings_();
    [SettingDesigns]
    public class Settings_
    {
        [SettingDesign("8a9e4007-6dc0-4183-b8bc-a70984455134", "3ab65ce1-4a5a-4c93-a922-09e3408ed767", "cb5f03c0-71c3-4a8d-a79f-20e6afae4a06")]
        [DefaultValue(true)]
        public IRuntimeSetting<bool> BoolValueOne { get; internal set; }
        [SettingDesign("6ac51990-8a0a-49ba-8084-71c4d8fbaf3d", "c294487c-cd9e-47b5-a45d-2b77bfcf49d9", "b1a49f73-0903-4ce7-ad6a-2e8564d9f1a9")]
        [DefaultValue(false)]
        public IRuntimeSetting<bool> BoolValueTwo { get; internal set; }
        
        [SettingDesign("355141a5-97e6-4ad5-9595-5dea73e4858f", "b5f43040-efe2-4703-97ff-38010b0f937e", "17ab87c1-49ae-4b01-b03e-d124d2cae379")]
        [DefaultValue(100.0)]
        public IRuntimeSetting<double> DoubleValueOne { get; internal set; }
        [SettingDesign("353559ab-92ae-4b53-b025-a094f03b09cf", "9f2833dd-5c8a-44b4-a3b0-006ec927dccc", "d7b0e26a-525d-4d8f-9c30-9b88985a8608")]
        [DefaultValue(200.0)]
        public IRuntimeSetting<double> DoubleValueTwo { get; internal set; }
        
        [SettingDesign("ac46df4a-50fe-41c5-87ea-4975467b2bac", "13ee622b-1da6-40d1-a1f4-1c0dd5913f5c", "8aba7431-ed5c-462a-b493-d378353974a4")]
        [DefaultValue((byte)10)]
        public IRuntimeSetting<byte> ByteValueOne { get; internal set; }
        [SettingDesign("aeb314b4-dee3-41e5-9488-3080e9914fb2", "4e84d5c9-fa68-43bd-ace2-872dc5b32134", "05f34a65-9975-4829-b7b7-74a077cb6f60")]
        [DefaultValue((byte)20)]
        public IRuntimeSetting<byte> ByteValueTwo { get; internal set; }
        
        [SettingDesign("c3154ae0-16ed-4fd7-950b-6fb2bd622e61", "c65903d0-c4d9-4af9-9508-39a2ac1839b5", "8adfd624-dc1c-4b49-9826-fba65c56d524")]
        [DefaultValue((sbyte)-5)]
        public IRuntimeSetting<sbyte> SByteValueOne { get; internal set; }
        [SettingDesign("85e3078b-06ab-4b89-a9be-6706fa546aeb", "62ed4aa7-818b-484a-9197-f6aa5f7d0f32", "b7af1cc7-fcb1-46c1-88a5-fb2b3458613a")]
        [DefaultValue((sbyte)5)]
        public IRuntimeSetting<sbyte> SByteValueTwo { get; internal set; }
        
        [SettingDesign("7462f41b-0f93-4bb6-9573-f0fd25916938", "b3885b7d-72c1-4519-ba98-f8c605ded479", "2a88385a-9af7-4fbf-b4b3-f097145211fc")]
        [DefaultValue('A')]
        public IRuntimeSetting<char> CharValueOne { get; internal set; }
        [SettingDesign("3542a3ea-5703-4eb1-bd9d-27ed39107721", "f8aee17a-b3d0-4172-80f6-6241e9896c09", "d401b6a0-340f-417c-8ac1-098b008837c6")]
        [DefaultValue('B')]
        public IRuntimeSetting<char> CharValueTwo { get; internal set; }
        
        [SettingDesign("35b23b03-35c4-4641-b0d7-56150ee83a4e", "4719e269-f7b1-47c0-8905-00113c5f69d4", "92fbb55c-cfb8-4194-b29d-fff57c183c5f")]
        [DefaultValue((short)1000)]
        public IRuntimeSetting<short> ShortValueOne { get; internal set; }
        
        [SettingDesign("4ed45cf3-9318-4aa0-872e-cae3046f2caa", "c8e68491-e7f4-4598-a64b-2ccba2d2edc5", "6f720433-022b-4b2a-8daf-8cc0e208bff5")]
        [DefaultValue((short)2000)]
        public IRuntimeSetting<short> ShortValueTwo { get; internal set; }
        
        [SettingDesign("5e5fcdfe-ae3d-4e0e-950f-4f7b28b3a1c3", "ac1eda71-14bc-4d3a-8475-300b28e4d57b", "497a8477-0ac6-4865-a007-f3c0e19b9eea")]
        [DefaultValue((ushort)3000)]
        public IRuntimeSetting<ushort> UShortValueOne { get; internal set; }
        [SettingDesign("af05ebc4-2c8f-42af-bc90-1cb92d6dd5f8", "ef3498e6-c75a-475e-bf25-822d80c53de1", "34a5cfda-ef0f-4d6f-ace0-7d1d7cc7a89f")]
        [DefaultValue((ushort)4000)]
        public IRuntimeSetting<ushort> UShortValueTwo { get; internal set; }
        
        [SettingDesign("c6bab13b-6641-4694-b09b-2957c0a4086e", "f23f2970-38db-4a39-863c-03bcfefc3fd5", "d2b479d4-fe8e-4cbd-98f4-d91c7ea91ccc")]
        [DefaultValue(50000)]
        public IRuntimeSetting<int> IntValueOne { get; internal set; }
        [SettingDesign("5d64506a-a311-45a0-904a-ef3b605afb73", "c03ce524-f58c-4eaf-89c8-2e884e9d99d3", "0f74374a-3422-48af-a375-69b0acaf1c7f")]
        [DefaultValue(100000)]
        public IRuntimeSetting<int> IntValueTwo { get; internal set; }
        
        [SettingDesign("e1322002-f56d-47a0-ad4c-434d8790561a", "9e4402a3-4260-4f6b-93cb-7bf787d383d6", "12e99abe-5253-4e38-a232-6ec00d9617bb")]
        [DefaultValue(3000000000u)]
        public IRuntimeSetting<uint> UIntValueOne { get; internal set; }
        [SettingDesign("f7db800c-2b3a-4ba2-a3ed-f792f8e7903d", "4afbddb6-8c89-4d00-a9d8-6849dbf12f55", "85d27cab-ce5f-4dda-bf99-19724de560f4")]
        [DefaultValue(4000000000u)]
        public IRuntimeSetting<uint> UIntValueTwo { get; internal set; }
        
        [SettingDesign("21ea42c2-31f0-4bd8-8303-fc6687de7e45", "704304db-cb03-4419-b15e-6ce93c91a3df", "0b393f2f-c8ad-4668-ad8b-7330eb5557e1")]
        [DefaultValue(9000000000L)]
        public IRuntimeSetting<long> LongValueOne { get; internal set; }
        [SettingDesign("551e396f-bfd5-423b-b50d-3d2536e35322", "372975e9-abf8-4d59-8a34-27b0f83bdcb2", "da24ba15-33d5-4e0e-be2c-11bf701ddb10")]
        [DefaultValue(9500000000L)]
        public IRuntimeSetting<long> LongValueTwo { get; internal set; }
        
        [SettingDesign("c85d10a1-0443-4688-8850-f10006555460", "713533b6-5080-4f02-bf70-046a226a8bef", "bf1c5bde-c284-412d-9459-f3b67c73e137")]
        [DefaultValue(18000000000UL)]
        public IRuntimeSetting<ulong> ULongValueOne { get; internal set; }
        [SettingDesign("378b74b4-0530-458a-bf23-ac43322d4156", "1cac74df-e44f-487e-bf36-7f8ab60bccec", "9b3ecd16-2428-4bfb-bb02-55126e3a8a58")]
        [DefaultValue(18500000000UL)]
        public IRuntimeSetting<ulong> ULongValueTwo { get; internal set; }
        
        [SettingDesign("6f43bcf2-1b72-4a1c-895f-bbeeb75eb54d", "2be83c05-fe82-4332-86db-d95d71c47fb9", "37196ed9-01d0-4f78-b611-6379164cd1ef")]
        [DefaultValue(3.14f)]
        public IRuntimeSetting<float> FloatValueOne { get; internal set; }
        [SettingDesign("1088507f-baeb-4ad7-bf35-29842849d0bf", "dec40c24-fc65-4d96-aa6a-c94b5b56292d", "832fee56-1746-4f3d-a368-a568747a4e99")]
        [DefaultValue(2.71f)]
        public IRuntimeSetting<float> FloatValueTwo { get; internal set; }
        
        [SettingDesign("8a72ba78-8046-4b9c-8dd3-572a2e9d135b", "8bd6d9a3-c519-4e88-b152-00766c960ec2", "96558c74-3d7f-441d-a712-391f26e07554")]
        [DecimalDefaultValueAttribute(3.14d)]
        public IRuntimeSetting<decimal> DecimalValueOne { get; internal set; }
        [SettingDesign("9780b63d-58ff-44fb-859b-a17174b3a124", "6fed6856-bdb8-43db-b19d-9c96066ad9ad", "71771f96-39e4-471e-b181-53742f02dc3a")]
        [DecimalDefaultValueAttribute(2.71d)]
        public IRuntimeSetting<decimal> DecimalValueTwo { get; internal set; }
        
        [SettingDesign("545d3c52-b57e-4159-99a1-8043bd02fc3a", "a2ea077b-d55f-4421-bd30-9cfa09f6ffbc", "9186ebf0-a36b-4986-bb1e-299dcd5acb9b")]
        [DefaultValue("Hello")]
        public IRuntimeSetting<string> StringValueOne { get; internal set; }
        [SettingDesign("a543e368-b23a-4daa-b278-2f7c5d54be65", "1a8cf39b-6eb1-402e-af11-bc9cd125c540", "ba5d4416-434f-47ea-b780-0e1cfde4f8ec")]
        [DefaultValue("World")]
        public IRuntimeSetting<string> StringValueTwo { get; internal set; }

        [SettingDesign("5cab63ae-fd70-486c-b6c6-6f6eb8cc6cf5", "5bdaad74-2246-40d8-b71a-a9f2afff3188", "748187d0-4d62-4b4e-b00d-632245f64e50")]
        [DateTimeDefaultValueAttribute("2025-01-01T10:00:00")]
        public IRuntimeSetting<DateTime> DateTimeValueOne { get; internal set; }
        [SettingDesign("0d74e455-a21b-4808-95d8-bb1f5213f03e", "35637cdb-312b-45f6-a2ff-6c14e2189fc3", "70e224fd-d5b7-4945-9caf-9ba5255a0326")]
        [DateTimeDefaultValueAttribute("2025-12-31T18:30:00")]
        public IRuntimeSetting<DateTime> DateTimeValueTwo { get; internal set; }

        [SettingDesign("99fd98cf-2e20-4170-9d71-abf147d75b42", "8a139b24-bd3e-4593-803a-ce18acf7abb7", "a7c8d2e4-5f1a-4b9c-8d3e-2f6a1b8c9d4e")]
        [TimeSpanDefaultValueAttribute("02:00:00")]
        public IRuntimeSetting<TimeSpan> TimeSpanValueOne { get; internal set; }
        [SettingDesign("b3e7f5a9-4c2d-4e8f-9a1b-7d3e5f8c2a4b", "c9d8e7f6-a5b4-4c3d-8e2f-1a9b8c7d6e5f", "d4e9f8a7-b6c5-4d3e-9f2a-1b8c7d6e5f4a")]
        [TimeSpanDefaultValueAttribute("00:45:00")]
        public IRuntimeSetting<TimeSpan> TimeSpanValueTwo { get; internal set; }
    }
}

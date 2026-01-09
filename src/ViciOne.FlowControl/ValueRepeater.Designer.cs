using System;
using ViciOne.Core.Contracts.DataModel;
using ViciOne.Core.Contracts.Runtime;
using ViciOne.Core.Runtime;
using ViciOne.Core.Runtime.Attributes;

namespace ViciOne.FlowControl;

[FunctionBlockDesign("c85a8b84-8954-4823-8ead-ed78b3c19587", "cb9bb30a-e152-4948-a3f6-9ecbb78a8150", "75f46862-36b5-48e4-aecd-4a94eb68852c")]
[DefaultRunMode(RunMode.Cyclic)]
[ValidRunModes(RunMode.Cyclic, RunMode.Change)]
public partial class ValueRepeater : RuntimeFunctionBlock<ValueRepeater>
{
    public new Inputs_ ProcessDataInputs { get; } = new Inputs_();
    [InputConnectorDesigns]
    public class Inputs_
    {
        [InputConnectorDesign("824426b4-ce4d-4f8f-83f1-cf10db256f2e")]
        public IRuntimeConnector<bool> BoolValue { get; internal set; }
        [InputConnectorDesign("47e36cf4-b2c5-44dd-a1b9-e4cd02ee41c6")]
        public IRuntimeConnector<double> DoubleValue { get; internal set; }
        [InputConnectorDesign("a164e734-a59f-44c6-9948-72065b5b6873")]
        public IRuntimeConnector<byte> ByteValue { get; internal set; }
        [InputConnectorDesign("a054a16c-dea4-4d38-8355-87da42eed76a")]
        public IRuntimeConnector<sbyte> SByteValue { get; internal set; }
        [InputConnectorDesign("0fcb9e31-f303-4672-ba5e-087653467a07")]
        public IRuntimeConnector<char> CharValue { get; internal set; }
        [InputConnectorDesign("42c33e89-d52d-4bfc-9a05-b2bed04b3d60")]
        public IRuntimeConnector<short> ShortValue { get; internal set; }
        [InputConnectorDesign("238d4ed4-bf58-4620-9ff3-9441cd1bb76d")]
        public IRuntimeConnector<ushort> UShortValue { get; internal set; }
        [InputConnectorDesign("6fab6d16-8504-460a-9336-4f7a3e84deaf")]
        public IRuntimeConnector<int> IntValue { get; internal set; }
        [InputConnectorDesign("3e5bebd2-dfe2-4e20-a539-f2bcb47edc2d")]
        public IRuntimeConnector<uint> UIntValue { get; internal set; }
        [InputConnectorDesign("ad534f9d-3168-4269-8aea-147b3e126ce4")]
        public IRuntimeConnector<long> LongValue { get; internal set; }
        [InputConnectorDesign("447bc6b1-9392-4c05-b576-99d0c5d8df17")]
        public IRuntimeConnector<ulong> ULongValue { get; internal set; }
        [InputConnectorDesign("4fd68f20-b208-434b-9020-43b87eaff1c9")]
        public IRuntimeConnector<float> FloatValue { get; internal set; }
        [InputConnectorDesign("60b67147-dd57-4e18-a1a0-053fd547b1ba")]
        public IRuntimeConnector<decimal> DecimalValue { get; internal set; }
        [InputConnectorDesign("ab7ec538-bb81-4d9d-9aab-49180c9c01f7")]
        public IRuntimeConnector<string> StringValue { get; internal set; }
        [InputConnectorDesign("86fcd1f8-1542-48ef-b163-0b32fae8e6d8")]
        public IRuntimeConnector<DateTime> DateTimeValue { get; internal set; }
        [InputConnectorDesign("239cadbd-67a6-4e29-bc72-d3d5084c434b")]
        public IRuntimeConnector<TimeSpan> TimeSpanValue { get; internal set; }
    }

    public new Outputs_ ProcessDataOutputs { get; } = new Outputs_();
    [OutputConnectorDesigns]
    public class Outputs_
    {
        [OutputConnectorDesign("14901283-d2c8-48e6-abd9-b0f55ba717f1")]
        public IRuntimeConnector<bool> BoolValue { get; internal set; }
        [OutputConnectorDesign("44a8779b-6778-49d9-9818-2df4f9e7fb84")]
        public IRuntimeConnector<double> DoubleValue { get; internal set; }
        [OutputConnectorDesign("6ba9064b-7e85-4d91-8049-a9d1eb56873b")]
        public IRuntimeConnector<byte> ByteValue { get; internal set; }
        [OutputConnectorDesign("bcf91007-b7b2-4729-ad3f-ceb9ee2a77c7")]
        public IRuntimeConnector<sbyte> SByteValue { get; internal set; }
        [OutputConnectorDesign("9253601b-9b16-4d87-ac3a-e0fae16c9e31")]
        public IRuntimeConnector<char> CharValue { get; internal set; }
        [OutputConnectorDesign("b14338e1-61c8-46bf-bde2-199654c86d97")]
        public IRuntimeConnector<short> ShortValue { get; internal set; }
        [OutputConnectorDesign("42b859d1-d532-43ab-85c9-79c9348b75d5")]
        public IRuntimeConnector<ushort> UShortValue { get; internal set; }
        [OutputConnectorDesign("3cb3c0c9-a982-450a-98a2-f82f82effe11")]
        public IRuntimeConnector<int> IntValue { get; internal set; }
        [OutputConnectorDesign("e5415bd8-e9a9-4d8d-8718-10522586a674")]
        public IRuntimeConnector<uint> UIntValue { get; internal set; }
        [OutputConnectorDesign("3700f3ae-f62d-4840-a1cd-81b5eb065ea3")]
        public IRuntimeConnector<long> LongValue { get; internal set; }
        [OutputConnectorDesign("69efb9a6-92ff-4c33-b431-f70556fb5278")]
        public IRuntimeConnector<ulong> ULongValue { get; internal set; }
        [OutputConnectorDesign("7a6c9ba2-2ed4-41f2-8c76-727668364698")]
        public IRuntimeConnector<float> FloatValue { get; internal set; }
        [OutputConnectorDesign("962ad481-1b3f-4e40-9643-45cce5a76a99")]
        public IRuntimeConnector<decimal> DecimalValue { get; internal set; }
        [OutputConnectorDesign("d955032f-c268-49fb-a2e6-b35594d72c9f")]
        public IRuntimeConnector<string> StringValue { get; internal set; }
        [OutputConnectorDesign("fbb6e897-443b-4494-9354-16cbfa0086d8")]
        public IRuntimeConnector<DateTime> DateTimeValue { get; internal set; }
        [OutputConnectorDesign("088afb4e-e341-41a6-a0bd-529103fd4f87")]
        public IRuntimeConnector<TimeSpan> TimeSpanValue { get; internal set; }
    }
}

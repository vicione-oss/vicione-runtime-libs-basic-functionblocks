using ViciOne.Core.Contracts.Runtime;
using ViciOne.Core.Runtime;
using ViciOne.Core.Runtime.Attributes;
using ViciOne.Engine.DefaultValidators;

namespace ViciOne.Math;

[FunctionBlockDesign("0b25091a-b4f5-48ab-a8f5-ef4ed90fd851", "84d47cb2-a18d-4f52-8612-98de03a40635", "3d2b0952-e869-4ce4-af2a-35fc8b5d00ad")]
public partial class Round : RuntimeFunctionBlock<Round>
{
    public new Inputs_ ProcessDataInputs { get; } = new Inputs_();
    [InputConnectorDesigns]
    public class Inputs_
    {
        [InputConnectorDesign("d7f17ac6-94c3-4461-be43-500ed420ab95")]
        public IRuntimeConnector<double> Value { get; internal set; }
    }

    public new Outputs_ ProcessDataOutputs { get; } = new Outputs_();
    [OutputConnectorDesigns]
    public class Outputs_
    {
        [OutputConnectorDesign("7764fa64-cb66-4dcf-b354-f5987536fdb0")]
        public IRuntimeConnector<double> Value { get; internal set; }
    }

    public new Settings_ Settings { get; } = new Settings_();
    [SettingDesigns]
    public class Settings_
    {
        [SettingDesign("46b68a37-f458-41e3-a4c9-9673bb74c5bf", "3e2a47cd-b5e3-42cd-a3a3-975935ff11dc", "ea62a17b-9459-4919-9989-4aa4b17bdc91")]
        [RangeValidator<int>(0, 17)]
        public IRuntimeSetting<int> DecimalPlaces { get; internal set; }
        [SettingDesign("74ff1db2-7fbc-4cec-bc53-37cef954fec6", "3b7dbc76-45ea-4d81-be8b-ffee065ab42e", "63e92c19-c370-4610-b678-9021e95aa947")]
        [EnumValidator<RoundingBehavior>(ViciOne.Math.RoundingBehavior.Up)]
        public IRuntimeSetting<RoundingBehavior> RoundingBehavior { get; internal set; }
    }
}

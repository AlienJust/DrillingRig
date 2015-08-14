using System.Xml.Linq;
using DrillingRig.Commands.BsEthernetNominals;

namespace DrillingRig.ConfigApp.BsEthernetNominals {
	class BsEthernetNominalsExporterXml : IBsEthernetNominalsExporter
	{
		private readonly string _filename;
		public BsEthernetNominalsExporterXml(string filename)
		{
			_filename = filename;
		}

		public void ExportSettings(IBsEthernetNominals nominals) {
			var doc = new XDocument(
				new XDeclaration("1,0", "utf-8", "yes"),
				new XElement(BsEthernetNominalsSerializationConstants.BsEthernetNominals,
					new XElement(BsEthernetNominalsSerializationConstants.RatedRotationFriquencyCalculated, nominals.RatedRotationFriquencyCalculated),
					new XElement(BsEthernetNominalsSerializationConstants.RatedPwmModulationCoefficient, nominals.RatedPwmModulationCoefficient),
					new XElement(BsEthernetNominalsSerializationConstants.RatedMomentumCurrentSetting, nominals.RatedMomentumCurrentSetting),
					new XElement(BsEthernetNominalsSerializationConstants.RatedRadiatorTemperature, nominals.RatedRadiatorTemperature),
					new XElement(BsEthernetNominalsSerializationConstants.RatedDcBusVoltage, nominals.RatedDcBusVoltage),
					new XElement(BsEthernetNominalsSerializationConstants.RatedAllPhasesCurrentAmplitudeEnvelopeCurve, nominals.RatedAllPhasesCurrentAmplitudeEnvelopeCurve),
					new XElement(BsEthernetNominalsSerializationConstants.RatedRegulatorCurrentDoutput, nominals.RatedRegulatorCurrentDoutput),
					new XElement(BsEthernetNominalsSerializationConstants.RatedRegulatorCurrentQoutput, nominals.RatedRegulatorCurrentQoutput),
					new XElement(BsEthernetNominalsSerializationConstants.RatedFriquencyIntensitySetpointOutput, nominals.RatedFriquencyIntensitySetpointOutput),
					new XElement(BsEthernetNominalsSerializationConstants.RatedFlowSetting, nominals.RatedFlowSetting),
					new XElement(BsEthernetNominalsSerializationConstants.RatedMeasuredMoment, nominals.RatedMeasuredMoment),
					new XElement(BsEthernetNominalsSerializationConstants.RatedSpeedRegulatorOutputOrMomentSetting, nominals.RatedSpeedRegulatorOutputOrMomentSetting),
					new XElement(BsEthernetNominalsSerializationConstants.RatedMeasuredFlow, nominals.RatedMeasuredFlow),
					new XElement(BsEthernetNominalsSerializationConstants.RatedSettingExcitationCurrent, nominals.RatedSettingExcitationCurrent)));
			doc.Save(_filename);
		}
	}
}
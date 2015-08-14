using System;
using System.Xml.Linq;
using DrillingRig.Commands.BsEthernetNominals;

namespace DrillingRig.ConfigApp.BsEthernetNominals {
	class BsEthernetNominalsImporterXml : IBsEthernetNominalsImporter
	{
		private readonly string _filename;
		public BsEthernetNominalsImporterXml(string filename)
		{
			_filename = filename;
		}

		public IBsEthernetNominals ImportSettings()
		{
			// TODO: move strings to constants (shared beetween import and export static class)
			try {
				var doc = XDocument.Load(_filename);
				var root = doc.Element(BsEthernetNominalsSerializationConstants.BsEthernetNominals);
				return new BsEthernetNominalsSimple(
					short.Parse(root.Element(BsEthernetNominalsSerializationConstants.RatedRotationFriquencyCalculated).Value),
					short.Parse(root.Element(BsEthernetNominalsSerializationConstants.RatedPwmModulationCoefficient).Value),
					short.Parse(root.Element(BsEthernetNominalsSerializationConstants.RatedMomentumCurrentSetting).Value),
					short.Parse(root.Element(BsEthernetNominalsSerializationConstants.RatedRadiatorTemperature).Value),
					short.Parse(root.Element(BsEthernetNominalsSerializationConstants.RatedDcBusVoltage).Value),
					short.Parse(root.Element(BsEthernetNominalsSerializationConstants.RatedAllPhasesCurrentAmplitudeEnvelopeCurve).Value),
					short.Parse(root.Element(BsEthernetNominalsSerializationConstants.RatedRegulatorCurrentDoutput).Value),
					short.Parse(root.Element(BsEthernetNominalsSerializationConstants.RatedRegulatorCurrentQoutput).Value),
					short.Parse(root.Element(BsEthernetNominalsSerializationConstants.RatedFriquencyIntensitySetpointOutput).Value),
					short.Parse(root.Element(BsEthernetNominalsSerializationConstants.RatedFlowSetting).Value),
					short.Parse(root.Element(BsEthernetNominalsSerializationConstants.RatedMeasuredMoment).Value),
					short.Parse(root.Element(BsEthernetNominalsSerializationConstants.RatedSpeedRegulatorOutputOrMomentSetting).Value),
					short.Parse(root.Element(BsEthernetNominalsSerializationConstants.RatedMeasuredFlow).Value),
					short.Parse(root.Element(BsEthernetNominalsSerializationConstants.RatedSettingExcitationCurrent).Value));
			}
			catch (Exception ex) {
				throw new Exception("Cannot import settings", ex);
			}
		}
	}
}
using System;
using System.Collections.Generic;
using DrillingRid.Commands.Contracts;
using DrillingRig.Commands.BsEthernetSettings;

namespace DrillingRig.Commands.BsEthernetNominals {
	public class WriteBsEthernetNominalsCommand : IRrModbusCommandWithReply, IRrModbusCommandResultGetter<IWriteBsEthernetSettingsResult>, IRrModbusCommandWithTestReply {
		private readonly IBsEthernetNominals _nominals;

		public WriteBsEthernetNominalsCommand(IBsEthernetNominals nominals) {
			_nominals = nominals;
		}

		public byte CommandCode {
			get { return 0x83; }
		}

		public string Name {
			get { return "Запись номинальных значений БС-Ethernet"; }
		}

		public byte[] Serialize() {
			var result = new List<byte>();
			result.Add((byte)(_nominals.RatedRotationFriquencyCalculated & 0x00FF));
			result.Add((byte)((_nominals.RatedRotationFriquencyCalculated & 0xFF00) >> 8));

			result.Add((byte)(_nominals.RatedPwmModulationCoefficient & 0x00FF));
			result.Add((byte)((_nominals.RatedPwmModulationCoefficient & 0xFF00) >> 8));

			result.Add((byte)(_nominals.RatedMomentumCurrentSetting & 0x00FF));
			result.Add((byte)((_nominals.RatedMomentumCurrentSetting & 0xFF00) >> 8));

			result.Add((byte)(_nominals.RatedRadiatorTemperature & 0x00FF));
			result.Add((byte)((_nominals.RatedRadiatorTemperature & 0xFF00) >> 8));

			result.Add((byte)(_nominals.RatedDcBusVoltage & 0x00FF));
			result.Add((byte)((_nominals.RatedDcBusVoltage & 0xFF00) >> 8));

			result.Add((byte)(_nominals.RatedAllPhasesCurrentAmplitudeEnvelopeCurve & 0x00FF));
			result.Add((byte)((_nominals.RatedAllPhasesCurrentAmplitudeEnvelopeCurve & 0xFF00) >> 8));

			result.Add((byte)(_nominals.RatedRegulatorCurrentDoutput & 0x00FF));
			result.Add((byte)((_nominals.RatedRegulatorCurrentDoutput & 0xFF00) >> 8));

			result.Add((byte)(_nominals.RatedRegulatorCurrentQoutput & 0x00FF));
			result.Add((byte)((_nominals.RatedRegulatorCurrentQoutput & 0xFF00) >> 8));

			result.Add((byte)(_nominals.RatedFriquencyIntensitySetpointOutput & 0x00FF));
			result.Add((byte)((_nominals.RatedFriquencyIntensitySetpointOutput & 0xFF00) >> 8));

			result.Add((byte)(_nominals.RatedFlowSetting & 0x00FF));
			result.Add((byte)((_nominals.RatedFlowSetting & 0xFF00) >> 8));

			result.Add((byte)(_nominals.RatedMeasuredMoment & 0x00FF));
			result.Add((byte)((_nominals.RatedMeasuredMoment & 0xFF00) >> 8));

			result.Add((byte)(_nominals.RatedSpeedRegulatorOutputOrMomentSetting & 0x00FF));
			result.Add((byte)((_nominals.RatedSpeedRegulatorOutputOrMomentSetting & 0xFF00) >> 8));

			result.Add((byte)(_nominals.RatedMeasuredFlow & 0x00FF));
			result.Add((byte)((_nominals.RatedMeasuredFlow & 0xFF00) >> 8));

			result.Add((byte)(_nominals.RatedSettingExcitationCurrent & 0x00FF));
			result.Add((byte)((_nominals.RatedSettingExcitationCurrent & 0xFF00) >> 8));
			
			return result.ToArray();
		}

		public IWriteBsEthernetSettingsResult GetResult(byte[] reply) {
			if (reply.Length != ReplyLength)
				throw new Exception("Reply error, reply length must be 0");
			return new WriteBsEthernetSettingsResultSimple();
		}

		public int ReplyLength {
			get { return 0; }
		}

		public byte[] GetTestReply() {
			return new byte[0];
		}
	}
}
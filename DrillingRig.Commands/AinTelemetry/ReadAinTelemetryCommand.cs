using System;
using System.Linq;
using DrillingRid.Commands.Contracts;

namespace DrillingRig.Commands.AinTelemetry {
	public class ReadAinTelemetryCommand : IRrModbusCommandWithReply, IRrModbusCommandResultGetter<IAinTelemetry>, IRrModbusCommandWithTestReply
	{
		private readonly byte _zeroBasedAinNumber;

		public ReadAinTelemetryCommand(byte zeroBasedAinNumber) {
			_zeroBasedAinNumber = zeroBasedAinNumber;
		}

		public byte CommandCode => 0x85;

		public string Name => "Чтение телеметрии АИН #" + (_zeroBasedAinNumber + 1);

		public byte[] Serialize()
		{
			return new[] {(byte)(_zeroBasedAinNumber + 1)};
		}

		public IAinTelemetry GetResult(byte[] reply) {
			// TODO: check if reply[0] is equal _zbAinNumber

			var oldReply = reply.Skip(4).ToList();

			
			
			return new AinTelemetrySimple(
				commonEngineState: (ushort) (reply[1] + (reply[2] << 8)),
				commonFaultState: (ushort) (reply[3] + (reply[4] << 8)),

				rotationFriquencyCalculated: ((short) (oldReply[1] + (oldReply[2] << 8)))*0.01,
				pwmModulationCoefficient: (short) (oldReply[3] + (oldReply[4] << 8))*1.0,
				momentumCurrentSetting: (short) (oldReply[5] + (oldReply[6] << 8))*1.0,
				radiatorTemperature: (short) (oldReply[7] + (oldReply[8] << 8))*1.0,
				dcBusVoltage: (short) (oldReply[9] + (oldReply[10] << 8))*1.0,
				allPhasesCurrentAmplitudeEnvelopeCurve: ((short) (oldReply[11] + (oldReply[12] << 8)))*1.0,
				regulatorCurrentDoutput: (short) (oldReply[13] + (oldReply[14] << 8))*1.0/256.0,
				regulatorCurrentQoutput: (short) (oldReply[15] + (oldReply[16] << 8))*1.0/256.0,
				friquencyIntensitySetpointOutput: (short) (oldReply[17] + (oldReply[18] << 8))*0.01,
				flowSetting: (short) (oldReply[19] + (oldReply[20] << 8))*1.0,
				measuredMoment: (short) (oldReply[21] + (oldReply[22] << 8))*1.0,
				speedRegulatorOutputOrMomentSetting: (short) (oldReply[23] + (oldReply[24] << 8))*1.0,
				measuredFlow: (short) (oldReply[25] + (oldReply[26] << 8))*1.0,
				settingExcitationCurrent: (short) (oldReply[27] + (oldReply[28] << 8))*1.0,

				runModeBits12: ModeSetRunModeBits12Extensions.FromInt(oldReply[29] & 0x03),

				resetZiToZero: (oldReply[29] & 0x04) == 0x04,
				resetFault: (oldReply[29] & 0x08) == 0x08,
				limitRegulatorId: (oldReply[29] & 0x10) == 0x10,
				limitRegulatorIq: (oldReply[29] & 0x20) == 0x20,
				limitRegulatorSpeed: (oldReply[29] & 0x40) == 0x40,
				limitRegulatorFlow: (oldReply[29] & 0x80) == 0x80,

				momentumSetterSelector: ModeSetMomentumSetterSelectorExtensions.FromInt((oldReply[30] & 0x03)),

				// Status:
				status: (ushort)(oldReply[31] + (oldReply[32] << 8)),

				driver1HasErrors: (oldReply[31] & 0x01) == 0x01, // 0
				driver2HasErrors: (oldReply[31] & 0x02) == 0x02, // 1
				driver3HasErrors: (oldReply[31] & 0x04) == 0x04, // 2
				driver4HasErrors: (oldReply[31] & 0x08) == 0x08, // 3
				driver5HasErrors: (oldReply[31] & 0x10) == 0x10, // 4
				driver6HasErrors: (oldReply[31] & 0x20) == 0x20, // 5
				somePhaseMaximumAlowedCurrentExcess: (oldReply[31] & 0x40) == 0x40, // 6
				radiatorKeysTemperatureRiseTo85DegreesExcess: (oldReply[31] & 0x80) == 0x80, // 7

				allowedDcVoltageExcess: (oldReply[32] & 0x01) == 0x01, // 8
				noLinkOnSyncLine: (oldReply[32] & 0x02) == 0x02, // 9
				externalTemperatureLimitExcess: (oldReply[32] & 0x04) == 0x04, // 10
				rotationFriquecnySensorFault: (oldReply[32] & 0x08) == 0x08, // 11
				eepromI2CErrorDefaultParamsAreLoaded: (oldReply[32] & 0x10) == 0x10, // 12
				eepromCrcErrorDefaultParamsAreLoaded: (oldReply[32] & 0x20) == 0x20, // 13
				someSlaveFault: (oldReply[32] & 0x40) == 0x40, // 14
				configChangeDuringParallelWorkConfirmationNeed: (oldReply[32] & 0x80) == 0x80, // 15

				// rotation freq measured dcv:
				rotationFriquencyMeasuredDcv: (short) (oldReply[33] + (oldReply[34] << 8))*1.0,

				afterFilterSpeedControllerFeedbackFriquency: (short) (oldReply[35] + (oldReply[36] << 8))*1.0,
				afterFilterFimag: (short) (oldReply[37] + (oldReply[38] << 8))*1.0,
				currentDpartMeasured: (short) (oldReply[39] + (oldReply[40] << 8))*1.0,
				currentQpartMeasured: (short) (oldReply[41] + (oldReply[42] << 8))*1.0,
				afterFilterFset: (short) (oldReply[43] + (oldReply[44] << 8))*1.0,
				afterFilterTorq: (short) (oldReply[45] + (oldReply[46] << 8))*1.0,

				// Text (External temperature)
				externalTemperature: (short) (oldReply[47] + (oldReply[48] << 8))*1.0,

				dCurrentRegulatorProportionalPart: (short) (oldReply[49] + (oldReply[50] << 8))*1.0,
				qcurrentRegulatorProportionalPart: (short) (oldReply[51] + (oldReply[52] << 8))*1.0,
				speedRegulatorProportionalPart: (short) (oldReply[53] + (oldReply[54] << 8))*1.0,
				flowRegulatorProportionalPart: (short) (oldReply[55] + (oldReply[56] << 8))*1.0,
				calculatorDflowRegulatorOutput: (short) (oldReply[57] + (oldReply[58] << 8))*1.0,
				calculatorQflowRegulatorOutput: (short) (oldReply[59] + (oldReply[60] << 8))*1.0,


				aux1: (ushort)(oldReply[61] + (oldReply[62] << 8)),
				aux2: (ushort)(oldReply[63] + (oldReply[64] << 8)),
				pver: (ushort)(oldReply[65] + (oldReply[66] << 8)),
				pvDate: ((ushort)(oldReply[67] + (oldReply[68] << 8))).FromUshort(),

				//status byte:
				ain1LinkFault: (oldReply[69] & 0x01) == 0x01,
				ain2LinkFault: (oldReply[69] & 0x02) == 0x02,
				ain3LinkFault: (oldReply[69] & 0x04) == 0x04);
		}

		public int ReplyLength => 74;

		public byte[] GetTestReply() {
			var rnd = new Random();
			var result = new byte[ReplyLength];
			rnd.NextBytes(result);
			result[0] = _zeroBasedAinNumber;
			
			result[1] = (byte) rnd.Next(0, 21);
			result[2] = 0;

			result[3] = (byte)rnd.Next(0, 6);
			result[4] = 0;

			result[71] = 0x85;
			result[72] = 0x1F;
			return result;
		}
	}

	static class DateTimeExtensions {
		public static DateTime? FromUshort(this ushort value) {
			DateTime? buildDate;
			try {
				int year = 2000 + ((value >> 9) & 0x7F);
				int month = (value >> 5) & 0x0F;
				int day = value & 0x1F;
				buildDate = new DateTime(year, month, day);
			}
			catch {
				buildDate = null;
			}
			return buildDate;
		}
	}
}
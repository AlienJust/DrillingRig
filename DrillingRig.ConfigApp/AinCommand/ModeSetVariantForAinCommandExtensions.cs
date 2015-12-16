using System;
namespace DrillingRig.ConfigApp.AinCommand {
	static class ModeSetVariantForAinCommandExtensions {
		public static string ToText(this ModeSetVariantForAinCommand variant) {
			switch (variant) {
				case ModeSetVariantForAinCommand.Off1:
					return "Останов 1 (OFF 1)";
				case ModeSetVariantForAinCommand.Off2:
					return "Останов 2 (выбег - OFF 2)";
				case ModeSetVariantForAinCommand.Off3:
					return "Останов 3 (OFF 3)";
				case ModeSetVariantForAinCommand.Run:
					return "Тяга (RUN)";
				case ModeSetVariantForAinCommand.Inching1:
					return "Толчок 1 (INCHING 1)";
				case ModeSetVariantForAinCommand.Inching2:
					return "Толчок 2 (INCHING 2)";
				case ModeSetVariantForAinCommand.Reset:
					return "Сброс аварий (RESET)";
				default:
					throw new Exception("Cannot conver such " + typeof (ModeSetVariantForAinCommand).FullName + " to " + typeof (string).FullName);
			}
		}

		public static ushort ToUshort(this ModeSetVariantForAinCommand variant)
		{
			switch (variant)
			{
				case ModeSetVariantForAinCommand.Off1:
					return 0x406; /*100 0000 0110*/;
				case ModeSetVariantForAinCommand.Off2:
					return 0x405; /*100 0000 0101*/
				case ModeSetVariantForAinCommand.Off3:
					return 0x403; /*100 0000 0011*/ 
				case ModeSetVariantForAinCommand.Run:
					return 0x40F; /*100 0000 1111*/
				case ModeSetVariantForAinCommand.Inching1:
					return 0x50F; /*101 0000 1111*/
				case ModeSetVariantForAinCommand.Inching2:
					return 0x60F; /*110 0000 1111*/
				case ModeSetVariantForAinCommand.Reset:
					return 0x480; /*100 1000 0000*/
				default:
					throw new Exception("Cannot conver such " + typeof(ModeSetVariantForAinCommand).FullName + " to " + typeof(ushort).FullName);
			}
		}
	}
}
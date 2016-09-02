using System;
using DrillingRig.Commands.AinSettings;

namespace DrillingRig.ConfigApp.LookedLikeAbb.AinSettingsRw {
	delegate void AinSettingsReadCompleteDelegate(byte zeroBasedAinNumber, Exception readInnerException, IAinSettings settings);
}
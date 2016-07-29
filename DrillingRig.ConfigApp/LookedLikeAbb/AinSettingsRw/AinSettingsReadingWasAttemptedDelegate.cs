using System;
using DrillingRig.Commands.AinSettings;

namespace DrillingRig.ConfigApp.LookedLikeAbb.AinSettingsRw {
	delegate void AinSettingsReadingWasAttemptedDelegate(byte zeroBasedAinNumber, Exception readInnerException, IAinSettings settings);
}
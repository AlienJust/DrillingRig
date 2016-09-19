using System;
using DrillingRig.Commands.AinSettings;

namespace DrillingRig.ConfigApp.AppControl.AinSettingsRead {
	delegate void AinSettingsReadCompleteDelegate(byte zeroBasedAinNumber, Exception readInnerException, IAinSettings settings);
}
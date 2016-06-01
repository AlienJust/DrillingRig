namespace DrillingRig.ConfigApp.LookedLikeAbb {
	internal interface ICycleReader {
		void AskToStartReadAin1TelemetryCycle();
		void AskToStopReadAin1TelemetryCycle();

		void AskToStartReadAin2TelemetryCycle();
		void AskToStopReadAin2TelemetryCycle();

		void AskToStartReadAin3TelemetryCycle();
		void AskToStopReadAin3TelemetryCycle();

		event AinTelemetryReadedDelegate Ain1TelemetryReaded;
		event AinTelemetryReadedDelegate Ain2TelemetryReaded;
		event AinTelemetryReadedDelegate Ain3TelemetryReaded;
	}
}
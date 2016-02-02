namespace DrillingRig.ConfigApp.SystemControl {
	interface INamedTrendsControl {
		void ClearTrendData(string name);
		bool GetTrendVisibility(string name);
		void SetTrendVisibility(string name, bool value);

		bool GetSignedFlag(string name);
		void SetSignedFlag(string name, bool isSigned);
	}
}
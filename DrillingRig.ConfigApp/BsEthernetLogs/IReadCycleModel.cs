namespace DrillingRig.ConfigApp.BsEthernetLogs
{
	internal interface IReadCycleModel
	{
		event IcAnotherLogLineWasReadedOrNot AnotherLogLineWasReaded;
		bool IsReadCycleEnabled { get; set; }

		/// <summary>
		/// Если циклический опрос совсем больше не нужен
		/// </summary>
		void StopBackgroundThreadAndWaitForIt();
	}
}
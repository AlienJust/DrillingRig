using System;
using System.Collections.Generic;
using System.Threading;
using AlienJust.Support.Concurrent;
using AlienJust.Support.Loggers.Contracts;

namespace DrillingRig.ConfigApp.AppControl.Cycle {
	class CycleThreadHolderThreadSafe : ICycleThreadHolder {
		private readonly IMultiLoggerWithStackTrace _debugLogger;
		private readonly object _cyclePartsSync;
		private readonly List<ICyclePart> _cycleParts;
		private readonly SingleThreadedRelayQueueWorker<Action> _backWorker;

		public CycleThreadHolderThreadSafe(IMultiLoggerWithStackTrace debugLogger) {
			_debugLogger = debugLogger;
			// циклический опрос
			_cyclePartsSync = new object();
			_cycleParts = new List<ICyclePart>();
			_backWorker = new SingleThreadedRelayQueueWorker<Action>("CycleBackWorker", a => a(), ThreadPriority.Lowest, true, null, _debugLogger.GetLogger(0));
			_backWorker.AddWork(CycleWork);
		}

		private void CycleWork() {
			while (true) {
				lock (_cyclePartsSync) {
					foreach (var cyclePart in _cycleParts) {
						if (!cyclePart.Cancel) {
							try {
								cyclePart.InCycleAction();
								Thread.Sleep(10);
							}
							catch {
								Thread.Sleep(10);
							}
						}
						else Thread.Sleep(5);
					}
				}
			}
		}

		public void RegisterAsCyclePart(ICyclePart part) {
			lock (_cyclePartsSync) {
				_cycleParts.Add(part);
			}
		}
	}
}
using System;
using System.Collections.Generic;
using System.Threading;
using AlienJust.Support.Concurrent;

namespace DrillingRig.ConfigApp.AppControl.Cycle {
	class CycleThreadHolderThreadSafe : ICycleThreadHolder {
		private readonly object _cyclePartsSync;
		private readonly List<ICyclePart> _cycleParts;
		private readonly SingleThreadedRelayQueueWorkerProceedAllItemsBeforeStopNoLog<Action> _backWorker; // TODO: use it to stop worker on app close

		public CycleThreadHolderThreadSafe() {
			// циклический опрос
			_cyclePartsSync = new object();
			_cycleParts = new List<ICyclePart>();
			_backWorker = new SingleThreadedRelayQueueWorkerProceedAllItemsBeforeStopNoLog<Action>("CycleBackWorker", a => a(), ThreadPriority.Lowest, true, null);
			_backWorker.AddWork(CycleWork);
		}

		private void CycleWork() {
			while (true) {
				lock (_cyclePartsSync) {
					foreach (var cyclePart in _cycleParts) {
						if (!cyclePart.Cancel) {
							try {
								cyclePart.InCycleAction();
								Thread.Sleep(20);
							}
							catch {
								Thread.Sleep(20);
							}
						}
						else Thread.Sleep(10);
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
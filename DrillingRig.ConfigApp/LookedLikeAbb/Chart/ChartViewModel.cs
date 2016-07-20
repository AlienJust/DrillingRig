using System.Collections.ObjectModel;
using Abt.Controls.SciChart;
using AlienJust.Support.ModelViewViewModel;
using RPD.SciChartControl;

namespace DrillingRig.ConfigApp.LookedLikeAbb.Chart {
	public class ChartViewModel : ViewModelBase {
		public ChartViewModel() {
			AnalogSeries = new ObservableCollection<IChartSeriesViewModel>();
			DiscreteSeries = new ObservableCollection<IChartSeriesViewModel>();

			AnalogSeriesAdditionalData = new ObservableCollection<ISeriesAdditionalData>();
			DiscreteSeriesAdditionalData = new ObservableCollection<ISeriesAdditionalData>();
		}

		public ObservableCollection<IChartSeriesViewModel> AnalogSeries { get; set; }
		public ObservableCollection<IChartSeriesViewModel> DiscreteSeries { get; set; }

		public ObservableCollection<ISeriesAdditionalData> AnalogSeriesAdditionalData { get; set; }
		public ObservableCollection<ISeriesAdditionalData> DiscreteSeriesAdditionalData { get; set; }
	}
}

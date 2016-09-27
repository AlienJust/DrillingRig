using System;
using DrillingRig.Commands.AinSettings;

namespace DrillingRig.ConfigApp.AppControl.AinSettingsWrite {
	public static class IainSettingsExtensions {
		public static void CompareSettingsAfterReReading(this IAinSettings settings, IAinSettings settingsReReaded, int zeroBasedAinNumber) {
			if (zeroBasedAinNumber == 0) {
				if (settingsReReaded.Ain1LinkFault) 
					throw new Exception("��� ��������� ������ (��� ������������� ������) ��������� ���� ��������, ������ ����� � ���1 ������������ (������� ���� ������ �����)");
			}
			else if (zeroBasedAinNumber == 1) {
				if (settingsReReaded.Ain2LinkFault)
					throw new Exception("��� ��������� ������ (��� ������������� ������) ��������� ���� ��������, ������ ����� � ���2 ������������ (������� ���� ������ �����)");
			}
			else if (zeroBasedAinNumber == 2) {
				if (settingsReReaded.Ain3LinkFault)
					throw new Exception("��� ��������� ������ (��� ������������� ������) ��������� ���� ��������, ������ ����� � ���3 ������������ (������� ���� ������ �����)");
			}

			if (settings.FiNom != settingsReReaded.FiNom) throw new Exception("��� ��������� ������ ���������� ��������� �� ������� � ������������� (�������� FiNom)");
			if (settings.Imax != settingsReReaded.Imax) throw new Exception("��� ��������� ������ ���������� ��������� �� ������� � ������������� (�������� Imax)");
			if (settings.UdcMax != settingsReReaded.UdcMax) throw new Exception("��� ��������� ������ ���������� ��������� �� ������� � ������������� (�������� UdcMax)");
			if (settings.UdcMin != settingsReReaded.UdcMin) throw new Exception("��� ��������� ������ ���������� ��������� �� ������� � ������������� (�������� UdcMin)");
			if (settings.Fnom != settingsReReaded.Fnom) throw new Exception("��� ��������� ������ ���������� ��������� �� ������� � ������������� (�������� Fnom)");
			if (settings.Fmax != settingsReReaded.Fmax) throw new Exception("��� ��������� ������ ���������� ��������� �� ������� � ������������� (�������� Fmax)");
			if (settings.DflLim != settingsReReaded.DflLim) throw new Exception("��� ��������� ������ ���������� ��������� �� ������� � ������������� (�������� DflLim)");
			if (settings.FlMinMin != settingsReReaded.FlMinMin) throw new Exception("��� ��������� ������ ���������� ��������� �� ������� � ������������� (�������� FlMinMin)");
			if (settings.IoutMax != settingsReReaded.IoutMax) throw new Exception("��� ��������� ������ ���������� ��������� �� ������� � ������������� (�������� IoutMax)");
			if (settings.FiMin != settingsReReaded.FiMin) throw new Exception("��� ��������� ������ ���������� ��������� �� ������� � ������������� (�������� FiMin)");
			if (settings.DacCh != settingsReReaded.DacCh) throw new Exception("��� ��������� ������ ���������� ��������� �� ������� � ������������� (�������� DacCh)");
			if (settings.Imcw != settingsReReaded.Imcw) throw new Exception("��� ��������� ������ ���������� ��������� �� ������� � ������������� (�������� Imcw)");
			if (settings.Ia0 != settingsReReaded.Ia0) throw new Exception("��� ��������� ������ ���������� ��������� �� ������� � ������������� (�������� Ia0), ������ ��������: " + settingsReReaded.Ia0 + ", � ���������: " + settings.Ia0);
			if (settings.Ib0 != settingsReReaded.Ib0) throw new Exception("��� ��������� ������ ���������� ��������� �� ������� � ������������� (�������� Ib0), ������ ��������: " + settingsReReaded.Ib0 + ", � ���������: " + settings.Ib0);
			if (settings.Ic0 != settingsReReaded.Ic0) throw new Exception("��� ��������� ������ ���������� ��������� �� ������� � ������������� (�������� Ic0), ������ ��������: " + settingsReReaded.Ic0 + ", � ���������: " + settings.Ic0);
			if (settings.Udc0 != settingsReReaded.Udc0) throw new Exception("��� ��������� ������ ���������� ��������� �� ������� � ������������� (�������� Udc0), ������ ��������: " + settingsReReaded.Udc0 + ", � ���������: " + settings.Udc0);
			if (settings.TauR != settingsReReaded.TauR) throw new Exception("��� ��������� ������ ���������� ��������� �� ������� � ������������� (�������� TauR)");
			if (settings.Lm != settingsReReaded.Lm) throw new Exception("��� ��������� ������ ���������� ��������� �� ������� � ������������� (�������� Lm)");
			if (settings.Lsl != settingsReReaded.Lsl) throw new Exception("��� ��������� ������ ���������� ��������� �� ������� � ������������� (�������� Lsl)");
			if (settings.Lrl != settingsReReaded.Lrl) throw new Exception("��� ��������� ������ ���������� ��������� �� ������� � ������������� (�������� Lrl)");
			if (settings.KpFi != settingsReReaded.KpFi) throw new Exception("��� ��������� ������ ���������� ��������� �� ������� � ������������� (�������� KpFi)");
			if (settings.KiFi != settingsReReaded.KiFi) throw new Exception("��� ��������� ������ ���������� ��������� �� ������� � ������������� (�������� KiFi)");
			if (settings.KpId != settingsReReaded.KpId) throw new Exception("��� ��������� ������ ���������� ��������� �� ������� � ������������� (�������� KpId)");
			if (settings.KiId != settingsReReaded.KiId) throw new Exception("��� ��������� ������ ���������� ��������� �� ������� � ������������� (�������� KiId)");
			if (settings.KpIq != settingsReReaded.KpIq) throw new Exception("��� ��������� ������ ���������� ��������� �� ������� � ������������� (�������� KpIq)");
			if (settings.KiIq != settingsReReaded.KiIq) throw new Exception("��� ��������� ������ ���������� ��������� �� ������� � ������������� (�������� KiIq)");
			if (settings.AccDfDt != settingsReReaded.AccDfDt) throw new Exception("��� ��������� ������ ���������� ��������� �� ������� � ������������� (�������� AccDfDt)");
			if (settings.DecDfDt != settingsReReaded.DecDfDt) throw new Exception("��� ��������� ������ ���������� ��������� �� ������� � ������������� (�������� DecDfDt)");
			if (settings.Unom != settingsReReaded.Unom) throw new Exception("��� ��������� ������ ���������� ��������� �� ������� � ������������� (�������� Unom)");
			if (settings.TauFlLim != settingsReReaded.TauFlLim) throw new Exception("��� ��������� ������ ���������� ��������� �� ������� � ������������� (�������� TauFlLim)");
			if (settings.Rs != settingsReReaded.Rs) throw new Exception("��� ��������� ������ ���������� ��������� �� ������� � ������������� (�������� Rs)");
			if (settings.Fmin != settingsReReaded.Fmin) throw new Exception("��� ��������� ������ ���������� ��������� �� ������� � ������������� (�������� Fmin)");
			if (settings.TauM != settingsReReaded.TauM) throw new Exception("��� ��������� ������ ���������� ��������� �� ������� � ������������� (�������� TauM)");
			if (settings.TauF != settingsReReaded.TauF) throw new Exception("��� ��������� ������ ���������� ��������� �� ������� � ������������� (�������� TauF)");
			if (settings.TauFSet != settingsReReaded.TauFSet) throw new Exception("��� ��������� ������ ���������� ��������� �� ������� � ������������� (�������� TauFSet)");
			if (settings.TauFi != settingsReReaded.TauFi) throw new Exception("��� ��������� ������ ���������� ��������� �� ������� � ������������� (�������� TauFi)");
			if (settings.IdSetMin != settingsReReaded.IdSetMin) throw new Exception("��� ��������� ������ ���������� ��������� �� ������� � ������������� (�������� IdSetMin)");
			if (settings.IdSetMax != settingsReReaded.IdSetMax) throw new Exception("��� ��������� ������ ���������� ��������� �� ������� � ������������� (�������� IdSetMax)");
			if (settings.UchMin != settingsReReaded.UchMin) throw new Exception("��� ��������� ������ ���������� ��������� �� ������� � ������������� (�������� UchMin)");
			if (settings.UchMax != settingsReReaded.UchMax) throw new Exception("��� ��������� ������ ���������� ��������� �� ������� � ������������� (�������� UchMax)");

			if (settings.Np != settingsReReaded.Np) throw new Exception("��� ��������� ������ ���������� ��������� �� ������� � ������������� (�������� Np)");
			if (settings.NimpFloorCode != settingsReReaded.NimpFloorCode) throw new Exception("��� ��������� ������ ���������� ��������� �� ������� � ������������� (�������� NimpFloorCode)");
			if (settings.FanMode != settingsReReaded.FanMode) throw new Exception("��� ��������� ������ ���������� ��������� �� ������� � ������������� (�������� FanMode)");

			if (settings.UmodThr != settingsReReaded.UmodThr) throw new Exception("��� ��������� ������ ���������� ��������� �� ������� � ������������� (�������� UmodThr)");
			if (settings.EmdecDfdt != settingsReReaded.EmdecDfdt) throw new Exception("��� ��������� ������ ���������� ��������� �� ������� � ������������� (�������� EmdecDfdt)");
			if (settings.TextMax != settingsReReaded.TextMax) throw new Exception("��� ��������� ������ ���������� ��������� �� ������� � ������������� (�������� TextMax)");
			if (settings.ToHl != settingsReReaded.ToHl) throw new Exception("��� ��������� ������ ���������� ��������� �� ������� � ������������� (�������� ToHl)");
		}
	}
}
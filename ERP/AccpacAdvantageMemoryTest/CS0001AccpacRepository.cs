using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccpacAdvantageMemoryTest
{
    public class CS0001AccpacRepository : BaseAccpacRepository
    {
        private const string VIEW_NAME = "CS0001";
        private const string APP_ID = "XX";
        private const string PROGRAM_NAME = "XX1000";
        private const string APP_VERSION = "62A";
        private readonly bool OUTPOUT_LOG = false;

        public CS0001AccpacRepository(bool outputLog)
        {
            OUTPOUT_LOG = outputLog;
        }
        public void InitializeSession()
        {
            InitializeSession(APP_ID, PROGRAM_NAME, APP_VERSION);
        }

        public void OpenView()
        {
            OpenView(VIEW_NAME);
        }

        public override void ReadLinkData()
        {

            short periods, qtr4Period, quarter;
            bool bActive, bRet;
            string year;
            DateTime startDate, endDate;

            ACCPAC.Advantage.Company compInfo = _mDBLinkCmpRW.Company;
            WriteLog(compInfo.Name + " " + compInfo.HomeCurrency + " " + compInfo.LegalName + "\r\n");

            WriteLog("Sage 300 Version: " + _session.ACCPACVersion + " build version: " + _session.ACCPACVersionBuild + " app: " + _session.AppID + "app ver: " + _session.AppVersion + "\r\n");
            WriteLog("user id: " + _session.UserID + " language: " + _session.UserLanguage + "\r\n");

            ACCPAC.Advantage.FiscalCalendar fiscCal = _mDBLinkCmpRW.FiscalCalendar;

            bRet = fiscCal.GetYear("2019", out periods, out qtr4Period, out bActive);
            WriteLog("2019 periods = " + periods.ToString() + " qtrPeriod = " + qtr4Period.ToString() + "\r\n");
            bRet = fiscCal.GetYearDates("2019", out startDate, out endDate);
            WriteLog("2019 start/end date = " + startDate.ToString() + " " + endDate.ToString() + "\r\n");

            bRet = fiscCal.GetFirstYear(out year, out periods, out qtr4Period, out bActive);
            WriteLog("First year = " + year + " periods: " + periods.ToString() + " qtrPeriod = " + qtr4Period.ToString() + "\r\n");
            bRet = fiscCal.GetLastYear(out year, out periods, out qtr4Period, out bActive);
            WriteLog("Last year = " + year + " periods: " + periods.ToString() + " qtrPeriod = " + qtr4Period.ToString() + "\r\n");

            bRet = fiscCal.GetPeriod(new DateTime(2019, 5, 23), out periods, out year, out bActive);
            WriteLog("Fisc infor for 2019/05/23 = " + year + " period: " + periods.ToString() + " open: " + bActive.ToString() + "\r\n");
            bRet = fiscCal.GetPeriodDates("2019", 5, out startDate, out endDate, out bActive);
            WriteLog("Period Dates for 2019/5 = " + startDate.ToString() + " to: " + endDate.ToString() + " open: " + bActive.ToString() + "\r\n");

            bRet = fiscCal.GetQuarter("2019", 5, out quarter);
            WriteLog("Quarter for 2019/5 = " + quarter.ToString() + "\r\n");
            bRet = fiscCal.GetQuarterDates("2019", 3, out startDate, out endDate);
            WriteLog("2019 Quarter 3 start/end dates: " + startDate.ToString() + " " + endDate.ToString() + "\r\n");

            bRet = fiscCal.DatesFromPeriod(5, ACCPAC.Advantage.FiscalPeriodType.Monthly, 1, new DateTime(2019, 1, 1), out startDate, out endDate);
            WriteLog("Period dates for 2019/5: " + startDate.ToString() + " " + endDate.ToString() + "\r\n");
            bRet = fiscCal.DateToPeriod(new DateTime(2019, 5, 1), ACCPAC.Advantage.FiscalPeriodType.Monthly, 1, new DateTime(2019, 1, 1), out periods);
            WriteLog("Period for 2019/5/1: " + periods.ToString() + "\r\n");

            ACCPAC.Advantage.Currency curInfo = _mDBLinkCmpRW.GetCurrency("CAD");

            WriteLog("DBLink currency: " + curInfo.Code + " " + curInfo.Description + " " + curInfo.Symbol + "\r\n");
            WriteLog(curInfo.Decimals.ToString() + " " + curInfo.ThousandSeparator.ToString() + " " + curInfo.NegativeDisplay.ToString() + "\r\n");

            WriteLog("Num active apps: " + _mDBLinkCmpRW.ActiveApplications.Count.ToString() + "\r\n");
            ACCPAC.Advantage.ActiveApplication appInfo = _mDBLinkCmpRW.ActiveApplications[3];
            WriteLog("app id: " + appInfo.AppID + " version: " + appInfo.AppVersion + " sequence: " + appInfo.Sequence + "\r\n");
            WriteLog("data leve: " + appInfo.DataLevel.ToString() + " installed: " + appInfo.IsInstalled.ToString() + "\r\n");

            ACCPAC.Advantage.CurrencyTable curTab = _mDBLinkCmpRW.GetCurrencyTable("USD", "SP");
            WriteLog("Currency Table: " + curTab.CurrencyCode + " " + curTab.RateType + " Desc: " + curTab.Description + " Source: " + curTab.SourceOfRates + "\r\n");

            string rateTypeDesc;
            _mDBLinkCmpRW.GetCurrencyRateTypeDescription("SP", out rateTypeDesc);
            WriteLog("SP Rate Type Description: " + rateTypeDesc + "\r\n");

            System.DateTime rateDate = new DateTime(2019, 10, 15);
            ACCPAC.Advantage.CurrencyRate curRate = _mDBLinkCmpRW.GetCurrencyRate("USD", "SP", "CAD", rateDate);
            WriteLog(curRate.HomeCurrency + " " + curRate.RateType + " " + curRate.SourceCurrency + " " + curRate.Rate + "\r\n");
        }

        public override void ReadViewData()
        {
            OpenView();
            _csView.Fetch(false);
            string tmpStr = _csView.Fields.FieldByID(1).Value + "  " + _csView.Fields.FieldByID(2).Value;
            WriteLog(tmpStr + "\r\n");
        }

        private void WriteLog(string info)
        {
            if (OUTPOUT_LOG)
            {
                Console.WriteLine(info);
            }
        }
    }
}

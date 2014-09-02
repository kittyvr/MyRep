using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccpacAdvantageMemoryTest
{
    public abstract class BaseAccpacRepository : IAccpacRepository
    {
        public ACCPAC.Advantage.Session _session;
        public ACCPAC.Advantage.DBLink _mDBLinkCmpRW;
        public ACCPAC.Advantage.View _csView;

        public virtual void InitializeSession(string appID, string programName, string appVersion)
        {
            _session = new ACCPAC.Advantage.Session();
            _session.Init("", appID, programName, appVersion);
            _session.Open("ADMIN", "ADMIN", "SAMLTD", DateTime.Today, 0);
        }

        public virtual void OpenDbLink()
        {
            _mDBLinkCmpRW = _session.OpenDBLink(ACCPAC.Advantage.DBLinkType.Company, ACCPAC.Advantage.DBLinkFlags.ReadWrite);

        }
        public virtual void OpenView(string viewName)
        {
            _csView = _mDBLinkCmpRW.OpenView(viewName);

        }

        public abstract void ReadLinkData();

        public abstract void ReadViewData();

        public virtual void Dispose()
        {
            _csView.Dispose();
            _mDBLinkCmpRW.Dispose();
            _session.Dispose();
        }
    }
}

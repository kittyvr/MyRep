using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccpacAdvantageMemoryTest
{
    public interface IAccpacRepository : IDisposable
    {
        void InitializeSession(string appID, string programName, string appVersion);
        void OpenDbLink();
        void OpenView(string viewName);
        void ReadLinkData();
        void ReadViewData();
    }
}

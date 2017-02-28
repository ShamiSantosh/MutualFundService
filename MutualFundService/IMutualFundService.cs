using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace MutualFundService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IMutualFundService" in both code and config file together.
    [ServiceContract]
    public interface IMutualFundService
    {
        [OperationContract]
        DataTable GetMutualFundData();
    }
}

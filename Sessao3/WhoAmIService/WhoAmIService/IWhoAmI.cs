﻿using System.ServiceModel;

namespace WhoAmIService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IWhoAmI" in both code and config file together.
    [ServiceContract]
    public interface IWhoAmI
    {
        [OperationContract]
        string Get();
    }
}

using System;

namespace Ampl.Web.Mvc.SecondaryLogon
{
    public interface ISecondaryLogonManager<TKey> where TKey : IEquatable<TKey>
    {
        void LogonAs(TKey userID);
        
        void LogonAs(TKey userID, string previousUrl);
        
        ISecondaryLogonResult LogonAsPreviousUser();
    }
}
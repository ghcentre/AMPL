namespace Ampl.Web.Mvc.SecondaryLogon
{
  public interface ISecondaryLogonResult
  {
    bool Success { get; }
    string PreviousUrl { get; }
  }
}

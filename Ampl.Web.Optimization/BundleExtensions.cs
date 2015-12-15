using System.Web.Optimization;

namespace Ampl.Web.Optimization
{
  public static class BundleExtensions
  {
    public static Bundle ForceOrder(this Bundle bundle)
    {
      bundle.Orderer = new AsIsBundleOrderer();
      return bundle;
    }
  }
}

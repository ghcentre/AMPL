using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ampl.System;

namespace Ampl.Core.Tests
{
  [TestFixture]
  public class DisposableExtensions_Tests
  {
    internal class DisposableClass : IDisposable
    {
      public string Property => "StringValue";

      public bool Disposed { get; private set; } = false;

      public void Dispose()
      {
        Disposed = true;
      }
    }

    [Test]
    public void Use_ThisNull_ReturnsDefault()
    {
      Stream stream = null;
      int result = stream.Use(x => x.ReadByte());
      Assert.That(result, Is.EqualTo(default(int)));
    }

    [Test]
    public void Use_NullAction_Throws()
    {
      // arrange
      Stream stream = new MemoryStream();
      // act-assert
      Assert.Throws<ArgumentNullException>(() => stream.Use<Stream, int>(null));
    }

    [Test]
    public void Use_AccessProperty_Returns()
    {
      // arrange-act
      string result = new DisposableClass().Use(x => x.Property);
      // assert
      Assert.That(result, Is.EqualTo("StringValue"));
    }

    [Test]
    public void Use_Use_Disposes()
    {
      // arrange
      var disposable = new DisposableClass();
      Assert.That(disposable.Disposed, Is.False);
      // act
      var result = disposable.Use(x => x.Property);
      // assert
      Assert.That(disposable.Disposed, Is.True);
    }

  }
}

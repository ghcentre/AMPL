using System.Runtime.CompilerServices;

namespace Ampl.Core
{
    /// <summary>
    /// <para>The <b>Ampl.Core</b> namespace contains classes and extension methods most commonly used.</para>
    /// <list type="table">
    ///     <listheader>
    ///         <term>Class</term>
    ///         <term>Typical Usage</term>
    ///     </listheader>
    ///     <item>
    ///         <term><see cref="StringExtensions"/></term>
    ///         <term><code><![CDATA[
    ///             string body = html.Between("<body>", "</body>");
    ///             body = body.RemoveAllBetween("<!--", "-->");
    ///             body = body.RemoveHtmlTags();
    ///             
    ///             string values = "1,2,3,oops,-5,null,42";
    ///             string validInts = values.Split(',').Select(x => x.ToNullableInt()).Where(x => x.HasValue).Select(x => x.Value).JoinWith(",");
    ///         ]]></code></term>
    ///     </item>
    ///     <item>
    ///         <term><see cref="Guard"/></term>
    ///         <term><code><![CDATA[
    ///             public class SomeClass
    ///             {
    ///                 private IEnumerable<string> _recipients;
    ///                 private string _greeting
    ///                 public int SomeMethod(IEnumerable<string> recipients, string greeting)
    ///                 {
    ///                     _recipients = Guard.Against.Null(recipients, nameof(recipients));
    ///                     _greeting = Guard.Against.NullOrWhiteSpace(greeting, nameof(greeting));
    ///                 }
    ///             }
    ///         ]]></code></term>
    ///     </item>
    ///     <item>
    ///         <term><see cref="EnumerableExtensions"/></term>
    ///         <term><code><![CDATA[string s = GetIds().JoinWith(", ");]]></code>
    ///         </term>
    ///     </item>
    ///     <item>
    ///         <term><see cref="CompactGuid"/> and <see cref="GuidExtensions"/></term>
    ///         <term>
    ///             <code><![CDATA[
    ///                 string id = Guid.NewGuid().ToCompactString();
    ///                 // ...
    ///                 var guid = CompactGuid.Parse(id);
    ///             ]]></code>
    ///         </term>
    ///     </item>
    ///     <item>
    ///         <term><see cref="Enums"/></term>
    ///         <term><code><![CDATA[var result = Enums.Parse<TestEnum>(arg);]]></code></term>
    ///     </item>
    ///     <item>
    ///         <term><see cref="DictionaryExtensions"/></term>
    ///         <term>
    ///             <code><![CDATA[
    ///                 var anonymousObject = new { id = 42, title = "This is a test" };
    ///                 var dic = new Dictionary<string, object>().IncludeObjectProperties(anonymousObject);
    ///             ]]></code></term>
    ///     </item>
    /// </list>
    /// </summary>
    [CompilerGenerated]
    class NamespaceDoc
    {
    }
}

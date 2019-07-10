using System;
using System.Threading.Tasks;

namespace Unosquare.Swan.Ldap.Samples
{
    public static class Program
    {
        /// <summary>
        /// Mains the specified arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <exception cref="SampleException"></exception>
        public static async Task Main(string[] args)
        {
            await TestLdapSearch();
            Terminal.Flush();
            "Enter any key to exit . . .".ReadKey();
        }

        private static async Task TestLdapSearch()
        {
            try
            {
                using (var cn = new LdapConnection())
                {
                    await cn.Connect("ldap.forumsys.com", 389);
                    await cn.Bind("uid=riemann,dc=example,dc=com", "password");
                    var lsc = await cn.Search("ou=scientists,dc=example,dc=com", LdapScope.ScopeSub);

                    while (lsc.HasMore())
                    {
                        var entry = lsc.Next();
                        var ldapAttributes = entry.GetAttributeSet();

                        $"{ldapAttributes["uniqueMember"]?.StringValue ?? string.Empty}".Info();
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Error(nameof(Main), "Error LDAP");
            }
        }
    }
}
using System.Linq;

using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;


namespace Simbrella.LoanManagement.TestHelpers.AutoFixture
{
    internal class AutoMoqDataAttribute : AutoDataAttribute
    {
        public AutoMoqDataAttribute(params ICustomization[] customizations)
            : base(() =>
            {
                var fixture = new Fixture().Customize(new AutoMoqCustomization { ConfigureMembers = true });

                return customizations.Aggregate(fixture, (current, customization) => current.Customize(customization));
            })
        {
        }
    }
}
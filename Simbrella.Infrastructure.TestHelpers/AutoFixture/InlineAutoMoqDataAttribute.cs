using System;

using AutoFixture;
using AutoFixture.Xunit2;


namespace Simbrella.Infrastructure.TestHelpers.AutoFixture
{
    public class InlineAutoMoqDataAttribute : InlineAutoDataAttribute
    {
        public InlineAutoMoqDataAttribute(Type customizationType, params object[] objects)
            : base(new AutoMoqDataAttribute(Activator.CreateInstance(customizationType) as ICustomization
                                            ?? throw new InvalidOperationException(nameof(AutoMoqDataAttribute))), objects)
        { }

        public InlineAutoMoqDataAttribute(params object[] objects) : base(new AutoMoqDataAttribute(), objects) { }
    }
}
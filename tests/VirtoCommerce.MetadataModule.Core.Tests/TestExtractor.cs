namespace FluentExtractor.Tests
{
    using FluentExtractor;
    using FluentExtractor.Tests.Models;

    using BooleanTests = Extractors.Boolean.BooleanExtractorsTests;
    using LengthTests = Extractors.Scalar.LengthExtractorsTests;

    public class TestExtractor : AbstractExtractor<TestModel>
    {
        public TestExtractor()
        {
            Length();
            MinLength();
            MaxLength();
            Required();
        }

        private void Length()
        {
            Projection(nameof(LengthTests.Length_FromLessZero_Exception), () =>
            {
                Configure(product => product.TestProperty)
                    .Length(-1, 20);
            });

            Projection(nameof(LengthTests.Length_FromGreaterTo_Exception), () =>
            {
                Configure(product => product.TestProperty)
                    .Length(50, 20);
            });

            Projection(nameof(LengthTests.Length_Returns_0_255), () =>
            {
                Configure(product => product.TestProperty)
                    .Length(0, 255);
            });

            Projection(nameof(LengthTests.Length_LocalOverride_Returns_10_20), () =>
            {
                Configure(product => product.TestProperty)
                    .Length(0, 255)
                    .Length(10, 20);
            });

            Projection(nameof(LengthTests.Length_ExtendedOverride_Returns_30_40), () =>
            {
                Configure(product => product.TestProperty)
                    .Length(0, 255);
            });
        }

        private void MinLength()
        {
            Projection(nameof(LengthTests.MinLength_Exception), () =>
            {
                Configure(product => product.TestProperty)
                    .MinLength(-1);
            });

            Projection(nameof(LengthTests.MinLength_Returns_60_Max), () =>
            {
                Configure(product => product.TestProperty)
                    .MinLength(60);
            });

            Projection(nameof(LengthTests.MinLength_LocalOverride_Returns_70_Max), () =>
            {
                Configure(product => product.TestProperty)
                    .MinLength(60)
                    .MinLength(70);
            });

            Projection(nameof(LengthTests.MinLength_ExtendedOverride_Returns_70_Max), () =>
            {
                Configure(product => product.TestProperty)
                    .MinLength(60);
            });
        }

        private void MaxLength()
        {
            Projection(nameof(LengthTests.MaxLength_Returns_0_60), () =>
            {
                Configure(product => product.TestProperty)
                    .MaxLength(60);
            });

            Projection(nameof(LengthTests.MaxLength_LocalOverride_Returns_0_70), () =>
            {
                Configure(product => product.TestProperty)
                    .MaxLength(60)
                    .MaxLength(70);
            });

            Projection(nameof(LengthTests.MaxLength_ExtendedOverride_Returns_0_70), () =>
            {
                Configure(product => product.TestProperty)
                    .MaxLength(60);
            });
        }

        private void Required()
        {
            Projection(nameof(BooleanTests.RequiredDescriptor), () =>
            {
                Configure(product => product.TestProperty)
                    .Required();
            });
        }

        // Order, DisplayName, KeyName, numbers of digits for double/float/decimal,
    }

    public class TestExtractorExtended : TestExtractor
    {
        public TestExtractorExtended()
        {
            Length();
            MinLength();
            MaxLength();
        }

        private void Length()
        {
            Projection(nameof(LengthTests.Length_ExtendedOverride_Returns_30_40), () =>
            {
                Configure(product => product.TestProperty)
                    .Length(30, 40);
            });
        }

        private void MinLength()
        {
            Projection(nameof(LengthTests.MinLength_ExtendedOverride_Returns_70_Max), () =>
            {
                Configure(product => product.TestProperty)
                    .MinLength(70);
            });
        }

        private void MaxLength()
        {
            Projection(nameof(LengthTests.MaxLength_ExtendedOverride_Returns_0_70), () =>
            {
                Configure(product => product.TestProperty)
                    .MaxLength(70);
            });
        }
    }
}

using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;
using System;
using Moq;
using FluentAssertions;

using Application.Interface;
using Application.ThirdPartyService.Interface;
using Application.Translator;

namespace Application.UnitTests
{
    public class TranslatorTest
    {
        private readonly Mock<IFunTranslation> _funTranslation;

        private readonly Mock<ITranslatorFactory> _translatorFactory;

        public TranslatorTest()
        {
            _funTranslation = new Mock<IFunTranslation>();

            _translatorFactory = new Mock<ITranslatorFactory>();
        }

        [SetUp]
        public void Setup()
        {
            _funTranslation.Setup(f => f.TranslateAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync("");
        }

        [Test]
        public void TestTranslatorFactory()
        {
            var translatorFactory = new TranslatorFactory(_funTranslation.Object);

            var translator = translatorFactory.GetITranslator("shakespeare");

            translator.Should().BeOfType(typeof(ShakespeareTranslator));

            translator = translatorFactory.GetITranslator("Yoda");

            translator.Should().BeOfType(typeof(YodaTranslator));

            FluentActions.Invoking(() => translatorFactory.GetITranslator(""))
                .Should()
                .Throw<ArgumentNullException>();

            FluentActions.Invoking(() => translatorFactory.GetITranslator("212"))
                .Should()
                .Throw<NotSupportedException>();
        }

        [Test]
        public async Task TestShakespeareTranslator()
        {
            _translatorFactory.Setup(t => t.GetITranslator(It.IsAny<string>())).Returns(new ShakespeareTranslator(_funTranslation.Object));

            var translator = _translatorFactory.Object.GetITranslator("shakespeare");

            var result = await translator.TranslateAsync("Hello, shakespear", new CancellationToken());

            result.Should().BeNullOrEmpty();
        }

        [Test]
        public async Task TestYodaTranslator()
        {
            _translatorFactory.Setup(t => t.GetITranslator(It.IsAny<string>())).Returns(new YodaTranslator(_funTranslation.Object));

            var translator = _translatorFactory.Object.GetITranslator("shakespeare");

            var result = await translator.TranslateAsync("Hello, shakespear", new CancellationToken());

            result.Should().BeNullOrEmpty();
        }
    }
}
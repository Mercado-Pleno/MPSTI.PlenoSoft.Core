using Microsoft.Extensions.Configuration;
using MPSTI.PlenoSoft.Core.Extensions.Static;

namespace MPSTI.PlenoSoft.Core.Test.Extensions.Static
{
	public class TestingConfigurations : IClassFixture<ConfigurationFixture>
	{
		private readonly IConfiguration _configuration;
		private readonly SmtpConfiguration _smtpConfiguration;

		public TestingConfigurations(ConfigurationFixture configurationFixture)
		{
			_configuration = configurationFixture.Configuration;
			_smtpConfiguration = _configuration.GetFromSection<SmtpConfiguration>("Smtp");
		}

		[Fact]
		public void QuandoInformaUmaChaveValida_DeveRetornarUmObjetoInstanciado() => _smtpConfiguration.Should().NotBeNull();

		[Fact]
		public void QuandoInformaUmaChaveValida_DeveRetornarHostPreenchido() => _smtpConfiguration.Host.Should().Be("smtp.gmail.com");

		[Fact]
		public void QuandoInformaUmaChaveValida_DeveRetornarPortPreenchido() => _smtpConfiguration.Port.Should().Be(587);

		[Fact]
		public void QuandoInformaUmaChaveValida_DeveRetornarUseSSLPreenchido() => _smtpConfiguration.UseSSL.Should().BeTrue();

		[Fact]
		public void QuandoInformaUmaChaveValida_DeveRetornarSmtpCredentialInstanciado() => _smtpConfiguration.SmtpCredential.Should().NotBeNull();

		[Fact]
		public void QuandoInformaUmaChaveValida_DeveRetornarUsernamePreenchido() => _smtpConfiguration.SmtpCredential.Username.Should().Be("bob");

		[Fact]
		public void QuandoInformaUmaChaveValida_DeveRetornarPasswordPreenchido() => _smtpConfiguration.SmtpCredential.Password.Should().Be("password");

		[Fact]
		public void QuandoInformaUmaChaveInvalidaComParametroNulo_DeveRetornarUmObjetoNulo()
		{
			var smtpConfiguration = _configuration.GetFromSection<SmtpConfiguration>("UmaChaveQueNaoExiste", null);
			smtpConfiguration.Should().BeNull();
		}

		[Fact]
		public void QuandoInformaUmaChaveInvalidaComParametroDefault_DeveRetornarUmObjetoDefault()
		{
			var smtpConfiguration = _configuration.GetFromSection("UmaChaveQueNaoExiste", new SmtpConfiguration { });
			smtpConfiguration.Should().NotBeNull();
			smtpConfiguration.SmtpCredential.Should().BeNull();
		}

		[Fact]
		public void QuandoInformaUmaChaveInvalidaSemParametro_DeveDispararExcecao()
		{
			Assert.Throws<InvalidOperationException>(() => _configuration.GetFromSection<SmtpConfiguration>("UmaChaveQueNaoExiste"));
		}
	}

	public class ConfigurationFixture : IDisposable
	{
		public readonly IConfiguration Configuration;
		public ConfigurationFixture()
		{
			var builder = new ConfigurationBuilder();
			builder.SetBasePath(Directory.GetCurrentDirectory());
			builder.AddJsonFile("Abstracao/appsettings.json", false, true);
			Configuration = builder.Build();
		}

		public void Dispose() { }
	}

	public class SmtpConfiguration
	{
		public string Host { get; set; }
		public int Port { get; set; }
		public bool UseSSL { get; set; }
		public SmtpCredential SmtpCredential { get; set; }
	}

	public class SmtpCredential
	{
		public string Username { get; set; }
		public string Password { get; set; }
	}
}
using MPSC.PlenoSoft.ControlFlux.Core;
using MPSC.PlenoSoft.ControlFlux.Parameters;

namespace MPSC.PlenoSoft.ControlFlux.Test.Core
{
	public class TestingFlux
	{
		[Fact]
		public void WhenCenario1_StatusTrue()
		{
			Flux.To(out var fluxArg, "Make Any Thing")
				.Do("With Lambda Expression To Action", fa => { /* Do Any Thing */ })
				.Do("And Add  Integer Number Parameter", fa => fa.AddParam("IntegerNumber", 1))
				.Validating("And Test Integer Number Parameter", fa => fa.Params.IntegerNumber == 1)
				.Do("And Add  DateTime Value Parameter", fa => fa.AddParam("DateTimeValue", DateTime.Today))
				.Validating("And Test DateTime Value Parameter", fa => fa.Params.DateTimeValue == DateTime.Today)
				.Do("And Add  a String Value Parameter", fa => fa.AddParam("StringValue", "AEIOU"))
				.Validating("And Test a String Value Parameter", fa => fa.Params.StringValue == "AEIOU")
				.Do("Return Success Message To Client", fa => fa.AddInformation("Ok"))
			;

			Assert.True(fluxArg.Status);
			Assert.Equal(1, fluxArg.Params.IntegerNumber);
			Assert.Equal(DateTime.Today, fluxArg.Params.DateTimeValue);
			Assert.Equal("AEIOU", fluxArg.Params.StringValue);
		}

		[Fact]
		public void WhenCenario2_StatusFalse()
		{
			Flux.To(out var fluxArg, "Obter Informacoes da pessoa")
				.Do("Informe o CPF", fa => fa.Params.CPF = 123456)
				.Do("Obter Pessoa Por CPF", ObterPessoaPorCPF)
				.Do("Obter Dependente Da Pessoa", ObterDependente)
				.Do("Obter Vendas da PessoaId", ObterVendasDaPessoaId)
				.Do("Aborta Missão", fa => fa.AddValidation("Aborta Missão"))
				.Do("Verificar se tá tudo ok", VerificarSeTaTudoOk)
			;

			Flux.With(fluxArg, "continue")
				.Do("Nothing", fa => fa.AddParam("IntValue", 11))
			;

			Assert.False(fluxArg.Status);
			Console.WriteLine(string.Join("\r\n", fluxArg.Messages));
		}

		private void ObterPessoaPorCPF(FluxArg fluxArg)
		{
			var cpf = (long)fluxArg.Params.CPF;
			fluxArg.Params.Titular = new Pessoa(cpf);
		}

		private void ObterDependente(FluxArg fluxArg)
		{
			var titular = fluxArg.Params.Titular as Pessoa;
			fluxArg.Params.Dependente = new Pessoa();
			fluxArg.Params.Dependente2 = new Pessoa();
		}

		private void ObterVendasDaPessoaId(FluxArg fluxArg)
		{
			var titular = fluxArg.Params.Titular as Pessoa;
			fluxArg.Params.Vendas = new[] { new Venda(), new Venda() };
		}

		private void VerificarSeTaTudoOk(FluxArg fluxArg)
		{
			var titular = fluxArg.Params.Titular as Pessoa;
			var dependente = fluxArg.Params.Dependente as Pessoa;
			var vendas = fluxArg.Params.Vendas as IEnumerable<Venda>;
		}
	}

	public class Pessoa
	{
		private long cpf;

		public Pessoa(long cpf = 0)
		{
			this.cpf = cpf;
		}
	}
	public class Venda { }
}
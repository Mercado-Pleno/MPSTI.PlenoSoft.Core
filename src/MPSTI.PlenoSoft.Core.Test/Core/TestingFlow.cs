using MPSTI.PlenoSoft.Core.Flux.Core;
using MPSTI.PlenoSoft.Core.Flux.Parameters;

namespace MPSTI.PlenoSoft.Core.Test.Core
{
	public class TestingFlow
	{
		[Fact]
		public void WhenCenario1_StatusTrue()
		{
			Flow.To(out var flowArg, "Make Any Thing")
				.Do("With Lambda Expression To Action", fa => { /* Do Any Thing */ })
				.Do("And Add  Integer Number Parameter", fa => fa.AddParam("IntegerNumber", 1))
				.Validating("And Test Integer Number Parameter", fa => fa.Params.IntegerNumber == 1)
				.Do("And Add  DateTime Value Parameter", fa => fa.AddParam("DateTimeValue", DateTime.Today))
				.Validating("And Test DateTime Value Parameter", fa => fa.Params.DateTimeValue == DateTime.Today)
				.Do("And Add  a String Value Parameter", fa => fa.AddParam("StringValue", "AEIOU"))
				.Validating("And Test a String Value Parameter", fa => fa.Params.StringValue == "AEIOU")
				.Do("Return Success Message To Client", fa => fa.AddInformation("Ok"))
			;

			Assert.True(flowArg.Status);
			Assert.Equal(1, flowArg.Params.IntegerNumber);
			Assert.Equal(DateTime.Today, flowArg.Params.DateTimeValue);
			Assert.Equal("AEIOU", flowArg.Params.StringValue);
		}

		[Fact]
		public void WhenCenario2_StatusFalse()
		{
			Flow.To(out var flowArg, "Obter Informacoes da pessoa")
				.Do("Informe o CPF", fa => fa.Params.CPF = 123456)
				.Do("Obter Pessoa Por CPF", ObterPessoaPorCPF)
				.Do("Obter Dependente Da Pessoa", ObterDependente)
				.Do("Obter Vendas da PessoaId", ObterVendasDaPessoaId)
				.Do("Aborta Missão", fa => fa.AddValidation("Aborta Missão"))
				.Do("Verificar se tá tudo ok", VerificarSeTaTudoOk)
			;

			Flow.With(flowArg, "continue")
				.Do("Nothing", fa => fa.AddParam("IntValue", 11))
			;

			Assert.False(flowArg.Status);
			Console.WriteLine(string.Join("\r\n", flowArg.Messages));
		}

		private void ObterPessoaPorCPF(FlowArg flowArg)
		{
			var cpf = (long)flowArg.Params.CPF;
			flowArg.Params.Titular = new Pessoa(cpf);
		}

		private void ObterDependente(FlowArg flowArg)
		{
			var titular = flowArg.Params.Titular as Pessoa;
			flowArg.Params.Dependente = new Pessoa();
			flowArg.Params.Dependente2 = new Pessoa();
		}

		private void ObterVendasDaPessoaId(FlowArg flowArg)
		{
			var titular = flowArg.Params.Titular as Pessoa;
			flowArg.Params.Vendas = new[] { new Venda(), new Venda() };
		}

		private void VerificarSeTaTudoOk(FlowArg flowArg)
		{
			var titular = flowArg.Params.Titular as Pessoa;
			var dependente = flowArg.Params.Dependente as Pessoa;
			var vendas = flowArg.Params.Vendas as IEnumerable<Venda>;
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
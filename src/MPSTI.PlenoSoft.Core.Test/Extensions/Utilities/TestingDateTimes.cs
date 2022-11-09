using MPSTI.PlenoSoft.Core.Extensions.Utilities;
using System.Threading.Tasks;

namespace MPSTI.PlenoSoft.Core.Test.Extensions.Utilities
{
	public class TestingDateTimes
	{
		[Fact]
		public void AoChamarDateTimeNowToIsoString_DeveRetornarStringNaoNulo() => DateTime.Now.ToIsoString().Should().NotBeNull();

		[Fact]
		public void AoChamarDateTimeUtcNowToIsoString_DeveRetornarStringNaoNulo() => DateTime.UtcNow.ToIsoString().Should().NotBeNull();

		[Fact]
		public void AoChamarToIsoStringDoUtcDoLocal_DeveRetornarStringsIguais()
		{
			var utcDateTime = DateTime.UtcNow;
			var locDateTime = utcDateTime.ToLocalTime();

			utcDateTime.Kind.Should().Be(DateTimeKind.Utc);
			locDateTime.Kind.Should().Be(DateTimeKind.Local);

			locDateTime.ToUniversalTime().Should().Be(utcDateTime.ToUniversalTime());
			locDateTime.ToLocalTime().Should().Be(utcDateTime.ToLocalTime());

			locDateTime.ToUniversalTime().Should().Be(utcDateTime);
			locDateTime.Should().Be(utcDateTime.ToLocalTime());

			var utcDateTimeIsoString = utcDateTime.ToIsoString();
			var locDateTimeIsoString = locDateTime.ToIsoString();

			utcDateTimeIsoString.Should().Be(locDateTimeIsoString);
		}

		[Fact]
		public void AoChamarToIsoStringDoUtcDoLocalNullables_DeveRetornarStringsIguais()
		{
			DateTime? utcDateTime = DateTime.UtcNow;
			DateTime? locDateTime = utcDateTime?.ToLocalTime();

			utcDateTime?.Kind.Should().Be(DateTimeKind.Utc);
			locDateTime?.Kind.Should().Be(DateTimeKind.Local);

			locDateTime?.ToUniversalTime().Should().Be(utcDateTime?.ToUniversalTime());
			locDateTime?.ToLocalTime().Should().Be(utcDateTime?.ToLocalTime());

			locDateTime?.ToUniversalTime().Should().Be(utcDateTime);
			locDateTime.Should().Be(utcDateTime?.ToLocalTime());

			var utcDateTimeIsoString = utcDateTime.ToIsoString();
			var locDateTimeIsoString = locDateTime.ToIsoString();

			utcDateTimeIsoString.Should().Be(locDateTimeIsoString);
		}

		[Fact]
		public void AoChamarToIsoStringDeUmDateTimeNulo_DeveRetornarUmaStringNula()
		{
			DateTime? dateTime = null;

			dateTime.Should().BeNull();
			dateTime.ToIsoString().Should().BeNull();
		}
	}
}
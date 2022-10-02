using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.Json;

namespace PadiScanner.Infra.Converters;

public class ProbabilitiesConverter : ValueConverter<Dictionary<string, double>, string>
{
	public ProbabilitiesConverter() : base(
        v => JsonSerializer.Serialize(v, JsonSerializerOptions.Default),
		v => JsonSerializer.Deserialize<Dictionary<string, double>>(v, JsonSerializerOptions.Default))
	{
	}
}

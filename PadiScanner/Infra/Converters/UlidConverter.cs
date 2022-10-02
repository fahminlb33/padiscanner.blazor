using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace PadiScanner.Infra.Converters;

public class UlidConverter : ValueConverter<Ulid, string>
{
    public UlidConverter() : base(v => v.ToString(), v => Ulid.Parse(v)) { }
}
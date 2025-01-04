using FluentMigrator;
using FluentMigrator.SqlServer;

namespace BoBit.Fetcher.Migrations
{
    [Migration(20250103)]
    public class InitData : Migration
    {
        public override void Up()
        {
            Create.Table("BitcoinPrices")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("Created").AsDateTimeOffset().NotNullable()
                .WithColumn("Timestamp").AsDateTimeOffset().NotNullable()
                .WithColumn("CryptoCurrency").AsFixedLengthString(32).NotNullable()
                .WithColumn("FiatCurrency").AsFixedLengthString(32).NotNullable()
                .WithColumn("Price").AsDecimal(18, 8).NotNullable();

            Create.Index("IX_BitcoinPrices_Timestamp")
                .OnTable("BitcoinPrices")
                .OnColumn("Timestamp").Descending()
                .Include("Price")
                .Include("FiatCurrency")
                .Include("CryptoCurrency");
        
        }

        public override void Down()
        {
            Delete.Index("IX_BitcoinPrices_Timestamp");
            Delete.Table("BitcoinPrices");
        }
    }
}

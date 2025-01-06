export interface BitcoinPricesResponse {
    maxPrice: number;
    avgPrice: number;
    from: string;
    to: string;
    cryptoCurrency: string;
    fiatCurrency: string;
    labels: string[];
    data: number[];
}
import { Component } from '@angular/core';
import { BitcoinApiClientService } from '../services/bitcoin-api-client.service';
import { Observable, Subscription } from 'rxjs';
import { BitcoinPricesResponse } from '../interfaces/bitcoinPricesResponse';
import { NgxEchartsDirective, provideEchartsCore } from 'ngx-echarts';
import * as echarts from 'echarts/core';
import { BarChart, LineChart } from 'echarts/charts';
import { GridComponent, TitleComponent, TooltipComponent } from 'echarts/components';
import { CanvasRenderer } from 'echarts/renderers';
import { EChartsCoreOption, ECharts } from 'echarts/core';
import { CurrencyPipe } from '@angular/common';
echarts.use([BarChart, GridComponent, CanvasRenderer, LineChart, TitleComponent, TooltipComponent]);

@Component({
  selector: 'app-chart-side',
  imports: [NgxEchartsDirective, CurrencyPipe],
  templateUrl: './chart-side.component.html',
  styleUrl: './chart-side.component.css',
  providers: [provideEchartsCore({ echarts })]
})
export class ChartSideComponent {
  options: EChartsCoreOption|null = null;
  bitcoinSubscription?: Subscription;
  chartMerge: any = {};
  chartInstance: ECharts|null = null;

  avgPrice = 0;
  maxPrice = 0;
  crypto = '';
  fiat = '';
  

  constructor(
    private api: BitcoinApiClientService
  ) {
    
  }

  ngOnInit() {
    let it = this;
    this.bitcoinSubscription = this.api.pollBitcoinPrices()
      .subscribe((x) => it.onData(x))
    
    this.options = {
      title: {
        text: 'Bitcoin Price',
        left: 'center',
      },
      tooltip: {
        trigger: 'axis',
      },
      xAxis: {
        type: 'category',
        data: ["a", "b"],
      },
      yAxis: {
        type: 'value',
      },
      series: [
        {
          name: 'Price',
          type: 'line',
          data: [1, 2],
        },
      ],
    };
  }


  onChartInit(ec: ECharts) {
    this.chartInstance = ec;
  }

  onData(bitcoinData: BitcoinPricesResponse | never[]) {
    let a = bitcoinData as BitcoinPricesResponse;
    if (a) {
      this.avgPrice = a.avgPrice;
      this.maxPrice = a.maxPrice;
      this.crypto = a.cryptoCurrency;
      this.fiat = a.fiatCurrency;
      this.chartMerge = {
        xAxis: {
          data: a.labels,
        },
        series: [
          {
            data: a.data,
          },
        ],
      };

      //this.chartInstance?.setOption(this.chartMerge, false);
    }
    
  }

  ngOnDestroy() {
    this.bitcoinSubscription?.unsubscribe();
  }
}

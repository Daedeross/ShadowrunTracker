import { Component } from '@angular/core';
import { SignalrService } from './services/signalr.service';
import { HttpClient } from '@angular/common/http';
import { ChartConfiguration, ChartType } from 'chart.js';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.less']
})
export class AppComponent {
  title = 'ShadowrunTrackerNg';
  chartOptions: ChartConfiguration['options'] = {
    responsive: true,
    scales: {
      y: {
        min: 0
      }
    }
  };

  chartLabels: string[] = ['Real time data for the chart'];
  chartType: ChartType = 'bar';
  chartLegend: boolean = true;

  constructor(public signalRService: SignalrService, private http: HttpClient) { }

  ngOnInit(): void {
    this.signalRService.startConnection();
    this.signalRService.addTransferChartDataListener();
    this.signalRService.addBroadcastChartDataListener(); 
    this.startHttpRequest();
  }

  public chartClicked = (event) => {
    console.log(event);
    this.signalRService.broadcastChartData();
  }

  private startHttpRequest = () => {
    this.http.get('https://localhost:5001/api/chart')
      .subscribe(res => {
        console.log(res);
      });
  }
}

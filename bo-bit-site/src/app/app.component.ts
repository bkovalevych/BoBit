import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { ChartSideComponent } from './chart-side/chart-side.component';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, ChartSideComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'bo-bit-site';
}

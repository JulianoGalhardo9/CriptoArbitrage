import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css'
})
export class DashboardComponent {
  // A variável DEVE estar aqui dentro das chaves da classe
  oportunidades = [
    { symbol: 'BTCUSDT', priceBinance: 65000, priceBitget: 65150, profit: 0.23 },
    { symbol: 'ETHUSDT', priceBinance: 3500, priceBitget: 3495, profit: -0.14 },
    { symbol: 'SOLUSDT', priceBinance: 145, priceBitget: 146.2, profit: 0.82 }
  ];
}
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ArbitrageService } from '../../services/arbitrage'; 

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css'
})
export class DashboardComponent implements OnInit {
  // Definimos que é um array de qualquer coisa por enquanto
  oportunidades: any[] = []; 

  constructor(private arbitrageService: ArbitrageService) {}

  ngOnInit(): void {
    this.atualizarDados();
    // Atualiza a cada 5 segundos
    setInterval(() => this.atualizarDados(), 5000);
  }

  atualizarDados(): void {
    // Tipamos o retorno para evitar o erro 7006
    this.arbitrageService.getOportunidades().subscribe({
      next: (data: any[]) => {
        this.oportunidades = data;
      },
      error: (err: any) => {
        console.error('Erro ao buscar dados da API:', err);
      }
    });
  }
}
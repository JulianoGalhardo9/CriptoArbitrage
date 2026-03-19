import { Component, signal } from '@angular/core';
import { DashboardComponent } from './components/dashboard/dashboard'; // Caminho ajustado

@Component({
  selector: 'app-root',
  standalone: true, // Garanta que o standalone está aqui se for Angular 18
  imports: [DashboardComponent],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected readonly title = signal('Monitor de Arbitragem de Criptomoedas');
}
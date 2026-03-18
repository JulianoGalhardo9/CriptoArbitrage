import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ArbitrageService {
  private apiUrl = 'http://localhost:8080/api/arbitrage'; // Ajuste para a sua rota real

  constructor(private http: HttpClient) { }

  // Método que busca as oportunidades do seu banco de dados
  getOportunidades(): Observable<any[]> {
    return this.http.get<any[]>(this.apiUrl);
  }
}
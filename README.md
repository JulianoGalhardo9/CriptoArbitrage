# 🚀 CryptoArbitrage — Monitor de Arbitragem em Tempo Real (.NET 9, Angular 18 & Docker)

---

## 🧭 Visão Geral

O **CryptoArbitrage** é uma plataforma Full Stack de alta performance projetada para monitorar oportunidades de arbitragem entre diferentes exchanges de criptomoedas. Desenvolvida com **.NET 9** no back-end e **Angular 18** no front-end, a aplicação rastreia variações de preços em tempo real, calculando instantaneamente o spread e o lucro potencial.

O projeto utiliza uma arquitetura robusta baseada em **Worker Services** para o monitoramento contínuo e segue os princípios de **Clean Architecture** e **DDD**, garantindo um sistema resiliente, escalável e totalmente containerizado com **Docker**.

---

## ⚙️ Funcionalidades e Arquitetura

### 🏗️ 1. Arquitetura Distribuída & DDD
* **Back-end:** Estrutura modular em camadas utilizando .NET 9, focada em performance e processamento assíncrono.
* **Front-end:** SPA (Single Page Application) desenvolvida com Angular 18, apresentando um Dashboard Dark Mode moderno e reativo.
* **Infraestrutura:** Ambiente totalmente orquestrado via **Docker Compose**, integrando API, Banco de Dados e Migrators.

---

### 🤖 2. Monitoramento Inteligente (Worker Service)
* **Background Processing:** Worker Service dedicado que consome APIs da **Binance** e **Bitget** simultaneamente.
* **Cálculo de Spread:** Algoritmo otimizado para identificar disparidades de preços e calcular a porcentagem de lucro em tempo real.
* **Persistência Seletiva:** Gravação automática de alertas de arbitragem no banco de dados para consulta histórica.

---

### 📊 3. Dashboard de Arbitragem Real-time
* **Interface Premium:** Design Dark Mode com foco em legibilidade de dados financeiros.
* **Atualização Automática:** Consumo dinâmico da API para refletir as últimas oportunidades sem necessidade de refresh manual.
* **Indicadores Visuais:** Destaque para ativos, preços em múltiplas exchanges e indicadores de lucro (Profit Percentage).

---

### 🛠️ 4. Persistência e Dockerização
* **Entity Framework Core:** Implementação do padrão Repository para acesso ao banco de dados MariaDB/MySQL.
* **Docker & Docker Compose:** Containerização completa da aplicação, facilitando o deploy e a replicação do ambiente de desenvolvimento.
* **Migrations Automatizadas:** Serviço de migração integrado ao ciclo de vida do Docker para garantir a integridade do esquema de dados.

---

### 🌐 5. Comunicação e Documentação
* **Swagger UI:** Documentação interativa da API (OpenAPI) para validação de endpoints e payloads.
* **Políticas de CORS:** Configuração rigorosa para permitir a comunicação segura entre o domínio do Angular e a API no Docker.
* **REST API:** Endpoints otimizados para entrega de grandes volumes de dados JSON.

---

## 🧰 Tecnologias Utilizadas

### **Back-end**
* **C# / .NET 9** (Última versão LTS)
* **Entity Framework Core** (ORM)
* **MariaDB / MySQL** (Banco de dados relacional)
* **Worker Services** (Processamento em segundo plano)
* **Serilog** (Logging estruturado)
* **Swagger / UI** (Documentação)

### **Front-end**
* **Angular 18**
* **TypeScript**
* **RxJS** (Programação Reativa para streams de dados)
* **CSS3 Moderno** (Dashboard responsivo e estilização Dark Mode)
* **Zone.js** (Gerenciamento de detecção de mudanças)

---

## 🧠 Conceitos Principais Dominados

* Desenvolvimento **Full Stack** com foco em sistemas de alta disponibilidade.
* Orquestração de microserviços e bancos de dados utilizando **Docker**.
* Implementação de **Background Tasks** (Worker Services) para consumo de APIs externas.
* Arquitetura de software baseada em camadas (Domain, Application, Infrastructure, API).
* Gerenciamento de políticas de **CORS** e segurança de rede em ambientes containerizados.
* Consumo de APIs financeiras de alta frequência (Exchanges de Cripto).

---

## 🏁 Conclusão

O **CryptoArbitrage** reflete o domínio de tecnologias de ponta para o mercado financeiro digital. Ao unir o processamento paralelo do .NET 9 com a reatividade do Angular, o projeto demonstra a capacidade de construir ferramentas capazes de lidar com dados em tempo real e infraestruturas modernas de deploy, estando pronto para desafios complexos de engenharia de software.

# 🛡️ VIGILANT
SISTEMA INTELIGENTE DE GESTÃO DE RISCOS 

O projeto VIGILANT é uma solução de software desenvolvida para modernizar e tornar proativa a gestão de riscos na linha de produção do Complexo Ayrton Senna da Renault, no Paraná (Brasil). A plataforma utiliza a Inteligência Artificial (IA) para transformar processos manuais e burocráticos em um sistema digital, preditivo e intuitivo.

---

## Indice

* [ Problema que Endereçamos](#Problema)
* [Core concepts](#core-concepts)
* [Example](#example)
* [Design & extensibility](#design--extensibility)
* [Roadmap](#roadmap)
* [License](#license)
* [Special Thanks](#special-thanks)

---

## Inicio Rápido

Clone o repositório e execute o exemplo incluído, e em seguida siga as instruções abaixo:

```bash
git clone https://github.com/karga-rs/karga.git
cd karga
cargo run --example http
```

That example demonstrates measuring request latency and success (HTTP 200) using a simple executor. Replace the action with any async closure to exercise custom code (Kafka producer, filesystem workload, or any I/O you want).

---

##  Problema 

A gestão de riscos na Renault era baseada em processos manuais e reativos, resultando em:

    Ineficiência: Lentidão no registro e acompanhamento de riscos.

    Falta de Visibilidade: Dificuldade em avaliar a criticidade de um risco e priorizar ações.

    Impacto na Segurança: Maior incidência de erros e acidentes evitáveis.
    

> **A Solução: Vigilant - Gestão Proativa:** O Vigilant é uma plataforma modular desenhada para ser o centro de controle da segurança industrial, alinhando a Renault aos conceitos da Indústria 4.0.

## Design & extensibility

  * **Serde-like core** — karga focuses on representing the *what* (scenarios, metrics) and not the *how*. Implementations live in separate crates.
  * **Generic-first API** — heavy use of traits and generics to make composing components ergonomic and zero-cost where possible.
  * **Closure-driven actions** — define workloads as simple async closures so users can embed arbitrary logic without boilerplate.
  * **Composable pipelines** — metrics flow from actions → aggregates → reports. Each stage is pluggable.

## Recursos-Chave

Módulo	Descrição	Valor
Análise Preditiva (IA)	O diferencial central. Utiliza IA para analisar dados históricos e prever riscos futuros, sugerindo soluções antes que incidentes ocorram.	Transforma a segurança de reativa para proativa.
Dashboard Centralizado	Painel principal com indicadores de segurança (KPIs) em tempo real, fornecendo visão instantânea do status da produção.	Melhora a tomada de decisão estratégica.
Gestão de Riscos Intuitiva	Interface simplificada para registro de riscos por qualquer colaborador, atribuindo prioridade e status de forma fácil.	Aumenta a adesão e o empoderamento dos colaboradores.
Monitoramento Integrado	Permite o cadastro e monitoramento de equipamentos, com emissão de alertas e notificações automáticas em caso de falhas.	Garante resposta rápida e eficiente.

## License

`karga` is MIT-licensed — see the `LICENSE` file in the repository.

---

## Equipe do Projeto

Este projeto foi desenvolvido por estudantes do SENAI, com a participação de:
Nome do Componente	Instituição de Origem
David Lima	SENAI SEDE, Camaçari, BA
Misla Brito	SENAI SEDE, Camaçari, BA
Orlando Lucas	SENAI SEDE, Camaçari, BA
Tiago Andrade	SENAI SEDE, Camaçari, BA
Yuri Silva	SENAI SEDE, Camaçari, BA



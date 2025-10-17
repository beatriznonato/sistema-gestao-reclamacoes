# Requisitos do Sistema

## 📝 Contexto

O banco recebe reclamações de clientes por dois canais distintos:

- **Digital**: Sites, aplicativos e APIs
- **Físico**: Documentos digitalizados (PDF, imagens)

Essas reclamações precisam ser padronizadas, classificadas e exibidas em um portal interno, além de serem enviadas para um sistema legado.

**Desafio**: Automatizar e centralizar esse fluxo, mantendo a rastreabilidade e o cumprimento do SLA de 10 dias úteis.

---

## ✅ Requisitos Funcionais

> O que o sistema deve fazer

- Receber reclamações de dois canais (digital e físico)
- Padronizar o formato dos dados recebidos
- Extrair informações principais (dados do reclamante, tipo de demanda, anexos)
- Classificar automaticamente o tipo de reclamação
- Exibir as informações no portal interno
- Enviar as reclamações processadas para um sistema legado
- Permitir consulta ao histórico do cliente e aos anexos

---

## 🛡️ Requisitos Não Funcionais

> Como o sistema deve se comportar

- Suportar até 1.000 novas reclamações por dia
- Garantir rastreabilidade completa de cada reclamação
- Garantir disponibilidade e performance no processamento
- Permitir alertas automáticos quando o prazo de 10 dias estiver se aproximando
- Assegurar integridade e segurança dos dados (dados bancários e pessoais)
- Facilitar manutenção e evolução futura

---

## 📊 Restrições

### Volume

- **Média diária**: 1.000 novas reclamações
- **Total mensal**: ~30.000 reclamações

### Canais de Entrada

- **Digital** e **Físico**: Fluxos independentes na entrada, mas unificados após processamento inicial

### SLA

- **Prazo**: 10 dias corridos para tratamento completo da reclamação

### Picos Esperados

- Períodos de instabilidade de serviço
- Datas específicas (final de mês, campanhas)
- Eventos de alto volume

### Medidas Preventivas

- **Processamento contínuo**: Evitar gargalos no sistema
- **Mecanismos de priorização**: Classificar por urgência (fraude > produto > atendimento)
- **Alertas automáticos**: Notificações aos 8, 9 e 10 dias

---

## 🎯 Decisões de Arquitetura

Para atender aos requisitos acima, foram implementados:

- **Filas SQS**: Absorvem picos de demanda
- **Auto Scaling**: Ajusta recursos automaticamente (2-6 instâncias)
- **Amazon Textract**: OCR com IA para canal físico
- **Cache Redis**: Reduz latência em 80% das consultas
- **Circuit Breaker**: Proteção na integração com legado
- **CloudWatch + SNS**: Monitoramento e alertas progressivos

---

Consulte o arquivo [respostas-case.md](./respostas-case.md) para detalhes de como cada requisito foi atendido.

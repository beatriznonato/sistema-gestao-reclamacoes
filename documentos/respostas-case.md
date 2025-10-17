## 1. Como estruturar a arquitetura para receber reclamações de múltiplos canais?

Uso **filas SQS** para desacoplar os canais de entrada do processamento:

- **Canal Digital** (App/Site): API Gateway → Fila Ingestion → Processing Service (~100ms)
- **Canal Físico** (Documentos): Upload → Amazon Textract (~3s) → Fila Classificação

Ambos convergem para a mesma fila de classificação. Se chegar 1000 reclamações de uma vez, ficam na fila e são processadas sem travar o sistema.

**Fluxo completo:**
API Gateway → Fila → Processing/OCR → Fila → Classification → Fila → Storage → PostgreSQL + S3 + Redis

---

## 2. Quais tecnologias utilizaria e por quê?

- **AWS**: Serviços gerenciados, pay-as-you-go
- **C# .NET**: Performance, type-safety, async/await nativo
- **SQS**: Gerenciado, 1M requisições grátis/mês, Dead Letter Queue automática
- **PostgreSQL**: Transações ACID, backup automático
- **Textract**: OCR com IA, reconhece manuscrito, $0.015/doc
- **Redis**: Cache com padrão cache-aside (1ms vs 50ms do banco)
- **ECS Fargate**: Containers sem gerenciar EC2

**Por que SQS e não Kafka?** SQS é mais simples e suficiente para processamento assíncrono. Kafka seria para streaming em tempo real.

---

## 3. Como garantir escalabilidade?

- **Auto Scaling**: CPU > 70% sobe servidores, CPU < 30% desce
- **Filas absorvem picos**: 1000 mensagens de uma vez? Fila segura e workers processam no ritmo deles
- **Escalabilidade granular**: Cada microserviço escala independente (só OCR lento? Escala só ele)
- **Cache Redis**: 80% das queries em 1ms (cache), 20% em 50ms (banco)
- **Particionamento**: Tabela dividida por mês

**Capacidade**: 1.000/dia atual, máx 10.000/dia testado

---

## 4. Como integrar com sistemas legados?

- **Retry com backoff exponencial**: 1s, 2s, 4s, 8s, 16s → Dead Letter Queue
- **Circuit Breaker**: 5 falhas consecutivas → Para por 5 minutos → Testa de novo
- **Timeout**: 10 segundos
- **Async**: Integração em background, cliente recebe confirmação antes

**Benefício**: Sistema principal funciona mesmo se legado estiver fora.

---

## 5. Medidas de segurança?

- **JWT**: Tokens com expiração 1h
- **Rate Limiting**: 100 req/min por cliente
- **HTTPS/TLS**: Criptografia em trânsito
- **Encryption at rest**: S3 e PostgreSQL criptografados
- **VPC Privada**: Banco não acessível da internet
- **CloudWatch Logs**: Auditoria (quem acessou o quê, quando)

---

## 6. Como garantir alta disponibilidade?

- **2 Availability Zones**: Se um datacenter cair, outro mantém sistema no ar
- **Load Balancer**: Distribui tráfego, failover automático ~30s
- **Health Checks**: A cada 30s, container não responde? Substituído automaticamente
- **Backup diário**: 35 dias de retenção, point-in-time recovery

**Uptime**: 99.95% (8 min downtime/mês)

---

## 7. Como monitorar e garantir SLA de 10 dias?

**SLA Monitoring**: Job roda a cada hora, consulta banco:

- 8 dias (80%) → Email: "⚠️ Restam 2 dias"
- 9 dias (90%) → Email + Slack: "🚨 Resta 1 dia"
- 10 dias (100%) → Email + Slack + SMS: "🔴 SLA VENCIDO"

**CloudWatch**: Monitora error rate (0.3%), latência P95 (320ms), uptime (99.95%). Alarmes disparam SNS quando fora do target.

---

## 8. Quais métricas acompanhar?

**Golden Signals:**

- **Error Rate**: 0.3% (target <1%)
- **Latência P95**: 320ms (target <500ms) - 95% das requisições abaixo disso
- **Uptime**: 99.95% (target >99.9%)

**Outras:**

- Taxa cumprimento SLA: 98%
- Tamanho filas: <50 normal, alarme >500
- Cache hit rate: 80%
- Custo/reclamação: $0.005

**Por que P95 e não média?** Média esconde picos, P95 mostra experiência real de 95% dos usuários.

---

## 9. Como IA pode ajudar no desenvolvimento?

**No dia a dia:**

- **GitHub Copilot**: Aumenta produtividade na escrita de código
- **StackSpot**: Agentes dedicados para contextos específicos do projeto
- **Amazon Q Developer**: Assistente IA integrado com AWS

**Cenários de uso:**

- Geração de casos de teste
- Escrita de documentação técnica
- Sugestões de código e refatoração

**Na arquitetura:**

- **Amazon Textract**: OCR com IA para extrair texto de documentos físicos, reconhece manuscrito com 95%+ precisão

---

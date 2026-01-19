# Notifications API

> Microserviço responsável por **processar e orquestrar notificações** de forma assíncrona usando **RabbitMQ + MassTransit**, seguindo boas práticas de arquitetura, isolamento de responsabilidades e escalabilidade.


---

## 🎯 Objetivo

Este serviço consome eventos publicados por outros domínios do sistema (ex: **Users** e **Payments**) e executa ações de notificação, como:

* Envio de **boas‑vindas** para novos usuários
* Notificação de **resultado de pagamento** (aprovado / recusado)

Tudo de forma **assíncrona**, desacoplada e resiliente.

---

## 🧠 Visão Geral da Arquitetura

* **.NET 8**
* **Minimal APIs**
* **MassTransit** como abstraction layer
* **RabbitMQ** como message broker
* Comunicação **event‑driven**
* Consumers isolados por tipo de mensagem
* Configuração centralizada via `appsettings`

Arquitetura simples, direta e fácil de escalar horizontalmente.

---

## 📦 Principais Componentes

### 🔹 Consumers

| Consumer                        | Responsabilidade                                                           |
| ------------------------------- | -------------------------------------------------------------------------- |
| `WelcomeCustomerConsumer`       | Processa evento de criação de usuário e dispara notificação de boas‑vindas |
| `PaymentProcessedEventConsumer` | Processa evento de pagamento e notifica o usuário com o resultado          |

---

### 🔹 Mensagens (Eventos)

Eventos consumidos via RabbitMQ:

* `WelcomeCustomerMessage`
* `PaymentProcessedMessage`

Cada mensagem representa **um fato do domínio**, não uma ação imperativa.

---

### 🔹 Configuração do RabbitMQ

A configuração é feita via **binding fortemente tipado**:

```json
"RabbitSettings": {
  "HostName": "localhost",
  "QueueName": "welcome_customer_queue",
  "QueueNamePaymentProcessedEvent": "payment_processed_queue",
  "StartConsumer": true
}
```

---

## ⚙️ Inicialização do Serviço

O bootstrap do serviço acontece no `Program.cs`:

* Swagger habilitado em ambiente de desenvolvimento
* Registro de consumers via extensão
* Inicialização opcional do consumidor via flag (`StartConsumer`)

```csharp
builder.Services.AddRabbitMq(builder.Configuration);
```

---

## 🐳 Docker

O projeto possui **Dockerfile multi‑stage**, preparado para:

* Build otimizado
* Imagem final enxuta
* Execução em ambiente containerizado

```bash
docker build -t notifications-service .
docker run -p 8080:8080 notifications-service
```

Pronto pra subir em qualquer cloud.

---

## 🧪 Observabilidade

Atualmente o serviço utiliza:

* `ILogger<T>`
* Logs estruturados por consumer

Pronto para integração com:

* OpenTelemetry
* ELK / Grafana / Azure Monitor

---

## 🧩 Extensibilidade

Este serviço foi pensado para crescer sem virar bagunça:

* Novos eventos = novos consumers
* Zero impacto nos produtores
* Fácil adição de novos canais (email, push, sms)

Single Responsibility aplicada de verdade.

---

## 🚫 O que **não** está aqui (de propósito)

* ❌ Credenciais
* ❌ Tokens
* ❌ Secrets
* ❌ Regras de negócio sensíveis

Tudo isso fica fora do repositório.

---

## 📌 Requisitos

* .NET SDK 8+
* RabbitMQ
* Docker (opcional)

---

## 🧠 Filosofia

> Eventos não pedem permissão. Eles só acontecem.

Esse serviço existe pra **reagir rápido**, **sem acoplamento** e **sem gambiarra**.

---

## 📄 Licença

Uso educacional e demonstrativo.

---

**Feito com foco em arquitetura, clareza e escalabilidade.**

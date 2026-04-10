# Atividades_ProjOO

---

## PadroesProjeto-a

### Exercício prático

Uma empresa deseja desenvolver um sistema capaz de enviar notificações por diferentes canais, como e-mail, SMS e push notification. O sistema deve ser flexível para permitir a inclusão de novos tipos de notificação no futuro, sem alterar excessivamente o código existente.

Além disso, a aplicação deve possuir um único componente central responsável por armazenar configurações globais do sistema, como nome da aplicação, servidor de envio e quantidade máxima de tentativas de reenvio.

### Objetivo

Implementar um pequeno sistema orientado a objetos que:

- [x] Utilize o padrão **Factory** para criar objetos de notificação
- [x] Utilize o padrão **Singleton** para garantir uma única instância de configuração global
- [x] Demonstre separação de responsabilidades e facilidade de extensão
- [x] Inclua testes de unidade para as principais funcionalidades

---

## PadroesProjeto-b

### Evolução da solução: Adapter e Proxy

Após implementar Factory e Singleton, evolua o sistema para novos requisitos:

#### Adapter

Integrar serviços externos ou legados com interfaces incompatíveis.

> **Exemplo:** Adaptar uma API externa de SMS incompatível com o método `send()`.

- [x] Implementar o padrão Adapter

#### Proxy

Intermediar acesso, adicionando controle, validação e logs.

> **Exemplo:** Validar permissões, registrar logs e limitar tentativas de envio.

- [x] Implementar o padrão Proxy

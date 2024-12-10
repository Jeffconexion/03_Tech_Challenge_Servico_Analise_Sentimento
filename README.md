# Tech Challenger - Serviço de Análise de sentimento

# Introdução

Imagine que a nossa aplicação cresça e outras pessoas tenha acesso a lista de contatos, seria interessante mantermos em nossa lista contatos que venham ter uma boa impressão, que forneça ótimos serviços, seja um contato que agregue valor.

# Análise de Sentimento

- **Função**: Este serviço consome feedbacks para análise de sentimentos, processando as mensagens para identificar sentimentos (positivo, negativo, neutro) e insights adicionais.
- **Processo**:
  - Consome mensagens da `fila-sentimento`.
  - Aplica um modelo de Machine Learning ou uma API de processamento de linguagem natural (NLP) para classificar o sentimento das mensagens.
  - Armazena o resultado da análise, incluindo o ID do contato, o texto original e o sentimento detectado, em uma tabela dedicada ou em um banco de dados de análise.
- **Tecnologias**: Bibliotecas de ML (como ML.NET ou integração com APIs de NLP como Azure Cognitive Services), armazenamento em um banco de dados para análises futuras, e uso de relatórios ou painéis para visualização.

# Tecnologias Utilizadas:

- **.NET 8**: Framework para construção da Minimal API.
- **C#**: Linguagem de programação usada no desenvolvimento do projeto.
- **RabbitMQ**: Broker para o gerenciamento das mensagens.
- **WorkerService**: Broker para o gerenciamento das mensagens.

# Documentação

- [Documentação da API](https://horse-neon-79c.notion.site/Documenta-o-da-API-04183b890d7c47cb89af4445d01d6678?pvs=4)
- [Documentação de Estilo para C#](https://horse-neon-79c.notion.site/Documenta-o-de-Estilo-para-C-de62b229fd01436a96f7a090b4d11e27?pvs=4)
- [Documentação dos Testes](https://horse-neon-79c.notion.site/Documenta-o-dos-Testes-a402a32a16a24b1b925dab83201e7d19?pvs=4)
- [Documentação de Banco de Dados](https://horse-neon-79c.notion.site/Documenta-o-de-Banco-de-Dados-6ba60c4c8533491a9d28da71f6b57c93?pvs=4)
- [Guia de Estrutura do Projeto](https://horse-neon-79c.notion.site/Guia-de-Estrutura-do-Projeto-fbfbc24c616d456bb56306cfda2c0bc9?pvs=4)

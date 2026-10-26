# ChatSeguro

Chat cliente-servidor em C# com mensagens cifradas, assinaturas digitais e autenticação segura.

Projeto de grupo (3 alunos) da unidade curricular de **Tópicos de Segurança**, TeSP em Programação de Sistemas de Informação, Politécnico de Leiria (2025/26). Desenvolvido em duas fases, com defesa. Todos os elementos participaram em todas as fases.

![Interface do chat](docs/chat.png)

## O que faz

- Vários clientes ligados ao mesmo servidor em simultâneo (sockets TCP, uma thread por cliente)
- Registo e login de utilizadores
- Mensagens para todos ou privadas para um utilizador
- Lista de utilizadores online atualizada em tempo real
- Registo de auditoria de todos os eventos no servidor

## Como funciona a segurança

| Problema | Solução |
|---|---|
| A chave de cifra não pode viajar em claro | **Handshake RSA**: o cliente envia a sua chave pública; o servidor gera uma chave AES só para essa sessão e envia-a cifrada com RSA |
| Ninguém pode ler as mensagens na rede | **AES por sessão**: cada cliente tem a sua chave. O servidor decifra com a chave de quem envia e volta a cifrar com a de quem recebe |
| Garantir quem enviou e que nada foi alterado | **Assinatura digital RSA** em cada mensagem, verificada pelo servidor antes de a reencaminhar. Se falhar, a mensagem é descartada e fica no log |
| Passwords guardadas em segurança | **Salt aleatório de 16 bytes + SHA-256**. Regras mínimas: 8 caracteres, uma maiúscula e um número |
| Saber o que aconteceu no servidor | **Log de auditoria** com data, tipo de evento e detalhe, com escrita protegida por `lock` |

## Tecnologias

C# · .NET Framework 4.8 · Windows Forms (cliente) · Aplicação de consola (servidor) · Sockets TCP · ProtoIP · `System.Security.Cryptography` (RSA, AES, SHA-256)

## Como executar

1. Abrir `FichaPratica3.sln` no Visual Studio 2022
2. Build → Build Solution
3. Iniciar primeiro o projeto **Server** (porta 10000)
4. Iniciar uma ou mais instâncias do **Client** e registar utilizadores

## O que melhoraria

- Gerar um IV (vetor de inicialização) aleatório por mensagem em vez de um valor fixo
- Guardar os utilizadores numa base de dados em vez de ficheiro de texto
- Trocar SHA-256 por um algoritmo próprio para passwords (PBKDF2 ou bcrypt)

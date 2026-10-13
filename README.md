# 01 · Pomodoro — o começo

O menor pedaço do Pomodoro que já funciona: **iniciar, pausar, retomar, finalizar e ver quanto falta**, com foco, pausa curta e pausa longa configuráveis.
É por aqui que o projeto completo começa, e é o que a gente constrói no vídeo.

## Como rodar

Precisa do [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0).

```bash
cd PomodoroApi/PomodoroApi
dotnet run
```

Abra **http://localhost:5019/swagger** e teste cada endpoint com o botão **Try it out**.

### O front (React)

Precisa do [Node.js](https://nodejs.org) 20+. Com a API rodando, em outro terminal:

```bash
cd frontend
npm install
npm run dev
```

Abra a URL que o Vite mostrar (normalmente **http://localhost:5173**).

- O ciclo é automático: Foco → Pausa curta → Foco… até completar os focos configurados, quando vem a Pausa longa.
- Ao fim de cada sessão toca um som e, se a opção estiver ligada, a próxima já começa sozinha.
- A engrenagem (⚙) abre a configuração: minutos de cada tipo, focos até a pausa longa e início automático.

O relógio anda no navegador, mas o backend continua sendo a fonte da verdade: a tela busca a configuração ao abrir e, a cada ação (iniciar, pausar, retomar, finalizar), se sincroniza com o `segundosRestantes` que o servidor devolve. Nada fica no navegador.

## A API

| Ação | Endpoint |
|---|---|
| Iniciar (envia `{ "tipo": "Foco" }`; tipos: `Foco`, `PausaCurta`, `PausaLonga`) | `POST /sessoes/iniciar` |
| Quanto falta? | `GET /sessoes/{id}` |
| Pausar | `POST /sessoes/{id}/pausar` |
| Retomar | `POST /sessoes/{id}/retomar` |
| Finalizar (encerra antes do fim) | `POST /sessoes/{id}/finalizar` |
| Ler a configuração | `GET /configuracao` |
| Salvar a configuração (minutos de cada tipo, focos até a pausa longa, início automático) | `PUT /configuracao` |

Tudo fica em memória: ao reiniciar a API, as sessões somem e a configuração volta ao padrão (25 / 5 / 15 min, ciclo de 4 focos, início automático ligado).

## Como está organizado

```
PomodoroApi/
├── Controllers/     ← recebe o HTTP e responde
├── Services/        ← conduz cada ação: busca, manda a Sessao agir, guarda
├── Domain/          ← Sessao e Configuracao: as regras moram aqui
├── Infrastructure/  ← repositórios em memória
├── Contratos/       ← o que a API recebe e devolve
└── Program.cs       ← liga tudo

frontend/            ← tela em React (App.tsx, api.ts, index.css)
```

O **controller** recebe, o **serviço** conduz, a **entidade** decide, o **repositório** guarda.

## O que vem depois

O projeto completo (relatório do dia, front em React, testes, modelagem) está na pasta ao lado.
Mas tente fazer o seu primeiro, depois compare com o meu.

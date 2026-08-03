---
name: firecrawl-dev-docs
description: |
  Skill enxuta do Firecrawl para um agente de programação. Usada para
  consultar documentação de APIs e frameworks, e buscar discussões
  técnicas em fóruns (Stack Overflow, GitHub Issues, etc). Não cobre
  integração de produto, workflows de entregável ou fluxos de auth —
  apenas o essencial para pesquisa técnica ao vivo.
---

# Firecrawl — Pesquisa Técnica (Docs & Fórum)

Este agente usa o Firecrawl só para uma coisa: **buscar e ler
informação técnica na web** (documentação oficial de API/framework,
threads de fórum, issues de GitHub) durante a própria sessão.

## Instalação (uma vez)

```bash
npx -y firecrawl-cli@latest init --all --browser
```

Verifique se está funcionando:

```bash
firecrawl --status
```

Se não houver `FIRECRAWL_API_KEY` configurada, o comando acima já abre
o login no navegador. Sem chave, ainda dá pra usar o tier gratuito sem
login (mais lento, com limite de requisições) — não precisa fazer nada
extra, os comandos abaixo funcionam do mesmo jeito.

## Fluxo de decisão

Use sempre nesta ordem:

1. **Não sei a URL exata (nome de uma lib, um erro, um conceito)**
   → `firecrawl search`
2. **Já tenho a URL** (ex: doc oficial de um endpoint, uma issue
   específica do GitHub, uma página do Stack Overflow)
   → `firecrawl scrape`
3. **A página exige interação** (precisa clicar em "expandir", trocar
   de aba de versão, passar por um seletor de linguagem)
   → `firecrawl interact`
4. **É um arquivo local** (PDF/DOCX de uma spec baixada)
   → `firecrawl parse`
5. **Uma chamada falhou ou voltou algo estranho**
   → `firecrawl ask` passando o `jobId` que falhou

Não use `monitor`, `crawl`, `map` nem os fluxos de workflow/deliverable
— não fazem parte do escopo desse agente.

## Comandos

**Buscar (ponto de partida padrão):**
```bash
firecrawl search "nome da lib erro específico" -o .firecrawl/busca.md
```

**Scrape de uma página conhecida:**
```bash
firecrawl scrape "https://docs.exemplo.com/api/endpoint" -o .firecrawl/doc.md
```

**Página com interação (ex: seletor de versão):**
```bash
firecrawl interact "https://docs.exemplo.com/api" \
  --action "click:#version-selector" \
  -o .firecrawl/doc-interativo.md
```

**Documento local:**
```bash
firecrawl parse ./especificacao.pdf -o .firecrawl/spec.md
```

**Debug de falha:**
```bash
firecrawl ask --job-id <id-que-falhou>
```

## Regras práticas

- Sempre salvar o resultado em `.firecrawl/` com `-o`, pra manter
  rastro do que foi consultado na sessão.
- Preferir `search` quando não tiver certeza da URL — economiza
  tentativa e erro.
- Ao citar uma doc para o usuário, sempre linkar a URL de origem
  (não só resumir sem fonte).
- Se a mesma página precisar ser checada várias vezes ao longo do
  tempo (ex: acompanhar changelog de uma lib), isso foge do escopo
  desta skill — nesse caso avise que existe `firecrawl monitor`, mas
  não implemente aqui.

## Fora de escopo (não usar)

- Integração de Firecrawl em código de produto (SDK, chamadas de API
  dentro do app do usuário)
- Geração de entregáveis (relatórios de SEO, lead lists, clones de
  design)
- Fluxo completo de autenticação/CLI OAuth — só rode o install acima
  uma vez; se pedir login, é o próprio comando que cuida disso
# Firecrawl — Pesquisa Técnica

## Objetivo

Use Firecrawl apenas para buscar informações técnicas atualizadas:
- documentação oficial;
- APIs e frameworks;
- GitHub Issues;
- Stack Overflow.

Não use para pesquisas gerais.

## Regra principal

Antes de usar Firecrawl:

1. Tente responder com conhecimento próprio.
2. Use Firecrawl somente quando precisar:
   - confirmar uma API ou versão;
   - consultar documentação atualizada;
   - investigar um erro específico;
   - encontrar uma solução existente.

Evite chamadas desnecessárias.

## Fluxo

### Tenho URL confiável

Use scrape:

firecrawl scrape "URL" -o .firecrawl/result.md

Exemplos:
- documentação oficial;
- issue específica;
- página conhecida.

---

### Não tenho URL

Use search com uma consulta específica:

firecrawl search "erro ou tecnologia específica" -o .firecrawl/search.md

Prioridade:
1. documentação oficial;
2. GitHub oficial;
3. Stack Overflow.

Depois do search, faça scrape apenas da melhor fonte.

---

### Página precisa interação

Use interact somente se:
- precisa trocar versão;
- conteúdo está escondido;
- depende de cliques.

## Performance

- Não use Firecrawl se a resposta já for conhecida.
- Não faça múltiplas buscas para a mesma pergunta.
- Prefira uma fonte boa a várias fontes.
- Não use crawl, map ou monitor.
- Sempre salve resultados em `.firecrawl/`.
- Sempre mantenha a URL original da fonte.

## Falhas

Se falhar:
- tente novamente uma vez;
- use ask com jobId se disponível.

firecrawl ask --job-id <id>

## Fora de escopo

Não usar:
- crawl;
- map;
- monitor;
- workflows;
- geração de relatórios;
- integração Firecrawl em aplicações.
# softools

Esse projeto contem as ferramentas internas da Softeam. Ele possui ferramentas para facilitar os processos da Softeam, tais como geração de documentos a partir de templates, gestão de projetos, métricas dos membros, etc.

## Executando o projeto

> [!IMPORTANT]
> É necessário o Docker Compose configurado para executar o projeto.
### Dev
Para executar o projeto em modo de desenvolvimento, execute o seguinte comando:

```bash
docker compose -f docker-compose.dev.yml up
```

### Prod
Para executar o projeto em modo de produção, execute o seguinte comando:

```bash
docker compose -f docker-compose.prod.yml up
```

> [!NOTE]
> No servidor, caso hospedado localmente, é necessário o **avahi-daemon** para que ele possa ser acessado na rede cabeada da UFS através de `softserver.local`.

## Componentes
- Website pro frontend (`src/softools.website`)
- Microserviços para o backend (`src/Softools.Projetos`, `src/Softools.Auth`...)
- Banco de dados PostgreSQL
- RabbitMQ
- N8N

## Observações
- Caso o volume do postgres já exista num estado em que o N8N não esteja presente, basta criar o banco de dados que resolve
```sql
CREATE DATABASE n8n;
GRANT ALL PRIVILEGES ON DATABASE n8n TO postgres;
```


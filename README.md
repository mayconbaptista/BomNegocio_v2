# 🛒 E-commerce Microservices - BnShop

> Um sistema de e-commerce moderno baseado em **microserviços**, inspirado no eShop da Microsoft.

![Arquitetura](link-para-a-imagem-do-diagrama)

## 🚀 Tecnologias Utilizadas

| Tecnologia/Padrão         | Descrição                                   |
| ------------------------- | ------------------------------------------- |
| **.NET 8**                | Backend principal                          |
| **Carter**                | APIs Mínimas                               |
| **MassTransit + RabbitMQ**| Comunicação assíncrona                     |
| **CQRS + MediatR**        | Separação de leitura e escrita             |
| **DDD**                   | Modelagem de domínio                       |
| **Arquitetura Limpa**     | Organização do código                      |
| **Docker**                | Containerização                            |
| **Building Blocks**       | Código compartilhado entre serviços        |
| **Unit of Work**          | Gerenciamento de transações                |
| **BFF com Ocelot**        | API Gateway                                |
| **gRPC**                  | Comunicação eficiente entre serviços       |
| **EF Core**               | ORM para acesso a banco de dados           |
| **Mapster**               | Mapeamento de objetos DTO                  |
| **PostgreSQL e MySQL**    | Bancos de dados                            |

---

## 📂 **Arquitetura do Projeto**
O sistema é composto pelos seguintes **microserviços**:

- **🛍️ Shopping.Web**: Frontend da aplicação.
- **🔑 Auth Service**: Autenticação e autorização com JWT.
- **📦 Catalog Service**: Gerenciamento de produtos e categorias.
- **🛒 Cart Service**: Armazena os itens do carrinho de compras.
- **📜 Ordering Service**: Processamento de pedidos e pagamentos.

### 📌 **Diagrama da Arquitetura**
![Diagrama](link-para-imagem-do-diagrama)

---

## ⚙️ **Como Executar o Projeto**

### 🚧 **Pré-requisitos**
Certifique-se de ter instalado:
- [Visual Staudio](https://visualstudio.microsoft.com/pt-br/))
- [Dbeaver](https://dbeaver.io/)
- [Docker Desktop](https://www.docker.com/)

### ▶ **Passo a Passo**
1️⃣ **Clone o repositório**  
```sh
git clone https://github.com/mayconbaptista/BomNegocio_v2.git
```

2️⃣ **Abra a solução no Visual Studio**  

Rode a aplicação com perfil docker compose

3️⃣ **Conectese ao banco de dados com o Dbeaver**

- **Postgres**
  - **Host**: localhost
  - **Port**: 5432
  - **Database**:catalog
  - **User**: catalog
  - **Password**: catalog123

- **MySQL**
  - **Host**: localhost
  - **Port**: 3308
  - **Database**: catalog.db
  - **User**: catalog
  - **Password**: P@ssw0rd2022
  - **Database**: catalog

3️⃣ **Populando o catalogo**
```sql
INSERT INTO `catalog`.categoria
	(id, nome, descricao, categoria_pai)
	VALUES('3fa85f64-5717-4562-b3fc-2c963f66afa6', 'Roupas e acessorios', 'Roupas e acessorios em geral', null);

INSERT INTO `catalog`.produto (id, nome, codigo_sku, descricao, preco, quantidade, categoria_id) VALUES
('1e18bfd4-88a4-44fe-8804-e13dcaadd139', 'Manfinity Homme Camisa Polo Slim Fit', 'SKU810', 'Manfinity Homme Camisa Polo Slim Fit de Manga Curta com Estampa Listrada para Uso Casual e Trabalho', 125.00, 30, '3fa85f64-5717-4562-b3fc-2c963f66afa6'),
('c7bc103f-81ea-4af6-a25b-e5038c562d20', 'Manfinity Homme Camisa Polo Esportiva', 'SKU811', 'Manfinity Homme Camisa Polo Masculina com Tecnologia Dry Fit, Ideal para Atividades Esportivas', 130.00, 18, '3fa85f64-5717-4562-b3fc-2c963f66afa6'),
('0132385e-45a2-4e14-b3d5-8f4e448c4e7c', 'Manfinity Homme Camisa Polo Clássica', 'SKU812', 'Manfinity Homme Camisa Polo Masculina Clássica em Algodão Premium com Corte Tradicional', 110.00, 9, '3fa85f64-5717-4562-b3fc-2c963f66afa6'),
('48de137d-8c9e-431f-8478-9aaa9743a377', 'Manfinity Homme Camisa Polo Casual', 'SKU813', 'Manfinity Homme Camisa Polo Masculina com Estampa Sutil e Tecido Confortável para Uso Diário', 115.00, 25, '3fa85f64-5717-4562-b3fc-2c963f66afa6'),
('ec6ad78d-c005-4beb-a2bc-7359aee2c91d', 'Manfinity Homme Camisa Polo com Zíper', 'SKU814', 'Manfinity Homme Camisa Polo com Gola com Zíper, Design Moderno e Tecido Respirável', 135.00, 25, '3fa85f64-5717-4562-b3fc-2c963f66afa6'),
('302253fa-23f8-491c-aabd-26ba01c787e4', 'Manfinity Homme Camisa Polo Estampada', 'SKU815', 'Manfinity Homme Camisa Polo Masculina com Estampa Floral Moderna e Tecido Leve', 128.00, 21, '3fa85f64-5717-4562-b3fc-2c963f66afa6'),
('bcc50aee-135b-47df-9356-daa004c87961', 'Manfinity Homme Camisa Polo Listrada', 'SKU816', 'Manfinity Homme Camisa Polo Masculina Listrada em Algodão Premium para Visual Casual Sofisticado', 122.00, 29, '3fa85f64-5717-4562-b3fc-2c963f66afa6'),
('f7f53f98-08fd-4356-935b-7aa1bcc15b29', 'Manfinity Homme Camisa Polo de Linho', 'SKU817', 'Manfinity Homme Camisa Polo Masculina de Linho para Máximo Conforto em Dias Quentes', 140.00, 21, '3fa85f64-5717-4562-b3fc-2c963f66afa6'),
('1615c2de-9943-452f-a613-7493efde92ae', 'Manfinity Homme Camisa Polo Social', 'SKU818', 'Manfinity Homme Camisa Polo Social Masculina com Design Elegante para Ambientes Profissionais e Eventos', 145.00, 27, '3fa85f64-5717-4562-b3fc-2c963f66afa6'),
('dac1c5af-e070-4082-bab9-78c809511e98', 'Manfinity Homme Camisa Polo Premium', 'SKU819', 'Manfinity Homme Camisa Polo Masculina Premium com Tecido de Alta Qualidade e Acabamento Refinado', 150.00, 14, '3fa85f64-5717-4562-b3fc-2c963f66afa6');
```

4️⃣ **Testando a API no Postman**

Para facilitar os testes da API, disponibilizamos uma coleção do **Postman**.

📥 **Baixe a coleção aqui:**  
[👉 Coleção Postman](https://drive.google.com/file/d/1H_d5KJSrrq_29GXND5aJUdOB7h1OElDJ/view?usp=drive_link)

### 🚀 Como Importar no Postman:
1. Baixe o arquivo `.json`
2. No Postman, vá em **File > Import**
3. Selecione o arquivo exportado
4. Agora você pode testar as requisições da API diretamente no Postman!

5️⃣ **Acesse o Frontend da Aplicação**

Você pode testar via o front end da aplicação, acesse o link do repositório abaixo:
[🌐 BnShop](https://github.com/mayconbaptista/bom-negocio_web/tree/master)





// viewer-request do behavior padrao (Angular).
// Rotas do client-side router (/produtos/42) nao existem no S3: reescreve para
// /index.html. Arquivos com extensao (main-HASH.js, favicon.ico) passam direto.
// Feito aqui, e nao com custom_error_response, para nao mascarar 404 da API.
function handler(event) {
  var request = event.request;
  var uri = request.uri;

  if (uri.lastIndexOf('.') > uri.lastIndexOf('/')) {
    return request;
  }

  request.uri = '/index.html';
  return request;
}

// viewer-request do behavior /media/*.
// O Catalog grava apenas a chave do objeto (ex.: "<productId>_foto.png").
// O browser pede /media/<chave>; aqui o prefixo e removido para que o S3
// receba exatamente a chave. Assim o dominio nao conhece a URL do CDN.
function handler(event) {
  var request = event.request;
  request.uri = request.uri.replace(/^\/media/, '');
  return request;
}

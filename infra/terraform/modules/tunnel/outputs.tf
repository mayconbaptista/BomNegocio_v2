output "tunnel_id" { value = cloudflare_zero_trust_tunnel_cloudflared.api.id }
output "origin_hostname" { value = var.origin_hostname }

output "token" {
  value     = data.cloudflare_zero_trust_tunnel_cloudflared_token.api.token
  sensitive = true
}

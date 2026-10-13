// Tudo que conversa com o backend mora aqui.

// Os mesmos nomes que o backend usa.
export type Tipo = 'Foco' | 'PausaCurta' | 'PausaLonga'

export type Sessao = {
  id: string
  tipo: Tipo
  segundosRestantes: number
  pausada: boolean
  finalizada: boolean
}

export type Configuracao = {
  focoMinutos: number
  pausaCurtaMinutos: number
  pausaLongaMinutos: number
  focosAteLonga: number
  iniciarAutomaticamente: boolean
}

async function chamar<T>(metodo: 'GET' | 'POST' | 'PUT', caminho: string, corpo?: unknown): Promise<T> {
  const resposta = await fetch(caminho, {
    method: metodo,
    headers: corpo ? { 'Content-Type': 'application/json' } : {},
    body: corpo ? JSON.stringify(corpo) : undefined,
  })
  if (!resposta.ok) throw new Error(`Erro ${resposta.status}`)
  return resposta.json()
}

export const api = {
  // A duração não vai no pedido: o servidor usa a que está na configuração.
  iniciar: (tipo: Tipo) => chamar<Sessao>('POST', '/sessoes/iniciar', { tipo }),
  pausar: (id: string) => chamar<Sessao>('POST', `/sessoes/${id}/pausar`),
  retomar: (id: string) => chamar<Sessao>('POST', `/sessoes/${id}/retomar`),
  finalizar: (id: string) => chamar<Sessao>('POST', `/sessoes/${id}/finalizar`),
  obterConfiguracao: () => chamar<Configuracao>('GET', '/configuracao'),
  salvarConfiguracao: (config: Configuracao) => chamar<Configuracao>('PUT', '/configuracao', config),
}

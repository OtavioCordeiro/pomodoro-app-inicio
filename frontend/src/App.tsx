import { useEffect, useRef, useState } from 'react'
import { api, type Configuracao, type Sessao, type Tipo } from './api'

const nomes: Record<Tipo, string> = { Foco: 'Foco', PausaCurta: 'Pausa curta', PausaLonga: 'Pausa longa' }

// Qual campo da configuração guarda os minutos de cada tipo.
const campoMinutos: Record<Tipo, 'focoMinutos' | 'pausaCurtaMinutos' | 'pausaLongaMinutos'> = {
  Foco: 'focoMinutos',
  PausaCurta: 'pausaCurtaMinutos',
  PausaLonga: 'pausaLongaMinutos',
}

// Só aparece até a configuração chegar do servidor. Quem manda é o backend.
const padrao: Configuracao = {
  focoMinutos: 25,
  pausaCurtaMinutos: 5,
  pausaLongaMinutos: 15,
  focosAteLonga: 4,
  iniciarAutomaticamente: true,
}

// Som sem arquivo: três bips gerados pelo navegador.
// O navegador só libera áudio depois de um clique, por isso liberamos nos botões.
let audio: AudioContext | undefined

function liberarSom() {
  audio ??= new AudioContext()
  void audio.resume()
}

function bip() {
  if (!audio) return
  for (let i = 0; i < 3; i++) {
    const inicio = audio.currentTime + i * 0.35
    const osc = audio.createOscillator()
    const ganho = audio.createGain()
    osc.frequency.value = 880
    ganho.gain.value = 0.2
    osc.connect(ganho).connect(audio.destination)
    osc.start(inicio)
    osc.stop(inicio + 0.2)
  }
}

// 1500 -> "25:00"
function mmss(segundos: number): string {
  const min = Math.floor(segundos / 60)
  const seg = segundos % 60
  return `${String(min).padStart(2, '0')}:${String(seg).padStart(2, '0')}`
}

export default function App() {
  const [sessao, setSessao] = useState<Sessao | null>(null)
  const [restante, setRestante] = useState(0)
  const [tipo, setTipo] = useState<Tipo>('Foco')
  const [focos, setFocos] = useState(0) // focos concluídos neste ciclo
  const [config, setConfig] = useState(padrao)
  const [rascunho, setRascunho] = useState(config)
  const [erro, setErro] = useState('')
  const modal = useRef<HTMLDialogElement>(null)
  const aoAcabar = useRef<() => void>(() => {})

  // Ao abrir a tela, busca a configuração salva no servidor.
  useEffect(() => {
    api.obterConfiguracao().then(setConfig).catch((e: Error) => setErro(e.message))
  }, [])

  // A cada ação, o servidor responde com a verdade. O front sincroniza com ela.
  function aplicar(nova: Sessao) {
    setSessao(nova)
    setRestante(nova.segundosRestantes)
  }

  // Entre as ações, o relógio anda aqui no navegador. Só roda enquanto não está pausado.
  useEffect(() => {
    if (!sessao || sessao.pausada) return
    const fim = Date.now() + sessao.segundosRestantes * 1000
    const timer = setInterval(() => {
      const falta = Math.max(0, Math.ceil((fim - Date.now()) / 1000))
      setRestante(falta)
      if (falta === 0) {
        clearInterval(timer)
        aoAcabar.current()
      }
    }, 250)
    return () => clearInterval(timer)
  }, [sessao])

  // Cada botão chama a API e mostra o que o servidor respondeu.
  function executar(acao: () => Promise<Sessao>) {
    liberarSom()
    setErro('')
    acao().then(aplicar).catch((e: Error) => setErro(e.message))
  }

  // Finalizar encerra antes do fim: o servidor registra e a tela volta a "Parado".
  function finalizar() {
    if (!sessao) return
    setErro('')
    api.finalizar(sessao.id).then(() => setSessao(null)).catch((e: Error) => setErro(e.message))
  }

  function abrirConfig() {
    setRascunho(config)
    modal.current?.showModal()
  }

  function salvarConfig(e: React.FormEvent) {
    e.preventDefault()
    setErro('')
    api
      .salvarConfiguracao(rascunho)
      .then((salva) => {
        setConfig(salva)
        modal.current?.close()
      })
      .catch((err: Error) => setErro(err.message))
  }

  // Acabou: toca o som e escolhe o próximo tipo. Foco -> pausa curta, até completar
  // o ciclo; aí vem a pausa longa. Toda pausa volta para o foco. Se ligado, já inicia.
  // Fica numa ref para o relógio sempre chamar a versão mais nova (tipo e config atuais).
  useEffect(() => {
    aoAcabar.current = () => {
      bip()
      let proximo: Tipo = 'Foco'
      if (tipo === 'Foco') {
        const feitos = focos + 1
        proximo = feitos >= config.focosAteLonga ? 'PausaLonga' : 'PausaCurta'
        setFocos(proximo === 'PausaLonga' ? 0 : feitos)
      }
      setTipo(proximo)
      setSessao(null)
      if (config.iniciarAutomaticamente) {
        api
          .iniciar(proximo)
          .then((nova) => {
            setSessao(nova)
            setRestante(nova.segundosRestantes)
          })
          .catch((e: Error) => setErro(e.message))
      }
    }
  })

  const estado = !sessao ? 'Parado' : sessao.pausada ? 'Pausado' : 'Rodando'

  return (
    <main className="pagina">
      <header className="cabecalho">
        <h1>Pomodoro</h1>
        <button className="icone" aria-label="Configurações" onClick={abrirConfig}>
          ⚙
        </button>
      </header>

      <p className="tipo">{nomes[tipo]}</p>
      <p className="tempo">{mmss(sessao ? restante : config[campoMinutos[tipo]] * 60)}</p>
      <p className="estado">{estado}</p>

      <div className="botoes">
        <button disabled={!!sessao} onClick={() => executar(() => api.iniciar(tipo))}>
          Iniciar
        </button>
        {sessao?.pausada ? (
          <button onClick={() => executar(() => api.retomar(sessao.id))}>Retomar</button>
        ) : (
          <button disabled={!sessao} onClick={() => executar(() => api.pausar(sessao!.id))}>
            Pausar
          </button>
        )}
      </div>
      <div className="botoes botoes--baixo">
        <button className="secundario" disabled={!sessao} onClick={finalizar}>
          Finalizar
        </button>
      </div>

      {erro && <p className="erro">{erro}</p>}

      <dialog ref={modal} className="modal">
        <form onSubmit={salvarConfig}>
          <h2>Configurações</h2>
          {(Object.keys(nomes) as Tipo[]).map((t) => (
            <label key={t}>
              {nomes[t]} (minutos)
              <input
                type="number"
                min={1}
                max={120}
                required
                value={rascunho[campoMinutos[t]]}
                onChange={(e) => setRascunho({ ...rascunho, [campoMinutos[t]]: Number(e.target.value) })}
              />
            </label>
          ))}
          <label>
            Focos até a pausa longa
            <input
              type="number"
              min={1}
              max={12}
              required
              value={rascunho.focosAteLonga}
              onChange={(e) => setRascunho({ ...rascunho, focosAteLonga: Number(e.target.value) })}
            />
          </label>
          <label>
            Iniciar o próximo automaticamente
            <input
              type="checkbox"
              checked={rascunho.iniciarAutomaticamente}
              onChange={(e) => setRascunho({ ...rascunho, iniciarAutomaticamente: e.target.checked })}
            />
          </label>
          <div className="botoes">
            <button type="button" className="secundario" onClick={() => modal.current?.close()}>
              Cancelar
            </button>
            <button type="submit">Salvar</button>
          </div>
        </form>
      </dialog>
    </main>
  )
}

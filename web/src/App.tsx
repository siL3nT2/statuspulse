import { useLatest } from './useLatest'

function formatUptime(value: number | null): string {
  return value === null ? 'no data' : `${value}%`
}

function App() {
  const state = useLatest()

  return (
    <main className="mx-auto max-w-3xl px-5 py-10">
      <h1 className="text-3xl font-semibold">StatusPulse</h1>

      {state.kind === 'loading' && <p className="mt-6">Loading…</p>}

      {state.kind === 'error' && (
        <p className="mt-6" role="alert">
          Could not load data: {state.message}
        </p>
      )}

      {state.kind === 'ready' && (
        <ul className="mt-6 space-y-3">
          {state.report.sites.map((site) => (
            <li key={site.id} className="rounded-lg border p-4">
              <p className="font-medium">{site.name}</p>
              <p>Status: {site.state}</p>
              <p>Uptime (24h): {formatUptime(site.uptime.last24Hours)}</p>
            </li>
          ))}
        </ul>
      )}
    </main>
  )
}

export default App
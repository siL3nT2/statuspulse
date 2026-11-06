import { STALE_AFTER_MINUTES } from './config'
import { isStale, timeAgo } from './format'
import { SiteCard } from './SiteCard'
import type { SiteStatus } from './types'
import { useLatest } from './useLatest'

function overallMessage(sites: SiteStatus[]): string {
  const down = sites.filter((site) => site.state === 'down').length
  const degraded = sites.filter((site) => site.state === 'degraded').length

  if (down > 0) return `${down} of ${sites.length} sites down`
  if (degraded > 0) return `${degraded} of ${sites.length} sites degraded`
  return 'All sites operational'
}

function App() {
  const state = useLatest()

  return (
    <div className="min-h-screen bg-slate-50 text-slate-900 dark:bg-slate-900 dark:text-slate-100">
      <main className="mx-auto max-w-3xl px-5 py-10">
        <h1 className="text-3xl font-semibold">StatusPulse</h1>

        {state.kind === 'loading' && <p className="mt-6">Loading…</p>}

        {state.kind === 'error' && (
          <p className="mt-6" role="alert">
            Could not load data: {state.message}
          </p>
        )}

        {state.kind === 'ready' && (
          <>
            <p className="mt-2 text-lg">{overallMessage(state.report.sites)}</p>

            <p className="mt-1 text-sm text-slate-600 dark:text-slate-300">
              Last updated{' '}
              <time
                dateTime={state.report.generatedAt}
                title={new Date(state.report.generatedAt).toLocaleString()}
              >
                {timeAgo(state.report.generatedAt)}
              </time>
            </p>

            {isStale(state.report.generatedAt, STALE_AFTER_MINUTES) && (
              <p
                role="status"
                className="mt-4 rounded-lg border border-amber-300 bg-amber-50 p-3 text-sm text-amber-900 dark:border-amber-700 dark:bg-amber-950 dark:text-amber-100"
              >
                ⚠ This data is more than {STALE_AFTER_MINUTES} minutes old. The
                scheduled checks may be delayed.
              </p>
            )}

            <ul className="mt-6 space-y-4">
              {state.report.sites.map((site) => (
                <SiteCard key={site.id} site={site} />
              ))}
            </ul>
          </>
        )}
      </main>
    </div>
  )
}

export default App
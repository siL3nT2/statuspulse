import { formatDuration, timeAgo } from './format'
import type { Incident, SiteStatus } from './types'

export function IncidentList({
  incidents,
  sites,
}: {
  incidents: Incident[]
  sites: SiteStatus[]
}) {
  const names = new Map(sites.map((site) => [site.id, site.name]))
  const shown = incidents.slice(0, 10)

  return (
    <section className="mt-10" aria-labelledby="incidents-heading">
      <h2 id="incidents-heading" className="text-xl font-semibold">
        Incidents
      </h2>

      {shown.length === 0 ? (
        <p className="mt-3 text-sm text-slate-600 dark:text-slate-300">
          No incidents in the last 30 days.
        </p>
      ) : (
        <ul className="mt-4 space-y-3">
          {shown.map((incident) => (
            <li
              key={`${incident.siteId}-${incident.start}`}
              className="rounded-xl border border-slate-200 bg-white p-4 shadow-sm dark:border-slate-700 dark:bg-slate-800"
            >
              <div className="flex flex-wrap items-center justify-between gap-2">
                <p className="font-medium">
                  {names.get(incident.siteId) ?? incident.siteId}
                </p>
                <p className="text-sm font-medium">
                  {incident.end === null ? '● Ongoing' : '✓ Resolved'}
                </p>
              </div>

              <p className="mt-1 text-sm text-slate-600 dark:text-slate-300">
                Started{' '}
                <time
                  dateTime={incident.start}
                  title={new Date(incident.start).toLocaleString()}
                >
                  {timeAgo(incident.start)}
                </time>
                {' · '}Duration: {formatDuration(incident.durationMinutes)}
              </p>
            </li>
          ))}
        </ul>
      )}
    </section>
  )
}
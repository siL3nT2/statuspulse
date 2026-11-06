import { formatMs, formatUptime, timeAgo } from './format'
import { StatusBadge } from './StatusBadge'
import type { SiteStatus } from './types'

function Stat({ label, value }: { label: string; value: string }) {
  return (
    <div>
      <dt className="text-slate-600 dark:text-slate-300">{label}</dt>
      <dd className="mt-1 text-xl font-semibold">{value}</dd>
    </div>
  )
}

export function SiteCard({ site }: { site: SiteStatus }) {
  return (
    <li className="rounded-xl border border-slate-200 bg-white p-5 shadow-sm dark:border-slate-700 dark:bg-slate-800">
      <div className="flex items-start justify-between gap-4">
        <div className="min-w-0">
          <h2 className="truncate text-lg font-semibold">{site.name}</h2>
          <p className="truncate text-sm text-slate-600 dark:text-slate-300">
            {site.url}
          </p>
        </div>
        <StatusBadge state={site.state} />
      </div>

      <dl className="mt-4 grid grid-cols-3 gap-4 text-sm">
        <Stat label="Uptime 24h" value={formatUptime(site.uptime.last24Hours)} />
        <Stat label="Uptime 7d" value={formatUptime(site.uptime.last7Days)} />
        <Stat label="Uptime 30d" value={formatUptime(site.uptime.last30Days)} />
      </dl>

      <p className="mt-4 text-sm text-slate-600 dark:text-slate-300">
        Response time: {formatMs(site.check.ms)} · Checked{' '}
        <time
          dateTime={site.check.time}
          title={new Date(site.check.time).toLocaleString()}
        >
          {timeAgo(site.check.time)}
        </time>
      </p>

      {site.check.error && (
        <p className="mt-2 text-sm text-slate-600 dark:text-slate-300">
          Last error: {site.check.error}
        </p>
      )}
    </li>
  )
}
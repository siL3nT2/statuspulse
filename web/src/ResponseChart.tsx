import { buildChart } from './chart'
import type { CheckResult } from './types'

const WIDTH = 600
const HEIGHT = 100

export function ResponseChart({ checks }: { checks: CheckResult[] }) {
  const { points, maxMs, slowestMs } = buildChart(checks, 24, WIDTH, HEIGHT)

  if (points.length === 0) {
    return (
      <p className="mt-4 text-sm text-slate-600 dark:text-slate-300">
        No checks in the last 24 hours.
      </p>
    )
  }

  const failed = points.filter((point) => !point.ok)
  const line = points
    .filter((point) => point.y !== null)
    .map((point) => `${point.x},${point.y}`)
    .join(' ')

  const summary = `${points.length} checks in the last 24 hours, ${failed.length} failed` +
    (slowestMs === null ? '' : `, slowest ${slowestMs} ms`)

  return (
    <figure className="mt-4">
      <svg
        viewBox={`0 0 ${WIDTH} ${HEIGHT + 20}`}
        role="img"
        aria-label={`Response times. ${summary}.`}
        className="w-full"
      >
        <line
          x1="0"
          y1={HEIGHT}
          x2={WIDTH}
          y2={HEIGHT}
          className="stroke-slate-300 dark:stroke-slate-600"
        />

        <polyline
          points={line}
          fill="none"
          strokeWidth="2"
          className="stroke-sky-600 dark:stroke-sky-400"
        />

        {points
          .filter((point) => point.y !== null)
          .map((point) => (
            <circle
              key={point.time}
              cx={point.x}
              cy={point.y as number}
              r="3"
              className="fill-sky-600 dark:fill-sky-400"
            />
          ))}

        {failed.map((point) => (
          <path
            key={point.time}
            d={`M ${point.x - 4} ${HEIGHT - 8} L ${point.x + 4} ${HEIGHT} M ${point.x + 4} ${HEIGHT - 8} L ${point.x - 4} ${HEIGHT}`}
            strokeWidth="2"
            className="stroke-red-600 dark:stroke-red-400"
          />
        ))}

        <text x="0" y="12" className="fill-slate-600 text-xs dark:fill-slate-300">
          {maxMs} ms
        </text>
        <text x="0" y={HEIGHT + 16} className="fill-slate-600 text-xs dark:fill-slate-300">
          24 h ago
        </text>
        <text
          x={WIDTH}
          y={HEIGHT + 16}
          textAnchor="end"
          className="fill-slate-600 text-xs dark:fill-slate-300"
        >
          now
        </text>
      </svg>

      <figcaption className="mt-1 text-sm text-slate-600 dark:text-slate-300">
        {summary}. ✕ marks a failed check.
      </figcaption>
    </figure>
  )
}
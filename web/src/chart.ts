import type { CheckResult } from './types'

export interface ChartPoint {
  x: number
  y: number | null
  ok: boolean
  ms: number | null
  time: string
}

export interface ChartData {
  points: ChartPoint[]
  maxMs: number
  slowestMs: number | null
}

export function buildChart(
  checks: CheckResult[],
  windowHours: number,
  width: number,
  height: number,
  now: Date = new Date(),
): ChartData {
  const windowMs = windowHours * 60 * 60 * 1000
  const start = now.getTime() - windowMs

  const recent = checks.filter(
    (check) => new Date(check.time).getTime() >= start,
  )

  const okTimes = recent
    .filter((check) => check.ok && check.ms !== null)
    .map((check) => check.ms as number)

  const slowestMs = okTimes.length > 0 ? Math.max(...okTimes) : null
  const maxMs = Math.max(1000, slowestMs ?? 0)

  const points = recent.map((check) => {
    const rawX = ((new Date(check.time).getTime() - start) / windowMs) * width
    const x = Math.min(width, Math.max(0, rawX))
    const y =
      check.ok && check.ms !== null ? height - (check.ms / maxMs) * height : null

    return { x, y, ok: check.ok, ms: check.ms, time: check.time }
  })

  return { points, maxMs, slowestMs }
}
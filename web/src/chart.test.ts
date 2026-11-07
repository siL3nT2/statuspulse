import { describe, expect, it } from 'vitest'
import { buildChart } from './chart'
import type { CheckResult } from './types'

const NOW = new Date('2026-11-07T12:00:00Z')

function check(time: string, ms: number | null): CheckResult {
  return {
    time,
    ok: ms !== null,
    status: ms === null ? null : 200,
    ms,
    error: ms === null ? 'timeout' : null,
  }
}

describe('buildChart', () => {
  it('drops checks older than the window', () => {
    const data = buildChart(
      [check('2026-11-05T12:00:00Z', 300), check('2026-11-07T11:00:00Z', 300)],
      24, 600, 100, NOW,
    )

    expect(data.points).toHaveLength(1)
  })

  it('places points by time', () => {
    const data = buildChart(
      [check('2026-11-07T00:00:00Z', 300), check('2026-11-07T12:00:00Z', 300)],
      24, 600, 100, NOW,
    )

    expect(data.points[0].x).toBe(300)
    expect(data.points[1].x).toBe(600)
  })

  it('uses at least a 1000 ms scale', () => {
    const data = buildChart([check('2026-11-07T11:00:00Z', 500)], 24, 600, 100, NOW)

    expect(data.maxMs).toBe(1000)
    expect(data.points[0].y).toBe(50)
  })

  it('grows the scale for slow checks', () => {
    const data = buildChart([check('2026-11-07T11:00:00Z', 2500)], 24, 600, 100, NOW)

    expect(data.maxMs).toBe(2500)
    expect(data.points[0].y).toBe(0)
  })

  it('has no y value for failed checks', () => {
    const data = buildChart([check('2026-11-07T11:00:00Z', null)], 24, 600, 100, NOW)

    expect(data.points[0].y).toBeNull()
    expect(data.points[0].ok).toBe(false)
  })

  it('keeps future checks inside the chart', () => {
    const data = buildChart([check('2026-11-07T12:05:00Z', 300)], 24, 600, 100, NOW)

    expect(data.points[0].x).toBe(600)
  })

  it('reports the slowest successful check', () => {
    const data = buildChart(
      [
        check('2026-11-07T10:00:00Z', 300),
        check('2026-11-07T11:00:00Z', 700),
        check('2026-11-07T11:30:00Z', null),
      ],
      24, 600, 100, NOW,
    )

    expect(data.slowestMs).toBe(700)
  })

  it('reports no slowest value when every check failed', () => {
    const data = buildChart([check('2026-11-07T11:00:00Z', null)], 24, 600, 100, NOW)

    expect(data.slowestMs).toBeNull()
  })
})